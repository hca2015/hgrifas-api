using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using api_lrpd.Models;
using api_lrpd.Util;
using api_lrpd.Util.Framework.Middlewares;
using api_lrpd.Util.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace api_lrpd;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        builder.Services.Configure<RouteOptions>(x =>
        {
            x.LowercaseUrls = true;
        });

        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration.WriteTo.Console();
            configuration.WriteTo.Debug();
        });

        ConfigurationManager configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        //builder.Services.AddScoped(typeof(ContextoExecucao));
        builder.ConfigurarServices();

        // For Entity Framework
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("Default");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        // For Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Lockout = new()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                MaxFailedAccessAttempts = 3                
            };

            options.User = new()
            {
                RequireUniqueEmail = true,
            };

            options.SignIn = new SignInOptions()
            {
                RequireConfirmedAccount = false,
                RequireConfirmedEmail = false,
                RequireConfirmedPhoneNumber = false
            };

            options.Password = new()
            {
                RequireUppercase = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequiredUniqueChars = 0,
                RequiredLength = 1
            };
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Adding Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // Adding Jwt Bearer
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
        {
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        //builder.Services.AddHealthChecks();
        builder.Services.RegisterMaps();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(options =>
        {
            options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        var logger = app.Services.GetRequiredService<Serilog.ILogger>();
        app.ConfigureExceptionHandler(logger);        

        Inicializador.ConfigurarBanco(app);

        app.Map("/", () =>
        {
            return "Olá mundo";
        });

        app.Run();
    }    
}

