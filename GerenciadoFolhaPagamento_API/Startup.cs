using GerenciadoFolhaPagamento_API.SignalRHub;
using GerenciadorFolhaPagamento_API.Configuration;
using GerenciadorFolhaPagamento_Application.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace GerenciadoFolhaPagamento_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIoCServices();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GerenciadoFolhaPagamento_API", Version = "v1" });
            });

            services.AddHangfire(op =>
            {
                op.UseMemoryStorage();
            });
            services.AddHangfireServer();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IProcessamentoFolhaApplication processamentoFolhaApplication)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GerenciadoFolhaPagamento_API v1"));
            }


            //RecurringJob.AddOrUpdate("Processamento arquivos de folha pagamento", () => processamentoFolhaApplication.IniciaProcessamento(), "*/10 * * * * *");

            app.UseHttpsRedirection();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MainHub>("/processados");
            });
        }
    }
}
