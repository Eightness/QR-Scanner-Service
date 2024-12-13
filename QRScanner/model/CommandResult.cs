using System;
using System.Text;
using QRScanner.utility;

namespace QRScanner.model
{
    /// <summary>
    /// Represents the result of a command executed on a Zebra scanner.
    /// Includes the command's status, output XML, a human-readable status message, 
    /// and the number of scanners involved (if applicable).
    /// </summary>
    public class CommandResult
    {
        #region Attributes and instances

        /// <summary>
        /// A human-readable message describing the command.
        /// </summary>
        public string CommandName { get; private set; }

        /// <summary>
        /// The status code returned by the command.
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// The output XML returned by the command.
        /// </summary>
        public string OutXml { get; private set; }

        /// <summary>
        /// A human-readable message describing the status.
        /// </summary>
        public string StatusMessage { get; private set; }

        /// <summary>
        /// The number of scanners associated with the command (if applicable).
        /// </summary>
        public int NumberOfScanners { get; private set; }

        /// <summary>
        /// XMLReader instance to process xml files.
        /// </summary>
        private XMLReader xmlReader = XMLReader.Instance;

        #endregion

        /// <summary>
        /// Default constructor initializing default values for the attributes.
        /// </summary>
        public CommandResult()
            : this(string.Empty, -1, string.Empty, 0) { }

        /// <summary>
        /// Default constructor initializing default values for the attributes.
        /// </summary>
        public CommandResult(string commandName)
            : this(commandName, -1, string.Empty, 0) { }


        /// <summary>
        /// Constructor for a result with only a status code.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        public CommandResult(string commandName, int status)
            : this(commandName, status, string.Empty, 0) { }

        /// <summary>
        /// Constructor for a result with a status code and output XML.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        /// <param name="outXml">The output XML returned by the command.</param>
        public CommandResult(string commandName, int status, string outXml)
            : this(commandName, status, outXml, 0) { }

        /// <summary>
        /// Constructor for a result with a status code, output XML, and number of scanners.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        /// <param name="outXml">The output XML returned by the command.</param>
        /// <param name="numberOfScanners">The number of scanners associated with the command.</param>
        public CommandResult(string commandName, int status, string outXml, int numberOfScanners)
        {
            CommandName = commandName;
            Status = status;
            OutXml = outXml;
            NumberOfScanners = numberOfScanners;
            StatusMessage = StatusHandler.HandleStatus(status);
        }

        #region Methods

        /// <summary>
        /// Returns a detailed, human-readable string representing all the information in the CommandResult object.
        /// </summary>
        public string GetCommandResultDetails()
        {
            var details = new StringBuilder();

            details.AppendLine("Command Result Details:");
            details.AppendLine($"- Command Name: {CommandName}");
            details.AppendLine($"- Status: {Status}");
            details.AppendLine($"- Status Message: {StatusMessage}");
            details.AppendLine($"- Number of Scanners: {NumberOfScanners}");
            details.AppendLine($"- Output XML: {(string.IsNullOrWhiteSpace(OutXml) ? "No XML output" : OutXml)}");

            if (!string.IsNullOrWhiteSpace(OutXml))
            {
                details.AppendLine("- Scanners Information:");

                try
                {
                    var scanners = GetAllScanners();
                    if (scanners != null && scanners.Count > 0)
                    {
                        foreach (var scanner in scanners)
                        {
                            details.AppendLine(scanner.GetScannerDetails());
                        }
                    }
                    else
                    {
                        details.AppendLine("  No scanners found in the output XML.");
                    }
                }
                catch (Exception ex)
                {
                    details.AppendLine($"Error parsing scanners from XML: {ex.Message}");
                }
            }

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
