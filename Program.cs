using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication1.Context;
using WebApplication1.Dtos.Mapping;
using WebApplication1.Filter;
using WebApplication1.Models;
using WebApplication1.RateLimit;
using WebApplication1.Repositories;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Enter a valid secret key");

builder.Services.AddControllers(opt => { opt.Filters.Add(typeof(ApiExceptionFilter)); })
  .AddJsonOptions(opt => { opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddCors(opt =>
{
  opt.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins("https://apirequest.io")
      .WithMethods("GET");
  });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "catalog-api", Version = "v1" });

  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
  {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Bearer JWT"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
  opt.SaveToken = true;
  opt.RequireHttpsMetadata = false;
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero,
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
  };
});

builder.Services.AddAuthorization(opt =>
{
  opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
  opt.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
  opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var myOptions = new RateLimitOptions();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
  opt.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
});

builder.Configuration.GetSection(RateLimitOptions.RateLimit).Bind(myOptions);

builder.Services.AddRateLimiter(opt =>
{
  opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

  opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(http =>
    RateLimitPartition.GetFixedWindowLimiter(
      partitionKey: http.User.Identity?.Name ?? http.Request.Headers.Host.ToString(),
      factory: _ => new FixedWindowRateLimiterOptions
      {
        AutoReplenishment = myOptions.AutoReplenishment,
        PermitLimit = myOptions.PermitLimit,
        QueueLimit = myOptions.QueueLimit,
        Window = TimeSpan.FromSeconds(myOptions.Window)
      }
    ));
});

builder.Services.AddApiVersioning(opt =>
{
  opt.DefaultApiVersion = new ApiVersion(1, 0);
  opt.AssumeDefaultVersionWhenUnspecified = true;
  opt.ReportApiVersions = true;
  opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
}).AddApiExplorer(opt =>
{
  opt.GroupNameFormat = "'v'VVV";
  opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnityOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseRouting();
app.UseRateLimiter();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
