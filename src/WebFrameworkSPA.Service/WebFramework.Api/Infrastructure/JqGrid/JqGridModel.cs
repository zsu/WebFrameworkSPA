using System;

namespace Web.Infrastructure.JqGrid
{
    public class JqGridModel
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public Array rows { get; set; }
    }
}
