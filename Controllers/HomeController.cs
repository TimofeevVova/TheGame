using Microsoft.AspNetCore.Mvc;

namespace CardGame.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DataBaseHelper dataBaseHelper) : base(dataBaseHelper)
        {
        }

        // Главная страница
        public IActionResult Index()
        {
            return View();
        }

        // Профиль
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}