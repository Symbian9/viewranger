using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger.Configuration
{
    public class AddressElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return base["url"] as string; }
            set { base["url"] = value; }
        }
    }
}
