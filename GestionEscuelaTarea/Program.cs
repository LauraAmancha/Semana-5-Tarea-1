using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GestionEscuelaTarea.Models;
using GestionEscuelaTarea.Data;
using GestionEscuelaTarea.Areas.Identity.Data;

namespace GestionEscuelaTarea
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configurar la conexión a MySQL (XAMPP)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 2. Configurar Identity con Roles y Usuario Personalizado
            builder.Services.AddDefaultIdentity<GestionEscuelaTareaUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>() // Habilita el uso de Roles
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // --- CONFIGURACIÓN DE SEGURIDAD (COOKIES) ---
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "EscuelaAuthCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // --- HABILITAR CORS PARA ANGULAR (NUEVO) ---
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirAngular",
                    policy => policy.WithOrigins("http://localhost:4200") // Puerto de Angular por defecto
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });

            // 3. Agregar servicios de MVC y Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // --- BLOQUE PARA CREAR ROLES AUTOMÁTICAMENTE ---
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Administrador", "Alumno" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
            // --- FIN DEL BLOQUE DE ROLES ---

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // --- ACTIVAR CORS ANTES DE LA AUTENTICACIÓN (IMPORTANTE) ---
            app.UseCors("PermitirAngular");

            // 4. Activar Autenticación y Autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}