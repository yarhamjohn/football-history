using System;

namespace football.history.api.Exceptions
{
    public class LeagueModelNotFoundException : Exception
    {
        public LeagueModelNotFoundException(string message): base(message) {}
    }

    public class MultipleLeagueModelsFoundException : Exception
    {
        public MultipleLeagueModelsFoundException(string message): base(message) {}

    }
}
