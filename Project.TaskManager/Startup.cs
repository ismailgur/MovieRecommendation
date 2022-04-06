using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Data.Context;
using Project.Data.Repository;
using Project.Service.Account;
using Project.Service.MovieServices;
using Project.Service.Security;
using Project.TaskManager.Hangfire;
using System;

namespace Project.TaskManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            #region Configure Hangfire  
            services.AddHangfire(x => x.UseRecommendedSerializerSettings().UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });            
            services.AddHangfireServer(opt =>
            {
                opt.WorkerCount = 1;                
            });
            #endregion


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout(10200);
                    sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Namespace);
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });


            #region DatabaseEnsureCreate

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            new ApplicationDbContext(optionsBuilder.Options).Database.Migrate(); 
            
            #endregion


            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

            ServiceInjections(services);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            #region Configure Hangfire      

            app.UseHangfireDashboard("/hfdashboard",
               new DashboardOptions()
               {
                   DashboardTitle = "Dash",
                   Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter{
                            User = Configuration.GetSection("HangfireSettings:UserName").Value,
                            Pass = Configuration.GetSection("HangfireSettings:Password").Value
                        }
                    }
               });

            #endregion


            RecurringJob.AddOrUpdate<IHangfireService>(x => x.SyncMovies(), Cron.Hourly());


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
            });
        }


        private void ServiceInjections(IServiceCollection services)
        {
            services.AddScoped<IHangfireService, HangfireService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            services.AddScoped<ITokenStoreService, TokenStoreService>();
            services.AddScoped<Project.Service.Logging.ILogger, Project.Service.Logging.Logger>();
        }
    }
}
