using System;

namespace Web.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException() { }
        public InvalidParameterException(string message) : base(message) { }
        public InvalidParameterException(string message, Exception ex) : base(message, ex) { }
    }
}
