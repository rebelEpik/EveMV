using EveMV.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EveMV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region DWM functions

        [DllImport("dwmapi.dll")]
        static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out System.Windows.Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        #endregion

        #region Constants

        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        public void updateGUI()
        {
            Process[] processes = Process.GetProcessesByName("exefile");
            foreach(Process p in processes)
            {
                EveClient ec = new EveClient()
                {
                    windowHandle = p.MainWindowHandle,
                    windowTitle = p.MainWindowTitle,
                    clientID = p.Id
                };


                    System.Windows.Rect rc;
                    GetWindowRect(ec.windowHandle, out rc);

                //Console.WriteLine("Constructing bmp");
                
                int width = Convert.ToInt32(rc.Size.Width);
                int height = Convert.ToInt32(rc.Size.Height);
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    Graphics gfxBmp = Graphics.FromImage(bmp);
                    IntPtr hdcBitmap = gfxBmp.GetHdc();


                    PrintWindow(ec.windowHandle, hdcBitmap, 0);
                    
                    Console.WriteLine("Releasing stuff");
                    gfxBmp.ReleaseHdc(hdcBitmap);
                    gfxBmp.Dispose();

                    Console.WriteLine("Made BMP!");
                    bmp.Save("J:\\AutomationTool\\screenshot at " + DateTime.Now.ToString("HH-mm-ss tt") + ".png", ImageFormat.Png);
                    Console.WriteLine("Saved bmp!");
                    bmp.Dispose();

                    /*
                    Rectangle rc;
                    GetWindowRect(hwnd, out rc);
                    Console.WriteLine("RC: " + rc);

                    Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                    Graphics gfxBmp = Graphics.FromImage(bmp);
                    IntPtr hdcBitmap = gfxBmp.GetHdc();

                    PrintWindow(hwnd, hdcBitmap, 0);

                    gfxBmp.ReleaseHdc(hdcBitmap);
                    gfxBmp.Dispose();

                    bmp.Save("J:\\AutomationTool");
                    Console.WriteLine("Saved image!");
                    */

                    Console.WriteLine("Attempting to register thumbnail!");
                    int i = DwmRegisterThumbnail(this.Handle, hWnd, out thumb);
                    Console.WriteLine("Succesfully registered thumbnail!");


                    UpdateThumb();
                    Console.WriteLine("Succesfully updated thumb!");



                    break;
                }
            }
    }
}
