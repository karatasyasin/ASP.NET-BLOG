using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//add model
using Blog.Models;
using System.Web.Helpers;
using System.IO;

namespace Blog.Controllers
{
    public class AdminArticleController : Controller
    {
        //initializing
        blogDB blogDB = new blogDB();

        // GET: AdminArticle
        public ActionResult Index()
        {
            var articles = blogDB.Articles.ToList();
            return View(articles);
        }

        // GET: AdminArticle/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminArticle/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(blogDB.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: AdminArticle/Create
        [HttpPost]
        public ActionResult Create(Article article, string tags, HttpPostedFileBase Photo)
        {
            try
            {
                // TODO: Add insert logic here
                if (Photo!=null)
                {
                    WebImage webImage = new WebImage(Photo.InputStream);
                    FileInfo fileInfo = new FileInfo(Photo.FileName);
                    string newPhoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(700, 350);
                    webImage.Save("~/Uploads/ArticlePhoto/" + newPhoto);
                    article.Photo = "/Uploads/ArticlePhoto/" + newPhoto;
                }
                if (tags!=null)
                {
                    string[] tagArray = tags.Split(',');
                    foreach (var item in tagArray)
                    {
                        var newTag = new Tag { TagName = item };
                        blogDB.Tags.Add(newTag);
                        article.Tags.Add(newTag);
                    }
                }
                article.UserId = Convert.ToInt32(Session["UserId"]);
                blogDB.Articles.Add(article);
                blogDB.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(article);
            }
        }

        // GET: AdminArticle/Edit/5
        public ActionResult Edit(int id)
        {
            var article = blogDB.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (article==null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(blogDB.Categories, "CategoryId", "CategoryName", article.CategoryId);
            return View(article);
        }

        // POST: AdminArticle/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Photo, Article article)
        {
            try
            {
                // TODO: Add update logic here
                var updateArticle = blogDB.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
                if (Photo!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(updateArticle.Photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(updateArticle.Photo));
                    }
                    WebImage webImage = new WebImage(Photo.InputStream);
                    FileInfo fileInfo = new FileInfo(Photo.FileName);
                    string newPhoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(700, 350);
                    webImage.Save("~/Uploads/ArticlePhoto/" + newPhoto);
                    updateArticle.Photo = "/Uploads/ArticlePhoto/" + newPhoto;
                    updateArticle.Title = article.Title;
                    updateArticle.Content = article.Content;
                    updateArticle.CategoryId = article.CategoryId;
                    blogDB.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(blogDB.Categories, "CategoryId", "CategoryName", article.CategoryId);
                return View(article);
            }
        }

        // GET: AdminArticle/Delete/5
        public ActionResult Delete(int id)
        {
            var artical = blogDB.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (artical==null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }

        // POST: AdminArticle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var updateArticle = blogDB.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
                
                if (updateArticle == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(updateArticle.Photo)))
                {
                    System.IO.File.Delete(Server.MapPath(updateArticle.Photo));
                }

                foreach (var item in updateArticle.Comments.ToList())
                {
                    blogDB.Comments.Remove(item);
                }
                foreach (var item in updateArticle.Tags.ToList())
                {
                    blogDB.Tags.Remove(item);
                }

                blogDB.Articles.Remove(updateArticle);
                blogDB.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
