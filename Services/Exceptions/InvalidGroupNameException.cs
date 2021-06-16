
using Library.Constants;
using Library.Resources.Resources;

namespace Services.Exceptions
{
    public class InvalidGroupNameException : CustomBaseException
    {
        public InvalidGroupNameException()
        {

        }
        public InvalidGroupNameException(string groupName)
            : base(Resources.InvalidGroupNameError + Constants.Colon + groupName)
        {

        }
    }
}