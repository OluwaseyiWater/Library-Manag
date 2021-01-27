using Library.Data;
using Library.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

namespace Library.Web
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(opts =>
            {
                // Determines whether user consent for non-essential cookies is needed for a given request
                opts.CheckConsentNeeded = context => true;
                opts.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSingleton(Configuration);
            services.AddScoped<ILibraryCardService, LibraryCardService>();
            services.AddScoped<ILibraryBranchService, LibraryBranchService>();
            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IStatusService, StatusService>();

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LibraryConnection")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           // ConsoleLoggerExtensions.AddConsole((ILoggingBuilder)Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // Where is UseHttps();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            // Where is UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute("default",
                    "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}