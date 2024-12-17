using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRScanner.utility;

namespace QRScanner.model
{
    /// <summary>
    /// Represents the result of a diagnostics operation performed on the QR scanner system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="DiagnosticsResult"/> class encapsulates the outcome of a diagnostics operation,
    /// providing details such as whether the diagnostics were successful, a descriptive message,
    /// the list of detected scanners, the selected scanner, and any relevant command result information.
    /// </para>
    /// <para>
    /// This class is used to aggregate all relevant information about the system's health and state during diagnostics.
    /// It is particularly useful for logging, debugging, and presenting diagnostic summaries.
    /// </para>
    /// <para>
    /// The class provides a method, <see cref="GetDiagnosticsResultDetails"/>, to return a structured and human-readable
    /// report of the diagnostics outcome, including details about detected scanners, the selected scanner,
    /// and the result of the last executed command.
    /// </para>
    /// </remarks>
    public class DiagnosticsResult
    {
        #region Attributes and instances

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Scanner> DetectedScanners { get; set; }
        public Scanner SelectedScanner { get; set; }
        public List<CommandResult> CommandResults = new List<CommandResult>();

        #endregion

        #region Constructors

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

        public DiagnosticsResult(bool success, string message, List<Scanner> detectedScanners, Scanner selectedScanner, List<CommandResult> commandResults)
        {
            Success = success;
            Message = message;
            DetectedScanners = detectedScanners;
            SelectedScanner = selectedScanner;
            CommandResults = commandResults;
        }

        #endregion

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
                details.AppendLine("- Detected scanners: None");
            

            // Selected scanner details
            if (SelectedScanner != null)
            {
                details.AppendLine($"- Selected scanner:");
                details.AppendLine(SelectedScanner.GetScannerDetails());
            }
            else
                details.AppendLine("- Selected scanner: Null.");

            // Command Result details
            details.AppendLine("Executed commands:");
            foreach (var commandResult in CommandResults)
            {
                details.AppendLine(commandResult.GetCommandResultDetails());
            }
            
            return details.ToString();
        }

        #endregion
    }
}
