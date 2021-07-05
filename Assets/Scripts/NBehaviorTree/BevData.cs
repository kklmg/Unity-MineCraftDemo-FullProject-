using Assets.Scripts.NPattern;

namespace Assets.Scripts.NBehaviorTree
{
    //Behavior states
    public enum eRunningState
    {
        Failed = 0,
        Suceed = 1,
        Ready,
        Running,
        Completed
    };

    //Working Data
    public class BevData
    {       
        public BevData()
        {
            m_BLackBoard = new BlackBoard();
        }
        //Field
        //------------------------------------------------------------
        private BlackBoard m_BLackBoard;

        public void SetValue(string key, object value)
        {
            m_BLackBoard.SetValue(key, value);
        }
        public bool GetValue<T>(string key, out T outData)
        {
            return m_BLackBoard.GetValue<T>(key, out outData);
        }
    }
}
