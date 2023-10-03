using LibBiz.Data;
using Microsoft.EntityFrameworkCore;
using UI.Controllers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<Gateway>();
builder.Services.AddScoped<IBusinessMethods, BusinessMethodsImpl>();

#if CNSTRING_DMYTRO
builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dmytro"), b => b.MigrationsAssembly("UI")));
#else
builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioneConsultas"), b => b.MigrationsAssembly("UI")));
#endif

var logger = new LoggerConfiguration()                                                              // Logging configuration
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>  // Add authentication
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Use your issuer configuration
//        ValidAudience = builder.Configuration["Jwt:Audience"], // Use your audience configuration
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Use your key configuration
//    };
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Adicione a configura��o da sess�o
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Adicione o middleware de sess�o
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.Run();