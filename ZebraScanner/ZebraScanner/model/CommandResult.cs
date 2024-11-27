using System;
using ZebraScanner.utility;

namespace ZebraScanner.model
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
        private XMLReader xmlReader = new XMLReader();

        #endregion

        /// <summary>
        /// Default constructor initializing default values for the attributes.
        /// </summary>
        public CommandResult()
            : this(-1, string.Empty, 0) { }

        /// <summary>
        /// Constructor for a result with only a status code.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        public CommandResult(int status)
            : this(status, string.Empty, 0) { }

        /// <summary>
        /// Constructor for a result with a status code and output XML.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        /// <param name="outXml">The output XML returned by the command.</param>
        public CommandResult(int status, string outXml)
            : this(status, outXml, 0) { }

        /// <summary>
        /// Constructor for a result with a status code, output XML, and number of scanners.
        /// </summary>
        /// <param name="status">The status code returned by the command.</param>
        /// <param name="outXml">The output XML returned by the command.</param>
        /// <param name="numberOfScanners">The number of scanners associated with the command.</param>
        public CommandResult(int status, string outXml, int numberOfScanners)
        {
            Status = status;
            OutXml = outXml;
            NumberOfScanners = numberOfScanners;
            StatusMessage = StatusHandler.HandleStatus(status);
        }

        #region Methods

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
