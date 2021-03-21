using Incrementer.Context;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GreenPipes;
using Incrementer.Services;

namespace Incrementer
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

            services.AddScoped<IRepo, Repo>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Incrementer", Version = "v1" });
            });

            string pgConnString = Configuration.GetSection("ConnectionStrings")["Postgres"];

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<IncrementContext>(ops =>
                    ops.UseNpgsql(pgConnString));


            services.AddMassTransit(x =>
            {
                x.AddConsumer<KvpConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host("rabbit", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("kvpQueue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Immediate(5));
                        e.ConfigureConsumer<KvpConsumer>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IncrementContext ctx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Incrementer v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ctx.Database.Migrate();
        }
    }
}
