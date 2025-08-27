using Microsoft.OpenApi.Models;
using Chat.Application;
using Chat.Persistence;
using Chat.Identity;
using Chat.Api.Middlewares;
using Chat.Api.Services;
using Chat.Application.Contracts;
using Chat.Api.Hubs;

namespace Chat.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureService(
            this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);

            builder.Services.AddApplicationService();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSignalR();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", policy =>
                {
                    policy.WithOrigins("https://localhost:7195")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return builder.Build();
        }


        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API");
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("Open");

            app.MapHub<NotificationHub>("/hub/chat");

            app.MapControllers();

            return app;
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Chat API",

                });
            });
        }
    }
}