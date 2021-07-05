using System.Collections.Generic;

namespace Assets.Scripts.NPattern
{
    class BlackBoard
    {
        private Dictionary<string, object> m_Items;
        //
        public BlackBoard()
        {
            m_Items = new Dictionary<string, object>();
        }
        public void SetValue(string key, object value)
        {
            if (m_Items.ContainsKey(key) == false)
            {
                m_Items.Add(key, value);
            }
            else
            {
                m_Items[key] = value;
            }          
        }
        public bool GetValue<T>(string key, out T outData)
        {
            if (m_Items.ContainsKey(key) == false)
            {
                outData = default(T);
                return false;
            }
            outData = (T)m_Items[key];
            return true;
        }
    }
}
