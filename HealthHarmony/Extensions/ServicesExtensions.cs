using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Repositories;
using HealthHarmony.Service;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using HealthHarmony.Presentation.Controllers;
using HealthHarmony.Services.Concreate;
using HealthHarmony.Repositories.Concreate;

namespace HealthHarmony.Extensions
{
    public static class ServicesExtensions
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            //User Activities
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();

            //Activities
            services.AddScoped<IActivitiesRepository, ActivitiesManager>();
            services.AddScoped<IActivityService, ActivityService>();

            //DailyData
            services.AddScoped<IDailyDataRepository, DailyDataRepository>();
            services.AddScoped<IDailyDataService, DailyDataService>();

            //Message
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();


            //Coach
            services.AddScoped<ICoachRepository, CoachRepository>();
            services.AddScoped<ICoachService, CoachService>();

            //Food
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IFoodService, FoodService>();

            //GeminiAı
            services.AddHttpClient<IGeminiService, GeminiService>();


        }

        public static void AddControllerPatch(this IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(AuthController).Assembly);
        }

        public static void AddAuthenticationJWT(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
        }

        public static void JWTSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Bearer token kullan",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                        new string[] {}
                    }
                });
            });
        }

        // CORS ayarlarını ekledik
        public static void AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()  // Her yerden gelen isteklere izin verir
                           .AllowAnyMethod()  // Her türlü HTTP metoduna izin verir
                           .AllowAnyHeader()
                           .SetIsOriginAllowed(_ => true); // Herhangi bir header'a izin verir
                });
            });
        }
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
