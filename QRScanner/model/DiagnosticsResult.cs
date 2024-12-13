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

            details.AppendLine("Diagnostics Result Details:");
            details.AppendLine($"- Success: {Success}");
            details.AppendLine($"- ErrorMessage: {ErrorMessage ?? "None"}");

            // Detected scanners details
            if (DetectedScanners != null && DetectedScanners.Any())
            {
                details.AppendLine("- Detected scanners:");
                for (int i = 0; i < DetectedScanners.Count; i++)
                {
                    details.AppendLine(new string('-', 50)); // Separator between scanners
                    details.AppendLine($"Scanner {i + 1}:");
                    details.AppendLine(DetectedScanners[i].GetScannerDetails());
                }
            }
            else
            {
                details.AppendLine("- Detected scanners: None");
            }

            // Selected scanner details
            if (SelectedScanner != null)
            {
                details.AppendLine($"- Selected scanner:");
                details.AppendLine(SelectedScanner.GetScannerDetails());
            }
            else
            {
                details.AppendLine("- Selected scanner: Null.");
            }

            return details.ToString();
        }

        #endregion
    }
}
