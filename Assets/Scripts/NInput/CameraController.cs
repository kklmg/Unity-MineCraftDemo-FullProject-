using System;

using UnityEngine;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NCharacter;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NInput
{
    class CameraController : MonoBehaviour
    {
        public GameObject Prefab_Camera;

        [SerializeField]
        private Character m_FollowingCha;
        private IController m_Controller;

        [SerializeField]
        private float m_Sensitivity = 1.0f;   
        [SerializeField]
        private float m_Yaw;
        [SerializeField]
        private float m_Pitch;

        public Camera CameraIns { private set; get; }

        public float RotateSensitivity { set { m_Sensitivity = value; }get { return m_Sensitivity; } }

        public void SetCameraAt(Transform trans)
        {
            CameraIns.transform.parent = trans;
            CameraIns.transform.localPosition = Vector3.zero;
            CameraIns.transform.localRotation= Quaternion.identity;
            CameraIns.transform.localScale = Vector3.one;
        }

        private void Awake()
        {
            CameraIns = Instantiate(Prefab_Camera).GetComponent<Camera>();
        }


        private void Start()
        {
            m_FollowingCha = MonoSingleton<GameSystem>.Instance.
                PlayerMngIns.PlayerIns.GetComponent<Character>();

            m_Controller = Locator<IController>.GetService();

            SetCameraAt(m_FollowingCha.TF_Eye);
        }



        private void Update()
        {
            m_Pitch = -m_Controller.Rotate_Pitch() * m_Sensitivity;

            if (Mathf.Approximately(0.0f, m_Pitch)) return;

            float Angle = CameraIns.transform.localRotation.eulerAngles.x;


            if (m_Pitch > 0 && Angle <= 90 && Angle + m_Pitch > 90)
            {
                CameraIns.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            else if (m_Pitch < 0 && Angle >= 270 && Angle + m_Pitch < 270)
            {
                CameraIns.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
            else
            {
                CameraIns.transform.Rotate(Vector3.right * m_Pitch);
            }
        }
    }
}
