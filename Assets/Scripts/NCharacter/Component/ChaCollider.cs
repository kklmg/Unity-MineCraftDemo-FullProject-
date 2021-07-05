using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {
        private IWorld m_World;
        private Character m_Character;
        private Communicator m_Communicator;

        private float m_BodyWidth;
        private float m_BodyHeight;
        private float m_BodyWidth_Half;
        private float m_BodyHeight_Half;

        private Vector3 m_Velocity = Vector3.zero;


        //Unity Function
        //---------------------------------------------------------------------
        private void Awake()
        {
            m_Character = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();

            //get body size
            m_BodyWidth = m_Character.BodyWidth;
            m_BodyHeight = m_Character.BodyHeight;
            m_BodyWidth_Half = m_BodyWidth / 2;
            m_BodyHeight_Half = m_BodyHeight / 2;
        }

        private void OnEnable()
        {
            //avoid intersection x z
            m_Communicator.SubsribeEvent(E_Cha_TranslateRequest_XZ.ID, Handle_RequestXZ, enPriority.level_2);

            //avoid intersection y
            m_Communicator.SubsribeEvent(E_Cha_TranslateRequest_Y.ID, Handle_RequestY, enPriority.level_5);
        }

        private void OnDisable()
        {
            //avoid intersection x z
            m_Communicator.UnSubscribe(E_Cha_TranslateRequest_XZ.ID, Handle_RequestXZ);

            //avoid intersection y
            m_Communicator.UnSubscribe(E_Cha_TranslateRequest_Y.ID, Handle_RequestY);
        }


        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
        }

        private void Update()
        {
            if (m_Velocity != Vector3.zero)
            {
                AvoidIntersection();
                m_Communicator.Publish(new E_Cha_TranslateOrder(m_Velocity));

                m_Velocity = Vector3.zero;
            }
        }


        //Event Handlers
        //---------------------------------------------------------------------

        private void Handle_RequestXZ(IEvent _event)
        {
            E_Cha_TranslateRequest_XZ req = _event.Cast<E_Cha_TranslateRequest_XZ>();

            m_Velocity += req.Translation;

            _event.HasHandled = true;
        }


        //Jump request Handler
        private void Handle_RequestY(IEvent _event)
        {
            E_Cha_TranslateRequest_Y Y = _event.Cast<E_Cha_TranslateRequest_Y>();

            m_Velocity += new Vector3(0, Y.Velocity, 0);

            _event.HasHandled = true;
        }


        private void AvoidIntersection()
        {
            m_Velocity.y = Mathf.Clamp(m_Velocity.y, -10, 10);


            //Z Axis
            if (m_Velocity.z > 0)
            {
                m_Velocity.z = AvoidInerSection_Front(m_Velocity.z, transform.position);
            }
            else if (m_Velocity.z < 0)
            {
                m_Velocity.z = AvoidInerSection_Back(m_Velocity.z, transform.position);
            }
            //X Axis 
            if (m_Velocity.x > 0)
            {
                m_Velocity.x = AvoidInerSection_Right(m_Velocity.x, transform.position);
            }
            else if (m_Velocity.x < 0)
            {
                m_Velocity.x = AvoidInerSection_Left(m_Velocity.x, transform.position);
            }

            //Y Axis
            if (m_Velocity.y > 0)
            {
                m_Velocity.y = AvoidInerSection_Up(m_Velocity.y, transform.position);
            }

            else if (m_Velocity.y < 0)
            {
                m_Velocity.y = AvoidInerSection_Down(m_Velocity.y, transform.position);
            }
        }

        public float AvoidInerSection_Front(float velocity, Vector3 CurPosition)
        {
            BlockLocation FrontLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < velocity; ++j)
            {
                for (int i = 0; i < m_BodyHeight; ++i)
                {
                    FrontLoc = new BlockLocation(CurPosition + new Vector3(0, i, m_BodyWidth_Half + unitV + j), m_World);

                    if (FrontLoc.HasObstacle())
                    {
                        m_Communicator.Publish(new E_Cha_Collision());
                        return
                            FrontLoc.Bound.min.z - CurPosition.z - m_BodyWidth_Half;
                    }
                }
            }
            return velocity;

        }
        public float AvoidInerSection_Back(float velocity, Vector3 CurPosition)
        {
            BlockLocation BackLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < -velocity; ++j)
            {
                for (int i = 0; i < m_BodyHeight; ++i)
                {
                    BackLoc = new BlockLocation(CurPosition + new Vector3(0, i, -m_BodyWidth_Half + unitV - j), m_World);

                    if (BackLoc.HasObstacle())
                    {
                        m_Communicator.Publish(new E_Cha_Collision());
                        return
                            BackLoc.Bound.max.z - CurPosition.z + m_BodyWidth_Half;
                    }
                }
            }
            return velocity;

        }
        public float AvoidInerSection_Right(float velocity, Vector3 CurPosition)
        {
            BlockLocation RightLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < velocity; ++j)
            {
                for (int i = 0; i < m_BodyHeight; ++i)
                {
                    RightLoc = new BlockLocation(CurPosition + new Vector3(m_BodyWidth_Half + unitV + j, i, 0), m_World);

                    if (RightLoc.HasObstacle())
                    {
                        m_Communicator.Publish(new E_Cha_Collision());
                        return
                            RightLoc.Bound.min.x - CurPosition.x - m_BodyWidth_Half;
                    }
                }
            }
            return velocity;

        }
        public float AvoidInerSection_Left(float velocity, Vector3 CurPosition)
        {
            BlockLocation LeftLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < -velocity; ++j)
            {
                for (int i = 0; i < m_BodyHeight; ++i)
                {
                    LeftLoc = new BlockLocation(CurPosition + new Vector3(-m_BodyWidth_Half + unitV - j, i, 0), m_World);

                    if (LeftLoc.HasObstacle())
                    {
                        m_Communicator.Publish(new E_Cha_Collision());
                        return
                            LeftLoc.Bound.max.x - CurPosition.x + m_BodyWidth_Half;
                    }
                }
            }
            return velocity;

        }
        public float AvoidInerSection_Up(float velocity, Vector3 CurPosition)
        {
            BlockLocation UpLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < velocity; ++j)
            {
                UpLoc = new BlockLocation(CurPosition + new Vector3(0, m_BodyHeight+ unitV + j, 0), m_World);

                if (UpLoc.HasObstacle())
                {
                    m_Communicator.Publish(new E_Cha_Collision());
                    m_Communicator.Publish(new E_Cha_TouchUpsideBlock());
                    
                    return
                        UpLoc.Bound.min.y - CurPosition.y - m_BodyHeight;

                }
            }
            return velocity;

        }
        public float AvoidInerSection_Down(float velocity, Vector3 CurPosition)
        {
            BlockLocation DownLoc;
            float unitV = velocity - (int)velocity;

            for (float j = 0; j < -velocity; ++j)
            {
                DownLoc = new BlockLocation(CurPosition + new Vector3(0, unitV - j, 0), m_World);
                if (DownLoc.HasObstacle())
                {
                    m_Communicator.Publish(new E_Cha_Collision());
                    m_Communicator.Publish(new E_Cha_TouchGround());
                    return
                        DownLoc.Bound.max.y - CurPosition.y;
                }
            }
            return velocity;
        }
    }

}







