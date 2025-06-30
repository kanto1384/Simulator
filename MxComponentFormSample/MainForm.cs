using System;
using System.Windows.Forms;
using ACTUtlTypeLib;

namespace MxComponentFormSample
{
    public partial class MainForm : Form
    {
        private ActUtlType _plc = new ActUtlType();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_plc != null)
            {
                _plc.Close();
            }

            _plc = new ActUtlType();
            _plc.ActLogicalStationNumber = int.Parse(txtLogicalStation.Text);
            _plc.ActHostAddress = txtIp.Text;
            int result = _plc.Open();
            lblStatus.Text = $"Connect result: {result}";
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (_plc == null) return;
            int value;
            int result = _plc.GetDevice(txtDevice.Text, out value);
            if (result == 0)
            {
                txtValue.Text = value.ToString();
            }
            lblStatus.Text = $"Read result: {result}";
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (_plc == null) return;
            int value = int.Parse(txtValue.Text);
            int result = _plc.SetDevice(txtDevice.Text, value);
            lblStatus.Text = $"Write result: {result}";
        }

        private void btnEnableWrite_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Write 기능을 활성화하면 PLC 데이터가 변경될 수 있습니다. 계속하시겠습니까?",
                "경고",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                btnWrite.Enabled = true;
                lblStatus.Text = "Write enabled";
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_plc != null)
            {
                _plc.Close();
            }
            base.OnFormClosing(e);
        }
    }
}
