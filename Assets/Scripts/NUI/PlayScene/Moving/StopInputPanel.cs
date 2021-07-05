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
    class StopInputPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Picker m_Picker;

        private void Start()
        {
            m_Picker = MonoSingleton<GameSystem>.Instance.InputMngIns.PickerIns;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_Picker.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_Picker.gameObject.SetActive(true);
        }
    }
}
