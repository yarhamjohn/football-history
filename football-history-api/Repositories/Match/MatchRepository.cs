using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Exceptions;

namespace football.history.api.Repositories.Match
{
    public interface IMatchRepository
    {
        public List<MatchModel> GetMatches(
            long? competitionId = null,
            long? seasonId = null,
            long? teamId = null,
            string? type = null,
            DateTime? matchDate = null);
        public MatchModel GetMatch(long matchId);
        public List<MatchModel> GetLeagueMatches(long competitionId);
        public List<MatchModel> GetPlayOffMatches(long competitionId);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly IMatchCommandBuilder _queryBuilder;

        public MatchRepository(IDatabaseConnection connection, IMatchCommandBuilder queryBuilder)
        {
            _connection   = connection;
            _queryBuilder = queryBuilder;
        }

        public List<MatchModel> GetMatches(
            long? competitionId = null,
            long? seasonId = null,
            long? teamId = null,
            string? type = null,
            DateTime? matchDate = null)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(
                _connection, competitionId, seasonId, teamId, type, matchDate);
            var matches = GetMatchModels(cmd);
            _connection.Close();
            
            return matches;
        }

        public MatchModel GetMatch(long matchId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, matchId);
            var matches = GetMatchModels(cmd);
            _connection.Close();

            return matches.Count switch
            {
                1 => matches.Single(),
                0 => throw new DataNotFoundException($"No match matched the specified id ({matchId})."),
                _ => throw new DataInvalidException($"{matches.Count} matches matched the specified id ({matchId}).")
            };
        }

        public List<MatchModel> GetLeagueMatches(long competitionId)
        {
            return GetMatches(competitionId, seasonId: null, teamId: null, "League");
        }

        public List<MatchModel> GetPlayOffMatches(long competitionId)
        {
            return GetMatches(competitionId, seasonId: null, teamId: null, "PlayOff");
        }

        private static List<MatchModel> GetMatchModels(DbCommand cmd)
        {
            var matches = new List<MatchModel>();
            
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                matches.Add(GetMatchModel(reader));
            }

            return matches;
        }

        private static MatchModel GetMatchModel(DbDataReader reader)
        => new(
                Id: reader.GetInt64(0),
                MatchDate: reader.GetDateTime(1),
                CompetitionId: reader.GetInt64(2),
                CompetitionName: reader.GetString(3),
                CompetitionStartYear: reader.GetInt16(4),
                CompetitionEndYear: reader.GetInt16(5),
                CompetitionTier: reader.GetByte(6),
                CompetitionRegion: reader.IsDBNull(7) ? null : reader.GetString(7),
                RulesType: reader.GetString(8),
                RulesStage: reader.IsDBNull(9) ? null : reader.GetString(9),
                RulesExtraTime: reader.GetBoolean(10),
                RulesPenalties: reader.GetBoolean(11),
                RulesNumLegs: reader.IsDBNull(12) ? null : reader.GetByte(12),
                RulesAwayGoals: reader.GetBoolean(13),
                RulesReplays: reader.GetBoolean(14),
                HomeTeamId: reader.GetInt64(15),
                HomeTeamName: reader.GetString(16),
                HomeTeamAbbreviation: reader.GetString(17),
                AwayTeamId: reader.GetInt64(18),
                AwayTeamName: reader.GetString(19),
                AwayTeamAbbreviation: reader.GetString(20),
                HomeGoals: reader.GetByte(21),
                AwayGoals: reader.GetByte(22),
                HomeGoalsExtraTime: reader.GetByte(23),
                AwayGoalsExtraTime: reader.GetByte(24),
                HomePenaltiesTaken: reader.GetByte(25),
                HomePenaltiesScored: reader.GetByte(26),
                AwayPenaltiesTaken: reader.GetByte(27),
                AwayPenaltiesScored: reader.GetByte(28));
    }
}
