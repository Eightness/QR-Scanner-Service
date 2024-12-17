using System;
using System.Collections.Concurrent;
using System.IO;

namespace QRScanner.utility
{
    /// <summary>
    /// Provides logging functionality for the QR Scanner application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="QRScannerLogger"/> class implements a singleton pattern to ensure a single instance of the logger is used throughout the application. 
    /// It logs messages at different levels (INFO, WARNING, ERROR) and appends them both to an in-memory queue and a persistent log file.
    /// </para>
    /// <para>
    /// The logger maintains a counter for log entries and ensures all logs include a timestamp, log level, and unique log index.
    /// Logs are saved in the "out" directory under the project root and can be retrieved or cleared programmatically.
    /// </para>
    /// </remarks>
    public sealed class QRScannerLogger
    {
        #region Attributes and instances

        private static readonly Lazy<QRScannerLogger> _instance = new(() => new QRScannerLogger());
        private readonly ConcurrentQueue<string> _logs = new();
        private int _logCounter = 0; // Counter for the number of logs
        private readonly string _logFilePath; // File path for the logs
        private DateTime _lastLogDate;
        public static QRScannerLogger Instance => _instance.Value;

        #endregion

        #region Constructors

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

            // Initialize last log date with  la última fecha registrada con la fecha actual
            _lastLogDate = DateTime.Now.Date;
        }

        #endregion

        #region Methods

        public void LogInfo(string message)
        {
            AddLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            AddLog("WARNING", message);
        }

        public void LogError(string message)
        {
            AddLog("ERROR", message);
        }

        private void AddLog(string level, string message)
        {
            CheckAndResetLogFile();

            int currentLogNumber = ++_logCounter; // Increment and get the current log count
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string formattedLog = $"[{currentLogNumber}] [{timestamp}] [{level}]: {message}";
            _logs.Enqueue(formattedLog);

            // Write to console for debugging
            Console.WriteLine(formattedLog);

            // Write to file to save logs
            AppendLogToFile(formattedLog);
        }

        private void CheckAndResetLogFile()
        {
            DateTime currentDate = DateTime.Now.Date;

            if (_lastLogDate != currentDate)
            {
                try
                {
                    File.WriteAllText(_logFilePath, string.Empty); // Deletes all logs content
                    Console.WriteLine($"Log file reset on {currentDate:dd/MM/yyyy}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to reset log file: {ex.Message}");
                }

                _lastLogDate = currentDate; // Updates last log date
            }
        }

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

        public string[] GetLogs()
        {
            return _logs.ToArray();
        }

        public void ClearLogs()
        {
            _logs.Clear();
        }

        #endregion
    }
}
