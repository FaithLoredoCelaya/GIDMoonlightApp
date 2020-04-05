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
            List<Businesses> businesses = new List<Businesses>();
            foreach(Businesses b in _context.Businesses)
            {
                businesses.Add(b);
            }
            foreach(Jobs j in _context.Jobs)
            {
                foreach(Businesses b in businesses)
                {
                    if(b.CompanyId == j.CompanyId)
                    {
                        j.Company = b;
                    }
                }
            }
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
            if(desc==null)
            {
                return View();
            }
            JobsReviewRepository jobRepo = new JobsReviewRepository();
            foreach(Jobs j in _context.Jobs)
            {
                if (j.JobType.Contains(desc) && j!=null)
                {
                    //job repo seems to be always null. needs to be fixed
                    jobRepo.Jobs.Add(j);

                    foreach(Reviews r in _context.Reviews)
                    {
                        if(r.JobId==j.JobId)
                        {
                            jobRepo.Reviews.Add(r);
                        }
                    }
                }
            }
            if(jobRepo.Jobs.Count==0)
            {
                ViewBag.errorMessage = "No jobs Found";
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
