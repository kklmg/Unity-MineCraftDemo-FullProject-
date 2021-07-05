using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;


namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaJumper : MonoBehaviour
    {
        [SerializeField]
        private float m_Velocity;

        private Communicator m_Communicator;
        private Character m_Character;

        [SerializeField]
        private bool m_IsJumping = false;

        public IWorld m_World;

        private void Awake()
        {
            m_Character = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();
        }

        private void OnEnable()
        {
            //Handle jump start
            m_Communicator.SubsribeEvent(E_Cha_JumpUp.ID, HandleJump, enPriority.level_2);

            //Handle Touch Ground
            m_Communicator.SubsribeEvent(E_Cha_TouchGround.ID, HandleTouchGround, enPriority.level_2);

            //Handle Touch Upside
            m_Communicator.SubsribeEvent(E_Cha_TouchUpsideBlock.ID, HandleTouchUpside, enPriority.level_2);
        }

        private void OnDisable()
        {
            //Handle jump start
            m_Communicator.UnSubscribe(E_Cha_JumpUp.ID, HandleJump);

            //Handle Touch Ground
            m_Communicator.UnSubscribe(E_Cha_TouchGround.ID, HandleTouchGround);

            //Handle Touch Upside
            m_Communicator.UnSubscribe(E_Cha_TouchUpsideBlock.ID, HandleTouchUpside);

            m_IsJumping = false;
            m_Velocity = 0.0f;
        }

        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
        }

        private void Update()
        {
            if (m_IsJumping)
            {
                m_Communicator.Publish(new E_Cha_TranslateRequest_Y(m_Velocity));
            }
        }

        private void HandleJump(IEvent _event)
        {           
            E_Cha_JumpUp EJump = _event.Cast<E_Cha_JumpUp>();

            m_IsJumping = true;
            m_Velocity = EJump.Force;

            m_Communicator.Publish(new E_Cha_LeaveGround());
        }

        private void HandleTouchGround(IEvent _event)
        {
            m_IsJumping = false;
            m_Velocity = 0;
        }

        private void HandleTouchUpside(IEvent _event)
        {
            if (m_Velocity > 0)
            {
                m_Velocity = 0;
            }
        }
    }
}