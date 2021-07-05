
namespace Assets.Scripts.NUI
{
    class ExitButton : ButtonEx
    {
        private void Awake()
        {
            SetClickCommand(new ExitGame());
        }
    }
}
