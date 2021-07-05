
using UnityEngine;

namespace Assets.Scripts.NUI
{
    class SelectionGroup : MonoBehaviour
    {
        [SerializeField]
        private bool m_isSelectionNecessary = true;
        [SerializeField]
        SelectableButton m_DefaultButton;

        public SelectableButton CurSelection { private set; get; }

        private void Start()
        {
            if (m_DefaultButton != null)
                OnSelect(m_DefaultButton);
        }

        public void SetSelection(SelectableButton Selectedbutton)
        {
            CurSelection = Selectedbutton;
        }

        public void ClearSelection()
        {
            if (CurSelection != null)
            {
                CurSelection.OnDisSelect();
                CurSelection = null;
            }
        }

        public void OnSelect(SelectableButton target)
        {
            ClearSelection();
            CurSelection = target;
            target.OnSelect();
        }

        public void OnDisSelect(SelectableButton target)
        {
            if (CurSelection == target && m_isSelectionNecessary)
            {
                return;
            }

            target.OnDisSelect();
        }
    }
}
