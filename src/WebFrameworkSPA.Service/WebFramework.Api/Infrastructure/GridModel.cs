using System.Linq;

namespace Web.Infrastructure
{
    public class GridModel<T>
    {
        public IQueryable<T> Items { get; set; }
        public int TotalNumber { get; set; }
    }
}