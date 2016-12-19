using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;

namespace MSB_Virus_Scanner.Service
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {    
        public ServiceInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            StartService();
        }

        public override void Uninstall(IDictionary savedState)
        {
            StopService();

            base.Uninstall(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            StopService();

            base.Rollback(savedState);
        }

        public static void StopService()
        {
            if (!IsServiceInstalled()) return;

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
        }

        public static void StartService()
        {
            if (!IsServiceInstalled()) return;

            ServiceController sc = new ServiceController("MSB_Virus_Sentry");
            if (!IsServiceRunning())
            {
                sc.Start();

                while (sc.Status != ServiceControllerStatus.Running)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
            }
        }

        public static Boolean IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == "MSB_Virus_Sentry");
        }

        public static Boolean IsServiceRunning()
        {
            if (!IsServiceInstalled()) return false;

            ServiceController sc = new ServiceController("MSB_Virus_Sentry");
            return (sc != null && sc.Status == ServiceControllerStatus.Running);
        }

    }
}
