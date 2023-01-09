using GerenciadorFolhaPagamento_Application.Applications;
using GerenciadorFolhaPagamento_Data.Repositories;
using GerenciadorFolhaPagamento_Domain.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
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
            services.AddApplicationServices();
            return services;
        }


        private static IServiceCollection AddHelpersService(this IServiceCollection services)
        {
            services.AddScoped<DbSession>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFuncionarioBuilder, FuncionarioBuilder>();
            services.AddScoped<IDepartamentoBuilder, DepartamentoBuilder>();
            services.AddScoped<IParametrosBuilder, ParametrosBuilder>();

            return services;
        }

        private static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {

            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
            services.AddScoped<IParametrosRepository, ParametrosRepository>();
            services.AddScoped<IProcessamentoFolhaRepository, ProcessamentoFolhaRepository>();
            services.AddScoped<IProcessamentoFolhaFuncionarioRepository, ProcessamentoFolhaFuncionarioRepository>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFuncionarioApplication, FuncionarioApplication>();
            services.AddScoped<IDepartamentoApplication, DepartamentoApplication>();
            services.AddScoped<IParametroApplication, ParametroApplication>();
            services.AddScoped<IProcessamentoFolhaApplication, ProcessamentoFolhaApplication>();
            services.AddScoped<IProcessamentoFolhaFuncionarioApplication, ProcessamentoFolhaFuncionarioApplication>();
            return services;
        }

    }
}
