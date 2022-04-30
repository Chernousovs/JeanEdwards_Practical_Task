using System;

namespace JE_PracticalTask.Exceptions
{
    public class MovieDetailsSearchException : Exception
    {
        public MovieDetailsSearchException(string message)
            : base(message)
        {
        }
    }
}
