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
using System.Runtime.InteropServices;


namespace MSB_Virus_Scanner
{
    class Program
    {
        public static string version = typeof(Program).Assembly.GetName().Version.ToString();

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
        public static void Main(string[] args)
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

                case "Exit":
                    Environment.Exit(0);
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

        public static void Install()
        {

            if ( IsServiceInstalled() )
            {
                
                if (unattended)
                {
                    Console.WriteLine("Service Already Installed... Uninstalling...");
                    SelfInstaller.UninstallMe();
                }

                else
                {
                    Console.WriteLine("Service Already Installed");
                    Console.WriteLine("Press <enter> to continue...");
                    Console.ReadLine();
                    ShowMenu();

                    return;
                }
            }

            Console.WriteLine("Installing Windows Service");
            SelfInstaller.InstallMe();


            // don't display the form to 
            if ( ! unattended && Environment.UserInteractive )
            {
                Configure();
            }
            

            Console.WriteLine("Starting Windows Service");
            
            StartService();

            Console.WriteLine("Service Started");

            if ( unattended ) return;


            
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

        public static void Uninstall()
        {
            if ( ! IsServiceInstalled() )
            {
                if (unattended) return; 

                Console.WriteLine("Service Already Uninstalled");
                Console.WriteLine("Press <enter> to continue...");
                Console.ReadLine();
                ShowMenu();
                return;
            }

            StopService();

            SelfInstaller.UninstallMe();

            if (unattended) return;
            
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

        public static void StopService()
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

        public static void StartService()
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
            if ( args.Length > 0 )
            {
                switch (args[0].Trim(new char[] { '/', '-' }).ToLower().Trim())
                {
                    case "sentry": return "Sentry";

                    case "scanner": return "Scanner";

                    case "install":
                        unattended = true;
                        Install();
                        return "Exit";

                    case "uninstall":
                        unattended = true;
                        Uninstall();
                        return "Exit";

                    case "stop":
                        StopService();
                        Environment.Exit(0);
                        break;

                    case "warn": // warn the user
                        IntPtr winHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                        ShowWindow(winHandle, 2); // minimize the window
                        Responder.warn();
                        Environment.Exit(0);
                        break;
                }
 
            }

            if (!Environment.UserInteractive) return "Service";

            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                if (identity.IsSystem) return "Service";
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

        [DllImport( "user32.dll" )]
        public static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );
    }


}
