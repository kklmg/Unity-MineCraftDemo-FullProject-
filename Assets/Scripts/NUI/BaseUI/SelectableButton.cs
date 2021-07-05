using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.NUI
{
    [RequireComponent(typeof(Image))]
    class SelectableButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Color m_SelectedColor = Color.green;
        [SerializeField]
        private Color m_UnSelectedColor = Color.white;

        [SerializeField]
        private SelectionGroup m_SelectionGroup;

        [SerializeField]
        private UnityEvent m_OnSelection;
        [SerializeField]
        private UnityEvent m_OnDisSelection;

        private Image m_Image;

        public bool IsSelected { private set; get; }

        public void AddOnSelectListner(UnityAction action)
        {
            m_OnSelection.AddListener(action);
        }
        public void AddOnDisSelectListner(UnityAction action)
        {
            m_OnDisSelection.AddListener(action);
        }

        public void Awake()
        {
            if (m_SelectionGroup == null)
                m_SelectionGroup = GetComponentInParent<SelectionGroup>();

            m_Image = GetComponent<Image>();
            m_Image.color = m_UnSelectedColor;
        }

        public void OnSelect()
        {
            //change color
            m_Image.color = m_SelectedColor;
         
            m_OnSelection.Invoke();

            IsSelected = true;
        }

        public void OnDisSelect()
        {
            //change color
            m_Image.color = m_UnSelectedColor;

            m_OnDisSelection.Invoke();
            IsSelected = false;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_SelectionGroup == null)
            {
                if (IsSelected)
                    OnDisSelect();
                else
                    OnSelect();
            }
            else 
            {
                if (IsSelected)
                    m_SelectionGroup.OnDisSelect(this);
                else
                    m_SelectionGroup.OnSelect(this);
            }
        }
    }
}
