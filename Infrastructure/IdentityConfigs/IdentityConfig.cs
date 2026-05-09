using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities.Account;
using Persistence.Context;
using Application.DTOs.Common;
using Microsoft.AspNetCore.DataProtection;

namespace Infrastructures.IdentityConfigs
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IdentitySettings identitySettings, CookieSettings cookieSettings=null)
        {

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<Role>()
                .AddErrorDescriber<CustomeIdentityError>();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
                options.Password.RequiredLength = identitySettings.PasswordRequiredLength;
                options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
                options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
                options.User.RequireUniqueEmail = identitySettings.UserRequireUniqueEmail;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                //options.Lockout.MaxFailedAccessAttempts = identitySettings.LockoutMaxFailedAccessAttempts;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.LockoutDefaultLockoutTimeSpan);
            });


            if(cookieSettings!=null)
            {
                services.ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieSettings.ExpireTimeSpan);
                    options.LoginPath = cookieSettings.LoginPath;
                    options.LogoutPath = cookieSettings.LogoutPath;
                    options.SlidingExpiration = true;
                });
            }

            services.AddDataProtection()
                .PersistKeysToFileSystem(
                new DirectoryInfo(Directory.GetCurrentDirectory() + "\\wwwroot\\Auth\\"))
                .SetApplicationName("LibraryManagementSystem").SetDefaultKeyLifetime(TimeSpan.FromDays(30));

            return services;
        }
    }
}
