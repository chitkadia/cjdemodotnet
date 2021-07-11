using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FileUpload.Common;
using FileUpload.Repositories;
using FileUpload.Services;
using AuthorizationMiddleware = FileUpload.Common.AuthorizationMiddleware;
using Microsoft.AspNetCore.Authorization;

namespace FileUpload
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
            AddDependencyToCollection(services);
            services.AddTransient<DbContext>(_ => new DbContext(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddMvc();
            _ = services.Configure<KestrelServerOptions>(options =>
              {
                  options.AllowSynchronousIO = true;
              });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileUpload", Version = "v1" });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Policy.Account),
                                  policy => policy.Requirements.Add(new AccountHandler()));
            });

            services.AddSingleton<IAuthorizationHandler, AuthHandler>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "bearer";
                option.DefaultChallengeScheme = "bearer";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware(typeof(AuthorizationMiddleware));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileUpload v1"));
            }
        }

        private void AddDependencyToCollection(IServiceCollection services)
        {
            services.AddTransient<IFileUploadRepository, FileUploadRepository>();
            services.AddTransient<IFileUploadService, FileUploadService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
