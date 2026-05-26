using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SampleDbContextConnection") ?? throw new InvalidOperationException("Connection string 'SampleDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Registro del Contexto
builder.Services.AddDbContext<WebApplication1.Data.SampleDbContext>();

// 2. CONFIGURACI�N DE IDENTITY
builder.Services.AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<WebApplication1.Data.SampleDbContext>();

// 3. Agregar soporte para p�ginas Razor (necesario para las vistas de login)
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. ORDEN CR�TICO: Authentication primero, luego Authorization
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapeo de rutas (incluye MapRazorPages para el login)
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

