using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace mvc.Models
{
    public class CustomAuthFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.User.Identity.IsAuthenticated)))
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result = new RedirectToRouteResult(
           new RouteValueDictionary
           {
                     { "controller", "Account" },
                     { "action", "Index" }
           });
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller  
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Account" },
                     { "action", "Unauthorized" }
                });
            }
        }
    }
}