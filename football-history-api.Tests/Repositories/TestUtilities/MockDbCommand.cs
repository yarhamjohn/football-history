using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient.Server;

namespace football.history.api.Tests.Repositories.TestUtilities
{
    public class MockDbCommand : DbCommand
    {
        private readonly MockDataParameterCollection _parameters = new();
        private readonly IEnumerable<IDataRecord> _data;
        private readonly long? _scalarResult;

        public MockDbCommand(IEnumerable<IDataRecord>? data = null, long? scalarResult = null)
        {
            _data         = data ?? Enumerable.Empty<IDataRecord>();
            _scalarResult = scalarResult;
        }
        
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection? DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection => _parameters;
        protected override DbTransaction? DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }
        
        public override void Cancel() => throw new NotImplementedException();
        public override int ExecuteNonQuery() => throw new NotImplementedException();
        protected override DbParameter CreateDbParameter() => throw new NotImplementedException();
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => new MockDataReader(_data);
        public override object? ExecuteScalar() => _scalarResult;
        public override void Prepare() => throw new NotImplementedException();
    }
}