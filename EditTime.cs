using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UHC_Tracker
{
    public partial class EditTime : Form
    {
        public EditTime()
        {
            InitializeComponent();
        }

        private double m_time;
        public double time { get { return m_time;  } set { m_time = value; } }

        private void btnOk_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(txtMins.Text) + (double.Parse(txtSecs.Text) / 60);
            time = (chkNegTime.Checked ? -temp : temp);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtMins_KeyPress(object sender, KeyPressEventArgs e)
        {
            UHCWaypoint.txtTime_KeyPress(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
