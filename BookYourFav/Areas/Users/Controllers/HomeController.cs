using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookYourFav.Models;
using System.Data.Entity;

namespace BookYourFav.Areas.Users.Controllers
{

    public class HomeController : Controller
    {
        // GET: Users/Home
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Uid"] != null)
            {
                ViewBag.Result = 0;
                ViewBag.Data = "";
                BookYourFavEntities Db = new BookYourFavEntities();
                List<Movie> movie = Db.Movies.ToList();
                return View(movie);
            }
            else
                return RedirectToAction("Login","Home",new { area = "" });

        }
        [HttpPost]
        public ActionResult Book(int? movieid)
        {
            ViewBag.Data = "Book";
            ViewBag.Result = movieid;
            BookYourFavEntities Db = new BookYourFavEntities();
            List<Movie> movie = Db.Movies.ToList();
            Movie details = Db.Movies.Find(movieid);
            details.mticketBooked += 1;
            Db.Entry(details).State = EntityState.Modified;
            Book Booking = new Book();
            Booking.uid =(int?)Session["Uid"];
            Booking.uname =Session["Uname"].ToString();
            Booking.mid = details.mid;
            Booking.mname = details.mname;           
            Db.Books.Add(Booking);
            Db.SaveChanges();
            return View("Index",movie);
        }
//--------------------------------------- Ticket Details --------------------------------------
        [HttpPost]
        public ActionResult Details(int? movieid)
        {
            ViewBag.Data = "Details";
            ViewBag.Result = movieid;
            BookYourFavEntities Db = new BookYourFavEntities();
            var userid = (int?)Session["Uid"];
            var count = Db.Books.Where(item => item.uid == userid && item.mid == movieid).Count();
            ViewBag.Detail = count + " Tickets Booked...";
            return View("Index",Db.Movies.ToList());
        }
    }
}