using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public struct BlockChange
    {
        public BlockChange(Vector3Int _slot, byte _blocktype)
        {
            Slot = _slot;
            Value = _blocktype;
        }

        [SerializeField]
        public Vector3Int Slot;
        [SerializeField]
        public byte Value;
    }

    [System.Serializable]
    public struct SectionChange 
    {
        public SectionChange(int _height)
        {
            Height = _height;
            Data = new List<BlockChange>();
        }

        public SectionChange(int _height, List<BlockChange> _Changes)
        {
            Height = _height;
            Data = _Changes;
        }
        public int Count
        {
            get
            {
                return Data.Count;
            }
        }

        [SerializeField]
        public int Height;

        [SerializeField]
        public List<BlockChange> Data;
    }

    [System.Serializable]
    public class ChunkChange
    {
        public ChunkChange(Vector2Int _Slot)
        {
            Slot = _Slot;
            Data = new List<SectionChange>();
        }

        public ChunkChange(Vector2Int _Slot, List<SectionChange> _Changes)
        {
            Slot = _Slot;
            Data = _Changes;
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (var v in Data)
                {
                    count += v.Count;
                }
                return count;
            }
        }

        [SerializeField]
        public Vector2Int Slot;
        [SerializeField]
        public List<SectionChange> Data;
    }

    [System.Serializable]
    public class WorldChange
    {
        public List<ChunkChange> m_Data;

        [SerializeField]
        public List<ChunkChange> Data { get { return m_Data; } }

        public WorldChange()
        {
            m_Data = new List<ChunkChange>();
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (var v in Data)
                {
                    count += v.Count;
                }
                return count;
            }
        }
    }
}
