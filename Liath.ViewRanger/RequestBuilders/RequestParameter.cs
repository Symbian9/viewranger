using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    public class RequestParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public RequestParameter(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
