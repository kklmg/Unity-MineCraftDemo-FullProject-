using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaFly : MonoBehaviour
    {
        private Communicator m_Communicator;
        private Character m_Character;

        private float m_ForceFront = 0.0f;
        private float m_Resistance = 0.1f;
        //private int 

        public IWorld m_World;

        private void Awake()
        {
            m_Character = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();
        }

        private void OnEnable()
        {
            //Handle fly
            m_Communicator.SubsribeEvent(E_Cha_Fly.ID, HandleFly, enPriority.level_2);
            m_Communicator.SubsribeEvent(E_Cha_Collision.ID, HandleCollidion, enPriority.Highest);
        }

        private void OnDisable()
        {
            m_ForceFront = 0.0f;
            //Handle fly
            m_Communicator.UnSubscribe(E_Cha_Fly.ID, HandleFly);
            m_Communicator.UnSubscribe(E_Cha_Collision.ID, HandleCollidion);
        }

        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
        }

        private void Update()
        {
            if (m_ForceFront > 0)
            {
                m_Communicator.Publish
                    (new E_Cha_TranslateRequest_XZ
                    (transform.rotation * new Vector3(0, 0, m_ForceFront)*Time.deltaTime));

                m_ForceFront -=  0.1f * Time.deltaTime;
            }
        }

        private void HandleFly(IEvent _event)
        {
            E_Cha_Fly EFly = _event.Cast<E_Cha_Fly>();

            m_ForceFront += EFly.ForceFront;
            Mathf.Clamp(m_ForceFront, 0, 0.5f);

            m_Communicator.Publish(new E_Cha_TranslateRequest_Y(EFly.ForceUp * 20));
        }

        private void HandleCollidion(IEvent _event)
        {
            m_ForceFront = 0.0f;
        }
    }
}
