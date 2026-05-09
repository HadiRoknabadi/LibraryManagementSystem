using DNTCaptcha.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CaptchaConfigs
{
    public static class CaptchaConfig
    {
        public static void AddCaptcha(this IServiceCollection services)
        {
            services.AddDNTCaptcha(options =>
            {
                // options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                options.UseCookieStorageProvider(SameSiteMode.Strict) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                                   // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                                                      // .UseDistributedSerializationProvider()

                // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
                // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
                /*.UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "IRANSans(FaNum)_Bold.ttf"))*/ // This is optional.
                .AbsoluteExpiration(minutes: 7)
                .RateLimiterPermitLimit(10) // for .NET 7x+, Also you need to call app.UseRateLimiter() after calling app.UseRouting().
                .WithRateLimiterRejectResponse("RateLimit Exceeded.") //you can instead provide an object, it will automatically converted to json result.
                .ShowThousandsSeparators(false)
                .WithNoise(0.015f, 0.015f, 1, 0.0f)
                .WithEncryptionKey("60d06279-be34-4ba4-8792-9474971b4e3d")
                .WithNonceKey("NETESCAPADES_NONCE")
                .WithCaptchaImageControllerRouteTemplate("my-custom-captcha/[action]")
                .WithCaptchaImageControllerNameTemplate("my-custom-captcha")
                .InputNames(// This is optional. Change it if you don't like the default names.
                    new DNTCaptchaComponent
                    {
                        CaptchaHiddenInputName = "DNTCaptchaText",
                        CaptchaHiddenTokenName = "DNTCaptchaToken",
                        CaptchaInputName = "DNTCaptchaInputText"
                    })
                .Identifier("dntCaptcha")// This is optional. Change it if you don't like its default name.
                ;
            });
        }
    }
}
