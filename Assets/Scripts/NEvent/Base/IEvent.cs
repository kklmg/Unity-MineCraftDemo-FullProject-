using System;
using UnityEngine;


namespace Assets.Scripts.NEvent
{
    public interface IEvent
    {
        Guid Type { get; }
        enPriority Priority{ get; }
        bool HasHandled { set; get; }

        T Cast<T>() where T : class;
    }

    public abstract class EventBase<TEventName> : IEvent
    {
        [SerializeField]
        public static readonly Guid ID = Guid.NewGuid();

        [SerializeField]
        private enPriority m_Priority = enPriority.Highest;

        [SerializeField]
        private bool m_HasHandled = false;

        //property
        public Guid Type { get { return ID; } }
        public enPriority Priority
        {
            get { return m_Priority; }
            protected set { m_Priority = value; }
        }
        public bool HasHandled
        {
            get { return m_HasHandled; }
            set { m_HasHandled = value; }
        }

        public T Cast<T>() where T : class
        {
            return this as T;
        }

        //T IEvent.Cast<T>() 
        //{

        //    T temp = this as T;
        //    return temp;

        //}
    }
}
