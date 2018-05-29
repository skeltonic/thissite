using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using thissite.Models;
using Newtonsoft.Json.Serialization;


namespace thissite
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
            string ThisSiteConnectionString = Configuration.GetConnectionString("ThisSite");
            services.AddDbContext<ThisSiteDbContext>(opt => opt.UseSqlServer(ThisSiteConnectionString));

            services.AddIdentity<ThisSiteUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                //for testing only, else true
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ThisSiteDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddMvc()
                .AddJsonOptions(
                    options =>
                    {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddTransient((x) => { return new EmailService(Configuration["SendGridKey"]); });

            services.AddTransient((x) => {
                return new Braintree.BraintreeGateway(
                    Configuration["BraintreeEnvironment"],
                    Configuration["BraintreeMerchantId"],
                    Configuration["BraintreePublicKey"],
                    Configuration["BraintreePrivateKey"]);
            });

            services.AddTransient((x) =>
            {
                SmartyStreets.ClientBuilder builder = new SmartyStreets.ClientBuilder(
                    Configuration["SmartyStreetsAuthID"], 
                    Configuration["SmartStreetsAuthToken"]);
                return builder.BuildUsStreetApiClient();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ThisSiteDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            db.Initialize();
        }
    }
}
