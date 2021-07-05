using UnityEngine;

using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;

namespace Assets.Scripts.NCharacter
{
    //class Control_Cha_RotateYaw : BevConditionBase
    //{
    //    private float Cache_Rotate;
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();

    //        //get input
    //        Cache_Rotate = control.Rotate_Yaw();

    //        //not valid input
    //        if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

    //        thisData.Rotation.y = Cache_Rotate;

    //        return true;
    //    }
    //}

    //class Control_Camera_UpDown : BevConditionBase
    //{
    //    private float Cache_Rotate;
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();

    //        //get input
    //        Cache_Rotate = control.Rotate_Yaw();

    //        //not valid input
    //        if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

    //        //save to blackboard
    //        thisData.SetValue(KEY_CONTROL.CAMERA_UD, Cache_Rotate);

    //        return true;
    //    }
    //}

    public class ChaBev_HandleMovement : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            Vector3 translationRequest;

            if (thisData.Request_Translation.TryHandle(out translationRequest))
            {
                translationRequest = thisData.Character.transform.rotation * translationRequest
                    * (thisData.isWalking ? thisData.Character.WalkSpeed : thisData.Character.RunSpeed)
                    * Time.deltaTime;
                              
                //publih this event to character's components
                thisData.NotifyOtherComponents(new E_Cha_TranslateRequest_XZ(translationRequest));

                m_enRunningState = eRunningState.Suceed;
                return m_enRunningState;
            }

            else
            {
                m_enRunningState = eRunningState.Failed;
                return m_enRunningState;
            }
        }
    }


    public class ChaBev_HandleYaw : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            if (thisData.Request_Yaw.TryHandle(out float reqYaw))
            {
                //publih this event to character's components
                thisData.NotifyOtherComponents(new E_Cha_YawRequest(reqYaw));

                m_enRunningState = eRunningState.Suceed;
                return m_enRunningState;
            }
            else
            {
                m_enRunningState = eRunningState.Failed;
                return m_enRunningState;
            }
        }
    }

    public class ChaBev_HandleJump : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            //Check Jump Request 
            if (thisData.Character.MovingType == enMovingType.Walking)
            {
                if (thisData.Request_Jump.TryHandle(out bool isJump) && !thisData.isInAir)
                {
                    thisData.isInAir = true;

                    thisData.NotifyOtherComponents(new E_Cha_JumpUp(thisData.Character.JumpForce));                 
                }
                m_enRunningState = eRunningState.Suceed;
                return eRunningState.Suceed;
            }
            else
            {
                m_enRunningState = eRunningState.Failed;
                return eRunningState.Failed;
            }
        }

    }

    public class ChaBev_HandleFlying : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            //Check Jump Request 
            if (thisData.Character.MovingType == enMovingType.Sliding)
            {
                if (thisData.Request_Slide.TryHandle(out var _value))
                {
                    thisData.isInAir = true;

                    thisData.NotifyOtherComponents(new E_Cha_Fly(_value.Key,_value.Value));
                }
                m_enRunningState = eRunningState.Suceed;
                return eRunningState.Suceed;
            }
            else
            {
                m_enRunningState = eRunningState.Failed;
                return eRunningState.Failed;
            }
        }
    }

    public class ChaBev_HandleSuspending : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            //Check Jump Request 
            if (thisData.Character.MovingType == enMovingType.suspending)
            {
                if (thisData.Request_UpDown.TryHandle(out float _value))
                {
                    thisData.isInAir = true;

                    thisData.NotifyOtherComponents(new E_Cha_Suspend(_value));
                }
                m_enRunningState = eRunningState.Suceed;
                return eRunningState.Suceed;
            }
            else
            {
                m_enRunningState = eRunningState.Failed;
                return eRunningState.Failed;
            }
        }
    }

    //public class Cha_NotGrounded : BevConditionBase
    //{
    //    private IWorld m_refWorld;
    //    //private E_Cha_TryMove m_ECha_TryMove = new E_Cha_TryMove();
    //    protected override void VEnter(BevData workData)
    //    {
    //        m_refWorld = Locator<IWorld>.GetService();
    //    }
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();


    //        IBlock adj = GWorldSearcher.GetBlock(thisData.Character.transform.position + Vector3.down,m_refWorld);
    //        return !(adj != null && adj.IsObstacle);
    //    }
    //}
}
