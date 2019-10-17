using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UsersRepository;
using SplitwiseAPI.Repository.UserFriendMappingsRepository;
using SplitwiseAPI.Repository.CategoriesRepository;
using SplitwiseAPI.Repository.ActivitiesRepository;
using SplitwiseAPI.Repository.GroupsRepository;
using SplitwiseAPI.Repository.GroupMemberMappingsRepository;
using SplitwiseAPI.Repository.ExpensesRepository;
using SplitwiseAPI.Repository.SettlementsRepository;
using SplitwiseAPI.Repository.PayersRepository;
using SplitwiseAPI.Repository.PayeesRepository;

namespace SplitwiseAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserFriendMappingsRepository, UserFriendMappingsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IGroupMemberMappingsRepository, GroupMemberMappingsRepository>();
            services.AddScoped<IExpensesRepository, ExpensesRepository>();
            services.AddScoped<ISettlementsRepository, SettlementsRepository>();
            services.AddScoped<IPayersRepository, PayersRepository>();
            services.AddScoped<IPayeesRepository, PayeesRepository>();
            services.AddDbContext<SplitwiseAPIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SplitwiseAPIContext")));
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
        }
    }
}
