using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoonlightGID.Infrastructure;
using MoonlightGID.Models;

namespace MoonlightGID.Controllers
{
    public class HomeController : Controller
    {
        private readonly MoonLightContext _context;

        public HomeController(MoonLightContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult JobSearch(Customers formResponse)
        {
            Customers logCheck = _context.Customers.Find(formResponse.UserLogin);
            if(logCheck.Equals(null))
            {
                ViewBag.errorMessage = "Wrong Username/Password";
                return View("Index");
            }
            if(formResponse.Password.Equals(logCheck.Password))
            {
                HttpContext.Session.SetJson("Customer", formResponse);
                ViewBag.customer = formResponse.FirstName + " " + formResponse.LastName;
                ViewBag.User = formResponse.UserLogin;
                return View();
            }
            else
            {
                ViewBag.errorMessage = "Wrong Username/Password";
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
