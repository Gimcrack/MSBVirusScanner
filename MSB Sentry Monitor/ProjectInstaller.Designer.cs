using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Threading;

namespace MSB_Sentry_Monitor
{
    public partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.BeforeInstall += new System.Configuration.Install.InstallEventHandler(ProjectInstaller_BeforeInstall);
            this.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(ProjectInstaller_BeforeUninstall);
            


            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "MSB Sentry Monitor";
            this.serviceInstaller1.DisplayName = "MSB Sentry Monitor";
            this.serviceInstaller1.ServiceName = "MSB_Sentry_Monitor";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.AfterInstall += ProjectInstaller_AfterInstall;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion



        public void ProjectInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            using (ServiceController s = new ServiceController(serviceInstaller1.ServiceName))
            {
                if (s.Status != ServiceControllerStatus.Running)
                {
                    s.Start();

                    while (s.Status != ServiceControllerStatus.Running)
                    {
                        Console.WriteLine("Waiting for service to start - status {0}", s.Status.ToString());
                        Thread.Sleep(1000);
                        s.Refresh();
                    }
                }
            }
        }

        public void ProjectInstaller_BeforeUninstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            List<ServiceController> services = new List<ServiceController>(ServiceController.GetServices());

            foreach (ServiceController s in services)
            {
                if (s.ServiceName == this.serviceInstaller1.ServiceName)
                {
                    if (s.Status != ServiceControllerStatus.Stopped)
                    {
                        s.Stop();

                        while (s.Status != ServiceControllerStatus.Stopped)
                        {
                            Thread.Sleep(1000);
                            s.Refresh();
                        }
                    }

                    break;
                }
            }
        }

        public void ProjectInstaller_BeforeInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            List<ServiceController> services = new List<ServiceController>(ServiceController.GetServices());

            foreach (ServiceController s in services)
            {
                if (s.ServiceName == this.serviceInstaller1.ServiceName)
                {
                    if (s.Status != ServiceControllerStatus.Stopped)
                    {
                        s.Stop();

                        while (s.Status != ServiceControllerStatus.Stopped)
                        {
                            Thread.Sleep(1000);
                            s.Refresh();
                        }
                    }

                    ServiceInstaller ServiceInstallerObj = new ServiceInstaller();
                    ServiceInstallerObj.Context = new System.Configuration.Install.InstallContext();
                    ServiceInstallerObj.Context = Context;
                    ServiceInstallerObj.ServiceName = "MSB_Sentry_Monitor";
                    ServiceInstallerObj.Uninstall(null);

                    break;
                }
            }
        }
    }
}