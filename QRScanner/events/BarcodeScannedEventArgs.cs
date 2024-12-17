using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRScanner.Exceptions;

namespace QRScanner.events
{
    /// <summary>
    /// Provides data for the barcode scanned event, including the raw data, decoded label, and additional details.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="BarcodeScannedEventArgs"/> class encapsulates the data associated with a barcode scan event. 
    /// It includes the type of barcode, the raw hexadecimal data received, and the decoded label for easy interpretation.
    /// </para>
    /// <para>
    /// The class decodes hexadecimal data strings (e.g., "0x30 0x31 0x32") into readable ASCII text using the <see cref="DecodeDataLabel"/> method. 
    /// The decoded string is stored in the <see cref="DecodedDataLabel"/> property and is available for external use.
    /// </para>
    /// </remarks>
    public class BarcodeScannedEventArgs : EventArgs
    {
        #region Attributes and instances

        public int DataType { get; }
        public string DataLabel { get; }
        public string RawXml { get; }
        public string DecodedDataLabel { get; set; }

        #endregion

        #region Constructors

        public BarcodeScannedEventArgs(int dataType, string dataLabel, string rawXml)
        {
            DataType = dataType;
            DataLabel = dataLabel;
            RawXml = rawXml;
            DecodeDataLabel(dataLabel);
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
