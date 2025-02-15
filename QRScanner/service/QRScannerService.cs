﻿using System;
using QRScanner.controller;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.utility;
using Windows.UI.Composition;

namespace QRScanner.service
{
    /// <summary>
    /// Manages the lifecycle, diagnostics, and scanning operations for Zebra scanners.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="QRScannerService"/> class serves as a high-level service to control scanner devices, 
    /// leveraging the <see cref="ScannerController"/> to interact with the CoreScanner SDK. 
    /// It provides methods for starting and stopping the scanning process, running diagnostics, 
    /// and handling events when a barcode or QR code is scanned.
    /// </para>
    /// <para>
    /// The class follows the Singleton design pattern, ensuring a single shared instance of the service. 
    /// It incorporates event-driven programming by exposing the <see cref="QRCodeDecoded"/> event, 
    /// which propagates decoded QR or barcode data to external subscribers.
    /// </para>
    /// <para>
    /// To ensure proper operation, a successful diagnostics check must be performed before starting the scanning process.
    /// Diagnostics verify that the CoreScanner API can be opened, scanners can be detected and selected, 
    /// and essential settings (such as disabling scans and LEDs) can be applied. 
    /// </para>
    /// <para>
    /// The scanning process runs asynchronously in a background task, allowing cancellation and graceful shutdown 
    /// using a cancellation token. The service logs all operations for transparency and debugging purposes.
    /// </para>
    /// </remarks>

    public sealed class QRScannerService
    {
        #region Attributes and instances

        private static readonly Lazy<QRScannerService> _instance = new(() => new QRScannerService());
        private Task? _scannerTask;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly QRScannerLogger _qrScannerLogger = QRScannerLogger.Instance;
        public ScannerController ScannerController = new();
        private bool requiredDiagnosis = true;
        public event EventHandler<BarcodeScannedEventArgs> QRCodeDecoded;
        public static QRScannerService Instance => _instance.Value;

        #endregion

        #region Constructors

        private QRScannerService() { }

        #endregion

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
            
            // Opens CoreScanner API 
            if (!ScannerController.IsOpen)
                ScannerController.OpenCoreScannerAPI();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                _scannerTask = ScanLoopAsync(1000, token);    // Start the scanning task (1000 ms between iterations)

