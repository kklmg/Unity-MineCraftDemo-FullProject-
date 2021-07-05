using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;



namespace Assets.Scripts.NCharacter
{
    public class ChaBevData : BevData
    {
        private Communicator m_Communicator;

        public bool isWalking;
        public bool isGrounded;
        public bool isInAir;

        //public bool JumpRequest;
        public Character Character;

        public Request<bool> Request_Jump;
        public Request<float> Request_UpDown;
        public Request<KeyValuePair<float, float>> Request_Slide;
        public Request<Vector3> Request_Translation;
        public Request<float> Request_Yaw;

        public ChaBevData(Character _Character, Communicator communicator)
        {
            Character = _Character;
            m_Communicator = communicator;

            Request_Translation = new Request<Vector3>();
            Request_Yaw = new Request<float>();
            Request_Jump = new Request<bool>();
            Request_UpDown = new Request<float>();
            Request_Slide = new Request<KeyValuePair<float, float>>();
        }

        public void NotifyOtherComponents(IEvent _event)
        {
            m_Communicator.Publish(_event);
        }
    }
}
