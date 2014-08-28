using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.JqGrid
{
    public enum SearchOperations
    {
        eq, // "equal"
        ne, // "not equal"
        lt, // "less"
        le, // "less or equal"
        gt, // "greater"
        ge, // "greater or equal"
        bw, // "begins with"
        bn, // "does not begin with"
        ew, // "ends with"
        en, // "does not end with"
        cn, // "contains"
        nc  // "does not contain"
        //in, // "in"
        //ni // "not in"
    }
}
