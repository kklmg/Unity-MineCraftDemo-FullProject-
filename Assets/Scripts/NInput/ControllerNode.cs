using UnityEngine;
using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NInput
{
    class Control_Horizontal : BevConditionBase
    {
        private float Cache_Hor;
        public override bool Check(BevData workData)
        {
            Cache_Hor = Locator<IController>.GetService().Horizontal();

            if (Mathf.Approximately(Cache_Hor, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, Cache_Hor);
                return true;
            }
        }
    }
    class Control_Vertical : BevConditionBase
    {
        private float Cache_Ver;
        public override bool Check(BevData workData)
        {
            Cache_Ver = Locator<IController>.GetService().Vertical();

            if (Mathf.Approximately(Cache_Ver, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.VERTICAL, Cache_Ver);
                return true;
            }
        }
    }
    class Control_Rotate_X : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = Locator<IController>.GetService().Rotate_Yaw();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Rotate_Y : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = Locator<IController>.GetService().Rotate_Pitch();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Rotate_Z : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = Locator<IController>.GetService().Rotate_Roll();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Jump : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return Locator<IController>.GetService().Jump();
        }
    }
    class Control_Fire : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return Locator<IController>.GetService().Fire();
        }
    }
    class Control_Sprint : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return Locator<IController>.GetService().Sprint();
        }
    }
}
