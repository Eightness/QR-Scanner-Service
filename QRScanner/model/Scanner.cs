using System;
using System.Text;

namespace QRScanner.model
{
    /// <summary>
    /// Represents a scanner device with detailed properties and methods for retrieving its information.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Scanner"/> class encapsulates all relevant details about a scanner device, 
    /// including its type, unique identifier (ID), serial number, vendor/product IDs, and other attributes such as 
    /// firmware version and model number.
    /// </para>
    /// <para>
    /// This class provides a method, <see cref="GetScannerDetails"/>, which generates a human-readable, structured
    /// representation of the scanner's properties. This method is particularly useful for logging, debugging, or displaying
    /// scanner information in a user interface.
    /// </para>
    /// <para>
    /// Instances of this class are immutable after construction, ensuring the integrity of scanner details throughout their lifecycle.
    /// </para>
    /// </remarks>
    public class Scanner
    {
        #region Attributes and instances

        // Private properties
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

        #region Constructors

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

        #endregion

        #region Public properties

        // Properties for accessing scanner details (read-only)
        public string ScannerType => scannerType;
        public int ScannerID => scannerID;
        public string SerialNumber => serialNumber;
        public string GUID => guid;
        public string VID => vid;
        public string PID => pid;
        public string ModelNumber => modelNumber;
        public string DOM => dom;
        public string Firmware => firmware;

        #endregion

        #region Methods

        public string GetScannerDetails()
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
            details.AppendLine("");

            return details.ToString();
        }

        #endregion
    }
}
