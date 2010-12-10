using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
        /// Sets the time value of a DateTime object to the midnight (23.59.59.999)
        /// </summary>
        /// <param name="date">Date to set</param>
        /// <returns></returns>
        public static DateTime ToMidnight(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Sets the time value of a DateTime object just after the midnight (00.00.00.000)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AfterMidnight(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns a list of years starting from current year
        /// </summary>
        /// <param name="nrYears">Nubmer of year to list</param>
        /// <returns>List of years</returns>
        public static int[] GetYearInterval(int nrYears)
        {
            return GetYearInterval(DateTime.Today, 0, nrYears);
        }

        /// <summary>
        /// Returns a list of years starting from current year minus the number of the past year 
        /// </summary>
        /// <param name="nrPastYears"></param>
        /// <param name="nrYears"></param>
        /// <returns>List of years</returns>
        public static int[] GetYearInterval(int nrPastYears, int nrYears)
        {
            return GetYearInterval(DateTime.Today, nrPastYears, nrYears);
        }

        /// <summary>
        /// Returns a list of years starting from the reference date year minus the number of the past year 
        /// </summary>
        /// <param name="reference">Reference date</param>
        /// <param name="nrPastYears">Number of years in the past to go</param>
        /// <param name="nrYears">Number of the years to retrieve</param>
        /// <returns>List of years</returns>
        public static int[] GetYearInterval(DateTime reference, int nrPastYears, int nrYears)
        {
            int[] years = new int[nrYears];
            int starting_year = DateTime.Today.Year - nrPastYears;
            for (int i = 0; i < nrYears; i++)
            {
                years[i] = starting_year + i;
            }
            return years;
        }

        /// <summary>
        /// Gets the list of the names of the months using the current culture info
        /// </summary>
        /// <returns>List of the month names for the current culture</returns>
        public static string[] GetMonthNames()
        {
            return GetMonthNames(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///  Gets the list of the names of the months using the provided culture info
        /// </summary>
        /// <param name="culture">Culture to use to retrieve month names</param>
        /// <returns>List of the month names for the provided culture</returns>
        public static string[] GetMonthNames(CultureInfo culture)
        {
            string[] months = new string[12];
            for (int i = 1; i <= 12; i++)
            {
                months[i - 1] = DateTimeFormatInfo.GetInstance(culture).GetMonthName(i);
            }
            return months;
        }

    }
}
