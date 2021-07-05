using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NCache;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{
    public class WorldEntity : MonoBehaviour
    {
        //Field
        //---------------------------------------------------------------------------
        public GameObject Prefab_Chunk;

        private IWorld m_World;
        private SaveMng m_SaveMng;
        private ChunkMeshBuilder m_ChunkMeshBuilder;

        [SerializeField]
        private uint m_CacheSize = 20;
        [SerializeField]
        private Dictionary<Vector2Int, Chunk> m_Chunks;
        private LRUCache<Vector2Int, Chunk> m_Garbages;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            //int view = MonoSingleton<GameSystem>.Instance.PlayerMngIns.PlayerView;

            //m_refWorld = GetComponentInParent<IWorld>();
            m_Chunks = new Dictionary<Vector2Int, Chunk>();
            m_Garbages = new LRUCache<Vector2Int, Chunk>(m_CacheSize);

            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
            m_ChunkMeshBuilder = GetComponent<ChunkMeshBuilder>();
        }

        //public Function
        //---------------------------------------------------------------------------  
        public void Spawn(ChunkInWorld _chunkinworld)
        {
            Chunk TempChunk;
            //Case: The Chunk has already spawned
            if (m_Chunks.ContainsKey(_chunkinworld.Value))
            {
                //Debug.Log("already spawned");
                return;
               
            }
            //Case: The Chunk is in GarbageCache
            else if (m_Garbages.TryRemove(_chunkinworld.Value, out TempChunk))
            {
                TempChunk.gameObject.SetActive(true);
                m_Chunks.Add(_chunkinworld.Value, TempChunk);
                //Debug.Log("From garbage");
            }
            //instantiate new
            else
            {
                //make a chunk instance
                TempChunk = Instantiate(Prefab_Chunk, transform).GetComponent<Chunk>();

                //Init Chunk
                TempChunk.SetLocation(_chunkinworld);
                TempChunk.GenerateBlocks();

                //Load data From save file
                m_SaveMng.LoadBlock(_chunkinworld, TempChunk);

                //Build Mesh
                //TempChunk.BuildMesh();
                m_ChunkMeshBuilder.AddRequest(TempChunk);

                //Add Chunk
                m_Chunks.Add(_chunkinworld.Value, TempChunk);

                //Debug.Log("Make New");
            }
        }

        public void Remove(ChunkInWorld _chunkinworld)
        {
            //Case: The Chunk has spawned
            if (m_Chunks.TryGetValue(_chunkinworld.Value,out var removedChunk))
            {
                //remove from chunk dictionary
                m_Chunks.Remove(_chunkinworld.Value);

                //disactive chunk
                removedChunk.gameObject.SetActive(false);

                //add to garbage Cache
                Chunk removedLRU = m_Garbages.Put(_chunkinworld.Value, removedChunk);

                //destroy lru garbage
                if (removedLRU != null)
                {
                    //Debug.Log("remove"+removedLRU.Loc);
                    Destroy(removedLRU.gameObject);
                }
            }
        }

        public Chunk GetChunk(ChunkInWorld _chunkinworld)
        {
            if (m_Chunks.TryGetValue(_chunkinworld.Value, out Chunk TempChunk))
            {
                return TempChunk;
            }
            else return null;
        }
    }
}


//    

//        public bool TryGetChunk(ChunkInWorld _chunkinworld, out Chunk chunk)
//        {
//            return m_Chunks.TryGetValue(_chunkinworld.Value, out chunk);
//        }

//        public bool Contains(ChunkInWorld _chunkinworld)
//        {
//            return m_Chunks.ContainsKey(_chunkinworld.Value);
//        }

//        public void Remove(ChunkInWorld _chunkinworld)
//        {
//            m_Chunks.Remove(_chunkinworld.Value);
//        }

//        public void AddChunk(ChunkInWorld inWorld, Chunk chunk)
//        {
//            m_Chunks.Add(inWorld.Value,chunk);
//        }

//        public void GetNotInArea(ref AreaRect area,List<ChunkInWorld> Receive)
//        {
//            foreach (var data in m_Chunks)
//            {
//                if (!area.IsInvolvePoint(data.Key))
//                {
//                    Receive.Add(new ChunkInWorld(data.Key,m_World));
//                }
//            }
//        }
//    }
//}



//using System.Collections.Generic;
//using UnityEngine;

//using Assets.Scripts.NData;

//namespace Assets.Scripts.NWorld
//{
//    public class WorldEntity : MonoBehaviour
//    {
//        //Field
//        //---------------------------------------------------------------------------
//        public GameObject Prefab_Chunk;

//        private IWorld m_refWorld;

//        [SerializeField]
//        private Dictionary<Vector2Int, Chunk> m_Chunks;

//        //Unity Function
//        //---------------------------------------------------------------------------
//        private void Awake()
//        {
//            m_refWorld = GetComponentInParent<IWorld>();
//            m_Chunks = new Dictionary<Vector2Int, Chunk>();
//        }


//        //public Function
//        //---------------------------------------------------------------------------  
//        public Chunk GetChunk(ChunkInWorld _chunkinworld)
//        {
//            if (m_Chunks.TryGetValue(_chunkinworld.Value, out Chunk TempChunk))
//            {
//                return TempChunk;
//            }
//            else return null;
//        }

//        public bool TryGetChunk(ChunkInWorld _chunkinworld, out Chunk chunk)
//        {
//            return m_Chunks.TryGetValue(_chunkinworld.Value, out chunk);
//        }

//        public bool Contains(ChunkInWorld _chunkinworld)
//        {
//            return m_Chunks.ContainsKey(_chunkinworld.Value);
//        }

//        public void Remove(ChunkInWorld _chunkinworld)
//        {
//            m_Chunks.Remove(_chunkinworld.Value);
//        }

//        public void AddChunk(ChunkInWorld inWorld, Chunk chunk)
//        {
//            m_Chunks.Add(inWorld.Value,chunk);
//        }

//        public void GetNotInArea(ref AreaRect area,List<ChunkInWorld> Receive)
//        {
//            foreach (var data in m_Chunks)
//            {
//                if (!area.IsInvolvePoint(data.Key))
//                {
//                    Receive.Add(new ChunkInWorld(data.Key,m_refWorld));
//                }
//            }
//        }
//    }
//}
