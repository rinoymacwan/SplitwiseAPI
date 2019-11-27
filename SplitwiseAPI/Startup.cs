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
using SplitwiseAPI.Utility;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using SplitwiseAPI.Utility.Helpers;
using Microsoft.AspNetCore.Identity;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace SplitwiseAPI
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

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

            services.AddScoped<IJwtFactory, JwtFactory>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = false,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            var builder = services.AddIdentityCore<Users>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<SplitwiseAPIContext>().AddDefaultTokenProviders();

            services.AddIdentity<Users, IdentityRole>()
            .AddEntityFrameworkStores<SplitwiseAPIContext>()
            .AddDefaultTokenProviders();

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
            {
                b
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
            }));

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();
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
            app.UseCors("CorsPolicy");

            app.UseSignalR(options =>
            {
                options.MapHub<ChatHub>("/notify");
            });
            app.UseMvc();
        }
    }
}
