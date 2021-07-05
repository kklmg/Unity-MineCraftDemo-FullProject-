using System;
using System.Collections.Generic;
using System.Threading;


using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{

    struct MeshBuildResult
    {
        Section m_Section;
        SectionInWorld m_SecInWorld;
        DynamicMesh m_Mesh;
    }


    class ChunkMeshBuilder : MonoBehaviour
    {
        //---------------------------------------------------------------

        private object m_Locker;

        [SerializeField]
        private System.Threading.ThreadPriority m_BuildPriority = System.Threading.ThreadPriority.AboveNormal;

        private Thread m_TMeshBuilder;

        private EventWaitHandle m_WaitHandle;

        //Building requests
        private Queue<Chunk> m_BuildRequests = new Queue<Chunk>();

        //Task Count Listeners
        private List<Action<int>> m_TCountListeners = new List<Action<int>>();

        private bool IsThreadInvoked = false;

        //Cache
        private Chunk m_ChunkCache;

        //---------------------------------------------------------------

        public int RemainedTask{ get { return m_BuildRequests.Count; } }

        private void Awake()
        {
            //make building Thread  
            m_TMeshBuilder = new Thread(new ThreadStart(Thread_BuildMesh));
            m_TMeshBuilder.Priority = m_BuildPriority;
            m_Locker = new object();

            m_WaitHandle = new EventWaitHandle(true,EventResetMode.ManualReset);
            m_WaitHandle.Set();
        }

        public void AddListener(Action<int> action)
        {
            m_TCountListeners.Add(action);
        }

        public void NotifyListeners()
        {
            foreach (var listener in m_TCountListeners)
            {
                listener(m_BuildRequests.Count);
            }
        }

        public void StartBuildingThread()
        {
            if (m_TMeshBuilder.ThreadState == ThreadState.Unstarted)
            {
                m_TMeshBuilder.Start();
            }
        }


        public void InvokeThread()
        {
            if (IsThreadInvoked == false)
            {
                m_WaitHandle.Set();
                IsThreadInvoked = true;
            }
        }

        public void PauseThread()
        {
            if (IsThreadInvoked)
            {
                if (m_BuildRequests.Count == 0)
                {
                    m_WaitHandle.Reset();
                    IsThreadInvoked = false;
                }
            }
        }

        public void AddRequest(Chunk _chunk)
        {           
            lock (m_Locker)
            {
                m_BuildRequests.Enqueue(_chunk);
                InvokeThread();
            }
        }

        void Thread_BuildMesh()
        {
            while (true)
            {
                m_WaitHandle.WaitOne();

                if (m_BuildRequests.Count != 0)
                {
                    lock (m_Locker)
                    {
                        m_ChunkCache = m_BuildRequests.Dequeue();
                    }
                    m_ChunkCache.BuildMesh();

                    NotifyListeners();
                }
                else
                {
                    lock (m_Locker)
                    {
                        PauseThread();
                    }
                }
            }
        }


        private void OnApplicationQuit()
        {
            m_TMeshBuilder.Abort();
        }

        private void OnDestroy()
        {
            m_TMeshBuilder.Abort();
            Debug.Log("Mesh Building Thread has aborted");
        }
    }
}
