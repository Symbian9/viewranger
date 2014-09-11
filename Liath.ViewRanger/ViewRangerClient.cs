using Liath.ViewRanger.Configuration;
using Liath.ViewRanger.RequestBuilders;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger
{
    public class ViewRangerClient : IViewRangerClient
    {
        private static ILog s_log = LogManager.GetLogger(typeof(ViewRangerClient));
        private string _applicationKey;
        private string _baseAddress;

        public const string DefaultApiBaseAddress = @"http://api.viewranger.com/public/v1/";

        /// <summary>
        /// Creates a new ViewRangerClient and loads values from the config files
        /// </summary>
        public ViewRangerClient()
        {
            this.InitialiseFromConfiguration();
        }        

        /// <summary>
        /// Creates a new ViewRangerClient using the specified values
        /// </summary>
        /// <param name="applicationKey">The API key to use to access the service</param>
        /// <param name="baseAddress">The address of the API</param>
        public ViewRangerClient(string applicationKey, string baseAddress = DefaultApiBaseAddress)
        {
            if (applicationKey == null) throw new ArgumentNullException("applicationKey");
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");

            _baseAddress = baseAddress;
            _applicationKey = applicationKey;
        }

        public IGetLastPositionRequest GetLastPosition()
        {
            s_log.Debug("Creating GetLastPositionRequest");
            return new GetLastPositionRequest(_applicationKey, _baseAddress);
        }


        public IGetTrackRequest GetTrack()
        {
            s_log.Debug("Creating GetTrackRequest");
            return new GetTrackRequest(_applicationKey, _baseAddress);
        }

        private void InitialiseFromConfiguration()
        {

            s_log.Debug("Initialising ViewRangerClient from config");
            var section = ConfigurationManager.GetSection("viewRanger") as ViewRangerConfigurationSection;
            if (section != null)
            {
                s_log.Debug("Loaded the 'viewRanger' ConfigurationSection from config");

                if (section.ApplicationKey != null && !string.IsNullOrWhiteSpace(section.ApplicationKey.Key))
                {
                    _applicationKey = section.ApplicationKey.Key;
                }
                else
                {
                    s_log.Error("The element ApplicationKey in the ConfigurationSection 'viewRanger' was not found");
                    throw new ConfigurationErrorsException("The element ApplicationKey in the ConfigurationSection 'viewRanger' was not found");
                }

                if (section.BaseAddress != null && !string.IsNullOrWhiteSpace(section.BaseAddress.Url))
                {
                    s_log.DebugFormat("BaseAddress element found, setting the API Address to '{0}'", section.BaseAddress.Url);
                    _baseAddress = section.BaseAddress.Url;
                }
                else
                {
                    s_log.DebugFormat("BaseAddress element not found, setting the API Address to '{0}'", DefaultApiBaseAddress);
                    _baseAddress = DefaultApiBaseAddress;
                }
            }
            else
            {
                s_log.Error("The ConfigurationSection 'viewRanger' was not found");
                throw new ConfigurationErrorsException("The ConfigurationSection 'viewRanger' was not found");
            }
        }
    }
}
