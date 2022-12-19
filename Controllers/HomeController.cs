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
        static readonly Stopwatch timer2 = new Stopwatch();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string image)
        {
            Console.WriteLine("Inicio de Script");
            timer2.Restart();
            BarcodeResultViewModel model = new();

            var imgBytes = Convert.FromBase64String(image);

            BarcodeReaderOptions options = new BarcodeReaderOptions()
            {
                Speed = ReadingSpeed.ExtremeDetail,
                ExpectBarcodeTypes = BarcodeEncoding.EAN13 | BarcodeEncoding.EAN8 | BarcodeEncoding.UPCA | BarcodeEncoding.UPCE | BarcodeEncoding.PharmaCode
            };

            timer.Restart();

            BarcodeResults Results = BarcodeReader.Read(imgBytes, options);

            timer.Stop();

            if (Results.Any())
            {
                BarcodeResult Result = Results.First();

                Console.WriteLine("Codigo: " + Result.Text);
                Console.WriteLine("Formato: " + Result.BarcodeType);
                Console.WriteLine("Tiempo en Decodificar: " + timer.Elapsed);

                model.ResponseCode = 0;
                model.Text = Result.Text;
                model.Format = Result.BarcodeType.ToString();
                model.TimeElapse = timer.Elapsed.ToString();

                timer2.Stop();
                Console.WriteLine("Tiempo en ejecutar script: " + timer2.Elapsed);

                return Json(model);
            }
            else
            {
                Console.WriteLine("No se encontro nigun codigo de barras");

                model.ResponseCode = 1;

                timer2.Stop();
                Console.WriteLine("Tiempo en ejecutar script: " + timer2.Elapsed);

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