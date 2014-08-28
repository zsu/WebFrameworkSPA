namespace App.Data
{
    /// <summary>
    /// Factory interface that the <see cref="UnitOfWorkScope"/> uses to create instances of
    /// <see cref="IUnitOfWork"/>
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns></returns>
        IUnitOfWork Create();
    }
}