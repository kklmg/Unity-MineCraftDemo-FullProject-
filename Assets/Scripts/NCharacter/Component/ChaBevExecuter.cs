using UnityEngine;

using Assets.Scripts.NEvent;
using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NCharacter;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaBevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private Communicator m_Communicator; 

        [SerializeField]
        private ChaBevData m_BevData;

        public ChaBevData BevData { get { return m_BevData; } }

        private void Awake()
        {
            m_Root = ChaBevFactory.Instance.ChaAct_Base();
            m_Communicator = GetComponent<Communicator>();
            m_BevData = new ChaBevData(GetComponent<Character>(), m_Communicator);            
        }

        private void Start()
        {
            m_Communicator.SubsribeEvent(E_Cha_TouchGround.ID,Handle_TouchGround,0);
            m_Communicator.SubsribeEvent(E_Cha_TouchUpsideBlock.ID,Handle_TouchUpSide, 0);
        }


        private void Update()
        {
            m_Root.Evaluate(m_BevData);
        }

        public void Handle_TouchGround(IEvent _event)
        {
            m_BevData.isInAir = false;
        }

        public void Handle_TouchUpSide(IEvent _event)
        {

        }


    }
}
