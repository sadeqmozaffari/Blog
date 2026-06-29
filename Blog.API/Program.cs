using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Blog.Application.Services.Authentication;
using Blog.Application.Services.Category;
using Blog.Application.Services.Image;
using Blog.Application.Services.Post;
using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Domain.UnitOfWork;
using Blog.Infrastructure.Contexts;
using Blog.Infrastructure.Repositories;
using Blog.Infrastructure.UnitOfWork;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSecret = builder.Configuration["JwtSettings:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret))
	throw new InvalidOperationException("JwtSettings:Secret is not configured.");

if (jwtSecret.Length < 32)
	throw new InvalidOperationException("JwtSettings:Secret must be at least 32 characters.");

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddCors();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
	.AddIdentity<ApplicationUser, IdentityRole>(options =>
	{
		options.User.RequireUniqueEmail = true;
		options.Password.RequiredLength = 8;
		options.Password.RequireDigit = true;
		options.Password.RequireLowercase = true;
		options.Password.RequireUppercase = true;
		options.Password.RequireNonAlphanumeric = true;
		options.Lockout.AllowedForNewUsers = true;
		options.Lockout.MaxFailedAccessAttempts = 5;
		options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = !string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Issuer"]),
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidateAudience = !string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Audience"]),
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		NameClaimType = ClaimTypes.Name,
		RoleClaimType = ClaimTypes.Role,
	};

});
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.ReportApiVersions = true;
}).AddApiExplorer(option =>
{
	option.GroupNameFormat = "'v'VVV";
	option.SubstituteApiVersionInUrl = true;
});

foreach (var versionName in new[] { "v1", "v2" })
{
	var displayName = $"Demo API -- {versionName}";

	builder.Services.AddOpenApi(versionName, options =>
	{
		options.AddDocumentTransformer((document, context, cancellationToken) =>
		{
			document.Info = new OpenApiInfo
			{
				Title = "Demo Royal API",
				Version = versionName,
				Description = displayName,
				Contact = new OpenApiContact
				{
					Name = "Bhrugen Patel",
					Email = "hello@dotnetmastery.com"
				}
			};

			document.Components ??= new();
			document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
			{
				["Bearer"] = new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Description = "Enter JWT Bearer token"
				}
			};

			document.SecurityRequirements =
				[
					new OpenApiSecurityRequirement
					{
						[
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							}
						] = []
					}
				];

			return Task.CompletedTask;
		});
	});

}







builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IImageService>(sp =>
{
	var env = sp.GetRequiredService<IWebHostEnvironment>();
	return new ImageServcie(env.WebRootPath);
});

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, Mapper>();
Blog.Application.MappingConfigurations.MappingConfig.Register();

var app = builder.Build();
await SeedDataAsync(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi("/openapi/{documentName}.json");
	var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

	app.MapScalarApiReference(option =>
	{
		option.Title = "Demo -  Blog API";

		var sortedVersion = provider.ApiVersionDescriptions.OrderBy(v => v.ApiVersion).ToList();

		foreach (var description in sortedVersion)
		{
			var versionName = description.GroupName;
			var versionNumber = description.ApiVersion.ToString();
			var displayName = $"Demo API -- {versionNumber}";

			var isDefault = description.ApiVersion.Equals(new ApiVersion(2, 0));
			option.AddDocument(versionName, displayName, $"/openapi/{versionName}.json", isDefault);
		}


	});
}
app.UseStaticFiles();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
app.UseCors(o =>
{
	o.AllowAnyHeader()
		.AllowAnyMethod()
		.WithExposedHeaders("*");

	if (allowedOrigins.Length > 0)
		o.WithOrigins(allowedOrigins);
	else if (app.Environment.IsDevelopment())
		o.AllowAnyOrigin();
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task SeedDataAsync(WebApplication app)
{
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

	await context.Database.MigrateAsync();
}
