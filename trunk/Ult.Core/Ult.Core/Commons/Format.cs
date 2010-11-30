using System;
using System.Collections.Generic;
using System.Text;

namespace Ult.Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class Format
    {

        /// <summary>
        /// Stampa la durata in millisecondi
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static string DurationInMilliSeconds(DateTime start)
        {
            // Formattazione
            return DurationInMilliSeconds(start, DateTime.Now);
        }

        /// <summary>
        /// Stampa la durata in millisecondi
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static string DurationInMilliSeconds(DateTime start, DateTime stop)
        {
            // Formattazione
            return String.Format("{1} ms", Convert.ToInt32(stop.Subtract(start).TotalMilliseconds));
        }

        /// <summary>
        /// Stampa la durata in secondi
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static string DurationInSeconds(DateTime start)
        {
            // Formattazione
            return DurationInSeconds(start, DateTime.Now);
        }

        /// <summary>
        /// Stampa la durata in secondi
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static string DurationInSeconds(DateTime start, DateTime stop)
        {
            // Durata
            TimeSpan duration = stop.Subtract(start);
            // Formattazione
            return String.Format("{0} sec {1} ms", Convert.ToInt32(duration.TotalSeconds), duration.Milliseconds);
        }
  
    }
}
