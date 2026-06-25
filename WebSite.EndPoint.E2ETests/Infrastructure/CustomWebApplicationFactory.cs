using Domain.Entities.Account;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace WebSite.EndPoint.E2ETests.Infrastructure
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // حذف دیتابیس واقعی
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // جایگزینی با InMemory Database
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("E2E_Test_Database");
                });

                // Mock کردن سرویس کپچا
                var captchaDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDNTCaptchaValidatorService));

                if (captchaDescriptor != null)
                {
                    services.Remove(captchaDescriptor);
                }

                services.AddSingleton<IDNTCaptchaValidatorService, FakeCaptchaValidatorService>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    db.Database.EnsureCreated();

                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();

                    var testPhone = "09123456789";
                    var testPassword = "Password123!";

                    var existingUser = userManager.FindByNameAsync(testPhone).Result;
                    if (existingUser == null)
                    {
                        var user = new User
                        {
                            UserName = testPhone,
                            PhoneNumber = testPhone,
                            PhoneNumberConfirmed = true,
                            Name = "Test",
                            Family = "User"
                        };

                        var result = userManager.CreateAsync(user, testPassword).Result;

                        if (!result.Succeeded)
                        {
                            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                            throw new Exception($"Failed to seed test user: {errors}");
                        }
                    }
                }
            });
        }
    }

    // Mock سرویس کپچا که همیشه معتبر برمی‌گرداند
    public class FakeCaptchaValidatorService : IDNTCaptchaValidatorService
    {
        public bool HasRequestValidCaptchaEntry()
        {
            return true;
        }
    }
}
