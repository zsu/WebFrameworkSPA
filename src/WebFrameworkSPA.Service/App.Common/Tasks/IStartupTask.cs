namespace App.Common.Tasks
{
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
