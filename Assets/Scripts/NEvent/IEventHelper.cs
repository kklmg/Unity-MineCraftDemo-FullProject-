using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.NEvent
{
    public interface IEventHelper
    {
        void Subscribe(Guid EventID, Del_HandleEvent handler,enPriority HandlePriority= enPriority.Default);
        void UnSubscribe(Guid EventID, Del_HandleEvent handler);

        void Publish(IEvent _event);

        //publish the event and handle this immediately
        void Handle(IEvent _event);
    }
}
