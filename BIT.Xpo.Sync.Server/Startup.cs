using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using Microsoft.Extensions.Options;
using BIT.Xpo.OfflineDataSync;
using BIT.Xpo.OfflineDataSync.AspNetCore;
using Demo.ORM;

namespace BIT.Xpo.Sync.Server
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
            services.AddControllers();

            var LogConnectionString = Configuration.GetConnectionString("LogConnectionString");
            var LocalConnectionString = Configuration.GetConnectionString("ConnectionString");
            SyncDataStore syncDataStore = new SyncDataStore("Master");
            syncDataStore.Initialize(LocalConnectionString, LogConnectionString, true, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, new Assembly[] { typeof(Customer).Assembly });
            SyncDataStore.EnableTransactionHistory = true;
            services.AddSyncDataStore(syncDataStore);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
