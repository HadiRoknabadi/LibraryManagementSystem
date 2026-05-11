using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Infrastructures.IoC
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, ApplicationDbContext>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IRoleService,RoleService>();
            services.AddScoped<IBookCategoryService,BookCategoryService>();
            services.AddScoped<IAuthorService,AuthorService>();
            services.AddScoped<IPublisherService,PublisherService>();
            services.AddScoped<IBookService,BookService>();


            return services;
        }
    }
}