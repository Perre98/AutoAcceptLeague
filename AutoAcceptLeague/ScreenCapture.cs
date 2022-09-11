using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RustCodeRaider
{
    public class ScreenCapture
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        public static void ActivateApp(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);

            // Activate the first application we find with this name
            if (p.Count() > 0)
                SetForegroundWindow(p[0].MainWindowHandle.ToInt32());
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }

        public static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        //OPS MODIFIED FOR LEAGUE AUTOACCEPT !!!
        public static bool isOnScreen(Bitmap screen, Bitmap bmp)
        {
            int tolerance = 80;
            bool returnValue = true;
            screen.Save(Environment.CurrentDirectory + "\\isonscreen.png");
            bmp.Save(Environment.CurrentDirectory + "bmp.png");
            for (int x = 0; x < screen.Width; x++)
            {
                for (int y = 0; y < screen.Height; y++)
                {
                    Color toCheckPixel = bmp.GetPixel(x, y);
                    Color screenPixel = screen.GetPixel(x, y);
                    if (toCheckPixel.A == 0
                       && toCheckPixel.R == 0
                       && toCheckPixel.G == 0
                       && toCheckPixel.B == 0)
                    {

                    }
                    else
                    {
                        if (!(toCheckPixel.R > screenPixel.R - tolerance && toCheckPixel.R < screenPixel.R + tolerance
                            && toCheckPixel.G > screenPixel.G - tolerance && toCheckPixel.G < screenPixel.G + tolerance
                            && toCheckPixel.B > screenPixel.B - tolerance && toCheckPixel.B < screenPixel.B + tolerance))
                        {
                            returnValue = false;
                        }
                    }
                }
            }
            return returnValue;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);


        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);


        public static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }
            
            return result;
        }
    }
}
