using System;
using System.Collections.Generic;
using System.Xml;
using QRScanner.Exceptions;
using QRScanner.model;

namespace QRScanner.utility
{
    /// <summary>
    /// Serves as a XML file worker class.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the logic associated to decode xml files. Finding scanner nodes
    /// to convert data to a Scanner object.
    /// </remarks>
    public class XMLReader
    {
        #region Methods

        /// <summary>
        /// Converts the first scanner in the XML to a Scanner object.
        /// </summary>
        public Scanner GetFirstScannerDetectedFromXml(string outXml)
        {
            ValidateXmlInput(outXml);
            XmlNode scannerNode = GetScannerNode(outXml, "/scanners/scanner");

            return ParseScannerNode(scannerNode);
        }

        /// <summary>
        /// Retrieves a specific scanner by its ID from the XML.
        /// </summary>
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

        /// <summary>
        /// Retrieves all scanners from the XML as a list.
        /// </summary>
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

        /// <summary>
        /// Parses an XmlNode into a Scanner object.
        /// </summary>
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

        /// <summary>
        /// Validates the XML input string.
        /// </summary>
        private static void ValidateXmlInput(string outXml)
        {
            if (string.IsNullOrWhiteSpace(outXml))
            {
                throw new ArgumentException("XML content is null or empty.", nameof(outXml));
            }
        }

        /// <summary>
        /// Loads and returns an XmlDocument from the given XML string.
        /// </summary>
        private static XmlDocument LoadXmlDocument(string outXml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(outXml);
            return xmlDoc;
        }

        /// <summary>
        /// Retrieves a single XmlNode based on the XPath query.
        /// </summary>
        private static XmlNode GetScannerNode(string outXml, string xPath)
        {
            XmlDocument xmlDoc = LoadXmlDocument(outXml);
            return xmlDoc.SelectSingleNode(xPath);
        }

        /// <summary>
        /// Safely parses an XmlNode's child node to an integer.
        /// </summary>
        private static int ParseNodeToInt(XmlNode parentNode, string nodeName)
        {
            return int.TryParse(parentNode.SelectSingleNode(nodeName)?.InnerText?.Trim(), out int result)
                ? result
                : 0;
        }

        /// <summary>
        /// Safely parses an XmlNode's child node to a string.
        /// </summary>
        private static string ParseNodeToString(XmlNode parentNode, string nodeName)
        {
            return parentNode.SelectSingleNode(nodeName)?.InnerText?.Trim() ?? "Unknown";
        }

        #endregion
    }
}
