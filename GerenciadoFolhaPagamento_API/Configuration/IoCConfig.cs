using GerenciadorFolhaPagamento_Data.Repositories;
using GerenciadorFolhaPagamento_Domain.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciadorFolhaPagamento_API.Configuration
{
    public static class IoCConfig
    {
        public static IServiceCollection AddIoCServices(this IServiceCollection services)
        {
            services.AddHelpersService();
            services.AddRepositoriesServices();
            return services;
        }


        private static IServiceCollection AddHelpersService(this IServiceCollection services)
        {
            services.AddScoped<DbSession>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFuncionarioBuilder, FuncionarioBuilder>();

            return services;
        }

        private static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();

            return services;
        }

    }
}
