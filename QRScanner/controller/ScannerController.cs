using System;
using System.Xml.Linq;
using CoreScanner;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.utility;
using Windows.Security.Authentication.OnlineId;

namespace QRScanner.controller
{
    /// <summary>
    /// Handles interactions with the Zebra Scanner API, including initialization, command execution, and event registration.
    /// Provides methods to manage scanners and process barcode data.
    /// </summary>
    public class ScannerController
    {
        #region Attributes and instances

        private readonly CCoreScanner _coreScanner; // CoreScanner SDK instance
        public List<Scanner> DetectedScanners;
        public Scanner SelectedScanner;

        /// <summary>
        /// Event triggered when a barcode is scanned.
        /// </summary>
        public event EventHandler<BarcodeScannedEventArgs> BarcodeScanned;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerController"/> class.
        /// </summary>
        public ScannerController()
        {
            _coreScanner = new CCoreScanner();
            DetectedScanners = new List<Scanner>();
        }

        #region Primary methods

        /// <summary>
        /// Initializes the Zebra Scanner SDK by establishing a connection with the CoreScanner API.
        /// This method enables communication with Zebra scanners by opening the CoreScanner SDK.
        /// </summary>
        /// <remarks>
        /// This method is required before performing any operations with the scanner, such as detecting devices or executing commands.
        /// If the operation fails, an exception will be thrown with details about the failure.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the operation and any relevant XML output.
        /// </returns>
        /// <exception cref="FailedToOpenCoreScannerAPIException">
        /// Thrown if the CoreScanner API fails to initialize. The exception includes the status message explaining the error.
        /// </exception>
        public CommandResult OpenCoreScannerAPI()
        {
            short[] scannerTypes = { 1 }; // 1 for all scanner types
            short numberOfScannerTypes = 1;
            _coreScanner.Open(0, scannerTypes, numberOfScannerTypes, out int status);

            CommandResult result = new CommandResult(status);

            if (result.Status != 0)
                throw new FailedToOpenCoreScannerAPIException(result.StatusMessage);

            SubscribeToBarcodeEvent();

            return result;
        }

        /// <summary>
        /// Terminates the Zebra Scanner SDK by closing the connection with the CoreScanner API.
        /// This method cleans up resources and ensures that the connection to the SDK is properly closed.
        /// </summary>
        /// <remarks>
        /// It is recommended to call this method when the application no longer needs to interact with the scanner
        /// or before shutting down the application to release the SDK's resources.
        /// If the operation fails, an exception will be thrown with details about the failure.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the operation.
        /// </returns>
        /// <exception cref="FailedToCloseCoreScannerAPIException">
        /// Thrown if the CoreScanner API fails to close the connection. The exception includes the status message explaining the error.
        /// </exception>
        public CommandResult CloseCoreScannerAPI()
        {
            _coreScanner.Close(0, out int status);

            CommandResult result = new CommandResult(status);

            if (result.Status != 0)
                throw new FailedToCloseCoreScannerAPIException(result.StatusMessage);

            UnsubscribeToBarcodeEvent();

            return result;
        }

        /// <summary>
        /// Detects all scanners currently connected to the host and retrieves their details.
        /// This method queries the CoreScanner API to list available scanners and stores their information.
        /// Additionally, it selects the first detected scanner by default.
        /// </summary>
        /// <remarks>
        /// This method should be called after initializing the CoreScanner API to ensure that the application
        /// has an up-to-date list of connected scanners. If no scanners are found or the detection fails, appropriate
        /// exceptions are thrown to handle the error scenarios.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the operation, the XML output with scanner details,
        /// and the total number of scanners detected.
        /// </returns>
        /// <exception cref="ScannersDetectionFailedException">
        /// Thrown when the scanner detection operation fails in the CoreScanner API.
        /// </exception>
        /// <exception cref="NoScannersFoundException">
        /// Thrown when no scanners are detected in the current environment.
        /// </exception>
        /// <exception cref="ScannerNotSelectedException">
        /// Thrown when a scanner is detected but the system fails to select the first available scanner.
        /// </exception>
        public CommandResult DetectScanners()
        {
            short numberOfScanners;
            int[] connectedScannerIDList = new int[255];
            _coreScanner.GetScanners(out numberOfScanners, connectedScannerIDList, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml, numberOfScanners);

            if (result.Status != 0)
                throw new ScannersDetectionFailedException(result.StatusMessage);
            
            // Get all scanners detected
            DetectedScanners = result.GetAllScanners();

            if (DetectedScanners.Count <= 0)
                throw new NoScannersFoundException();

            // Select first scanner detected by default
            SelectedScanner = result.GetFirstScannerDetected();

            if (SelectedScanner == null)
                throw new ScannerNotSelectedException();

            return result;
        }

