using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Calculator.Controllers
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
        public IActionResult Add()
        {
            try
            {
                int num1 = Convert.ToInt32(HttpContext.Request.Form["num1"].ToString());
                int num2 = Convert.ToInt32(HttpContext.Request.Form["num2"].ToString());
                int sum = num1 + num2;

                ViewBag.SumResult = sum.ToString();

            }
            catch (Exception)
            {
                ViewBag.SumResult = "Wrong Answer";

            }
            return View("index");
        }

        public IActionResult Subtract()
        {
            try
            {
                int num1 = Convert.ToInt32(HttpContext.Request.Form["num1"].ToString());
                int num2 = Convert.ToInt32(HttpContext.Request.Form["num2"].ToString());
                int subtract = num1 - num2;

                ViewBag.SubtractResult = subtract.ToString();

            }
            catch (Exception)
            {
                ViewBag.SubtractResult = "Wrong Answer";

            }
            return View("index");
        }

        public IActionResult Multi()
        {
            try
            {
                int num1 = Convert.ToInt32(HttpContext.Request.Form["num1"].ToString());
                int num2 = Convert.ToInt32(HttpContext.Request.Form["num2"].ToString());
                int multi = num1 * num2;

                ViewBag.MultiResult = multi.ToString();

            }
            catch (Exception)
            {
                ViewBag.MultiResult = "Wrong Answer";

            }
            return View("index");
        }
        public IActionResult Div()
        {
            try
            {
                int num1 = Convert.ToInt32(HttpContext.Request.Form["num1"].ToString());
                int num2 = Convert.ToInt32(HttpContext.Request.Form["num2"].ToString());
                float div = (float)num1 / (float)num2;

                ViewBag.DivResult = div.ToString();

            }
            catch (Exception)
            {
                ViewBag.DivResult = "Wrong Answer";

            }
            return View("index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}