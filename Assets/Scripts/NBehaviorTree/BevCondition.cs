using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NCondition;


namespace Assets.Scripts.NBehaviorTree
{
    //Base Condition Node
    public abstract class BevConditionBase : BevNodeBase
    { //Field
        //------------------------------------------------------------
        [SerializeField]
        private bool m_bTrue;

        //Override Function
        //-------------------------------------------------------------
        protected override eRunningState Tick(BevData workData)
        {
            m_bTrue = Check(workData);
            if (m_bTrue)
                return eRunningState.Suceed;
            else
                return eRunningState.Failed;
        }

        //Abstact Function
        public abstract bool Check(BevData workData);
       
    };
    public abstract class BevConComposite: BevConditionBase
    {
        //Field
        //----------------------------------------------------------------       
        [SerializeField]
        protected List<BevConditionBase> m_listNodes = new List<BevConditionBase>(); //Child node

        //Property
        //----------------------------------------------------------------       
        protected List<BevConditionBase> Children { get { return m_listNodes; } }

        //public Function
        //-----------------------------------------------------------------
        public void AddChild(BevConditionBase child)
        {
            m_listNodes.Add(child);
        }
    }
    public class BevCondition_And : BevConComposite
    {
        public override bool Check(BevData workData)
        {
            bool res = true;
            foreach (var node in m_listNodes)
            {
                res &= node.Check(workData);            
            }
            return res;
        }
    }
    public class BevCondition_Or : BevConComposite
    {
        public override bool Check(BevData workData)
        {
            bool res = false;
            foreach (var node in m_listNodes)
            {
                res |= node.Check(workData);
            }
            return res;
        }
    }

    //Condition Node construct use Interface
    public class BevCondition_Itf : BevNodeBase
    {
        //Field
        //------------------------------------------------------------
        [SerializeField]
        private ConditionBase m_Con;

        [SerializeField]
        private bool isTrue;

        //Constructor
        //-------------------------------------------------------------
        public BevCondition_Itf(ConditionBase conbase)
        {
            m_Con = conbase;
        }


        //Override Function
        //-------------------------------------------------------------
        protected override eRunningState Tick(BevData workData)
        {
            isTrue = m_Con.Check();
            if (isTrue)
                return eRunningState.Suceed;
            else
                return eRunningState.Failed;
        }
    };
    //Condition Node construct use delegate
    [CreateAssetMenu(menuName = "Bev/Con_delegate")]
    public class BevCondition_Del : BevNodeBase
    {
        //Field
        //------------------------------------------------------------
        [SerializeField]
        private del_Condition m_delCondition;

        [SerializeField]
        private bool isTrue;

        //Constructor
        //-------------------------------------------------------------
        public BevCondition_Del(del_Condition del_con)
        {
            m_delCondition = del_con;
        }


        //Override Function
        //-------------------------------------------------------------
        protected override eRunningState Tick(BevData workData)
        {
            isTrue = m_delCondition();
            if (isTrue)
                return eRunningState.Suceed;
            else
                return eRunningState.Failed;
        }
    };
}
