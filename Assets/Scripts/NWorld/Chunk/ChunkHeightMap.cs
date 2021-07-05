using System;

using UnityEngine;

using Assets.Scripts.NNoise;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public struct HeightUnit
    {
        public LayerData Layer;
        public int Height;

        public HeightUnit(int height,LayerData layer)
        {
            Height = height;
            Layer = layer;
        }
    }


    [Serializable]
    public class ChunkHeightMap
    {
        HeightUnit[,] m_MapData;       //Height map data

        public Vector2Int ChunkInWorld { private set; get; }
        public Vector2Int PivotLB { private set; get; }
        public int Width { private set; get; }
        public int Depth { private set; get; }
        public int MinAltitude { private set; get; }
        public int MaxAltitude { private set; get; }

        //Indexer
        public HeightUnit this[int i, int j]
        {
            get
            {
                return m_MapData[i, j];
            }
            set
            {
                m_MapData[i, j] = value;
                UpdateMaxMinAltitude(m_MapData[i, j].Height);
            }
        }

        public void SetHeight(int i, int j, int height)
        {
            m_MapData[i, j].Height = height;
            UpdateMaxMinAltitude(height);
        }

        public void SetLayer(int i, int j, LayerData Layer)
        {
            m_MapData[i, j].Layer = Layer;
        }

        //Constructor
        public ChunkHeightMap(int _Width, int _Depth, Vector2Int _ChunkInWorld)
        {
            Width = _Width;
            Depth = _Depth;
            ChunkInWorld = _ChunkInWorld;
            PivotLB = new Vector2Int(_ChunkInWorld.x * _Width, _ChunkInWorld.y * _Depth);

            m_MapData = new HeightUnit[Width, Depth];
        }

        public ChunkHeightMap(int _Width, int _Depth)
        {
            Width = _Width;
            Depth = _Depth;

            m_MapData = new HeightUnit[Width, Depth];
        }

        public ChunkHeightMap(ChunkHeightMap Other)
        {
            Width = Other.Width;
            Depth = Other.Depth;
            ChunkInWorld = Other.ChunkInWorld;
            PivotLB = new Vector2Int(ChunkInWorld.x * Width, ChunkInWorld.y * Depth);

            m_MapData = new HeightUnit[Width, Depth];


            //Save Max and Min Altitude of This Chunk
            MaxAltitude = Other.MaxAltitude;
            MinAltitude = Other.MinAltitude;

            int i, j;
            for (i = 0; i < Width; ++i)
            {
                for (j = 0; j < Depth; ++j)
                {
                    m_MapData[i, j] = Other[i, j];
                }
            }
        }

        private void UpdateMaxMinAltitude(int altitude)
        {
            //Save Max and Min Altitude of This Chunk
            MaxAltitude = Math.Max(MaxAltitude, altitude);
            MinAltitude = Math.Min(MinAltitude, altitude);
        }

        public void GenerateNormal(Biome _Biome, INoiseMaker _NoiseMaker, int octave)
        {
            int i, j;
            for (i = 0; i < Width; ++i)
            {
                for (j = 0; j < Depth; ++j)
                {
                    //Calculate Height 
                    m_MapData[i, j].Height = _Biome.GetHeight(new Vector2Int(i + PivotLB.x, j + PivotLB.y), _NoiseMaker, octave);    //Height

                    //Assign layer Data
                    m_MapData[i, j].Layer = _Biome.Layer;

                    Debug.Assert(m_MapData[i, j].Layer != null);

                    //Save Max and Min Altitude of This Chunk
                    MaxAltitude = Math.Max(MaxAltitude, m_MapData[i, j].Height);
                    MinAltitude = Math.Min(MinAltitude, m_MapData[i, j].Height);
                }
            }
        }

    }
}