        /// <summary>
        /// Registers the application to listen for all scanner events, including barcode scanning.
        /// </summary>
        /// <remarks>
        /// This method configures the scanner to send all events, such as barcode scan data, 
        /// to the application. It uses the Zebra CoreScanner API to register for the specified events. 
        /// Events like barcode scanning and trigger actions are supported by default.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the registration process and any relevant XML output.
        /// </returns>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to register for events fails. The exception provides details 
        /// about the opcode, event parameters, and the error message.
        /// </exception>
        public CommandResult RegisterForAllEvents()
        {
            // All events by default
            string inXml = "<inArgs>" +
                           "<cmdArgs>" +
                           "<arg-int>6</arg-int>" +             // Number of events to register
                           "<arg-int>1,2,4,8,16,32</arg-int>" + // Event IDs
                           "</cmdArgs>" +
                           "</inArgs>";

            _coreScanner.ExecCommand(OpcodesHandler.REGISTER_FOR_EVENTS, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.REGISTER_FOR_EVENTS, "6 and 1,2,4,8,16,32", result.StatusMessage);

            return result;
        }

        /// <summary>
        /// Unregisters the application from all scanner events.
        /// </summary>
        /// <remarks>
        /// This method removes the application's subscription to all scanner events, including barcode scanning
        /// and trigger actions. After this method is executed, the application will no longer receive any event
        /// notifications from the scanner.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the unregistration process and any relevant XML output.
        /// </returns>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to unregister from events fails. The exception provides details about the opcode,
        /// the parameters used for the command, and the associated error message.
        /// </exception>
        public CommandResult UnregisterForAllEvents()
        {
            string inXml = "<inArgs>" +
                           "<cmdArgs>" +
                           "<arg-int>1</arg-int>" + // Unregister all events
                           "</cmdArgs>" +
                           "</inArgs>";

            _coreScanner.ExecCommand(OpcodesHandler.UNREGISTER_FOR_EVENTS, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.UNREGISTER_FOR_EVENTS, "6", result.StatusMessage);

            return result;
        }

        /// <summary>
        /// Claims the currently selected scanner for exclusive use by this application.
        /// </summary>
        /// <remarks>
        /// Claiming a scanner is required to perform operations that require exclusive access,
        /// such as scanning or configuring the device. Once a scanner is claimed, other applications 
        /// will not be able to interact with it until it is released.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the claim operation and any relevant XML output.
        /// </returns>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when no scanner is currently selected. Ensure a scanner is selected before calling this method.
        /// </exception>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to claim the scanner fails. The exception contains details about the failed command,
        /// including the opcode, scanner ID, and the error message.
        /// </exception>
        public CommandResult ClaimScanner()
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID></inArgs>";
            _coreScanner.ExecCommand(OpcodesHandler.CLAIM_DEVICE, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.CLAIM_DEVICE, $"{SelectedScanner.ScannerID}", result.StatusMessage);

            return result;
        }

        /// <summary>
        /// Releases the exclusive claim on the currently selected scanner, allowing other applications to use it.
        /// </summary>
        /// <remarks>
        /// This method sends a command to the Zebra Scanner API to release the claim on the selected scanner.
        /// Releasing a scanner is necessary when the application no longer needs exclusive access to the scanner,
        /// or before closing the application.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the operation and any relevant XML output.
        /// </returns>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when no scanner is currently selected. Ensure a scanner is selected before calling this method.
        /// </exception>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to release the scanner fails. The exception contains details about the failed command,
        /// including the opcode, scanner ID, and the error message.
        /// </exception>
        public CommandResult ReleaseScanner()
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID></inArgs>";
            _coreScanner.ExecCommand(OpcodesHandler.RELEASE_DEVICE, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.RELEASE_DEVICE, $"{SelectedScanner.ScannerID}", result.StatusMessage);

