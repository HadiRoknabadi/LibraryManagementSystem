using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCoreLatest;

namespace Infrastructure.WebMarkupMinConfigs
{
    public static class WebMarkupMinConfig
    {
        public static void AddWebMarkupMinServices(this IServiceCollection services)
        {
            services.AddWebMarkupMin(options =>
            {
                options.AllowCompressionInDevelopmentEnvironment = true;
                options.AllowMinificationInDevelopmentEnvironment = true;
            }).AddHtmlMinification(options=>
            {
                options.MinificationSettings.RemoveHtmlComments = true;
                options.MinificationSettings.RemoveHtmlCommentsFromScriptsAndStyles = true;
                options.MinificationSettings.MinifyEmbeddedCssCode = true;
                options.MinificationSettings.MinifyEmbeddedJsCode = true;
                options.MinificationSettings.MinifyInlineCssCode = true;
                options.MinificationSettings.MinifyInlineJsCode = true;
            })
            .AddHttpCompression();
        }
    }
}
