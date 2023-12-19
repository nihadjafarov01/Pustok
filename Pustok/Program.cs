using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PustokDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:MSSql"]);
            }).AddIdentity<AppUser, IdentityRole>(opt =>
            {
            opt.SignIn.RequireConfirmedEmail = false;
            opt.User.RequireUniqueEmail = true;
            opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._";
            opt.Lockout.MaxFailedAccessAttempts = 5;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 4;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();
			builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            PathConstants.RootPath = builder.Environment.WebRootPath;

            app.Run();
        }
    }
}
