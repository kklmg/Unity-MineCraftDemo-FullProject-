using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;


namespace Assets.Scripts.NUI
{
    class BlockMenu : MonoBehaviour
    {
        //Field
        //-----------------------------------------------------------

        [SerializeField]
        private Color m_SelectedColor;
        [SerializeField]
        private Color m_UnselectedColor;

        private bool m_IsOpened;

        [SerializeField]
        private BlockSlot_Menu m_SelectedSlot;

        //Property
        //-----------------------------------------------------------

        public bool IsOpened { get { return m_IsOpened; } }
        public bool IsSlotSelected { get { return m_SelectedSlot != null; } }
        public Color SelectedColor { get { return m_SelectedColor; } }
        public Color UnselectedColor { get { return m_UnselectedColor; } }

        public IBlock SelectedBlock
        {
            get
            {
                if (m_SelectedSlot != null)
                {
                    return m_SelectedSlot._Block;
                }
                else return null;
            }
        }

        //Unity function
        //-----------------------------------------------------------

        public void Start()
        {
            BlockPalette palette = Locator<IWorld>.GetService().BlkPalette;
            var Slots = GetComponentsInChildren<BlockSlot_Menu>();

            byte i = 1;
            foreach (var slot in Slots)
            {
                if (i >= palette.Count) break;

                slot.SetBlock(palette[i]);
                ++i;
            }
        }


        public void OnSelectSlot(BlockSlot_Menu Slot)
        {
            if (m_SelectedSlot != null)
            {
                //swap blocks
                IBlock temp = m_SelectedSlot._Block;
                m_SelectedSlot.SetBlock(Slot._Block);
                Slot.SetBlock(temp);


                m_SelectedSlot.DisSelect();
                Slot.DisSelect();
            }
            else
            {
                m_SelectedSlot = Slot;
                Slot.GetComponent<Image>().color = m_SelectedColor;
            }
        }

        public void DisselectSlot(BlockSlot_Menu Slot)
        {
            m_SelectedSlot = null;
            Slot.GetComponent<Image>().color = m_UnselectedColor;
        }

        public void Open(BlockBar bar)
        {
            m_IsOpened = true;
            gameObject.SetActive(true);

            bar.ClearSelection();
            //Debug.Log("clear");

            MonoSingleton<GameSystem>.Instance.InputMngIns.StopHandleInput();
        }

        public void Close(BlockBar bar)
        {
            m_IsOpened = false;

            if (m_SelectedSlot != null)
            {
                m_SelectedSlot.DisSelect();
            }
            gameObject.SetActive(false);

            MonoSingleton<GameSystem>.Instance.InputMngIns.StartHandleInput();
        }

        public void ClearSelection()
        {
            if (m_SelectedSlot != null)
            {
                m_SelectedSlot.DisSelect();
            }
        }

    }
}
