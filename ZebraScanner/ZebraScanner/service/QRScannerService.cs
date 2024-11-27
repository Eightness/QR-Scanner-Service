using System;
using System.Threading;
using System.Threading.Tasks;
using ZebraScanner.controller;
using ZebraScanner.utility;

namespace ZebraScanner.service
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
        private ScannerController scannerController = new ScannerController();

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
        public void StartScanning()
        {
            if (_scannerTask != null && !_scannerTask.IsCompleted)
            {
                _logger.LogWarning("Scanning already in progress.");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            _logger.LogInfo("Starting QR scanner...");

            // Start the scanning task
            _scannerTask = Task.Run(() => ScanLoop(token), token);
        }

        /// <summary>
        /// Stops the QR scanning process gracefully.
        /// </summary>
        public void StopScanning()
        {
            if (_cancellationTokenSource == null)
            {
                _logger.LogWarning("No active scanning to stop.");
                return;
            }

            _logger.LogInfo("Stopping QR scanner...");
            _cancellationTokenSource.Cancel();
            _scannerTask?.Wait();
            _cancellationTokenSource = null;
            _logger.LogInfo("QR scanner stopped.");
        }

        /// <summary>
        /// Main loop for QR scanning. Simulates QR code detection and triggers the QRCodeScanned event.
        /// </summary>
        /// <param name="token">Cancellation token to stop the loop gracefully.</param>
        private void ScanLoop(CancellationToken token)
        {
            _logger.LogInfo("QR scanning loop started.");
            while (!token.IsCancellationRequested)
            {
                try
                {
                    /*
                     * Most important piece of code here.
                     */
                    scannerController.OpenCoreScannerAPI();
                    scannerController.DetectScanners();
                    scannerController.RegisterForAllEvents();

                    // Simulate QR code detection with a delay
                    Thread.Sleep(1000); // Simulates the time taken to scan a QR code
                    string scannedCode = "QR_CODE"; // Replace with actual QR code detection logic

                    _logger.LogInfo($"QR code detected: {scannedCode}");
                    QRCodeScanned?.Invoke(this, scannedCode);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInfo("QR scanning loop canceled.");
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
