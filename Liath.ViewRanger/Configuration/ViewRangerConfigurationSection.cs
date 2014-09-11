using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Configuration
{
    public class ViewRangerConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("apiAddress", IsRequired = false)]
        public AddressElement BaseAddress
        {
            get { return base["apiAddress"] as AddressElement; }
            set { base["apiAddress"] = value; }
        }

        [ConfigurationProperty("applicationKey", IsRequired = true)]
        public ApplicationKeyElement ApplicationKey
        {
            get { return base["applicationKey"] as ApplicationKeyElement; }
            set { base["applicationKey"] = value; }
        }
    }
}
