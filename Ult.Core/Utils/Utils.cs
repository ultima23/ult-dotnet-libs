using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace Ult.Util
{
  /// <summary>
  /// 
  /// </summary>
  public class Utils
  {

    /// <summary>
    /// Divisione di due interi, arrotndata per eccesso se c'è resto
    /// </summary>
    /// <param Description="dividend">Dividendo</param>
    /// <param Description="divisor">Divisore</param>
    /// <returns>Divisione arrotondata per eccesso</returns>
    public static short Division(short dividend, short divisor)
    {
      // calcolo divisione
      short div = (short)(dividend / divisor);
      // Calcolo resto
      short mod = (short)(dividend % divisor);
      // Risultato arrotondato
      return mod != 0 ? (short)(div + 1 ) : div;
    }

    /// <summary>
    /// Divisione di due interi, arrotndata per eccesso se c'è resto
    /// </summary>
    /// <param Description="dividend">Dividendo</param>
    /// <param Description="divisor">Divisore</param>
    /// <returns>Divisione arrotondata per eccesso</returns>
    public static int Division(int dividend, int divisor)
    {
      // calcolo divisione
      int div = dividend / divisor;
      // Calcolo resto
      int mod = dividend % divisor;
      // Risultato arrotondato
      return mod != 0 ? div + 1 : div;
    }
  
    /// <summary>
    /// Perform a integer calculation of the percentage, ceiling the result
    /// </summary>
    /// <param name="total"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    public static int Percentage(int total, int current)
    {
      return Division(current * 100, total);
    }
  
    /// <summary>
    /// Deep copies a source array into a new array
    /// </summary>
    /// <param name="source">Source array</param>
    /// <returns>A copy of the source array</returns>
    public static T[] Clone<T>(T[] source) where T : class
    {
      T[] destination = new T[source.Length];
      Array.Copy(source, destination, source.Length);
      return destination;
    }
  
    /// <summary>
    /// Deep copy reference provided into a new referece
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="referene"></param>
    /// <returns></returns>
    public static T Clone<T>(T referene)
    {
      // Result object
      object result = null;
      // Stream
      MemoryStream ms = new MemoryStream();
      // Serializzation
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(ms, referene);
      ms.Position = 0;
      // Deserialize
      result = (T) formatter.Deserialize(ms);
      ms.Close();
      // Copy
      return (T)result;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static E ToEnum<E>(string value)  
    {  
      if( !typeof(E).IsEnum )throw new NotSupportedException("E must be an Enum");
      return (E) Enum.Parse( typeof(E), value );
    }  
  
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IList<T> ListEnum<T>()
    {
      if( !typeof(T).IsEnum )throw new NotSupportedException("E must be an Enum");
      IList<T> list = new List<T>();
      foreach (object value in Enum.GetValues(typeof (T)))  
      {  
          list.Add((T) value);  
      }  
      return list;  
    } 
    
    /// <summary>
    /// Metodo centralizzato per l'inviocazione degli EventHandler delegati alla gestione degli eventi.
    /// </summary>
    /// <param Description="Event">Evento da invocare</param>
    /// <param Description="sender">Sender dell'evento</param>
    public static void Invoke(EventHandler handler, EventArgs args, object sender)
    {
      // Verifica handler
      if (null != handler)
      {
        // Scorrimento invoker
        foreach (EventHandler singleCast in handler.GetInvocationList())
        {
          // Verifica tipo di invocazione
          ISynchronizeInvoke syncInvoke = singleCast.Target as ISynchronizeInvoke;
          if ((null != syncInvoke) && (syncInvoke.InvokeRequired))
          {
            syncInvoke.Invoke(singleCast, new object[] { sender, new EventArgs() });
          }
          else
          {
            singleCast(sender, new EventArgs());
          }
        }
      }
    }
  
    /// <summary>
    /// Metodo centralizzato per l'inviocazione degli EventHandler delegati alla gestione degli eventi.
    /// </summary>
    /// <param Description="Event">Evento da invocare</param>
    /// <param Description="sender">Sender dell'evento</param>
    public static void Invoke(ref EventHandler Event, object sender)
    {
      // Casting a EventHandler
      EventHandler handler = Event;
      // Verifica handler
      if (null != handler)
      {
        // Scorrimento invoker
        foreach (EventHandler singleCast in handler.GetInvocationList())
        {
          // Verifica tipo di invocazione
          ISynchronizeInvoke syncInvoke = singleCast.Target as ISynchronizeInvoke;
          if ((null != syncInvoke) && (syncInvoke.InvokeRequired))
          {
            syncInvoke.Invoke(singleCast, new object[] { sender, new EventArgs() });
          }
          else
          {
            singleCast(sender, new EventArgs());
          }
        }
      }
    }
  
  }
}
