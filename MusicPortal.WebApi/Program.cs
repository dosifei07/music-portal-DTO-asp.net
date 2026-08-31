using Microsoft.AspNetCore.Authentication.Cookies;
using MusicPortal.Infrastructure;

namespace MusicPortal.WebApi
{
    public class Program
    {
        public const string CorsPolicy = "FrontendCors";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMusicPortalContext(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Events.OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                });

            var frontendOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? new[] { "http://localhost:3000" };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins(frontendOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}