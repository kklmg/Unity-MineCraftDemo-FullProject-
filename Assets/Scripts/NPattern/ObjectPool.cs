using System;

using UnityEngine;

namespace Assets.Scripts.NPattern
{
    public class Pool<T> where T : new()
    {
        [SerializeField]
        protected T[] m_Pool;

        private readonly Action<T> m_DelOnSpawn;
        private readonly Action<T> m_DelOnDeSpawn;

        protected readonly int m_nPoolSize;

        protected Pool(int size,Action<T> spawn, Action<T> despawn)
        {
            m_DelOnSpawn = spawn;
            m_DelOnDeSpawn = despawn;
            m_Pool = new T[size];
        }

        private T Spawn()
        {
            int Length = m_Pool.Length;

            for (int i = 0; i < Length; ++i)
            {
                if (m_Pool[i] == null)
                {
                    m_Pool[i] = new T();

                    m_DelOnSpawn(m_Pool[i]);
                    return m_Pool[i];
                }
            }
            return default(T);
        }



        private void DeSpawn()
        { 
        }
        
    }

}
