using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Abstract;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private INormalRepository repository;

        public HomeController(INormalRepository repo)
        {
            repository = repo;
        }
        // GET: Home
        
        public ActionResult Index3()
        {
            
            return View(repository.fTSetups);

            //return View(repository.fTSetups);
        }
        

    }
}