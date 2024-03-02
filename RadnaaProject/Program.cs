
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity;
using RadnaaProject.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Home");
    options.Conventions.AuthorizePage("/EditUserInfo");
    options.Conventions.AuthorizePage("/BlogPosts/CreateRecord");
    options.Conventions.AuthorizePage("/BlogPosts/RecordEditor");
    options.Conventions.AuthorizePage("/BlogPosts/ViewUserPosts");
    options.Conventions.AuthorizePage("/BlogPosts/RecordItems");
});
builder.Services.AddDbContext<UserContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<UserContext>();
builder.Services.ConfigureApplicationCookie(o => 
{
    //cookie tohirgoo
    o.Cookie.HttpOnly = true;
    o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    o.LoginPath = "/Login";
    o.AccessDeniedPath = "/Index";
    o.SlidingExpiration = true;
});
builder.Services.Configure<IdentityOptions>(options =>
{
    //password tohirgoo
    //nuuts ug hamgiin bagadaa 6 usegtei bna
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;//zaaval too aguulsan bna
    options.Password.RequireLowercase = true;//zaabal jijig useg aguulsan bna
    //options.Password.RequiredUniqueChars = 1;//zaaval neg temdegt
    options.Password.RequireUppercase = false; //zaaval tom useg aguulsan bna  
    //lockout tohirgoo
    options.Lockout.MaxFailedAccessAttempts = 10;//login hiih oroldlogiin too
    //lock bolson hereglech huleeh hugatsaa
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers = true;
    //user tohirgoo
    options.User.RequireUniqueEmail = false;
    options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234568790@.-_";
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1024 * 1024 * 1024;
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});
var app = builder.Build();


//app.MapGet("/", () => "Hello World!");
app.UseHttpsRedirection();
app.MapRazorPages();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
