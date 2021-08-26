using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using barcodeGenerator.Models;
using BarcodeLib;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using QRCoder;

namespace barcodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        //Barcode generator
        public IActionResult GenerateBarcode(string code="112233")
        {
            Barcode barcode = new Barcode();
            Image img = barcode.Encode(TYPE.CODE39, code, Color.Black, Color.White, 250, 100);
            var data = ConvertImageToByte(img);
            return File(data, "image/jpeg");
        }

        private byte[] ConvertImageToByte(Image img)
        {
            using(MemoryStream memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        //QR Code generator
        public IActionResult GenerateQRCode(string code ="Welcome to QR Code Gen")
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap bitmap = qRCode.GetGraphic(15);
            var bitmapBytes = CovertBitMapToBytes(bitmap);
            return File(bitmapBytes, "image/jpeg");
        }

        private byte[] CovertBitMapToBytes(Bitmap bitmap)
        {
            using(MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }            
        }
    }


}
