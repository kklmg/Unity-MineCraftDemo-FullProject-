using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NData;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGameSystem;

namespace Assets.Scripts.NUI
{
    [RequireComponent(typeof(Button))]
    class SaveContent : MonoBehaviour
    {
        public Text Name;
        public Text Date;
        public Text Seed;

        private GameSave SaveFile { set; get; }
        private string FilePath { set; get; }

        public void Init(GameSave save,string path)
        {
            //throw new System.NotImplementedException();
            SaveFile = save;
            FilePath = path;
            Name.text += save.WorldName;
            Date.text += save.LastPlayed.ToString();
            Seed.text += save.WorldSeed.ToString();
            GetComponent<Button>().onClick.AddListener(Click);
        }

        public void Click()
        {
            GameSystem system = MonoSingleton<GameSystem>.Instance;
            system.SaveMngIns.LoadSaveIns(SaveFile, FilePath);
            system.RunPlayScene();
        }
    }
}
