using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Configuration;
using System.Security.Principal;
using System.ServiceProcess;


namespace MSB_Virus_Scanner
{
    class Program
    {
        public static string mode;  // Scanner | Sentry

        public static Scanner scanner;

        public static Sentry sentry;

        public static Responder responder;

        public static ILogger log;

        public static Service.MSB_Virus_Sentry MVS;

        public static int debug = Int32.Parse(ConfigurationManager.AppSettings["debug"]);
       
        static void Main(string[] args)
        {
            // get the mode of operation
            mode = GetMode(args);

            // init responder
            responder = new Responder();

            /*
            // make sure this one supersedes any other running ones.
            Process current = Process.GetCurrentProcess();
            
            foreach(Process p in Process.GetProcessesByName(current.ProcessName))
            {
                if (p.Id != current.Id)
                {
                    p.Kill();
                }
            }
             */

            Route();
            
        }

        private static void Route()
        {
            // Handle the different modes of operation
            switch (mode)
            {
                case "Menu":

                    ShowMenu();
                    break;

                case "Service":
                    Service();

                    break;

                case "Sentry":
                    Sentry();

                    break;

                case "Scanner":
                    Scanner();
                    break;

                case "Install":
                    Install();
                    break;

                case "Uninstall":
                    Uninstall();
                    break;

                default:
                    Usage();
                    ShowMenu();
                    break;
            }
        }

        private static void ShowMenu()
        {
            int userChoice = 0;
            bool invalid = false;

            do
            {
                if (userChoice == -1) invalid = true;
                userChoice = Menu.Display(invalid);
            }
            while (userChoice < 1);
 
            switch(userChoice)
            {
                case 1: mode = "Scanner"; break;
                case 2: mode = "Sentry"; break;
                case 3: mode = "Install"; break;
                case 4: mode = "Uninstall"; break;

                case 5: 
                    Usage();
                    ShowMenu();
                    return;
                
                case 6: 
                    Environment.Exit(0); 
                    return;
                
                default: 
                    ShowMenu();
                    return;
            }

            Route();
        }

        private static void Install()
        {

            bool serviceExists = ServiceController.GetServices().Any(s => s.ServiceName == "MSB_Virus_Sentry");

            if ( serviceExists )
            {
                Console.WriteLine("Service Already Installed");
                Console.WriteLine("Press <enter> to continue...");
                Console.ReadLine();
                ShowMenu();
                return;
            }

            SelfInstaller.InstallMe();

            Console.WriteLine("Starting Windows Service");
            ServiceController sc = new ServiceController("MSB_Virus_Sentry");

            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();

                while (sc.Status != ServiceControllerStatus.Running)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
            }
            
            Console.WriteLine("Service Started");

            Console.WriteLine("Press <enter> to continue...");

            Console.ReadLine();
            ShowMenu();
        }

        private static void Uninstall()
        {
            var serviceExists = ServiceController.GetServices().Any(s => s.ServiceName == "MSB_Virus_Sentry");

            if ( ! serviceExists )
            {
                Console.WriteLine("Service Already Uninstalled");
                Console.WriteLine("Press <enter> to continue...");
                Console.ReadLine();
                ShowMenu();
                return;
            }

            ServiceController sc = new ServiceController("MSB_Virus_Sentry");

            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.Stop();

                while (sc.Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
            }
            

            SelfInstaller.UninstallMe();

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
            ShowMenu();
        }

        private static void Usage()
        {
            Menu.Usage();
        }

        private static void Service()
        {
            log = new EventLogger();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                MVS = new Service.MSB_Virus_Sentry()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void Sentry()
        {
            log = new EventLogger();

            sentry = new Sentry();
        }


        private static void Scanner()
        {
            log = new Logger();

            scanner = new Scanner();
            scanner.Scan();

            if (scanner.infected)
            {
                responder.respond(scanner);
            }

            else
            {
                log.tear_down();
            }
        }


        private static string GetMode(string[] args)
        {
            if ( !Environment.UserInteractive ) return "Service";


            if ( args.Length > 0 )
            {
                switch( args[0].Trim( new char[] {'/','-'}).ToLower().Trim() )
                {
                    case "sentry": return "Sentry";

                    case "scanner": return "Scanner";

                    case "install": return "Install";

                    case "uninstall": return "Uninstall";
                }
            }

            return "Menu";
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

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static List<DriveInfo> GetLocalDrives()
        {
            List<DriveInfo> drives = new List<DriveInfo>();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                switch (d.DriveType)
                {
                    case DriveType.Fixed:
                    case DriveType.Removable:
                        drives.Add(d);
                        break;
                }
            }
            return drives;
        }
    }


}
