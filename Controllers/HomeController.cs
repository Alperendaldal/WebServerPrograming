using Microsoft.AspNetCore.Mvc;
using WebServerPrograming.Models;
using System.Text.Json;

namespace WebServerPrograming.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Check if the user is logged in
            string userJson = Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(userJson))
            {
                ViewBag.IsLoggedIn = false;
                return View();
            }

            // Display user information
            var user = JsonSerializer.Deserialize<User>(userJson);
            ViewBag.IsLoggedIn = true;
            ViewBag.UserName = $"{user.FirstName} {user.LastName}";

            // Create and store movies in session if they don't exist
            if (HttpContext.Session.GetString("Movies") == null)
            {
                var movies = new List<Movie>
                {
                    new Movie { MovieID = 1, Title = "X-Men: The Last Stand", Director = "Brett Ratner", Stars = "Patrick Stewart, Hugh Jackman, Halle Berry", ReleaseYear = 2006, ImageUrl = "~/Images/xman.jpg" },
                    new Movie { MovieID = 2, Title = "Spider Man 2", Director = "Sam Raimi", Stars = "Tobey Maguire, Kirsten Dunst, Alfred Molina", ReleaseYear = 2004, ImageUrl = "~/Images/spiderman2.jpg" },
                    new Movie { MovieID = 3, Title = "Spider Man 3", Director = "Sam Raimi", Stars = "Tobey Maguire, Kirsten Dunst, Topher Grace", ReleaseYear = 2007, ImageUrl = "~/Images/spiderman3.jpg" },
                    new Movie { MovieID = 4, Title = "Valkyrie", Director = "Bryan Singer", Stars = "Tom Cruise, Bill Nighy, Carice van Houten", ReleaseYear = 2008, ImageUrl = "~/Images/valkyrie.jpg" },
                    new Movie { MovieID = 5, Title = "Gladiator", Director = "Ridley Scott", Stars = "Russell Crowe, Joaquin Phoenix, Connie Nielsen", ReleaseYear = 2000, ImageUrl = "~/Images/gladiator.png" }
                };
                HttpContext.Session.SetString("Movies", JsonSerializer.Serialize(movies));
            }

            // Send movies to the view
            var movieList = JsonSerializer.Deserialize<List<Movie>>(HttpContext.Session.GetString("Movies"));
            return View(movieList);
        }

        public IActionResult Logout()
        {
            // Clear the cookie
            Response.Cookies.Delete("UserInfo");
            return RedirectToAction("Index");
        }
    }
}