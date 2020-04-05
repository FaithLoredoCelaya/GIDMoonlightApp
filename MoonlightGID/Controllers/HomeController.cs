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
            HttpContext.Session.Clear();
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

        [HttpPost]
        public IActionResult SearchResults(string desc)
        {

            ViewBag.User = HttpContext.Session.GetJson<Customers>("Customer").UserLogin;
            if (desc==null)
            {
                return View();
            }
            JobsReviewRepository jobRepo = new JobsReviewRepository();
            jobRepo.Jobs = new List<Jobs>();
            jobRepo.Reviews = new List<Reviews>();
            foreach (Jobs j in _context.Jobs)
            {
                if (j.JobType.Contains(desc))
                {
                    jobRepo.Jobs.Add(j);
                }
            }
            if (jobRepo.Jobs.Count == 0)
            {
                ViewBag.errorMessage = "No jobs Found";
            }
            else
            {
                for (int i = 0; i < jobRepo.Jobs.Count(); i++)
                {
                    foreach (Reviews r in _context.Reviews)
                    {
                        if (r.JobId == jobRepo.Jobs[i].JobId)
                        {
                            jobRepo.Reviews.Add(r);
                        }
                    }
                }
            }
            return View(jobRepo);
        }

        public IActionResult CompareJobDetails(int id)
        {
            ViewBag.User = HttpContext.Session.GetJson<Customers>("Customer").UserLogin;

            JobsReviewRepository jobRepo = new JobsReviewRepository();
            jobRepo.Jobs = new List<Jobs>();
            jobRepo.Reviews = new List<Reviews>();

            jobRepo.Jobs.Add(_context.Jobs.Find(id));

            for (int i = 0; i < jobRepo.Jobs.Count(); i++)
            {
                foreach (Reviews r in _context.Reviews)
                {
                    if (r.JobId == jobRepo.Jobs[i].JobId)
                    {
                        jobRepo.Reviews.Add(r);
                    }
                }
            }
            return View(jobRepo);
        }

        [HttpPost]
        public IActionResult Contact(int i)
        {
            Jobs j = new Jobs();
            j = _context.Jobs.Find(i);
            j.Company = _context.Businesses.Find(j.CompanyId);
            ViewBag.User = HttpContext.Session.GetJson<Customers>("Customer").UserLogin;
            return View(j);
        }

        [HttpPost]
        public IActionResult SideToSideComparison(List<int> toCompare)
        {
            ViewBag.User = HttpContext.Session.GetJson<Customers>("Customer").UserLogin;
            List<JobsReviewRepository> jobRepo = new List<JobsReviewRepository>();
            JobsReviewRepository toAdd = new JobsReviewRepository();
            JobsReviewRepository toAdd2 = new JobsReviewRepository();
            if(toCompare.Count()==0)
            {
                return NotFound();
            }
            toAdd = GetReviews(toCompare[0]);
            jobRepo.Add(toAdd);
            if (toCompare.Count()<2)
            {
                return View(jobRepo);
            }
            else
            {
                toAdd2 = GetReviews(toCompare[1]);
                jobRepo.Add(toAdd2);
            }
            return View(jobRepo);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JobsReviewRepository GetReviews(int id)
        {
            JobsReviewRepository jobRepo = new JobsReviewRepository();
            jobRepo.Jobs = new List<Jobs>();
            jobRepo.Reviews = new List<Reviews>();

            jobRepo.Jobs.Add(_context.Jobs.Find(id));

            for (int i = 0; i < jobRepo.Jobs.Count(); i++)
            {
                foreach (Reviews r in _context.Reviews)
                {
                    if (r.JobId == jobRepo.Jobs[i].JobId)
                    {
                        jobRepo.Reviews.Add(r);
                    }
                }
            }
            return jobRepo;
        }
    }
}
