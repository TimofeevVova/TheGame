using Microsoft.AspNetCore.Mvc;

namespace CardGame.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DataBaseHelper dataBaseHelper) : base(dataBaseHelper)
        {
        }

        // ������� ��������
        public IActionResult Index()
        {
            return View();
        }

        // �������
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