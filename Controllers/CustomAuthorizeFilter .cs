using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
{
    // Фильтр проверяющий авторизацию, а именно куки и если нет записей, то идет перенаправление на страницу входа
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var path = context.HttpContext.Request.Path;

        // Исключаем корневой путь и страницы логина и регистрации из проверки
        if (path.StartsWithSegments("/auth/Login", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/auth/Register", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Проверяем, авторизован ли пользователь через куки
        if (!context.HttpContext.Request.Cookies.ContainsKey("UserData"))
        {
            // Перенаправление на страницу входа
            context.Result = new RedirectResult("/auth/Login");
        }
    }
}