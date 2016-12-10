using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.Mail;
using System.Configuration;




namespace MSB_Virus_Scanner
{
    class Program
    {

        public static Scanner scanner = new Scanner();

        public static Responder responder = new Responder();

        public static Logger log = new Logger();

        public static int debug = Int32.Parse(ConfigurationManager.AppSettings["debug"]);
       
        static void Main(string[] args)
        {
            scanner.Scan();

            if (scanner.infected)
            {
                responder.respond(scanner);
            }

        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }


}
