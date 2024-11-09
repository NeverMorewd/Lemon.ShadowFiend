namespace OcxHome.AxMsRdpHome
{
    partial class AxMsRdpHomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AxMsRdpHomeForm));
            axMsRdpClient8 = new AxMSTSCLib.AxMsRdpClient8NotSafeForScripting();
            ((System.ComponentModel.ISupportInitialize)axMsRdpClient8).BeginInit();
            SuspendLayout();
            // 
            // axMsRdpClient8
            // 
            axMsRdpClient8.Dock = DockStyle.Fill;
            axMsRdpClient8.Enabled = true;
            axMsRdpClient8.Location = new Point(0, 0);
            axMsRdpClient8.Name = "axMsRdpClient8";
            axMsRdpClient8.OcxState = (AxHost.State)resources.GetObject("axMsRdpClient8.OcxState");
            axMsRdpClient8.Size = new Size(800, 450);
            axMsRdpClient8.TabIndex = 0;
            // 
            // AxMsRdpHomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(axMsRdpClient8);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AxMsRdpHomeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "AxMsRdpHomeForm";
            ((System.ComponentModel.ISupportInitialize)axMsRdpClient8).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AxMSTSCLib.AxMsRdpClient8NotSafeForScripting axMsRdpClient8;
    }
}