//using UnityEngine;

//using Assets.Scripts.NGlobal.ServiceLocator;
//using Assets.Scripts.NWorld;
//using Assets.Scripts.NEvent;
//using Assets.Scripts.NData;

//namespace Assets.Scripts.NCharacter
//{
//    [RequireComponent(typeof(Character))]
//    [RequireComponent(typeof(Communicator))]
//    class ChaCollider : MonoBehaviour
//    {
//        private IWorld m_World;
//        private Character m_Character;
//        private Communicator m_Communicator;

//        private float m_BodyWidth;
//        private float m_BodyHeight;
//        private float m_BodyWidth_Half;
//        private float m_BodyHeight_Half;

//        private Vector3 m_Translation;





//        //Unity Function
//        //---------------------------------------------------------------------
//        private void Awake()
//        {
//            m_Character = GetComponent<Character>();
//            m_Communicator = GetComponent<Communicator>();

//            //get body size
//            m_BodyWidth = m_Character.BodyWidth;
//            m_BodyHeight = m_Character.BodyHeight;
//            m_BodyWidth_Half = m_BodyWidth / 2;
//            m_BodyHeight_Half = m_BodyHeight / 2;
//        }

//        private void OnEnable()
//        {
//            //avoid intersection x z
//            m_Communicator.SubsribeEvent(E_Cha_MoveRequest_XZ.ID, AvoidIntersection_XZ, enPriority.level_2);

//            //avoid intersection y
//            m_Communicator.SubsribeEvent(E_Cha_Velocity_Y.ID, AvoidInterSection_Y, enPriority.level_5);
//        }

//        private void OnDisable()
//        {
//            //avoid intersection x z
//            m_Communicator.UnSubscribe(E_Cha_MoveRequest_XZ.ID, AvoidIntersection_XZ);

//            //avoid intersection y
//            m_Communicator.UnSubscribe(E_Cha_Velocity_Y.ID, AvoidInterSection_Y);
//        }


//        private void Start()
//        {
//            m_World = Locator<IWorld>.GetService();
//        }


//        //intersection Handler
//        //---------------------------------------------------------------------

