using System;
using QRScanner.model;
using QRScanner.utility;

namespace QRScanner.Exceptions
{
    /// <summary>
    /// Serves as the base class for all custom exceptions in the QRScanner namespace.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="QRScannerException"/> class provides a foundation for exceptions that occur in the QRScanner system. 
    /// It extends the standard <see cref="System.Exception"/> class and includes additional context by associating a <see cref="CommandResult"/> object.
    /// </para>
    /// <para>
    /// The <see cref="CommandResult"/> property provides detailed information about the command that triggered the exception, such as its execution status, 
    /// any XML output, and relevant messages. This allows for better debugging and diagnostic capabilities when handling scanner-related errors.
    /// </para>
    /// <para>
    /// Derived exceptions should call one of the provided constructors to initialize the message and associate the relevant <see cref="CommandResult"/> object.
    /// Additionally, an optional inner exception can be passed to maintain the exception chain.
    /// </para>
    /// </remarks>
    public abstract class QRScannerException : Exception
    {
        public CommandResult CommandResult { get; }

        protected QRScannerException(string message)
            : base(message) { }

        protected QRScannerException(string message, CommandResult commandResult)
            : base(message)
        {
            CommandResult = commandResult;
        }

        protected QRScannerException(string message, CommandResult commandResult, Exception innerException)
            : base(message, innerException)
        {
            CommandResult = commandResult;
        }
    }

    #region Scanner related Exceptions

    public class NoScannersFoundException : QRScannerException
    {
        public NoScannersFoundException()
            : base("No scanners found.") { }

        public NoScannersFoundException(CommandResult commandResult)
            : base("No scanners found.", commandResult) { }

        public NoScannersFoundException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public NoScannersFoundException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    public class ScannerNotFoundException : QRScannerException
    {
        public ScannerNotFoundException(int scannerId)
            : base($"Scanner with ID {scannerId} was not found within detected scanners.") { }

        public ScannerNotFoundException(int scannerId, CommandResult commandResult)
            : base($"Scanner with ID {scannerId} was not found within detected scanners.", commandResult) { }

        public ScannerNotFoundException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public ScannerNotFoundException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    public class SelectedScannerIsNullException : QRScannerException
    {
        public SelectedScannerIsNullException()
            : base("No scanner is currently selected. Please select a scanner before performing this operation.") { }

        public SelectedScannerIsNullException(CommandResult commandResult)
            : base("No scanner is currently selected. Please select a scanner before performing this operation.", commandResult) { }

        public SelectedScannerIsNullException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public SelectedScannerIsNullException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    public class ScannerAlreadySelectedException : QRScannerException
    {
        public ScannerAlreadySelectedException(int scannerId)
            : base($"Scanner with ID {scannerId} is already selected.") { }

        public ScannerAlreadySelectedException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public ScannerAlreadySelectedException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    public class ScannerNotSelectedException : QRScannerException
    {
        public ScannerNotSelectedException(CommandResult commandResult)
            : base("Failed to select any scanner. Please ensure a valid scanner is connected and selected.", commandResult) { }

        public ScannerNotSelectedException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public ScannerNotSelectedException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    #endregion

    #region SDK related Exceptions

    public class FailedToOpenCoreScannerAPIException : QRScannerException
    {
        public FailedToOpenCoreScannerAPIException(string details, CommandResult commandResult)
            : base($"Failed to initialize the SDK. Details -> {details}", commandResult) { }

        public FailedToOpenCoreScannerAPIException(string message, string details, CommandResult commandResult)
            : base($"{message}. Details -> {details}", commandResult) { }

        public FailedToOpenCoreScannerAPIException(string details, CommandResult commandResult, Exception innerException)
            : base($"Failed to initialize the SDK. Details -> {details}", commandResult, innerException) { }
    }

    public class FailedToCloseCoreScannerAPIException : QRScannerException
    {
        public FailedToCloseCoreScannerAPIException(string details)
            : base($"Failed to stop the SDK. Details -> {details}") { }

        public FailedToCloseCoreScannerAPIException(string details, CommandResult commandResult)
            : base($"Failed to stop the SDK. Details -> {details}", commandResult) { }

        public FailedToCloseCoreScannerAPIException(string message, string details, CommandResult commandResult)
            : base($"{message}. Details -> {details}", commandResult) { }

        public FailedToCloseCoreScannerAPIException(string details, CommandResult commandResult, Exception innerException)
            : base($"Failed to stop the SDK. Details -> {details}", commandResult, innerException) { }
    }

    public class ScannersDetectionFailedException : QRScannerException
    {
        public ScannersDetectionFailedException(string details, CommandResult commandResult)
            : base($"Scanners detection failed. Details -> {details}", commandResult) { }

        public ScannersDetectionFailedException(string message, string details, CommandResult commandResult)
            : base($"{message}. Details -> {details}", commandResult) { }

        public ScannersDetectionFailedException(string details, CommandResult commandResult, Exception innerException)
            : base($"Scanners detection failed. Details -> {details}", commandResult, innerException) { }
    }

    public class CommandExecutionFailedException : QRScannerException
    {
        public CommandExecutionFailedException(int opcode, string inXml, string details, CommandResult commandResult)
            : base($"Command {opcode} with XML '{inXml}' failed. Details -> {details}", commandResult) { }

        public CommandExecutionFailedException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public CommandExecutionFailedException(int opcode, string inXml, string details, CommandResult commandResult, Exception innerException)
            : base($"Command {opcode} with XML '{inXml}' failed. Details: {details}", commandResult, innerException) { }
    }

    #endregion

    #region Decoding related Exceptions

    public class DataLabelNotFoundException : QRScannerException
    {
        public DataLabelNotFoundException()
            : base("DataLabel was not found in the provided XML.") { }

        public DataLabelNotFoundException(CommandResult commandResult)
            : base("DataLabel was not found in the provided XML.", commandResult) { }

        public DataLabelNotFoundException(string message, CommandResult commandResult)
            : base(message, commandResult) { }

        public DataLabelNotFoundException(string message, CommandResult commandResult, Exception innerException)
            : base(message, commandResult, innerException) { }
    }

    #endregion
}
