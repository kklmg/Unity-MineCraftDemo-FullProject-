using UnityEngine;
using Assets.Scripts.NBehaviorTree;

namespace Assets.Scripts.NInput
{
    public interface IController
    {
        float Horizontal();
        float Vertical();

        float Move_Y();

        bool Jump();
        bool Fire();
       


        bool Sprint();
        bool Back();

        float Rotate_Yaw();
        float Rotate_Pitch();
        float Rotate_Roll();

        bool CursorDown();
        bool CursorUp();
        bool CursorPress();
        Vector3 CursorPosition();

        bool HasCursorMoved();
    }
}
