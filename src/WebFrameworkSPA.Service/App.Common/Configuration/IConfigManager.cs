using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common.Configuration
{
    public interface IConfigManager<TInterface>
    {
        void Clear();
        TInterface GetConfig();
    }
}
