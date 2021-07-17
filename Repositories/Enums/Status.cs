using System.Collections.Generic;

namespace Repositories.Enums
{
    public enum Status
    {
        NEW,    //  New
        PLA,    //  Planning
        INP,    //  In Progress
        FIN     //  Finished
    }
    public static class StatusHelper
    {
        /// <summary>
        /// A string to display Status to user. Ex: Status.FIN => Finished
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StatusText(Status s)
        {
            switch (s)
            {
                case Status.FIN:
                    return Resources.Resources.Resources.Resources.StatusFIN;
                case Status.INP:
                    return Resources.Resources.Resources.Resources.StatusINP;
                case Status.PLA:
                    return Resources.Resources.Resources.Resources.StatusPLA;
                default:
                    return Resources.Resources.Resources.Resources.StatusNEW;
            }
        }

        /// <summary>
        /// Convert a string to Status. Ex: "FIN" => Status.FIN
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Status StringToStatus(string str)
        {
            switch (str)
            {
                case "FIN":
                    return Status.FIN;
                case "INP":
                    return Status.INP;
                case "PLA":
                    return Status.PLA;
                default:
                    return Status.NEW;
            }
        }

        /// <summary>
        /// List all enum Status
        /// </summary>
        /// <returns></returns>
        public static List<Status> GetAllStatus()
        {
            return new List<Status>
            {
                Status.NEW,
                Status.PLA,
                Status.INP,
                Status.FIN
            };
        }
    }
}