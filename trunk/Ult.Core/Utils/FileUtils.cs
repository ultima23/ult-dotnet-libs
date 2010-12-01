using System;
using System.IO;

namespace Ult.Util
{
  /// <summary>
  /// 
  /// </summary>
  public class FileUtils
  {
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static byte[] Read(string file)
    {
      byte[] buffer;
      FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
      try
      {
        int length = (int)fileStream.Length;  // get file length
        buffer = new byte[length];            // create buffer
        int count;                            // actual number of bytes read
        int sum = 0;                          // total number of bytes read

        // read until Read method returns 0 (end of the stream has been reached)
        while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
          sum += count;  // sum is a buffer offset for next reading
      }
      finally
      {
        fileStream.Close();
      }
      return buffer;
    }
  
  }
}
