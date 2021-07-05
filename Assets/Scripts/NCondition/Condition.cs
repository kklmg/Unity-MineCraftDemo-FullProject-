using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NCondition
{
    public delegate bool del_Condition();

    public abstract class ConditionBase : ScriptableObject
    {
        public abstract bool Check();
    }

    //Condtion Composite 
    public abstract class Con_Composite : ConditionBase
    {
        [SerializeField]
        protected List<ConditionBase> m_Conditions;

        public void AddNode(ConditionBase con)
        {
            m_Conditions.Add(con);
        }
    }

    //Condtion And 
    [CreateAssetMenu(menuName = "Condition/And")]
    public class Con_And : Con_Composite
    {
        public override bool Check()
        {
            bool temp = true;

            foreach (var con in m_Conditions)
            {
                temp &= con.Check();
            }
            return temp;
        }
    }

    //Condtion Or 
    [CreateAssetMenu(menuName = "Condition/Or")]
    public class Con_Or : Con_Composite
    {
        public override bool Check()
        {
            bool temp = true;

            foreach (var con in m_Conditions)
            {
                temp |= con.Check();
            }
            return temp;
        }
    }

    //Condition simple 
    [CreateAssetMenu(menuName = "Condition/Del")]
    public class Con_Delegate: ConditionBase
    {
        [SerializeField]
        del_Condition m_delFun;

        public Con_Delegate(del_Condition con)
        {
            m_delFun = new del_Condition(con);
        }

        public override bool Check()
        {
            return m_delFun();
        }
    }

    //Condition Not
    [CreateAssetMenu(menuName = "Condition/Not")]
    class Con_Not : ConditionBase
    {
        [SerializeField]
        ConditionBase m_Con; // condition

        Con_Not(ConditionBase con)
        {
            m_Con = con;
        }
        public override bool Check()
        {
            return !m_Con.Check();
        }
    }
}
