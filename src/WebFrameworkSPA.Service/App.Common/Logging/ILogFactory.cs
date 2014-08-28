using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common.Logging
{
    public interface ILogFactory
    {
        ILog Create();
        ILog Create(string logType);
    }
}
