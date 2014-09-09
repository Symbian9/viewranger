using Liath.ViewRanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var appKey = @"123456789";
            var username = "user1";
            var pin = "1234";
            var client = new ViewRangerClient(appKey);
            var lastLocation = client.GetLastPosition()
                .ForUser(username, pin)
                .Request();
        }
    }
}
