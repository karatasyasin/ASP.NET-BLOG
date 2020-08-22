using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//add model
using Blog.Models;
using PagedList;
using PagedList.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        //initializing
        blogDB blogDB = new blogDB();

        // GET: Home
        public ActionResult Index(int Page = 1)
        {
            // 4: Article List
            var article = blogDB.Articles.OrderByDescending(m => m.ArticleId).ToPagedList(Page, 5);
            return View(article);
        }
        // GET: ArticleDetail
        public ActionResult ArticleDetail(int id)
        {
            var article = blogDB.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (article==null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        public JsonResult AddComment(string comment, int ArticleId)
        {
            if (comment == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            blogDB.Comments.Add(new Comment { UserId = Convert.ToInt32(Session["UserId"]), ArticleId = ArticleId, Content = comment, Date = DateTime.Now });
            blogDB.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DelComment(int id)
        {
            var userId = Session["UserId"];
            var comment = blogDB.Comments.Where(c => c.CommentId == id).SingleOrDefault();
            var article = blogDB.Articles.Where(a => a.ArticleId == comment.ArticleId).SingleOrDefault();
            if (comment.UserId==Convert.ToInt32(userId))
            {
                blogDB.Comments.Remove(comment);
                blogDB.SaveChanges();
                return RedirectToAction("ArticleDetail", "Home", new { id = article.ArticleId });
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult IncrementDisplay(int ArticleId)
        {
            var article = blogDB.Articles.Where(a => a.ArticleId == ArticleId).SingleOrDefault();
            if (article.Displayed == null)
            {
                article.Displayed = 0;
            }
            article.Displayed += 1;
            blogDB.SaveChanges();
            return View();
        }
        // GET: About
        public ActionResult About() { return View(); }
        // GET: Contact
        public ActionResult Contact() { return View(); }
        // GET: CategoryPartial
        public ActionResult CategoryPartial() { return View(blogDB.Categories.ToList()); }
        // GET: SearchPartial
        public ActionResult Search(string Search = null)
        {
            var searching = blogDB.Articles.Where(s => s.Title.Contains(Search)).ToList();
            return View(searching.OrderByDescending(d => d.Date));
        }
    }
}