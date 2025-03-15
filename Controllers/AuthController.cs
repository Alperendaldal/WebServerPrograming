using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebServerPrograming.Models;

namespace WebServerPrograming.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                // Kullanıcı bilgilerini çerez olarak kaydet
                var user = new User { FirstName = firstName, LastName = lastName };
                string userJson = JsonSerializer.Serialize(user);
                Response.Cookies.Append("UserInfo", userJson, new CookieOptions { Expires = System.DateTime.Now.AddMonths(1) });

                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Lütfen adınızı ve soyadınızı girin!";
            return View();
        }
    }
}
