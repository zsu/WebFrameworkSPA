namespace App.Common.Configuration
{
    public interface IConfigManager<TInterface>
    {
        void Clear();
        TInterface GetConfig();
    }
}
