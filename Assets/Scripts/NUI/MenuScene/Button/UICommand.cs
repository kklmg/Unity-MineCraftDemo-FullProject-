using UnityEngine;

using Assets.Scripts.NCommand;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    public class ExitGame : ICommand
    {
        public void Execute()
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        public void Undo() { }

    }


    public abstract class UICommand : ICommand
    {

        public void Execute()
        {
            MonoSingleton<GameMenu>.Instance.PushCommand(this);
            UIExecute();
        }
        public void Undo()
        {
            UIUndo();
        }

        public abstract void UIExecute();

        public abstract void UIUndo();

    }

    public class SwitchMenu : UICommand
    {
        MenuBase m_Orig;
        MenuBase m_Dest;

        public SwitchMenu(MenuBase Cur, MenuBase Dest)
        {
            m_Orig = Cur;
            m_Dest = Dest;
        }

        public override void UIExecute()
        {
            MonoSingleton<GameMenu>.Instance.CurMenu = m_Dest;
            m_Orig.gameObject.SetActive(false);
            m_Dest.gameObject.SetActive(true);
        }

        public override void UIUndo()
        {
            MonoSingleton<GameMenu>.Instance.CurMenu = m_Orig;
            m_Orig.gameObject.SetActive(true);
            m_Dest.gameObject.SetActive(false);         
        }
    }

    public class GoBackMenu : ICommand
    {
        public void Execute()
        {
            MonoSingleton<GameMenu>.Instance.Undo();
        }

        public void Undo()
        {
        }
    }
}
