using AccountApi;
using AccountApi.Authentication;
using AccountApi.Entities;
using AccountApi.Middleware;
using AccountApi.Models.Validators;
using AccountApi.Models;
using AccountApi.Services;
using Application;
using DataAccess;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Pizzeria.Service.Abstractions;
using Pizzeria.Service.Services;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);



builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});


//Nlog
builder.Logging.ClearProviders();
builder.Host.UseNLog();
// Add services to the container.
builder.Services.AddScoped<AccountSeeder>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton(authenticationSettings);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddScoped<IPizzeriaService, PizzeriaService>();
builder.Services.AddScoped<IPizzeriaUserService, PizzeriaUserService>();

builder.Services.AddDbContext<AccountDbContext>();
builder.Services.AddDbContext<PizzeriaDbContext>();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

builder.Services.AddEndpointsApiExplorer();

var db = new AccountDbContext();
var seeder = new AccountSeeder(db);
seeder.Seed();



builder.Services
    .AddApplication()
    .AddDataAccess()
    .AddDomain()
    .AddAccountAPIAccess();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AccountClient");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapControllers();

app.Run();
