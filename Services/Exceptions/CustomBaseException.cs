using System;

namespace Services.Exceptions
{
    /// <summary>
    /// Base class for all custom exceptions
    /// </summary>
    public class CustomBaseException : Exception
    {
        public CustomBaseException()
        {

        }
        public CustomBaseException(string message) : base(message)
        {

        }
    }
}