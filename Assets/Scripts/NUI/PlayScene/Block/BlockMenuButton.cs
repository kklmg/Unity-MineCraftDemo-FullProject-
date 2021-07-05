using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.NUI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    class BlockMenuButton : MonoBehaviour
    {    
        [SerializeField]
        private BlockBar m_BlockBar;

        [SerializeField]
        private BlockMenu m_BlockMenu;

        private void Start()
        {
            m_BlockMenu.Close(m_BlockBar);
            GetComponent<Button>().onClick.AddListener(Click);
        }

        private void Click()
        {
            if (m_BlockMenu.IsOpened)
            {
                m_BlockMenu.Close(m_BlockBar);
            }
            else
            {
                m_BlockMenu.Open(m_BlockBar);
            }
        }
    }
}
