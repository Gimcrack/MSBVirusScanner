using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Management;
using System.Timers;

namespace MSB_Virus_Scanner
{
    public class Responder
    {
        public static int display_message_length_ms = 15000; // length of time to display the message to the user.

        public string action = ConfigurationManager.AppSettings["action"];

        public Slack.Client slack = new Slack.Client( ConfigurationManager.AppSettings["slack_hook"] );

        private Boolean slack_frozen;

        private Boolean mail_frozen;

        public Scanner scanner;

        public void respond(Scanner scnr)
        {
            this.scanner = scnr;

            if ( Program.debug == 1 )
            {
                debug();
                return;
            }

            warn();
            notify();

            switch( action )
            {
                case "alert" :
                    break;

                case "disconnect" :
                    disconnect();

                    break;

                case "shutdown" :
                    shutdown();

                    break;

                default :
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
                Program.log.Get()
            );

            sendDashboard();

            sendMail(message, subject);

            sendSlack();

            this.scanner.Reset();
        }

        private void sendDashboard()
        {
            foreach(string file in scanner.infected_files)
            {
                Program.dashboard.MatchedFile(file, scanner.matched_pattern);
            }
        }

        private void sendSlack()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["slack_enabled"])) return; // slack not enabled

            if (slack_frozen) return; // wait until slack is allowed again;

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

            freezeSlack();
        }

        private void freezeSlack()
        {
            slack_frozen = true;

            Timer SlackTimer = new Timer()
            {
                Interval = 15 * 60 * 1000, // freeze for 15 min
                Enabled = true,
            };

            SlackTimer.Elapsed += (object sender, ElapsedEventArgs a) =>
            {
                slack_frozen = false;
            };
        }

        private void freezeMail()
        {
            mail_frozen = true;

            Timer MailTimer = new Timer()
            {
                Interval = 15 * 60 * 1000, // freeze for 15 min
                Enabled = true,
            };

            MailTimer.Elapsed += (object sender, ElapsedEventArgs a) =>
            {
                mail_frozen = false;
            };
        }

        public static void warn()
        {
            if ( ! Environment.UserInteractive )
            {
                ApplicationLauncher.CreateProcessInConsoleSession(
                    System.Reflection.Assembly.GetEntryAssembly().Location + " /warn", 
                    true // elevate
                );

                return;
            }

            AutoClosingMessageBox.Show(string.Format("{0} {1} {2} {3}",
                     "Your computer appears to have a virus infection",
                     Environment.NewLine + Environment.NewLine,
                     "Please shut down your computer and call Service Desk immediately at x8553.",
                     "Your computer will be disconnected from the network to prevent further infection."
                 ),
                 "Virus Infection Found",
                 display_message_length_ms
             );
        }

        private void notify()
        {
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

            sendDashboard();

            sendMail(message, subject);

            sendSlack();

            Console.WriteLine(message);
 
        }

        private void sendMail(string message, string subject)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["mail_enabled"])) return; // mail not enabled

            if (mail_frozen) return; // wait until mail is allowed again;
            
            Mailer.mail(message, subject);

            freezeMail();
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
                Program.log.Write(String.Format("Killing Network Connections On {0}", Environment.MachineName));
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
                Program.log.Write(String.Format("Error Disconnecting Computer From Network: \n\r {0}",e.Message));
            }
        }

        private void shutdown()
        {
            Program.log.Write(String.Format("Shuting Down {0}", Environment.MachineName));
            Process.Start("shutdown", "-s -t 300");
        }
    }
}
