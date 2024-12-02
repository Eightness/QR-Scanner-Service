using System;

namespace QRScanner.Exceptions
{
    /// <summary>
    /// Base class for all custom exceptions in the QRScanner namespace.
    /// </summary>
    public abstract class QRScannerException : Exception
    {
        protected QRScannerException(string message) : base(message) { }

        protected QRScannerException(string message, Exception innerException) : base(message, innerException) { }
    }

    #region Scanner related Exceptions

    /// <summary>
    /// Exception thrown when no scanners are found in the provided XML.
    /// </summary>
    public class NoScannersFoundException : QRScannerException
    {
        public NoScannersFoundException()
            : base("No scanners found in the provided XML.") { }

        public NoScannersFoundException(string message)
            : base(message) { }

        public NoScannersFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a specific scanner is not found by its ID.
    /// </summary>
    public class ScannerNotFoundException : QRScannerException
    {
        public ScannerNotFoundException(int scannerId)
            : base($"Scanner with ID {scannerId} was not found within detected scanners.") { }

        public ScannerNotFoundException(string message)
            : base(message) { }

        public ScannerNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when attempting an operation without a selected scanner.
    /// </summary>
    public class SelectedScannerIsNullException : QRScannerException
    {
        public SelectedScannerIsNullException()
            : base("No scanner is currently selected. Please select a scanner before performing this operation.") { }

        public SelectedScannerIsNullException(string message)
            : base(message) { }

        public SelectedScannerIsNullException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when attempting an operation without a selected scanner.
    /// </summary>
    public class ScannerAlreadySelectedException : QRScannerException
    {
        public ScannerAlreadySelectedException(int scannerId)
            : base($"Scanner with ID {scannerId} already selected.") { }

        public ScannerAlreadySelectedException(string message)
            : base(message) { }

        public ScannerAlreadySelectedException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when attempting an operation without a selected scanner.
    /// </summary>
    public class ScannerNotSelectedException : QRScannerException
    {
        public ScannerNotSelectedException()
            : base("Failed to select any scanner. Please ensure a valid scanner is connected and selected.") { }

        public ScannerNotSelectedException(string message)
            : base(message) { }

        public ScannerNotSelectedException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    #endregion

    #region SDK related Exceptions

    /// <summary>
    /// Exception thrown when the SDK fails to initialize.
    /// </summary>
    public class FailedToOpenCoreScannerAPIException : QRScannerException
    {
        public FailedToOpenCoreScannerAPIException(string details)
            : base($"Failed to initialize the SDK. Details -> {details}") { }

        public FailedToOpenCoreScannerAPIException(string message, string details)
            : base($"{message}. Details -> {details}") { }

        public FailedToOpenCoreScannerAPIException(string details, Exception innerException)
            : base($"Failed to initialize the SDK. Details -> {details}", innerException) { }
    }

    /// <summary>
    /// Exception thrown when the SDK fails to stop.
    /// </summary>
    public class FailedToCloseCoreScannerAPIException : QRScannerException
    {
        public FailedToCloseCoreScannerAPIException(string details)
            : base($"Failed to stop the SDK. Details -> {details}") { }

        public FailedToCloseCoreScannerAPIException(string message, string details)
            : base($"{message}. Details -> {details}") { }

        public FailedToCloseCoreScannerAPIException(string details, Exception innerException)
            : base($"Failed to stop the SDK. Details -> {details}", innerException) { }
    }

    /// <summary>
    /// Exception thrown when the SDK fails to stop.
    /// </summary>
    public class ScannersDetectionFailedException : QRScannerException
    {
        public ScannersDetectionFailedException(string details)
            : base($"Scanners detection failed. Details -> {details}") { }

        public ScannersDetectionFailedException(string message, string details)
            : base($"{message}. Details -> {details}") { }

        public ScannersDetectionFailedException(string details, Exception innerException)
            : base($"Scanners detection failed. Details -> {details}", innerException) { }
    }

    /// <summary>
    /// Exception thrown when a Scanner operation fails during command execution.
    /// </summary>
    public class CommandExecutionFailedException : QRScannerException
    {
        public CommandExecutionFailedException(int opcode, string args, string details)
            : base($"Command {opcode} with args '{args}' failed. Details -> {details}") { }

        public CommandExecutionFailedException(string message)
            : base(message) { }

        public CommandExecutionFailedException(int opcode, string args, string details, Exception innerException)
            : base($"Command {opcode} with args '{args}' failed. Details: {details}", innerException) { }
    }

    /// <summary>
    /// Exception thrown when communication issues are detected with the scanner.
    /// </summary>
    public class ScannerCommunicationFailedException : QRScannerException
    {
        public ScannerCommunicationFailedException()
            : base("Communication problems were detected with the scanner.") { }

        public ScannerCommunicationFailedException(string message)
            : base(message) { }

        public ScannerCommunicationFailedException(Exception innerException)
            : base("Communication problems were detected with the scanner.", innerException) { }
    }

    #endregion
}
