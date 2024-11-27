namespace QRScanner.view
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            startService_Button = new Button();
            stopService_Button = new Button();
            textBox1 = new TextBox();
            detectedScanners_DataGrid = new DataGridView();
            scannerId = new DataGridViewTextBoxColumn();
            scannerType = new DataGridViewTextBoxColumn();
            serialNumber = new DataGridViewTextBoxColumn();
            GUID = new DataGridViewTextBoxColumn();
            VID = new DataGridViewTextBoxColumn();
            PID = new DataGridViewTextBoxColumn();
            modelNumber = new DataGridViewTextBoxColumn();
            dom = new DataGridViewTextBoxColumn();
            firmware = new DataGridViewTextBoxColumn();
            logs_Label = new Label();
            detectedScanners_Label = new Label();
            selectedScanner_Label = new Label();
            selectScanner_Button = new Button();
            scannerId_Label = new Label();
            scannerId_TextBox = new TextBox();
            claimScanner_CheckBox = new CheckBox();
            registerEvents_CheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)detectedScanners_DataGrid).BeginInit();
            SuspendLayout();
            // 
            // startService_Button
            // 
            startService_Button.Font = new Font("Segoe UI", 12F);
            startService_Button.Location = new Point(12, 12);
            startService_Button.Name = "startService_Button";
            startService_Button.Size = new Size(125, 50);
            startService_Button.TabIndex = 0;
            startService_Button.Text = "Start Service";
            startService_Button.UseVisualStyleBackColor = true;
            startService_Button.Click += startServiceButton_Click;
            // 
            // stopService_Button
            // 
            stopService_Button.Enabled = false;
            stopService_Button.Font = new Font("Segoe UI", 12F);
            stopService_Button.Location = new Point(143, 12);
            stopService_Button.Name = "stopService_Button";
            stopService_Button.Size = new Size(125, 50);
            stopService_Button.TabIndex = 1;
            stopService_Button.Text = "Stop Service";
            stopService_Button.UseVisualStyleBackColor = true;
            stopService_Button.Click += stopServiceButton_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 99);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(400, 350);
            textBox1.TabIndex = 2;
            // 
            // detectedScanners_DataGrid
            // 
            detectedScanners_DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            detectedScanners_DataGrid.Columns.AddRange(new DataGridViewColumn[] { scannerId, scannerType, serialNumber, GUID, VID, PID, modelNumber, dom, firmware });
            detectedScanners_DataGrid.Location = new Point(422, 99);
            detectedScanners_DataGrid.Name = "detectedScanners_DataGrid";
            detectedScanners_DataGrid.Size = new Size(400, 250);
            detectedScanners_DataGrid.TabIndex = 4;
            // 
            // scannerId
            // 
            scannerId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            scannerId.HeaderText = "ID";
            scannerId.Name = "scannerId";
            scannerId.Width = 43;
            // 
            // scannerType
            // 
            scannerType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            scannerType.HeaderText = "Type";
            scannerType.Name = "scannerType";
            scannerType.Width = 56;
            // 
            // serialNumber
            // 
            serialNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            serialNumber.HeaderText = "Serial Number";
            serialNumber.Name = "serialNumber";
            serialNumber.Width = 107;
            // 
            // GUID
            // 
            GUID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            GUID.HeaderText = "GUID";
            GUID.Name = "GUID";
            GUID.Width = 59;
            // 
            // VID
            // 
            VID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            VID.HeaderText = "VID";
            VID.Name = "VID";
            VID.Width = 50;
            // 
            // PID
            // 
            PID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PID.HeaderText = "PID";
            PID.Name = "PID";
            PID.Width = 50;
            // 
            // modelNumber
            // 
            modelNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            modelNumber.HeaderText = "Model Number";
            modelNumber.Name = "modelNumber";
            modelNumber.Width = 113;
            // 
            // dom
            // 
            dom.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dom.HeaderText = "DoM";
            dom.Name = "dom";
            dom.Width = 58;
            // 
            // firmware
            // 
            firmware.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            firmware.HeaderText = "Firmware";
            firmware.Name = "firmware";
            firmware.Width = 81;
            // 
            // logs_Label
            // 
            logs_Label.AutoSize = true;
            logs_Label.Font = new Font("Segoe UI", 12F);
            logs_Label.Location = new Point(12, 75);
            logs_Label.Name = "logs_Label";
            logs_Label.Size = new Size(43, 21);
            logs_Label.TabIndex = 5;
            logs_Label.Text = "Logs";
            // 
            // detectedScanners_Label
            // 
            detectedScanners_Label.AutoSize = true;
            detectedScanners_Label.Font = new Font("Segoe UI", 12F);
            detectedScanners_Label.Location = new Point(422, 75);
            detectedScanners_Label.Name = "detectedScanners_Label";
            detectedScanners_Label.Size = new Size(154, 21);
            detectedScanners_Label.TabIndex = 6;
            detectedScanners_Label.Text = "Detected Scanners: 0";
            // 
            // selectedScanner_Label
            // 
            selectedScanner_Label.AutoSize = true;
            selectedScanner_Label.Font = new Font("Segoe UI", 12F);
            selectedScanner_Label.Location = new Point(422, 352);
            selectedScanner_Label.Name = "selectedScanner_Label";
            selectedScanner_Label.Size = new Size(287, 21);
            selectedScanner_Label.TabIndex = 7;
            selectedScanner_Label.Text = "Selected Scanner: None [NOT CLAIMED]";
            // 
            // selectScanner_Button
            // 
            selectScanner_Button.Enabled = false;
            selectScanner_Button.Font = new Font("Segoe UI", 12F);
            selectScanner_Button.Location = new Point(697, 399);
            selectScanner_Button.Name = "selectScanner_Button";
            selectScanner_Button.Size = new Size(125, 50);
            selectScanner_Button.TabIndex = 8;
            selectScanner_Button.Text = "Select Scanner";
            selectScanner_Button.UseVisualStyleBackColor = true;
            selectScanner_Button.Click += selectScannerButton_Click;
            // 
            // scannerId_Label
            // 
            scannerId_Label.AutoSize = true;
            scannerId_Label.Font = new Font("Segoe UI", 12F);
            scannerId_Label.ImeMode = ImeMode.NoControl;
            scannerId_Label.Location = new Point(600, 396);
            scannerId_Label.Name = "scannerId_Label";
            scannerId_Label.Size = new Size(85, 21);
            scannerId_Label.TabIndex = 25;
            scannerId_Label.Text = "Scanner ID";
            // 
            // scannerId_TextBox
            // 
            scannerId_TextBox.Enabled = false;
            scannerId_TextBox.Font = new Font("Segoe UI", 12F);
            scannerId_TextBox.Location = new Point(600, 420);
            scannerId_TextBox.MaxLength = 10;
            scannerId_TextBox.Name = "scannerId_TextBox";
            scannerId_TextBox.Size = new Size(91, 29);
            scannerId_TextBox.TabIndex = 24;
            scannerId_TextBox.KeyPress += scannerIdLabel_KeyPress;
            // 
            // claimScanner_CheckBox
            // 
            claimScanner_CheckBox.AutoSize = true;
            claimScanner_CheckBox.Checked = true;
            claimScanner_CheckBox.CheckState = CheckState.Checked;
            claimScanner_CheckBox.Enabled = false;
            claimScanner_CheckBox.Font = new Font("Segoe UI", 12F);
            claimScanner_CheckBox.Location = new Point(422, 424);
            claimScanner_CheckBox.Name = "claimScanner_CheckBox";
            claimScanner_CheckBox.Size = new Size(129, 25);
            claimScanner_CheckBox.TabIndex = 26;
            claimScanner_CheckBox.Text = "Claim Scanner";
            claimScanner_CheckBox.UseVisualStyleBackColor = true;
            claimScanner_CheckBox.CheckedChanged += claimScannerCheckBox_CheckedChanged;
            // 
            // registerEvents_CheckBox
            // 
            registerEvents_CheckBox.AutoSize = true;
            registerEvents_CheckBox.Checked = true;
            registerEvents_CheckBox.CheckState = CheckState.Checked;
            registerEvents_CheckBox.Enabled = false;
            registerEvents_CheckBox.Font = new Font("Segoe UI", 12F);
            registerEvents_CheckBox.Location = new Point(422, 395);
            registerEvents_CheckBox.Name = "registerEvents_CheckBox";
            registerEvents_CheckBox.Size = new Size(135, 25);
            registerEvents_CheckBox.TabIndex = 27;
            registerEvents_CheckBox.Text = "Register Events";
            registerEvents_CheckBox.UseVisualStyleBackColor = true;
            registerEvents_CheckBox.CheckedChanged += registerEventsCheckBox_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 461);
            Controls.Add(registerEvents_CheckBox);
            Controls.Add(claimScanner_CheckBox);
            Controls.Add(scannerId_Label);
            Controls.Add(scannerId_TextBox);
            Controls.Add(selectScanner_Button);
            Controls.Add(selectedScanner_Label);
            Controls.Add(detectedScanners_Label);
            Controls.Add(logs_Label);
            Controls.Add(detectedScanners_DataGrid);
            Controls.Add(textBox1);
            Controls.Add(stopService_Button);
            Controls.Add(startService_Button);
            Name = "MainForm";
            Text = "QR Scanner Service";
            ((System.ComponentModel.ISupportInitialize)detectedScanners_DataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startService_Button;
        private Button stopService_Button;
        private TextBox textBox1;
        private DataGridView detectedScanners_DataGrid;
        private DataGridViewTextBoxColumn scannerId;
        private DataGridViewTextBoxColumn scannerType;
        private DataGridViewTextBoxColumn serialNumber;
        private DataGridViewTextBoxColumn GUID;
        private DataGridViewTextBoxColumn VID;
        private DataGridViewTextBoxColumn PID;
        private DataGridViewTextBoxColumn modelNumber;
        private DataGridViewTextBoxColumn dom;
        private DataGridViewTextBoxColumn firmware;
        private Label logs_Label;
        private Label detectedScanners_Label;
        private Label selectedScanner_Label;
        private Button selectScanner_Button;
        private Label scannerId_Label;
        private TextBox scannerId_TextBox;
        private CheckBox claimScanner_CheckBox;
        private CheckBox registerEvents_CheckBox;
    }
}