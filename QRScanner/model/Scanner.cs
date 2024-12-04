using System;
using System.Text;

namespace QRScanner.model
{
    /// <summary>
    /// Represents a scanner device with its associated properties and details.
    /// Provides methods to access scanner information and display it in a structured format.
    /// </summary>
    public class Scanner
    {
        #region Attributes and instances

        // Private fields
        private readonly string scannerType;
        private readonly int scannerID;
        private readonly string serialNumber;
        private readonly string guid;
        private readonly string vid;
        private readonly string pid;
        private readonly string modelNumber;
        private readonly string dom;
        private readonly string firmware;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class with specified details.
        /// Defaults to "Unknown" for strings and 0 for numeric values if inputs are invalid or empty.
        /// </summary>
        /// <param name="scannerType">The type of the scanner (e.g., USB, Bluetooth).</param>
        /// <param name="scannerID">The unique ID of the scanner.</param>
        /// <param name="serialNumber">The serial number of the scanner.</param>
        /// <param name="guid">The globally unique identifier (GUID) of the scanner.</param>
        /// <param name="vid">The vendor ID of the scanner.</param>
        /// <param name="pid">The product ID of the scanner.</param>
        /// <param name="modelNumber">The model number of the scanner.</param>
        /// <param name="dom">The date of manufacture (DOM) of the scanner.</param>
        /// <param name="firmware">The firmware version of the scanner.</param>
        public Scanner(
            string scannerType,
            int scannerID,
            string serialNumber,
            string guid,
            string vid,
            string pid,
            string modelNumber,
            string dom,
            string firmware)
        {
            // Validate and initialize fields
            this.scannerType = string.IsNullOrWhiteSpace(scannerType) ? "Unknown" : scannerType;
            this.scannerID = scannerID > 0 ? scannerID : 0; // Default to 0 if invalid
            this.serialNumber = string.IsNullOrWhiteSpace(serialNumber) ? "Unknown" : serialNumber;
            this.guid = string.IsNullOrWhiteSpace(guid) ? "Unknown" : guid;
            this.vid = string.IsNullOrWhiteSpace(vid) ? "Unknown" : vid;
            this.pid = string.IsNullOrWhiteSpace(pid) ? "Unknown" : pid;
            this.modelNumber = string.IsNullOrWhiteSpace(modelNumber) ? "Unknown" : modelNumber;
            this.dom = string.IsNullOrWhiteSpace(dom) ? "Unknown" : dom;
            this.firmware = string.IsNullOrWhiteSpace(firmware) ? "Unknown" : firmware;
        }

        #region Public properties

        // Properties for accessing scanner details (read-only)

        /// <summary>
        /// Gets the type of the scanner.
        /// </summary>
        public string ScannerType => scannerType;

        /// <summary>
        /// Gets the unique ID of the scanner.
        /// </summary>
        public int ScannerID => scannerID;

        /// <summary>
        /// Gets the serial number of the scanner.
        /// </summary>
        public string SerialNumber => serialNumber;

        /// <summary>
        /// Gets the globally unique identifier (GUID) of the scanner.
        /// </summary>
        public string GUID => guid;

        /// <summary>
        /// Gets the vendor ID (VID) of the scanner.
        /// </summary>
        public string VID => vid;

        /// <summary>
        /// Gets the product ID (PID) of the scanner.
        /// </summary>
        public string PID => pid;

        /// <summary>
        /// Gets the model number of the scanner.
        /// </summary>
        public string ModelNumber => modelNumber;

        /// <summary>
        /// Gets the date of manufacture (DOM) of the scanner.
        /// </summary>
        public string DOM => dom;

        /// <summary>
        /// Gets the firmware version of the scanner.
        /// </summary>
        public string Firmware => firmware;

        #endregion

        #region Methods

        /// <summary>
        /// Get the details of the scanner in a structured and readable format.
        /// </summary>
        public string GetDetails()
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine("");
            details.AppendLine($"- Type: {ScannerType}");
            details.AppendLine($"- ID: {ScannerID}");
            details.AppendLine($"- Serial Number: {SerialNumber}");
            details.AppendLine($"- GUID: {GUID}");
            details.AppendLine($"- Vendor ID (VID): {VID}");
            details.AppendLine($"- Product ID (PID): {PID}");
            details.AppendLine($"- Model Number: {ModelNumber}");
            details.AppendLine($"- Date of Manufacture (DOM): {DOM}");
            details.Append($"- Firmware Version: {Firmware}");

            return details.ToString();
        }

        #endregion
    }
}
