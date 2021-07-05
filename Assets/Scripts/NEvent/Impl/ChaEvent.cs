using UnityEngine;

namespace Assets.Scripts.NEvent
{
    //Event Character Movement
    public class E_Cha_TranslateRequest_XZ : EventBase<E_Cha_TranslateRequest_XZ>
    {
        public Vector3 Translation { get; set; }

        public E_Cha_TranslateRequest_XZ(Vector3 value)
        {
            Translation = value;
        }
    }

    public class E_Cha_TranslateRequest_Y : EventBase<E_Cha_TranslateRequest_Y>
    {
        public float Velocity { set; get; }

        public E_Cha_TranslateRequest_Y(float velocity)
        {
            Velocity = velocity;
        }
    }

    public class E_Cha_TranslateOrder : EventBase<E_Cha_TranslateOrder>
    {
        public Vector3 Translation { get; set; }

        public E_Cha_TranslateOrder(Vector3 value)
        {
            Translation = value;
        }
    }


    public class E_Cha_YawRequest : EventBase<E_Cha_YawRequest>
    {
        public float Value { set; get; }
        public E_Cha_YawRequest(float _value)
        {
            Value = _value;
        }
    }

    public class E_Cha_RotateRequest : EventBase<E_Cha_RotateRequest>
    {
        public Vector3 Rotation { get; }
        public E_Cha_RotateRequest(Vector3 value)
        {
            Rotation = value;
        }
    }


    public class E_Cha_Spawned : EventBase<E_Cha_Spawned>
    {
    }
    public class E_Cha_LeaveGround : EventBase<E_Cha_LeaveGround>
    {
    }

    public class E_Cha_Collision : EventBase<E_Cha_Collision>
    {
    }
    public class E_Cha_TouchGround : EventBase<E_Cha_TouchGround>
    {
    }
    public class E_Cha_TouchUpsideBlock : EventBase<E_Cha_TouchUpsideBlock>
    {
    }



    public class E_Cha_JumpUp : EventBase<E_Cha_JumpUp>
    {
        private float m_Force;
        public float Force { get { return m_Force; } }

        public E_Cha_JumpUp(float force)
        {
            m_Force = force;
        }
    }
    public class E_Cha_Fly : EventBase<E_Cha_Fly>
    {
        [SerializeField]
        private float m_ForceUp;
        [SerializeField]
        private float m_ForceFront;

        public float ForceUp { get { return m_ForceUp; } }
        public float ForceFront { get { return m_ForceFront; } }

        public E_Cha_Fly(float forceUP,float forceFront)
        {
            m_ForceUp = forceUP;
            m_ForceFront = forceFront;
        }
    }
    public class E_Cha_Suspend : EventBase<E_Cha_Suspend>
    {
        private float m_Force;
        public float Force { get { return m_Force; } }

        public E_Cha_Suspend(float force)
        {
            m_Force = force;
        }
    }

    public class E_Cha_HasMoved : EventBase<E_Cha_HasMoved>
    {
    }
}
