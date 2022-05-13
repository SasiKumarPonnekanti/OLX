namespace operation_OLX.CustomFilters
{
    public class AdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {


        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (SecurityServices.IsLoogedIn != true || SecurityServices.UserRole != "Admin")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else
            {

            }

        }
    }
}
