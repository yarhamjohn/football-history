using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.TeamRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamRepositoryContext _context;

        public TeamRepository(TeamRepositoryContext context)
        {
            _context = context;
        }
        
        public List<string> GetAllTeams()
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn);
                return SelectAllTeams(cmd);
            }
        }
        
        private static List<string> SelectAllTeams(DbCommand cmd)
        {
            var teams = new List<string>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    teams.Add(reader.GetString(0));
                }
            }

            return teams;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"SELECT c.Name AS TeamName FROM dbo.Clubs AS c";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            return cmd;
        }
    }
}