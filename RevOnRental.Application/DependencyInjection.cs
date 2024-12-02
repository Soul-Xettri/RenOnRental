using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Users.Command;
using RevOnRental.Application.Services.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterApplicationDependencyInjection(this IServiceCollection services, Assembly assembly)
        {
           
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
