using System.IO;

using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.NData;

namespace Assets.Scripts.NGameSystem
{
    [System.Serializable]
    class SaveMng : MonoBehaviour, IGameMng
    {
        public string Test_Name = "TestWorld";
        public string Test_Seed = "HelloWorld";

        [SerializeField]
        private string m_SaveDirectory = "/save";
        [SerializeField]
        private string m_FilePostfix = ".txt";

        [SerializeField]
        private GameSave m_LoadedSave;
        private string m_LoadedFilePath;

        [SerializeField]
        private WorldMng m_WorldMng;

        private SaveHelper_World m_SaveHelper;

        public string SaveDirectory
        {
            get { return Application.dataPath + m_SaveDirectory; }
        }
        public Vector3 PlayerPos
        {
            get { return m_LoadedSave.PlayerPos; }
            set { m_LoadedSave.PlayerPos = value; }
        }
        public float PlayerAltitude
        {
            get { return m_LoadedSave.PlayerPos.y; }
            set { m_LoadedSave.PlayerPos.y = value; }
        }
        public Quaternion PlayerRot
        {
            get { return m_LoadedSave.playerRot; }
            set { m_LoadedSave.playerRot = value; }
        }
        public bool IsFirstCreated
        {
            get { return m_LoadedSave.HasPlayerSpawned; }
            set { m_LoadedSave.HasPlayerSpawned = value; }
        }
        public string WorldSeed
        {
            get { return m_LoadedSave.WorldSeed; }
        }
        public bool IsSaveFileLoaded { get; private set; }

        public void Awake()
        {
            //Create Directory
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        public void OnApplicationQuit()
        {
            SaveCurProgress();
            IsSaveFileLoaded = false;
        }
        public void OnDisable()
        {
            SaveCurProgress();
            IsSaveFileLoaded = false;
        }

        public void InitSaveSystem()
        {
#if UNITY_EDITOR
            if (IsSaveFileLoaded == false)
            {
                if (File.Exists(SaveDirectory + '/' + Test_Name + m_FilePostfix))
                {
                    LoadSaveFile(SaveDirectory + '/' + Test_Name + m_FilePostfix);
                }
                else
                {
                    Debug.Log(Test_Name);
                    Debug.Log(Test_Seed);
                    Debug.Log(SaveDirectory + '/' + Test_Name + m_FilePostfix);
                    CreateSaveFile(Test_Name, Test_Seed);
                }
            }
#endif
            m_SaveHelper = new SaveHelper_World(m_LoadedSave.WorldModfication);
        }

        public string[] GetAllSavePaths()
        {
            if (!Directory.Exists(SaveDirectory)) return null;

            DirectoryInfo directoryInfo  = new DirectoryInfo(SaveDirectory);

            FileInfo[] files = directoryInfo.GetFiles("*.txt");

            if (files.Length == 0) return null;

            string [] res = new string[files.Length];

            int i = 0;
            foreach (var file in files)
            {
                res[i++] = file.FullName;
            }
            return res;
        }

        public void CreateSaveFile(string WorldName, string WorldSeed)
        {
            //Create instance of Save Data
            m_LoadedSave = new GameSave();
            m_LoadedSave.Init(WorldName, WorldSeed);

            string tempPath = SaveDirectory +"/"+ WorldName + m_FilePostfix;

            //Debug.Log(SaveDirectory + "/" + WorldName + m_FilePostfix);
            //Debug.Log(WorldName);
            //Debug.Log(m_FilePostfix);


            //Avoid Duplicate file
            int i = 1;
            while (File.Exists(tempPath))
            {
                tempPath = SaveDirectory + "/" + WorldName + '(' + i + ')' + m_FilePostfix;
                ++i;
            }

            m_LoadedFilePath = tempPath;

            //Debug.Log(tempPath);
            string strJson = JsonUtility.ToJson(m_LoadedSave);
            File.WriteAllText(tempPath, strJson);

            IsSaveFileLoaded = true;
        }

        public void LoadSaveFile(string path)
        {
            string StrJson = File.ReadAllText(path);
            m_LoadedSave= JsonUtility.FromJson<GameSave>(StrJson);
            m_LoadedFilePath = path;

            IsSaveFileLoaded = true;
        }

        public void LoadSaveIns(GameSave save, string path)
        {
            m_LoadedSave = save;
            m_LoadedFilePath = path;

            IsSaveFileLoaded = true;
        }

        public void SaveBlock(BlockLocation Location, byte blockId)
        {
            m_SaveHelper.Modify(Location, blockId,m_WorldMng.WorldServ);
        }
        public void LoadBlock(ChunkInWorld chunkInWorld, Chunk chunk)
        {
            m_SaveHelper.ApplyModification(chunkInWorld, chunk, m_WorldMng.WorldServ);
        }

        public void SavePlayerLocation(Transform PlayerTransform)
        {
            m_LoadedSave.PlayerPos = PlayerTransform.localPosition;
            m_LoadedSave.playerRot = PlayerTransform.localRotation;
        }

        public void SaveCurProgress()
        {
            if (IsSaveFileLoaded)
            {
                //Debug.Log(m_LoadedFilePath);
                m_LoadedSave.LastPlayed.Assign(System.DateTime.Now);
                File.WriteAllText(m_LoadedFilePath,JsonUtility.ToJson(m_LoadedSave));
            }
        }

        public void ApplySettings(GameSetting setting)
        {

            return;
        }
    }
}



