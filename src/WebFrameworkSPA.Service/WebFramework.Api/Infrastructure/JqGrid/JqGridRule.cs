using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.JqGrid
{
    public class JqGridRule
    {
        public string field { get; set; }
        public SearchOperations op { get; set; }
        public string data { get; set; }
    }
}
