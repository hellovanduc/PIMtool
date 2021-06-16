namespace Repositories.Enums
{
    /// <summary>
    /// The sort order of list project
    /// </summary>
    public enum SortOrder
    {
        number,
        number_desc,
        name,
        name_desc,
        status,
        status_desc,
        customer,
        customer_desc,
        date,
        date_desc
    }
    public class SortOrderHelper
    {
        /// <summary>
        /// Convert a string to Enum SortOrder
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static SortOrder StringToSortOrder(string sortOrder)
        {
            switch (sortOrder)
            {
                case "number_desc":
                    return SortOrder.number_desc;
                case "name":
                    return SortOrder.name;
                case "name_desc":
                    return SortOrder.name_desc;
                case "status":
                    return SortOrder.status;
                case "status_desc":
                    return SortOrder.status_desc;
                case "customer":
                    return SortOrder.customer;
                case "customer_desc":
                    return SortOrder.customer_desc;
                case "date":
                    return SortOrder.date;
                case "date_desc":
                    return SortOrder.date_desc;
                default:
                    return SortOrder.number;
            }
        }
    }
}