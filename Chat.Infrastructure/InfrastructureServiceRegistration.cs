using Chat.Application.Contracts.Infrastructure;
using Chat.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasherService, PasswordHasherService>();

            return services;
        }
    }
}
