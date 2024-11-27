using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZebraScanner.controller;
using ZebraScanner.Exceptions;
using ZebraScanner.model;
using ZebraScanner.utility;

namespace ZebraScanner.view
{
    public partial class ScannersDetectionProgressForm : Form
    {
        // Instances
        private ScannerController controller = new ScannerController();
        private XMLReader xmlReader = new XMLReader();
        private Logger logger = Logger.Instance;

        public List<Scanner> DetectedScanners { get; private set; } = new List<Scanner>();
        public CommandResult CommandResult { get; private set; }
        private bool cancelRequested = false;

        public ScannersDetectionProgressForm()
        {
            InitializeComponent();
        }

        public async Task DetectScannersAsync(int maxAttempts, int retryDelay)
        {
            controller.OpenCoreScannerAPI();
            int currentAttempt = 0;

            while (currentAttempt < maxAttempts && !cancelRequested)
            {
                // Update UI for attempt
                numAttempts_Label.Text = $"Attempt {currentAttempt + 1} / {maxAttempts}";
                retrying_Label.Text = $"Retrying in {retryDelay / 1000} seconds...";

                // Reset and configure progress bar
                ScannersDetection_ProgressBar.Value = 0;
                ScannersDetection_ProgressBar.Maximum = 100;

                try
                {
                    CommandResult = controller.DetectScanners();
                    DetectedScanners = xmlReader.GetAllScannersFromXml(CommandResult.OutXml);

                    if (DetectedScanners.Count > 0)
                    {
                        // Scanners detected
                        retrying_Label.Text = "Scanner(s) detected!";
                        ScannersDetection_ProgressBar.Value = 100;
                        cancelDetection_Button.Text = "Done";
                        break;
                    }
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
                catch (NoScannersFoundException noScannersFoundException)
                {
                    Console.WriteLine(noScannersFoundException.Message);
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    Console.WriteLine(invalidOperationException.Message);
                }

                if (cancelRequested)
                {
                    Console.WriteLine($"Detection canceled by the user.");
                    break;
                }

                // Simulate delay with progress bar update
                int interval = 100; // Update every 100ms
                int totalIntervals = retryDelay / interval;

                for (int i = 0; i < totalIntervals; i++)
                {
                    if (cancelRequested)
                    {
                        break;
                    }

                    // Increment progress
                    ScannersDetection_ProgressBar.Value = (int)((i + 1) / (double)totalIntervals * 100);

                    await Task.Delay(interval); // Wait for the interval
                }

                currentAttempt++;
            }

            controller.CloseCoreScannerAPI();
            Close();
        }

        private void cancelDetectionButton_Click(object sender, EventArgs e)
        {
            cancelRequested = true;
            cancelDetection_Button.Enabled = false;
            cancelDetection_Button.Text = "Canceling...";
            retrying_Label.Text = "Aborting detection...";
            Close();
        }

        public bool IsCanceled()
        {
            return cancelRequested;
        }
    }
}
