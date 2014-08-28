using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class GridModel<T>
    {
        public IQueryable<T> Items { get; set; }
        public int TotalNumber { get; set; }
    }
}