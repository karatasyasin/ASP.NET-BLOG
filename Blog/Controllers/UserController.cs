using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
//add model
using Blog.Models;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        //initializing
        blogDB blogDB = new blogDB();

        // GET: User
        public ActionResult Index(int id)
        {
            var user = blogDB.Users.Where(u => u.UserId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["UserId"]) != user.UserId)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user, HttpPostedFileBase Photo)
        {
            try
            {
                // if photo != null
                    WebImage webImage = new WebImage(Photo.InputStream);
                    FileInfo fileInfo = new FileInfo(Photo.FileName);
                    string newPhoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(200, 200);
                    webImage.Save("~/Uploads/UserPhoto/" + newPhoto);
                    user.Photo = "/Uploads/UserPhoto/" + newPhoto;
                    user.AuthId = 2;
                    blogDB.Users.Add(user);
                    blogDB.SaveChanges();
                    Session["UserId"] = user.UserId;
                    Session["UserName"] = user.UserName;

                    return RedirectToAction("Index", "Home");

            }
            catch
            {
                ModelState.AddModelError("Photo", "Choise a photo!");
                return View(user);
            }
        }
        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }
        // POST: User/Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            var login = blogDB.Users.Where(u => u.UserName == user.UserName).SingleOrDefault();
            if (login.UserName==user.UserName && login.Email==user.Email && login.Password==user.Password)
            {
                Session["UserId"] = login.UserId;
                Session["UserName"] = login.UserName;
                Session["AuthId"] = login.AuthId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Warn = "Please check your informations again!";
                return View();
            }
        }
        // GET: User/Logout
        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        // GET: User/Edit
        public ActionResult Edit(int id)
        {
            var user = blogDB.Users.Where(u => u.UserId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["UserId"])!=user.UserId)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: User/Edit
        [HttpPost]
        public ActionResult Edit(User user, int id, HttpPostedFileBase Photo)
        {
            try
            {
                // TODO: Add update logic here
                var updateUser = blogDB.Users.Where(m => m.UserId == id).SingleOrDefault();
                if (Photo != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(updateUser.Photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(updateUser.Photo));
                    }
                    WebImage webImage = new WebImage(Photo.InputStream);
                    FileInfo fileInfo = new FileInfo(Photo.FileName);
                    string newPhoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(200, 200);
                    webImage.Save("~/Uploads/UserPhoto/" + newPhoto);
                    updateUser.Photo = "/Uploads/UserPhoto/" + newPhoto;
                }
                updateUser.FullName = user.FullName;
                updateUser.UserName = user.UserName;
                updateUser.Password = user.Password;
                updateUser.Email = user.Email;
                blogDB.SaveChanges();
                Session["UserId"] = user.UserId;
                return RedirectToAction("Index", "Home", new { id = updateUser.UserId });
            }
            catch
            {
                return View();
            }
        }
    }
}