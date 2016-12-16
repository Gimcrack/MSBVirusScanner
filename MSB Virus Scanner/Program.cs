using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public static NameValueCollection config = ConfigurationManager.AppSettings;
        
        public static string mode;  // Scanner | Sentry

        public static Boolean unattended;

        public static Scanner scanner;

        public static Sentry sentry;

        public static Responder responder;

        public static LogHandler log;

        public static Service.MSB_Virus_Sentry MVS;

        public static int debug = Int32.Parse(config["debug"]);

        public static bool database_logging = Convert.ToBoolean(config["database_enabled"]);

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // set up the log handler
            log = new LogHandler();

            // get the mode of operation
            mode = GetMode(args);

            // init responder
            responder = new Responder();

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

            HandleMenuChoice(userChoice);
        }

        private static void HandleMenuChoice( int userChoice )
        {
            switch (userChoice)
            {
                case 1: mode = "Scanner"; break;
                case 2: mode = "Sentry"; break;
                case 3: mode = "Install"; break;
                case 4: mode = "Uninstall"; break;
                case 5:
                    Configure();
                    ShowMenu();
                    return;

                case 6:
                    Usage();
                    ShowMenu();
                    return;

                case 7:
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

            if ( IsServiceInstalled() )
            {
                Console.WriteLine("Service Already Installed");
                Console.WriteLine("Press <enter> to continue...");
                Console.ReadLine();
                ShowMenu();
                return;
            }

            SelfInstaller.InstallMe();


            // don't display the form to 
            if ( unattended || Environment.UserInteractive )
            {
                Configure();
            }
            

            Console.WriteLine("Starting Windows Service");
            
            StartService();

            Console.WriteLine("Service Started");

            Console.WriteLine("Press <enter> to continue...");

            Console.ReadLine();
            ShowMenu();
        }

        private static void Configure()
        {
            StopService();
            Config.ShowForm();
            StartService();
        }

        private static void Uninstall()
        {
            if ( ! IsServiceInstalled() )
            {
                Console.WriteLine("Service Already Uninstalled");
                Console.WriteLine("Press <enter> to continue...");
                Console.ReadLine();
                ShowMenu();
                return;
            }

            StopService(); 
            

            SelfInstaller.UninstallMe();

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
            ShowMenu();
        }

        private static void Usage()
        {
            Menu.Usage();
        }

        public static Boolean IsServiceRunning()
        {
            if (!IsServiceInstalled()) return false;
            
            ServiceController sc = new ServiceController("MSB_Virus_Sentry");
            return (sc != null && sc.Status == ServiceControllerStatus.Running);
        }

        public static Boolean IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == "MSB_Virus_Sentry");
        }

        private static void StopService()
        {
            if (!IsServiceInstalled()) return;

            ServiceController sc = new ServiceController("MSB_Virus_Sentry");
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                Program.log.Write("Stopping Service");
                sc.Stop();

                while (sc.Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
            }
        }

        private static void StartService()
        {
            if (!IsServiceInstalled()) return;

            ServiceController sc = new ServiceController("MSB_Virus_Sentry");
            if ( ! IsServiceRunning() )
            {
                sc.Start();

                while (sc.Status != ServiceControllerStatus.Running)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
            }
        }

        private static void Service()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                MVS = new Service.MSB_Virus_Sentry()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void Sentry()
        {
            sentry = new Sentry();
        }


        private static void Scanner()
        {
            log.Add(new Logger());
            
            scanner = new Scanner();
            scanner.Scan();

            if (scanner.infected)
            {
                responder.respond(scanner);
            }

            else
            {
                log.CleanUp();
            }
        }

        public static void Scan()
        {
            Scanner();
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

                    case "install":
                        unattended = true;
                        return "Install";

                    case "uninstall":
                        unattended = true;
                        return "Uninstall";
                }
            }

            unattended = false;
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
