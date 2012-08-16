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
    public partial class AddDesc : Form
    {
        private String mDesc;
        public String desc {
            get { return mDesc;  }
            set { mDesc = value; }
        }
        
        public AddDesc()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            mDesc = txtDesc.Text;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            mDesc = "";
            this.Close();
        }
    }
}
