using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lumina_Learning.Filters;

public class RequireSupabaseAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var supabaseClient = context.HttpContext.RequestServices.GetService<Supabase.Client>();
        
        if (supabaseClient == null)
        {
            context.Result = new ObjectResult(new
            {
                error = "Service Unavailable",
                message = "Supabase is not configured. Please contact the administrator.",
                details = "This API requires Supabase configuration. Add Supabase__Url and Supabase__Key environment variables."
            })
            {
                StatusCode = 503
            };
        }
        
        base.OnActionExecuting(context);
    }
}
