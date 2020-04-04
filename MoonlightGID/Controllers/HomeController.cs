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
        public IActionResult Index(Customers formResponse)
        {
            Customers logCheck=null;
            foreach (Customers c in _context.Customers)
            {
                if (c.UserLogin == formResponse.UserLogin)
                {
                    logCheck = _context.Customers.Find(c.CustomerId);
                }
            }
            if(logCheck==null)
            {
                ViewBag.errorMessage = "Wrong Username/Password";
                return View("Index");
            }
            if(formResponse.Password.Equals(logCheck.Password))
            {
                HttpContext.Session.SetJson("Customer", formResponse);
                ViewBag.customer = formResponse.FirstName + " " + formResponse.LastName;
                ViewBag.User = formResponse.UserLogin;
                return View("JobSearch");
            }
            else
            {
                ViewBag.errorMessage = "Wrong Username/Password";
                return View("Index");
            }
        }

        //this is a partial view needs to be modifed
        [HttpPost]
        public IActionResult SearchResults(string desc)
        {
            List<Jobs> jobRepo = new List<Jobs>();
            foreach(Jobs j in _context.Jobs)
            {
                if (j.JobType.Contains(desc))
                {
                    jobRepo.Add(j);
                }
            }
            return View(jobRepo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
