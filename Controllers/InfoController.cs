using Microsoft.AspNetCore.Mvc;
using WebServerPrograming.Extensions;
using WebServerPrograming.Models;
using System.Text.Json;

namespace WebServerPrograming.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Info(int? id)
        {
            // Check if the movie ID is provided
            if (!id.HasValue)
            {
                return View("Error", new ErrorViewModel { Message = "Please specify a movie ID." });
            }

            // Retrieve movies from session
            var movies = HttpContext.Session.GetObjectFromJson<List<Movie>>("Movies");
            var movie = movies?.FirstOrDefault(m => m.MovieID == id.Value);

            // Check if the movie exists
            if (movie == null)
            {
                return View("Error", new ErrorViewModel { Message = "Invalid movie ID." });
            }

            // Check if the user is logged in
            string userJson = Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(userJson))
            {
                ViewBag.IsLoggedIn = false;
            }
            else
            {
                // Display user information
                var user = JsonSerializer.Deserialize<User>(userJson);
                ViewBag.IsLoggedIn = true;
                ViewBag.UserName = $"{user.FirstName} {user.LastName}";
            }

            return View("info", movie);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Check if the user is logged in
            string userJson = Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["Message"] = "Please sign in to add this movie to your cart.";
                return RedirectToAction("Info", new { id });
            }

            // Retrieve the user's unique identifier
            var user = JsonSerializer.Deserialize<User>(userJson);

            // Retrieve or create the user's cart
            if (!UserCartStorage.UserCarts.ContainsKey(user.UserId))
            {
                UserCartStorage.UserCarts[user.UserId] = new List<int>();
            }
            var cart = UserCartStorage.UserCarts[user.UserId];

            if (cart.Contains(id))
            {
                TempData["Message"] = "This movie is already in your cart.";
            }
            else
            {
                cart.Add(id);
                TempData["Message"] = "Movie added to cart.";
            }

            return RedirectToAction("Info", new { id });
        }
    }
}