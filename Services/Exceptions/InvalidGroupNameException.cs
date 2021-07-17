
using Resources.Constants;

namespace Services.Exceptions
{
    public class InvalidGroupNameException : CustomBaseException
    {
        public InvalidGroupNameException()
        {

        }
        public InvalidGroupNameException(string groupName)
            : base(Resources.Resources.Resources.Resources.InvalidGroupNameError + Constants.Colon + groupName)
        {

        }
    }
}