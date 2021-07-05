using System;
using System.Collections.Generic;

using UnityEngine;


namespace Assets.Scripts.NEvent
{
    [RequireComponent(typeof(Communicator))]
    abstract class EventMono : MonoBehaviour
    {
        private Communicator m_Communicator;

        private List<KeyValuePair<Guid,Del_HandleEvent>> m_EventHandlers;

        private void Awake()
        {
            m_Communicator = GetComponent<Communicator>();
            m_EventHandlers = new List<KeyValuePair<Guid, Del_HandleEvent>>();

            AAwake();
        }

        abstract protected void AAwake();

        private void OnDisable()
        {
            foreach (var handlers in m_EventHandlers)
            {
                m_Communicator.UnSubscribe(handlers.Key, handlers.Value);
            }

            AOnDisable();
        }

        abstract protected void AOnDisable();

        protected void SubscribeEvent(Guid EventID, Del_HandleEvent handler, enPriority priority = enPriority.Lowest)
        {
            m_EventHandlers.Add(new KeyValuePair<Guid, Del_HandleEvent>(EventID, handler));
            m_Communicator.SubsribeEvent(EventID, handler, priority);
        }

        protected void PublishEvent(IEvent _event)
        {
            m_Communicator.Publish(_event);
        }
    }
}

