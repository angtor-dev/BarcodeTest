using BarcodeTest.Models;
using IronBarCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace BarcodeTest.Controllers
{
    public class WebCamController : Controller
    {
        static readonly Stopwatch timer = new Stopwatch();
        public WebCamController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CaptureImage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CaptureImage(string name)
        {
            try
            {
                BarcodeResultViewModel model = new();
                var files = HttpContext.Request.Form.Files;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var Stream = new MemoryStream())
                            {
                                file.CopyTo(Stream);
                                using (var img = Image.FromStream(Stream))
                                {
                                    timer.Restart();
                                    timer.Start();

                                    BarcodeResults Results = BarcodeReader.Read(img);

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
                            }
                        }
                    }
                    model.ResponseCode = 2;
                    return Json(model);
                }
                else
                {
                    model.ResponseCode = 2;
                    return Json(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
