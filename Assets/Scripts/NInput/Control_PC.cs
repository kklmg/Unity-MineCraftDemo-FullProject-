using UnityEngine;

namespace Assets.Scripts.NInput
{
    public class Control_PC : IController
    {
        public float Horizontal()
        {
            return Input.GetAxis(KEY_INPUT.HORIZONTAL);
        }
        public float Vertical()
        {
            return Input.GetAxis(KEY_INPUT.VERTICAL);
        }
        public float Move_Y()
        {

            return Input.GetAxis(KEY_INPUT.MOVE_Y);
        }


        public bool Fire()
        {
            return Input.GetButtonDown(KEY_INPUT.ATTACK);
        }
        public bool Jump()
        {
            return Input.GetButtonDown(KEY_INPUT.JUMP);
        }
        public bool Sprint()
        {
            return Input.GetButtonDown(KEY_INPUT.RUN);
        }

        public float Rotate_Yaw()
        {
            return Input.GetAxis(KEY_INPUT.MOUSE_HORIZONTAL);
        }
        public float Rotate_Pitch()
        {
            return Input.GetAxis(KEY_INPUT.MOUSE_VERTICAL);
        }

        public float Rotate_Roll()
        {
            new System.NotImplementedException("Roll");
            return 0;
        }
        //xxxxxx

        public bool Back()
        {
            return Input.GetMouseButtonDown(1);
        }

        public bool CursorDown()
        {
            return Input.GetMouseButtonDown(0);
        }
        public bool CursorUp()
        {
            return Input.GetMouseButtonUp(0);
        }
        public bool CursorPress()
        {
            return Input.GetMouseButton(0);
        }
        public Vector3 CursorPosition()
        {
            return Input.mousePosition;
        }

        public bool HasCursorMoved()
        {
            return Input.GetAxis(KEY_INPUT.MOUSE_VERTICAL)!=0 
                || Input.GetAxis(KEY_INPUT.MOUSE_HORIZONTAL)!=0;
        }

    }
}
