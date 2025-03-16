using Microsoft.AspNetCore.Mvc;
using WebServerPrograming.Models;
using System.Text.Json;
using WebServerPrograming.Extensions;

namespace WebServerPrograming.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Cart()
        {
            // Check if the user is logged in
            string userJson = Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(userJson))
            {
                ViewBag.IsLoggedIn = false;
                TempData["Message"] = "Please sign in to access your cart.";
                return RedirectToAction("Index", "Home");
            }

            // Display user information
            var user = JsonSerializer.Deserialize<User>(userJson);
            ViewBag.IsLoggedIn = true;
            ViewBag.UserName = $"{user.FirstName} {user.LastName}";

            // Retrieve the user's cart from the dictionary
            var cart = UserCartStorage.UserCarts.ContainsKey(user.UserId)
                ? UserCartStorage.UserCarts[user.UserId]
                : new List<int>();

            // Retrieve all movies from session
            var movies = HttpContext.Session.GetObjectFromJson<List<Movie>>("Movies");

            // Filter movies that are in the cart
            var cartMovies = movies?.Where(m => cart.Contains(m.MovieID)).ToList();

            return View("cart", cartMovies);
        }
    }
}