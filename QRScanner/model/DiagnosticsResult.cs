using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRScanner.utility;

namespace QRScanner.model
{
    class DiagnosticsResult
    {
        #region Attributes and instances

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<Scanner> DetectedScanners { get; set; }
        public Scanner SelectedScanner { get; set; }

        #endregion

        public DiagnosticsResult(bool success, string errorMessage, List<Scanner> detectedScanners, Scanner selectedScanner)
        {
            Success = success;
            ErrorMessage = errorMessage;
            DetectedScanners = detectedScanners;
            SelectedScanner = selectedScanner;
        }

        #region Methods

        public string GetDiagnosticsResultDetails()
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine("");
            details.AppendLine($"- Success: {Success}");
            details.AppendLine($"- ErrorMessage: {ErrorMessage}");
            details.AppendLine($"- Detected scanners: {() => { Scanner.GetScannersDetails(DetectedScanners); }}");
            details.AppendLine($"- Selected scanner: {SelectedScanner.GetScannerDetails()}");

            return details.ToString();
        }

        #endregion
    }
}
