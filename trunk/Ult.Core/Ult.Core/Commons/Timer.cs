using System;
using System.Collections.Generic;
using System.Text;

namespace Ult.Commons
{
  /// <summary>
  /// 
  /// </summary>
  public class Timer
  {

    // -----------------------------------------------------------------------------------------------------------
    #region STATIC FIELDS

    /// <summary>
    /// 
    /// </summary>
    protected static object _locker = new object();

    /// <summary>
    /// 
    /// </summary>
    private static Dictionary<string, DateTime> _intervalls = new Dictionary<string, DateTime>();
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------
    #region STATIC METHODS

    /// <summary>
    /// Checks a name is already defined for the provided name name
    /// </summary>
    /// <param name="name">Stap name</param>
    /// <returns>True if the name exists</returns>
    public static bool Exists(string name)
    {
      lock (_locker)
      {
        return _intervalls.ContainsKey(name);
      }
    }
  
    /// <summary>
    /// Begins to track an interval with the specified name
    /// </summary>
    /// <param name="name"></param>
    public static void Start(string name)
    {
      lock (_locker)
      {
        DateTime now = DateTime.Now;
        if (!_intervalls.ContainsKey(name))
        {
          _intervalls.Add(name, now);
        }
      }
    }

    /// <summary>
    /// Ends to track the intervall 
    /// </summary>
    /// <param name="name">name of the tracked interval</param>
    public static void Stop(string name)
    {
      lock (_locker)
      {
        if (_intervalls.ContainsKey(name))
        {
          _intervalls.Remove(name);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public static void Reset(string name)
    {
      Stop(name);
      Start(name);
    }

    /// <summary>
    /// Numero di millisecondi trascorsi dall'avvio dell'intervallo
    /// </summary>
    /// <param name="name">Chiave con qui viene salvato l'intervallo</param>
    /// <returns>Numero di millisecondi trascorsi dall'avvio dell'intervallo</returns>
    public static TimeSpan Elapsed(string name)
    {
      lock (_locker)
      {
        return _intervalls.ContainsKey(name) ? DateTime.Now.Subtract(_intervalls[name]) : new TimeSpan();      
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static double Milliseconds(string name)
    {
      return Elapsed(name).TotalMilliseconds;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static string Format(string name, string msg)
    {
      return String.Format("{0} {1} ({2}ms)", new object[] { name, msg, Milliseconds(name) });
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="msg"></param>
    public static void Trace(string name, string msg)
    {
      Tracer.Trace(Format(name, msg));
    }

    #endregion
    // -----------------------------------------------------------------------------------------------------------
  
  }
}
