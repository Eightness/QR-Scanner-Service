using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRScanner.view
{
    public partial class MainForm : Form
    {
        #region Attributes and instances

 

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        #region Buttons

        private void startServiceButton_Click(object sender, EventArgs e)
        {

        }

        private void stopServiceButton_Click(object sender, EventArgs e)
        {

        }

        private void selectScannerButton_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Checkboxes

        private void registerEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void claimScannerCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Input related

        private void scannerIdLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region UI related


        #endregion
    }
}
