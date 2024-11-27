using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using QRScanner.view;
using QRScanner.controller;

/// <summary>
/// Entry point for the Zebra Scanner application.
/// Sets up and runs the main form within a Windows Forms environment.
/// </summary>
class Program
{
    /// <summary>
    /// The main method serves as the entry point for the application.
    /// Configures application-level settings and launches the main form.
    /// </summary>
    [STAThread] // Indicates that the COM threading model for the application is single-threaded apartment.
    static void Main()
    {
        // Enable modern visual styles for the application.
        Application.EnableVisualStyles();

        // Ensure compatible text rendering across different controls.
        Application.SetCompatibleTextRenderingDefault(false);

        // Launch the main form (MainForm) of the application.
        Application.Run(new MainForm());
    }
}
