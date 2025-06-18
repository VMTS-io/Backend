using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VMTS.API.ActionFilters;

public class ValidateModelActionFilter<T> : IAsyncActionFilter
    where T : class
{
    private readonly IValidator<T> _validator;

    public ValidateModelActionFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var argument = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
        if (argument == null)
        {
            await next();
            return;
        }

        var validationResult = await _validator.ValidateAsync(argument);
        if (!validationResult.IsValid)
        {
            // Convert errors to ProblemDetails
            var errors = validationResult.ToDictionary();
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
            return;
        }

        await next();
    }
}
