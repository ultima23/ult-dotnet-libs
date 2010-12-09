using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ult.Core.Utils
{
    public class DateTimeUtils
    {
        /// <summary>
        /// Return a DateTime object containing 
        /// </summary>
        public static DateTime Midnight
        {
            get
            {
                return DateTime.Today.AddDays(1).AddMilliseconds(-1);
            }
        }

        /// <summary>
        /// Sets the time part of a DateTime object to the midnight (23.59.59.999)
        /// </summary>
        /// <param name="date">Date to set</param>
        /// <returns></returns>
        public static DateTime ToMidnight(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }


    }
}
