using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute() { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // With RouteData.Values dictionary we can get the values produced by routes on the current routing path

            // Get the name of the action and controller
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            // ActionArguments dictionary to extract the DTO parameter that we send to the PUT and POST actions
            // Going through the ActionArguments dictionary and getting the first one with a value whose object
            // has a name that contains Dto then get the value
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

            // If no parameter was sent send a bad request
            if(param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }

            // If there were inconsistencies with passed in model
            if(!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}
