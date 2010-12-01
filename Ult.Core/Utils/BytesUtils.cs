
using System;
using System.Text;
using System.Collections.Generic;
using Ult.Commons;
// using Ult.Protocols.L2;

namespace Ult.Util
{
  /// <summary>
  /// Byte and bytes array utility class
  /// </summary>
  public class BytesUtils
  {
    
    /// <summary>
    /// Checks if a char/byte is a valid ASCII char
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsASCII(byte value)
    {
      return value >= 0x20;
    }

    /// <summary>
    /// Converts bytes to an ASCII string
    /// </summary>
    /// <param name="ascii"></param>
    /// <returns></returns>
    public static string ToASCII(byte data)
    {
      return ToASCII(new byte[] { data });
    }

    /// <summary>
    /// Converts bytes to an ASCII string
    /// </summary>
    /// <param name="ascii"></param>
    /// <returns></returns>
    public static string ToASCII(byte[] data)
    {
      return Encoding.ASCII.GetString(data);
    }

    /// <summary>
    /// Converts bytes to an ASCII string
    /// </summary>
    /// <param name="ascii"></param>
    /// <returns></returns>
    public static string ToASCII(byte[] data, int offset, int count)
    {
      return Encoding.ASCII.GetString(data, offset, count);
    }

    /// <summary>
    /// Gets the first ASCII chart to a byte
    /// </summary>
    /// <param name="ascii"></param>
    /// <returns></returns>
    public static byte ToByte(string ascii)
    {
      return ToByte(ascii, 0);
    }
    
    /// <summary>
    /// Gets a single ASCII char to byte
    /// </summary>
    /// <param name="ascii">ASCII string</param>
    /// <param name="index">Byte index into the ASCII string</param>
    /// <returns></returns>
    public static byte ToByte(string ascii, int index)
    {
      return Encoding.ASCII.GetBytes(ascii.ToCharArray())[index];
    }

    /// <summary>
    /// Converts ASCII string to byte array
    /// </summary>
    /// <param name="ascii">ASCII string</param>
    /// <returns></returns>
    public static byte[] ToBytes(string ascii)
    {
      return Encoding.ASCII.GetBytes(ascii.ToCharArray());
    }
    
    /// <summary>
    /// Formats raw byte into a readable string, replacing non ASCII / non readable
    /// values with hexadecimal rapresentation
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static string Hex(byte raw)
    {
      return Hex(new byte[] { raw });
    }
    
    /// <summary>
    /// Formats a raw bytes array into a readable string, replacing non ASCII / non readable
    /// values with hexadecimal rapresentation
    /// </summary>
    /// <param name="raw">Raw byte array</param>
    /// <returns>A readable string containing the values of the provided byte array</returns>
    public static string Hex(byte[] raw)
    {
      // Buffer
      StringBuilder buffer = new StringBuilder();
      // Message looping
      for (int i = 0; i < raw.Length; i++)
      {
        if (IsASCII(raw[i]))
        {
          buffer.Append(Encoding.ASCII.GetString(raw, i, 1));
        }
        else
        {
          buffer.AppendFormat("[0x{0:x2}]", raw[i]);
        }
      }
      return buffer.ToString().Trim();
    }
  
    /// <summary>
    /// Traces a message using Trace.Trace
    /// </summary>
    /// <param name="message">Message to trace</param>
    /// <param name="arguments">Prameters to be replaced into the message</param>
    public static void Trace(string message, params object[] arguments)
    {
      Tracer.Trace(String.Format(message, arguments));
    }
  
    /// <summary>
    /// Dumps a raw byte array using Tracer.Trace using hex formatting
    /// </summary>
    /// <param name="raw">Raw byte array</param>
    public static void Dump(byte[] raw)
    {
      Trace(Hex(raw));
    }
  
  }
}