//        private void AvoidIntersection_XZ(IEvent _event)
//        {
//            E_Cha_MoveRequest_XZ req = (_event as E_Cha_MoveRequest_XZ);

//            float limited_x = req.Translation.x > 1 ? 1 : req.Translation.x;
//            limited_x = req.Translation.x < -1 ? -1 : req.Translation.x;
//            float limited_z = req.Translation.z > 1 ? 1 : req.Translation.z;
//            limited_z = req.Translation.z < -1 ? -1 : req.Translation.z;

//            req.Translation = new Vector3(limited_x, 0, limited_z);


//            Vector3 Intersection;
//            if (req.Translation.z > 0 && Front(req.Translation.z, out Intersection))
//            {
//                req.Translation -= Intersection;
//            }

//            else if (req.Translation.z < 0 && Back(req.Translation.z, out Intersection))
//            {
//                req.Translation += Intersection;
//            }

//            if (req.Translation.x > 0 && Right(req.Translation.x, out Intersection))
//            {
//                req.Translation -= Intersection;
//            }
//            else if (req.Translation.x < 0 && Left(req.Translation.x, out Intersection))
//            {
//                req.Translation += Intersection;
//            }
//        }

//        //Jump request Handler
//        private void AvoidInterSection_Y(IEvent _event)
//        {
//            E_Cha_Velocity_Y Y = _event.Cast<E_Cha_Velocity_Y>();

//            //limit velocity for aabb
//            Y.Velocity = Y.Velocity > 1 ? 1 : Y.Velocity;
//            Y.Velocity = Y.Velocity < -1 ? -1 : Y.Velocity;

//            if (Y.Velocity > 0 && Up(Y.Velocity, out Vector3 intersection))
//            {
//                Y.Velocity -= intersection.y;

//                m_Communicator.Publish(new E_Cha_TouchUpsideBlock());
//            }

//            else if (Y.Velocity < 0 && Down(Y.Velocity, out intersection))
//            {
//                Y.Velocity += intersection.y;
//                m_Communicator.Publish(new E_Cha_TouchGround());
//            }
//        }

//        //Check if there is a solid block in front of me
//        private bool Front(float trans_z, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;

//            for (int i = 0; i < m_BodyHeight; ++i)
//            {
//                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, m_BodyWidth_Half + trans_z), m_World);

//                if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//                {
//                    intersect.z = transform.position.z + trans_z + m_BodyWidth_Half - Loc.Bound.min.z;
//                    //Debug.Log("front intersection"+ intersect);
//                    return true;
//                }
//            }

//            return false;
//        }
//        private bool Back(float trans_z, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;

//            for (int i = 0; i < m_BodyHeight; ++i)
//            {
//                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, -m_BodyWidth_Half + trans_z), m_World);

//                if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//                {
//                    intersect.z = Loc.Bound.max.z - (transform.position.z + trans_z - m_BodyWidth_Half);
//                    //Debug.Log("back intersection" + intersect);
//                    return true;
//                }
//            }
//            return false;
//        }
//        private bool Left(float trans_x, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;
//            for (int i = 0; i < m_BodyHeight; ++i)
//            {
//                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(-m_BodyWidth_Half + trans_x, i, 0), m_World);

//                if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//                {
//                    intersect.x = Loc.Bound.max.x - (transform.position.x + trans_x - m_BodyWidth_Half);
//                    //Debug.Log("left intersection" + intersect);
//                    return true;
//                }
//            }
//            return false;
//        }
//        private bool Right(float trans_x, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;
//            for (int i = 0; i < m_BodyHeight; ++i)
//            {
//                BlockLocation Loc = new BlockLocation
//                    (transform.position + new Vector3(m_BodyWidth_Half + trans_x, i, 0), m_World);

//                if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//                {
//                    intersect.x = (transform.position.x + trans_x + m_BodyWidth_Half) - Loc.Bound.min.x;
//                    //Debug.Log("right intersection" + intersect);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool Down(float trans_y, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;
//            BlockLocation Loc =
//                new BlockLocation(transform.position + new Vector3(0, trans_y, 0), m_World);

//            if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//            {
//                intersect.y = Loc.Bound.max.y - (transform.position.y + trans_y);
//                //Debug.Log("Down intersection" + intersect);
//                return true;
//            }
//            return false;
//        }
//        private bool Up(float trans_y, out Vector3 intersect)
//        {
//            intersect = Vector3.zero;
//            BlockLocation Loc =
//                new BlockLocation(transform.position + new Vector3(0, trans_y + m_BodyHeight, 0), m_World);

//            if (Loc.IsBlockExists() && Loc.CurBlock.IsObstacle)
//            {
//                intersect.y = (transform.position.y + trans_y + m_BodyHeight) - Loc.Bound.min.y;

//                return true;
//            }
//            return false;
//        }
//    }

//}




