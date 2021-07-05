using UnityEngine;

namespace Assets.Scripts.NUI
{   
    public abstract class MenuBase : MonoBehaviour
    {
        protected GameMenu m_GameMenu;

        private void Awake()
        {
            m_GameMenu = GetComponentInParent<GameMenu>();
            Init();
        }

        protected abstract void Init();      
    }
}
