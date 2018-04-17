﻿using System;
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
//using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


using DOMAIN;
using DOMAIN.Interfaces;
using INFRAESTRUCTURE;
using INFRAESTRUCTURE.Data;

namespace API
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
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("API")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                //options.UseMySql(                    
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("API")
                )
            );
            
            // ResolveDependencies(services);
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    "application/json"
                };
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

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
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new Info
            //     {
            //         Title = "API Legislação",
            //         Version = "v1"
            //     });
            // });

            services.AddMvc();
        }
        private static void ResolveDependencies(IServiceCollection services)
        {
            //services.AddScoped<IContext, ApplicationDbContext>();
            
            services.AddScoped<DbInitializer>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseMvc();
            // app.UseSwagger();
            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Legislação");
            // });
            // dar uma lida em http://docs.identityserver.io/en/release/quickstarts/8_entity_framework.html?highlight=entity
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var init = scope.ServiceProvider.GetService<DbInitializer>();
                init.SeedData(context);
            }
        }
    }
}
