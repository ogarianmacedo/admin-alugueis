using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using ProjetoAlugar.Repositorios;
using System;

namespace ProjetoAlugar
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
            services.AddDbContext<Contexto>(options => options.UseSqlServer(Configuration.GetConnectionString("Conexao")));

            services.AddIdentity<Usuario, NivelAcesso>().AddDefaultUI().AddEntityFrameworkStores<Contexto>();

            services.ConfigureApplicationCookie(opcoes =>
            {
                opcoes.Cookie.HttpOnly = true;
                opcoes.ExpireTimeSpan = TimeSpan.FromMinutes(50);
                opcoes.LoginPath = "/Usuario/Login";
                opcoes.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(opcoes =>
            {
                opcoes.Password.RequireDigit = false;
                opcoes.Password.RequireLowercase = false;
                opcoes.Password.RequireNonAlphanumeric = false;
                opcoes.Password.RequireUppercase = false;
                opcoes.Password.RequiredLength = 6;
                opcoes.Password.RequiredUniqueChars = 1;
            });

            services.AddScoped<INivelAcesso, NivelAcessoRepository>();
            services.AddScoped<IUsuario, UsuarioRepository>();
            services.AddScoped<IEndereco, EnderecoRepository>();
            services.AddScoped<IConta, ContaRepository>();
            services.AddScoped<ICarro, CarroRepository>();
            services.AddScoped<IAluguel, AluguelRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStatusCodePagesWithReExecute("/Erros/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Usuario}/{action=Login}/{id?}");
            });
        }
    }
}
