using System;
using System.Collections.Generic;
using System.Xml;
using QRScanner.Exceptions;
using QRScanner.model;

namespace QRScanner.utility
{
    /// <summary>
    /// Provides utility methods for parsing XML data related to scanner devices.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="XMLReader"/> class is a singleton that processes XML data, typically received from the CoreScanner API.
    /// It extracts scanner details such as IDs, serial numbers, model numbers, and other relevant properties.
    /// </para>
    /// <para>
    /// This class includes methods for retrieving a single scanner, a scanner by ID, or all scanners present in the XML data.
    /// </para>
    /// </remarks>
    public class XMLReader
    {
        #region Attributes and Instances

        // Lazy initialization of the singleton instance.
        private static readonly Lazy<XMLReader> _instance = new Lazy<XMLReader>(() => new XMLReader());
        public static XMLReader Instance => _instance.Value;

        #endregion

        #region Constructors

        private XMLReader() { }

        #endregion

        #region Methods

        public Scanner GetFirstScannerDetectedFromXml(string outXml)
        {
            ValidateXmlInput(outXml);
            XmlNode scannerNode = GetScannerNode(outXml, "/scanners/scanner");

            return ParseScannerNode(scannerNode);
        }

        public Scanner GetScannerByIdFromXml(string outXml, int scannerId)
        {
            ValidateXmlInput(outXml);
            XmlNode scannerNode = GetScannerNode(outXml, $"/scanners/scanner[scannerID='{scannerId}']");

            if (scannerNode == null)
            {
                throw new ScannerNotFoundException(scannerId);
            }

            return ParseScannerNode(scannerNode);
        }

        public List<Scanner> GetAllScannersFromXml(string outXml)
        {
            ValidateXmlInput(outXml);

            XmlDocument xmlDoc = LoadXmlDocument(outXml);
            XmlNodeList scannerNodes = xmlDoc.SelectNodes("/scanners/scanner");

            if (scannerNodes == null || scannerNodes.Count == 0)
            {
                throw new NoScannersFoundException();
            }

            var scanners = new List<Scanner>();
            foreach (XmlNode scannerNode in scannerNodes)
            {
                scanners.Add(ParseScannerNode(scannerNode));
            }

            return scanners;
        }

        #endregion

        #region Utility methods

        private Scanner ParseScannerNode(XmlNode scannerNode)
        {
            if (scannerNode == null)
            {
                throw new ArgumentNullException(nameof(scannerNode), "Scanner node cannot be null.");
            }

            return new Scanner(
                scannerType: scannerNode.Attributes["type"]?.Value?.Trim() ?? "Unknown",
                scannerID: ParseNodeToInt(scannerNode, "scannerID"),
                serialNumber: ParseNodeToString(scannerNode, "serialnumber"),
                guid: ParseNodeToString(scannerNode, "GUID"),
                vid: ParseNodeToString(scannerNode, "VID"),
                pid: ParseNodeToString(scannerNode, "PID"),
                modelNumber: ParseNodeToString(scannerNode, "modelnumber"),
                dom: ParseNodeToString(scannerNode, "DoM"),
                firmware: ParseNodeToString(scannerNode, "firmware")
            );
        }

        private static void ValidateXmlInput(string outXml)
        {
            if (string.IsNullOrWhiteSpace(outXml))
            {
                throw new ArgumentException("XML content is null or empty.", nameof(outXml));
            }
        }

        private static XmlDocument LoadXmlDocument(string outXml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(outXml);
            return xmlDoc;
        }

        private static XmlNode GetScannerNode(string outXml, string xPath)
        {
            XmlDocument xmlDoc = LoadXmlDocument(outXml);
            return xmlDoc.SelectSingleNode(xPath);
        }

        private static int ParseNodeToInt(XmlNode parentNode, string nodeName)
        {
            return int.TryParse(parentNode.SelectSingleNode(nodeName)?.InnerText?.Trim(), out int result)
                ? result
                : 0;
        }

        private static string ParseNodeToString(XmlNode parentNode, string nodeName)
        {
            return parentNode.SelectSingleNode(nodeName)?.InnerText?.Trim() ?? "Unknown";
        }

        #endregion
    }
}
