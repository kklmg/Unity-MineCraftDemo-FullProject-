using System;
using UnityEngine;

namespace Assets.Scripts.NEvent
{
    public class Communicator : MonoBehaviour
    {
        private EventCenter m_EventCenter;
        private EventHelper m_EventHelper;

        private void Awake()
        {
            m_EventCenter = new EventCenter(10);
            m_EventHelper = new EventHelper(m_EventCenter);
        }

        private void LateUpdate()
        {
            m_EventCenter.HandleAll();
        }

        public void Publish(IEvent _event)
        {
            m_EventHelper.Publish(_event);
        }
        public void HandleImmediately(IEvent _event)
        {
            m_EventHelper.Handle(_event);
        }

        public void SubsribeEvent(Guid EventID, Del_HandleEvent handler,enPriority priority=enPriority.Lowest)
        {
            m_EventHelper.Subscribe(EventID, handler,priority);
        }

        public void UnSubscribe(Guid EventID, Del_HandleEvent handler)
        {
            m_EventHelper.UnSubscribe(EventID, handler);
        }
    }
}
