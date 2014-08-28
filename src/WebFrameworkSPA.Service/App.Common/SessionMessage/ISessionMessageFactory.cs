using App.Common.SessionMessage;
namespace App.Common.SessionMessage
{
    public interface ISessionMessageFactory
    {
        ISessionMessageProvider CreateInstance();
    }
}