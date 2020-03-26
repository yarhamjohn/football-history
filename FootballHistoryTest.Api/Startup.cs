using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using FootballHistoryTest.Api.Repositories.Season;
using FootballHistoryTest.Api.Repositories.Team;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FootballHistoryTest.Api
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
            
            services.AddTransient<ITeamBuilder, TeamBuilder>();
            services.AddTransient<ISeasonBuilder, SeasonBuilder>();
            services.AddTransient<IMatchBuilder, MatchBuilder>();
            services.AddTransient<ILeagueBuilder, LeagueBuilder>();

            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<IPointsDeductionRepository, PointsDeductionRepository>();
            services.AddTransient<ITierRepository, TierRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<TeamRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<SeasonRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<LeagueRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<MatchRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PlayOffMatchRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<PointsDeductionRepositoryContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<TierRepositoryContext>(options => options.UseSqlServer(connString));
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
