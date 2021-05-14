using System.Data.Common;
using football.history.api.Domain;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories
{
    public interface IDatabaseConnection
    {
        void Open();
        void Close();
        public DbCommand CreateCommand();
    }
    
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly DbConnection _conn;

        // ReSharper disable once SuggestBaseTypeForParameter
        public DatabaseConnection(DatabaseContext context)
        {
            _conn = context.Database.GetDbConnection();
        }
        
        public void Open()
        {
            _conn.Open();
        }

        public void Close()
        {
            _conn.Close();
        }

        public DbCommand CreateCommand()
        {
            return _conn.CreateCommand();
        }
    }
}