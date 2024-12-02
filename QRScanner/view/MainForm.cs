using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRScanner.Exceptions;
using QRScanner.model;
using QRScanner.service;
using QRScanner.utility;

namespace QRScanner.view
{
    public partial class MainForm : Form
    {
        #region Attributes and instances

        private Logger _logger = Logger.Instance;
        private QRScannerService _qrScannerService = QRScannerService.Instance;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            setDefaultOperations();
        }

        #region Buttons

        private void startServiceButton_Click(object sender, EventArgs e)
        {
            bool success = _qrScannerService.StartScanning();

            UpdateLogs();

            if (success)
            {
                enableOperations(true);

                detectedScanners_Label.Text = $"Detected Scanners: {_qrScannerService.ScannerController.DetectedScanners.Count}";
                selectedScanner_Label.Text = $"Selected Scanner: {_qrScannerService.ScannerController.SelectedScanner.ScannerID}";

                fillScannersTable();
            }
        }

        private void stopServiceButton_Click(object sender, EventArgs e)
        {
            bool success = _qrScannerService.StopScanning();

            UpdateLogs();

            if (success)
            {
                clearScannersTable();
                enableOperations(false);
                setDefaultOperations();
            }
        }

        private void selectScannerButton_Click(object sender, EventArgs e)
        {
            int scannerId = getScannerIdInput();
            if (scannerId == -1)
            {
                return;
            }

            try
            {
                _qrScannerService.ScannerController.SelectScannerById(scannerId);

                MessageBox.Show($"Scanner with ID {scannerId} selected succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _logger.LogInfo($"Scanner with ID {scannerId} selected succcesfully.");

                UpdateLogs();

                scannerId_TextBox.Clear();

                registerEvents_CheckBox.Checked = false;
                claimScanner_CheckBox.Checked = false;
                selectedScanner_Label.Text = $"Selected Scanner: {scannerId}";
            }
            catch (ScannerNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError(ex.Message);
                UpdateLogs();
            }
            catch (ScannerAlreadySelectedException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _logger.LogError(ex.Message);
                UpdateLogs();
            }
        }

        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            _logger.ClearLogs();
            UpdateLogs();
        }

        private void beepButton_Click(object sender, EventArgs e)
        {
            try
            {
                _qrScannerService.ScannerController.BeepScanner();
            }
            catch (CommandExecutionFailedException ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        #endregion

        #region Checkboxes

        private void registerEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (registerEvents_CheckBox.Checked)
            {
                try
                {
                    _qrScannerService.ScannerController.RegisterForAllEvents();

                    _logger.LogInfo("Registering for all events...");
                    UpdateLogs();
                }
                catch (CommandExecutionFailedException ex)
                {
                    _logger.LogError(ex.Message);
                    UpdateLogs();
                }
            }
            else
            {
                try
                {
                    _qrScannerService.ScannerController.UnregisterForAllEvents();

                    _logger.LogInfo("Unregistering for all events...");
                    UpdateLogs();
                }
                catch (CommandExecutionFailedException ex)
                {
                    _logger.LogError(ex.Message);
                    UpdateLogs();
                }
            }
        }

        private void claimScannerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (claimScanner_CheckBox.Checked)
            {
                try
                {
                    _qrScannerService.ScannerController.ClaimScanner();

                    _logger.LogInfo("Scanner claimed.");
                    UpdateLogs();
                }
                catch (CommandExecutionFailedException ex)
                {
                    _logger.LogError(ex.Message);
                    UpdateLogs();
                }
            }
            else
            {
                try
                {
                    _qrScannerService.ScannerController.ReleaseScanner();

                    _logger.LogInfo("Scanner released.");
                    UpdateLogs();
                }
                catch (CommandExecutionFailedException ex)
                {
                    _logger.LogError(ex.Message);
                    UpdateLogs();
                }
            }
        }

        #endregion

        #region Input related

        private int getScannerIdInput()
        {
            // Get the raw input from the TextBox
            string rawInputId = scannerId_TextBox.Text.Trim();

            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(rawInputId))
            {
                MessageBox.Show("Please enter a scanner ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _logger.LogWarning("The scanner ID input is empty.");
                return -1; // Return -1 to indicate invalid input
            }

            // Try to parse the input to an integer
            if (!int.TryParse(rawInputId, out int scannerId))
            {
                MessageBox.Show("The scanner ID must be a valid number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _logger.LogWarning($"The scanner ID must be a valid number: '{rawInputId}'");
                scannerId_TextBox.Clear(); // Clear the invalid input
                return -1;
            }

            // Check if the scanner ID is valid (greater than 0)
            if (scannerId <= 0)
            {
                MessageBox.Show("The scanner ID must be a positive number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _logger.LogWarning($"The scanner ID must be a positive number: '{scannerId}'.");
                scannerId_TextBox.Clear();
                return -1;
            }

            // Return the valid scanner ID
            return scannerId;
        }

        private void scannerIdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region UI related

        private void setDefaultOperations()
        {
            detectedScanners_Label.Text = "Detected Scanners: 0";
            selectedScanner_Label.Text = "Selected Scanner: None";
            scannerId_TextBox.Clear();
        }

        private void enableOperations(bool enable)
        {
            stopService_Button.Enabled = enable;
            registerEvents_CheckBox.Enabled = enable;
            claimScanner_CheckBox.Enabled = enable;
            scannerId_TextBox.Enabled = enable;
            selectScanner_Button.Enabled = enable;
            beep_Button.Enabled = enable;
        }

        public void UpdateLogs()
        {
            logs_TextBox.Clear();
            foreach (string log in _logger.GetLogs())
            {
                logs_TextBox.AppendText(log + Environment.NewLine);
            }
        }

        private void fillScannersTable()
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

        private void clearScannersTable()
        {
            detectedScanners_DataGridView.Rows.Clear();
        }

        #endregion
    }
}
