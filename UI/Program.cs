using LibBiz.Data;
using Microsoft.EntityFrameworkCore;
using UI.Controllers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Mudar connection string
//builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dmytro"), b => b.MigrationsAssembly("UI")));
builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioneConsultas"), b => b.MigrationsAssembly("UI")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<Gateway>();

builder.Services.AddScoped<IBusinessMethods, BusinessMethodsImpl>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT authentication options here
});

// Adicione a configuração da sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Adicione o middleware de sessão
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.Run();