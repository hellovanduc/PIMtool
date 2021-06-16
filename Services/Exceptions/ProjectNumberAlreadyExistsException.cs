using Library.Constants;
using Library.Resources.Resources;
using System;

namespace Services.Exceptions
{
    public class ProjectNumberAlreadyExistsException : CustomBaseException
    {
        public ProjectNumberAlreadyExistsException()
        {

        }

        public ProjectNumberAlreadyExistsException(int projectNumber)
            : base(Resources.FollowingProjectNumberAlreadyExist + Constants.Colon + Convert.ToString(projectNumber))
        {

        }
    }
}