using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Helpers;
using MovieTeka.Interfaces;
using MovieTeka.Models;
using MovieTeka.Repository;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IMovieActorRepository, MovieActorRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IFavoriteMoviesIdRepository, FavoriteMoviesIdRepository>();
builder.Services.AddScoped<IFavoriteActorsIdRepository, FavoriteActorsIdRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seedata")
{
    Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();