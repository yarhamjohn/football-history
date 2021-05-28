using System;

namespace football.history.api.Exceptions
{
    public class FootballHistoryException : Exception
    {
        public string Code { get; }

        protected FootballHistoryException(string message, string code) : base(message)
        {
            Code = code;
        }
    }

    public class DataInvalidException : FootballHistoryException
    {
        public DataInvalidException(string message) : base(message, "DATA_INVALID")
        {
        }
    }

    public class DataNotFoundException : FootballHistoryException
    {
        public DataNotFoundException(string message) : base(message, "DATA_NOT_FOUND")
        {
        }
    }
}