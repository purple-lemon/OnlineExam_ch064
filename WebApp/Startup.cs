using System;
using System.Globalization;
using DAL;
using DAL.Interface;
using DAL.Repositories;
using DAL.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.DB;
using BAL.IoC;
using BAL.Interfaces;
using BAL.Managers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;

namespace WebApp
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

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddLocalization(options => options.ResourcesPath = "Res");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;//no ssl if false
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,//issuer valid if true

                            ValidIssuer = AuthOptions.ISSUER,

                            // consumer valid if true
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,

                            ValidateLifetime = true,

                            //set key
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

                            ValidateIssuerSigningKey = true,
                        };
                    });
            services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedRes));
                })
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("uk")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var config = new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AutoMapperProfileConfiguration());
                });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MainDbContext>();

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<CodeManager, CodeManager>();
            //add dependecy injection for dal repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //Adding DI for managers
            services.AddScoped<ICourseManager, CourseManager>();
            services.AddScoped<IExerciseManager, ExerciseManager>();
            services.AddScoped<ISandboxManager, SandboxManager>(snbx => new SandboxManager(Configuration.GetConnectionString("SandboxAPI")));
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<ICodeManager, CodeManager>();
            services.AddScoped<INewsManager, NewsManager>();
            services.AddScoped<IUserRatingManager, UserRatingManager>();
            services.AddScoped<IMessagesManager, MessagesManager>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;


            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

			app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
			{
				settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
				settings.GeneratorSettings.DefaultEnumHandling = EnumHandling.String;
				settings.GeneratorSettings.Title = "Platform Services";
			});
			app.UseAuthentication();

            dbInitializer.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

			
		}
    }


}