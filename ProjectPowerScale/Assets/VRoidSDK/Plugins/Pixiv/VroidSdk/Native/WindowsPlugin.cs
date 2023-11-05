using System;
using System.Runtime.InteropServices;

namespace Pixiv.VroidSdk.Native
{
#if UNITY_EDITOR_WIN || (!UNITY_EDITOR && UNITY_STANDALONE_WIN)
    // ref https://stackoverflow.com/a/56961044
    public sealed class WindowsPlugin
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        const int ALT = 0xA4;
        const int EXTENDEDKEY = 0x1;
        const int KEYUP = 0x2;

        public static void SetApplicationFocus(IntPtr hWnd)
        {
            // Simulate alt press
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

            // Simulate alt release
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

            SetForegroundWindow(hWnd);
        }
    }
#endif
}
