using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace operation_OLX.CustomFilters
{
    public class AutherizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
           

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (AccountController.IsLoggedIn != true)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else
            {

            }
           
        }
    }
}
