using FootballHistory.Api.Builders;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FootballHistory.Api
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Football  History API", Version = "v1" });
            });
            
            services.AddTransient<ILeagueSeasonBuilder, LeagueSeasonBuilder>();
            services.AddTransient<ILeagueSeasonFilterBuilder, LeagueSeasonFilterBuilder>();
            services.AddTransient<IResultMatrixBuilder, ResultMatrixBuilder>();
            services.AddTransient<IPlayOffMatchesBuilder, PlayOffMatchesBuilder>();
            services.AddTransient<ILeagueTableDrillDownBuilder, LeagueTableDrillDownBuilder>();

            services.AddTransient<IDivisionRepository, DivisionRepository>();
            services.AddTransient<IPlayOffMatchesRepository, PlayOffMatchesRepository>();
            services.AddTransient<ILeagueMatchesRepository, LeagueMatchesRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<ILeagueFormRepository, LeagueFormRepository>();
            services.AddTransient<IPointDeductionsRepository, PointDeductionsRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<LeagueRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PlayOffMatchesContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<LeagueMatchesContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PointDeductionsContext>(options => options.UseSqlServer(connString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("WhitelistedUrls").Get<string[]>());
                });
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseHsts();
            }


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Football History API v1");
            });
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
