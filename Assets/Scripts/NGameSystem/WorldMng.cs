using UnityEngine;
using System.Threading;

using Assets.Scripts.NData;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameSystem
{
    [System.Serializable]
    [RequireComponent(typeof(BiomeMng))]
    class WorldMng : MonoBehaviour, IGameMng
    {
        public GameObject m_WorldPrefab;

        public IWorld WorldServ { private set; get; }
        public GameObject WorldIns { private set; get; }
        public BiomeMng BiomeMng { get { return GetComponent<BiomeMng>(); } }


        public void ProvideWorldServ(string seed)
        {
            WorldIns = Instantiate(m_WorldPrefab);

            WorldServ = WorldIns.GetComponent<World>();
            
            WorldServ.Init(seed);

            Locator<IWorld>.ProvideService(WorldServ);
        }

        public void SpawnWorld(Vector3 center,int extends)
        {
            //Get Spawner
            ChunkSpawner spawner = WorldServ.Entity.GetComponent<ChunkSpawner>();

            //Spawn
            spawner.SpawnAtArea(center, extends, WorldServ);
        }

        public void ApplySettings(GameSetting setting)
        {
            return;
        }

    }
}


