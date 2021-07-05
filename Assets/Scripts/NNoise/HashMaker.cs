using UnityEngine;

namespace Assets.Scripts.NNoise
{
    public interface IHashMaker
    {
        int GetHash(int key);
        int GetHash_2D(Vector2 key);
        int GetHash_2D(Vector2Int key);
        int GetHash_3D(Vector3 key);
        int GetHash_3D(Vector3Int key);
    }

    public class HashMakerBase : IHashMaker
    {
        readonly uint BIT_NOISE1;
        readonly uint BIT_NOISE2;
        readonly uint BIT_NOISE3;

        //cache
        private uint m_Cache_Uint;

        public HashMakerBase(uint noise)
        {
            BIT_NOISE1 = noise;
            BIT_NOISE2 = 0xcf87c3e6;
            BIT_NOISE3 = 0x7a39ccf3;
        }

        public HashMakerBase(string str)
        {
            BIT_NOISE1 = 7;
            BIT_NOISE2 = 0xcf87c3e6;
            BIT_NOISE3 = 0x7a39ccf3;

            foreach (var c in str)
            {
                BIT_NOISE1 = BIT_NOISE1*31 + c;
            }
        }


        public int GetHash(int key)
        {
            m_Cache_Uint = (uint)key;
            m_Cache_Uint *= BIT_NOISE1;
            m_Cache_Uint ^= (m_Cache_Uint >> 8);
            m_Cache_Uint += BIT_NOISE2;
            m_Cache_Uint ^= (m_Cache_Uint << 8);
            m_Cache_Uint *= BIT_NOISE3;
            m_Cache_Uint ^= (m_Cache_Uint >> 8);

            return (int)m_Cache_Uint;
        }
        public int GetHash_2D(Vector2 key)
        {
            return GetHash(((int)(key.x) << 8) + (int)(key.y));
        }
        public int GetHash_2D(Vector2Int key)
        {
            return GetHash((key.x << 8) + key.y);

        }
        public int GetHash_3D(Vector3 key)
        {
            return GetHash(
                    ((int)(key.x) << 16)
                      + ((int)(key.x) << 8)
                         + (int)(key.z));
        }
        public int GetHash_3D(Vector3Int key)
        {
            return GetHash((key.x << 16) + (key.x << 8) + key.z);
        }
    }

    //delegate of hash funcionn
    public delegate int del_HashFunction(int key);

}
