using NLog.Web;
using NLog;
using AccountApi;
using AccountApi.Entities;
using AccountApi.Services;
using AccountApi.Middleware;
using AccountApi.Authentication;
using FluentValidation;
using AccountApi.Models;
using AccountApi.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        var builder = WebApplication.CreateBuilder(args);
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

        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddDbContext<AccountDbContext>();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
        builder.Services.AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AccountClient",builder=>
            
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    //.WithOrigins(Configuration["AllowOrigins"])
            );
        });

        var db = new AccountDbContext();
        var app = builder.Build();
        var seeder = new AccountSeeder(db);
        seeder.Seed();

        // Configure the HTTP request pipeline.

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AccountClient");
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}