using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Common.Logging;

namespace App.Infrastructure.Log4net
{
    public class Log4netLogFactory:ILogFactory
    {
        #region ILogFactory Members

        public ILog Create()
        {
            return Create(null);
        }
        
        public ILog Create(string logType)
        {
            return new Log4netAdapter(logType);
        }

        #endregion
    }
}
