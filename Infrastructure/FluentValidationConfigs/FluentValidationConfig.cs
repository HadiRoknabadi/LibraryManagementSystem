using Application.DTOs.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FluentValidationConfigs
{
    public static class FluentValidationConfig
    {
        public static IServiceCollection AddFluentValidationService(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            #region User

            services.AddScoped<IValidator<AddUserDTO>, AddUserDTOValidator>();
            services.AddScoped<IValidator<EditUserDTO>, EditUserDTOValidator>();

            #endregion

            return services;
        }
    }
}
