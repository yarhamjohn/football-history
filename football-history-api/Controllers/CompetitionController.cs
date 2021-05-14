using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/competitions")]
    public class CompetitionController : Controller
    {
        private readonly ICompetitionRepository _repository;

        public CompetitionController(ICompetitionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ApiResponse<List<CompetitionDto>?> GetAllCompetitions()
        {
            try
            {
                var competitions = _repository
                    .GetAllCompetitions()
                    .Select(BuildCompetitionDto)
                    .ToList();
                return new(competitions);
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

        [HttpGet("{id:long}")]
        public ApiResponse<CompetitionDto?> GetCompetition(long id)
        {
            try
            {
                var match = _repository.GetCompetition(id);
                return new(BuildCompetitionDto(match));
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

        [HttpGet("season/{seasonId:long}")]
        public ApiResponse<List<CompetitionDto>?> GetCompetitions(long seasonId)
        {
            try
            {
                var matches = _repository
                    .GetCompetitionsInSeason(seasonId)
                    .Select(BuildCompetitionDto)
                    .ToList();
                return new(matches);
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
