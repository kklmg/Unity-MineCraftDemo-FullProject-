using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NCharacter;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NUI
{
    class ActionPanel : MonoBehaviour
    {
        public PressButton UP_Button;
        public PressButton DOWN_Button;
        public Button JUMP_Button;

        [SerializeField]
        private MovingPanel m_MovingPanel;

        private Character m_Character;
        private ChaBevData m_BevData;

        private void Start()
        {
            m_Character = MonoSingleton<GameSystem>.Instance.
                PlayerMngIns.PlayerIns.GetComponent<Character>();

            m_BevData = m_Character.GetComponent<ChaBevExecuter>().BevData;

            JUMP_Button.onClick.AddListener(DoJump);
        }

        private void DoJump()
        {
            m_BevData.Request_Jump.SetRequest(true);
        }

        public void Update()
        {
            float _up = 0, _down = 0;
            if (m_Character.MovingType == enMovingType.suspending)
            {
                if (UP_Button.TryGetValue(out _up) || DOWN_Button.TryGetValue(out _down))
                {
                    m_BevData.Request_UpDown.SetRequest(_up - _down);
                }
            }

            else if (m_Character.MovingType == enMovingType.Sliding)
            {
                float ver = 0;
                if (UP_Button.TryGetValue(out _up) || DOWN_Button.TryGetValue(out _down)
                    || m_MovingPanel.TryGetVertical(out ver))
                {
                    m_BevData.Request_Slide.SetRequest
                        (new KeyValuePair<float, float>(_up - _down, ver));
                    Debug.Log(_up - _down);
                    Debug.Log(ver);
                }
            }

            

        }
    }
}
