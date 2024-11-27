namespace ZebraScanner.view
{
    partial class ScannersDetectionProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScannersDetectionProgressForm));
            ScannersDetection_ProgressBar = new ProgressBar();
            numAttempts_Label = new Label();
            retrying_Label = new Label();
            cancelDetection_Button = new Button();
            SuspendLayout();
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
            // cancelDetection_Button
            // 
            resources.ApplyResources(cancelDetection_Button, "cancelDetection_Button");
            cancelDetection_Button.Name = "cancelDetection_Button";
            cancelDetection_Button.UseVisualStyleBackColor = true;
            cancelDetection_Button.Click += cancelDetectionButton_Click;
            // 
            // ScannersDetectionProgressForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cancelDetection_Button);
            Controls.Add(retrying_Label);
            Controls.Add(numAttempts_Label);
            Controls.Add(ScannersDetection_ProgressBar);
            Name = "ScannersDetectionProgressForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar ScannersDetection_ProgressBar;
        private Label numAttempts_Label;
        private Label retrying_Label;
        private Button cancelDetection_Button;
    }
}