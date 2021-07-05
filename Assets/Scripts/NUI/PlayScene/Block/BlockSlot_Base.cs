using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NWorld;

namespace Assets.Scripts.NUI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    abstract class BlockSlot_Base : MonoBehaviour
    {
        [SerializeField]
        private Image m_Icon;

        [SerializeField]
        private IBlock m_block;

        public IBlock _Block { get { return m_block; } }

        public bool IsSelected { private set; get; }

        //public bool IsTrigger

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Click);
        }

        public void SetBlock(IBlock block)
        {
            if (block == null)
            {
                m_block = null;
                m_Icon.sprite = null;
            }
            else
            {
                m_block = block;
                m_Icon.sprite = m_block.Icon;
            }
        }

        public void OnSelect()
        {
            if (IsSelected == false)
            {
                IsSelected = true;
                _OnSelect();
            }
        }

        public void DisSelect()
        {
            if (IsSelected == true)
            {
                IsSelected = false;
                _DisSelect();
            }
        }

        private void Click()
        {
            if (IsSelected == true)
            {
                IsSelected = false;
                _DisSelect();
                
                //Debug.Log("DisSelect");
            }
            else
            {
                IsSelected = true;
                _OnSelect();
               
                //Debug.Log("OnSelect");
            }
        }

        protected abstract void _OnSelect();

        protected abstract void _DisSelect();
    }
}
