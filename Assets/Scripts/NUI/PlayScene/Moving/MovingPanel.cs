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
    class MovingPanel : MonoBehaviour
    {
        public PressButton FRONT_Button;
        public PressButton BACK_Button;
        public PressButton LEFT_Button;
        public PressButton RIGHT_Button;

        private ChaPlayer m_Player;
        private ChaBevData m_BevData;

        private void Start()
        {
            m_Player = MonoSingleton<GameSystem>.Instance.
                PlayerMngIns.PlayerIns.GetComponent<ChaPlayer>();

            m_BevData = m_Player.GetComponent<ChaBevExecuter>().BevData;
        }

        public void Update()
        {
            float _front = 0, _back = 0, _left = 0, _right = 0;
            if (FRONT_Button.TryGetValue(out _front) || BACK_Button.TryGetValue(out _back)
                || LEFT_Button.TryGetValue(out _left) || RIGHT_Button.TryGetValue(out _right))
            {
                m_BevData.Request_Translation.SetRequest(new Vector3(_right - _left, 0, _front - _back));
            }
        }

        public bool TryGetHorizontol(out float _value)
        {
            float _left = 0, _right = 0;
            if (LEFT_Button.TryGetValue(out _left) || RIGHT_Button.TryGetValue(out _right))
            {
                _value = _right - _left;
                return true;
            }
            else
            {
                _value = 0;
                return false;
            }
        }

        public bool TryGetVertical(out float _value)
        {
            float _front = 0, _back = 0;
            if (FRONT_Button.TryGetValue(out _front) || BACK_Button.TryGetValue(out _back))
            {
                _value = _front - _back;
                return true;
            }
            else
            {
                _value = 0;
                return false;
            }
        }
    }
}
