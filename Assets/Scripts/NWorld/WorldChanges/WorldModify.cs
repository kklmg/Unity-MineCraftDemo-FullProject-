using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public class SaveHelper_World
    {
        [SerializeField]
        private Dictionary<Vector2Int,SaveHelper_Chunk> m_Helpers;
        private List<ChunkChange> m_Changes;

        public SaveHelper_World(WorldChange Changes)
        {
            m_Changes = Changes.Data;
            m_Helpers = new Dictionary<Vector2Int, SaveHelper_Chunk>();

            foreach (var ChunkModify in m_Changes)
            {
                m_Helpers.Add(ChunkModify.Slot, new SaveHelper_Chunk(ChunkModify));
            }
        }

        public void Modify(BlockLocation Location, byte blockId, IWorld world)
        {
            if (m_Helpers.TryGetValue(Location.ChunkInWorld.Value, out var Chunkhelper))
            {
                Chunkhelper.Modify(Location.SecInChunk, Location.BlkInSec, blockId);
            }
            else
            {
                m_Changes.Add(new ChunkChange(Location.ChunkInWorld.Value));
                Chunkhelper = new SaveHelper_Chunk(m_Changes[m_Changes.Count - 1]);

                m_Helpers.Add(Location.ChunkInWorld.Value, Chunkhelper);
                Chunkhelper.Modify(Location.SecInChunk, Location.BlkInSec, blockId);
            }

        }

        public void ApplyModification(ChunkInWorld chunkInWorld, Chunk chunk, IWorld world)
        {
            if(m_Helpers.TryGetValue(chunkInWorld.Value, out var helper))
            {
                helper.ApplyModification(chunk,world);
            }
        }

        public int Count
        {         
            get
            {
                int ct = 0;
                foreach (var v in m_Changes)
                {
                    ct += v.Count;
                }
                return ct;
            }
        }
    }
}
