using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NNoise;
using Assets.Scripts.NCache;
using Assets.Scripts.NWorld;

namespace Assets.Scripts.NGameSystem
{
    class BiomeMng : MonoBehaviour
    {
        //BiomeMng
        [SerializeField]
        private float m_NoiseFrequency = 0.1f;
        [SerializeField]
        private uint m_CacheSize = 50;
        [SerializeField]
        private Biome m_BiomeBegin;
        [SerializeField]
        private Biome m_BiomeEnd;

        private INoiseMaker m_NoiseMaker;
        private IWorld m_World;

        private LRUCache<Vector2Int, Biome> m_BiomerCache;
        private LRUCache<Vector2Int, float> m_NoiseCache;

        [SerializeField]
        private List<BiomeProportion> m_Biomes;


        private void Awake()
        {
            //Init biomes 
            foreach (var biomePropor in m_Biomes)
            {
                biomePropor.m_Biome.Init();
            }

            m_BiomeBegin.Init();
            m_BiomeEnd.Init();

            m_BiomerCache = new LRUCache<Vector2Int, Biome>(m_CacheSize);
            m_NoiseCache = new LRUCache<Vector2Int, float>(m_CacheSize);
        }

        public void Init(INoiseMaker noisemaker,IWorld world)
        {
            m_World = world;
            m_NoiseMaker = noisemaker;

            //Debug.Log(m_NoiseFrequency); 
        }


        public Biome GetBiome(Vector2Int pos)
        {
            Biome biome;

            if (m_BiomerCache.TryGetValue(pos, out biome))
            {
                Debug.Assert(biome != null);
                Debug.Assert(biome.Layer != null);
                return biome;
            }
            else
            {
                float noise = m_NoiseMaker.Make_2D(pos, m_NoiseFrequency);

                biome = noise < 0.5f ? m_BiomeBegin : m_BiomeEnd;

                m_NoiseCache.Put(pos, noise);

                foreach (var bp in m_Biomes)
                {
                    if (noise > bp.m_Min && noise < bp.m_Max)
                    {
                        biome = bp.m_Biome;
                        break;
                    }
                }
                Debug.Assert(biome != null);
                Debug.Assert(biome.Layer != null);
                m_BiomerCache.Put(pos, biome);

                return biome;
            }
        }

        
    }
}
