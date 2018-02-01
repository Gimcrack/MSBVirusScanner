namespace MSB_Virus_Scanner
{
    partial class ConfigForms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label11;
            this.label1 = new System.Windows.Forms.Label();
            this.email_to = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.email_from = new System.Windows.Forms.TextBox();
            this.email_server = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.email_username = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.email_password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.email_port = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.email_enabled = new System.Windows.Forms.CheckBox();
            this.slack_enabled = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.slack_webhook = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.whitelist = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.patterns = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.debug_email = new System.Windows.Forms.TextBox();
            this.debug = new System.Windows.Forms.CheckBox();
            this.action_on_find = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.action = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.database_enabled = new System.Windows.Forms.CheckBox();
            this.api_base_url = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Send To These Email Addresses (One Per Line)";
            // 
            // email_to
            // 
            this.email_to.AcceptsReturn = true;
            this.email_to.Location = new System.Drawing.Point(6, 59);
            this.email_to.Multiline = true;
            this.email_to.Name = "email_to";
            this.email_to.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.email_to.Size = new System.Drawing.Size(337, 61);
            this.email_to.TabIndex = 1;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(477, 536);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(396, 535);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Email From Address";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // email_from
            // 
            this.email_from.Location = new System.Drawing.Point(6, 144);
            this.email_from.Name = "email_from";
            this.email_from.Size = new System.Drawing.Size(337, 20);
            this.email_from.TabIndex = 5;
            // 
            // email_server
            // 
            this.email_server.Location = new System.Drawing.Point(6, 184);
            this.email_server.Name = "email_server";
            this.email_server.Size = new System.Drawing.Size(337, 20);
            this.email_server.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Email Server";
            // 
            // email_username
            // 
            this.email_username.Location = new System.Drawing.Point(6, 223);
            this.email_username.Name = "email_username";
            this.email_username.Size = new System.Drawing.Size(337, 20);
            this.email_username.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Email Username";
            // 
            // email_password
            // 
            this.email_password.Location = new System.Drawing.Point(6, 262);
            this.email_password.Name = "email_password";
            this.email_password.PasswordChar = '*';
            this.email_password.Size = new System.Drawing.Size(337, 20);
            this.email_password.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Email Password";
            // 
            // email_port
            // 
            this.email_port.Location = new System.Drawing.Point(6, 303);
            this.email_port.Name = "email_port";
            this.email_port.Size = new System.Drawing.Size(337, 20);
            this.email_port.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 286);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Email Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.email_enabled);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.email_port);
            this.groupBox1.Controls.Add(this.email_to);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.email_password);
            this.groupBox1.Controls.Add(this.email_from);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.email_username);
            this.groupBox1.Controls.Add(this.email_server);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(393, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 331);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email Alerts";
            // 
            // email_enabled
            // 
            this.email_enabled.AutoSize = true;
            this.email_enabled.Location = new System.Drawing.Point(6, 19);
            this.email_enabled.Name = "email_enabled";
            this.email_enabled.Size = new System.Drawing.Size(65, 17);
            this.email_enabled.TabIndex = 18;
            this.email_enabled.Text = "Enabled";
            this.email_enabled.UseVisualStyleBackColor = true;
            // 
            // slack_enabled
            // 
            this.slack_enabled.AutoSize = true;
            this.slack_enabled.Location = new System.Drawing.Point(6, 19);
            this.slack_enabled.Name = "slack_enabled";
            this.slack_enabled.Size = new System.Drawing.Size(65, 17);
            this.slack_enabled.TabIndex = 15;
            this.slack_enabled.Text = "Enabled";
            this.slack_enabled.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Webhook";
            // 
            // slack_webhook
            // 
            this.slack_webhook.Location = new System.Drawing.Point(7, 55);
            this.slack_webhook.Name = "slack_webhook";
            this.slack_webhook.Size = new System.Drawing.Size(336, 20);
            this.slack_webhook.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.slack_enabled);
            this.groupBox2.Controls.Add(this.slack_webhook);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(393, 366);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 90);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Slack Alerts";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.whitelist);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.patterns);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.debug_email);
            this.groupBox3.Controls.Add(this.debug);
            this.groupBox3.Controls.Add(this.action_on_find);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.action);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(12, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(375, 385);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Settings";
            // 
            // whitelist
            // 
            this.whitelist.AcceptsReturn = true;
            this.whitelist.Location = new System.Drawing.Point(7, 112);
            this.whitelist.Multiline = true;
            this.whitelist.Name = "whitelist";
            this.whitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.whitelist.Size = new System.Drawing.Size(359, 52);
            this.whitelist.TabIndex = 10;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 95);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(179, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "White-Listed Patterns (One Per Line)";
            // 
            // patterns
            // 
            this.patterns.AcceptsReturn = true;
            this.patterns.Location = new System.Drawing.Point(7, 34);
            this.patterns.Multiline = true;
            this.patterns.Name = "patterns";
            this.patterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.patterns.Size = new System.Drawing.Size(359, 54);
            this.patterns.TabIndex = 8;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(237, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Additional Patterns To Search For (One Per Line)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 334);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Send Debug Email To";
            // 
            // debug_email
            // 
            this.debug_email.Location = new System.Drawing.Point(7, 353);
            this.debug_email.Name = "debug_email";
            this.debug_email.Size = new System.Drawing.Size(359, 20);
            this.debug_email.TabIndex = 5;
            // 
            // debug
            // 
            this.debug.AutoSize = true;
            this.debug.Location = new System.Drawing.Point(10, 310);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(124, 17);
            this.debug.TabIndex = 4;
            this.debug.Text = "Enable Debug Mode";
            this.debug.UseVisualStyleBackColor = true;
            // 
            // action_on_find
            // 
            this.action_on_find.FormattingEnabled = true;
            this.action_on_find.Items.AddRange(new object[] {
            "Stop",
            "Continue"});
            this.action_on_find.Location = new System.Drawing.Point(9, 267);
            this.action_on_find.Name = "action_on_find";
            this.action_on_find.Size = new System.Drawing.Size(359, 30);
            this.action_on_find.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 250);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Scanner Mode - Action On Find";
            // 
            // action
            // 
            this.action.FormattingEnabled = true;
            this.action.Items.AddRange(new object[] {
            "Alert - Notify the user and send alerts",
            "Alert and Disconnect - Disconnect the user from the network",
            "Alert and Shutdown - Shutdown the computer after 5 minutes"});
            this.action.Location = new System.Drawing.Point(9, 200);
            this.action.Name = "action";
            this.action.Size = new System.Drawing.Size(359, 43);
            this.action.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Mitigation Action";
            // 
            // database_enabled
            // 
            this.database_enabled.AutoSize = true;
            this.database_enabled.Location = new System.Drawing.Point(7, 20);
            this.database_enabled.Name = "database_enabled";
            this.database_enabled.Size = new System.Drawing.Size(167, 17);
            this.database_enabled.TabIndex = 0;
            this.database_enabled.Text = "Enable Dashboard Interaction";
            this.database_enabled.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(9, 47);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(56, 13);
            label11.TabIndex = 1;
            label11.Text = "Base URL";
            // 
            // api_base_url
            // 
            this.api_base_url.Location = new System.Drawing.Point(66, 44);
            this.api_base_url.Name = "api_base_url";
            this.api_base_url.Size = new System.Drawing.Size(302, 20);
            this.api_base_url.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.api_base_url);
            this.groupBox4.Controls.Add(label11);
            this.groupBox4.Controls.Add(this.database_enabled);
            this.groupBox4.Location = new System.Drawing.Point(12, 404);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(374, 81);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "API Dashboard Settings";
            // 
            // ConfigForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 567);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Name = "ConfigForms";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MSB Virus Scanner Config";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox email_to;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox email_from;
        private System.Windows.Forms.TextBox email_server;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox email_username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox email_password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox email_port;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox slack_webhook;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox slack_enabled;
        private System.Windows.Forms.CheckBox email_enabled;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox debug;
        private System.Windows.Forms.ListBox action_on_find;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox action;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox debug_email;
        private System.Windows.Forms.TextBox whitelist;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox patterns;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox database_enabled;
        private System.Windows.Forms.TextBox api_base_url;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}