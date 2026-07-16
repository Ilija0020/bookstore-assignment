using BookstoreApplication.Controllers.Middleware;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using BookstoreApplication.Infrastructure.External.ComicVine;
using BookstoreApplication.Infrastructure.Identity;
using BookstoreApplication.Infrastructure.Persistence.Mongo;
using BookstoreApplication.Infrastructure.Persistence.Mongo.Repositories;
using BookstoreApplication.Infrastructure.Persistence.Sql;
using BookstoreApplication.Infrastructure.Persistence.Sql.Repositories;
using BookstoreApplication.Services;
using BookstoreApplication.Services.External;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.Services.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Building Example API", Version = "v1" });

    // Definisanje JWT Bearer autentifikacije
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insert JWT token"
    });

    // Primena Bearer autentifikacije
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


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<MongoDbSettings>>()
        .Value;

    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();

    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();
builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
builder.Services.AddScoped<IBookRepo, BookRepo>();
builder.Services.AddScoped<IAwardRepo, AwardRepo>();
builder.Services.AddScoped<IIssueRepo, ComicNoSqlRepository>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVolumeService, VolumeService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IComicVineConnection, ComicVineConnection>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddHttpClient<ComicVineConnection>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
});

builder.Services.AddAuthentication(options =>
{ // Naglašavamo da koristimo JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true, // Validacija da li je token istekao

        ValidateIssuer = true,   // Validacija URL-a aplikacije koja izdaje token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // URL aplikacije koja izdaje token (čita se iz appsettings.json)

        ValidateAudience = true, // Validacija URL-a aplikacije koja koristi token
        ValidAudience = builder.Configuration["Jwt:Audience"], // URL aplikacije koja koristi token (čita se iz appsettings.json)

        ValidateIssuerSigningKey = true, // Validacija ključa za potpisivanje tokena (koji se koristi i pri proveri potpisa)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), //Ključ za proveru tokena (čita se iz appsettings.json)

        RoleClaimType = ClaimTypes.Role // Potrebno za kontrolu pristupa, što ćemo videti kasnije
    };
});


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
