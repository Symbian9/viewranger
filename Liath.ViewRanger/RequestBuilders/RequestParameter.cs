using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    /// <summary>
    /// A parameter to send to ViewRanger
    /// </summary>
    public class RequestParameter
    {
        /// <summary>
        /// The QueryString key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value to pass
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new RequestParameter
        /// </summary>
        /// <param name="key">The key for the parameter</param>
        /// <param name="value">The value to supply</param>
        public RequestParameter(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
