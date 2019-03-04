using FootballHistory.Api.Domain;
using FootballHistory.Api.LeagueSeason.LeagueSeasonFilter;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.LeagueSeason.LeagueTableDrillDown;
using FootballHistory.Api.LeagueSeason.PlayOffs;
using FootballHistory.Api.LeagueSeason.ResultMatrix;
using FootballHistory.Api.Repositories.DivisionRepository;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Football History API", Version = "v1" });
            });
            
            services.AddTransient<ILeagueTable, LeagueTable>();
            services.AddTransient<ILeagueTableCalculatorFactory, LeagueTableCalculatorFactory>();
            services.AddTransient<ILeagueTableCalculator, LeagueTableCalculator>();
            
            services.AddTransient<ILeagueTableBuilder, LeagueTableBuilder>();
            services.AddTransient<ILeagueSeasonFilterBuilder, LeagueSeasonFilterBuilder>();
            services.AddTransient<IResultMatrixBuilder, ResultMatrixBuilder>();
            services.AddTransient<IPlayOffMatchesBuilder, PlayOffMatchesBuilder>();
            services.AddTransient<ILeagueTableDrillDownBuilder, LeagueTableDrillDownBuilder>();

            services.AddTransient<IDivisionRepository, DivisionRepository>();
            services.AddTransient<IPlayOffMatchesRepository, PlayOffMatchesRepository>();
            services.AddTransient<ILeagueMatchesRepository, LeagueMatchesRepository>();
            services.AddTransient<ILeagueDetailRepository, LeagueDetailRepository>();
            services.AddTransient<IPointDeductionsRepository, PointDeductionsRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<LeagueDetailRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<DivisionRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PlayOffMatchesRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<LeagueMatchesRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PointDeductionsRepositoryContext>(options => options.UseSqlServer(connString));
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
                app.UseMiddleware<MiddlewareExtensions>();
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
