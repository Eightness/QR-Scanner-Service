using System;
using System.Threading;
using System.Threading.Tasks;
using QRScanner.controller;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.utility;
using QRScanner.view;
using Windows.Security.Authentication.OnlineId;

namespace QRScanner.service
{
    /// <summary>
    /// Service for managing QR code scanning in a background task using a singleton pattern.
    /// </summary>
    public sealed class QRScannerService
    {
        #region Attributes and instances

        private static readonly Lazy<QRScannerService> _instance = new(() => new QRScannerService());
        private Task? _scannerTask;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly QRScannerLogger _qrScannerLogger = QRScannerLogger.Instance;
        public ScannerController ScannerController = new ScannerController();

        /// <summary>
        /// Event triggered when a QR code is scanned.
        /// </summary>
        public event EventHandler<BarcodeScannedEventArgs> QRCodeDecoded;

        /// <summary>
        /// Singleton instance of the QRScannerService.
        /// </summary>
        public static QRScannerService Instance => _instance.Value;

        #endregion

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private QRScannerService() { }

        #region Methods

        /// <summary>
        /// Starts the QR scanning process in a background task.
        /// </summary>
        public bool StartScanning()
        {
            if (_scannerTask != null && !_scannerTask.IsCompleted)
            {
                _qrScannerLogger.LogWarning("Scanning already in progress.");
                return false;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                // Open CoreScanner API, detect scanners and select the first one detected
                _qrScannerLogger.LogInfo("Starting QR scanner service...");

                ScannerController.OpenCoreScannerAPI();
                _qrScannerLogger.LogInfo("CoreScanner API opened.");

                ScannerController.DetectScanners();
                _qrScannerLogger.LogInfo($"Detected {ScannerController.DetectedScanners.Count} Scanners.");
                _qrScannerLogger.LogInfo($"Selected scanner {ScannerController.SelectedScanner.ScannerID}.");

                ScannerController.RegisterForAllEvents();
                _qrScannerLogger.LogInfo("Registered for all events.");

                ScannerController.EnableScan(true);

                // Start the scanning task
                _qrScannerLogger.LogInfo("Scanning...");
                _scannerTask = ScanLoopAsync(token);

                return true;
            }
            catch (FailedToOpenCoreScannerAPIException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
                return false;
            }
            catch (ScannersDetectionFailedException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
                return false;
            }
            catch (NoScannersFoundException ex)
            {
                _qrScannerLogger.LogError(ex.Message);

                try
                {
                    _qrScannerLogger.LogInfo("Stopping QR scanner service...");

                    ScannerController.CloseCoreScannerAPI();
                    _qrScannerLogger.LogInfo("CoreScanner API closed.");

                    _qrScannerLogger.LogInfo("QR scanner service stopped.");
                    _qrScannerLogger.LogInfo("Try again.");
                }
                catch (FailedToCloseCoreScannerAPIException subEx)
                {
                    _qrScannerLogger.LogError($"{subEx.Message}");
                    _qrScannerLogger.LogError("Please restart.");
                }

                return false;
            }
            catch (ScannerNotSelectedException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Stops the QR scanning process gracefully.
        /// </summary>
        public async Task<bool> StopScanningAsync()
        {
            if (_cancellationTokenSource == null)
            {
                _qrScannerLogger.LogWarning("No active scanning to stop.");
                return false;
            }

            try
            {
                _cancellationTokenSource.Cancel();
                
                if (_scannerTask != null)
                    await _scannerTask;

                _cancellationTokenSource = null;

                _qrScannerLogger.LogInfo("QR scanner service stopped.");

                ScannerController.EnableScan(false);
                ScannerController.BeepScanner(false);

                ScannerController.CloseCoreScannerAPI();
                _qrScannerLogger.LogInfo("CoreScanner API closed.");

                return true;
            }
            catch (FailedToCloseCoreScannerAPIException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
                return false;
            }
            catch (OperationCanceledException ex)
            {
                _qrScannerLogger.LogInfo("Scanning stopped via cancellation.");
                return true;
            }
            catch (Exception ex)
            {
                _qrScannerLogger.LogError($"Error while stopping scanning: {ex.Message}");
                return false;
            }
            finally
            {
                UnsubscribeToBarcodeScannedEvent();
            }
        }

        /// <summary>
        /// Main loop for QR scanning. Simulates QR code detection and triggers the QRCodeScanned event.
        /// </summary>
        /// <param name="token">Cancellation token to stop the loop gracefully.</param>
        private async Task ScanLoopAsync(CancellationToken token)
        {
            SubscribeToBarcodeScannedEvent();
            ScannerController.BeepScanner(true);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(1000, token); // Breathing time to try and scan a QR code, with a token to cancel
                    Console.WriteLine("...");   // Simulates the scanning process to provide a visual representation or feedback in the console
                }
                catch (OperationCanceledException)
                {
                    _qrScannerLogger.LogInfo("Scanning stopped.");
                }
                catch (Exception ex)
                {
                    _qrScannerLogger.LogError($"Error in QR scanning loop: {ex.Message}");
                }
            }
        }

        #endregion

        #region Events Handling

        private void SubscribeToBarcodeScannedEvent()
        {
            // Subscribes to BarcodeScanned event from Scanner Controller
            ScannerController.BarcodeScanned += HandleBarcodeScanned;
        }

        private void UnsubscribeToBarcodeScannedEvent()
        {
            // Unsubscribes to BarcodeScanned event from Scanner Controller
            ScannerController.BarcodeScanned -= HandleBarcodeScanned;
        }

        private void HandleBarcodeScanned(object sender, BarcodeScannedEventArgs e)
        {
            // Whenever a BarcodeScanned event is triggered, a new event is triggered with processed/decoded arguments
            QRCodeDecoded.Invoke(this, e);
            //StopScanning();   // Uncomment this to automatically stop the service after scanning a single code
        }

        #endregion
    }
}
