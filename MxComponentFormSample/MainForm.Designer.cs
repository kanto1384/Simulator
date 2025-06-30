namespace MxComponentFormSample
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtLogicalStation;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnEnableWrite;
        private System.Windows.Forms.Label lblStatus;

        private void InitializeComponent()
        {
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtLogicalStation = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnEnableWrite = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(12, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(150, 21);
            // 
            // txtLogicalStation
            // 
            this.txtLogicalStation.Location = new System.Drawing.Point(168, 12);
            this.txtLogicalStation.Name = "txtLogicalStation";
            this.txtLogicalStation.Size = new System.Drawing.Size(50, 21);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(224, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(12, 50);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.Size = new System.Drawing.Size(75, 21);
            this.txtDevice.Text = "B0";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(93, 50);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(75, 21);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(174, 48);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(60, 23);
            this.btnRead.Text = "Read";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            //
            this.btnWrite.Enabled = false;
            this.btnWrite.Location = new System.Drawing.Point(240, 77);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(60, 23);
            this.btnWrite.Text = "Write";
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);

            // btnEnableWrite
            //
            this.btnEnableWrite.Location = new System.Drawing.Point(240, 48);
            this.btnEnableWrite.Name = "btnEnableWrite";
            this.btnEnableWrite.Size = new System.Drawing.Size(60, 23);
            this.btnEnableWrite.Text = "Enable";
            this.btnEnableWrite.Click += new System.EventHandler(this.btnEnableWrite_Click);
            //
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 85);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 12);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 110);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnEnableWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtLogicalStation);
            this.Controls.Add(this.txtIp);
            this.Name = "MainForm";
            this.Text = "PLC Control";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
