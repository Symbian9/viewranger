using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests.RequestTests.SampleResponses
{
    public class SampleResponse
    {
        public static XDocument Empty
        {
            get
            {
                return GetXDocument("Empty.xml");
            }
        }

        public static XDocument Error
        {
            get
            {
                return GetXDocument("Error.xml");
            }
        }

        public static XDocument NoLocations
        {
            get
            {
                return GetXDocument("NoLocations.xml");
            }
        }

        public static XDocument TwoLocations
        {
            get
            {
                return GetXDocument("TwoLocations.xml");
            }
        }

        public static XDocument Successful
        {
            get
            {
                return GetXDocument("Successful.xml");
            }
        }

        public static XDocument GetXDocument(string name)
        {
            return XDocument.Load(string.Concat(@"RequestBuilderTests\GetLastPositionRequestTests\RequestTests\SampleResponses\", name));
        }
    }
}
