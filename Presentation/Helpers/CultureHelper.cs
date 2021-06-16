using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PIMTool.Helpers
{
    public class CultureHelper
    {
        private static readonly List<string> cultures = new List<string> {
            "en",  // first culture is the DEFAULT
            "vi", // Vietnam NEUTRAL culture
        };
        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            // make sure it's not null
            if (string.IsNullOrEmpty(name))
            {
                return GetDefaultCulture(); // return Default culture
                                            // make sure it is a valid culture first
            }

            if (cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return name; // accept it
                             // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
                             // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            }
            var n = GetNeutralCulture(name);
            foreach (var c in cultures)
            {
                if (c.StartsWith(n))
                {
                    return c;
                }
            }
            // else 
            // It is not implemented
            return GetDefaultCulture(); // return Default culture as no match found
        }
        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return cultures[0]; // return Default culture
        }
        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }
        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-")) return name;

            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }
    }
}