using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    class ToolBar : MonoBehaviour
    { 
        public Button UndoButton;

        private Picker m_BlockPicker;

        public void Awake()
        {
            m_BlockPicker = MonoSingleton<GameSystem>.Instance.GetPicker();

            UndoButton.onClick.AddListener(ClickUndoButton);
        }

        public void ActivePutMode()
        {
            m_BlockPicker.PickerMode = EnPickerMode.ePut;
        }

        public void ActiveSetMode()
        {
            m_BlockPicker.PickerMode = EnPickerMode.eSet;
        }

        public void ActiveDestroyMode()
        {
            m_BlockPicker.PickerMode = EnPickerMode.eDestroy;
        }

        public void DisActiveButton()
        {
            m_BlockPicker.PickerMode = EnPickerMode.eNone;
        }

        public void ClickUndoButton()
        {
            m_BlockPicker.UndoBlockModify(); 
        }

    }
}
