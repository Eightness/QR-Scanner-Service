using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRScanner.Exceptions;

namespace QRScanner.events
{
    /// <summary>
    /// Provides detailed information about a barcode-scanned event.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the data associated with a barcode scanning event, 
    /// including the type of the barcode, its label, and the raw XML data received from the scanner.
    /// </remarks>
    public class BarcodeScannedEventArgs : EventArgs
    {
        #region Attributes and instances

        /// <summary>
        /// Gets the type of the barcode data.
        /// </summary>
        /// <remarks>
        /// The data type is represented as an integer and typically corresponds to a predefined
        /// set of barcode types in the scanner's API.
        /// </remarks>
        public int DataType { get; }

        /// <summary>
        /// Gets the label or content of the scanned barcode.
        /// </summary>
        /// <remarks>
        /// This property contains the actual data encoded in the barcode, such as a number or string.
        /// </remarks>
        public string DataLabel { get; }

        /// <summary>
        /// Gets the raw XML string containing the barcode data as received from the scanner.
        /// </summary>
        /// <remarks>
        /// This property is useful for debugging or for cases where additional details
        /// from the raw data are required.
        /// </remarks>
        public string RawXml { get; }

        /// <summary>
        /// Gets the decoded label or content of the scanned barcode in human-readable format.
        /// </summary>
        /// <remarks>
        /// This property contains the decoded data from the hexadecimal format of <see cref="DataLabel"/>.
        /// </remarks>
        public string DecodedDataLabel { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeScannedEventArgs"/> class.
        /// </summary>
        /// <param name="dataType">The type of the barcode data.</param>
        /// <param name="dataLabel">The label or content of the scanned barcode.</param>
        /// <param name="rawXml">The raw XML string containing the barcode data.</param>
        public BarcodeScannedEventArgs(int dataType, string dataLabel, string rawXml)
        {
            DataType = dataType;
            DataLabel = dataLabel;
            RawXml = rawXml;
            DecodeDataLabel(dataLabel);
        }

        /// <summary>
        /// Decodes a hexadecimal dataLabel string into a readable ASCII string.
        /// </summary>
        /// <param name="dataLabel">The hexadecimal dataLabel string, with each value prefixed by "0x".</param>
        /// <returns>The decoded ASCII string.</returns>
        private void DecodeDataLabel(string dataLabel)
        {
            if (string.IsNullOrWhiteSpace(dataLabel))
                throw new DataLabelNotFoundException();

            // Split the dataLabel into individual hex values (e.g., "0x30", "0x31")
            string[] hexValues = dataLabel.Split(' ');

            // Initialize a StringBuilder to store the decoded string
            StringBuilder decodedBuilder = new StringBuilder();

            // Process each hex value
            foreach (string hex in hexValues)
            {
                // Remove the "0x" prefix and parse the value
                string hexValue = hex.Replace("0x", "");
                int byteValue = Convert.ToInt32(hexValue, 16);

                // Convert the byte to a character and append it to the result
                decodedBuilder.Append((char)byteValue);
            }

            // Sets DecodedDataLabel
            DecodedDataLabel = decodedBuilder.ToString();
        }
    }
}
