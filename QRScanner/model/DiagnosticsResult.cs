using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRScanner.utility;

namespace QRScanner.model
{
    public class DiagnosticsResult
    {
        #region Attributes and instances

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Scanner> DetectedScanners { get; set; }
        public Scanner SelectedScanner { get; set; }
        public CommandResult CommandResult { get; set; }

        #endregion

        public DiagnosticsResult() 
        {
            Success = true;
            Message = string.Empty;
            DetectedScanners = new List<Scanner>();
        }

        public DiagnosticsResult(bool success, string message, List<Scanner> detectedScanners, Scanner selectedScanner)
        {
            Success = success;
            Message = message;
            DetectedScanners = detectedScanners;
            SelectedScanner = selectedScanner;
        }

        public DiagnosticsResult(bool success, string message, List<Scanner> detectedScanners, Scanner selectedScanner, CommandResult commandResult)
        {
            Success = success;
            Message = message;
            DetectedScanners = detectedScanners;
            SelectedScanner = selectedScanner;
            CommandResult = commandResult;
        }

        #region Methods

        public string GetDiagnosticsResultDetails()
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine("Diagnostics result details:");
            details.AppendLine($"- Success: {Success}");
            details.AppendLine($"- Message: {Message}");

            // Detected scanners details
            if (DetectedScanners != null && DetectedScanners.Any())
            {
                details.AppendLine("- Detected scanners:");
                details.AppendLine(new string('-', 50)); // Separator between scanners

                for (int i = 0; i < DetectedScanners.Count; i++)
                {
                    details.AppendLine($"Scanner {i + 1}:");
                    details.AppendLine(DetectedScanners[i].GetScannerDetails());
                    details.AppendLine(new string('-', 50)); // Separator between scanners
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

            // Command Result details
            if (!Success)
            {
                details.AppendLine("Last command result:");
                details.Append(CommandResult.GetCommandResultDetails());
            }

            return details.ToString();
        }

        #endregion
    }
}
