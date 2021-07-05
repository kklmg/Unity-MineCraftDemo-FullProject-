using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaGravity : MonoBehaviour
    {
        public float m_Gravity = 9.8f;
        public bool m_IsOnGround = false;

        [SerializeField]
        public float m_VDrop = 0.0f;
        
        private float m_BodyHeight_Half;

        private Communicator m_Communicator;
        private Character m_Character;

        public IWorld m_World;

        private void Awake()
        {
            m_Character = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();
            m_BodyHeight_Half = m_Character.BodyHeight / 2;
        }

        private void OnEnable()
        {
            //Handle Leave Ground
            m_Communicator.SubsribeEvent(E_Cha_LeaveGround.ID, HandleleaveGround, enPriority.level_2);

            //Handle Touch Ground
            m_Communicator.SubsribeEvent(E_Cha_TouchGround.ID, HandleTouchGround, enPriority.level_2);

            //Handle character moving
            m_Communicator.SubsribeEvent(E_Cha_HasMoved.ID, Handle_CheckGround, enPriority.Highest);

            //Handle character moving
            m_Communicator.SubsribeEvent(E_Cha_Fly.ID, Handle_Fly, enPriority.Highest);


            //Global Event
            Locator<IEventHelper>.GetService().Subscribe(E_Block_Recover.ID, Handle_CheckGround, enPriority.level_5);
            Locator<IEventHelper>.GetService().Subscribe(E_Block_Modify.ID, Handle_CheckGround, enPriority.level_5);
        }

        private void OnDisable()
        {
            //Handle leave ground
            m_Communicator.UnSubscribe(E_Cha_LeaveGround.ID, HandleleaveGround);
            
            //Handle Touch Ground
            m_Communicator.UnSubscribe(E_Cha_TouchGround.ID, HandleTouchGround);

            //Handle Character move
            m_Communicator.UnSubscribe(E_Cha_HasMoved.ID, Handle_CheckGround);

            //Handle character moving
            m_Communicator.UnSubscribe(E_Cha_Fly.ID, Handle_Fly);

            //Global Event
            Locator<IEventHelper>.GetService().UnSubscribe(E_Block_Recover.ID, Handle_CheckGround);
            Locator<IEventHelper>.GetService().UnSubscribe(E_Block_Modify.ID, Handle_CheckGround);

            CheckIsOnGround();
        }

        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
        }

        private void Update()
        {
            switch (m_Character.MovingType)
            {
                case enMovingType.Walking:
                    {
                        if (m_IsOnGround == false)
                        {
                            m_VDrop += m_Gravity * Time.deltaTime;
                            m_Communicator.Publish(new E_Cha_TranslateRequest_Y(-m_VDrop));
                        }
                    }
                    break;
                case enMovingType.Sliding:
                    {
                        if (m_IsOnGround == false)
                        {
                            m_VDrop = m_Gravity * Time.deltaTime * 0.5f;
                            m_Communicator.Publish(new E_Cha_TranslateRequest_Y(-m_VDrop));
                        }
                    }
                    break;
                case enMovingType.suspending:
                    {
                        


                    }
                    break;
                default:
                    break;
            }


        }

        private void HandleleaveGround(IEvent _event)
        {
            m_IsOnGround = false;
        }

        private void HandleTouchGround(IEvent _event)
        {
            m_VDrop = 0;
            m_IsOnGround = true;
        }

        private void CheckIsOnGround()
        {
            Vector3 intersection = Vector3.zero;

            BlockLocation Loc =
                new BlockLocation(m_Character.TF_Bottom.position + new Vector3(0, -1, 0), m_World);

            //there is a obstacle block downside
            if (!Loc.IsBlockExists() || !Loc.CurBlock.IsObstacle)
            {
                m_IsOnGround = false;
            }
        }

        //Check Ground
        private void Handle_CheckGround(IEvent _event)
        {
            CheckIsOnGround();
        }

        //Handle fly
        private void Handle_Fly(IEvent _event)
        {
            var fly = _event.Cast<E_Cha_Fly>();

            if (fly.ForceUp > 0) { m_VDrop = 0.0f; }
        }
    }
}
