namespace App.Common.Logging
{
    public interface ILogFactory
    {
        ILog Create();
        ILog Create(string logType);
    }
}
