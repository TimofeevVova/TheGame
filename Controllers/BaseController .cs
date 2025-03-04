using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CardGame;
using CardGame.Models;

namespace CardGame.Controllers
{
    public class BaseController : Controller
    {
        private readonly DataBaseHelper _dataBaseHelper;

        // Внедрение зависимости через конструктор
        public BaseController(DataBaseHelper dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
        }

        // Выполняется перед всеми действиями
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Проверяем, авторизован ли пользователь
            if (filterContext.HttpContext.Request.Cookies.ContainsKey("UserData"))
            {
                // Получаем данные пользователя через DataBaseHelper
                User user = _dataBaseHelper.GetUserData(filterContext.HttpContext);

                // Передаем данные в ViewBag
                ViewBag.UserEmail = user?.Email;
                ViewBag.UserName = user?.Name;
                ViewBag.UserIcon = user?.avatarIcon;

            }
        }
    }
}