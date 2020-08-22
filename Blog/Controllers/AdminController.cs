using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//add model
using Blog.Models;

namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        //initializing
        blogDB blogDB = new blogDB();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.CategoriesCount = blogDB.Categories.Count();
            ViewBag.ArticleCount = blogDB.Articles.Count();
            ViewBag.CommentsCount = blogDB.Comments.Count();
            ViewBag.UsersCount = blogDB.Users.Count();
            return View();
        }
    }
}