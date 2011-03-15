using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Ult.Util
{
    /// <summary>
    /// Collection of debugging utilities
    /// </summary>
    public class DebugUtils
    {

        /// <summary>
        /// Formats a MethodBase reference sign in a readable format
        /// </summary>
        /// <param name="method">Metod tho format</param>
        /// <returns>method sign string in readable format</returns>
        public static string FormatMethodSign(MethodBase method)
        {
            // Builder
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}.{1}(", method.ReflectedType.FullName, method.Name);
            for (int i = 0; i < method.GetParameters().Length; i++)
            {
                if (i > 0) builder.Append(", ");
                builder.AppendFormat("{0} {1}", method.GetParameters()[i].ParameterType.FullName, method.GetParameters()[i].Name);
            }
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Formats a StackFrame in a readable format
        /// </summary>
        /// <param name="frame">Stack frame to format</param>
        /// <returns></returns>
        public static string FormatStackFrame(StackFrame frame)
        {
            return String.Format("{0} [file:{3}, row:{2}, col:{1}]", FormatMethodSign(frame.GetMethod()),
                                                                     frame.GetFileColumnNumber(),
                                                                     frame.GetFileLineNumber(),
                                                                     frame.GetFileName() ?? "<none>");
        }

    }
}
