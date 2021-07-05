using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NData;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;


namespace Assets.Scripts.NUI
{
    class BasicSlider : MonoBehaviour
    {
        [SerializeField]
        private Text m_SettingName;

        [SerializeField]
        private Text m_SliderValue;

        [SerializeField]
        private Slider m_Slider;

        public float Value
        {
            set
            {
                m_Slider.value = value;
                m_SliderValue.text = value.ToString();
            }
            get
            {
                return m_Slider.value;
            }
        }

        public void SetValue(RangedInt IntValue)
        {
            m_Slider.wholeNumbers = true;

            m_Slider.minValue = IntValue.MinValue;
            m_Slider.maxValue = IntValue.MaxValue;

            m_Slider.value = IntValue.Get();
            m_SliderValue.text = IntValue.Get().ToString();
        }
        public void SetValue(RangedFloat FloatValue)
        {
            m_Slider.wholeNumbers = false;

            m_Slider.minValue = FloatValue.MinValue;
            m_Slider.maxValue = FloatValue.MaxValue;

            m_Slider.value = FloatValue.Get();
            m_SliderValue.text = FloatValue.Get().ToString();
        }



        private void Awake()
        {
            m_Slider.onValueChanged.AddListener(ShowValue);
        }

        private void ShowValue(float ChangedValue)
        {
            m_SliderValue.text = ChangedValue.ToString();
        }

    }
}
