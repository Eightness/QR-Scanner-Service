using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRScanner.controller;
using QRScanner.events;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.service;
using QRScanner.utility;

namespace QRScanner.view
{
    public partial class MainForm : Form
    {
        #region Attributes and instances

        private QRScannerLogger _qrScannerLogger = QRScannerLogger.Instance;
        private QRScannerService _qrScannerService = QRScannerService.Instance;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            SetDefaultOperations();
        }

        #region Buttons

        private async void diagnosticsButton_Click(object sender, EventArgs e)
        {
            DiagnosticsResult result = await _qrScannerService.RunDiagnostics(5, 3000);

            if (result.Success)
                startService_Button.Enabled = true;
            else
                startService_Button.Enabled = false;

            UpdateLogs();
            FillScannersTable();
            EnableOperations(true);

            detectedScanners_Label.Text = $"Detected Scanners: {_qrScannerService.ScannerController.DetectedScanners.Count}";
            selectedScanner_Label.Text = $"Selected Scanner: {_qrScannerService.ScannerController.SelectedScanner.ScannerID}";
            diagnostics_Button.Enabled = false;
        }

        private void startServiceButton_Click(object sender, EventArgs e)
        {
            bool success = _qrScannerService.StartScanning();

            UpdateLogs();

            if (success)
            {
                SubscribeToQRCodeDecoded();
                startService_Button.Enabled = false;
                stopService_Button.Enabled = true;
            }
        }

        private async void stopServiceButton_Click(object sender, EventArgs e)
        {
            bool success = await _qrScannerService.StopScanningAsync();

            UpdateLogs();

            if (success)
            {
                UnsubscribeToQRCodeDecoded();

                InvokeUI(() =>
                {
                    ClearScannersTable();
                    EnableOperations(false);
                    SetDefaultOperations();
                });
            }

            diagnostics_Button.Enabled = true;
            stopService_Button.Enabled = false;
        }

        private void selectScannerButton_Click(object sender, EventArgs e)
        {
            int scannerId = GetScannerIdInput();
            if (scannerId == -1)
            {
                return;
            }

            try
            {
                _qrScannerService.ScannerController.SelectScannerById(scannerId);
                _qrScannerService.ScannerController.DisableScanForAllScanners();
                _qrScannerService.ScannerController.EnableScan(true);

                MessageBox.Show($"Scanner with ID {scannerId} selected succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _qrScannerLogger.LogInfo($"Scanner with ID {scannerId} selected succcesfully.");

                UpdateLogs();

                scannerId_TextBox.Clear();

                selectedScanner_Label.Text = $"Selected Scanner: {scannerId}";
            }
            catch (ScannerNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _qrScannerLogger.LogError(ex.Message);
                UpdateLogs();
            }
            catch (ScannerAlreadySelectedException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _qrScannerLogger.LogError(ex.Message);
                UpdateLogs();
            }
        }

        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            _qrScannerLogger.ClearLogs();
            UpdateLogs();
        }

        private void beepButton_Click(object sender, EventArgs e)
        {
            try
            {
                _qrScannerService.ScannerController.BeepScanner("10");
            }
            catch (CommandExecutionFailedException ex)
            {
                _qrScannerLogger.LogError(ex.Message);
            }
        }

        #endregion

        #region Input related

        private int GetScannerIdInput()
        {
            // Get the raw input from the TextBox
            string rawInputId = scannerId_TextBox.Text.Trim();

            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(rawInputId))
            {
                MessageBox.Show("Please enter a scanner ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _qrScannerLogger.LogWarning("The scanner ID input is empty.");
                return -1; // Return -1 to indicate invalid input
            }

            // Try to parse the input to an integer
            if (!int.TryParse(rawInputId, out int scannerId))
            {
                MessageBox.Show("The scanner ID must be a valid number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _qrScannerLogger.LogWarning($"The scanner ID must be a valid number: '{rawInputId}'");
                scannerId_TextBox.Clear(); // Clear the invalid input
                return -1;
            }

            // Check if the scanner ID is valid (greater than 0)
            if (scannerId <= 0)
            {
                MessageBox.Show("The scanner ID must be a positive number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _qrScannerLogger.LogWarning($"The scanner ID must be a positive number: '{scannerId}'.");
                scannerId_TextBox.Clear();
                return -1;
            }

            // Return the valid scanner ID
            scannerId_TextBox.Clear();
            return scannerId;
        }

        private void scannerIdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region UI related

        private void SetDefaultOperations()
        {
            if (startService_Button.InvokeRequired ||
                detectedScanners_Label.InvokeRequired ||
                selectedScanner_Label.InvokeRequired ||
                scannerId_TextBox.InvokeRequired)
            {
                startService_Button.Invoke(new Action(SetDefaultOperations));
            }
            else
            {
                detectedScanners_Label.Text = "Detected Scanners: 0";
                selectedScanner_Label.Text = "Selected Scanner: None";
                scannerId_TextBox.Clear();
            }
        }

        private void EnableOperations(bool enable)
        {
            if (scannerId_TextBox.InvokeRequired ||
                selectScanner_Button.InvokeRequired ||
                beep_Button.InvokeRequired)
            {
                stopService_Button.Invoke(new Action(() => EnableOperations(enable)));
            }
            else
            {
                scannerId_TextBox.Enabled = enable;
                selectScanner_Button.Enabled = enable;
                beep_Button.Enabled = enable;
            }
        }

        private void UpdateLogs()
        {
            if (logs_TextBox.InvokeRequired)
            {
                logs_TextBox.Invoke(new Action(UpdateLogs));
            }
            else
            {
                logs_TextBox.Clear();
                foreach (string log in _qrScannerLogger.GetLogs())
                {
                    logs_TextBox.AppendText(log + Environment.NewLine);
                }
            }
        }

        private void FillScannersTable()
        {
            detectedScanners_DataGridView.Rows.Clear();

            foreach (Scanner scanner in _qrScannerService.ScannerController.DetectedScanners)
            {
                detectedScanners_DataGridView.Rows.Add(
                    scanner.ScannerID,
                    scanner.ScannerType,
                    scanner.SerialNumber,
                    scanner.GUID,
                    scanner.VID,
                    scanner.PID,
                    scanner.ModelNumber,
                    scanner.DOM,
                    scanner.Firmware
                );
            }
        }

        private void ClearScannersTable()
        {
            if (detectedScanners_DataGridView.InvokeRequired)
            {
                detectedScanners_DataGridView.Invoke(new Action(ClearScannersTable));
            }
            else
            {
                detectedScanners_DataGridView.Rows.Clear();
            }
        }

        private void InvokeUI(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }


        #endregion

        #region Events Handling

        private void SubscribeToQRCodeDecoded()
        {
            // Subscribes to QRCodeDecoded event from QR Scanner Service
            _qrScannerService.QRCodeDecoded += OnQRCodeDecoded;
        }

        private void UnsubscribeToQRCodeDecoded()
        {
            // Unsubscribes to QRCodeDecoded event from QR Scanner Service
            _qrScannerService.QRCodeDecoded -= OnQRCodeDecoded;
        }

        private void OnQRCodeDecoded(object sender, BarcodeScannedEventArgs e)
        {
            _qrScannerLogger.LogInfo($"QR Scanned: {e.DecodedDataLabel}");
            UpdateLogs();
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
