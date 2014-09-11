using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger.Configuration
{
    public class ApplicationKeyElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return base["key"] as string; }
            set { base["key"] = value; }
        }
    }
}
