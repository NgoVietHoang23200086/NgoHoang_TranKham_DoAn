using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Services;
using JobBoard.Domain.Interfaces;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Repositories;

namespace JobBoard.Infrastructure
{
    // Extension method để đăng ký tất cả services vào DI Container
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext với SQLite
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký Repositories (Scoped = 1 instance per request)
            services.AddScoped<IJobPostRepository, JobPostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();

            // Đăng ký Application Services
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
