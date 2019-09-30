using System;
using System.IO;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SbpExampleShop.Backend.Abstractions;

namespace SbpExampleShop.Backend.Utils
{
    public class QrEncoder : IQrEncoder
    {
        private readonly QRCodeGenerator _qrGenerator;

        public QrEncoder()
        {
            _qrGenerator = new QRCodeGenerator();
        }

        private MemoryStream Build(QRCodeData data, int pixelsPerModule = 40)
        {
            var size = data.ModuleMatrix.Count * pixelsPerModule;
            using (var image = new Image<Rgba32>(size, size))
            {
                for (var x = 0; x < size; x += pixelsPerModule)
                for (var y = 0; y < size; y += pixelsPerModule)
                {
                    int CalcMatrixOffset(int ax) => (ax + pixelsPerModule) / pixelsPerModule - 1;

                    var color = data.ModuleMatrix[CalcMatrixOffset(y)][CalcMatrixOffset(x)]
                        ? Rgba32.Black
                        : Rgba32.White;
                    FillRectangle(image, color, x, y, pixelsPerModule,
                        pixelsPerModule);
                }
                var ms = new MemoryStream();
                image.SaveAsJpeg(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        private void FillRectangle(Image<Rgba32> image, Rgba32 color, int x, int y, int width, int height)
        {
            for (var i = x; i < x + width; i++)
            for (var k = y; k < y + height; k++)
            {
                image[i, k] = color;
            }
        }
        
        public string EncodeToBase64(string generatedQrPayload)
        {
            var qrCodeData = _qrGenerator.CreateQrCode(generatedQrPayload, QRCodeGenerator.ECCLevel.L);
            return Convert.ToBase64String(Build(qrCodeData).ToArray());
        }
    }
}