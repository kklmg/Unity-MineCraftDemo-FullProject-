using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public class RangedInt
    {
        public int MinValue { private set; get; }
        public int MaxValue { private set; get; }
        public int Value { get { return m_Value; } }

        [SerializeField]
        private int m_Value;

        public RangedInt(int _minValue, int _maxValue)
        {
            Debug.Assert(_minValue < _maxValue, "InValid Range");

            MinValue = _minValue;
            MaxValue = _maxValue;
            m_Value = _minValue;
        }

        public RangedInt(int _minValue, int _maxValue, int _defaultValue)
        {
            Debug.Assert(_defaultValue > _minValue && _defaultValue < _maxValue,
                "InValid Range" + " min " + MinValue + " max " + MaxValue + " default " + _defaultValue);
            MinValue = _minValue;
            MaxValue = _maxValue;
            m_Value = _defaultValue;
        }

        public void Set(int _value)
        {
            if (_value < MinValue) m_Value = MinValue;
            else if (_value > MaxValue) m_Value = MaxValue;
            else m_Value = _value;
        }

        public int Get()
        {
            return m_Value;
        }
    }

    [System.Serializable]
    public class RangedFloat
    {
        public float MinValue { private set; get; }
        public float MaxValue { private set; get; }

        [SerializeField]
        private float m_Value;

        public RangedFloat(float _minValue, float _maxValue)
        {
            Debug.Assert(_minValue < _maxValue, "InValid Range");

            MinValue = _minValue;
            MaxValue = _maxValue;
            m_Value = _minValue;
        }

        public RangedFloat(float _minValue, float _maxValue,float _defaultValue)
        {
            Debug.Assert(_defaultValue >= _minValue && _defaultValue <= _maxValue, "InValid Range");

            MinValue = _minValue;
            MaxValue = _maxValue;
            m_Value = _defaultValue;
        }

        public void Set(float _value)
        {
            if (_value < MinValue) m_Value = MinValue;
            else if (_value > MaxValue) m_Value = MaxValue;
            else m_Value = _value;
        }
        public float Get()
        {
            return m_Value;
        }
    }
}
