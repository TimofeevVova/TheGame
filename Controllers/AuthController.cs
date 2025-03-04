using CardGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CardGame.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        //Установка куки
        private void setCoocies(User user)
        {
            // Сериализуем объект User в JSON
            string userJson = JsonSerializer.Serialize(user);

            // Устанавливаем куки
            var options = new CookieOptions
            {
                HttpOnly = true, // Куки доступны только на сервере
                Expires = DateTime.UtcNow.AddHours(1), // Время жизни куки
                Secure = true, // Передавать куки только по HTTPS
                SameSite = SameSiteMode.Strict // Защита от CSRF-атак
            };

            Response.Cookies.Append("UserData", userJson, options);
        }

        // Страница Входа
        [AllowAnonymous]
        [HttpGet("login")] // Явно указываем маршрут
        public IActionResult Login()
        {
            ViewData["ShowHeader"] = false; // Указываем, что хедер не нужен
            return View();
        }

        // Страница Регистрации
        [AllowAnonymous]
        [HttpGet("register")] // Явно указываем маршрут
        public IActionResult Register()
        {
            ViewData["ShowHeader"] = false; // Указываем, что хедер не нужен
            return View();
        }

        // Регистрация нового пользователя
        [HttpPost("register")]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return BadRequest("Пароли не совпадают!");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                return BadRequest("Пользователь с таким email уже существует!");
            }

            var user = new User
            {
                Email = email,
                Password = password // Позже добавим хэширование!
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Устанавливаем куки для авторизованного пользователя
            setCoocies(user);          

            return new RedirectResult("/Home/Index");
        }


        // Авторизация
        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return BadRequest(new { message = "Пользователь с таким email не найден." });
            }

            // Пока проверяем пароль без хэширования (позже добавим хэширование)
            if (user.Password != password)
            {
                return BadRequest(new { message = "Неверный пароль." });
            }

            // Устанавливаем куки для авторизованного пользователя
            setCoocies(user);

            // Перенаправление на главную страницу
            return Json(new { success = true});
        }

        /*
        // Вход как временный пользователь
        [HttpPost("temp-login")]
        public IActionResult TempLogin()
        {
            string tempUsername = "Guest" + new Random().Next(1000, 9999);

            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("Username", tempUsername, options);

            return Json(new { success = true, username = tempUsername });
        }

        // Выход временного пользователя
        [HttpPost("exit-temp-login")]
        public IActionResult ExitTempLogin()
        {
            Response.Cookies.Delete("Username");
            return Json(new { success = true });
        }
        */
        // Проверка на авторизацию
        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            if (Request.Cookies.ContainsKey("Username"))
            {
                string username = Request.Cookies["Username"];
                return Json(new { isAuthenticated = true, username });
            }

            return Json(new { isAuthenticated = false });
        }

        //Выход
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Удаляем куки
            Response.Cookies.Delete("UserData");

            // Перенаправляем на страницу авторизации
            return RedirectToAction("login", "auth");
        }
    }
}