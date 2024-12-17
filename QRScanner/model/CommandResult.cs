using System;
using System.Text;
using QRScanner.utility;

namespace QRScanner.model
{
    /// <summary>
    /// Represents the result of a command executed by the CoreScanner SDK.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="CommandResult"/> class encapsulates details about a command's execution, including its name, status code, status message, and any associated XML output.
    /// This provides a standardized way to interpret and log results from SDK operations in the QRScanner system.
    /// </para>
    /// <para>
    /// The class includes helper methods to determine whether the command executed successfully (via <see cref="IsSuccessful"/>), 
    /// and to parse scanner-related information from the XML output, such as retrieving all detected scanners or a specific scanner by its ID.
    /// </para>
    /// <para>
    /// Use this class when handling responses from the CoreScanner SDK to provide detailed insights into command outcomes, particularly for debugging and diagnostics purposes.
    /// </para>
    /// </remarks>
    public class CommandResult
    {
        #region Attributes and instances

        public string CommandName { get; private set; }
        public int Status { get; private set; }
        public string OutXml { get; private set; }
        public string StatusMessage { get; private set; }
        public int NumberOfScanners = -1;
        private XMLReader xmlReader = XMLReader.Instance;

        #endregion

        #region Constructors

        public CommandResult()
        {
            CommandName = string.Empty;
            Status = -1;
            StatusMessage = string.Empty;
            OutXml = string.Empty;
        }

        public CommandResult(string commandName, int status)
        {
            CommandName = commandName;
            Status = status;
            StatusMessage = StatusHandler.HandleStatus(status);
        }

        public CommandResult(string commandName, int status, string outXml)
        {
            CommandName = commandName;
            Status = status;
            StatusMessage = StatusHandler.HandleStatus(status);
            OutXml = outXml;
        }

        public CommandResult(string commandName, int status, string outXml, int numberOfScanners)
        {
            CommandName = commandName;
            Status = status;
            StatusMessage = StatusHandler.HandleStatus(status);
            OutXml = outXml;
            NumberOfScanners = numberOfScanners;
        }

        #endregion

        #region Methods

        public bool IsSuccessful()
        {
            return Status == 0;
        }

        public string GetCommandResultDetails()
        {
            var details = new StringBuilder();

            details.AppendLine("Command result details:");
            details.AppendLine($"- Command: {CommandName}");
            details.AppendLine($"- {StatusMessage}");
            if (NumberOfScanners != -1)
                details.AppendLine($"- Number of scanners detected: {NumberOfScanners}");
            details.AppendLine($"- Output XML: {(string.IsNullOrWhiteSpace(OutXml) ? "No XML output." : OutXml)}");

            return details.ToString();
        }

        #endregion

        #region XMLReader Methods

        public Scanner GetFirstScannerDetected()
        {
            return xmlReader.GetFirstScannerDetectedFromXml(OutXml);
        }

        public Scanner GetScannerById(int scannerId)
        {
            return xmlReader.GetScannerByIdFromXml(OutXml, scannerId);
        }

        public List<Scanner> GetAllScanners()
        {
            return xmlReader.GetAllScannersFromXml(OutXml);
        }

        #endregion
    }
}
