

namespace Yes.Blog.Endpoints
{
	public static class EndpointValidationExtensions
	{
		public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
		{
			return builder
				.AddEndpointFilter<EndpointValidatorFilter<TRequest>>()
				.ProducesValidationProblem();
		}
	}


	public class EndpointValidatorFilter<T> : IEndpointFilter
	{
		private readonly IValidator<T> _validator;
		public EndpointValidatorFilter(IValidator<T> validator)
		{
			_validator = validator;
		}

		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			T? inputData = context.GetArgument<T>(0);

			if (inputData is not null)
			{
				var validationResult = await _validator.ValidateAsync(inputData);
				if (!validationResult.IsValid)
				{
                    return Results.ValidationProblem(validationResult.ToDictionary(),
													 statusCode: (int)HttpStatusCode.UnprocessableEntity);
				}
			}

			return await next.Invoke(context);
		}
	}
}
