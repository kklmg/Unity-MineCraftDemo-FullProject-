using UnityEngine;

namespace Assets.Scripts.NBehaviorTree
{
    //Base Decorate Node
    abstract class BevDecorator : BevNodeBase
    {
        //Child
        [SerializeField]
        protected BevNodeBase m_Child;

        public BevDecorator() { }
        public BevDecorator(BevNodeBase child)
        {
            m_Child = child;
        }

        public void setChild(BevNodeBase Child)
        {
            m_Child = Child;
        }
    };

    //Repeat Decorator
    [CreateAssetMenu(menuName = "Bev/Repeator")]
    class BevRepeator : BevDecorator
    {
        public BevRepeator(BevNodeBase child) : base(child) { }
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;
            res = m_Child.Evaluate(workData);

            if (res == eRunningState.Completed) res = eRunningState.Ready;
            return res;
        }
    };

    //Reverse Decorator
    [CreateAssetMenu(menuName = "Bev/Reverser")]
    class BevReverser : BevDecorator
    {
        public BevReverser(BevNodeBase child) : base(child) { }
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;

            res = m_Child.Evaluate(workData);
            res = res == eRunningState.Suceed ? eRunningState.Failed : eRunningState.Suceed;

            return res;
        }
    };
}
