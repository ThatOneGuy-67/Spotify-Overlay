using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SpotifyOverlay.Services
{
    public class HotkeyService
    {
        private HwndSource? source;

        public event Action<int>? HotkeyPressed;

        public void Register(Window window)
        {
            source = (HwndSource)PresentationSource.FromVisual(window)!;

            source.AddHook(HwndHook);

            RegisterHotKey(source.Handle, 1, 0, 0x70); // F1
            RegisterHotKey(source.Handle, 2, 0, 0x71); // F2
            RegisterHotKey(source.Handle, 3, 0, 0x72); // F3
            RegisterHotKey(source.Handle, 4, 0, 0x73); // F4
            RegisterHotKey(source.Handle, 5, 0, 0x74); // F5
            RegisterHotKey(source.Handle, 6, 0, 0x75); // F6
            RegisterHotKey(source.Handle, 7, 0, 0x76); // F7
            RegisterHotKey(source.Handle, 8, 0, 0x77); // F8
        }

        public void Unregister()
        {
            if (source == null)
                return;

            for (int i = 1; i <= 8; i++)
                UnregisterHotKey(source.Handle, i);
        }

        private IntPtr HwndHook(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY)
            {
                HotkeyPressed?.Invoke(wParam.ToInt32());
                handled = true;
            }

            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(
            IntPtr hWnd,
            int id,
            uint fsModifiers,
            uint vk);

        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id);
    }
}