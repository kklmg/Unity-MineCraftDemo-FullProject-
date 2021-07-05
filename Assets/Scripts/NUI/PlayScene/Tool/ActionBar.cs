using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NCharacter;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    class ActionBar : MonoBehaviour
    {
        private Character m_Character;

        [SerializeField]
        private PressButton m_GoUpButton;
        [SerializeField]
        private PressButton m_GoDownButton;
        [SerializeField]
        private Button m_JumpButton;

        public void Awake()
        {
            m_Character = MonoSingleton<GameSystem>.Instance.PlayerMngIns.
                PlayerIns.GetComponent<Character>();
        }

        public void ActiveWalkMode()
        {
            m_Character.SetMovingType(enMovingType.Walking);

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                m_GoUpButton.gameObject.SetActive(false);
                m_GoDownButton.gameObject.SetActive(false);
                m_JumpButton.gameObject.SetActive(true);
            }
        }

        public void ActiveFlyMode()
        {
            m_Character.SetMovingType( enMovingType.Sliding);

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                m_GoUpButton.gameObject.SetActive(true);
                m_GoDownButton.gameObject.SetActive(true);
                m_JumpButton.gameObject.SetActive(false);
            }
        }

        public void ActiveSuspendMode()
        {
            m_Character.SetMovingType(enMovingType.suspending);

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                m_GoUpButton.gameObject.SetActive(true);
                m_GoDownButton.gameObject.SetActive(true);
                m_JumpButton.gameObject.SetActive(false);
            }
        }
    }
}
