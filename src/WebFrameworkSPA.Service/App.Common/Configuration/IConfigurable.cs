using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common
{
    public interface IConfigurable
    {
        void Configure(string configFilePath);
    }
}
