
namespace Assets.Scripts.NBehaviorTree
{
    //Behavior Action Interface
    public interface IBevAction
    {
        void Enter(BevData data);
        eRunningState Run(BevData data);
        void Exit(BevData data);
    }

    //Base leaf Node
    public abstract class BevLeaf : BevNodeBase
    {
    };

    public class BevAction : BevLeaf
    {
        IBevAction Action { set; get; }

        protected override eRunningState Tick(BevData data)
        {
           return Action.Run(data);
        }
    }

}