                return true;
            }
            catch (Exception e)
            {
                _qrScannerLogger.LogError($"Failed to start service: {e.Message}");
                return false;
            }
        }

        private async Task ScanLoopAsync(int delayMilliseconds, CancellationToken token)
        {
            CommandResult commandResult;

            SubscribeToBarcodeScannedEvent();   // Subscribes to event

            // Enabling scan (and LED)
            _qrScannerLogger.LogInfo("Enabling scan and LED...");
            commandResult = ScannerController.EnableScan(true);
            _qrScannerLogger.LogInfo(commandResult.GetCommandResultDetails());

            // Beeping the scanner (uncomment to beep)
            /*
            _qrScannerLogger.LogInfo("Turn on beep...");
            commandResult = ScannerController.BeepScanner("1");
            _qrScannerLogger.LogInfo(commandResult.GetCommandResultDetails());
            */

            _qrScannerLogger.LogInfo("Scanning...");

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

        public async Task<bool> StopScanningAsync()
        {
            if (_cancellationTokenSource == null)
            {
                _qrScannerLogger.LogWarning("No active scanning to stop.");
                return false;
            }

            CommandResult commandResult;

            try
            {
                // Stops current ScanLoop via token
                _cancellationTokenSource.Cancel(); 
                
                if (_scannerTask != null)
                    await _scannerTask; // Waits for the ScanLoop to finish its current iteration

                _cancellationTokenSource = null;

                // Disable scan
                _qrScannerLogger.LogInfo("Disabling scan and LED...");
                commandResult = ScannerController.EnableScan(false);
                _qrScannerLogger.LogInfo(commandResult.GetCommandResultDetails());

                // Beep to indicate that the service stopped
                /*
                _qrScannerLogger.LogInfo("Turn off beep...");
                commandResult = ScannerController.BeepScanner("6");
                _qrScannerLogger.LogInfo(commandResult.GetCommandResultDetails());
                */

                // Close CoreScanner API 
                _qrScannerLogger.LogInfo("Closing CoreScanner API...");
                commandResult = ScannerController.CloseCoreScannerAPI();
                _qrScannerLogger.LogInfo(commandResult.GetCommandResultDetails());
                

                _qrScannerLogger.LogInfo("QR scanner service stopped.");

                return true;
            }
            catch (FailedToCloseCoreScannerAPIException e)
            {
                _qrScannerLogger.LogError(e.Message);
                _qrScannerLogger.LogError(e.CommandResult.GetCommandResultDetails());
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
                //requiredDiagnosis = true;   // Uncomment to: After stopping, a new diagnosis is required
                UnsubscribeToBarcodeScannedEvent(); // Unsubscribe to BarcodeScanned event
            }
        }

        #endregion

        #region Diagnostics

        public DiagnosticsResult RunDiagnostics(int maxAttempts, int sleepMilliseconds)
        {
            List<CommandResult> executedCommandsResults = new List<CommandResult>();
            DiagnosticsResult diagnosticsResult;

            try
            {
                _qrScannerLogger.LogInfo("Running diagnostics for QR scanner service...");

                // Step 1: Open CoreScanner API
                _qrScannerLogger.LogInfo("Opening CoreScanner API...");
                var openResult = ScannerController.OpenCoreScannerAPI();
                _qrScannerLogger.LogInfo(openResult.GetCommandResultDetails());
                executedCommandsResults.Add(openResult);

                // Step 2: Scanner detection and selection 
                _qrScannerLogger.LogInfo("Detection and selection process...");
                var detectionResult = TryScannerDetectionProcess(maxAttempts, sleepMilliseconds);
                if (!detectionResult.IsSuccessful())
                {
                    // Diagnostics failed
                    diagnosticsResult = new DiagnosticsResult(false, $"Scanner detection process failed after {maxAttempts} attempts.", ScannerController.DetectedScanners, ScannerController.SelectedScanner);
                    _qrScannerLogger.LogError(diagnosticsResult.GetDiagnosticsResultDetails());
                    return diagnosticsResult;
                }
                _qrScannerLogger.LogInfo(detectionResult.GetCommandResultDetails());
                executedCommandsResults.Add(detectionResult);

                // Step 3: Register for events
                _qrScannerLogger.LogInfo("Registering events...");
                var registerResult = ScannerController.RegisterForAllEvents(true);
                _qrScannerLogger.LogInfo(registerResult.GetCommandResultDetails());
                executedCommandsResults.Add(registerResult);

                // Step 4: Claim scanner
                _qrScannerLogger.LogInfo("Claiming selected scanner...");
                var claimResult = ScannerController.ClaimScanner(true);
                _qrScannerLogger.LogInfo(claimResult.GetCommandResultDetails());
                executedCommandsResults.Add(claimResult);

                // Step 5: Disable scan and LED manually for all scanners
                _qrScannerLogger.LogInfo("Disabling scan and LED...");
                var disableResults = ScannerController.DisableScanForAllScanners();
                foreach (var disableResult in disableResults) 
                { 
                    _qrScannerLogger.LogInfo(disableResult.GetCommandResultDetails());
                    executedCommandsResults.Add(disableResult);
                }

                // Step 6: Beeping the scanner
                /*
                _qrScannerLogger.LogInfo("Diagnosis beep...");
                var beepResult = ScannerController.BeepScanner("20");    // Fast warble beep
                _qrScannerLogger.LogInfo(beepResult.GetCommandResultDetails());
                */
                
                // Diagnostics completed successfully
                diagnosticsResult = new DiagnosticsResult(true, "Diagnostics successfully completed.", ScannerController.DetectedScanners, ScannerController.SelectedScanner, executedCommandsResults);
                _qrScannerLogger.LogInfo(diagnosticsResult.GetDiagnosticsResultDetails());

                _qrScannerLogger.LogInfo("Diagnostics successfully completed.");

                requiredDiagnosis = false;  // As diagnosis is completed, service can start

                return diagnosticsResult;
            }
            catch (QRScannerException e)
            {
                // Diagnostics failed
                diagnosticsResult = new DiagnosticsResult(false, "Diagnostics unsuccessful.", ScannerController.DetectedScanners, ScannerController.SelectedScanner, executedCommandsResults);
                _qrScannerLogger.LogError(e.Message);
                _qrScannerLogger.LogError(diagnosticsResult.GetDiagnosticsResultDetails());

                requiredDiagnosis = true;

                return diagnosticsResult;
            }
            catch (Exception e)
            {
                _qrScannerLogger.LogError($"An unexpected error occurred: {e.Message}");
                diagnosticsResult = new DiagnosticsResult(false, "Unexpected error during diagnostics.", ScannerController.DetectedScanners, ScannerController.SelectedScanner);
                return diagnosticsResult;
            }

        }

        private CommandResult TryScannerDetectionProcess(int maxAttempts, int sleepMilliseconds)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    _qrScannerLogger.LogInfo($"Attempt {attempt}/{maxAttempts}: Trying to detect scanners...");

                    CommandResult result = ScannerController.DetectScannersAndSelection();
                    _qrScannerLogger.LogInfo($"Detected {ScannerController.DetectedScanners.Count} scanner(s).");
                    _qrScannerLogger.LogInfo($"Selected scanner with ID {ScannerController.SelectedScanner.ScannerID}: {ScannerController.SelectedScanner.GetScannerDetails()}");

                    return result; // Success
                }
                catch (QRScannerException e)
                {
                    _qrScannerLogger.LogError($"Attempt {attempt} failed: {e.Message}");

                    if (attempt < maxAttempts)
                    {
                        _qrScannerLogger.LogWarning($"Retrying...");
                        Thread.Sleep(sleepMilliseconds);
                    }

                    if (attempt == maxAttempts)
                        return new CommandResult("Get Scanners.", 112, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<scanners>\r\n</scanners>", 0);
                }
            }

            // Fallback return statement
            throw new InvalidOperationException("Unexpected state: The detection loop exited without returning a CommandResult.");
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
