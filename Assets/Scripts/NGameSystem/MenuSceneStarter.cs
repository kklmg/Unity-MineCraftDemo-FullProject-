using UnityEngine;

namespace Assets.Scripts.NGameSystem
{
    class MenuSceneStarter : MonoBehaviour
    {
        public GameObject PrefabGameSystem;
        private GameObject InsGameSystem;

        private void Awake()
        {
            InsGameSystem = GameObject.Find(PrefabGameSystem.name);

            if (InsGameSystem == null)
            {
                InsGameSystem = Instantiate(PrefabGameSystem);
                InsGameSystem.name = PrefabGameSystem.name;
            }

            GameSystem system = InsGameSystem.GetComponent<GameSystem>();

            system.InitMenuScene();
        }
    }
}