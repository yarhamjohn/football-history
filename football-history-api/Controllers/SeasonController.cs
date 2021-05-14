using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Season;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1", Deprecated = true)]
    [ApiVersion("2")]
    public class SeasonController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly ICompetitionRepository _competitionRepository;

        public SeasonController(ISeasonRepository seasonRepository, ICompetitionRepository competitionRepository)
        {
            _seasonRepository = seasonRepository;
            _competitionRepository = competitionRepository;
        }

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("api/v{version:apiVersion}/seasons")]
        public ApiResponse<List<SeasonDto>?> GetAllSeasons()
        {
            try
            {
                var seasons = _seasonRepository.GetAllSeasons().Select(BuildSeasonDto).ToList();
                return new(seasons);
            }
            catch (FootballHistoryException ex)
            {
                return new(
                    Result: null,
                    Error: new(ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                return new(
                    Result: null,
                    Error: new($"Something went wrong. {ex.Message}"));
            }
        }

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("api/v{version:apiVersion}/seasons/{id:long}")]
        public ApiResponse<SeasonDto?> GetSeason(long id)
        {
            try
            {
                var season = _seasonRepository.GetSeason(id);
                return new(BuildSeasonDto(season));
            }
            catch (FootballHistoryException ex)
            {
                return new(
                    Result: null,
                    Error: new(ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                return new(
                    Result: null,
                    Error: new($"Something went wrong. {ex.Message}"));
            }
        }
        
        // Obsolete API method to get all seasons. I would remove it but it demonstrates how api versioning might work.
        [Obsolete]
        [HttpGet]
        [MapToApiVersion("1")]
        [Route("api/v{version:apiVersion}/Season/GetSeasons")]
        public List<Season> GetSeasons() =>
            _seasonRepository.GetAllSeasons().ToList().Select(s =>
                new Season
                (
                    s.StartYear, s.EndYear,
                    _competitionRepository.GetCompetitionsInSeason(s.Id)
                        .Select(c => new Division(c.Name, c.Tier)).ToList()
                )
            ).ToList();
        
        private static SeasonDto BuildSeasonDto(SeasonModel season) =>
            new(season.Id, season.StartYear, season.EndYear);
        
        public record Season(int StartYear, int EndYear, List<Division> Divisions);

        public record Division(string Name, int Tier);
    }
}
