using System;
using System.Collections.Generic;

namespace Assets.Scripts.NUI
{
    class BackButton : ButtonEx
    {
        private void Awake()
        {
            SetClickCommand(new GoBackMenu());
        }
    }
}
