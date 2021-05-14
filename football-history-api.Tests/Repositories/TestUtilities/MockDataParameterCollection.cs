using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;

namespace football.history.api.Tests.Repositories.TestUtilities
{
    public class MockDataParameterCollection : DbParameterCollection
    {
        private readonly Collection<DbParameter> _collection = new();

        public override int Count => _collection.Count;
        public override object SyncRoot { get; }

        public override int Add(object value)
        {
            _collection.Add((DbParameter) value);
            return _collection.Count;
        }

        protected override DbParameter GetParameter(int index) => _collection[index];
        protected override DbParameter GetParameter(string parameterName) 
            => _collection
                .FirstOrDefault(p => 
                    p.ParameterName.Equals(parameterName, StringComparison.CurrentCultureIgnoreCase)) 
               ?? throw new InvalidOperationException();
        
        public override IEnumerator GetEnumerator() => _collection.GetEnumerator();

        public override void Clear() => throw new NotImplementedException();
        public override bool Contains(object value) => throw new NotImplementedException();
        public override int IndexOf(object value) => throw new NotImplementedException();
        public override void Insert(int index, object value) => throw new NotImplementedException();
        public override void Remove(object value) => throw new NotImplementedException();
        public override void RemoveAt(int index) => throw new NotImplementedException();
        public override void RemoveAt(string parameterName) => throw new NotImplementedException();
        public override int IndexOf(string parameterName) => throw new NotImplementedException();
        public override bool Contains(string value) => throw new NotImplementedException();
        public override void CopyTo(Array array, int index) => throw new NotImplementedException();
        public override void AddRange(Array values) => throw new NotImplementedException();
        
        protected override void SetParameter(int index, DbParameter value) => throw new NotImplementedException();
        protected override void SetParameter(string parameterName, DbParameter value) =>
            throw new NotImplementedException();

    }
}