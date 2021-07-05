using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NCharacter;
using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameSystem
{
    class PlayerMng : MonoBehaviour,IGameMng
    {
        public GameObject m_PrefabPlayer;

        [SerializeField]
        [Range(0, 5)]
        private int m_ViewDistance = 2;

        [SerializeField]
        private int m_Speed = 10;

        [SerializeField]
        private float m_JumpForce = 0.5f;


        private GameObject m_playerIns; //player instance

        //------------------------------------------------------------

        public GameObject PlayerIns { get { return m_playerIns; } }
        public int PlayerView { get { return m_ViewDistance; } }   
        public int Speed { get { return m_Speed; }set { m_Speed = value; } }
        public float JumpForce { get { return m_JumpForce; } set { m_JumpForce = value; } }


        public void ApplySettings(GameSetting setting)
        {
            m_ViewDistance = setting.PlayerView.Get();
            m_Speed = setting.PlayerSpeed.Get();
            m_JumpForce = setting.JumpForce.Get();
        }

        //------------------------------------------------------------

        public void SpawnPlayer()
        {
            SaveMng Mng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
            IWorld world = Locator<IWorld>.GetService();

            if (!Mng.IsFirstCreated)
            {                
                Mng.PlayerAltitude = GWorldSearcher.GetGroundHeight(Mng.PlayerPos, world) + 3;
                Mng.IsFirstCreated = true;
            }

            //make a player Instance
            m_playerIns = Instantiate(m_PrefabPlayer,
                MonoSingleton<WorldMng>.Instance.WorldIns.transform);

            //set character attribute
            Character character = m_playerIns.GetComponent<Character>();
            character.WalkSpeed = m_Speed;
            character.RunSpeed = m_Speed * 2;
            character.JumpForce = m_JumpForce;

            //set player's postion
            m_playerIns.transform.localPosition = Mng.PlayerPos;
            m_playerIns.transform.localRotation = Mng.PlayerRot;

            //publish event
            Locator<IEventHelper>.GetService().
               Publish(new E_Player_Spawned(Mng.PlayerPos, m_playerIns.GetComponent<Character>()));
        }
    }
}
