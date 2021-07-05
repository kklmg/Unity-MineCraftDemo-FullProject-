using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.NBehaviorTree
{
    //Base Composite Node
    public abstract class BevComposite : BevNodeBase
    {
        //Field
        //----------------------------------------------------------------       
        [SerializeField]
        protected List<BevNodeBase> m_listNodes = new List<BevNodeBase>(); //Child node

        //Property
        //----------------------------------------------------------------       
        protected List<BevNodeBase> Children { get { return m_listNodes; } }

        //public Function
        //-----------------------------------------------------------------
        public void AddChild(BevNodeBase child)
        {
            m_listNodes.Add(child);
        }
    }

    //Sequence Node
    [CreateAssetMenu(menuName = "Bev/Sequence")]
    class BevSequence : BevComposite
    {
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate(workData);
                //Debug.Log(res);
                if (res == eRunningState.Running || res == eRunningState.Failed) return eRunningState.Failed;
            }
           
            return eRunningState.Failed;
        }
    };

    //Selector Node
    [CreateAssetMenu(menuName = "Bev/Selector")]
    class BevSelector : BevComposite
    {
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate(workData);
                if (res == eRunningState.Running || res == eRunningState.Suceed) return res;
            }
            return eRunningState.Failed;
        }
    }


    //Parallel Node
    [CreateAssetMenu(menuName = "Bev/Parall")]
    class BevParallel : BevComposite
    {
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res = eRunningState.Ready;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate(workData);
            }
            return res;
        }
    };

    //[CreateAssetMenu(menuName = "Bev/Parall_Or")]
    //class BevParallel_Or : BevComposite
    //{
    //    protected override eRunningState Tick(BevData workData)
    //    {
    //        eRunningState res = eRunningState.Ready;
    //        foreach (var node in m_listNodes)
    //        {
    //            res = node.Evaluate(workData);
    //        }
    //        return res;
    //    }
    //};

    //Random Selector
    [CreateAssetMenu(menuName = "Bev/RandSelector")]
    class BevRandSelector : BevComposite
    {
        [SerializeField]
        private int m_nRandFigure;
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res = m_listNodes[m_nRandFigure].Evaluate(workData);
            return res;
        }
    };
}
