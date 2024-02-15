using BookYourFav.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookYourFav.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {

            if (Session["Aid"] != null)
            {
                ViewBag.Amessage = "";
                BookYourFavEntities Db = new BookYourFavEntities();
                List<Movie> movie = Db.Movies.ToList();
                return View(movie);
            }
            else
                return RedirectToAction("AdminLogin", "Home", new { area = "" });

        }
//--------------------------------------------  Book Details Hyperlink -----------------------------------

        public ActionResult Book(int? movieId)
        {
            if (Session["Aid"] != null)
            {
                ViewBag.Amessage = "";
                BookYourFavEntities Db = new BookYourFavEntities();
                Movie name = Db.Movies.Where(item => item.mid == movieId).FirstOrDefault();
                ViewBag.name = name.mname;
                List<Tuple<int, String>> Booked = new List<Tuple<int, String>>();
                var username = Db.Users.Select(item => item.uname);
                foreach (var start in username)
                {
                    Booked.Add(new Tuple<int, String>(Db.Books.Where(item => start == item.uname && item.mid == movieId).Count(), start));
                }
                return View(Booked);
            }
            else
                return RedirectToAction("AdminLogin", "Home", new { area = "" });
        }
//----------------------------------------Add New Movie ---------------------------------------------
        [HttpGet]
        public ActionResult AddNew()
        {
            ViewBag.Aresult = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddNew(Movie movie)
        {
            BookYourFavEntities Db = new BookYourFavEntities();

            var data = Db.Movies.Where(item => item.mname.Equals(movie.mname)).FirstOrDefault();
            if (data == null)
            {
                Db.Movies.Add(movie);
                Db.SaveChanges();
                List<Movie> movies = Db.Movies.ToList();
                ViewBag.Amessage = "Movie Added Successfully.....";
                return View("index", movies);
            }
            else
            {
                ViewBag.Aresult = "Movie already available..";
                return View();
            }

        }
//-------------------------------------------Admin Edit--------------------------------------
        [HttpGet]
        public ActionResult Edit(int? movieid)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            Movie movie = Db.Movies.Find(movieid);
            return View(movie);
        }
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            Db.Entry(movie).State = EntityState.Modified;
            Db.SaveChanges();
            List<Movie> movies = Db.Movies.ToList();
            return View("index", movies);
        }
 //----------------------------------Remove the unwanted movie --------------------------------
       
        [HttpPost]
        public ActionResult Delete(int? movieId)
        {
            BookYourFavEntities Db = new BookYourFavEntities();
            Movie movie = Db.Movies.Find(movieId);
            Db.Movies.Remove(movie);
            Db.SaveChanges();
            List<Movie> movies = Db.Movies.ToList();
            return View("index", movies);

        }

    }
}