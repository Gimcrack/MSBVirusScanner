using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace MSB_Virus_Scanner.Service
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            if (IsServiceInstalled())
            {
                RemoveService();
            }

            base.Install(stateSaver);

            StartService();
        }

        private void RemoveService()
        {
            System.ServiceProcess.ServiceInstaller ServiceInstallerObj = new System.ServiceProcess.ServiceInstaller();
            InstallContext Context = new InstallContext(@"C:\temp\MSB_Virus_Sentry.log", null);
            ServiceInstallerObj.Context = Context;
            ServiceInstallerObj.ServiceName = "MSB_Virus_Sentry";
            ServiceInstallerObj.Uninstall(null);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            StopService();

            RemoveService();

            base.Uninstall(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
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
