using System;

namespace football.history.api.Exceptions
{
    public class TierNotFoundException : Exception
    {
        public TierNotFoundException()
        {
        }

        public TierNotFoundException(string message)
            : base(message)
        {
        }

        public TierNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
