namespace ZebraScanner.view
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            initialize_Button = new Button();
            detectScanners_Button = new Button();
            selectScanner_Button = new Button();
            processedOutput_DataGridView = new DataGridView();
            scannerId = new DataGridViewTextBoxColumn();
            scannerType = new DataGridViewTextBoxColumn();
            serialNumber = new DataGridViewTextBoxColumn();
            GUID = new DataGridViewTextBoxColumn();
            VID = new DataGridViewTextBoxColumn();
            PID = new DataGridViewTextBoxColumn();
            modelNumber = new DataGridViewTextBoxColumn();
            dom = new DataGridViewTextBoxColumn();
            firmware = new DataGridViewTextBoxColumn();
            rawOutput_TextBox = new TextBox();
            rawOutput_Label = new Label();
            detectedScanners_Label = new Label();
            selectedScanner_Panel = new Panel();
            label3 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            led_Button = new Button();
            command_TextBox = new TextBox();
            beeper_Button = new Button();
            executeCommand_Button = new Button();
            captureVideo_Button = new Button();
            captureImage_Button = new Button();
            rebootScanner_Button = new Button();
            resetParams_Button = new Button();
            enableScan_CheckBox = new CheckBox();
            enableAim_CheckBox = new CheckBox();
            registerEvents_CheckBox = new CheckBox();
            getSDKVersion_Button = new Button();
            selectedScanner_Label = new Label();
            logs_Label = new Label();
            logs_TextBox = new TextBox();
            clearLogs_Button = new Button();
            scannerId_TextBox = new TextBox();
            label1 = new Label();
            claimScanner_Button = new Button();
            ScannersDetection_ProgressBar = new ProgressBar();
            numAttempts_Label = new Label();
            retrying_Label = new Label();
            start_Button = new Button();
            stop_Button = new Button();
            ((System.ComponentModel.ISupportInitialize)processedOutput_DataGridView).BeginInit();
            selectedScanner_Panel.SuspendLayout();
            SuspendLayout();
            // 
            // initialize_Button
            // 
            resources.ApplyResources(initialize_Button, "initialize_Button");
            initialize_Button.Name = "initialize_Button";
            initialize_Button.UseVisualStyleBackColor = true;
            initialize_Button.Click += initializeButton_Click;
            // 
            // detectScanners_Button
            // 
            resources.ApplyResources(detectScanners_Button, "detectScanners_Button");
            detectScanners_Button.Name = "detectScanners_Button";
            detectScanners_Button.UseVisualStyleBackColor = true;
            detectScanners_Button.Click += detectScannerButton_Click;
            // 
            // selectScanner_Button
            // 
            resources.ApplyResources(selectScanner_Button, "selectScanner_Button");
            selectScanner_Button.Name = "selectScanner_Button";
            selectScanner_Button.UseVisualStyleBackColor = true;
            selectScanner_Button.Click += selectScannerButton_Click;
            // 
            // processedOutput_DataGridView
            // 
            processedOutput_DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            processedOutput_DataGridView.Columns.AddRange(new DataGridViewColumn[] { scannerId, scannerType, serialNumber, GUID, VID, PID, modelNumber, dom, firmware });
            resources.ApplyResources(processedOutput_DataGridView, "processedOutput_DataGridView");
            processedOutput_DataGridView.Name = "processedOutput_DataGridView";
            // 
            // scannerId
            // 
            scannerId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(scannerId, "scannerId");
            scannerId.Name = "scannerId";
            // 
            // scannerType
            // 
            scannerType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(scannerType, "scannerType");
            scannerType.Name = "scannerType";
            // 
            // serialNumber
            // 
            serialNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(serialNumber, "serialNumber");
            serialNumber.Name = "serialNumber";
            // 
            // GUID
            // 
            GUID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(GUID, "GUID");
            GUID.Name = "GUID";
            // 
            // VID
            // 
            VID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(VID, "VID");
            VID.Name = "VID";
            // 
            // PID
            // 
            PID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(PID, "PID");
            PID.Name = "PID";
            // 
            // modelNumber
            // 
            modelNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(modelNumber, "modelNumber");
            modelNumber.Name = "modelNumber";
            // 
            // dom
            // 
            dom.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(dom, "dom");
            dom.Name = "dom";
            // 
            // firmware
            // 
            firmware.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(firmware, "firmware");
            firmware.Name = "firmware";
            // 
            // rawOutput_TextBox
            // 
            resources.ApplyResources(rawOutput_TextBox, "rawOutput_TextBox");
            rawOutput_TextBox.Name = "rawOutput_TextBox";
            rawOutput_TextBox.ReadOnly = true;
            // 
            // rawOutput_Label
            // 
            resources.ApplyResources(rawOutput_Label, "rawOutput_Label");
            rawOutput_Label.Name = "rawOutput_Label";
            // 
            // detectedScanners_Label
            // 
            resources.ApplyResources(detectedScanners_Label, "detectedScanners_Label");
            detectedScanners_Label.Name = "detectedScanners_Label";
            // 
            // selectedScanner_Panel
            // 
            selectedScanner_Panel.Controls.Add(label3);
            selectedScanner_Panel.Controls.Add(label2);
            selectedScanner_Panel.Controls.Add(textBox1);
            selectedScanner_Panel.Controls.Add(led_Button);
            selectedScanner_Panel.Controls.Add(command_TextBox);
            selectedScanner_Panel.Controls.Add(beeper_Button);
            selectedScanner_Panel.Controls.Add(executeCommand_Button);
            selectedScanner_Panel.Controls.Add(captureVideo_Button);
            selectedScanner_Panel.Controls.Add(captureImage_Button);
            selectedScanner_Panel.Controls.Add(rebootScanner_Button);
            selectedScanner_Panel.Controls.Add(resetParams_Button);
            selectedScanner_Panel.Controls.Add(enableScan_CheckBox);
            selectedScanner_Panel.Controls.Add(enableAim_CheckBox);
            selectedScanner_Panel.Controls.Add(registerEvents_CheckBox);
            selectedScanner_Panel.Controls.Add(getSDKVersion_Button);
            resources.ApplyResources(selectedScanner_Panel, "selectedScanner_Panel");
            selectedScanner_Panel.Name = "selectedScanner_Panel";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // textBox1
            // 
            resources.ApplyResources(textBox1, "textBox1");
            textBox1.Name = "textBox1";
            // 
            // led_Button
            // 
            resources.ApplyResources(led_Button, "led_Button");
            led_Button.Name = "led_Button";
            led_Button.UseVisualStyleBackColor = true;
            led_Button.Click += ledButton_Click;
            // 
            // command_TextBox
            // 
            resources.ApplyResources(command_TextBox, "command_TextBox");
            command_TextBox.Name = "command_TextBox";
            command_TextBox.KeyPress += commandTextBox_KeyPress;
            // 
            // beeper_Button
            // 
            resources.ApplyResources(beeper_Button, "beeper_Button");
            beeper_Button.Name = "beeper_Button";
            beeper_Button.UseVisualStyleBackColor = true;
            beeper_Button.Click += beeperButton_Click;
            // 
            // executeCommand_Button
            // 
            resources.ApplyResources(executeCommand_Button, "executeCommand_Button");
            executeCommand_Button.Name = "executeCommand_Button";
            executeCommand_Button.UseVisualStyleBackColor = true;
            executeCommand_Button.Click += executeCommandButton_Click;
            // 
            // captureVideo_Button
            // 
            resources.ApplyResources(captureVideo_Button, "captureVideo_Button");
            captureVideo_Button.Name = "captureVideo_Button";
            captureVideo_Button.UseVisualStyleBackColor = true;
            captureVideo_Button.Click += captureVideoButton_Click;
            // 
            // captureImage_Button
            // 
            resources.ApplyResources(captureImage_Button, "captureImage_Button");
            captureImage_Button.Name = "captureImage_Button";
            captureImage_Button.UseVisualStyleBackColor = true;
            captureImage_Button.Click += captureImageButton_Click;
            // 
            // rebootScanner_Button
            // 
            resources.ApplyResources(rebootScanner_Button, "rebootScanner_Button");
            rebootScanner_Button.Name = "rebootScanner_Button";
            rebootScanner_Button.UseVisualStyleBackColor = true;
            rebootScanner_Button.Click += rebootScannerButton_Click;
            // 
            // resetParams_Button
            // 
            resources.ApplyResources(resetParams_Button, "resetParams_Button");
            resetParams_Button.Name = "resetParams_Button";
            resetParams_Button.UseVisualStyleBackColor = true;
            resetParams_Button.Click += resetParamsButton_Click;
            // 
            // enableScan_CheckBox
            // 
            resources.ApplyResources(enableScan_CheckBox, "enableScan_CheckBox");
            enableScan_CheckBox.Checked = true;
            enableScan_CheckBox.CheckState = CheckState.Checked;
            enableScan_CheckBox.Name = "enableScan_CheckBox";
            enableScan_CheckBox.UseVisualStyleBackColor = true;
            enableScan_CheckBox.CheckedChanged += scan_CheckedChange;
            // 
            // enableAim_CheckBox
            // 
            resources.ApplyResources(enableAim_CheckBox, "enableAim_CheckBox");
            enableAim_CheckBox.Checked = true;
            enableAim_CheckBox.CheckState = CheckState.Checked;
            enableAim_CheckBox.Name = "enableAim_CheckBox";
            enableAim_CheckBox.UseVisualStyleBackColor = true;
            enableAim_CheckBox.CheckedChanged += aim_CheckedChange;
            // 
            // registerEvents_CheckBox
            // 
            resources.ApplyResources(registerEvents_CheckBox, "registerEvents_CheckBox");
            registerEvents_CheckBox.Checked = true;
            registerEvents_CheckBox.CheckState = CheckState.Checked;
            registerEvents_CheckBox.Name = "registerEvents_CheckBox";
            registerEvents_CheckBox.UseVisualStyleBackColor = true;
            registerEvents_CheckBox.CheckedChanged += registerEvents_CheckedChanged;
            // 
            // getSDKVersion_Button
            // 
            resources.ApplyResources(getSDKVersion_Button, "getSDKVersion_Button");
            getSDKVersion_Button.Name = "getSDKVersion_Button";
            getSDKVersion_Button.UseVisualStyleBackColor = true;
            getSDKVersion_Button.Click += getSDKVersionButton_Click;
            // 
            // selectedScanner_Label
            // 
            resources.ApplyResources(selectedScanner_Label, "selectedScanner_Label");
            selectedScanner_Label.Name = "selectedScanner_Label";
            // 
            // logs_Label
            // 
            resources.ApplyResources(logs_Label, "logs_Label");
            logs_Label.Name = "logs_Label";
            // 
            // logs_TextBox
            // 
            resources.ApplyResources(logs_TextBox, "logs_TextBox");
            logs_TextBox.Name = "logs_TextBox";
            logs_TextBox.ReadOnly = true;
            // 
            // clearLogs_Button
            // 
            resources.ApplyResources(clearLogs_Button, "clearLogs_Button");
            clearLogs_Button.Name = "clearLogs_Button";
            clearLogs_Button.UseVisualStyleBackColor = true;
            clearLogs_Button.Click += clearLogsButton_Click;
            // 
            // scannerId_TextBox
            // 
            resources.ApplyResources(scannerId_TextBox, "scannerId_TextBox");
            scannerId_TextBox.Name = "scannerId_TextBox";
            scannerId_TextBox.KeyPress += scannerIdTextBox_KeyPress;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // claimScanner_Button
            // 
            resources.ApplyResources(claimScanner_Button, "claimScanner_Button");
            claimScanner_Button.Name = "claimScanner_Button";
            claimScanner_Button.UseVisualStyleBackColor = true;
            claimScanner_Button.Click += claimScannerButton_Click;
            // 
            // ScannersDetection_ProgressBar
            // 
            resources.ApplyResources(ScannersDetection_ProgressBar, "ScannersDetection_ProgressBar");
            ScannersDetection_ProgressBar.Name = "ScannersDetection_ProgressBar";
            // 
            // numAttempts_Label
            // 
            resources.ApplyResources(numAttempts_Label, "numAttempts_Label");
            numAttempts_Label.Name = "numAttempts_Label";
            // 
            // retrying_Label
            // 
            resources.ApplyResources(retrying_Label, "retrying_Label");
            retrying_Label.Name = "retrying_Label";
            // 
            // start_Button
            // 
            resources.ApplyResources(start_Button, "start_Button");
            start_Button.Name = "start_Button";
            start_Button.UseVisualStyleBackColor = true;
            start_Button.Click += startButton_Click;
            // 
            // stop_Button
            // 
            resources.ApplyResources(stop_Button, "stop_Button");
            stop_Button.Name = "stop_Button";
            stop_Button.UseVisualStyleBackColor = true;
            stop_Button.Click += stopButton_Click;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(stop_Button);
            Controls.Add(start_Button);
            Controls.Add(retrying_Label);
            Controls.Add(numAttempts_Label);
            Controls.Add(ScannersDetection_ProgressBar);
            Controls.Add(claimScanner_Button);
            Controls.Add(label1);
            Controls.Add(scannerId_TextBox);
            Controls.Add(clearLogs_Button);
            Controls.Add(logs_TextBox);
            Controls.Add(logs_Label);
            Controls.Add(selectedScanner_Label);
            Controls.Add(selectedScanner_Panel);
            Controls.Add(detectedScanners_Label);
            Controls.Add(rawOutput_Label);
            Controls.Add(rawOutput_TextBox);
            Controls.Add(processedOutput_DataGridView);
            Controls.Add(selectScanner_Button);
            Controls.Add(detectScanners_Button);
            Controls.Add(initialize_Button);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)processedOutput_DataGridView).EndInit();
            selectedScanner_Panel.ResumeLayout(false);
            selectedScanner_Panel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button initialize_Button;
        private Button detectScanners_Button;
        private Button selectScanner_Button;
        private DataGridView processedOutput_DataGridView;
        private DataGridViewTextBoxColumn scannerId;
        private DataGridViewTextBoxColumn scannerType;
        private DataGridViewTextBoxColumn serialNumber;
        private DataGridViewTextBoxColumn GUID;
        private DataGridViewTextBoxColumn VID;
        private DataGridViewTextBoxColumn PID;
        private DataGridViewTextBoxColumn modelNumber;
        private DataGridViewTextBoxColumn dom;
        private DataGridViewTextBoxColumn firmware;
        private TextBox rawOutput_TextBox;
        private Label rawOutput_Label;
        private Label detectedScanners_Label;
        private Panel selectedScanner_Panel;
        private Label selectedScanner_Label;
        private Label logs_Label;
        private TextBox logs_TextBox;
        private Button getSDKVersion_Button;
        private CheckBox registerEvents_CheckBox;
        private CheckBox enableAim_CheckBox;
        private CheckBox enableScan_CheckBox;
        private Button resetParams_Button;
        private Button captureImage_Button;
        private Button rebootScanner_Button;
        private Button captureVideo_Button;
        private Button executeCommand_Button;
        private Button clearLogs_Button;
        private TextBox scannerId_TextBox;
        private Label label1;
        private Button beeper_Button;
        private Button claimScanner_Button;
        private TextBox command_TextBox;
        private Button led_Button;
        private Label label3;
        private Label label2;
        private TextBox textBox1;
        private ProgressBar ScannersDetection_ProgressBar;
        private Label numAttempts_Label;
        private Label retrying_Label;
        private Button start_Button;
        private Button stop_Button;
    }
}