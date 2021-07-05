using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts.NUI
{
    class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool m_IsButtonPressed;

        [Range(0f, 1f)]
        [SerializeField]
        private float m_Sensitivity = 0.5f;
        [SerializeField]
        private float m_Value = 0.0f;

        public bool TryGetValue(out float Value)
        {
            if (m_IsButtonPressed)
            {
                Value = m_Value;
                return true;
            }
            else
            {
                Value = 0.0f;
                return false;
            }
        }
       
        public void Update()
        {
            if (m_IsButtonPressed)
            {
                m_Value += m_Sensitivity * Time.deltaTime;
                m_Value = Mathf.Clamp(m_Value, -1f, 1f);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_IsButtonPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_Value = 0.0f;
            m_IsButtonPressed = false;
        }
    }
}
