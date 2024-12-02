using System;
using System.Threading;
using System.Threading.Tasks;
using QRScanner.controller;
using QRScanner.Exceptions;
using QRScanner.utility;
using QRScanner.view;

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
        private readonly Logger _logger = Logger.Instance;
        public ScannerController ScannerController = new ScannerController();

        /// <summary>
        /// Event triggered when a QR code is scanned.
        /// </summary>
        public event EventHandler<string>? QRCodeScanned;

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
                _logger.LogWarning("Scanning already in progress.");
                return false;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                // Open CoreScanner API, detect scanners and select the first one detected
                _logger.LogInfo("Starting QR scanner service...");

                ScannerController.OpenCoreScannerAPI();
                _logger.LogInfo("CoreScanner API opened.");

                ScannerController.DetectScanners();
                _logger.LogInfo($"Detected {ScannerController.DetectedScanners.Count} Scanners.");
                _logger.LogInfo($"Selected scanner {ScannerController.SelectedScanner.ScannerID}.");

                // Start the scanning task
                _logger.LogInfo("Scanning...");
                _scannerTask = Task.Run(() => ScanLoop(token), token);

                return true;
            }
            catch (FailedToOpenCoreScannerAPIException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (ScannersDetectionFailedException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (NoScannersFoundException ex)
            {
                _logger.LogError(ex.Message);

                try
                {
                    _logger.LogInfo("Stopping QR scanner service...");

                    ScannerController.CloseCoreScannerAPI();
                    _logger.LogInfo("CoreScanner API closed.");

                    _logger.LogInfo("QR scanner service stopped.");
                    _logger.LogInfo("Try again.");
                }
                catch (FailedToCloseCoreScannerAPIException subEx)
                {
                    _logger.LogError($"{subEx.Message}");
                    _logger.LogError("Please restart.");
                }

                return false;
            }
            catch (ScannerNotSelectedException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Stops the QR scanning process gracefully.
        /// </summary>
        public bool StopScanning()
        {
            if (_cancellationTokenSource == null)
            {
                _logger.LogWarning("No active scanning to stop.");
                return false;
            }

            try
            {
                _cancellationTokenSource.Cancel();
                _scannerTask?.Wait();
                _cancellationTokenSource = null;

                _logger.LogInfo("QR scanner service stopped.");

                ScannerController.CloseCoreScannerAPI();
                _logger.LogInfo("CoreScanner API closed.");

                return true;
            }
            catch (FailedToCloseCoreScannerAPIException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (AggregateException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Main loop for QR scanning. Simulates QR code detection and triggers the QRCodeScanned event.
        /// </summary>
        /// <param name="token">Cancellation token to stop the loop gracefully.</param>
        private void ScanLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Delay
                    Task.Delay(1000).Wait(); // Simulates the time taken to scan a QR code
                    Console.WriteLine("QR...");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInfo("Scanning stopped.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in QR scanning loop: {ex.Message}");
                }
            }
        }

        #endregion
    }
}
