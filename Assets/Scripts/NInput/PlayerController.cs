using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NCharacter;

namespace Assets.Scripts.NInput
{
    class PlayerController : MonoBehaviour
    {
        private float m_fHor; //Horizontol Input
        private float m_fVer; //Vertical Input
        private float m_fYaxis;// Yaxis Input
        private float m_fYaw; //Yaw Input
        private bool m_bJump;


        private Vector3 m_Translation;
        private IController m_Controller;

        private Character m_Character;
        private ChaPlayer m_Player;
        private ChaBevData m_BevData;   //Player Behavior Board

        

        private float m_Sensitivity = 1.0f;
        public float RotateSensitivity
        {
            set
            {
                m_Sensitivity = value;
            }
        }

        //-------------------------------------------------------------------------------------------

        private void Start()
        {
            m_Controller = Locator<IController>.GetService();

            m_Player = MonoSingleton<GameSystem>.Instance.
                PlayerMngIns.PlayerIns.GetComponent<ChaPlayer>();

            m_BevData = m_Player.GetComponent<ChaBevExecuter>().BevData;

            m_Character = m_Player.GetComponent<Character>();
        }

        private void Update()
        {
            switch (m_Character.MovingType)
            {
                case enMovingType.Walking:
                    {
                        CheckInput_XZ();
                        CheckInput_Yaw();
                        CheckInput_Jump();
                    }
                    break;
                case enMovingType.Sliding:
                    {
                        CheckInput_XZ();
                        CheckInput_YZ();
                        CheckInput_Yaw();
                        CheckInput_Jump();
                    }
                    break;
                case enMovingType.suspending:
                    {
                        CheckInput_XZ();
                        CheckInput_Y();
                        CheckInput_Yaw();
                        CheckInput_Jump();
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnGUI()
        {

            GUI.contentColor = Color.blue;
            switch (m_Character.MovingType)
            {
                case enMovingType.Walking:
                    {
                        GUI.Label(new Rect(0 + 10, 0 + 70, 160, 20), "Jump: Space");
                    }
                    break;
                case enMovingType.Sliding:
                    {
                        GUI.Label(new Rect(0 + 10, 0 + 70, 160, 20), "GoUp:   R");
                        GUI.Label(new Rect(0 + 10, 0 + 90, 160, 20), "GoDown: F");
                    }
                    break;
                case enMovingType.suspending:
                    {
                        GUI.Label(new Rect(0 + 10, 0 + 70, 160, 20), "GoUp:   R");
                        GUI.Label(new Rect(0 + 10, 0 + 90, 160, 20), "GoDown: F");    
                    }
                    break;
                default:
                    break;
            }
        }

        //-------------------------------------------------------------------------------------------------


        private void CheckInput_XZ()
        {
            //get input
            m_fHor = m_Controller.Horizontal();
            m_fVer = m_Controller.Vertical();

            //check if translation valuies are valid
            if (!Mathf.Approximately(0.0f, m_fHor)
                || !Mathf.Approximately(0.0f, m_fVer))
            {
                //compute translation per frame
                m_Translation.x = m_fHor;
                m_Translation.z = m_fVer;

                //write value to bev Board
                m_BevData.Request_Translation.SetRequest(m_Translation); ;
            }
        }

        private void CheckInput_Y()
        {
            m_fYaxis = m_Controller.Move_Y();
           

            if (!Mathf.Approximately(0.0f, m_fYaxis))
            {
                //write value to bev Board
                m_BevData.Request_UpDown.SetRequest(m_fYaxis); ;
            }
        }

        private void CheckInput_YZ()
        {
            m_fYaxis = m_Controller.Move_Y();
            m_fVer = m_Controller.Vertical();

            if (!Mathf.Approximately(0.0f, m_fYaxis) || !Mathf.Approximately(0.0f, m_fVer))
            {
                //write value to bev Board
                m_BevData.Request_Slide.SetRequest
                    (new KeyValuePair<float, float>
                    (m_fYaxis, m_fVer));
            }
        }


        private void CheckInput_Jump()
        {
            m_bJump = m_Controller.Jump();

            //jump 
            if (m_bJump)
                m_BevData.Request_Jump.SetRequest(true);
        }

        private void CheckInput_Yaw()
        {
            m_fYaw = m_Controller.Rotate_Yaw();

            //check if Yaw value is valid
            if (!Mathf.Approximately(0.0f, m_fYaw))
            {
                //write value to bev Board
                m_BevData.Request_Yaw.SetRequest(m_fYaw * m_Sensitivity);
            }
        }



    }
}

