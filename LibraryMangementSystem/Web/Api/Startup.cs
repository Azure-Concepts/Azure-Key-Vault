using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KeyVault.Core;
using LMS.Auth;
using LMS.Data;
using LMS.Data.Repository;
using LMS.Data.StorageEntities;
using LMS.Shared.Configuration;
using LMS.Shared.Spec;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api
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
            services.AddMvc();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            services.Configure<KeyVaultConfiguation>(configuration.GetSection("KeyVault"));
            services.Configure<AzureStorageConfiguration>(configuration.GetSection("AzureStorage"));

            var tokenProvider = new AzureServiceTokenProvider();
            services.AddSingleton<IApplicationAuthorizationContext>(sp => new ApplicationAuthorizationContext(tokenProvider));

            services.AddSingleton(typeof(KeyVaultConfiguation), (sp) => sp.GetService<IOptions<KeyVaultConfiguation>>().Value);
            services.AddSingleton<IKeyStoreReader, KeyStoreReader>();

            services.AddSingleton(typeof(KeyVaultConfiguation), (sp) =>
            {
                var settings = sp.GetService<IOptions<AzureStorageConfiguration>>().Value;
                settings.AccountKey = sp.GetService<IKeyStoreReader>().GetSecretAsync("storage-key").Result;
                return settings;
            });


            services.AddDbContext<LibraryContext>((sp, options) => 
            {
                var connectionString = sp.GetService<IKeyStoreReader>().GetSecretAsync("db-connection-string").Result;
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LMS.Api"));
            });
            services.AddScoped<ITableService<BookCheckoutEntity>, TableService<BookCheckoutEntity>>();
            services.AddScoped<IBookRespository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}