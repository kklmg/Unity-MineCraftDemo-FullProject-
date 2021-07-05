using System.Text;

using UnityEngine.UI;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    class CreateMenu : MenuBase
    {
        public Button Button_RandSeed;
        public Button Button_CreateWorld;
        public InputField IF_Seed;
        public InputField IF_Name;

        protected override void Init()
        {
            IF_Seed.text = GenerateRandString(10);

            //Button generate random seed
            Button_RandSeed.onClick.AddListener(GetRandomSeed);

            //Button Create World
            Button_CreateWorld.onClick.AddListener(CreateWorld);

            //Button_CreateWorld.
        }

        private void CreateWorld()
        {
            GameSystem system = MonoSingleton<GameSystem>.Instance;
            system.SaveMngIns.CreateSaveFile(IF_Name.text, IF_Seed.text);
            system.RunPlayScene();
        }

        private string GenerateRandString(int length)
        {
            const string strs = "abcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder builder = new StringBuilder();

            while (length-->0)
            {
                builder.Append(strs[UnityEngine.Random.Range(0, strs.Length)]);
            }
            return builder.ToString();
        }

        private void GetRandomSeed()
        {
            IF_Seed.text = GenerateRandString(UnityEngine.Random.Range(4, 10));
        }
    }
}
