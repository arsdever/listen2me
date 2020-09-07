using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;
using System.Drawing;
using System.Security.Permissions;
using System.Reflection;

namespace listen2me
{
    static class Listen2MeApplication
    {
        /// <summary>
        ///  The main entry point for the listen2me.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Listen2MeMediaController widget = new Listen2MeMediaController();
            widget.Show();

            Application.Run(widget);
        }
    }
}
