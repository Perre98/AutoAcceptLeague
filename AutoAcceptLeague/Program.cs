// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsInput;

namespace RustCodeRaider
{
    public class AutoAccept
    {
        static InputSimulator iss = new InputSimulator();
        private const int SECONDS_TO_RETRY = 15;
        public static void Main()
        {
            List<string> res = Regex.Split( File.ReadAllText(Application.StartupPath + "\\settings.txt"), "\n").ToList();
            IntPtr leagueHandle;
            Stopwatch toExit = new Stopwatch();
            Stopwatch toOpenLeague = new Stopwatch();
            toOpenLeague.Start();
            Bitmap toLook = (Bitmap)Image.FromFile(Application.StartupPath + "\\toLook\\toLookAccept" + res[0] + res[1] + ".png");
            while (true)
            {
                leagueHandle = ScreenCapture.FindWindow(null, "League of Legends");
                
                ScreenCapture.GetWindowRect(leagueHandle, out Rectangle rect);
                if (toOpenLeague.Elapsed.Seconds > 4)
                {
                    ScreenCapture.SetForegroundWindow(leagueHandle.ToInt32());
                    toOpenLeague.Restart();
                }
                
                if (rect.Width > 700 && rect.Height > 500)
                {
                    leagueHandle = ScreenCapture.FindWindow(null, "League of Legends");
                    
                    using (Bitmap map = ScreenCapture.CaptureWindow(leagueHandle))
                    {
                        
                        if (int.Parse(res[0]) == map.Height
                        && int.Parse(res[1]) == map.Width
                        && ScreenCapture.isOnScreen(map, toLook))
                        {
                            iss.Mouse.MoveMouseTo((int)((rect.X * (65535 / Screen.PrimaryScreen.Bounds.Width)) + (map.Width * 0.5f) * (65535 / Screen.PrimaryScreen.Bounds.Width)), (int)(rect.Y * (65535 / Screen.PrimaryScreen.Bounds.Height)) + ((map.Height * 0.77f) * (65535 / Screen.PrimaryScreen.Bounds.Height)));
                            iss.Mouse.LeftButtonClick();
                            toExit.Restart();
                        }
                    }
                    
                }

                if (toExit.Elapsed.Seconds == SECONDS_TO_RETRY)
                {
                    break;
                }
            }

            Thread.Sleep(500);
        }

    }
}

