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

namespace application
{
    static class Listen2MeApplication
    {
        static Listen2MeMediaController widget = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //widget.SizeChanged += new EventHandler(ResizeEventHandler);
            //ResizeEventHandler(new Object(), new EventArgs());
            widget = new Listen2MeMediaController();
            //widget.SetupColors();
            widget.Show();
            widget.Initialize();
            Application.Run(widget);
        }
    }
}
