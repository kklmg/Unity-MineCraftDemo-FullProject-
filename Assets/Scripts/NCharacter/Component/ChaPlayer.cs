using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]

    class ChaPlayer : MonoBehaviour
    {
        //Field
        //-------------------------------------------------------------------------------      
        private IWorld m_World;
        private Communicator m_Communicator;
        private SaveMng m_SaveMng;
        private Character m_Character;

        //Player Location
        private BlockLocation m_CurLoc;
        private Vector2Int m_PreChunkLoc;
        private Vector3Int m_PreSecLoc;

        //caches
        private E_Player_LeaveChunk m_Cache_EChunkChange;

        private void Awake()
        {               
            m_Communicator = GetComponent<Communicator>();
            m_Character = GetComponent<Character>();
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;        
        }
        private void Start()
        {
            m_World = Locator<IWorld>.GetService();

            //Compute Current Location
            m_CurLoc = new BlockLocation(transform.position, m_World);
            m_PreChunkLoc = m_CurLoc.ChunkInWorld.Value;
            m_PreSecLoc = m_CurLoc.SecInWorld.Value;

            //make event cache
            m_Cache_EChunkChange = new E_Player_LeaveChunk(Vector2Int.zero);
        }
     
        private void Update()
        {
            //Save Location Data in Previous Frame 
            m_PreChunkLoc = m_CurLoc.ChunkInWorld.Value;
            m_PreSecLoc = m_CurLoc.SecInWorld.Value;

            //save position to file
            m_SaveMng.SavePlayerLocation(transform);

            //Update Location 
            m_CurLoc.Update(transform.position, m_World);

            if (m_PreChunkLoc != m_CurLoc.ChunkInWorld.Value)
            {
                //Set Event
                m_Cache_EChunkChange.Offset = m_CurLoc.ChunkInWorld.Value - m_PreChunkLoc;

                //Publish Event: player's Chunk location has Changed
                Locator<IEventHelper>.GetService().Publish(m_Cache_EChunkChange);
            }
        }

        public bool IntersectWithBound(Bounds target)
        {
            return m_Character.GlobalBodyBound.Intersects(target);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0 + 10, 0 + 10, 160, 20), "Location: " + m_CurLoc.BlkInWorld.Value.ToString());
            GUI.Label(new Rect(0 + 10, 0 + 30, 160, 20), "Sec Location: " + m_CurLoc.SecInWorld.Value.ToString());
            GUI.Label(new Rect(0 + 10, 0 + 50, 160, 20), "Blk In Section: " + m_CurLoc.BlkInSec.Value.ToString());
        }
    }

}
