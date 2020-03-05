using System.Collections.Generic;
using System.Data.Common;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Team
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamRepositoryContext _context;

        public TeamRepository(TeamRepositoryContext context)
        {
            _context = context;
        }
        
        public List<TeamModel> GetTeamModels()
        {
            using var conn = _context.Database.GetDbConnection();
            
            var cmd = GetDbCommand(conn);
            return SelectAllTeams(cmd);
        }
        
        private static List<TeamModel> SelectAllTeams(DbCommand cmd)
        {
            var teams = new List<TeamModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    teams.Add(new TeamModel { Name = reader.GetString(0), Abbreviation = reader.GetString(1)});
                }
            }

            return teams;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"SELECT Name, Abbreviation FROM dbo.Clubs";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            return cmd;
        }
    }
}