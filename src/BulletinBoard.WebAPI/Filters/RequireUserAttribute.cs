using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BulletinBoard.WebAPI.Filters
{
    /// <summary>
    /// Дія доступна тільки з заголовком X-User-Id (ціле число).
    /// Без нього запит не доходить до контролера і отримує 401 Unauthorized.
    /// Чи існує такий користувач, перевіряє вже BLL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string value = context.HttpContext.Request.Headers[ApiHeaders.UserId];
            int userId;
            if (!int.TryParse(value, out userId))
            {
                context.Result = new UnauthorizedObjectResult(new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = "Потрібен заголовок " + ApiHeaders.UserId + " з номером користувача."
                });
            }
        }
    }
}
