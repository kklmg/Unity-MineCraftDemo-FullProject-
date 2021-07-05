
using UnityEngine;

namespace Assets.Scripts.NGlobal.Singleton
{
    public class Singleton<T>  where T : new()
    {
        private static T m_Instance;
        // thread safe
        private static readonly object syslock = new object();
        public static T Instance
        {
            get
            {
                lock (syslock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new T();
                    }
                    return m_Instance;
                }
            }
        }
    }


    public class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;

        // thread safe
        private static readonly object syslock = new object();
        public static T Instance
        {
            get
            {
                lock (syslock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = FindObjectOfType(typeof(T)) as T;

                        if (m_Instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            obj.hideFlags = HideFlags.DontSave;
                            //obj.hideFlags = HideFlags.HideAndDontSave;
                            m_Instance = (T)obj.AddComponent<T>();

                        }
                    }

                    return m_Instance;
                }
            }
        }
    }
}
