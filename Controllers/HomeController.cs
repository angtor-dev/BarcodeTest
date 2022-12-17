using BarcodeTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IronBarCode;

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

        public IActionResult Index()
        {
            BarcodeReaderOptions options = new BarcodeReaderOptions()
            {
                Speed = ReadingSpeed.ExtremeDetail,
                ExpectBarcodeTypes = BarcodeEncoding.UPCA | BarcodeEncoding.UPCE
            };

            timer.Restart();
            timer.Start();
            BarcodeResults results = BarcodeReader.Read("Barcodes/barcode3.png", options);
            // BarcodeResult Result = BarcodeReader.ReadASingleBarcode("Barcodes/barcode2.png", BarcodeEncoding.AllOneDimensional, BarcodeReader.BarcodeRotationCorrection.Extreme, BarcodeReader.BarcodeImageCorrection.MediumCleanPixels);

            Console.WriteLine("Stop reading Barcode - " + timer.Elapsed.ToString());
            timer.Stop();

            foreach(var result in results)
            {
                Console.WriteLine("Value: " + result.Text + " Format: " + result.BarcodeType);
            }

            //if (Result != null && Result.Text != "")
            //{
            //    Console.WriteLine("GetStarted was a success.  Read Value: " + Result.Text + " Format: " + Result.BarcodeType);
            //}

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
    }
}