using Assets.Scripts.NCommand;
using Assets.Scripts.NData;

namespace Assets.Scripts.NEvent
{
    class E_Block_Modify : EventBase<E_Block_Modify>
    {
        public Com_ModifyBlock Request { set; get; }
        public E_Block_Modify(ref BlockLocation Loc, byte BlockID)
        {
            Request = new Com_ModifyBlock(ref Loc, BlockID);
        }
    }

    class E_Block_Recover : EventBase<E_Block_Recover>
    {
    }



}
