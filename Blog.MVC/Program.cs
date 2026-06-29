using Blog.Common;
using Blog.MVC;
using Blog.MVC.Services;
using Blog.MVC.Services.IServices;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
	option.IdleTimeout = TimeSpan.FromMinutes(60);
	option.Cookie.HttpOnly = true;
	option.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.HttpOnly = true;
		options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
		options.SlidingExpiration = true;
		options.LoginPath = "/auth/login";
		options.AccessDeniedPath = "/auth/accessdenied";
	});

SD.APIBaseUrl = builder.Configuration.GetValue<string>("ServiceUrls:BlogAPI")
	?? throw new InvalidOperationException("ServiceUrls:BlogAPI is not configured.");
builder.Services.AddHttpClient("BlogAPI", client =>
{
	var blogAPIUrl = SD.APIBaseUrl;
	client.BaseAddress = new Uri(blogAPIUrl);
	client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, Mapper>();
MappingConfig.Register();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
