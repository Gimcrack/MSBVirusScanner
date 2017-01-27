using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;

namespace MSB_Virus_Scanner
{
    public class Menu
    {
        public static int Display(bool invalid = false)
        {
            int max_choice = 7;
            int choice = -1;

            string service_text = Program.IsServiceInstalled() ? "Installed" : "Not Installed";
            Color service_color = Program.IsServiceInstalled() ? Color.LimeGreen : Color.IndianRed;

            string running_text = Program.IsServiceRunning() ? "Running" : "Stopped";
            Color running_color = Program.IsServiceRunning() ? Color.LimeGreen : Color.IndianRed;


            Console.Clear();
            Console.WriteLine();
            Console.WriteAscii(" MSB VIRUS ", Color.LightGoldenrodYellow);
            Console.WriteAscii(" *SCANNER* ", Color.LightGoldenrodYellow);


            Console.WriteLineFormatted("Service: [{0}] [{1}] [{2}]",
                Color.Gray,
                new Formatter[] 
                { 
                    new Formatter(service_text,service_color),
                    new Formatter(running_text,running_color),
                    new Formatter(Program.version, Color.Lime)
                }   
            );

            Console.WriteLine();
            Console.WriteLine(" MAIN MENU ", Color.LightCyan);
            Console.WriteLine("  1.) Scanner Mode - Scan this computer now. ", Color.LightBlue);
            Console.WriteLine("  2.) Sentry Mode - Actively monitor this computer for threats.", Color.DodgerBlue);
            Console.WriteLine("  3.) Install Service", Color.DeepSkyBlue);
            Console.WriteLine("  4.) Uninstall Service", Color.Aquamarine);
            Console.WriteLine("  5.) Configure Settings", Color.AliceBlue);
            Console.WriteLine("  6.) Help", Color.Aqua);
            Console.WriteLine("  7.) Exit", Color.CornflowerBlue);
            Console.WriteLine(" Please Enter An Option (1-7). Press <Enter>", Color.LightCyan);
            
            if (invalid)
            {
                Console.WriteLine("    Please Enter A Valid Option", Color.Red);
            }
            

            var key = Console.ReadLine();
            try
            {
                 choice = Convert.ToInt32(key);

                if (choice > max_choice) return -1;
                if (choice < 1) return -1;
            }

            catch( Exception e)
            { }
            
            return choice;
        }

        public static void Usage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteAscii(" MSB VIRUS ", Color.LightGoldenrodYellow);
            Console.WriteAscii("  SCANNER  ", Color.LightGoldenrodYellow);
            Console.WriteLine();
            Console.WriteLine(" USAGE ", Color.LightCyan);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" MSB Virus Scanner.exe /scanner ", Color.LightCyan);
            Console.WriteLine("   Initiate a full scan of the local computer ", Color.LightGoldenrodYellow);
            Console.WriteLine(" MSB Virus Scanner.exe /sentry ", Color.LightCyan);
            Console.WriteLine("   Actively monitor this computer for threats ", Color.LightGoldenrodYellow);
            Console.WriteLine(" MSB Virus Scanner.exe /install ", Color.LightCyan);
            Console.WriteLine("   Install a Windows Service to monitor this computer ", Color.LightGoldenrodYellow);
            Console.WriteLine(" MSB Virus Scanner.exe /uninstall ", Color.LightCyan);
            Console.WriteLine("   Uninstall Windows Service ", Color.LightGoldenrodYellow);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Press <enter> to continue... ", Color.LightCyan);
            Console.ReadLine();
        }
    }
}
