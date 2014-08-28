using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.JqGrid
{
    public class JqGridFilter
    {
        public GroupOperations groupOp { get; set; }
        public List<JqGridRule> rules { get; set; }
        public List<JqGridFilter> groups { get; set; }
    }
}
