using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace MSB_Virus_Scanner
{
    public partial class ConfigForms : Form
    {
        private NameValueCollection config = ConfigurationManager.AppSettings;

        private Boolean saved;

        public ConfigForms()
        {
            InitializeComponent();

            saved = false;

            ReadConfig();

            this.FormClosing += ConfigForms_FormClosing;
        }

        void ConfigForms_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display a MsgBox asking the user to save changes or abort.
            if (! saved && MessageBox.Show("Do you want to save changes?", "Configuration",
               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveConfig();
            }
        }

        private void ReadConfig()
        {
            //general settings
            this.patterns.Text = String.Join(Environment.NewLine, config["patterns"].ToString().Split('|'));
            this.whitelist.Text = String.Join(Environment.NewLine, config["whitelist"].ToString().Split('|'));

            switch( config["action"].ToString().ToLower() )
            {
                case "alert": this.action.SelectedIndex = 0; break;
                case "disconnect": this.action.SelectedIndex = 1; break;
                case "shutdown": this.action.SelectedIndex = 2; break;
            }

            switch (config["action_on_find"].ToString().ToLower())
            {
                case "stop": this.action_on_find.SelectedIndex = 0; break;
                case "continue": this.action_on_find.SelectedIndex = 1; break;
            }
            
            //debug
            this.debug.Checked = (config["debug"].ToString() == "1");
            this.debug_email.Text = config["debug_email"].ToString();

            //email settings
            this.email_enabled.Checked = Convert.ToBoolean( config["email_enabled"] );
            this.email_to.Text = String.Join(Environment.NewLine, config["email_to"].ToString().Split('|'));
            this.email_from.Text = config["email_from"].ToString();
            this.email_server.Text = config["email_server"].ToString();
            this.email_username.Text = config["email_username"].ToString();
            this.email_password.Text = config["email_password"].ToString();
            this.email_port.Text = config["email_port"].ToString();

            //database settings
            this.database_enabled.Checked = Convert.ToBoolean(config["database_enabled"]);
            this.database_name.Text = config["database_name"].ToString();
            this.database_server.Text = config["database_server"].ToString();
            this.database_username.Text = config["database_username"].ToString();
            this.database_password.Text = config["database_password"].ToString();

            //slack settings
            this.slack_enabled.Checked = Convert.ToBoolean( config["slack_enabled"] );
            this.slack_webhook.Text = config["slack_hook"].ToString();
        }

        private void SaveConfig()
        {

            SetSetting("patterns", String.Join("|", this.patterns.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            SetSetting("whitelist", String.Join("|", this.whitelist.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            switch ( this.action.SelectedIndex )
            {
                case 0: SetSetting("action","alert"); break;
                case 1: SetSetting("action","disconnect"); break;
                case 2: SetSetting("action","shutdown"); break; 
            }

            switch (this.action_on_find.SelectedIndex)
            {
                case 0: SetSetting("action_on_find","stop"); break;
                case 1: SetSetting("action_on_find","continue"); break;
            }

            SetSetting("debug",(this.debug.Checked) ? "1" : "0");
            SetSetting("debug_email",this.debug_email.Text);

            SetSetting("email_enabled",(this.email_enabled.Checked) ? "true" : "false");
            SetSetting("email_to",String.Join("|", this.email_to.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            SetSetting("email_from",this.email_from.Text);
            SetSetting("email_server",this.email_server.Text);
            SetSetting("email_username",this.email_username.Text);
            SetSetting("email_password",this.email_password.Text);
            SetSetting("email_port",this.email_port.Text);

            SetSetting("database_enabled", (this.database_enabled.Checked) ? "true" : "false");
            SetSetting("database_server", this.database_server.Text);
            SetSetting("database_name", this.database_name.Text);
            SetSetting("database_username", this.database_username.Text);
            SetSetting("database_password", this.database_password.Text);          

            SetSetting("slack_enabled",(this.email_enabled.Checked) ? "true" : "false");
            SetSetting("slack_hook",this.slack_webhook.Text);

            saved = true;
        }

        internal static bool SetSetting(string Key, string Value)
        {
            bool result = false;
            try
            {
                System.Configuration.Configuration config =
                  ConfigurationManager.OpenExeConfiguration(
                                       ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove(Key);
                var kvElem = new KeyValueConfigurationElement(Key, Value);
                config.AppSettings.Settings.Add(kvElem);

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");

                result = true;
            }
            finally
            { }
            return result;
        } // function

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveConfig();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
