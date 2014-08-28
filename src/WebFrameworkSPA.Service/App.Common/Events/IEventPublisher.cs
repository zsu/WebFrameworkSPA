
namespace App.Common.Events
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
