using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;


namespace Assets.Scripts.NUI
{
    class BlockSlot_Menu : BlockSlot_Base
    {
        protected override void _OnSelect()
        {
            BlockMenu BlkMenu = GetComponentInParent<BlockMenu>();
            BlkMenu.OnSelectSlot(this);
        }

        protected override void _DisSelect()
        {
            BlockMenu BlkMenu = GetComponentInParent<BlockMenu>();
            BlkMenu.DisselectSlot(this);
        }
    }
}
