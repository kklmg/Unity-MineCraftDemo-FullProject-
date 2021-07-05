using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NWorld;

namespace Assets.Scripts.NGlobal.WorldSearcher
{
    static class GWorldSearcher
    {
        //Search Chunk 
        //-----------------------------------------------------------------------
        static public Chunk GetChunk(Vector3 Coord, IWorld _World)
        {
            return _World.Entity.GetChunk(new ChunkInWorld(Coord, _World));
        }
        static public Chunk GetChunk(ChunkInWorld chunkinworld, IWorld _World)
        {
            return _World.Entity.GetChunk(chunkinworld);
        }

        //Search Section 
        //-----------------------------------------------------------------------
        static public Section GetSection(Vector3 Coord, IWorld _World)
        {
            //Try get chunk
            Chunk _chunk = GetChunk(Coord, _World);
            if (_chunk == null) return null;

            //Get Section
            return _chunk.GetSection(new SectionInChunk(Coord, _World));
        }
        static public Section GetSection(SectionInWorld Slot, IWorld _World)
        {
            //Try get chunk
            Chunk _chunk = GetChunk(Slot.ToChunkInWorld(_World), _World);
            if (_chunk == null) return null;

            //get Section
            return _chunk.GetSection(Slot.ToSectionInChunk());
        }

        //Search Block 
        //-----------------------------------------------------------------------
        static public IBlock GetBlock(Vector3 Coord, IWorld _World)
        {
            //Try get section
            Section section = GetSection(Coord, _World);
            if (section == null) return null;

            BlockInSection BlockSlot = new BlockInSection(Coord, _World);
            return section.Voxel.GetBlock(BlockSlot);
        }

        static public int GetGroundHeight(Vector3 Coord, IWorld _World)
        {
            Chunk chk = GetChunk(Coord, _World);
            if (chk == null) return 0;

            BlockInSection BlkInSec = new BlockInSection(Coord, _World);

            var hmap = chk.HeightMap;

            return hmap[BlkInSec.x, BlkInSec.z].Height;
        }
    }
}
