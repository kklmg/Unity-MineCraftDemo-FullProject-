using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NCommand;

namespace Assets.Scripts.NUI
{
    public class GameMenu : MonoBehaviour
    {
        public MenuBase StartMenu;
        public MenuBase CurMenu { set; get; }

        private Stack<ICommand> CmdCached;

        public void PushCommand(ICommand cmd)
        {
            CmdCached.Push(cmd);
            //Debug.Log("pushed "+ CmdCached.Count);
        }
        public void PopCommand()
        {
            CmdCached.Pop();
            //Debug.Log("poped " + CmdCached.Count);
        }

        public void Undo()
        {
            //Debug.Log("undo " + CmdCached.Count);
            ICommand command = CmdCached.Pop();
            if (command != null) command.Undo();
        }

        private void Awake()
        {
            CmdCached = new Stack<ICommand>();

            gameObject.SetActive(true);

            var Menus = GetComponentsInChildren<MenuBase>();
            foreach (var Menu in Menus)
            {
                Menu.gameObject.SetActive(false);
            }

            CurMenu = StartMenu;
            CurMenu.gameObject.SetActive(true);
        }

    }
}
