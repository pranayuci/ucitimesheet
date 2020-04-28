using BusinessServices.Contracts;
using BusinessServices.Implementations;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.BO;
using Repository.Contracts;
using Repository.Implementations;
using System;

namespace WebAppCore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            });
        }

        public static void SqlDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionString:dbConnectionString"];
            services.AddDbContextPool<DataContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
            //var connectionString = config["ConnectionString:dbConnectionString"];
            //services.AddDbContextPool<DataContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("dbConnectionString")).EnableSensitiveDataLogging());
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 4;
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.Tokens.EmailConfirmationTokenProvider = "emailconf";
                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.MaxFailedAccessAttempts = 3;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
                 .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureBusinessService(this IServiceCollection services)
        {
            services.AddTransient<IServiceFactory, ServiceFactory>();
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}