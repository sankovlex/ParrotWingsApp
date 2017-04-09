using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using ParrotWings.Data.Core;
using ParrotWings.Data.Core.Repository;
using AutoMapper;
using ParrotWings.WebAPI.MappingProfiles;
using ParrotWings.Services.Account;
using ParrotWings.Services.Secure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ParrotWings.Models.Options;
using ParrotWings.Services.Transactions;
using ParrotWings.Services.Users;
using ParrotWings.Data.Extensions;

namespace ParrotWings.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                //.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Database services
            services.AddScoped<IEfContext, EfContext>();
            services.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(DefaultRepository<>), ServiceLifetime.Scoped));
            services.AddDbContext<EfContext>();

            //Options
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //Custom services
            services.AddScoped<IAuthService, DefaultAuthService>();
            services.AddScoped<IPasswordProtector, DefaultPasswordProtector>();
            services.AddScoped<ITransactionService, DefaultTransactionService>();
            services.AddScoped<IUserService, DefaultUserService>();

            //AutoMapper configure
            var mapperConfiguration = new MapperConfiguration(options =>
            {
                //add profiles from MappingProfiles
                options.AddProfile<DomainToViewProfile>();
                options.AddProfile<ViewToDomainProfile>();
            });
            services.AddSingleton<IMapper>(serviceProvider => mapperConfiguration.CreateMapper());

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<EfContext>().Seed();
            }

            app.Use(async (context, next) =>
            {
                await next();

                // If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html"; // Angular root page
                    context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
                    await next();
                }
            });

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.AppId)),

                ValidateIssuer = true,
                ValidIssuer = appSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = appSettings.Audience,

                ValidateLifetime = true
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = validationParameters
            });

            // Serve wwwroot as root
            app.UseFileServer();

            app.UseMvc();
        }
    }
}
