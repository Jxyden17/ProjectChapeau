using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Services;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Repositories;

namespace ProjectChapeau
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
            builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddSingleton<ITableService, TableService>();
            builder.Services.AddSingleton<ITableRepository, TableRepository>();

            builder.Services.AddSingleton<IRoleService, RoleService>();
            builder.Services.AddSingleton<IRoleRepository, RoleRepository>();

            builder.Services.AddSingleton<IOrderService, OrderService>();
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

            builder.Services.AddSingleton<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddSingleton<IOrderItemService, OrderItemService>();

            builder.Services.AddSingleton<IMenuItemRepository, MenuItemRepository>();
            builder.Services.AddSingleton<IMenuItemService, MenuItemService>();

            builder.Services.AddSingleton<IFinancialService, FinancialService>();


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
