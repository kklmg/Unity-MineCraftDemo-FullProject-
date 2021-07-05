
namespace Assets.Scripts.NEvent
{
    public interface IEventHandler
    {
        bool Handle(IEvent _event);
    }

    public delegate void Del_HandleEvent(IEvent _event);

    public delegate IEvent Del_DecorateEvent(IEvent _event);
    
}
