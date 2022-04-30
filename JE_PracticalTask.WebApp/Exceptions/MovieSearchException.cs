using System;

namespace JE_PracticalTask.Exceptions
{
    public class MovieListSearchException : Exception
    {
        public MovieListSearchException(string message)
            : base(message)
        {
        }
    }
}
