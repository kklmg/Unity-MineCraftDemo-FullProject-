using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NCache;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;


namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(WorldEntity))]
    public class ChunkSpawner : MonoBehaviour
    {
        private IWorld m_World;
        private WorldEntity m_WorldEntity;
        private SaveMng m_SaveMng;
       
        private PlayerMng m_PlayerMng;

        [SerializeField]
        private AreaRect m_CoverArea;
        private HashSet<Vector2Int> m_CoveredLoc;

        private LRUCache<Vector2Int, Chunk> m_Garbage;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            m_WorldEntity = GetComponent<WorldEntity>();
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
            m_CoveredLoc = new HashSet<Vector2Int>();
        }

        private void OnEnable()
        {
            Locator<IEventHelper>.GetService().
                Subscribe(E_Player_LeaveChunk.ID, Handle_PlayerLeaveChunk, enPriority.Highest);
            //Locator<IEventHelper>.GetService().
             //   Subscribe(E_Player_Spawned.ID, Handle_PlayerSpawn, enPriority.Highest);
        }
        private void OnDisable()
        {
            Locator<IEventHelper>.GetService().
               UnSubscribe(E_Player_LeaveChunk.ID, Handle_PlayerLeaveChunk);
        }


        //Event Handler
        //---------------------------------------------------------------------------   
        public bool SpawnAtArea(Vector3 center,int extend,IWorld world)
        {
            ChunkInWorld SpawnCenter = new ChunkInWorld(center, world);

            //figure out visiable area 
            AreaRect ViewArea = new AreaRect(SpawnCenter.Value, extend);

            for (int i = ViewArea.left; i <= ViewArea.right; ++i)
            {
                for (int j = ViewArea.bottom; j <= ViewArea.top; ++j)
                {
                    m_CoveredLoc.Add(new Vector2Int(i, j));
                    m_WorldEntity.Spawn(new ChunkInWorld(new Vector2Int(i, j), world));               
                }
            }

            SetCoverArea(ViewArea);

            return true;
        }

        public void Handle_PlayerSpawn(IEvent _event)
        {
            E_Player_Spawned Player_Spawned = _event.Cast<E_Player_Spawned>();

            ChunkInWorld SpawnCenter = new ChunkInWorld(Player_Spawned.SpawnPos, m_World);

            //figure out visiable area 
            m_CoverArea = new AreaRect(SpawnCenter.Value, (int)m_PlayerMng.PlayerView);
        }

        //Event Handler
        //--------------------------------------------------------------------------- 
        public void Handle_PlayerLeaveChunk(IEvent _event)
        {
            GSW.RestartTimer();
            //Interpret Event
            E_Player_LeaveChunk LeaveChunk = (_event as E_Player_LeaveChunk);

            AreaRect TempArea = m_CoverArea;
            TempArea.Move(LeaveChunk.Offset);

            int i, j = 0;
            ChunkInWorld SpawnLoc;

            for (i = TempArea.left; i <= TempArea.right; ++i)
            {
                for (j = TempArea.bottom; j <= TempArea.top; ++j)
                {
                    SpawnLoc = new ChunkInWorld(new Vector2Int(i, j), m_World);

                    m_WorldEntity.Spawn(SpawnLoc);

                    m_CoveredLoc.Remove(SpawnLoc.Value);
                }
            }

            //remove not covered chunk
            foreach (var uncoverd in m_CoveredLoc)
            {
                m_WorldEntity.Remove(new ChunkInWorld(uncoverd,m_World));
            }
            //update cover area
            SetCoverArea(TempArea);
        }

        public void SetCoverArea(AreaRect area)
        {
            m_CoverArea = area;
            m_CoveredLoc.Clear();

            int i, j;
            for (i = m_CoverArea.left; i <= m_CoverArea.right; ++i)
            {
                for (j = m_CoverArea.bottom; j <= m_CoverArea.top; ++j)
                {
                    m_CoveredLoc.Add(new Vector2Int(i, j));
                }
            }
        }
    }
}