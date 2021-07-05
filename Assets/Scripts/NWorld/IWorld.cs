using UnityEngine;

using Assets.Scripts.NNoise;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    public interface IWorld
    {
        void Init(string Seed);

        //Section Size
        ushort Section_Width { get; }
        ushort Section_Height { get; }
        ushort Section_Depth { get; }

        //Chunk's Max Height
        ushort Chunk_Height { get; }

        //Chunk Creator
        WorldEntity Entity { get; }

        //INoiseMaker NoiseMaker { get; }
        //IHashMaker HashMaker { get; }

        //array of block type array
        BlockPalette BlkPalette { get; }

        //block textures
        TextureSheet TexSheet { get; }

        //Chunk Biome
        //Biome GetBiome(ChunkInWorld chunkInWorld);

        Bounds GetBound(Vector3 Coord);
    }
}
