using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebServerPrograming.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace WebServerPrograming.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Kullan�c� �erezlerde var m�?
            string userJson = Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(userJson))
            {
                ViewBag.IsLoggedIn = false;
                return View();
            }

            // Kullan�c� bilgilerini g�ster
            var user = JsonSerializer.Deserialize<User>(userJson);
            ViewBag.IsLoggedIn = true;
            ViewBag.UserName = $"{user.FirstName} {user.LastName}";

            // E�er film listesi yoksa olu�tur ve session�a kaydet
            if (HttpContext.Session.GetString("Movies") == null)
            {
                var movies = new List<Movie>
                {
                    new Movie { MovieID = 1, Title = "X-Men: The Last Stand", Director = "Brett Ratner" },
                    new Movie { MovieID = 2, Title = "Spider Man 2", Director = "Sam Raimi" },
                    new Movie { MovieID = 3, Title = "Spider Man 3", Director = "Sam Raimi" },
                    new Movie { MovieID = 4, Title = "Valkyrie", Director = "Bryan Singer" },
                    new Movie { MovieID = 5, Title = "Gladiator", Director = "Ridley Scott" }
                };
                HttpContext.Session.SetString("Movies", JsonSerializer.Serialize(movies));
            }

            // Filmleri ekrana g�nder
            var movieList = JsonSerializer.Deserialize<List<Movie>>(HttpContext.Session.GetString("Movies"));
            return View(movieList);
        }

        public IActionResult Logout()
        {
            // �erezi temizle
            Response.Cookies.Delete("UserInfo");
            return RedirectToAction("Index");
        }
    }
}
