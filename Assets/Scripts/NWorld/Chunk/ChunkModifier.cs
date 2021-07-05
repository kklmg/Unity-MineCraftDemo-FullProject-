using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NEvent;
using Assets.Scripts.NCommand;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(WorldEntity))]
    class ChunkModifier : MonoBehaviour
    {
        private IWorld m_refWorld;
        private WorldEntity m_WorldEntity;
        private SaveMng m_SaveMng;


        private Dictionary<Vector2Int, Chunk> m_Modifications;

        [SerializeField]
        private LinkedList<Com_ModifyBlock> m_BlockModified = new LinkedList<Com_ModifyBlock>();


        [SerializeField]
        [Range(5, 50)]
        private int m_ModifyBackup = 20; 


        private void Awake()
        {          
            m_WorldEntity = GetComponent<WorldEntity>();
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
        }

        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();

            Locator<IEventHelper>.GetService().Subscribe(E_Block_Modify.ID, Handle_BlockModify, enPriority.Highest);
            Locator<IEventHelper>.GetService().Subscribe(E_Block_Recover.ID, Handle_BlockRecover, enPriority.Highest);
        }

        public void Handle_BlockModify(IEvent _event)
        {
            E_Block_Modify temp = (_event as E_Block_Modify);

            //Execute modify request
            temp.Request.Execute();

            //Save Changes
            m_SaveMng.SaveBlock(temp.Request.Location, temp.Request.ModifyID);

            //save modify command
            m_BlockModified.AddLast(temp.Request);


            while (m_BlockModified.Count >= m_ModifyBackup)
            {
                m_BlockModified.RemoveFirst();
            }
        }

        public void Handle_BlockRecover(IEvent _event)
        {
            if (m_BlockModified.Count > 0)
            {
                //Undo
                m_BlockModified.Last.Value.Undo();

                //Save Changes
                m_SaveMng.SaveBlock(m_BlockModified.Last.Value.Location, m_BlockModified.Last.Value.PreID);

                m_BlockModified.RemoveLast();

            }
        }
    }

}

