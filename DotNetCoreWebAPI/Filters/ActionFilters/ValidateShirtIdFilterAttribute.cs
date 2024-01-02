using DotNetCoreWebAPI.Data;
using DotNetCoreWebAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreWebAPI.Filters
{
    public class ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;

        public ValidateShirtIdFilterAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirtId = context.ActionArguments["id"] as int?;
            if (shirtId.HasValue)
            {
                if(shirtId.Value < 0)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId is Invalid");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var shirt = _db.Shirts.Find(shirtId.Value);

                    if(shirt == null)
                    {
                        context.ModelState.AddModelError("ShirtId", "ShirtId doesn't exists");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["shirt"] = shirt;
                    }
                }
            }
        }
    }
}
