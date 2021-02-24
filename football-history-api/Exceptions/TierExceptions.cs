using System;

namespace football.history.api.Exceptions
{
    public class TierNotFoundException : Exception
    {
        public TierNotFoundException(string message)
            : base(message) {}
    }
}
