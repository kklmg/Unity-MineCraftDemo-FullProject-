using UnityEngine;

namespace Assets.Scripts.NBehaviorTree
{
    //Base Behavior Node
    //template method pattern

    public abstract class BevNodeBase : ScriptableObject //,ICloneable
    {
        //Field
        //----------------------------------------------------------------------
        protected bool m_bVisited = false;
        protected eRunningState m_enRunningState = eRunningState.Ready;
        protected string m_strNodeName;

        //Property
        //----------------------------------------------------------------------
        public bool Visited { get { return m_bVisited; } }
        public eRunningState RunningState{ get { return m_enRunningState; }  }


        //Public Function
        //----------------------------------------------------------------------
        //Run Node
        public eRunningState Evaluate(BevData workData)
        {
            if (m_bVisited == false)
                VEnter(workData);

            //run node
            m_enRunningState = Tick(workData);

            if (m_enRunningState == eRunningState.Completed)
                VExit(workData);

            return m_enRunningState;
        }

        //virtual Function     
        //----------------------------------------------------------------------
        protected virtual void Reset()
        {
            m_bVisited = false;
            m_enRunningState = eRunningState.Ready;
        }
        protected virtual void VEnter(BevData workData) { }
        protected virtual void VExit(BevData workData) { }

        //abstract Function
        //----------------------------------------------------------------------
        protected abstract eRunningState Tick(BevData workData);
    }

}