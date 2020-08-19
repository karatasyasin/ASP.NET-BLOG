using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//add model
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        //initializing
        blogDB blogDB = new blogDB();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        // GET: About
        public ActionResult About() { return View(); }
        // GET: Contact
        public ActionResult Contact() { return View(); }
        // GET: CategoryPartial
        public ActionResult CategoryPartial() { return View(blogDB.Categories.ToList()); }
    }
}