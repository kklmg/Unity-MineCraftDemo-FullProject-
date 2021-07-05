using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Scripts.NNoise;
using Assets.Scripts.NCache;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGameSystem;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(BiomeMng))]
    class MapMaker : MonoBehaviour
    {
        [SerializeField]
        private int m_Octave = 4;

        [SerializeField]
        private int m_Border = 6;

        [SerializeField]
        private float m_BorderNoise = 0.1f;

        private INoiseMaker m_NoiseMaker;
        private IWorld m_World;

        private LRUCache<Vector2Int, ChunkHeightMap> m_MapCache;
        private LRUCache<Vector2Int, float> m_NoiseCache;

        private BiomeMng m_BiomeMng;

        private void Awake()
        {
            m_BiomeMng = GetComponent<BiomeMng>();
            m_MapCache = new LRUCache<Vector2Int, ChunkHeightMap>(50);
        }

        public void Init()
        {
            m_NoiseMaker = Locator<INoiseMaker>.GetService();
            m_World = Locator<IWorld>.GetService();
        }

        public ChunkHeightMap GetHeightMap(Vector2Int ChunkInWorld)
        {
            if (m_MapCache.TryGetValue(ChunkInWorld, out var _map))
            {
                return _map;
            }
            else
            {
                _map = new ChunkHeightMap(m_World.Section_Width, m_World.Section_Depth, ChunkInWorld);
                m_MapCache.Put(ChunkInWorld, _map);

                Biome biome = m_BiomeMng.GetBiome(ChunkInWorld);

                _map.GenerateNormal(biome, m_NoiseMaker, m_Octave);

                return _map;
            }
        }

        public ChunkHeightMap GetBlendedHeightMap(Vector2Int ChunkInWorld)
        {
            Biome BiomeNow = m_BiomeMng.GetBiome(ChunkInWorld);
            Biome BiomeLeft = m_BiomeMng.GetBiome(ChunkInWorld + Vector2Int.left);           
            Biome BiomeBottom = m_BiomeMng.GetBiome(ChunkInWorld + Vector2Int.down);
            Biome BiomeLeftBottom = m_BiomeMng.GetBiome(ChunkInWorld + Vector2Int.down + Vector2Int.left);

            //all biome are the same
            if (BiomeNow == BiomeLeft && BiomeNow == BiomeBottom && BiomeNow == BiomeLeftBottom)
            {
                return GetHeightMap(ChunkInWorld);
            }

            else
            {
                //Get a cloned Height Map;
                ChunkHeightMap mapBlended = new ChunkHeightMap(GetHeightMap(ChunkInWorld));

                //blend with left heightmap
                if (BiomeNow != BiomeLeft)
                {
                    ChunkHeightMap mapLeft = GetHeightMap(ChunkInWorld + Vector2Int.left);
                    Smooth_X(mapBlended, mapLeft, mapBlended, m_Border);
                }

                //blend with bottom heightmap
                if (BiomeNow != BiomeBottom)
                {
                    ChunkHeightMap mapBottom = GetHeightMap(ChunkInWorld + Vector2Int.down);
                    Smooth_Z(mapBlended, mapBottom, mapBlended, m_Border);
                }


                //blend with Left bottom heightmap
                if (BiomeNow == BiomeLeft && BiomeBottom == BiomeLeftBottom)
                {
                    return mapBlended;
                }

                else if (BiomeNow == BiomeBottom && BiomeLeft == BiomeLeftBottom)
                {
                    return mapBlended;
                }
                else
                {
                    ChunkHeightMap mapLeftBottom = GetHeightMap(ChunkInWorld + Vector2Int.left + Vector2Int.down);

                    Smooth_Corner(mapBlended, mapLeftBottom, mapBlended, m_Border);
                }

                return mapBlended;
            }
        }

        void Smooth_X(ChunkHeightMap target, ChunkHeightMap first, ChunkHeightMap second, int border)
        {
            Debug.Assert(border < m_World.Section_Width);

            int x, y, z;
            float noise;

            for (x = 0; x < border; ++x)
            {
                for (z = 0; z < m_World.Section_Depth; ++z)
                {
                    //compute(lerp) height
                    y = (int)Mathf.Lerp(first[x, z].Height, second[x, z].Height, (x + 1) / (float)border);

                    //compute border
                    noise = m_NoiseMaker.Make_2D(second.PivotLB + new Vector2(x, z), m_BorderNoise);

                    target.SetHeight(x, z, y);
                    target.SetLayer(x, z, (noise > 0.5f ? first[x, z].Layer : second[x, z].Layer));
                }
            }
        }

        void Smooth_Z(ChunkHeightMap target, ChunkHeightMap first, ChunkHeightMap second, int border)
        {
            Debug.Assert(border < m_World.Section_Depth);

            int x, y, z;
            float noise;

            for (x = 0; x < m_World.Section_Width; ++x)
            {
                for (z = 0; z < border; ++z)
                {
                    //compute height
                    y = (int)Mathf.Lerp(first[x, z].Height, second[x, z].Height, (z + 1) / (float)(border));

                    //compute border
                    noise = m_NoiseMaker.Make_2D(second.PivotLB + new Vector2(x, z), m_BorderNoise);

                    target.SetHeight(x, z, y);
                    target.SetLayer(x, z, (noise > 0.5f ? first[x, z].Layer : second[x, z].Layer));
                }
            }
        }

        void Smooth_Corner(ChunkHeightMap target,ChunkHeightMap MapLB, ChunkHeightMap MapCur, int border)
        {
            Debug.Assert(border < m_World.Section_Width);
            Debug.Assert(border < m_World.Section_Depth);

            int x, z;
            float y;

            float tx, tz;        
            float noise;

            for (x = 0; x < border; ++x)
            {
                for (z = 0; z < border; ++z)
                {
                    tx = (x+1) / (float)(border);
                    tz = (z+1) / (float)(border);

                    y = Mathf.Lerp(MapLB[x, z].Height, MapCur[x, z].Height, tx);
                    y = Mathf.Lerp(y, MapCur[x, z].Height, tz);

                    //compute border
                    noise = m_NoiseMaker.Make_2D(target.PivotLB + new Vector2(x, z), m_BorderNoise);

                    target.SetHeight(x, z, (int)y);
                    target.SetLayer(x, z, (noise > 0.5f ? MapCur[x, z].Layer : MapLB[x, z].Layer));

                }
            }
        }
    }
}