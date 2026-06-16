using ZXing;
using ZXing.QrCode;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace qrschool_deckstop.Services
{
    public static class QrCodeService
    {
        public static byte[] GenerateQrCode(string text, int width = 300, int height = 300)
        {
            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = width,
                        Height = height,
                        Margin = 1,
                        CharacterSet = "UTF-8"
                    }
                };

                var pixelData = writer.Write(text);

                using (var bitmap = new Bitmap(
                    pixelData.Width, 
                    pixelData.Height, 
                    PixelFormat.Format32bppRgb))
                {
                    var bitmapData = bitmap.LockBits(
                        new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppRgb);

                    System.Runtime.InteropServices.Marshal.Copy(
                        pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);

                    bitmap.UnlockBits(bitmapData);

                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка генерации QR-кода: {ex.Message}");
            }
        }

        public static void SaveQrToFile(string text, string filePath, int width = 300, int height = 300)
        {
            var qrBytes = GenerateQrCode(text, width, height);
            File.WriteAllBytes(filePath, qrBytes);
        }
    }
}
