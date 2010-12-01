using System;
using System.IO.Ports;
using Ult.Commons;

namespace Ult.Util
{

  /// <summary>
  /// Collection of utility methods and helpers for SerialPort handling
  /// </summary>
  public class SerialPortUtils
  {
    
    /// <summary>
    /// Count the number of serial port installed
    /// </summary>
    /// <returns>Number of serial ports installed</returns>
    public static int Count()
    {
      return SerialPort.GetPortNames().Length;
    }
    
    /// <summary>
    /// Checks if there are at least one port installed
    /// </summary>
    /// <returns>True if at least one port id present into the system</returns>
    public static bool PortsAvalilable()
    {
      return Count() > 0;
    }
    
    /// <summary>
    /// Checks the existance of a port by his port name
    /// </summary>
    /// <param name="port_name"></param>
    /// <returns></returns>
    public static bool Exists(string port_name)
    {
      foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
      return false;
    }
  
    /// <summary>
    /// Formats SerialPort properties in a readable form
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public static string Format(SerialPort port)
    {
      return Format(port, "{0} ({1},{2},{3},{4},{5})");
    }
  
    /// <summary>
    /// Formats SerialPort properties in a readable form, using a custom provided format. 
    /// 
    /// Markers <-> MasterProperty association
    /// {0}: SerialPort.PortName
    /// {1}: SerialPort.BaudRate
    /// {2}: SerialPort.DataBits
    /// {3}: SerialPort.StopBits
    /// {4}: SerialPort.Parity
    /// {5}: SerialPort.Handshake
    /// {6}: SerialPort.DtrEnable
    /// {7}: SerialPort.RtsEnable
    /// {8}: SerialPort.ReadTimeout
    /// {9}: SerialPort.WriteTimeout
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string Format(SerialPort port, string format)
    {
      if (port == null) return "<null>";
      return String.Format(format,  port.PortName,
                                    port.BaudRate,
                                    port.DataBits,
                                    port.StopBits,
                                    port.Parity,
                                    port.Handshake);
    }
    
    /// <summary>
    /// Copy source SerialPort configuration over the destination port configuration
    /// </summary>
    /// <param name="source">Source SerialPort object from who copy the configuration</param>
    /// <param name="destination">Destination SerialPort object in wich read the configuration</param>
    /// <returns>True if the copy is valid</returns>
    public static bool Copy(SerialPort source, SerialPort destination)
    {
      // Parameters check
      if (source.IsOpen) throw new ArgumentException("source", "Could not perform a configuration copy while source SerialPort is open");
      if (destination.IsOpen) throw new ArgumentException("source", "Could not perform a configuration copy while destination SerialPort is open");
      // 
      bool copied = false;
      //
      try
      {
        // MasterProperty copy
        destination.PortName        = source.PortName;
        destination.BaudRate        = source.BaudRate;
        destination.Parity          = source.Parity;
        destination.DataBits        = source.DataBits;
        destination.StopBits        = source.StopBits;
        destination.Handshake       = source.Handshake;
        destination.ReadTimeout     = source.ReadTimeout;
        destination.WriteTimeout    = source.WriteTimeout;
        destination.ReadBufferSize  = source.ReadBufferSize;
        destination.WriteBufferSize = source.WriteBufferSize;
        destination.DtrEnable       = source.DtrEnable;
        if (destination.Handshake != Handshake.RequestToSend && destination.Handshake != Handshake.RequestToSendXOnXOff)
        {
          destination.RtsEnable = source.RtsEnable;
        }
        // Copy success
        copied = true;
      }
      catch (Exception ex)
      {
        Tracer.Debug(ex);
        copied = false;
      }
      return copied;
    }
    
  }
  
  /// <summary>
  /// serialPort baudrates list
  /// </summary>
  public enum SerialPortDatabits : int
  {
    FiveBits      = 5,
    SeventBits    = 7,
    EightBits     = 8
  }
  
  /// <summary>
  /// serialPort baudrates list
  /// </summary>
  public enum SerialPortBaudRates : int
  {
     BaudRate_75      = 75,
     BaudRate_150     = 150,
     BaudRate_300     = 300,
     BaudRate_600     = 600,
     BaudRate_1200    = 1200,
     BaudRate_2400    = 2400,
     BaudRate_4800    = 4800,
     BaudRate_9600    = 9600,
     BaudRate_19200   = 19200,
     BaudRate_38400   = 38400,
     BaudRate_57600   = 57600,
     BaudRate_115200  = 115200,
     BaudRate_230400  = 230400
  }
  
}
