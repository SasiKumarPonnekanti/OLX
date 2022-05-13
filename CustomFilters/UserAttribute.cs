

namespace operation_OLX.CustomFilters
{
    public class UserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
           

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (SecurityServices.IsLoogedIn != true||SecurityServices.UserRole!="User")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else
            {

            }
           
        }
    }
}
