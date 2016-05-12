using NHibernate.Dialect;
using NHibernate.Dialect.Function;
namespace Web.Infrastructure
{
    public class FixedMsSqlCe40Dialect : MsSqlCe40Dialect
    {
        public FixedMsSqlCe40Dialect()
        {
            RegisterFunction("trim", (ISQLFunction)new AnsiTrimEmulationFunction());//NHibernate SqlCE has bug when using .Trim().ToLowerInvariant()
        }
        public override bool SupportsVariableLimit
        {
            get
            {
                return true;
            }
        }
    }
}
