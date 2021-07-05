using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public class Request<T>
    {
        [SerializeField]
        private T m_Data;

        [SerializeField]
        private bool m_IsHandled;

        public bool IsHandled { get { return m_IsHandled; } }

        public Request()
        {
            m_Data = default;
            m_IsHandled = true;
        }

        public Request(T data)
        {
            m_Data = data;
            m_IsHandled = false;
        }

        public void SetRequest(T data)
        {
            m_Data = data;
            m_IsHandled = false;
        }

        public T Handle(bool SetHandled = true)
        {
            if (m_IsHandled == false)
            {
                if (SetHandled == true) { m_IsHandled = true; }
                return m_Data;
            }
            else
            {
                return default;
            }
        }

        public bool TryHandle(out T reqData, bool SetHandled = true)
        {
            if (m_IsHandled == false)
            {
                reqData = m_Data;

                if (SetHandled == true) { m_IsHandled = true; }

                return true;
            }
            else
            {
                reqData = default;
                return false;
            }
        }
    }
}
