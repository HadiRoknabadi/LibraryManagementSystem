using Application.DTOs.Account;
using Application.DTOs.Author;
using Application.DTOs.Book;
using Application.DTOs.BookCategory;
using Application.DTOs.Publisher;
using Application.DTOs.User;
using Domain.Entities.Book;
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

            #region Account

            services.AddScoped<IValidator<LoginUserDTO>, LoginUserDTOValidator>();


            #endregion

            #region Book Category

            services.AddScoped<IValidator<AddBookCategoryDTO>, AddBookCategoryDTOValidator>();
            services.AddScoped<IValidator<EditBookCategoryDTO>, EditBookCategoryDTOValidator>();


            #endregion

            #region Author

            services.AddScoped<IValidator<AddAuthorDTO>, AddAuthorDTOValidator>();
            services.AddScoped<IValidator<EditAuthorDTO>, EditAuthorDTOValidator>();


            #endregion

            #region Publisher

            services.AddScoped<IValidator<AddPublisherDTO>, AddPublisherDTOValidator>();
            services.AddScoped<IValidator<EditPublisherDTO>, EditPublisherDTOValidator>();


            #endregion

            #region Book

            services.AddScoped<IValidator<AddBookDTO>, AddBookDTOValidator>();


            #endregion


            return services;
        }
    }
}
