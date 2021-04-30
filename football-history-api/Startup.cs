using System.Linq;
using System.Reflection;
using football.history.api.Builders;
using football.history.api.Domain;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;
using football.history.api.Repositories.Season;
using football.history.api.Repositories.Team;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace football.history.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITeamBuilder, TeamBuilder>();
            services.AddTransient<ISeasonBuilder, SeasonBuilder>();
            services.AddTransient<IMatchBuilder, MatchBuilder>();
            services.AddTransient<ILeagueBuilder, LeagueBuilder>();
            services.AddTransient<IPositionBuilder, PositionBuilder>();

            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<IPointsDeductionRepository, PointsDeductionRepository>();
            services.AddTransient<ITierRepository, TierRepository>();

            var connString = Configuration.GetConnectionString("FootballHistory");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connString));
            
            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true; 
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Football History API v1",
                            Version = "v1"
                        });
            
                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                options.DocInclusionPredicate((version, desc) => 
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;

                    var versions = methodInfo.DeclaringType!
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = methodInfo
                        .GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versions.Any(v => $"v{v.ToString()}" == version)
                           && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version));
                });
            });
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
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}