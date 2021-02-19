using football.history.api.Builders;
using football.history.api.Calculators;
using football.history.api.Domain;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;
using football.history.api.Repositories.Season;
using football.history.api.Repositories.Team;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace football.history.api
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
            services.AddSwaggerGen(
                c =>
                    {
                        c.SwaggerDoc(
                            "v1",
                            new OpenApiInfo
                            {
                                Title = "Football History API",
                                Version = "v1"
                            });
                    });

            services.AddTransient<ITeamBuilder, TeamBuilder>();
            services.AddTransient<ISeasonBuilder, SeasonBuilder>();
            services.AddTransient<IMatchBuilder, MatchBuilder>();
            services.AddTransient<ILeagueBuilder, LeagueBuilder>();
            services.AddTransient<IPositionBuilder, PositionBuilder>();
            services.AddTransient<ILeagueTableBuilder, LeagueTableBuilder>();
            services.AddTransient<IDateCalculator, DateCalculator>();

            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<IPointsDeductionRepository, PointsDeductionRepository>();
            services.AddTransient<ITierRepository, TierRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connString));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(
                    builder =>
                        {
                            builder.WithOrigins(
                                Configuration.GetSection("WhitelistedUrls").Get<string[]>());
                        });
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Football History API v1"));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
