using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;

namespace MSB_Virus_Scanner
{
    public class Menu
    {
        public static int Display(bool invalid = false)
        {
            int max_choice = 6;

            Console.Clear();
            Console.WriteLine();
            Console.WriteAscii(" MSB VIRUS ", Color.LightGoldenrodYellow);
            Console.WriteAscii("  SCANNER  ", Color.LightGoldenrodYellow);
            Console.WriteLine();
            Console.WriteLine(" MAIN MENU ", Color.LightCyan);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  1.) Scanner Mode - Scan this computer now. ", Color.LightBlue);
            Console.WriteLine("  2.) Sentry Mode - Actively monitor this computer for threats.", Color.DodgerBlue);
            Console.WriteLine("  3.) Install Windows Service to continually monitor this computer.", Color.DeepSkyBlue);
            Console.WriteLine("  4.) Uninstall Windows Service.", Color.Aquamarine);
            Console.WriteLine("  5.) Display Usage - Get Help.", Color.Aqua);
            Console.WriteLine("  6.) Exit", Color.CornflowerBlue);
            Console.WriteLine();
            Console.WriteLine("  Please Enter An Option (1-3). Press <Enter>", Color.LightCyan);
            
            if (invalid)
            {
                Console.WriteLine("    Please Enter A Valid Option", Color.Red);
            }
            

            var key = Console.ReadLine();
            try
            {
                int choice = Convert.ToInt32(key);

                if (choice > max_choice) return -1;
                if (choice < 1) return -1;

                return choice;
            }
            catch(Exception e)
            {
                return -1;
            }
            
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
