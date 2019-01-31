using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Builders;
using Backend.Domain;
using Backend.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<ILeagueSeasonRepository, LeagueSeasonRepository>();
            services.AddTransient<IDivisionRepository, DivisionRepository>();
            services.AddTransient<IResultMatrixRepository, ResultMatrixRepository>();
            services.AddTransient<ILeagueSeasonFilterBuilder, LeagueSeasonFilterBuilder>();
            services.AddTransient<IResultMatrixBuilder, ResultMatrixBuilder>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<LeagueSeasonContext>(options => options.UseSqlServer(connString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
