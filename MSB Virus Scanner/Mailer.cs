using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace MSB_Virus_Scanner
{
    public class Mailer
    {
        public static void mail(string message, string subject)
        {
            string mailServer = ConfigurationManager.AppSettings["email_server"];
            string mailUser = ConfigurationManager.AppSettings["email_username"];
            string mailPass = ConfigurationManager.AppSettings["email_password"];
            string mailFrom = ConfigurationManager.AppSettings["email_from"];
            int mailPort = Int32.Parse(ConfigurationManager.AppSettings["email_port"]);

            string mail_key = ( Program.debug != 1 ) ? "email_to" : "debug_email";

            IEnumerable<string> emails = ConfigurationManager.AppSettings[mail_key].Split('|');

            try
            {
                MailMessage m = new MailMessage();
                SmtpClient smtp = new SmtpClient(mailServer);

                m.From = new MailAddress(mailFrom);

                foreach(string email in emails)
                {
                    m.To.Add(email);
                }

                m.Subject = subject;
                m.Body = message;
                m.IsBodyHtml = true;

                smtp.Port = mailPort;
                smtp.Credentials = new System.Net.NetworkCredential(mailUser, mailPass);
                smtp.EnableSsl = true;

                smtp.Send(m);
                smtp.Dispose();
            }

            catch (Exception e)
            {
                Program.log.Write("Problem Sending Email" + Environment.NewLine + e.Source + Environment.NewLine + e.GetType().ToString() + Environment.NewLine + e.Message + Environment.NewLine + e.HResult.ToString() + Environment.NewLine + "Message" + Environment.NewLine + message + Environment.NewLine + e.StackTrace.ToString());
            }
        }
    }

    
}
