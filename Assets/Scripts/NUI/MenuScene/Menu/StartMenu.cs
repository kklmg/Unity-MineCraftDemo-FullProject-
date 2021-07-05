
namespace Assets.Scripts.NUI
{
    class StartMenu : MenuBase
    {
        public SwitchMenuButton StartButton;
        public ExitButton ExitButton;
        public SwitchMenuButton SettingButton;

        public MenuBase WorldMenu;
        public MenuBase SettingMenu;

        protected override void Init()
        {
            StartButton.SetDestMenu(WorldMenu);
            SettingButton.SetDestMenu(SettingMenu);
        }
    }
}
