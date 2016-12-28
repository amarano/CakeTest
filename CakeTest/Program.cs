using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace CakeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG
            Console.WriteLine("DEBUG MODE ENABLED! Press any key to start the server...");
            Console.ReadKey();
            #endif
            using (WebApp.Start<Startup>("http://localhost:9000/"))
            {
                Console.WriteLine("Service has started on port 9000");
                Console.ReadLine();
            }
        }
    }
}
