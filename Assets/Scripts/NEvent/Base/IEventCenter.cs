using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.NEvent
{
    public interface IEventCenter
    {
        void AddEvent(IEvent _event);
        void SubScribe(Guid ID, Del_HandleEvent EventHandler, enPriority HandlePriority);
        void UnSubScribe(Guid ID, Del_HandleEvent EventHandler);
        void Handle(IEvent _event);
        void HandleAll();
    }
}
