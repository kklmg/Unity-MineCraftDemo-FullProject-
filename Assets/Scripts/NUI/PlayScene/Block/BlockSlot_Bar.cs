using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI
{
    class BlockSlot_Bar : BlockSlot_Base
    {
        protected override void _OnSelect()
        {
            BlockBar bar = GetComponentInParent<BlockBar>();
            bar.OnSelectSlot(this);
        }

        protected override void _DisSelect()
        {
            BlockBar bar = GetComponentInParent<BlockBar>();
            bar.DisselectSlot(this);
        }
    }
}
