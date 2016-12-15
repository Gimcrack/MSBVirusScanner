using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Management;

namespace MSB_Virus_Scanner
{
    public class Responder
    {
        public string action = ConfigurationManager.AppSettings["action"];

        public Slack.Client slack = new Slack.Client( ConfigurationManager.AppSettings["slack_hook"] );

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

            this.scanner.Reset();
        }

        private void debug() 
        {
            string subject = string.Format("TEST -- MSB Infection Found On {0} - {1}",
                Environment.MachineName,
                Environment.UserName
            );
            
            string message = string.Format("TEST -- Infected Files <br> {0} Pattern: {1} {2}<br/>{3} <br/> {4}",
                Environment.NewLine,
                scanner.matched_pattern,
                Environment.NewLine,
                String.Join(@"<br/>" + Environment.NewLine, scanner.infected_files),
                Program.log.get()
            );

            Mailer.mail(message, subject);

            sendSlack();
        }

        private void sendSlack()
        {
            Slack.Attachment a = new Slack.Attachment()
            {
                Title = "MSB Virus Scanner Results",
            };

            a.AddField("Computer", Environment.MachineName);
            a.AddField("User", Environment.UserName);
            a.AddField("IP", Program.GetLocalIPAddress());
            a.AddField("Pattern", scanner.matched_pattern);
            a.AddField("Files", String.Join(Environment.NewLine, scanner.infected_files), false);

            Slack.Payload p = new Slack.Payload()
            {
                Text = ( Program.debug == 1 ) ? "TEST -- MSB Infection Found" : "MSB Infection Found"
            };

            p.Attach(a);

            slack.PostMessage(p);
        }



        private void warn()
        {
            int display_message_length_ms = 15000; // length of time to display the message to the user.

            string subject = string.Format("MSB Infection Found On {0} - {1}",
                Environment.MachineName,
                Environment.UserName
            );

            string message = string.Format("Infected Files <br> {0} Pattern: {1} {2}<br/>{3}",
                Environment.NewLine,
                scanner.matched_pattern,
                Environment.NewLine,
                String.Join(@"<br/>" + Environment.NewLine, scanner.infected_files)
            );

            Mailer.mail(message, subject);

            Console.WriteLine(message);

            sendSlack();

            AutoClosingMessageBox.Show( string.Format("{0} {1} {2} {3}",
                    "Your computer appears to have a virus infection",
                    Environment.NewLine + Environment.NewLine,
                    "Please shut down your computer and call Service Desk immediately at x8553.",
                    "Your computer will be disconnected from the network to prevent further infection."
                ), 
                "Virus Infection Found", 
                display_message_length_ms
            );
        }

        private void disconnect()
        {
            if ( ! Program.IsAdministrator() )
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
    }
}
