using System;
using System.Collections.Generic;

using System.Collections;

using UnityEngine;

namespace Assets.Scripts.NEvent
{
    class EventCenter : IEventCenter
    {
        //Field 
        //-----------------------------------------------------------------------------
        Dictionary<Guid/*EventID*/, ListenerStorage> m_EventListeners; //Event listeners

        Queue<IEvent>[] m_EventStorage; //Event Storage

        int m_PriorityLevelCount;

        //Constructor
        //-----------------------------------------------------------------------------
        public EventCenter(int priorityCount)
        {
            m_PriorityLevelCount = priorityCount;

            m_EventListeners = new Dictionary<Guid, ListenerStorage>();
            m_EventStorage = new Queue<IEvent>[10];
            
            for (int i = 0; i < priorityCount; ++i)
            {
                m_EventStorage[i] = new Queue<IEvent>();
            }
        }

        //public Functions
        //-----------------------------------------------------------------------------
        public void AddEvent(IEvent _event)
        {
            m_EventStorage[(int)_event.Priority].Enqueue(_event);         
        }

        public void SubScribe(Guid EventID, Del_HandleEvent EventHandler
            , enPriority HandlePriority)
        {
            //Case: Exist listenerStorage
            if (m_EventListeners.TryGetValue(EventID,
                out ListenerStorage tempListeners))
            {
                tempListeners.Add(EventID, EventHandler,HandlePriority);
            }
            //Case: No listenerStorage
            else
            {
                //Make a new instance of  this event listeneres
                tempListeners = new ListenerStorage();
                //add this event Handler
                tempListeners.Add(EventID, EventHandler, HandlePriority);
                //save this event listeners
                m_EventListeners.Add(EventID, tempListeners);
            }
        }
        
        public void UnSubScribe(Guid ID, Del_HandleEvent EventHandler)
        {
            //Case: Exist listener list
            if (m_EventListeners.TryGetValue(ID,
                out ListenerStorage tempListeners))
            {
                tempListeners.Remove(ID,EventHandler);
            }
        }


        public void Handle(IEvent _event)
        {
            //find mapped listerners 
            if (m_EventListeners.TryGetValue
                (_event.Type, out ListenerStorage Listeners))
            {
                Listeners.Handle(_event);
            }
        }

        public void HandleAll()
        {
            foreach (var eventqueue in m_EventStorage)
            {
                while (eventqueue.Count != 0)
                {
                    Handle(eventqueue.Dequeue());
                }
            }
        }
    }
}
