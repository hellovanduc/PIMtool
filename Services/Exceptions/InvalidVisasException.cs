using Library.Constants;
using Library.Resources.Resources;
using System.Collections.Generic;

namespace Services.Exceptions
{
    public class InvalidVisasException : CustomBaseException
    {
        public InvalidVisasException()
        {

        }
        public InvalidVisasException(List<string> invalidVisas)
            : base(Resources.InvalidVisasError + Constants.Colon + string.Join(Constants.Seperator.ToString(), invalidVisas))
        {

        }
    }
}