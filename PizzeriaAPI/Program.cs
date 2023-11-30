using AccountApi;
using AccountApi.Authentication;
using AccountApi.Entities;
using AccountApi.Services;
using Application;
using DataAccess;
using Pizzeria.Service.Abstractions;
using Pizzeria.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IPizzeriaService, PizzeriaService>();
builder.Services.AddScoped<IPizzeriaUserService, PizzeriaUserService>();

builder.Services.AddDbContext<AccountDbContext>();
builder.Services.AddDbContext<PizzeriaDbContext>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

var app = builder.Build();

builder.Services
    .AddApplication()
    .AddDataAccess()
    .AddDomain()
    .AddAccoungAPIAccess();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
