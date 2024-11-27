using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScanner.events
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
        }
    }
}
