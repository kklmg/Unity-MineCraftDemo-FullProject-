using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NObserver
{
    public class MyObserver<T>
    {
        //Field
        //---------------------------------------------------------------
        //UnSubscriber
        private Unsubscriber<T> m_UnSubscriber;
        //Data
        private T m_Obj;

        //Property
        //---------------------------------------------------------------
        public bool IsActive{ set; get; }      
        public T Obj { get { return m_Obj; } }

        //public Function
        //---------------------------------------------------------------
        public void Update(T _Obj)
        {
            m_Obj = _Obj;
        }

    }


    public class MySubject<T> where T : new()
    {
        //Field 
        [SerializeField]
        private T m_Obj = new T();

        //Observers
        private List<MyObserver<T>> m_listObservers;

        //update Subject and notify to all Observers
        public void Update(T _data)
        {
            m_Obj = _data;
            __notify();
        }

        //notify to all observers
        private void __notify()
        {
            foreach (var observer in m_listObservers)
            {
                observer.Update(m_Obj);              
            }
        }

        //Add Observer
        public Unsubscriber<T> Subscribe(MyObserver<T> _Observer)
        {
            m_listObservers.Add(_Observer);
            return new Unsubscriber<T>(m_listObservers, _Observer);
        }
    }

    //Class Unsubscriber
    public class Unsubscriber<T>
    {
        private List<MyObserver<T>> m_listObeservers;
        private MyObserver<T> m_Observer;

        public Unsubscriber(List<MyObserver<T>> observers, MyObserver<T> observer)
        {
            this.m_listObeservers = observers;
            this.m_Observer = observer;
        }

        public void Unsubscribe()
        {
            if (m_Observer != null && m_listObeservers.Contains(m_Observer))
                m_listObeservers.Remove(m_Observer);
        }
    }

}
