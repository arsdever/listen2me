using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace application
{
    class Win32ApiHelper
    {
        internal delegate IntPtr GetMsgProcDelegate(int code, IntPtr wParam, IntPtr lParam);

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [SuppressUnmanagedCodeSecurity, SecurityCritical]
        internal static class SafeNativeMethods
        {

            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            [DllImport("User32.dll", SetLastError = true)]
            internal static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll")]
            internal static extern long SetWindowLongW(IntPtr hWnd, int nIndex, long dwNewLong);

            [DllImport("user32.dll")]
            internal static extern long GetWindowLongW(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            internal static extern long WindowProc(IntPtr hwnd, WM uMsg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            internal static extern IntPtr SetWindowsHookExW(int idHook, IntPtr lpfn, IntPtr hmod, long dwThreadId);

            [DllImport("user32.dll", EntryPoint = "CallNextHookEx")]
            internal static extern IntPtr CallNextHookExW(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern long GetWindowThreadProcessId(IntPtr hWnd, IntPtr pid);

            [DllImport("Kernel32.dll")]
            internal static extern long GetLastError();

            [DllImport("kernel32.dll")]
            internal static extern IntPtr GetModuleHandleA([MarshalAs(UnmanagedType.LPStr)] string moduleName);

            [DllImport("user32.dll")]
            internal static extern bool AnimateWindow(IntPtr handle, int msec, int flags);


            [DllImport("user32.dll")]
            public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
            //[DllImport("user32.dll", SetLastError = true)]
            //public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        }
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        [SecuritySafeCritical]
        public static IntPtr FindWindowByClassName(IntPtr hwndParent, string className)
        {
            return SafeNativeMethods.FindWindowEx(hwndParent, IntPtr.Zero, className, null);
        }

        [SecuritySafeCritical]
        public static Rectangle GetWindowRectangle(IntPtr windowHandle)
        {
            RECT rect;
            new UIPermission(UIPermissionWindow.AllWindows).Demand();
            SafeNativeMethods.GetWindowRect(windowHandle, out rect);
            return rect.ToRectangle();
        }

        [SecuritySafeCritical]
        public static IntPtr SetParentWindow(IntPtr hWndChild, IntPtr hWndNewParent)
        {
            return SafeNativeMethods.SetParent(hWndChild, hWndNewParent);
        }


        [SecuritySafeCritical]
        public static long SetWindowAttributes(IntPtr hWnd, int nIndex, long dwNewLong)
        {
        return SafeNativeMethods.SetWindowLongW(hWnd, nIndex, dwNewLong);
        }

        [SecuritySafeCritical]
        public static long GetWindowAttributes(IntPtr hWnd, int nIndex)
        {
        return SafeNativeMethods.GetWindowLongW(hWnd, nIndex);
        }


        [SecuritySafeCritical]
        public static IntPtr SetWindowsHook(WH idHook, IntPtr lpfn, IntPtr hmod, long dwThreadId)
        {
        return SafeNativeMethods.SetWindowsHookExW((int)idHook, lpfn, hmod, dwThreadId);
        }

        [SecuritySafeCritical]
        public static IntPtr CallNextHook(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam)
        {
            return SafeNativeMethods.CallNextHookExW(hhk, nCode, wParam, lParam);
        }

        [SecuritySafeCritical]
        public static long GetWindowThreadProcessId(IntPtr hWnd, IntPtr pid)
        {
            return SafeNativeMethods.GetWindowThreadProcessId(hWnd, pid);
        }

        [SecuritySafeCritical]
        public static long GetLastError()
        {
            return SafeNativeMethods.GetLastError();
        }

        [SecuritySafeCritical]
        public static IntPtr GetModuleHandle(string moduleName)
        {
            return SafeNativeMethods.GetModuleHandleA(moduleName);
        }

        [SecuritySafeCritical]
        public static bool AnimateWindow(IntPtr handle, int msec, int flags)
        {
            return SafeNativeMethods.AnimateWindow(handle, msec, flags);
        }

        [SecuritySafeCritical]
        public static void Mute(IntPtr handler)
        {
            SafeNativeMethods.SendMessageW(handler, WM_APPCOMMAND, handler,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        [SecuritySafeCritical]
        public static void VolumeDown(IntPtr handler)
        {
            SafeNativeMethods.SendMessageW(handler, WM_APPCOMMAND, handler,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        [SecuritySafeCritical]
        public static void VolumeUp(IntPtr handler)
        {
            SafeNativeMethods.SendMessageW(handler, WM_APPCOMMAND, handler,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }


        //[SecuritySafeCritical]
        //public static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint)
        //{
        //return SafeNativeMethods.MoveWindow(hWnd, X, Y, nWidth, nHeight, bRepaint);
        //}
    }
}