            return result;
        }

        public CommandResult EnableScan(bool enable)
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID></inArgs>";
            string outXml;
            int status;

            if (enable)
                _coreScanner.ExecCommand(OpcodesHandler.SCAN_ENABLE, ref inXml, out outXml, out status);
            else
                _coreScanner.ExecCommand(OpcodesHandler.SCAN_DISABLE, ref inXml, out outXml, out status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.RELEASE_DEVICE, $"{SelectedScanner.ScannerID}", result.StatusMessage);

            return result;
        }

        #endregion

        #region Operational methods

        /// <summary>
        /// Selects a scanner from the list of detected scanners by its unique ID.
        /// </summary>
        /// <remarks>
        /// This method searches the list of previously detected scanners and assigns the scanner
        /// with the specified ID as the currently selected scanner. If no matching scanner is found,
        /// an exception is thrown to indicate the error.
        /// </remarks>
        /// <param name="scannerId">The unique ID of the scanner to be selected.</param>
        /// <exception cref="ScannerNotFoundException">
        /// Thrown when no scanner with the specified ID is found in the list of detected scanners.
        /// </exception>
        public void SelectScannerById(int scannerId)
        {
            if (SelectedScanner.ScannerID == scannerId)
                throw new ScannerAlreadySelectedException(scannerId);

            bool found = false;

            foreach (Scanner scanner in DetectedScanners)
            {
                if (scanner.ScannerID == scannerId)
                {
                    found = true;
                    SelectedScanner = scanner;
                }
            }

            if (!found)
                throw new ScannerNotFoundException(scannerId);
        }

        /// <summary>
        /// Restarts the currently selected scanner.
        /// </summary>
        /// <remarks>
        /// This method sends a command to reboot the selected scanner. Restarting a scanner is useful for resetting 
        /// its state, applying certain configurations, or resolving potential operational issues.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the restart operation and any relevant XML output.
        /// </returns>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when no scanner is currently selected. Ensure a scanner is selected before calling this method.
        /// </exception>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to restart the scanner fails. The exception includes details about the failed command, 
        /// such as the opcode, scanner ID, and the error message.
        /// </exception>
        public CommandResult RestartScanner()
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID>";
            _coreScanner.ExecCommand(OpcodesHandler.REBOOT_SCANNER, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.REBOOT_SCANNER, $"{SelectedScanner.ScannerID}", result.StatusMessage);

            return result;
        }

        /// <summary>
        /// Beeps the scanner.
        /// </summary>
        /// <remarks>
        /// Serves as a test to see if the scanner is actually connected and selected.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the restart operation and any relevant XML output.
        /// </returns>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when no scanner is currently selected. Ensure a scanner is selected before calling this method.
        /// </exception>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the API command to restart the scanner fails. The exception includes details about the failed command, 
        /// such as the opcode, scanner ID, and the error message.
        /// </exception>
        public CommandResult BeepScanner(bool scanning)
        {
            ValidateScanner(SelectedScanner);

            string actionValue;

            if (scanning)
                actionValue = "0";
            else
                actionValue = "5";

            string inXml = "<inArgs>" +
                                $"<scannerID>{SelectedScanner.ScannerID}</scannerID>" + // The scanner you need to beep
                                "<cmdArgs>" +
                                    $"<arg-int>{actionValue}</arg-int>" + // Beep type, depends on if starts / stops scanning
                                "</cmdArgs>" +
                            "</inArgs>";

            _coreScanner.ExecCommand(OpcodesHandler.SET_ACTION, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(OpcodesHandler.REBOOT_SCANNER, $"{SelectedScanner.ScannerID}", result.StatusMessage);

            return result;
        }

        /// <summary>
        /// Executes a specific command on the currently selected scanner.
        /// </summary>
        /// <remarks>
        /// This method sends a command to the selected scanner using the specified operation code (opcode)
        /// and command arguments. The response from the scanner, including the status and any output XML, 
        /// is encapsulated in a <see cref="CommandResult"/> object.
        /// </remarks>
        /// <param name="opcode">
        /// The operation code representing the command to be executed. Refer to <see cref="OpcodesHandler"/>
        /// for a list of valid opcodes.
        /// </param>
        /// <param name="args">
        /// The arguments required by the command, formatted as a string in XML.
        /// </param>
        /// <returns>
        /// A <see cref="CommandResult"/> object containing the status of the command execution and any relevant XML output.
        /// </returns>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when no scanner is selected. Ensure a scanner is selected before attempting to execute a command.
        /// </exception>
        /// <exception cref="CommandExecutionFailedException">
        /// Thrown when the command execution fails. This exception provides details about the opcode,
        /// the arguments used, and the associated error message.
        /// </exception>
        public CommandResult CommandScanner(int opcode, string args)
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID><cmdArgs><arg-int>{args}</arg-int></cmdArgs></inArgs>";
            _coreScanner.ExecCommand(opcode, ref inXml, out string outXml, out int status);

            CommandResult result = new CommandResult(status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(opcode, args, result.StatusMessage);

            return result;
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Ensures that the provided scanner object is not null.
        /// </summary>
        /// <remarks>
        /// This validation is used to prevent operations from being executed on a null scanner object.
        /// It is commonly called before performing any actions that require a valid, selected scanner.
        /// </remarks>
        /// <param name="scanner">
        /// The <see cref="Scanner"/> object to validate. This represents the currently selected scanner.
        /// </param>
        /// <exception cref="SelectedScannerIsNullException">
        /// Thrown when the provided <paramref name="scanner"/> is null. 
        /// Ensure a scanner is selected before invoking this method.
        /// </exception>
        private static void ValidateScanner(Scanner scanner)
        {
            if (scanner == null)
                throw new SelectedScannerIsNullException();
        }

        #endregion

        #region Events Handling

        /// <summary>
        /// Processes barcode events raised by the scanner and extracts relevant barcode data.
        /// </summary>
        /// <remarks>
        /// This method is triggered when a barcode is scanned. It parses the XML data received from the scanner,
        /// extracts the barcode type and label, and raises the <see cref="BarcodeScanned"/> event with the parsed details.
        /// </remarks>
        /// <param name="eventType">
        /// The type of the event raised by the scanner. This parameter is typically used to differentiate between 
        /// various event types, although it is not directly utilized in this implementation.
        /// </param>
        /// <param name="barcodeData">
        /// A reference to the XML string containing the barcode data provided by the scanner.
        /// This XML data is parsed to extract the barcode type and label.
        /// </param>
        /// <exception cref="Exception">
        /// Logs any exceptions encountered during the processing of barcode data to the console.
        /// The method ensures that the application continues running despite errors.
        /// </exception>
        /// <example>
        /// Example barcode event data processed by this method:
        /// <code>
        /// <barcodeEvent>
        ///   <datatype>1</datatype>
        ///   <datalabel>1234567890</datalabel>
        /// </barcodeEvent>
        /// </code>
        /// </example>
        private void BarcodeEventHandler(short eventType, ref string barcodeData)
        {
            try
            {
                var document = XDocument.Parse(barcodeData);
                int dataType = int.Parse(document.Descendants("datatype").First().Value);
                string dataLabel = document.Descendants("datalabel").First().Value;

                var myArgs = new BarcodeScannedEventArgs(dataType, dataLabel, barcodeData);

                BarcodeScanned.Invoke(this, myArgs);
            }
            catch (DataLabelNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing barcode data: {e.Message}");
            }
        }

        private void SubscribeToBarcodeEvent()
        {
            _coreScanner.BarcodeEvent += BarcodeEventHandler;
        }

        private void UnsubscribeToBarcodeEvent()
        {
            _coreScanner.BarcodeEvent -= BarcodeEventHandler;
        }

        #endregion
    }
}
