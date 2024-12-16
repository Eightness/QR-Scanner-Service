using System;
using System.Xml.Linq;
using CoreScanner;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.utility;

namespace QRScanner.controller
{
    public class ScannerController
    {
        #region Attributes and instances

        private readonly CCoreScanner _coreScanner; // CoreScanner SDK instance
        public List<Scanner> DetectedScanners;
        public Scanner SelectedScanner;
        public bool IsOpen { get; private set; } = false;

        public event EventHandler<BarcodeScannedEventArgs> BarcodeScanned;

        #endregion

        public ScannerController()
        {
            _coreScanner = new CCoreScanner();
            DetectedScanners = new List<Scanner>();
        }

        #region Primary methods

        public CommandResult OpenCoreScannerAPI()
        {
            if (IsOpen)
                return new CommandResult("Open CoreScanner.", 0);    // If the connection is already open, skip the remaining code and return success.

            short[] scannerTypes = { 1 }; // 1 for all scanner types
            short numberOfScannerTypes = 1;
            _coreScanner.Open(0, scannerTypes, numberOfScannerTypes, out int status);

            CommandResult result = new CommandResult("Open CoreScanner.", status);

            if (result.Status != 0)
                throw new FailedToOpenCoreScannerAPIException(result.StatusMessage, result);

            IsOpen = true;
            SubscribeToBarcodeEvent();

            return result;
        }

        public CommandResult CloseCoreScannerAPI()
        {
            if (!IsOpen)
                throw new FailedToCloseCoreScannerAPIException("CoreScanner API is not opened.");

            _coreScanner.Close(0, out int status);

            CommandResult result = new CommandResult("Close CoreScanner.", status);

            if (result.Status != 0)
                throw new FailedToCloseCoreScannerAPIException(result.StatusMessage, result);

            IsOpen = false;
            UnsubscribeToBarcodeEvent();

            return result;
        }

        public CommandResult DetectScannersAndSelection()
        {
            short numberOfScanners;
            int[] connectedScannerIDList = new int[255];
            _coreScanner.GetScanners(out numberOfScanners, connectedScannerIDList, out string outXml, out int status);

            CommandResult result = new CommandResult("Get Scanners.", status, outXml, numberOfScanners);

            if (result.Status != 0)
                throw new ScannersDetectionFailedException(result.StatusMessage, result);
            
            // Get all scanners detected
            DetectedScanners = result.GetAllScanners();

            if (DetectedScanners.Count <= 0)
                throw new NoScannersFoundException(result);

            // Select first scanner detected by default
            if (SelectedScanner == null)
                SelectedScanner = result.GetFirstScannerDetected();

            if (SelectedScanner == null)
                throw new ScannerNotSelectedException(result);

            return result;
        }

        public CommandResult RegisterForAllEvents(bool register)
        {
            // All events by default
            string inXml = "<inArgs>" +
                           "<cmdArgs>" +
                           "<arg-int>6</arg-int>" +             // Number of events to register
                           "<arg-int>1,2,4,8,16,32</arg-int>" + // Event IDs
                           "</cmdArgs>" +
                           "</inArgs>";

            int opcode = register ? OpcodesHandler.REGISTER_FOR_EVENTS : OpcodesHandler.UNREGISTER_FOR_EVENTS;

            var result = ExecuteCommand(opcode, inXml);

            return result;
        }

        #endregion

        #region Operational methods

        public void SelectScannerById(int scannerId)
        {
            if (SelectedScanner.ScannerID == scannerId)
                throw new ScannerAlreadySelectedException(scannerId);

            Scanner newSelectedScanner = DetectedScanners.FirstOrDefault(scanner => scanner.ScannerID == scannerId) ?? throw new ScannerNotFoundException(scannerId);

            SelectedScanner = newSelectedScanner;
        }

        public CommandResult RebootScanner()
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID>";
            int opcode = OpcodesHandler.REBOOT_SCANNER;

            return ExecuteCommand(opcode, inXml);
        }

        public CommandResult ClaimScanner(bool claim)
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID></inArgs>";
            int opcode = claim ? OpcodesHandler.CLAIM_DEVICE : OpcodesHandler.RELEASE_DEVICE;

            return ExecuteCommand(opcode, inXml); ;
        }

        public CommandResult EnableScan(bool enable)
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID></inArgs>";
            int opcode = enable ? OpcodesHandler.SCAN_ENABLE : OpcodesHandler.SCAN_DISABLE;       

            return ExecuteCommand(opcode, inXml);
        }

        public List<CommandResult> DisableScanForAllScanners()
        {
            if (DetectedScanners == null || DetectedScanners.Count == 0)
                throw new NoScannersFoundException();

            var results = new List<CommandResult>();
            int opcode = OpcodesHandler.SCAN_DISABLE;

            foreach (var scanner in DetectedScanners)
            {
                ValidateScanner(scanner);

                string inXml = $"<inArgs><scannerID>{scanner.ScannerID}</scannerID></inArgs>";
                CommandResult result = ExecuteCommand(opcode, inXml);

                results.Add(result);
            }

            return results;
        }

        public CommandResult TurnLED(bool on)
        {
            ValidateScanner(SelectedScanner);

            int ledAction = on ? 43 : 42;  // Green LED on / off, change color if needed
            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID><cmdArgs><arg-int>{ledAction}</arg-int></cmdArgs></inArgs>";
            int opcode = OpcodesHandler.SET_ACTION;

            return ExecuteCommand(opcode, inXml);
        }

        public CommandResult BeepScanner(string actionValue)
        {
            ValidateScanner(SelectedScanner);

            string inXml = $"<inArgs><scannerID>{SelectedScanner.ScannerID}</scannerID><cmdArgs><arg-int>{actionValue}</arg-int></cmdArgs></inArgs>";
            int opcode = OpcodesHandler.SET_ACTION;
            
            return ExecuteCommand(opcode, inXml);
        }

        #endregion

        #region Utility methods

        private static void ValidateScanner(Scanner scanner)
        {
            if (scanner == null)
                throw new SelectedScannerIsNullException();
        }

        private CommandResult ExecuteCommand(int opcode, string inXml)
        {
            _coreScanner.ExecCommand(opcode, ref inXml, out string outXml, out int status);
            CommandResult result = new CommandResult(OpcodesHandler.HandleOpcode(opcode), status, outXml);

            if (result.Status != 0)
                throw new CommandExecutionFailedException(opcode, inXml, result.StatusMessage, result);

            return result;
        }

        #endregion

        #region Events Handling

        private void SubscribeToBarcodeEvent()
        {
            _coreScanner.BarcodeEvent += BarcodeEventHandler;
        }

        private void UnsubscribeToBarcodeEvent()
        {
            _coreScanner.BarcodeEvent -= BarcodeEventHandler;
        }

        private void BarcodeEventHandler(short eventType, ref string barcodeData)
        {
            try
            {
                var document = XDocument.Parse(barcodeData);
                int dataType = int.Parse(document.Descendants("datatype").First().Value);
                string dataLabel = document.Descendants("datalabel").First().Value;

                var myArgs = new BarcodeScannedEventArgs(dataType, dataLabel, barcodeData);

                BarcodeScanned?.Invoke(this, myArgs);
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

        #endregion
    }
}
