namespace App.Common.SessionMessage
{
    public interface ISessionMessageFactory
    {
        ISessionMessageProvider CreateInstance();
    }
}