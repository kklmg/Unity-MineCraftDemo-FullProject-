using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    class ToMenuButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(GoToMenu);
        }

        void GoToMenu()
        {
            GameSystem system = MonoSingleton<GameSystem>.Instance;

            system.SaveMngIns.SaveCurProgress();
            system.RunMenuScene(); 
        }

    }
}
