using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.NCache
{
    class LRUCache<TKEY,TVALUE> 
    {
        //Field
        //-----------------------------------------------------------------
        private uint m_Capacity;    //Cache Capicity

        Dictionary<TKEY, TVALUE> m_DataDic; 
        
        Dictionary<TKEY, LinkedListNode<TKEY>> m_LRUDic;
        LinkedList<TKEY> m_LRULinkList;

        //Constructor
        //-----------------------------------------------------------------
        public LRUCache(uint capacity)
        {
            m_DataDic = new Dictionary<TKEY, TVALUE>();
            m_LRUDic = new Dictionary<TKEY, LinkedListNode<TKEY>>();
            m_LRULinkList = new LinkedList<TKEY>();

            m_Capacity = capacity;
        }


        //Public Function
        //-----------------------------------------------------------------
        public bool TryGetValue(TKEY key,out TVALUE Value)
        {
            //int Value;
            if (m_DataDic.TryGetValue(key, out Value))
            {
                //update Least recent used 
                _UpdateLRUState(key);

                //get Data
                Value = m_DataDic[key];

                return true;
            }
            return false;
        }

        public bool TryRemove(TKEY key, out TVALUE Value)
        {
            if (m_DataDic.TryGetValue(key, out Value))
            {
                m_LRULinkList.Remove(m_LRUDic[key]);
                m_LRUDic.Remove(key);
                m_DataDic.Remove(key);

                return true;
            }
            else return false;
        }

        public TVALUE Pop()
        {
            TVALUE temp = m_DataDic[m_LRULinkList.Last.Value];

            //remove mapping data
            m_LRUDic.Remove(m_LRULinkList.Last.Value);

            //remove data
            m_DataDic.Remove(m_LRULinkList.Last.Value);

            //remove lru order data
            m_LRULinkList.RemoveLast();

            return temp;
        }

        //Get least rencently used data
        public TVALUE GetLRUData()
        {
            TKEY TempKey = m_LRULinkList.Last.Value;
            _UpdateLRUState(m_LRULinkList.Last.Value);
            return m_DataDic[TempKey];
        }

        //try to put data to cache. if no enough space remove LRU(least rencently used) Data
        public TVALUE Put(TKEY key, TVALUE value)
        {
            TVALUE removedData = default;

            //space if full and the key is not exist
            if (m_DataDic.Count == m_Capacity && !m_DataDic.TryGetValue(key, out TVALUE Value))
            {
                removedData = Pop();
            }
            _UpdateLRUState(key);
            m_DataDic[key] = value;

            return removedData;
        }


        public bool IsFull()
        {
            return m_DataDic.Count == m_Capacity;
        }

        //Private Function
        //-----------------------------------------------------------------

        private void _UpdateLRUState(TKEY key)
        {
            if (m_DataDic.TryGetValue(key, out TVALUE Value))
            {
                m_LRULinkList.Remove(m_LRUDic[key]);
            }
            //Move Current Value to first of lru list
            m_LRULinkList.AddFirst(key);

            //Save key Position
            m_LRUDic[key] = m_LRULinkList.First;
        }
    };
}
