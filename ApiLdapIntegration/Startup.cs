using ApiLdapIntegration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiLdapIntegration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Ldap Options
            services.Configure<LdapConfig>(c =>
            {
                c.ServerHost = Configuration["LdapConfig:ServerHost"];
                c.ServerPort = int.Parse(Configuration["LdapConfig:ServerPort"]);
                c.BaseDN = Configuration["LdapConfig:BaseDN"];
                c.BindDN = Configuration["LdapConfig:BindDN"];
                c.BindPassword = Configuration["LdapConfig:BindPassword"];
                c.UserFilter = Configuration["LdapConfig:UserFilter"];
                c.SecureSocketLayer = bool.Parse(Configuration["LdapConfig:SecureSocketLayer"]);
            });

            services.AddScoped<IAuthenticationService, LdapAuthenticationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
