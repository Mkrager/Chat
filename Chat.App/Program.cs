using Chat.App.Contracts;
using Chat.App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.LoginPath = "/account/login";
        //options.AccessDeniedPath = "/account/denied";
    });

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IChatDataService, ChatDataService>(); 
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();