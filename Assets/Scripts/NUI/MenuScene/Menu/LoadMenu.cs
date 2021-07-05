using System.IO;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NData;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGameSystem;

namespace Assets.Scripts.NUI
{
    class LoadMenu : MenuBase
    {
        public GameObject ContentsArea;
        public GameSave[] m_arrWorldSaves;

        public Button buttonPrefab;

        protected override void Init()
        {
            string[] Paths
                = MonoSingleton<GameSystem>.Instance.SaveMngIns.GetAllSavePaths();

            if (Paths == null) return;

            string StrJson;
            GameSave SaveIns;
            foreach (var path in Paths)
            {
                StrJson = File.ReadAllText(path);
                SaveIns = JsonUtility.FromJson<GameSave>(StrJson);

                var Content = Instantiate(buttonPrefab, ContentsArea.transform)
                    .GetComponent<SaveContent>();
                Content.Init(SaveIns, path);
            }

        }
    }
}
