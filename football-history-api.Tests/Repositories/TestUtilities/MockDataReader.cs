using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace football.history.api.Tests.Repositories.TestUtilities
{
    public class MockDataReader : DbDataReader
    {
        private readonly IEnumerator<IDataRecord> _records;
    
        public override int FieldCount { get; }
        public override int RecordsAffected { get; }
        public override bool HasRows { get; }
        public override bool IsClosed { get; }
        public override int Depth { get; }
        
        public MockDataReader(IEnumerable<IDataRecord> records)
        {
            _records = records.GetEnumerator();
        }
        
        public override bool Read() => _records.MoveNext();

        public override object this[int ordinal] => throw new NotImplementedException();
        public override object this[string name] => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal) => false;
        public override byte GetByte(int ordinal) => (byte)ordinal;
        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => throw new NotImplementedException();
        public override char GetChar(int ordinal) => throw new NotImplementedException();
        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => throw new NotImplementedException();
        public override string GetDataTypeName(int ordinal) => throw new NotImplementedException();
        public override DateTime GetDateTime(int ordinal) => new(ordinal, ordinal, ordinal);
        public override decimal GetDecimal(int ordinal) => throw new NotImplementedException();
        public override double GetDouble(int ordinal) => throw new NotImplementedException();
        public override Type GetFieldType(int ordinal) => throw new NotImplementedException();
        public override float GetFloat(int ordinal) => throw new NotImplementedException();
        public override Guid GetGuid(int ordinal) => throw new NotImplementedException();
        public override short GetInt16(int ordinal) => (short) ordinal;
        public override int GetInt32(int ordinal) => ordinal;
        public override long GetInt64(int ordinal) => ordinal;
        public override string GetName(int ordinal) => throw new NotImplementedException();
        public override int GetOrdinal(string name) => throw new NotImplementedException();
        public override string GetString(int ordinal) => ordinal.ToString();
        public override object GetValue(int ordinal) => throw new NotImplementedException();
        public override int GetValues(object[] values) => throw new NotImplementedException();
        public override bool IsDBNull(int ordinal) => false;
        public override bool NextResult() => throw new NotImplementedException();
        public override IEnumerator GetEnumerator()  => throw new NotImplementedException();
    }
}