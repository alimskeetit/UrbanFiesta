using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrbanFiesta.Models.Event;

namespace UrbanFiesta.Filters
{
    public class ModelStateIsValid: ActionFilterAttribute
    {
        private readonly string _model;

        public ModelStateIsValid(string model = "viewmodel")
        {
            _model = model;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                
                context.Result = new BadRequestObjectResult(new
                {
                    error = context.ModelState.Values.SelectMany(v => v.Errors).ToList().Select(er => er.ErrorMessage),
                    model = context.ActionArguments.First(arg => arg.Key.ToLower().Contains(_model.ToLower())).Value
                });
            }
        }
    }
}
