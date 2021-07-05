namespace Assets.Scripts.NUI
{
    class WorldMenu : MenuBase
    {
        public SwitchMenuButton CreateButton;
        public SwitchMenuButton LoadButton;

        public MenuBase CreateMenu;
        public MenuBase LoadMenu;

        protected override void Init()
        {
            CreateButton.SetDestMenu(CreateMenu);
            LoadButton.SetDestMenu(LoadMenu);
        }
    }
}
