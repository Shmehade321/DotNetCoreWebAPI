using System;
using DotNetCoreWebAPI.Data;
using DotNetCoreWebAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreWebAPI.Filters.ExceptionFilters
{
	public class HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
	{
        private readonly ApplicationDbContext _db;

        public HandleUpdateExceptionsFilterAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strShirtId = context.RouteData.Values["id"] as string;
            if(int.TryParse(strShirtId, out int shirtId))
            {
                if (_db.Shirts.FirstOrDefault(x => x.Id == shirtId) == null)
                {
                    context.ModelState.AddModelError("ShirtId", "Shirt doesn't exists anymore.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}

