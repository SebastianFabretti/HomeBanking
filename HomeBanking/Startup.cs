using HomeBanking.Controller;
using HomeBanking.Models;
using HomeBanking.Repositories;
using HomeBanking.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

namespace HomeBanking
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
            //Agregamos el Contexto
            services.AddDbContext<HomeBankingContext>
            (opt => opt.UseSqlServer(Configuration.GetConnectionString("HomeBankingConexion")));
            
            services.AddRazorPages();
            
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);           

            services.AddScoped<IClientRepository, ClientRepository>(); //Agregamos el scope del repositorio del cliente
            
            services.AddScoped<IAccountRepository, AccountRepository>(); //Agregamos el scope del repositorio de las cuentas

            services.AddScoped<ICardRepository, CardRepository>(); //Agregamos el scope del repositorio de las tarjetas

            services.AddScoped<ITransactionRepository, TransactionRepository>(); //Agregamos el scope del repositorio de transaction

            services.AddScoped<AccountsController,  AccountsController>(); //Agregamos el controller de accounts

            services.AddScoped<ClientsController, ClientsController>(); //Agregamos el controller de clients


            //autenticación
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = new PathString("/index.html");
            });

            //autorización
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            //le decimos que use autenticación
            app.UseAuthentication();
            
            //autorización
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>   //Agregamos el endpoint
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=games}/{ action = Get}");
            });
        }
    }
}
