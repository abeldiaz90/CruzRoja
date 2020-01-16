using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Globalization;
using System.Collections.Generic;


[assembly: OwinStartup(typeof(mvc.App_Start.Startup))]

namespace mvc.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888
            SetCookieAuthenticationAsDefault(app);
        }
        private void SetCookieAuthenticationAsDefault(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // only allow authenticated users
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();            

   
            services.AddAuthorizationCore(options =>
            {
                // inline policies
                //options.AddPolicy("Ventas", policy =>
                //{
                //    policy.RequireClaim("Administrador", "Administrador");
                //});
                //options.AddPolicy("SalesSenior", policy =>
                //{
                //    policy.RequireClaim("department", "sales");
                //    policy.RequireClaim("status", "senior");
                //});

                options.AddPolicy("Ventas", policy =>
                  policy.RequireRole("Administrador", "Recepcionista"));
            });

           

            //services.AddMvc(setup =>
            //{
            //    setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
            //});
        }

    }
}
