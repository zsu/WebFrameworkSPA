
namespace App.Common.Events
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
