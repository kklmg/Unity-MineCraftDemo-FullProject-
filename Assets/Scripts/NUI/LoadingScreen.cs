using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NUI
{
    class LoadingScreen : MonoBehaviour
    {
        public Image m_FrameImg;
        public Image m_FillImage;

        private float m_CurProgress = 0;

        private List<Action> m_Listeners;

        private int m_TotalTaskCount = 10;

        private bool IsAllTaskCompleted = false;


        private void Awake()
        {
            m_FillImage.fillAmount = 0;
            IsAllTaskCompleted = false;
            m_Listeners = new List<Action>();
        }

        private void Update()
        {
            if (m_FillImage.fillAmount < m_CurProgress)
            {
                m_FillImage.fillAmount += 0.5f * Time.deltaTime;
            }
            if (m_FillImage.fillAmount >= 1)
            {
                IsAllTaskCompleted = true;

                //invoke all listeners
                foreach (var listener in m_Listeners)
                {
                    listener();
                }
                gameObject.SetActive(false);
            }
        }


        public void SetTotalTask(int TotalTask)
        {
            m_TotalTaskCount = TotalTask;
            //Debug.Log(m_TotalTaskCount);
        }

        public void SetRemainedTask(int RemainedTask)
        {
            m_CurProgress = (m_TotalTaskCount - RemainedTask) / (float)m_TotalTaskCount;
        }

        public void AddListener(Action _action)
        {
            m_Listeners.Add(_action);
        }
    }
}
