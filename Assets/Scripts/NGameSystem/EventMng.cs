using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameSystem
{
    class EventMng : MonoBehaviour, IGameMng
    {
        private IEventCenter m_EventCenter;
        private IEventHelper m_EventHelper;

        [SerializeField]
        private byte PriorityLevleCount = 10;

        public void ApplySettings(GameSetting setting)
        {
            return;
        }

        public void ProvideEventServ()
        {
            m_EventCenter = new EventCenter(PriorityLevleCount);
            m_EventHelper = new EventHelper(m_EventCenter);
            
            Locator<IEventHelper>.ProvideService(m_EventHelper);
        }

        private void LateUpdate()
        {
            if (m_EventCenter != null)
                m_EventCenter.HandleAll();
        }
    }
}
