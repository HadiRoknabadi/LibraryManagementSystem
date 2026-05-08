using Application.DTOs.Common;
using Infrastructure.FluentValidationConfigs;
using Infrastructure.MappingProfiles;
using Infrastructure.WebMarkupMinConfigs;
using Infrastructures.IdentityConfigs;
using Infrastructures.IoC;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebMarkupMin.AspNetCoreLatest;

var builder = WebApplication.CreateBuilder(args);

var siteSettings = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddIdentityService(siteSettings.IdentitySettings);
builder.Services.RegisterServices();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserMappingProfile).Assembly);
});

builder.Services.AddFluentValidationService();

#region Html Encoder

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

#endregion

builder.Services.AddWebMarkupMinServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseWebMarkupMin();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
