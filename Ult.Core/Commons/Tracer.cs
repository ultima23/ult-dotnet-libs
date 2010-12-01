#define TRACE

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Ult.Core.Utils;

namespace Ult.Commons
{
  /// <summary>
  /// Utility class, collects 
  /// </summary>
  public class Tracer
  {

    // -----------------------------------------------------------------------------------------------------------
    #region CONSTANTS
      
    /// <summary>
    /// 
    /// </summary>
    protected const string __HEAVY_DECORATOR  = "-- ----------------------------------------------------------------------------- --";
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------
    #region STATIC FIELDS
    
    /// <summary>
    /// Debug method lock
    /// </summary>
    private static object debug_lock = new object();
    
    /// <summary>
    /// Header added on top of the string used to filter messages
    /// </summary>
    public static string Header;
    
    /// <summary>
    /// 
    /// </summary>
    public static string TimeStampFormat = "HH:mm:ss.fff";
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------
    #region PRIVATE STATIC METHODS

    /// <summary>
    /// Builds message to trace
    /// </summary>
    /// <param name="message">Message to build</param>
    /// <returns>message formatted</returns>
    private static string Build(string message)
    {
      string timestamp = DateTime.Now.ToString(TimeStampFormat);
      return String.IsNullOrEmpty(Header) ? String.Format("[{0}] {1}", timestamp, message) : String.Format("[{0}] [{1}] {2}", Header, timestamp, message);
    }
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------
    #region STATIC METHODS

    /// <summary>
    /// Prints an empty new line into System.Diagnostics.Trace 
    /// </summary>
    public static void Line()
    {
      System.Diagnostics.Trace.WriteLine(string.Empty);
    }
    
    /// <summary>
    /// Prints the provided message string into System.Diagnostics.Trace 
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    public static void Trace(string msg)
    {
      System.Diagnostics.Trace.WriteLine(Build(msg));
    }

    /// <summary>
    /// Formats the message with the provided values and prints it into System.Diagnostics.Trace 
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="values">Values to format into the message</param>
    public static void Trace(string msg, params object[] values)
    {
      Trace(String.Format(msg, values));
    }

    /// <summary>
    /// Prints the provided message string into System.Diagnostics.Trace
    /// and, if specified, adds a new emptu line after it
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="addEmptyLine">Flag to add new empty line after message</param>
    public static void Trace(string msg, bool addEmptyLine)
    {
      System.Diagnostics.Trace.WriteLine(msg);
      if (addEmptyLine) Line();
    }
    
    /// <summary>
    /// Prints an important message into System.Diagnostics.Trace, modifying it to upper case
    /// and spacing it from the rest of the spam with empty lines
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    public static void Heavy(string msg)
    {
      Line();
      Trace(msg.ToUpper());
      Line();
    }

    /// <summary>
    /// Formats the message with the provided values and prints it into System.Diagnostics.Trace, modifying it to upper case
    /// and spacing it from the rest of the spam with empty lines
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="values">Values to format into the message</param>
    public static void Heavy(string msg, params object[] values)
    {
      Heavy(String.Format(msg, values));
    }

    /// <summary>
    /// Formats the message with the provided values and prints it into System.Diagnostics.Trace, modifying it to upper case
    /// and spacing it from the rest of the spam with empty lines
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="strings">Strings to insert into the message</param>
    public static void Heavy(string msg, params string[] strings)
    {
      Heavy(String.Format(msg, strings));
    }

    /// <summary>
    /// Prints a very important message into System.Diagnostics.Trace, modifying it to upper case
    /// and spacing it from the rest of the spam with empty lines and decorators
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    public static void VeryHeavy(string msg)
    {
      Trace(String.Empty);
      Trace(__HEAVY_DECORATOR);
      Trace(msg.ToUpper());
      Trace(__HEAVY_DECORATOR);
      Trace(String.Empty);
    }

    /// <summary>
    /// Formats the message with the provided values and prints it into System.Diagnostics.Trace, modifying it to upper case
    /// and spagin it from the rest of the spam with empty lines and decorators
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="values">Values to format into the message</param>
    public static void VeryHeavy(string msg, params object[] values)
    {
      VeryHeavy(String.Format(msg, values));
    }

    /// <summary>
    /// Formats the message with the provided values and prints it into System.Diagnostics.Trace, modifying it to upper case
    /// and spagin it from the rest of the spam with empty lines and decorators
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    /// <param name="strings">String to insert into the message</param>
    public static void VeryHeavy(string msg, params string[] strings)
    {
      VeryHeavy(String.Format(msg, strings));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="error"></param>
    /// <param name="printStack"></param>
    public static void Debug(Exception error)
    {
      lock (debug_lock)
      {
        try
        {
          // Stack stack
          StackTrace stack = new StackTrace(true);
          // Caller
          StackFrame caller = stack.GetFrame(1);
          // Message
          Line();
          Trace(":: EXCEPTION :: START :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
          Trace(":: Exception : {0} ({1})", error.Message ?? "<empty>", error.GetType().Name);
          Trace(":: File      : {0}", caller.GetFileName());
          Trace(":: Row       : {0}", caller.GetFileLineNumber());
          Trace(":: Column    : {0}", caller.GetFileColumnNumber());
          Trace(":: Caller    : {0}", DebugUtils.FormatMethodSign(caller.GetMethod()));
          Trace(":: Stack     : {0:D2}. at {1}", 1, DebugUtils.FormatStackFrame(stack.GetFrame(1)));
          // stack
          for (int i=2; i<stack.GetFrames().Length;i++)
          {
              Trace("::             {0:D2}. at {1}", i, DebugUtils.FormatStackFrame(stack.GetFrame(i)));          
          }
          Trace(":: EXCEPTION :: STOP ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
          Line();
        }
        catch (Exception ex)
        {
          Trace(String.Format("Tracer.Debug(string msg, Exception error) IN ERROR: {0}", ex.Message));
        }
      }
    }

    #endregion
    // -----------------------------------------------------------------------------------------------------------

  }
}
