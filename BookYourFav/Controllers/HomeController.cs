using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookYourFav.Models;

namespace BookYourFav.Controllers
{
    public class HomeController : Controller
    {
//-------------------------------- index ---------------------------------------------
        public ActionResult Index()
        {
            return View();
        }
//-------------------------------- Login ---------------------------------------------
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User details)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            var data = Db.Users.Where(user => user.uname.Equals(details.uname) && user.upassword.Equals(details.upassword)).FirstOrDefault();
            if(data != null)
            {
                Session["Uid"] = data.uid;
                Session["Uname"] = data.uname;
                return RedirectToAction("Index","Home",new { area = "Users" });
            }
            else
            return RedirectToAction("Login");
        }

//-------------------------------- Register ---------------------------------------------

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.result = "";
            return View();
        }
        [HttpPost]
        public ActionResult Register(User details)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            var data = Db.Users.Where(user => user.uname.Equals(details.uname)).FirstOrDefault();
            if (data == null)
            {
                Db.Users.Add(details);
                Db.SaveChanges();
                ViewBag.result = "Registed Successfully.....";
            }
            else
                ViewBag.result = "UserName already available.. Please try different One...";
            return View() ;
        }

//-------------------------------- Admin ---------------------------------------------

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(Admin details)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            var data = Db.Admins.Where(admin => admin.aname.Equals(details.aname) && admin.apassword.Equals(details.apassword)).FirstOrDefault();
            if (data != null)
            {
                Session["Aid"] = details.aid.ToString();
                Session["Aname"] = details.aname.ToString();
                return RedirectToAction("Index","Home",new { area = "Admin" });
            }
            else
                return RedirectToAction("AdminLogin");
        }
    
//-------------------------------- Logout ---------------------------------------------

        [HttpPost]
        public ActionResult uLogout()
        {
            Session["Uid"] = null;
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult aLogout()
        {
            Session["Aid"] = null;
            return RedirectToAction("AdminLogin");
        }

    }
}