using Microsoft.EntityFrameworkCore;
using RevOnRental.Domain;
using RevOnRental.Infrastructure.Data;
using RevOnRental.Infrastructure.Identity;
using System.Reflection;
using RevOnRental.Application;
using RevOnRental.SignalR;
using RevOnRental.SignalR.Implementations;
using RevOnRental.SignalR.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RevOnRentalDBConnection"));
});
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBusinessDetailsHandler).Assembly));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

//builder.Services.AddMediatR(typeof(GetBusinessDetailsHandler).Assembly);
//builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
//builder.Services.AddMediatR(cfg => cfg.AsSingleton(), AppDomain.CurrentDomain.GetAssemblies());




var jwtConfig = new JwtIssuerOptions
{
    Issuer = builder.Configuration["JwtConfig:Issuer"],
    Audience = builder.Configuration["JwtConfig:Audience"],
};

var jwtPrivateKey = builder.Configuration["JwtConfig:Key"];


IServiceCollection services = builder.Services;


services.RegisterApplicationDependencyInjection(Assembly.GetExecutingAssembly());
services.AddTransient<IUserConnectionManager, UserConnectionManager>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read CORS settings from appsettings.json
var corsSettings = builder.Configuration.GetSection("Cors");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



builder.Services.AddIdentityInfrastructure();
builder.Services.AddIdentityAuthInfrastructure(jwtPrivateKey, jwtConfig);

builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigins");

app.MapControllers();

app.MapHub<MessageHub>("/message");
app.UseWebSockets();


app.Run();
