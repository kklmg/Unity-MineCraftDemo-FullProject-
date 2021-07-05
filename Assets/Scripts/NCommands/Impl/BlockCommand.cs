using Assets.Scripts.NData;

namespace Assets.Scripts.NCommand
{
    public class Com_ModifyBlock : ICommand
    {
        
        BlockLocation m_BlkLoc;
        public BlockLocation Location { get { return m_BlkLoc; } }

        byte m_PreBlkID;
        byte m_ToModifyID;

        public byte ModifyID { get { return m_ToModifyID; } }
        public byte PreID { get { return m_PreBlkID; } }

        //Constructor
        public Com_ModifyBlock(ref BlockLocation blkloc, byte blkID)
        {
            m_BlkLoc = blkloc;
            m_ToModifyID = blkID;
            m_PreBlkID = m_BlkLoc.CurBlockID;
        }

        public void Execute()
        {
            m_BlkLoc.CurBlockID = m_ToModifyID;
        }
        public void Undo()
        {
            m_BlkLoc.CurBlockID = m_PreBlkID;
        }
    }
}
