
/// <summary>
/// This class allow you create a Cursor form a Bitmap
/// </summary>
/// 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ThunderEditor.Editor
{
    /// <summary>
    /// TODO:没有透明通道
    /// </summary>
    internal class BitmapCursor : SafeHandle
    {

        public override bool IsInvalid
        {
            get
            {
                return handle == (IntPtr)(-1);
            }
        }

        public static Cursor Create(Bitmap cursorBitmap)
        {

            var c = new BitmapCursor(cursorBitmap);

            return CursorInteropHelper.Create(c);
        }

        public static Cursor Create(BitmapSource source)
        {
            var c = new BitmapCursor(BitmapSourceToBitmap(source));

            return CursorInteropHelper.Create(c);
        }

        private static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);

            bmp.UnlockBits(data);

            return bmp;
        }



        protected BitmapCursor(Bitmap cursorBitmap)
            : base((IntPtr)(-1), true)
        {
            handle = cursorBitmap.GetHicon();
        }


        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }

        [DllImport("user32")]
        private static extern bool DestroyIcon(IntPtr hIcon);
    }
}