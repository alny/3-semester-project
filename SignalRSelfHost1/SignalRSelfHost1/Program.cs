using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSelfHost1 {
    public class Program {
        static void Main(string[] args) {
            string url = "http://localhost:8080";
            using (WebApp.Start(url)) {
                Console.WriteLine($"Server Running on {url}");
                Console.ReadLine();
            }
        }
    }
}
