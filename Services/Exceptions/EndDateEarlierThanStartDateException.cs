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
            return $"{Resources.Resources.Resources.Resources.EndDateEalierThanStartDateError}; " +
                $"{Resources.Resources.Resources.Resources.StartDate}: {startDate.ToShortDateString()}; " +
                $"{Resources.Resources.Resources.Resources.EndDate}: {endDate.ToShortDateString()}";
        }
    }
}