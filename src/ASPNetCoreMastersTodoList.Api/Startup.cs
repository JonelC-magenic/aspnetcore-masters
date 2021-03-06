using ASPNetCoreMastersTodoList.Api.Authorization;
using ASPNetCoreMastersTodoList.Api.Filters;
using DomainModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;
using System.Text;

namespace ASPNetCoreMastersTodoList.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ItemDbContext>(o => {
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ItemDbContext>()
                .AddDefaultTokenProviders();

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:JWT:SecurityKey"]));
            services.Configure<JwtOptions>(o => o.SecurityKey = securityKey);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = securityKey
                    };
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("OnlyCreatorCanEditItem",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsItemCreatorRequirement()
                        ));
            });

            services.AddScoped<IAuthorizationHandler, IsItemCreatorHandler>();

            services.AddControllers(options => {
                options.Filters.Add(new PerformanceFilter());
            });

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemService, ItemService>();

            services.Configure<Authentication>(Configuration.GetSection("Authentication"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}