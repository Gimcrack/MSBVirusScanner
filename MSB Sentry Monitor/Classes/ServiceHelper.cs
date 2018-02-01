using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Management;
using System.Net.Mail;

namespace MSB_Sentry_Monitor
{
    public partial class ServiceHelper
    {   

        public void CheckService()
        {
            var sc = new ServiceController("MSB_Virus_Sentry");

            if (sc.Status != ServiceControllerStatus.Running)
            {
                StartAgent();
            }
        }

        public void StartAgent()
        {
            RestartWindowsService("MSB_Virus_Sentry");
        }

        public void StopAgent()
        {
            var sc = new ServiceController("MSB_Virus_Sentry");
            if ((sc.Status.Equals(ServiceControllerStatus.Running)) || (sc.Status.Equals(ServiceControllerStatus.StartPending)))
            {
                sc.Stop();
            }
        }

        public void RestartAgent()
        {
            RestartWindowsService("MSB_Virus_Sentry");
        }

        private void RestartWindowsService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            try
            {
                if ((sc.Status.Equals(ServiceControllerStatus.Running)) || (sc.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    sc.Stop();
                }
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch ( Exception e )
            {
                //Program.Events.WriteEntry("Error Restarting MSB_Virus_Sentry \n\n" + e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

    }
}
