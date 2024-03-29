﻿using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using MovieManager.Data.DataModels;
using MovieManager.Data.DBConfig;
using MovieManager.Services;
using MovieManager.Services.Repositories;
using MovieManager.Services.ServiceContracts;
using MovieManager.Services.ServicesContracts;
using System.Reflection;

namespace MovieManager.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //repo
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            
            //services
            services.AddScoped<ISearchMethodsService, SearchMethodsService>();
            services.AddScoped<IAddToDbService, AddToDbService>();
            services.AddScoped<IGetFromDbService, GetFromDbService>();
            services.AddScoped<ISaveMovieToDbObjectService, SaveMovieToDbObjectService>();
            services.AddScoped<IDeleteFromDbService, DeleteFromDbService>();
            services.AddScoped<IApiGetListsService, ApiGetListsService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddScoped<IUserService, UserService>(); //for admin user edit/view

            services.AddControllersWithViews();

            return services;
        }


        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services)
        {
            //Add connection string
            services.AddDbContext<MovieContext>(options => 
                options.UseSqlServer(Configuration.ConnectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }


        public static IServiceCollection AddIdentityContext(this IServiceCollection services)
        {
            //Add DB and Identity Services
            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MovieContext>();
            
            return services;
        }


        public static IServiceCollection AddRedisCache(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            return services;
        }


        public static IServiceCollection AddFluentValidationWithReflection(this IServiceCollection services)
        {
            services.AddFluentValidation(v =>
            {
                v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }


        public static IServiceCollection AddCookieConsentPolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // Determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // Microsoft.AspNetCore.Http is used here
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return services;
        }
    }
}
