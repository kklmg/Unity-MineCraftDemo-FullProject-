using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NUI
{
    class BlockBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BlockSlot_Bar DefaultSlot;

        [SerializeField]
        private Color m_SelectedColor;
        [SerializeField]
        private Color m_UnSelectedColor;

        public Color SelectedColor { get { return m_SelectedColor; } }
        public Color UnselectedColor { get { return m_UnSelectedColor; } }

        public bool IsMenuOpen { get { return m_BlockMenu.IsOpened; } }

        [SerializeField]
        private BlockMenu m_BlockMenu;

        private BlockSlot_Bar m_SelectedSlot;

        private IWorld m_World;
        private Picker m_Picker;

        public void SetSelection(BlockSlot_Bar slot)
        {
            if (m_SelectedSlot != null)
                m_SelectedSlot.DisSelect();

            m_SelectedSlot = slot;
        }

        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
            m_Picker = MonoSingleton<GameSystem>.Instance.InputMngIns.PickerIns;

            BlockPalette Palette = m_World.BlkPalette;

            var buttons = GetComponentsInChildren<BlockSlot_Bar>();

            byte i = 1;
            foreach (var bt in buttons)
            {
                if (i >= Palette.Count) break;

                bt.SetBlock(Palette[i]);
                ++i;
            }

            //set default selected
            DefaultSlot.OnSelect();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_BlockMenu.IsOpened == false)
                MonoSingleton<GameSystem>.Instance.InputMngIns.StopHandleInput();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_BlockMenu.IsOpened == false)
                MonoSingleton<GameSystem>.Instance.InputMngIns.StartHandleInput();
        }

        public void OnSelectSlot(BlockSlot_Bar Slot)
        {
            //Case: Block Menu is Opened
            if (m_BlockMenu.IsOpened)
            {
                Slot.DisSelect();
                if (m_BlockMenu.IsSlotSelected)
                    Slot.SetBlock(m_BlockMenu.SelectedBlock);
                m_BlockMenu.ClearSelection();
            }
            else
            {
                //DisSelect Current Slot
                if (m_SelectedSlot != null)
                {
                    m_SelectedSlot.DisSelect();
                }

                //Set Selected Slot
                m_SelectedSlot = Slot;

                //Change Color
                m_SelectedSlot.GetComponent<Image>().color = m_SelectedColor;

                //Set Picker 
                m_Picker.SelectedBlock = m_SelectedSlot._Block;
            }
        }

        public void DisselectSlot(BlockSlot_Bar Slot)
        {
            //Clear Selection
            m_SelectedSlot = null;

            //Change Color
            Slot.GetComponent<Image>().color = m_UnSelectedColor;

            //Set Picker 
            m_Picker.SelectedBlock = null;
        }

        public void ClearSelection()
        {
            //DisSelect Current Slot
            if (m_SelectedSlot != null)
            {
                m_SelectedSlot.DisSelect();
            }
        }
    }
}
