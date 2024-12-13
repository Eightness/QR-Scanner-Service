using System;
using QRScanner.controller;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.utility;

namespace QRScanner.service
{
    public sealed class QRScannerService
    {
        #region Attributes and instances

        private static readonly Lazy<QRScannerService> _instance = new(() => new QRScannerService());
        private Task? _scannerTask;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly QRScannerLogger _qrScannerLogger = QRScannerLogger.Instance;
        public ScannerController ScannerController = new();
        private bool requiredDiagnosis = true;
        public bool IsDiagnosisRequired => requiredDiagnosis;   // To access requiredDiagnosis from outside


        public event EventHandler<BarcodeScannedEventArgs> QRCodeDecoded;

        public static QRScannerService Instance => _instance.Value;

        #endregion

        private QRScannerService() { }

        #region Main methods

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
                _qrScannerLogger.LogInfo("Scanning...");
                _scannerTask = ScanLoopAsync(1000, token);    // Start the scanning task (1000 ms between iterations)

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
                // Stops current ScanLoop via token
                _cancellationTokenSource.Cancel(); 
                
                if (_scannerTask != null)
                    await _scannerTask; // Waits for the ScanLoop to finish its current iteration

                _cancellationTokenSource = null;

                _qrScannerLogger.LogInfo("QR scanner service stopped.");

                // Disable scan
                CommandResult disableScanResult = ScannerController.EnableScan(false);
                _qrScannerLogger.LogInfo(disableScanResult.StatusMessage);
                _qrScannerLogger.LogInfo(disableScanResult.OutXml);

                // Beep to indicate that the service stopped
                CommandResult beepScannerResult = ScannerController.BeepScanner("6");
                _qrScannerLogger.LogInfo(disableScanResult.StatusMessage);
                _qrScannerLogger.LogInfo(disableScanResult.OutXml);

                // Close CoreScanner API
                CommandResult closeCoreScannerAPIResult = ScannerController.CloseCoreScannerAPI();
                _qrScannerLogger.LogInfo(closeCoreScannerAPIResult.StatusMessage);
                _qrScannerLogger.LogInfo(disableScanResult.OutXml);


                _qrScannerLogger.LogInfo("CoreScanner API closed.");

                return true;
            }
            catch (FailedToCloseCoreScannerAPIException e)
            {
                _qrScannerLogger.LogError(e.Message);
                return false;
            }
            catch (OperationCanceledException e)
            {
                _qrScannerLogger.LogInfo($"Scanning stopped via cancellation: {e.Message}");
                return true;
            }
            catch (Exception e)
            {
                _qrScannerLogger.LogError($"Error while stopping scanning: {e.Message}");
                return false;
            }
            finally
            {
                requiredDiagnosis = true;   // After stopping, a new diagnosis is required
                UnsubscribeToBarcodeScannedEvent(); // Unsubscribe to BarcodeScanned event
            }
        }

        private async Task ScanLoopAsync(int delayMilliseconds, CancellationToken token)
        {
            SubscribeToBarcodeScannedEvent();
            ScannerController.BeepScanner("1");
            ScannerController.EnableScan(true);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(delayMilliseconds, token); // Breathing time to try and scan a QR code, with a token to cancel
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

        public async Task<DiagnosticsResult> RunDiagnostics(int maxAttempts, int delayMilliseconds)
        {
            DiagnosticsResult result;

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
                    result = new DiagnosticsResult(false, $"Unable to detect scanners after {maxAttempts} attempts.", ScannerController.DetectedScanners, ScannerController.SelectedScanner);
                    _qrScannerLogger.LogError($"Diagnostics unsuccessful: {result.ErrorMessage}");
                    return result;
                }

                // Step 3: Register for events
                ScannerController.RegisterForAllEvents(true);
                _qrScannerLogger.LogInfo("Successfully registered for scanner events.");

                // Step 4: Claim scanner
                ScannerController.ClaimScanner(true);

                // Step 5: Disable scan (and LED) manually
                ScannerController.EnableScan(false);

                // Diagnostics completed
                result = new DiagnosticsResult(true, "Diagnostics completed successfully.", ScannerController.DetectedScanners, ScannerController.SelectedScanner);
                ScannerController.BeepScanner("20");    // Fast warble beep
                _qrScannerLogger.LogInfo("Diagnostics completed successfully.");

                requiredDiagnosis = false;  // As diagnosis is completed, service can start

                return result;
            }
            catch (Exception e)
            {
                // Diagnostics failed
                _qrScannerLogger.LogError($"Diagnostics unsuccessful: {e.Message}");
                requiredDiagnosis = true;

                return result;
            }
        }

        private async Task<bool> TryDetectScannersAsync(int maxAttempts, int delayMilliseconds)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    _qrScannerLogger.LogInfo($"Attempt {attempt}/{maxAttempts}: Trying to detect scanners...");

                    CommandResult result = ScannerController.DetectScanners();
                    _qrScannerLogger.LogInfo($"Detected {ScannerController.DetectedScanners.Count} scanner(s).");
                    _qrScannerLogger.LogInfo($"Selected scanner with ID {ScannerController.SelectedScanner.ScannerID}: {ScannerController.SelectedScanner.GetScannerDetails()}");

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

        private void HandleBarcodeScanned(object sender, BarcodeScannedEventArgs e)
        {
            QRCodeDecoded?.Invoke(this, e);
            //await StopScanningAsync();   // Automatically stop the service after scanning a single code
        }

        #endregion
    }
}
