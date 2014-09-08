using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Responses
{
    public class Location
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
    }
}
