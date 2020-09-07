using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace listen2me
{
    class Win32Window : IWin32Window
    {
        private IntPtr __handle { get; } = IntPtr.Zero;
        private Rectangle __rect = new Rectangle();

        public Size Size { get { return new Size(__rect.Width, __rect.Height); } }

        IntPtr IWin32Window.Handle => __handle;

        Win32Window(IntPtr handle)
        {
            __handle = handle;
            FetchInfo();
        }

        public void FetchInfo()
        {
            __rect = GetWindowRectangle();
        }

        Rectangle GetWindowRectangle()
        {
            if (__handle == IntPtr.Zero)
                return new Rectangle();

            return Win32ApiHelper.GetWindowRectangle(__handle);
        }

        public void SetAsParentOf(Form form)
        {
            Win32ApiHelper.SetWindowAttributes(form.Handle, (int)GWL.GWL_STYLE, (int)WS.WS_CHILD);
            Win32ApiHelper.SetParentWindow(form.Handle, __handle);
        }

        public event PaintEventHandler Paint;
        public event ContentsResizedEventHandler Resize;

        public static Win32Window FindWindowByRelationTree(IEnumerable<string> tree)
        {
            IntPtr handleToNext = IntPtr.Zero;
            foreach (string treeNode in tree)
            {
                handleToNext = Win32ApiHelper.FindWindowByClassName(handleToNext, treeNode);
                if (handleToNext == IntPtr.Zero)
                    return new Win32Window(IntPtr.Zero);
            }

            return new Win32Window(handleToNext);
        }
    }
}
