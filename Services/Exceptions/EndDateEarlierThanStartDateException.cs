using Library.Resources.Resources;
using System;

namespace Services.Exceptions
{
    public class EndDateEarlierThanStartDateException : CustomBaseException
    {
        public EndDateEarlierThanStartDateException()
        {

        }
        public EndDateEarlierThanStartDateException(DateTime startDate, DateTime endDate)
            : base(CreateMessage(startDate, endDate))
        {

        }
        private static string CreateMessage(DateTime startDate, DateTime endDate)
        {
            return $"{Resources.EndDateEalierThanStartDateError}; " +
                $"{Resources.StartDate}: {startDate.ToShortDateString()}; " +
                $"{Resources.EndDate}: {endDate.ToShortDateString()}";
        }
    }
}