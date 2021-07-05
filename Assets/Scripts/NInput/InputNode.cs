using UnityEngine;
using Assets.Scripts.NBehaviorTree;

namespace Assets.Scripts.NInput
{
    public delegate void InputEvent(BevData workData);

    public abstract class InputNodeBase : BevConditionBase
    {
        [SerializeField]
        protected string m_strKey;
        public InputEvent InputEvent { set; get; }

        public InputNodeBase(string key)
        {
            m_strKey = key;
        }
    }

    //Input Axis 
    [CreateAssetMenu(menuName = "Condition/Input/Axis")]
    public class ConInputAxis : InputNodeBase
    {
        public float AxisValue{ set; get; }
        public ConInputAxis(string key) : base(key) { }
        public override bool Check(BevData workData)
        {
            
            AxisValue = Input.GetAxis(m_strKey);

            if (!Mathf.Approximately(AxisValue, 0))
            {
                workData.SetValue(m_strKey, AxisValue);
                if (InputEvent != null) InputEvent(workData);
                return true;
            }
            
            return false;
        }
    }

    //Input Button Up
    [CreateAssetMenu(menuName = "Condition/Input/ButtonUp")]
    public class ConButtonUp : InputNodeBase
    {
        public ConButtonUp(string key) : base(key) { }

        public override bool Check(BevData workData)
        {
            if (Input.GetButtonUp(m_strKey))
            {
                if (InputEvent != null) InputEvent(workData);
                return true;
            }
            return false;
        }
    }

    //Input Button Down
    [CreateAssetMenu(menuName = "Condition/Input/ButtonDown")]
    public class ConButtonDown : InputNodeBase
    {
        public ConButtonDown(string key) : base(key) { }
        public override bool Check(BevData workData)
        {
            if (Input.GetButtonDown(m_strKey))
            {
                if (InputEvent != null) InputEvent(workData);
                return true;
            }
            return false;
        }
    }

    //Input Button Press
    [CreateAssetMenu(menuName = "Condition/Input/ButtonPress")]
    public class ConButtonPress : InputNodeBase
    {
        public ConButtonPress(string key) : base(key) { }
        public override bool Check(BevData workData)
        {
            if (Input.GetButton(m_strKey))
            {
                if (InputEvent != null) InputEvent(workData);
                return true;
            }
            return false;
        }
    }

}
