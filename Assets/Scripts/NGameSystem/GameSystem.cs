using System;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.NUI;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameSystem
{
    class GameSystem : MonoSingleton<GameSystem>
    {
        [SerializeField]
        private EventMng m_EventMng;
        [SerializeField]
        private InputMng m_InputMng;
        [SerializeField]
        private WorldMng m_WorldMng;
        [SerializeField]
        private PlayerMng m_PlayerMng;
        [SerializeField]
        private SaveMng m_SaveMng;
        [SerializeField]
        private GameUtility m_Utility;


        [SerializeField]
        private string m_strMenuScene = "GameMenu";
        [SerializeField]
        private string m_strPlayScene = "GamePlay";
        [SerializeField]
        private string m_strSettingFile = "/Setting.txt";
        [SerializeField]
        private GameSetting m_GameSetting = null;

        [SerializeField]
        private GameObject m_Prefab_LoadingScreen;

        //public GameSetting GameSettingIns { private set; get; }
        public EventMng EventMngIns { get { return m_EventMng; } }
        public InputMng InputMngIns { get { return m_InputMng; } }
        public WorldMng WorldMngIns { get { return m_WorldMng; } }
        public PlayerMng PlayerMngIns { get { return m_PlayerMng; } }
        public SaveMng SaveMngIns { get { return m_SaveMng; } }
        public GameSetting GameSettingIns { get { return m_GameSetting; } }
        public LoadingScreen LoadingScreenIns { private set; get; }
        public GameUtility Utility { get { return m_Utility; } }


        public string SettingPath { get { return Application.dataPath + m_strSettingFile; } }

        public Picker GetPicker()
        {
            return InputMngIns.PickerIns;
        }

        //Unity Function
        //--------------------------------------------------

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
     
            LoadSettingFile();
        }

        //Game Setting Relatived Function
        //--------------------------------------------------

        private void LoadSettingFile()
        {
            //Case: There is a setting file
            if (File.Exists(SettingPath))
            {
                string StrJson = File.ReadAllText(SettingPath);
                m_GameSetting = JsonUtility.FromJson<GameSetting>(StrJson);
            }
            else
            {
                m_GameSetting = new GameSetting();

                string StrJson = JsonUtility.ToJson(m_GameSetting);

                File.WriteAllText(SettingPath, StrJson);
            }
        }

        public GameSetting ResetSettingToDefault()
        {
            m_GameSetting = new GameSetting();

            string StrJson = JsonUtility.ToJson(m_GameSetting);

            File.WriteAllText(SettingPath, StrJson);

            return m_GameSetting;
        }

        public void SaveSettings()
        {
            if (m_GameSetting != null)
            {
                string StrJson = JsonUtility.ToJson(m_GameSetting);
                File.WriteAllText(SettingPath, StrJson);
            }
        }


        public void ApplySettings(bool SaveToFile = true)
        {
            EventMngIns.ApplySettings(m_GameSetting);
            InputMngIns.ApplySettings(m_GameSetting);
            WorldMngIns.ApplySettings(m_GameSetting);
            PlayerMngIns.ApplySettings(m_GameSetting);
            SaveMngIns.ApplySettings(m_GameSetting);

            //save settings to a file
            if (SaveToFile)
            {
                string StrJson = JsonUtility.ToJson(m_GameSetting);
                File.WriteAllText(SettingPath, StrJson);
            }
        }

        //Scene Relatived Function
        //--------------------------------------------------

        public void RunMenuScene()
        {
            SceneManager.LoadScene(m_strMenuScene);
        }

        public void RunPlayScene()
        {         
            SceneManager.LoadScene(m_strPlayScene);
        }

        public void InitMenuScene()
        {
            m_EventMng.enabled = false;
            m_InputMng.enabled = false;
            m_WorldMng.enabled = false;
            m_PlayerMng.enabled = false;
            m_SaveMng.enabled = false;
        }

        public void InitPlayScene()
        {
            m_EventMng.enabled = true;
            m_WorldMng.enabled = true;
            m_PlayerMng.enabled = true;
            m_InputMng.enabled = true;
            m_SaveMng.enabled = true;

            ApplySettings(false);
            
            //Load Save File
            m_SaveMng.InitSaveSystem();

            //Provide Event Service
            m_EventMng.ProvideEventServ();

            //Init Game Utility
            m_Utility.Init(m_SaveMng.WorldSeed);

            //Spawn World
            m_WorldMng.ProvideWorldServ(m_SaveMng.WorldSeed);
            m_WorldMng.BiomeMng.Init(m_Utility.NoiseMaker, m_WorldMng.WorldServ);
            m_WorldMng.GetComponent<MapMaker>().Init();
            m_WorldMng.SpawnWorld(m_SaveMng.PlayerPos, m_PlayerMng.PlayerView);



            //Init Loading Screen
            LoadingScreenIns =
                Instantiate(m_Prefab_LoadingScreen).GetComponent<LoadingScreen>();

            LoadingScreenIns.gameObject.SetActive(true);
            LoadingScreenIns.AddListener(InitPlayer);
            LoadingScreenIns.AddListener(InitInput);

            //Init Mesh Builder 
            ChunkMeshBuilder meshBuilder =
                Locator<IWorld>.GetService().Entity.GetComponent<ChunkMeshBuilder>();

            meshBuilder.AddListener(LoadingScreenIns.SetRemainedTask);

            LoadingScreenIns.SetTotalTask(meshBuilder.RemainedTask);

            meshBuilder.StartBuildingThread();
        }

        private void InitPlayer()
        {
            m_PlayerMng.SpawnPlayer();
        }
        private void InitInput()
        {
            m_InputMng.InitInputService();
            m_InputMng.InitController();
            m_InputMng.InitUISet();
        }
    }
}
