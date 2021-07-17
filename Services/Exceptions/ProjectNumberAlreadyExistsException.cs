using Resources.Constants;
using System;

namespace Services.Exceptions
{
    public class ProjectNumberAlreadyExistsException : CustomBaseException
    {
        public ProjectNumberAlreadyExistsException()
        {

        }

        public ProjectNumberAlreadyExistsException(int projectNumber)
            : base(Resources.Resources.Resources.Resources.FollowingProjectNumberAlreadyExist + Constants.Colon + Convert.ToString(projectNumber))
        {

        }
    }
}