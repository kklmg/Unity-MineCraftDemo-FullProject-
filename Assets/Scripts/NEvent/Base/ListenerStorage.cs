using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NEvent
{
    class ListenerStorage
    {
        Dictionary<Guid/*EventID*/, 
            SortedDictionary<enPriority/*HandlePriority*/, 
                List<Del_HandleEvent>/*Handlers*/>> m_Listeners; //Event listeners

        Dictionary<Del_HandleEvent,enPriority> m_PrioritiesDic;//

        //Dictionary
        //    <Del_HandleEvent, 
        //    KeyValuePair<enPriority, int /*IndexInLIst*/>> m_PrioritiesDic;//

       
        public ListenerStorage()
        {
            m_Listeners 
                = new Dictionary<Guid, SortedDictionary<enPriority, List<Del_HandleEvent>>>();
            m_PrioritiesDic
                = new Dictionary<Del_HandleEvent, enPriority>();
        }

        public void Add(Guid EventID, Del_HandleEvent EventHandler, enPriority HandlePriority)
        {
            //Case:Exist eventId matched ListenerGroup
            if (m_Listeners.TryGetValue(EventID, out var ListenerGroup))
            {
                //Case: Exist priority matched Listener List 
                //Act: Just Add Listener
                if (ListenerGroup.TryGetValue(HandlePriority, out var ListenerList))
                {
                    m_PrioritiesDic.Add(EventHandler,HandlePriority);

                    ListenerList.Add(EventHandler);
                }
                //Case: no priority matched Listener List 
                //Act: Make a instnace of Listener list first and Add Listener
                else
                {
                    ListenerList = new List<Del_HandleEvent>();

                    m_PrioritiesDic.Add(EventHandler,HandlePriority);

                    ListenerList.Add(EventHandler);

                    ListenerGroup.Add(HandlePriority, ListenerList);
                }
            }
            //Case: no eventId matched ListenerGroup
            else
            {
                //Make an instance of ListenerGroup
                ListenerGroup = new SortedDictionary<enPriority, List<Del_HandleEvent>>();

                //Make an instance of ListenerList
                List<Del_HandleEvent> ListenerList = new List<Del_HandleEvent>();

                //Save to dictionary
                m_PrioritiesDic.Add(EventHandler,HandlePriority);

                //add Handler
                ListenerList.Add(EventHandler);

                //add ListenerList
                ListenerGroup.Add(HandlePriority, ListenerList);

                //add ListenerGroup
                m_Listeners.Add(EventID, ListenerGroup);
            }
        }

        public void Remove(Guid ID, Del_HandleEvent EventHandler)
        {
            if (m_PrioritiesDic.TryGetValue(EventHandler, out var Priority))
            {
                m_Listeners[ID][Priority].Remove(EventHandler);

                m_PrioritiesDic.Remove(EventHandler);
            }
        }

        public void Handle(IEvent _event)
        {
            //find mapped listerners 
            if (m_Listeners.TryGetValue
                (_event.Type, out var ListenerGroup))
            {
                foreach (var ListenerList in ListenerGroup)
                {
                    foreach (var Handler in ListenerList.Value)
                    {
                        if (_event.HasHandled) return;
                        //Handle event
                        Handler(_event);
                    }
                }
            }
        }
    }
}
