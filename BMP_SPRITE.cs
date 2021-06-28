using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;

namespace Aladdin_SNES_Text_Changer
{
    class BMP_SPRITE
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource BmpToIMG(Bitmap Source)
        {
            var H = Source.GetHbitmap();
            try { return Imaging.CreateBitmapSourceFromHBitmap(H, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); }
            finally { DeleteObject(H); }
        }
    }
}
