using Messaging.Responses.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.ExtentionMethods;

namespace WebApp.ActionFilters
{
    public class AuthenticationFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetObject<UserViewModel>("loggedUser") == null)
                context.Result = new RedirectResult("/Users/Login");
        }
    }
}
