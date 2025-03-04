using CardGame.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



// Добавление фильтра
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomAuthorizeFilter>();
});





// Регистрируем DataBaseHelper как сервис
builder.Services.AddScoped<DataBaseHelper>();



// Подключение Базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


/*
// Маршрут по умолчанию (корневой путь ведёт на страницу логина)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=auth}/{action=login}");
*/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
