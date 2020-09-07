using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace listen2me
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Point
    {
        long X { get; }
        long Y { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Message
    {
        public IntPtr WindowHandle;
        public int MessageIdentifier;
        public IntPtr WParam;
        public IntPtr LParam;
        public long Time;
        public Point Point;
        public long Private;
    }
}
