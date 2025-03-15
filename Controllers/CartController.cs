using Microsoft.AspNetCore.Mvc;
using WebServerPrograming.Extensions;
using WebServerPrograming.Models;

namespace WebServerPrograming.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Cart() // Action name is "Cart"
        {
            // Retrieve cart movie IDs from session
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();

            // Retrieve all movies from session
            var movies = HttpContext.Session.GetObjectFromJson<List<Movie>>("Movies");

            // Filter movies that are in the cart
            var cartMovies = movies?.Where(m => cart.Contains(m.MovieID)).ToList();

            return View("cart", cartMovies); // Explicitly specify the view name as "cart"
        }
    }
}