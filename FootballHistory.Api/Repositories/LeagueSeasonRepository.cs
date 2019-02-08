using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class LeagueSeasonRepository : ILeagueSeasonRepository
    {
        private LeagueSeasonContext Context { get; }

        public LeagueSeasonRepository(LeagueSeasonContext context)
        {
            Context = context;
        }

        public List<LeagueTableRow> GetLeagueTable(int tier, string season)
        {
            var table = new List<LeagueTableRow>();

            using(var conn = Context.Database.GetDbConnection())
            {
                var seasonStartYear = season.Substring(0, 4);
                var seasonEndYear = season.Substring(7, 4);

                var leagueMatchDetails = CommonStuff.GetLeagueMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                var playOffMatchDetails = GetPlayOffMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                var leagueDetail = GetLeagueDetail(conn, tier, season);
                var pointDeductions = CommonStuff.GetPointDeductions(conn, tier, season);

                CommonStuff.AddLeagueRows(table, leagueMatchDetails);
                CommonStuff.IncludePointDeductions(table, pointDeductions);

                table = CommonStuff.SortLeagueTable(table);

                CommonStuff.SetLeaguePosition(table);
                AddTeamStatus(table, leagueDetail, playOffMatchDetails);            
            }

            return table;
        }

        private void AddTeamStatus(List<LeagueTableRow> leagueTable, LeagueDetail leagueDetail, List<MatchDetailModel> playOffMatchDetails)
        {
            var playOffFinal = playOffMatchDetails.Where(m => m.Round == "Final").ToList();
            
            foreach (var row in leagueTable)
            {
                if (row.Position == 1)
                {
                    row.Status = "C";
                }
                else if (row.Position <= leagueDetail.PromotionPlaces)
                {
                    row.Status = "P";
                }
                else if (playOffFinal.Count == 1 && row.Position <= leagueDetail.PlayOffPlaces + leagueDetail.PromotionPlaces)
                {
                    var final = playOffFinal.Single();
                    var winner = final.PenaltyShootout 
                        ? (final.HomePenaltiesScored > final.AwayPenaltiesScored ? final.HomeTeam : final.AwayTeam) 
                        : final.ExtraTime
                            ? (final.HomeGoalsET > final.AwayGoalsET ? final.HomeTeam : final.AwayTeam) 
                            : (final.HomeGoals > final.AwayGoals ? final.HomeTeam : final.AwayTeam);
                    
                    if (row.Team == winner)
                    {
                        row.Status = "PO (P)";
                    }
                    else
                    {
                        row.Status = "PO";
                    }
                }
                else if (row.Position > leagueDetail.TotalPlaces - leagueDetail.RelegationPlaces)
                {
                    row.Status = "R";
                }
                else
                {
                    row.Status = string.Empty;
                }
            }
        }

        private List<MatchDetailModel> GetPlayOffMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,pom.Round
    ,pom.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,pom.HomeGoals
    ,pom.AwayGoals
    ,pom.ExtraTime AS ExtraTime
    ,pom.HomeGoalsET
    ,pom.AwayGoalsET
    ,pom.PenaltyShootout
    ,pom.HomePenaltiesTaken
    ,pom.HomePenaltiesScored
    ,pom.AwayPenaltiesTaken
    ,pom.AwayPenaltiesScored
FROM dbo.PlayOffMatches AS pom
INNER JOIN dbo.Divisions d ON d.Id = pom.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = pom.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = pom.AwayClubId
WHERE d.Tier = @Tier
    AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var matchDetails = new List<MatchDetailModel>();

            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var extraTime = reader.GetBoolean(9); // == 1 ? true : false;
                    var penaltyShootout = reader.GetBoolean(12); // == 1 ? true : false;

                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Competition = reader.GetString(0),
                            Round = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            HomeTeam = reader.GetString(3),
                            HomeTeamAbbreviation = reader.GetString(4),
                            AwayTeam = reader.GetString(5),
                            AwayTeamAbbreviation = reader.GetString(6),
                            HomeGoals = reader.GetByte(7),
                            AwayGoals = reader.GetByte(8),
                            ExtraTime = extraTime,
                            HomeGoalsET = extraTime ? reader.GetByte(10) : (int?) null,
                            AwayGoalsET = extraTime ? reader.GetByte(11) : (int?) null,
                            PenaltyShootout = penaltyShootout,
                            HomePenaltiesTaken = penaltyShootout ? reader.GetByte(13) : (int?) null,
                            HomePenaltiesScored = penaltyShootout ? reader.GetByte(14) : (int?) null,
                            AwayPenaltiesTaken = penaltyShootout ? reader.GetByte(15) : (int?) null,
                            AwayPenaltiesScored = penaltyShootout ? reader.GetByte(16) : (int?) null
                        }
                    );
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();
            conn.Close();

            return matchDetails;
        }
        
        private LeagueDetail GetLeagueDetail(DbConnection conn, int tier, string season)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,ls.TotalPlaces
    ,ls.PromotionPlaces
    ,ls.PlayOffPlaces
    ,ls.RelegationPlaces
FROM dbo.LeagueStatuses AS ls
INNER JOIN dbo.Divisions d ON d.Id = ls.DivisionId
WHERE d.Tier = @Tier AND ls.Season = @Season
";

            var leagueDetails = new LeagueDetail();

            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@Season", season));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    leagueDetails = new LeagueDetail
                    {
                        Competition = reader.GetString(0),
                        TotalPlaces = reader.GetByte(1),
                        PromotionPlaces = reader.GetByte(2),
                        PlayOffPlaces = reader.GetByte(3),
                        RelegationPlaces = reader.GetByte(4)
                    };
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();
            conn.Close();

            return leagueDetails;
        }
    }
}
