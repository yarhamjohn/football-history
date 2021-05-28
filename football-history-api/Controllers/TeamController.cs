using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders.Team;
using football.history.api.Exceptions;
using football.history.api.Repositories.Team;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/teams")]
    public class TeamController : Controller
    {
        private readonly ITeamRepository _repository;

        public TeamController(ITeamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ApiResponse<List<TeamDto>?> GetAllTeams()
        {
            try
            {
                var teams = _repository.GetAllTeams()
                    .Select(BuildTeamDto)
                    .ToList();

                return new(teams);
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
        public ApiResponse<TeamDto?> GetTeam(long id)
        {
            try
            {
                var team = _repository.GetTeam(id);
                return new(BuildTeamDto(team));
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

        private static TeamDto BuildTeamDto(TeamModel team) =>
            new(team.Id, team.Name, team.Abbreviation, team.Notes);
    }
}