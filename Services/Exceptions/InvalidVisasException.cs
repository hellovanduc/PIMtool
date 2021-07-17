using Resources.Constants;
using System.Collections.Generic;

namespace Services.Exceptions
{
    public class InvalidVisasException : CustomBaseException
    {
        public InvalidVisasException()
        {

        }
        public InvalidVisasException(List<string> invalidVisas)
            : base(Resources.Resources.Resources.Resources.InvalidVisasError + Constants.Colon + string.Join(Constants.Seperator.ToString(), invalidVisas))
        {

        }
    }
}