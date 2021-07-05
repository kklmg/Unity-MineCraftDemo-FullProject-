using UnityEngine;

namespace Assets.Scripts.NCharacter
{
    public class Character : MonoBehaviour
    {
        //Field
        //-----------------------------------------------------------------------
        public Transform TF_Eye;
        public Transform TF_Bottom;
        public Transform TF_Top;

        [SerializeField]
        [Range(0, 5)]
        private byte m_ViewDistance = 1;

        [SerializeField]
        private float m_WalkSpeed = 1;
        [SerializeField]
        private float m_RunSpeed = 2;
        [SerializeField]
        private float m_JumpForce = 0.5f;

        [Range(0.1f, 0.9f)]
        public float m_BodyWidth = 0.8f;
        [Range(1, 3)]
        public float m_BodyHeight = 1.8f;

        [SerializeField]
        private enMovingType m_MovingType;

        public enMovingType MovingType { get { return m_MovingType; } }
        private Bounds m_BodyBound;


        //Properties
        //-----------------------------------------------------------------------
        public float BodyWidth { get { return m_BodyWidth; } }
        public float BodyHeight { get { return m_BodyHeight; } }
        public float WalkSpeed { get { return m_WalkSpeed; } set { m_WalkSpeed = value; } }
        public float RunSpeed { get { return m_RunSpeed; } set { m_RunSpeed = value; } }
        public float JumpForce { get { return m_JumpForce; } set { m_JumpForce = value; } }
        public byte ViewDistance { get { return m_ViewDistance; } }
        public Vector3 Movement { get; set; }

        public Bounds LocalBodyBound { get { return m_BodyBound; } }
        public Bounds GlobalBodyBound
        {
            get
            {
                Bounds tempBound = m_BodyBound;
                tempBound.center += transform.position;
                return tempBound;
            }
        }

        public void SetMovingType(enMovingType movingType)
        {
            m_MovingType = movingType;

            switch (movingType)
            {
                case enMovingType.Walking:
                    {
                        GetComponent<ChaJumper>().enabled = true;
                        GetComponent<ChaFly>().enabled = false;
                        GetComponent<ChaSuspend>().enabled = false;
                    }
                    break;
                case enMovingType.Sliding:
                    {
                        GetComponent<ChaJumper>().enabled = false;
                        GetComponent<ChaFly>().enabled = true;
                        GetComponent<ChaSuspend>().enabled = false;
                    }
                    break;
                case enMovingType.suspending:
                    {
                        GetComponent<ChaJumper>().enabled = false;
                        GetComponent<ChaFly>().enabled = false;
                        GetComponent<ChaSuspend>().enabled = true;
                    }
                    break;
                default:
                    break;
            }
        } 

        public void Awake()
        {
            m_BodyBound = new Bounds
                (new Vector3(0, m_BodyHeight / 2, 0),
                new Vector3(m_BodyWidth, m_BodyHeight, m_BodyWidth));
        }

        public void Update()
        {
            if (transform.position.y < -50)
            {
                transform.position += new Vector3(0,550,0);
            }
            
        }

    }
}