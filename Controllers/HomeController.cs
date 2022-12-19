using BarcodeTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IronBarCode;
using System.Drawing;

namespace BarcodeTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static readonly Stopwatch timer = new Stopwatch();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Leyendo un codigo de barras desde una imagen
            BarcodeReaderOptions options = new BarcodeReaderOptions()
            {
                Speed = ReadingSpeed.ExtremeDetail,
                ExpectBarcodeTypes = BarcodeEncoding.EAN13 | BarcodeEncoding.EAN8 | BarcodeEncoding.UPCA | BarcodeEncoding.UPCE | BarcodeEncoding.PharmaCode
            };

            timer.Restart();
            timer.Start();
            BarcodeResults results = BarcodeReader.Read("Barcodes/barcode1.png", options);
            // BarcodeResult Result = BarcodeReader.ReadASingleBarcode("Barcodes/barcode2.png", BarcodeEncoding.AllOneDimensional, BarcodeReader.BarcodeRotationCorrection.Extreme, BarcodeReader.BarcodeImageCorrection.MediumCleanPixels);

            Console.WriteLine("Stop reading Barcode - " + timer.Elapsed.ToString());
            timer.Stop();

            foreach(var result in results)
            {
                Console.WriteLine("Value: " + result.Text + " Format: " + result.BarcodeType);
            }

            return View();
        }

        [HttpPost]
        public JsonResult Index(string image)
        {
            BarcodeResultViewModel model = new();

            var imgBytes = Convert.FromBase64String(image);

            timer.Restart();
            timer.Start();

            BarcodeResults Results = BarcodeReader.Read(imgBytes);

            timer.Stop();

            if (Results.Any())
            {
                BarcodeResult Result = Results.First();

                Console.WriteLine("Codigo: " + Result.Text);
                Console.WriteLine("Formato: " + Result.BarcodeType);

                model.ResponseCode = 0;
                model.Text = Result.Text;
                model.Format = Result.BarcodeType.ToString();
                model.TimeElapse = timer.Elapsed.ToString();

                return Json(model);
            }
            else
            {
                Console.WriteLine("No se encontro nigun codigo de barras");

                model.ResponseCode = 1;

                return Json(model);
            }
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
    }
}