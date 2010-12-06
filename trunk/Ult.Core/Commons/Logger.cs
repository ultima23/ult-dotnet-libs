using System;
using System.Collections.Generic;
using System.Text;

namespace Ult.Commons
{

    /// <summary>
    /// Model to implement to write a ILogEntry
    /// </summary>
    public interface ILogWriter
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        void Setup(Dictionary<string, object> parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Write(ILogEntry entry);

	}

    /// <summary>
    /// Model log entry
    /// </summary>
    public interface ILogEntry
    {

        /// <summary>
        /// 
        /// </summary>
        LogEntryLevel Level
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        DateTime Date
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Message
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ILogEntryParameter[] Parameters
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Format();

    }

    /// <summary>
    /// Entry parameter model
    /// </summary>
    public interface ILogEntryParameter
    {

        string Name
        {
            get;
        }

        string Value
        {
            get;
        }

    }

    /// <summary>
    /// Log entry relevance level
    /// </summary>
    public enum LogEntryLevel : byte
    {
        Debug       = 0,
        Info        = 1,
        Warning     = 2,
        Error       = 3,
        Fatal       = 4
    }

    /// <summary>
    /// Entry standard per logging. La data risulta essere la data di creazione dell'oggetto contiene messaggio da loggare e
    /// i parametri correlati. 
    /// Permette di fornire una formattazione eprsonalizzata, considerando che il primo parametro {0} sarà sempre il livello,
    /// il second parametro {1} sarà sempre la data e il terzo parametro {2] il messaggio
    /// </summary>
    public class LogEntry : ILogEntry
    {

        /// <summary>
        /// Default log entry format
        /// </summary>
        public const string DEFAULT_FORMAT = "[{1:HH:MM:ss.fff}] [{0}] {2}";

        //
        protected LogEntryLevel _level;
        //
        protected DateTime _date;
        //
        protected string _message;
        //
        protected ILogEntryParameter[] _parameters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LogEntry(LogEntryLevel level, string message)
        {
            _date = DateTime.Now;
            _level = level;
            _message = message;
            _parameters = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LogEntry(LogEntryLevel level, string message, object[] message_params)
        {
            _date = DateTime.Now;
            _level = level;
            _message = String.Format(message, message_params);
            _parameters = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public LogEntry(LogEntryLevel level, string message, ILogEntryParameter[] parameters)
        {
            _date = DateTime.Now;
            _level = level;
            _message = message;
            _parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected object[] GetParameters()
        {
            List<object> values = new List<object>();
            foreach (ILogEntryParameter param in _parameters) values.Add(param.Value);
            return values.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public LogEntryLevel Level
        {
            get { return _level; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILogEntryParameter[] Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Format(string format)
        {
            string msg = _parameters != null && _parameters.Length > 0 ? String.Format(Message, GetParameters()) : _message;
            return String.Format(format, _level.ToString().ToUpper(), _date, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            return Format(DEFAULT_FORMAT);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ErrorLogEntry : ILogEntry
    {

        /// <summary>
        /// Forma used to log exceptions, first placeholder is for custom message, second one is for exception type,
        /// the third is for excepion message
        /// </summary>
        protected const string DEFAULT_FORMAT = "[{0,-8}][{1:hh:MM:ss.fff}] {2}. exception: {3}({4})";


        //
        protected LogEntryLevel _level;
        //
        protected string _message;
        //
        protected Exception _error;
        //
        protected DateTime _date;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="error"></param>
        public ErrorLogEntry(LogEntryLevel level, string message, Exception error)
        {
            // Init
            _date = DateTime.Now;
            _level = level;
            _message = message;
            _error = error;
        }



        /// <summary>
        /// 
        /// </summary>
        public LogEntryLevel Level
        {
            get { return _level; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILogEntryParameter[] Parameters
        {
            get { return null; ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error
        {
            get { return _error; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            return String.Format(DEFAULT_FORMAT, _level.ToString(), _date, _message, _error.Message, _error.GetType().Name);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Logger
    {

        // -----------------------------------------------------------------------------------------------------------
        #region STATIC FIELDS

        // Default, singleton istance
        private static Logger _default;

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region FIELDS

        // Writers list
        protected List<ILogWriter> _writers;

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PRIVATE METHODS

        /// <summary>
        /// 
        /// </summary>
        public Logger()
        {
            // Init
            _writers = new List<ILogWriter>();
            // Tracer logger
            Register(new TraceLogger());
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PUBLIC METHODS

        /// <summary>
        /// Register a new log writer
        /// </summary>
        /// <param name="writer"></param>
        public void Register(ILogWriter writer)
        {
            if (!_writers.Contains(writer)) _writers.Add(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        private void Log(ILogEntry entry)
        {
            foreach (ILogWriter writer in _writers)
            {
                writer.Write(entry);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            Log(new LogEntry(LogEntryLevel.Debug, message));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug(string message, params object[] args)
        {
            Log(new LogEntry(LogEntryLevel.Debug, message, args));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void Debug(string message, ILogEntryParameter[] parameters)
        {
            Log(new LogEntry(LogEntryLevel.Debug, message, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message, Exception error)
        {
            Log( new ErrorLogEntry(LogEntryLevel.Debug, message, error) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            Log(new LogEntry(LogEntryLevel.Info, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info(string message, params object[] args)
        {
            Log(new LogEntry(LogEntryLevel.Info, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void Info(string message, ILogEntryParameter[] parameters)
        {
            Log(new LogEntry(LogEntryLevel.Info, message, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message, Exception error)
        {
            Log(new ErrorLogEntry(LogEntryLevel.Info, message, error));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            Log(new LogEntry(LogEntryLevel.Warning, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warning(string message, params object[] args)
        {
            Log(new LogEntry(LogEntryLevel.Warning, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void Warning(string message, ILogEntryParameter[] parameters)
        {
            Log(new LogEntry(LogEntryLevel.Warning, message, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message, Exception error)
        {
            Log(new ErrorLogEntry(LogEntryLevel.Warning, message, error));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Log(new LogEntry(LogEntryLevel.Error, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error(string message, params object[] args)
        {
            Log(new LogEntry(LogEntryLevel.Error, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void Error(string message, ILogEntryParameter[] parameters)
        {
            Log(new LogEntry(LogEntryLevel.Error, message, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message, Exception error)
        {
            Log(new ErrorLogEntry(LogEntryLevel.Error, message, error));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            Log(new LogEntry(LogEntryLevel.Fatal, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal(string message, params object[] args)
        {
            Log(new LogEntry(LogEntryLevel.Fatal, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void Fatal(string message, ILogEntryParameter[] parameters)
        {
            Log(new LogEntry(LogEntryLevel.Fatal, message, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message, Exception error)
        {
            Log(new ErrorLogEntry(LogEntryLevel.Fatal, message, error));
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region STATIC METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Logger GetDefaultLogger()
        {
            if (_default == null)
            {
                _default = new Logger();
            }
            return _default;
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

    }

    /// <summary>
    /// 
    /// </summary>
    public class TraceLogger : ILogWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public void Setup(Dictionary<string, object> parameters) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Write(ILogEntry entry)
        {
            System.Diagnostics.Trace.WriteLine(entry.Format());
        }
    
    }
  
}
