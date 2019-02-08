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

        public List<LeagueTableRow> GetLeagueTable(int tier, string season, List<MatchDetailModel> playOffMatches)
        {
            var table = new List<LeagueTableRow>();

            using(var conn = Context.Database.GetDbConnection())
            {
                var seasonStartYear = season.Substring(0, 4);
                var seasonEndYear = season.Substring(7, 4);

                var leagueMatchDetails = CommonStuff.GetLeagueMatchDetails(conn, tier, seasonStartYear, seasonEndYear);

                var leagueDetail = GetLeagueDetail(conn, tier, season);
                var pointDeductions = CommonStuff.GetPointDeductions(conn, tier, season);

                CommonStuff.AddLeagueRows(table, leagueMatchDetails);
                CommonStuff.IncludePointDeductions(table, pointDeductions);

                table = CommonStuff.SortLeagueTable(table);

                CommonStuff.SetLeaguePosition(table);
                AddTeamStatus(table, leagueDetail, playOffMatches);            
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
