namespace Assets.Scripts.NUI
{
    public class SwitchMenuButton : ButtonEx
    {
        public MenuBase m_DestMenu;

        public void SetDestMenu(MenuBase Dest)
        {
            m_DestMenu = Dest;
            SetClickCommand(new SwitchMenu(GetComponentInParent<MenuBase>(), m_DestMenu));
        }
    }
}
