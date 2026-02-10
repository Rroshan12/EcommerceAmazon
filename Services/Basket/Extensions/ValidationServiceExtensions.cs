using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Basket.Validators;
using System.Linq;

namespace Basket.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            // Customize validation error responses for model binding
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(kvp => kvp.Value.Errors.Count > 0)
                        .Select(kvp => new
                        {
                            Field = kvp.Key,
                            Messages = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        })
                        .ToArray();

                    var problem = new
                    {
                        Message = "Validation failed",
                        Errors = errors
                    };

                    var result = new BadRequestObjectResult(problem);
                    result.ContentTypes.Add("application/json");
                    return result;
                };
            });

            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssemblyContaining<ShoppingCartDtoValidator>();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Register MediatR pipeline validation behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Basket.Behaviors.ValidationBehavior<,>));

            return services;
        }
    }
}
