using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NCommand;

namespace Assets.Scripts.NUI
{
    [RequireComponent(typeof(Button))]
    public class ButtonEx : MonoBehaviour
    {
        protected ICommand m_Command;

        public void SetClickCommand(ICommand command)
        {
            m_Command = command;
            GetComponent<Button>().onClick.AddListener(m_Command.Execute);
        }
    }
}
