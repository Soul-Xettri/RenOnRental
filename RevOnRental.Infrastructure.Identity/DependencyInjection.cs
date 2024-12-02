using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain;
using RevOnRental.Domain.Models;
using RevOnRental.Infrastructure.Data;
using RevOnRental.Infrastructure.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevOnRental.Application.Services.Users.Command;

namespace RevOnRental.Infrastructure.Identity
{
    public static class DependencyInjection
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();

            //services.AddScoped<IJwtService, JwtService>();
            //services.AddScoped<IUserCommandService, UserCommandService>();
            //services.AddScoped<IUserQueryService, UserQueryService>();
            //services.AddScoped<IUserAuthService, UserAuthService>();
            //services.AddTransient<IUserJwtService, UserJwtService>();
        }

        public static void AddIdentityAuthInfrastructure(this IServiceCollection services, string privateKey, JwtIssuerOptions config)
        {
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));

            //Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = config.Issuer;
                options.Audience = config.Audience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationParamenter = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                RequireExpirationTime = false,
                IssuerSigningKey = _signingKey,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = config.Issuer;
                options.TokenValidationParameters = tokenValidationParamenter;
                options.SaveToken = true;
                // Allow SignalR to receive token in query string
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        //Console.WriteLine($"Access Token: {accessToken}");

                        //var path = context.HttpContext.Request.Path;
                        context.Token = accessToken;

                        //if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/message"))
                        //{
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Access token is missing or invalid path.");
                        //}

                        return Task.CompletedTask;
                    }
                };

            });


            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                //options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            services.AddScoped<IJwtService, JwtService>();









            services.AddAuthorization();
        }
    }
}
