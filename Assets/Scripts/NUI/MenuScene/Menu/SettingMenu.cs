using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NUI.Menu
{
    class SettingMenu : MenuBase
    {
        public BackButton backButton;
        public Button ResetButton;
        public Button SaveButton;
        public Button DefaultButton;

        public Dropdown ScreenModeOptions;

        public BasicSlider ViewSlider;
        public BasicSlider RotateSensSlider;
        public BasicSlider JumpForceSlider;
        public BasicSlider PlayerSpeedSlider;
        public BasicSlider PickingDisSlider;

       

        private  GameSetting m_GameSetting;

        protected override void Init()
        {
            //_AssignDataToSlider(m_GameSetting);
        }

        private void Awake()
        {
            m_GameSetting = MonoSingleton<GameSystem>.Instance.GameSettingIns;

            _AssignDataToSlider(m_GameSetting);

            SaveButton.onClick.AddListener(HandleSave);
            ResetButton.onClick.AddListener(HandleReset);
            DefaultButton.onClick.AddListener(HandleResetToDefault);

            //Init Screen Mode Options
            ScreenModeOptions.options.Add(new Dropdown.OptionData(FullScreenMode.ExclusiveFullScreen.ToString()));
            ScreenModeOptions.options.Add(new Dropdown.OptionData(FullScreenMode.FullScreenWindow.ToString()));
            ScreenModeOptions.options.Add(new Dropdown.OptionData(FullScreenMode.MaximizedWindow.ToString()));
            ScreenModeOptions.options.Add(new Dropdown.OptionData(FullScreenMode.Windowed.ToString()));

            ScreenModeOptions.SetValueWithoutNotify((int)Screen.fullScreenMode);
        }

        private void OnEnable()
        {
            //m_GameSetting = MonoSingleton<GameSystem>.Instance.GameSettingIns;
            _AssignDataToSlider(m_GameSetting);
        }

        private void HandleReset()
        {
            _AssignDataToSlider(m_GameSetting);
        }

        private void HandleResetToDefault()
        {
            GameSetting temp = new GameSetting();        

            _AssignDataToSlider(temp);
        }

        private void HandleSave()
        {
            _AssignDataToSystem();

            //save
            MonoSingleton<GameSystem>.Instance.SaveSettings();

            Screen.fullScreenMode = (FullScreenMode)ScreenModeOptions.value;
        }

        private void _AssignDataToSlider(GameSetting Setting)
        {
            //Debug.Log(m_GameSetting);

            //view distance
            ViewSlider.SetValue(Setting.PlayerView);
            //rotate sensitivity
            RotateSensSlider.SetValue(Setting.RotateSensitivity);
            //player jump force
            JumpForceSlider.SetValue(Setting.JumpForce);
            //Picking distance
            PickingDisSlider.SetValue(Setting.PickingDistance);
            //player speed
            PlayerSpeedSlider.SetValue(Setting.PlayerSpeed);
        }

        private void _AssignDataToSystem()
        {
            //view distance
            m_GameSetting.PlayerView.Set((int)ViewSlider.Value);
            //rotate sensitivity
            m_GameSetting.RotateSensitivity.Set(RotateSensSlider.Value);
            //player gravity
            m_GameSetting.JumpForce.Set(JumpForceSlider.Value);
            //Picking distance
            m_GameSetting.PickingDistance.Set((int)PickingDisSlider.Value);
            //player speed
            m_GameSetting.PlayerSpeed.Set((int)PlayerSpeedSlider.Value);
        }
    }
}


