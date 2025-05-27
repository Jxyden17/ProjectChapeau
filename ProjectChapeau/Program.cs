using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Inject Repositories
            builder.Services.AddSingleton<IMenuItemsRepository, MenuItemsRepository>();

            // Inject Services
            builder.Services.AddSingleton<IMenuItemsService, MenuItemsService>();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
