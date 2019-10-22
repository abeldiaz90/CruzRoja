using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Data.Entity;
using mvc.Models;
using System.Security.Principal;

namespace mvc
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            //Database.SetInitializer<Contexto>(new DropCreateDatabaseIfModelChanges<Contexto>());
        }
        protected void Application_Error(object sender, EventArgs e)
        {            
            Exception ex = Server.GetLastError();
            Server.Transfer("~/throws/index");
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
                return;

            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            // retrieve roles from UserData
            string[] roles = authTicket.UserData.Split(';');

            if (Context.User != null)
                Context.User = new GenericPrincipal(Context.User.Identity, roles);
        }
    }
}