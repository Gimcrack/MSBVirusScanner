using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSB_Virus_Scanner
{
    public static class Config
    {
        public static ConfigForms Frm;

        public static void ShowForm()
        {
            Application.Run(Frm = new ConfigForms());
        }
    }
}
