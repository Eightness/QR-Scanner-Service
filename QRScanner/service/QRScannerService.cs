using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using QRScanner.controller;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.utility;
using QRScanner.view;
using Windows.Security.Authentication.OnlineId;
using Windows.UI.Composition.Interactions;

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
        private ScannerMonitor scannerMonitor;
        public ScannerController ScannerController = new();
        private bool requiredDiagnosis = true;

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

        #region Main methods

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

            if (requiredDiagnosis)
            {
                _qrScannerLogger.LogWarning("A successful diagnosis is required to start the service.");
                return false;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                // Start the scanning task
                _qrScannerLogger.LogInfo("Scanning...");
                _scannerTask = ScanLoopAsync(token);

                return true;
            }
            catch (Exception e)
            {
                _qrScannerLogger.LogError($"Failed to start service: {e.Message}");
                return false;
            }
        }

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
                ScannerController.BeepScanner("6");

                ScannerController.CloseCoreScannerAPI();
                _qrScannerLogger.LogInfo("CoreScanner API closed.");

                return true;
            }
            catch (FailedToCloseCoreScannerAPIException e)
            {
                _qrScannerLogger.LogError(e.Message);
                return false;
            }
            catch (OperationCanceledException)
            {
                _qrScannerLogger.LogInfo("Scanning stopped via cancellation.");
                return true;
            }
            catch (Exception e)
            {
                _qrScannerLogger.LogError($"Error while stopping scanning: {e.Message}");
                return false;
            }
            finally
            {
                requiredDiagnosis = true;
                UnsubscribeToBarcodeScannedEvent();
                UnsubscribeToScannerDisconnectedEvent();
            }
        }

        private async Task ScanLoopAsync(CancellationToken token)
        {
            SubscribeToBarcodeScannedEvent();
            ScannerController.BeepScanner("1");
            ScannerController.EnableScan(true);

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
                catch (Exception e)
                {
                    _qrScannerLogger.LogError($"Error in QR scanning loop: {e.Message}");
                }
            }
        }

        #endregion

        #region Diagnostics

        public async Task<bool> RunDiagnostics(int maxAttempts, int delayMilliseconds)
        {
            try
            {
                _qrScannerLogger.LogInfo("Running diagnostics for QR scanner service...");

                // Step 1: Open CoreScanner API
                ScannerController.OpenCoreScannerAPI();
                _qrScannerLogger.LogInfo("CoreScanner API opened successfully.");

                // Step 2: Detect scanners with retries
                bool scannersDetected = await TryDetectScannersAsync(maxAttempts, delayMilliseconds);
                if (!scannersDetected)
                {
                    // Diagnostics failed
                    _qrScannerLogger.LogError($"Diagnostics unsuccessful: Unable to detect scanners after {maxAttempts} attempts.");
                    return false;
                }

                // Step 3: Start monitoring scanner disconnection
                scannerMonitor = new(ScannerController.SelectedScanner.VID, ScannerController.SelectedScanner.PID);
                SubscribeToScannerDisconnectedEvent();

                // Step 4: Register for events
                ScannerController.RegisterForAllEvents();
                _qrScannerLogger.LogInfo("Successfully registered for scanner events.");

                // Step 5: Disable scan (and LED) manually
                ScannerController.EnableScan(false);

                // Diagnostics completed
                ScannerController.BeepScanner("20");    // Fast warble beep
                _qrScannerLogger.LogInfo("Diagnostics completed successfully.");

                requiredDiagnosis = false;
                return true;
            }
            catch (Exception e)
            {
                // Diagnostics failed
                _qrScannerLogger.LogError($"Diagnostics unsuccessful: {e.Message}");
                requiredDiagnosis = true;

                return false;
            }
        }

        private async Task<bool> TryDetectScannersAsync(int maxAttempts, int delayMilliseconds)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    _qrScannerLogger.LogInfo($"Attempt {attempt}/{maxAttempts}: Trying to detect scanners...");

                    ScannerController.DetectScanners();
                    _qrScannerLogger.LogInfo($"Detected {ScannerController.DetectedScanners.Count} scanner(s).");
                    _qrScannerLogger.LogInfo($"Selected scanner with ID {ScannerController.SelectedScanner.ScannerID}: {ScannerController.SelectedScanner.GetDetails()}");

                    return true; // Success
                }
                catch (Exception ex)
                {
                    _qrScannerLogger.LogError($"Attempt {attempt} failed: {ex.Message}");

                    if (attempt < maxAttempts)
                    {
                        _qrScannerLogger.LogWarning($"Retrying in {delayMilliseconds} ms...");
                        await Task.Delay(delayMilliseconds);
                    }
                }
            }

            return false; // Failed after all attempts
        }

        #endregion

        #region Events Handling

        private void SubscribeToBarcodeScannedEvent()
        {
            ScannerController.BarcodeScanned += HandleBarcodeScanned;
        }

        private void UnsubscribeToBarcodeScannedEvent()
        {
            ScannerController.BarcodeScanned -= HandleBarcodeScanned;
        }

        private async void HandleBarcodeScanned(object sender, BarcodeScannedEventArgs e)
        {
            QRCodeDecoded.Invoke(this, e);
            //await StopScanningAsync();   // Automatically stop the service after scanning a single code
        }

        private void SubscribeToScannerDisconnectedEvent()
        {
            scannerMonitor.ScannerDisconnected += OnScannerDisconnected;
        }

        private void UnsubscribeToScannerDisconnectedEvent()
        {
            scannerMonitor.ScannerDisconnected -= OnScannerDisconnected;
        }

        private async void OnScannerDisconnected(object sender, EventArgs e)
        {
            _qrScannerLogger.LogWarning("The currently selected scanner has been disconnected.");
            requiredDiagnosis = true;
            await StopScanningAsync();
        }

        #endregion
    }
}
