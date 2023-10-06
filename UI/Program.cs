using LibBiz.Data;
using Microsoft.EntityFrameworkCore;
using UI.Controllers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#if CNSTRING_DANIEL
builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioneConsultas"), b => b.MigrationsAssembly("UI")));
#else
builder.Services.AddDbContext<ddContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dmytro"), b => b.MigrationsAssembly("UI")));
#endif

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllersWithViews();
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
    options.RequireHttpsMetadata = false; // Defina como true em ambiente de produção
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valide o emissor do token
        ValidateAudience = true, // Valide a audiência do token
        ValidateLifetime = true, // Valide o tempo de vida do token
        ValidateIssuerSigningKey = true, // Valide a chave de assinatura

        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Defina o emissor (issuer) válido
        ValidAudience = builder.Configuration["Jwt:Audience"], // Defina a audiência válida
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Defina a chave secreta usada para assinar os tokens
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Adicione a configuração da sessão
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

// Adicione o middleware de sessão
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.Run();