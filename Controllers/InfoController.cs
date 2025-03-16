using Microsoft.AspNetCore.Mvc;
using WebServerPrograming.Extensions;
using WebServerPrograming.Models;

namespace WebServerPrograming.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Info(int? id)
        {
            if (!id.HasValue)
            {
                return View("Error", new ErrorViewModel { Message = "No movie ID provided." });
            }

            // Retrieve movies from session
            var movies = HttpContext.Session.GetObjectFromJson<List<Movie>>("Movies");
            var movie = movies?.FirstOrDefault(m => m.MovieID == id.Value);

            if (movie == null)
            {
                return View("Error", new ErrorViewModel { Message = "Movie not found." });
            }

            return View("info", movie);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();

            if (cart.Contains(id))
            {
                TempData["Message"] = "This movie is already in your cart.";
            }
            else
            {
                cart.Add(id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                TempData["Message"] = "Movie added to cart.";
            }

            return RedirectToAction("Info", new { id });
        }
    }
}