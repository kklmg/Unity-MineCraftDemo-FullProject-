using UnityEngine;

using Assets.Scripts.NWorld;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public struct ChunkInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public ChunkInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector2Int(
                Coord.x >= 0 ?
                    (int)(Coord.x / _world.Section_Width)
                    : (int)(Coord.x / _world.Section_Width) - 1,
                Coord.z >= 0 ?
                    (int)(Coord.z / _world.Section_Depth)
                    : (int)(Coord.z / _world.Section_Depth) - 1);
        }
        public ChunkInWorld(Vector2Int slot, IWorld _world)
        {
            m_Value = slot;
        }

        //Convert functions
        //-------------------------------------------------------------------------
        public Vector2 ToCoord2D(IWorld _world)
        {
            return new Vector2(
             m_Value.x * _world.Section_Width,
             m_Value.y * _world.Section_Depth);
        }
        public Vector3 ToCoord3D(IWorld _world)
        {
            return new Vector3(
                m_Value.x * _world.Section_Width,
                0,
                m_Value.y * _world.Section_Depth);
        }
        public Vector2Int ToCoord2DInt(IWorld _world)
        {
            return new Vector2Int(
             m_Value.x * _world.Section_Width,
             m_Value.y * _world.Section_Depth);
        }
        public Vector3Int ToCoord3DInt(IWorld _world)
        {
            return new Vector3Int(
                m_Value.x * _world.Section_Width,
                0,
                m_Value.y * _world.Section_Depth);
        }

        //Property 
        //----------------------------------------------------------------------------------
       
        public Vector2Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        private Vector2Int m_Value;
    }

    [System.Serializable]
    public struct SectionInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public SectionInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
                Coord.x >= 0 ?
                    (int)(Coord.x / _world.Section_Width)
                    : (int)(Coord.x / _world.Section_Width) - 1,
                Coord.y >= 0 ?
                    (int)(Coord.y / _world.Section_Height)
                    : (int)(Coord.y / _world.Section_Height) - 1,
                Coord.z >= 0 ?
                    (int)(Coord.z / _world.Section_Depth)
                    : (int)(Coord.z / _world.Section_Depth) - 1);
        }
        public SectionInWorld(int x, int y, int z)
        {
            m_Value = new Vector3Int(x, y, z);
        }
        public SectionInWorld(Vector3Int SecSlot)
        {
            m_Value = SecSlot;
        }

        //Convert functions
        //-------------------------------------------------------------------------
        public Vector3 ToCoord(IWorld _world)
        {
            return new Vector3(
               (float)m_Value.x * _world.Section_Width,
               (float)m_Value.y * _world.Section_Height,
               (float)m_Value.z * _world.Section_Depth);

        }
        public ChunkInWorld ToChunkInWorld(IWorld _world)
        {
            return new ChunkInWorld(new Vector2Int(m_Value.x,m_Value.z), _world);
        }
        public SectionInChunk ToSectionInChunk()
        {
            return new SectionInChunk(m_Value.y);
        }

        //Translate
        public Vector3Int Move(Vector3Int offset)
        {
            m_Value += offset;
            return m_Value;
        }
        public SectionInWorld Offset(Vector3Int offset)
        {
            return new SectionInWorld(m_Value + offset);
        }


        //Property
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }

        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        public Vector3Int m_Value;
    }

    [System.Serializable]
    public struct SectionInChunk
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public SectionInChunk(Vector3 Coord, IWorld _world)
        {
            m_Value = (int)(Coord.y / _world.Section_Height);
        }
        public SectionInChunk(int Slot)
        {
            m_Value = Slot;
        }
        public bool IsValid(IWorld _world)
        {
            if (m_Value > _world.Chunk_Height) return false;
            else return true;
        }

        //Property
        //----------------------------------------------------------------------------------
        public int Value { set { m_Value = value; } get { return m_Value; } }

        //Field
        [SerializeField]
        private int m_Value;
    }

    [System.Serializable]
    public struct BlockInSection
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public BlockInSection(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
            Coord.x >= 0 ?
                (int)(Coord.x % _world.Section_Width)
                : (int)(Coord.x % _world.Section_Width) + _world.Section_Width - 1,
            Coord.y >= 0 ?
                (int)(Coord.y % _world.Section_Height)
                : (int)(Coord.y % _world.Section_Height) + _world.Section_Height - 1,
            Coord.z >= 0 ?
                (int)(Coord.z % _world.Section_Depth)
                : (int)(Coord.z % _world.Section_Depth) + _world.Section_Depth - 1
            );
        }
        public BlockInSection(Vector3Int slot, IWorld _world)
        {
            m_Value = new Vector3Int(
            (slot.x >= 0 ?
                slot.x % _world.Section_Width
                : slot.x % _world.Section_Width + _world.Section_Width - 1),
            (slot.y >= 0 ?
                slot.y % _world.Section_Height
                : slot.y % _world.Section_Height + _world.Section_Height - 1),
            (slot.z >= 0 ?
                slot.z % _world.Section_Depth
                : slot.z % _world.Section_Depth + _world.Section_Depth - 1)
            );
        }
        public BlockInSection(int x, int y, int z, IWorld _world)
        {
            m_Value = new Vector3Int(
            (x >= 0 ?
                x % _world.Section_Width
                : x % _world.Section_Width + _world.Section_Width - 1),
            (y >= 0 ?
                y % _world.Section_Height
                : y % _world.Section_Height + _world.Section_Height - 1),
            (z >= 0 ?
                z % _world.Section_Depth
                : z % _world.Section_Depth + _world.Section_Depth - 1)
            );
        }


        //Public Functions
        public bool IsInSectionBorder(IWorld _world)
        {
            if (Value.x == _world.Section_Width - 1 || Value.x == 0) return true;
            if (Value.y == _world.Section_Width - 1 || Value.y == 0) return true;
            if (Value.z == _world.Section_Width - 1 || Value.z == 0) return true;
            return false;
        }
        public BlockInSection Offset(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            Vector3Int dest = m_Value + blkOffset;

            _MapToValidRange(ref dest,_world,out SecOffset);


            return new BlockInSection(dest, _world);
        }
        public void Move(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            m_Value += blkOffset;
            _MapToValidRange(ref m_Value, _world, out SecOffset);
        }

        //Private Function
        private void _MapToValidRange(ref Vector3Int _value, IWorld _world, out Vector3Int SecOffset)
        {
            SecOffset = Vector3Int.zero;

            if (_value.x >= _world.Section_Width)
            {
                SecOffset.x = _value.x / _world.Section_Width;
                _value.x %= _world.Section_Width;
            }
            if (_value.y >= _world.Section_Height)
            {
                SecOffset.y = _value.y / _world.Section_Height;
                _value.y %= _world.Section_Height;
            }
            if (_value.z >= _world.Section_Depth)
            {
                SecOffset.z = _value.z / _world.Section_Depth;
                _value.z %= _world.Section_Depth;
            }

            if (_value.x < 0)
            {
                SecOffset.x = _value.x / _world.Section_Width - 1;
                _value.x = _value.x % _world.Section_Width + _world.Section_Width ;
            }
            if (_value.y < 0)
            {
                SecOffset.y = _value.y / _world.Section_Height - 1;
                _value.y = _value.y % _world.Section_Height + _world.Section_Height;
            }
            if (_value.z < 0)
            {
                SecOffset.z = _value.z / _world.Section_Depth - 1;
                _value.z = _value.z % _world.Section_Depth + _world.Section_Depth;
            }
        }

        //Property Value
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }

        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        private Vector3Int m_Value;
    }

    [System.Serializable]
    public struct BlockInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public BlockInWorld(Vector3 Coord)
        {
            m_Value = new Vector3Int(
                (Coord.x >= 0 ? (int)Coord.x : (int)Coord.x - 1),
                (Coord.y >= 0 ? (int)Coord.y : (int)Coord.y - 1),
                (Coord.z >= 0 ? (int)Coord.z : (int)Coord.z - 1));

            m_Bound = new Bounds();
            m_Bound.SetMinMax(m_Value, m_Value + Vector3Int.one);
        }

        public BlockInWorld(SectionInWorld SecInWorld, BlockInSection blockInSec, IWorld _World)
        {
            m_Value = new Vector3Int(
                SecInWorld.x * _World.Section_Width + blockInSec.x,
                SecInWorld.y * _World.Section_Height + blockInSec.y,
                SecInWorld.z * _World.Section_Depth + blockInSec.z);
            m_Bound = new Bounds();
            m_Bound.SetMinMax(m_Value, m_Value + Vector3Int.one);
        }

        //Convert Function
        public SectionInWorld ToSectionInWorld(IWorld _World)
        {
            return new SectionInWorld(
                m_Value.x / _World.Section_Width,
                m_Value.y / _World.Section_Height,
                m_Value.z / _World.Section_Depth);
        }
        public BlockInSection ToBlockInsection(IWorld _World)
        {
            return new BlockInSection(
                new Vector3Int(
                m_Value.x / _World.Section_Width,
                m_Value.y / _World.Section_Height,
                m_Value.z / _World.Section_Depth),
                _World);
        }

        //Properties
        public Bounds Bound { get { return m_Bound; } }
        public Vector3Int Value { get { return m_Value; } }

        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        private Vector3Int m_Value;
        [SerializeField]
        private Bounds m_Bound;
    }

    [System.Serializable]
    public struct BlockLocation
    {
        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        private ChunkInWorld m_ChunkInWorld; 
        [SerializeField]
        private SectionInWorld m_SecInWorld;
        [SerializeField]
        private BlockInSection m_BlkInSection;
        [SerializeField]
        private BlockInWorld m_BlkInWorld;

        [SerializeField]
        private Chunk m_Chunk;      //The Chunk Where block located in
        [SerializeField]
        private Section m_Section;  //The Section Where block located in
        [SerializeField]
        private IBlock m_Block;      //blockType

        //properties
        //----------------------------------------------------------------------------------
        public ChunkInWorld ChunkInWorld {  get { return m_ChunkInWorld; } }
        public SectionInWorld SecInWorld {  get { return m_SecInWorld; } }
        public SectionInChunk SecInChunk { get { return m_SecInWorld.ToSectionInChunk(); } }
        public BlockInSection BlkInSec { get { return m_BlkInSection; } }
        public BlockInWorld BlkInWorld { get { return m_BlkInWorld; } }
        public Bounds Bound { get { return m_BlkInWorld.Bound; } }

        public Chunk CurChunk { get { return m_Chunk; } }
        public Section CurSection { get { return m_Section; } }
        public IBlock CurBlock
        {
            get
            {
                if (m_Section == null) return null;
                else return m_Section.Voxel.GetBlock(m_BlkInSection);
            }
        }
        public byte CurBlockID
        {
            get
            {
                if (m_Section == null) return 0;
                return m_Section.Voxel.GetBlockID(m_BlkInSection);
            }
            set
            {
                if (m_Section == null)
                {
                    m_Section = CurChunk.CreateBlankSection(SecInChunk);
                }

                if (m_Section == null)
                {
                    Debug.LogError("InValid Section");
                }
                else
                {
                    m_Section.Voxel.SetBlock(m_BlkInSection, value);
                }
            }
        }
        


        //Constructor
        //----------------------------------------------------------------------------------
        public BlockLocation(Vector3 Coord, IWorld _World)
        {
            //Get Chunk Location at World
            m_ChunkInWorld = new ChunkInWorld(Coord, _World);
            //Get Section Location at World
            m_SecInWorld = new SectionInWorld(Coord, _World);
            //Get Block Location In Section
            m_BlkInSection = new BlockInSection(Coord, _World);
            //Get Block Location at World
            m_BlkInWorld = new BlockInWorld(Coord);

            //Get Chunk
            m_Chunk = _World.Entity.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; return; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; ; return; }

            //Get Block
            m_Block = m_Section.Voxel.GetBlock(m_BlkInSection);
        }
        public BlockLocation(SectionInWorld SecInWorld,BlockInSection BlkInSec, IWorld _World)
        {
            //Get Section Location
            m_SecInWorld = SecInWorld;
            //Get Block Location
            m_BlkInSection = BlkInSec;
            //Get Chunk Location
            m_ChunkInWorld = m_SecInWorld.ToChunkInWorld(_World);
            //Get Block Location In World
            m_BlkInWorld = new BlockInWorld(m_SecInWorld,m_BlkInSection,_World);

            //Get Chunk
            m_Chunk = _World.Entity.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; return; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; ; return; }

            //Get Block
            m_Block = m_Section.Voxel.GetBlock(m_BlkInSection);
        }


        public void Update(Vector3 Coord, IWorld _World)
        {
            //Get Chunk Location
            m_ChunkInWorld = new ChunkInWorld(Coord, _World);
            //Get Section Location
            m_SecInWorld = new SectionInWorld(Coord, _World);
            //Get Block Location
            m_BlkInSection = new BlockInSection(Coord, _World);
            //Get Block Location In World
            m_BlkInWorld = new BlockInWorld(m_SecInWorld, m_BlkInSection, _World);

            //Get Chunk
            m_Chunk = _World.Entity.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; return; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; ; return; }

            //Get Block
            m_Block = m_Section.Voxel.GetBlock(m_BlkInSection);
        }

        public BlockLocation Offset_Blk(Vector3Int offset,IWorld _world)
        {        
            //get offset block
            BlockInSection BlkInSec = m_BlkInSection.Offset(offset, _world, out Vector3Int SecOffset);
            SectionInWorld SecInWorld = m_SecInWorld.Offset(SecOffset);

            return new BlockLocation(SecInWorld,BlkInSec,_world);
        }

        public void Reset()
        {
            m_Chunk = null;
            m_Section = null;
            m_Block = null;
        }

        public bool IsPutable(IWorld world)
        {
            if (m_Chunk == null) return false;
            if (m_Block != null) return false;
            else return SecInChunk.IsValid(world);
        }

        public bool IsChunkExists()
        {
            return m_Chunk != null;
        }
        public bool IsSectionExists()
        {
            return m_Section != null;
        }

        public bool IsBlockExists()
        {
            return m_Block != null;
        }
        public bool HasObstacle()
        {
            return m_Block != null && m_Block.IsObstacle == true;
        }

    }
}
