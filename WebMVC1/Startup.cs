using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebMVC1.Services;
using Microsoft.EntityFrameworkCore;
using WebMVC1.DBContext;

namespace WebMVC1
{
    public class Startup
    {
        private string _aspnetcoreENV = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            #region API Controller
            services.AddScoped<IConvertService, ConvertService>();
            services.AddScoped<IMailMergeService, MailMergeService>();
            services.AddScoped<ITextEditorService, TextEditorService>();
            services.AddScoped<IFileService, FileService>();
            #endregion

            #region DBContext
            //services.AddControllersWithViews();
            services.AddDbContext<FWContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MSSQLConnectionString")));
            //services.AddDbContext<FWContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MSSQLConnectionString"),
            //    builder => {
            //        builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            //}));
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebMVC1", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Create path docker
            var appPath = Directory.GetCurrentDirectory();
            //template
            var path = Path.Combine(appPath, "template");
            var dir = Directory.Exists(path);
            if (!dir)
            {
                Directory.CreateDirectory(path);
            }
            //template/result
            path = Path.Combine(appPath, "template", "Result");
            dir = Directory.Exists(path);
            if (!dir)
            {
                Directory.CreateDirectory(path);
            }
            //template/data
            path = Path.Combine(appPath, "template", "Data");
            dir = Directory.Exists(path);
            if (!dir)
            {
                Directory.CreateDirectory(path);
            }
            #endregion


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebMVC1 v1"));
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebMVC1 v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "template")
                   ),
                RequestPath = "/staticfiles"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "template")
                    ),
                RequestPath = "/staticfiles"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
