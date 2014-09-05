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
            var client = new ViewRangerClient(appKey);
            var position = client.GetLastPosition()
                .ForUser(username)
                .Request();
        }
    }
}
