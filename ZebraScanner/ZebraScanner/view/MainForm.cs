using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZebraScanner.model;
using ZebraScanner.utility;
using ZebraScanner.controller;
using ZebraScanner.events;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ZebraScanner.view
{
    public partial class MainForm : Form
    {
        // Instances
        private readonly ScannerController controller;
        private readonly Logger logger = Logger.Instance;   // Using singleton Logger instance
        private List<Scanner> connectedScanners = new List<Scanner>();
        private Scanner selectedScanner;
        private CommandResult commandResult = new CommandResult();

        public MainForm()
        {
            InitializeComponent();
            controller = new ScannerController();

            // Subscribe to the BarcodeScanned event
            controller.BarcodeScanned += OnBarcodeScanned;
        }

        private void OnBarcodeScanned(object sender, BarcodeScannedEventArgs e)
        {
            if (logs_TextBox.InvokeRequired)
            {
                // Invoke this method withing main subprocess
                logs_TextBox.Invoke(new Action(() => OnBarcodeScanned(sender, e)));
                return;
            }

            populateRawOutput(e.RawXml);

            logger.LogInfo($"Scanned Barcode: Type={e.DataType}, Label={e.DataLabel}");
        }

        // Utility methods for the logs
        private void updateLogsUI()
        {
            logs_TextBox.Clear();
            foreach (var log in logger.GetLogs())
            {
                logs_TextBox.AppendText(log + Environment.NewLine);
            }
        }

        private bool checkAndLogCommandResult(string message)
        {
            if (commandResult.Status == 0)
            {
                logger.LogInfo(message);
                updateLogsUI();
                return true;
            }
            else
            {
                logger.LogError(commandResult.StatusMessage);
                updateLogsUI();
                return false;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {

        }

        private void stopButton_Click(object sender, EventArgs e)
        {

        }

        // API related methods
        private void initializeButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.OpenCoreScannerAPI();

            if (checkAndLogCommandResult("CoreScanner API initialized."))
            {
                detectScanners_Button.Enabled = true;
            }
        }

        private async void detectScannerButton_Click(object sender, EventArgs e)
        {
            // Instance and show new Form
            var scannersDetectionProgressForm = new ScannersDetectionProgressForm();
            scannersDetectionProgressForm.Show();

            // Start detection
            // Change if needed
            int maxAttempts = 3;
            int retryDelay = 3000;

            // Clear current Form output
            clearRawOutput();
            clearDetectedScanners();

            await scannersDetectionProgressForm.DetectScannersAsync(maxAttempts, retryDelay);

            // Process detected scanners
            if (scannersDetectionProgressForm.DetectedScanners.Count > 0)
            {
                logger.LogInfo($"Detected {scannersDetectionProgressForm.CommandResult.NumberOfScanners} scanner(s).");
                connectedScanners = scannersDetectionProgressForm.DetectedScanners;
                detectedScanners_Label.Text = $"Detected Scanners: {scannersDetectionProgressForm.CommandResult.NumberOfScanners}";
                populateScannersTable();
                populateRawOutput(scannersDetectionProgressForm.CommandResult.OutXml);
                enableScannerSelection();
            }
            else
            {
                logger.LogError(scannersDetectionProgressForm.IsCanceled()
                    ? "Detection canceled."
                    : "No scanners detected after maximum attempts.");
            }
            updateLogsUI();
        }

        private void selectScannerButton_Click(object sender, EventArgs e)
        {
            // Get and validate the scanner ID from the input
            int scannerId = getScannerIdInput();
            if (scannerId == -1)
            {
                return; // Exit if the input is invalid
            }

            // Search for the scanner in the connectedScanners list
            selectedScanner = connectedScanners.FirstOrDefault(scanner => scanner.ScannerID == scannerId);

            if (selectedScanner != null)
            {
                // Scanner found
                MessageBox.Show($"Scanner with ID {scannerId} selected succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.LogInfo($"Scanner {scannerId} selected succcesfully.");
                scannerId_TextBox.Clear();
                selectedScanner_Label.Text = $"Selected Scanner: {selectedScanner.ScannerID} [NOT CLAIMED]";
                enableScannerOperations();
            }
            else
            {
                // Scanner not found
                MessageBox.Show($"Scanner with ID {scannerId} not found in the connected scanners. Make sure the device is currently connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogError($"Scanner {scannerId} not found.");
                scannerId_TextBox.Clear();
            }
            updateLogsUI();
        }

        private void claimScannerButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.ClaimScanner();

            selectedScanner_Label.Text = $"Selected Scanner: {selectedScanner.ScannerID} [CLAIMED]";
            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Scanner claimed succesfully.");
        }

        private void getSDKVersionButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.GET_VERSION, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Scanner's SDK version info.");
        }

        private void resetParamsButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.SET_PARAMETER_DEFAULTS, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Scanner's parameters reseted.");
        }


        private void rebootScannerButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.REBOOT_SCANNER, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Scanner rebooted.");
        }

        private void captureImageButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.DEVICE_CAPTURE_IMAGE, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Capturing image...");
        }

        private void captureVideoButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.DEVICE_CAPTURE_VIDEO, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Capturing video...");
        }

        private void registerEvents_CheckedChanged(object sender, EventArgs e)
        {
            if (registerEvents_CheckBox.Checked)
            {
                commandResult = controller.RegisterForAllEvents();

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Registering for events...");
            }
            else
            {
                commandResult = controller.UnregisterForAllEvents();

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Unregistering for events...");
            }
        }

        private void aim_CheckedChange(object sender, EventArgs e)
        {
            if (enableAim_CheckBox.Checked)
            {
                commandResult = controller.CommandScanner(OpcodesHandler.AIM_ON, "");

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Aim enabled.");
            }
            else
            {
                commandResult = controller.CommandScanner(OpcodesHandler.AIM_OFF, "");

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Aim disabled.");
            }
        }

        private void scan_CheckedChange(object sender, EventArgs e)
        {
            if (enableScan_CheckBox.Checked)
            {
                commandResult = controller.CommandScanner(OpcodesHandler.SCAN_ENABLE, "");

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Scan enabled.");
            }
            else
            {
                commandResult = controller.CommandScanner(OpcodesHandler.SCAN_DISABLE, "");

                clearRawOutput();
                populateRawOutput(commandResult.OutXml);

                checkAndLogCommandResult("Scan disabled.");
            }
        }

        private void beeperButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.SET_ACTION, "1");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Beep!");
        }

        private void ledButton_Click(object sender, EventArgs e)
        {
            commandResult = controller.CommandScanner(OpcodesHandler.SET_ACTION, "0x2F");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Red LED on!");
        }

        private void executeCommandButton_Click(object sender, EventArgs e)
        {
            // Get and validate the command from the input
            int command = getCommandInput();
            if (command == -1)
            {
                return; // Exit if the input is invalid
            }

            commandResult = controller.CommandScanner(command, "");

            clearRawOutput();
            populateRawOutput(commandResult.OutXml);

            checkAndLogCommandResult("Command executed successfully.");
        }

        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            logger.ClearLogs();
            updateLogsUI();
        }

        private void exportLogsButton_Click(object sender, EventArgs e)
        {

        }

        // Custom methods

        // Methods to modify current Form
        private void populateScannersTable()
        {
            processedOutput_DataGridView.Rows.Clear();

            foreach (Scanner scanner in connectedScanners)
            {
                processedOutput_DataGridView.Rows.Add(
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

        private void populateRawOutput(string outXML)
        {
            rawOutput_TextBox.Text = outXML;
        }

        private void clearDetectedScanners()
        {
            processedOutput_DataGridView.Rows.Clear();
        }

        private void clearRawOutput()
        {
            rawOutput_TextBox.Clear();
        }

        private void enableScannerOperations()
        {
            getSDKVersion_Button.Enabled = true;
            resetParams_Button.Enabled = true;
            rebootScanner_Button.Enabled = true;
            captureImage_Button.Enabled = true;
            captureVideo_Button.Enabled = true;
            beeper_Button.Enabled = true;
            executeCommand_Button.Enabled = true;
            command_TextBox.Enabled = true;
            registerEvents_CheckBox.Enabled = true;
            enableAim_CheckBox.Enabled = true;
            enableScan_CheckBox.Enabled = true;
            claimScanner_Button.Enabled = true;
        }

        private void enableScannerSelection()
        {
            selectScanner_Button.Enabled = true;
            scannerId_TextBox.Enabled = true;
        }

        private void scannerIdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void commandTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private int getCommandInput()
        {
            // Get the raw input from the TextBox
            string rawInputCommand = command_TextBox.Text.Trim();

            // Check and parse the input
            if (string.IsNullOrWhiteSpace(rawInputCommand))
            {
                MessageBox.Show("The command input is empty. Please enter a valid opcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogWarning("The command input is empty.");
                return -1;
            }

            if (!int.TryParse(rawInputCommand, out int parsedOpCode) || parsedOpCode <= 0)
            {
                MessageBox.Show($"Invalid command input: '{rawInputCommand}'. The opcode must be a positive integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogWarning($"Invalid command input: '{rawInputCommand}'.");
                command_TextBox.Clear(); // Clear the invalid input
                return -1;
            }

            // Valid opcode
            return parsedOpCode;
        }

        private int getScannerIdInput()
        {
            // Get the raw input from the TextBox
            string rawInputId = scannerId_TextBox.Text.Trim();

            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(rawInputId))
            {
                MessageBox.Show("Please enter a scanner ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogWarning("The scanner ID input is empty.");
                return -1; // Return -1 to indicate invalid input
            }

            // Try to parse the input to an integer
            if (!int.TryParse(rawInputId, out int scannerId))
            {
                MessageBox.Show("The scanner ID must be a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogWarning($"The scanner ID must be a valid number: '{rawInputId}'");
                scannerId_TextBox.Clear(); // Clear the invalid input
                return -1;
            }

            // Check if the scanner ID is valid (greater than 0)
            if (scannerId <= 0)
            {
                MessageBox.Show("The scanner ID must be a positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.LogWarning($"The scanner ID must be a positive number: '{scannerId}'.");
                scannerId_TextBox.Clear();
                return -1;
            }

            // Return the valid scanner ID
            return scannerId;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
