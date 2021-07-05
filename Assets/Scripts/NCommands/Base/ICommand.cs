
namespace Assets.Scripts.NCommand
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public abstract class CommandBase : ICommand
    {
        public bool IsExecuted { set; get; }
        public void Execute()
        {
            if (IsExecuted == false)
            {
                AExecute();
                IsExecuted = true;
            }
        }
        public void Undo()
        {
            if (IsExecuted == true)
            {
                AExecute();
                IsExecuted = false;
            }
        }

        protected abstract void AExecute();
        protected abstract void AUndo();
    }
}
