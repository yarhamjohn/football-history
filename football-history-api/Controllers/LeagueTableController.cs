using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/league-table")]
    public class LeagueTableController : Controller
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;

        public LeagueTableController(
            ICompetitionRepository competitionRepository,
            ILeagueTableBuilder leagueTableBuilder)
        {
            _competitionRepository = competitionRepository;
            _leagueTableBuilder = leagueTableBuilder;
        }

        [HttpGet("competition/{Id:long}")]
        public ApiResponse<LeagueTableDto?> GetLeagueTable(long id)
        {
            try
            {
                var competition = _competitionRepository.GetCompetition(id);
                var leagueTable = _leagueTableBuilder.BuildFullLeagueTable(competition);

                return new(BuildLeagueTableDto(competition, leagueTable));
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

        [HttpGet("season/{seasonId:long}/team/{teamId:long}")]
        public ApiResponse<LeagueTableDto?> GetLeagueTable(long seasonId, long teamId)
        {
            try
            {
                var competition = _competitionRepository.GetCompetitionForSeasonAndTeam(seasonId, teamId);
                if (competition is null)
                {
                    throw new DataNotFoundException($"No competition was found for the specified seasonId ({seasonId}) and teamId ({teamId}).");
                }
                
                var leagueTable = _leagueTableBuilder.BuildFullLeagueTable(competition);

                return new(BuildLeagueTableDto(competition, leagueTable));
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

        private static LeagueTableDto BuildLeagueTableDto(CompetitionModel competition, ILeagueTable leagueTable)
        {
            return new(
                Table: leagueTable.GetRows().OrderBy(x => x.Position).ToList(),
                Competition: BuildCompetitionDto(competition));
        }
        
        private static CompetitionDto BuildCompetitionDto(CompetitionModel competition) =>
            new(competition.Id,
                competition.Name,
                Season: new(
                    competition.SeasonId,
                    competition.StartYear,
                    competition.EndYear),
                competition.Level,
                competition.Comment,
                Rules: new(
                    competition.PointsForWin,
                    competition.TotalPlaces,
                    competition.PromotionPlaces,
                    competition.RelegationPlaces,
                    competition.PlayOffPlaces,
                    competition.RelegationPlayOffPlaces,
                    competition.ReElectionPlaces,
                    competition.FailedReElectionPosition));
    }
}