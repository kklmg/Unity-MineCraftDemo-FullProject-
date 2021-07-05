using UnityEngine;

namespace Assets.Scripts.NUI
{
    public class FPSCounter : MonoBehaviour
    {
        private const float updateTime = 1f;
        private float frames = 0f;
        private float time = 0f;

       
        public string strFps;

        private void Update()
        {
            if (time >= updateTime)
            {
                strFps = "FPS: " + (frames / time).ToString("f2");
                time = 0f;
                frames = 0f;
            }
            frames++;
            time += Time.deltaTime;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 100, Screen.height - 45, 100, 20), strFps);
        }

    }
}
