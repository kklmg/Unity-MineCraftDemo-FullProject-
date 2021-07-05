using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NUI;
using Assets.Scripts.NInput;

namespace Assets.Scripts.NGameSystem
{
    [System.Serializable]
    class InputMng : MonoBehaviour, IGameMng
    {
        public Canvas prefab_Canvas;
        public GameObject Prefab_Picker;
        public GameObject Prefab_CamCtrl;    //camera controller
        public GameObject Prefab_PlayerCtrl; //Plyaer controller

        public Picker PickerIns { private set; get; }
        public CameraController CamCtrlIns { private set; get; }
        public PlayerController PlayerCtrlIns { private set; get; }

        [SerializeField]
        [Range(1, 5)]
        private float m_RotateSens;

        [SerializeField]
        [Range(1, 5)]
        private int m_PickDistance;

        public int PickDistance { set { m_PickDistance = value; } get { return m_PickDistance; } }
        public float RotateSens { set { m_RotateSens = value; } get { return m_RotateSens; } }


        public bool InitInputService()
        {
            //Check Device Type        
            switch (SystemInfo.deviceType)
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Handheld:
                    break;
                case DeviceType.Console:
                    break;
                case DeviceType.Desktop:
                    Locator<IController>.ProvideService(new Control_PC());
                    break;
                default:
                    break;
            }

            if (Locator<IController>.GetService() == null)
                Locator<IController>.ProvideService(new Control_PC());

            //Debug.Log(Locator<IController>.GetService());

            return true;
        }

        public void StopHandleInput()
        {
            PickerIns.gameObject.SetActive(false);
            CamCtrlIns.gameObject.SetActive(false);
            PlayerCtrlIns.gameObject.SetActive(false);
        }

        public void StartHandleInput()
        {
            PickerIns.gameObject.SetActive(true);
            CamCtrlIns.gameObject.SetActive(true);
            PlayerCtrlIns.gameObject.SetActive(true);
        }

        public void InitController()
        {
            //Picker
            PickerIns =
                Instantiate(Prefab_Picker).GetComponent<Picker>();
            PickerIns.SetPickingDistance(m_PickDistance);

            //Camera controller
            CamCtrlIns =
                Instantiate(Prefab_CamCtrl).GetComponent<CameraController>();
            CamCtrlIns.RotateSensitivity = m_RotateSens;

            //Player Controller
            PlayerCtrlIns =
                Instantiate(Prefab_PlayerCtrl).GetComponent<PlayerController>();
            PlayerCtrlIns.RotateSensitivity = m_RotateSens;

        }

        public void InitUISet()
        {
            Instantiate(prefab_Canvas);
        }

        public void ApplySettings(GameSetting setting)
        { 
            m_RotateSens = setting.RotateSensitivity.Get();
            m_PickDistance = setting.PickingDistance.Get();
        }
    }
}
