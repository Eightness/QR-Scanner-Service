using System;
using System.Collections.Concurrent;
using System.IO;

namespace QRScanner.utility
{
    /// <summary>
    /// Singleton Logger class to handle application logs.
    /// Provides thread-safe logging with support for different log levels (INFO, WARNING, ERROR).
    /// Logs are stored in memory and appended to a file in the "out" directory.
    /// </summary>
    public sealed class QRScannerLogger
    {
        #region Attributes and instances

        private static readonly Lazy<QRScannerLogger> _instance = new(() => new QRScannerLogger());
        private readonly ConcurrentQueue<string> _logs = new();
        private int _logCounter = 0; // Counter for the number of logs
        private readonly string _logFilePath; // File path for the logs

        /// <summary>
        /// Provides the singleton instance of the Logger.
        /// </summary>
        public static QRScannerLogger Instance => _instance.Value;

        #endregion

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// Sets up the log file in the "out" directory.
        /// </summary>
        private QRScannerLogger()
        {
            // Define the log file path inside the "out" folder
            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            string logDirectory = Path.Combine(projectDirectory, "out");

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            _logFilePath = Path.Combine(logDirectory, "logs.txt");

            // Initialize the log file if it doesn't exist
            if (!File.Exists(_logFilePath))
                File.Create(_logFilePath).Close();
        }

        #region Methods

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            AddLog("INFO", message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            AddLog("WARNING", message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            AddLog("ERROR", message);
        }

        /// <summary>
        /// Adds a log entry to the in-memory queue and appends it to the log file.
        /// </summary>
        /// <param name="level">The log level (e.g., INFO, WARNING, ERROR).</param>
        /// <param name="message">The log message.</param>
        private void AddLog(string level, string message)
        {
            int currentLogNumber = ++_logCounter; // Increment and get the current log count
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string formattedLog = $"[{currentLogNumber}] [{timestamp}] [{level}]: {message}";
            _logs.Enqueue(formattedLog);

            // Write to console for debugging
            Console.WriteLine(formattedLog);

            // Write to file to save logs
            AppendLogToFile(formattedLog);
        }

        /// <summary>
        /// Appends a log entry to the log file.
        /// </summary>
        /// <param name="logEntry">The log entry to append.</param>
        private void AppendLogToFile(string logEntry)
        {
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all logs currently in memory.
        /// </summary>
        /// <returns>An array of log entries.</returns>
        public string[] GetLogs()
        {
            return _logs.ToArray();
        }

        /// <summary>
        /// Clear all logs currently in memory.
        /// </summary>
        public void ClearLogs()
        {
            _logs.Clear();
        }

        #endregion
    }
}
