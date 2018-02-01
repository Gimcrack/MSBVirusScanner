using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace MSB_Sentry_Monitor
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            try
            {
                base.Uninstall(stateSaver);

            }
            catch
            { }
            
            base.Install(stateSaver);

            StartService();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public static void StopService()
        {
            if (!IsServiceInstalled() || (!IsServiceRunning())) return;

            ServiceController sc = new ServiceController("MSB_Sentry_Monitor");
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

            ServiceController sc = new ServiceController("MSB_Sentry_Monitor");
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
            return ServiceController.GetServices().Any(s => s.ServiceName == "MSB_Sentry_Monitor");
        }

        public static Boolean IsServiceRunning()
        {
            if (!IsServiceInstalled()) return false;

            ServiceController sc = new ServiceController("MSB_Sentry_Monitor");
            return (sc != null && sc.Status == ServiceControllerStatus.Running);
        }
    }
}
