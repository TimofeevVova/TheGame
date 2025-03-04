using CardGame.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CardGame.Controllers
{
    public class DataBaseHelper
    {
        private readonly AppDbContext _context;

        public DataBaseHelper(AppDbContext context)
        {
            _context = context;
        }

        // Получение данных о пользователе
        public User GetUserData(HttpContext httpContext)
        {
            // Получаем JSON из куки
            string userJson = httpContext.Request.Cookies["UserData"];

            if (string.IsNullOrEmpty(userJson))
            {
                return null; // Куки не найдены
            }

            // Десериализуем JSON в объект User
            User user = JsonSerializer.Deserialize<User>(userJson);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}