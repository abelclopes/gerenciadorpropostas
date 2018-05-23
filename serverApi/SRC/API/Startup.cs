using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
//using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

using DOMAIN;
using DOMAIN.Interfaces;
using INFRAESTRUCTURE;
using INFRAESTRUCTURE.Data;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using System.IO;
using Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;

namespace API
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
            
            // services.Configure<FormOptions>(x =>
            // {
            //     x.ValueLengthLimit = int.Parse(Configuration["maxAllowedContentLength:MaxValue"]);
            //     x.MultipartBodyLengthLimit = int.Parse(Configuration["maxAllowedContentLength:MaxValue"]);
            //     x.MultipartHeadersLengthLimit = int.Parse(Configuration["maxAllowedContentLength:MaxValue"]);
            // });

            // services.AddDbContext<ApplicationDbContext>(
            //     options =>options.UseSqlite("Data Source=MvcEmployee.db")
            // );  
           services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                //options.UseMySql(                    
                    Configuration.GetConnectionString("DefaultConnection"),
                    //b => b.MigrationsAssembly(migrationsAssembly)
                    b => b.MigrationsAssembly("API")
                )
            );
          
            ResolveDependencies(services);
          
           // services.AddCors();  
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
           services.ConfigureJwtAuthentication();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    "application/json"
                };
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme
                    ).RequireAuthenticatedUser().Build();
            });

            services.AddMvc();
            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    // Force all ISO8601 timestamp conversions to use UTC (ie: YYYY-MM-DDTHH:MM:SS.FFFZ)
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'";
                });
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //         ValidateIssuerSigningKey = true,
            //         ValidIssuer = Configuration["Jwt:Issuer"],
            //         ValidAudience = Configuration["Jwt:Issuer"],
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //     };
            // });

            ResolveDependencies(services);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                .Build());
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Gerenciador de Propostas API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });
        }
        private static void ResolveDependencies(IServiceCollection services)
        {
            services.AddScoped<IContext, ApplicationDbContext>();
            services.AddScoped<IActivityLog, FileSystemActivityLog>();
            services.AddScoped<IErrorLog, FileSystemErrorLog>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           app.UseResponseCompression();
           /* descomentar codigo abaixo caso for usar o angular na propria aplicação api como single page application */
            // app.Use(async (context, next) =>
            // {
            //     await next();
            //     if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api/"))
            //     {
            //         context.Request.Path = "/index.html";
            //         await next();
            //     }
            // });

            // app.UseMvcWithDefaultRoute();
            // app.UseDefaultFiles();
            // app.UseStaticFiles();


            // app.UseCorsMiddleware();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Seed();
            }
        }
    }
}
