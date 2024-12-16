using System;
using System.Text;
using QRScanner.utility;

namespace QRScanner.model
{
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

        /// <summary>
        /// Returns true or false depending if status is 0 or not
        /// </summary>
        public bool IsSuccessful()
        {
            return Status == 0;
        }

        /// <summary>
        /// Returns a detailed, human-readable string representing all the information in the CommandResult object.
        /// </summary>
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
