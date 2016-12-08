using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Security.Principal;
using System.Management;

namespace MSB_Virus_Scanner
{
    public class Responder
    {
        public string action = ConfigurationManager.AppSettings["action"];

        public SlackClient slack = new SlackClient( ConfigurationManager.AppSettings["slack_hook"] );

        public Scanner scanner;

        public void respond(Scanner scnr)
        {
            this.scanner = scnr;

            if ( Program.debug == 1 )
            {
                debug();
                return;
            }

            switch( action )
            {
                case "alert" :
                    warn();
                    break;

                case "disconnect" :
                    warn();
                    disconnect();

                    break;

                case "shutdown" :
                    warn();
                    shutdown();

                    break;

                default :
                    warn();
                    disconnect();
                    break;
            }
        }

        private void debug() 
        {
            string subject = "TEST -- MSB Infection Found On " + Environment.MachineName + " - " + Environment.UserName;
            string message = "TEST -- Infected Files <br>" + String.Join(@"<br/>", Program.scanner.infected_files); 
            message += Program.log.get();

            Mailer.mail(message, subject);

            slack.PostMessage(text: subject);
        }



        private void warn()
        {

            string subject = "MSB Infection Found On " + Environment.MachineName + " - " + Environment.UserName;
            string message = "Infected Files <br>" + String.Join(@"<br/>", Program.scanner.infected_files);

            Mailer.mail(message, subject);

            slack.PostMessage(text: subject);

            AutoClosingMessageBox.Show("Your computer appears to have a virus infection. Please shut down your computer and call Service Desk immediately at x8553.  Your computer will automatically be disconnected from the network to prevent further infection.", "Virus Infection Found", 15000);
        }

        private void disconnect()
        {
            if ( ! IsAdministrator() )
            {
                shutdown();
                return;
            }

            try
            {
                // kill connections
                SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
                ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    item.InvokeMethod("Disable", null);
                }
            }

            // shutdown the computer if the user cannot disable network connections
            catch (Exception e)
            {
                Program.log.write(e.Message);
            }


        }

        private void shutdown()
        {
            Process.Start("shutdown", "-s -t 300");
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
