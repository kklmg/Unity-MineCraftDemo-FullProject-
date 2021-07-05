using UnityEngine;

namespace Assets.Scripts.NGameSystem
{
    class PlaySceneStarter : MonoBehaviour
    {
        public GameObject PrefabGameSystem;
        private GameObject InsGameSystem = null;

        private void Start()
        {
            InsGameSystem = GameObject.Find(PrefabGameSystem.name);          

            if (InsGameSystem == null)
            {
                InsGameSystem = Instantiate(PrefabGameSystem);
                InsGameSystem.name = PrefabGameSystem.name;
            }

            GameSystem system = InsGameSystem.GetComponent<GameSystem>();

            system.InitPlayScene();
        }
    }
}
