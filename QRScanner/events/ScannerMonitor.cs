using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using QRScanner.utility;

namespace QRScanner.events
{
    class ScannerMonitor
    {
        private readonly string targetDeviceVID;
        private readonly string targetDevicePID;
        private ManagementEventWatcher watcher;
        private readonly QRScannerLogger _qrScannerLogger = QRScannerLogger.Instance;

        public event EventHandler ScannerDisconnected;

        public ScannerMonitor(string vid, string pid)
        {
            targetDeviceVID = vid;
            targetDevicePID = pid;
            InitializeScannerMonitor();
        }

        public void InitializeScannerMonitor()
        {
            string query = @"SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'";

            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += new EventArrivedEventHandler(OnDeviceRemoved);
            watcher.Start();

            _qrScannerLogger.LogInfo("Started monitoring scanner disconnection events...");
        }

        private void OnDeviceRemoved(object sender, EventArrivedEventArgs e)
        {
            var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string? deviceId = instance["DeviceID"]?.ToString();

            if (deviceId != null && deviceId.Contains(targetDeviceVID) && deviceId.Contains(targetDevicePID))
            {
                ScannerDisconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void StopMonitoring()
        {
            if (watcher != null)
            {
                watcher.Stop();
                watcher.Dispose();
                _qrScannerLogger.LogInfo("Stopped monitoring scanner disconnection events...");
            }
        }
    }
}
