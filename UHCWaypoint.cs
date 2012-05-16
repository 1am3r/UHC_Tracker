using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Reflection;

namespace UHC_Tracker
{
    public partial class UHCWaypoint : Form, DataSource.DSHandlerPlayerLocation, DataSource.DSHandlerConnectionStatusChanged
    {
        public UHCWaypoint()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            test.updateTimer = timer1;

            DataSource.DS.InitializeConnectionStatusChanged(this);
            DataSource.DS.InitializePlayerLocation(this);
            DataSource.DS.Connect("localhost", 50191);

            cmbPlayer.SelectedIndex = 0;
        }

        public void ConnectionStatusChanged(DataSource.ConnectionStatus newStatus)
        {
            switch (newStatus)
            {
                case DataSource.ConnectionStatus.Idle:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Not Connected");
                    break;
                case DataSource.ConnectionStatus.Connecting:
                    timer1.Enabled = false;
                    timer1.Start();
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connecting");
                    break;
                case DataSource.ConnectionStatus.Connected:
                    timer1.Enabled = true;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connected");
                    break;
                case DataSource.ConnectionStatus.Refused:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connection refused");
                    break;
                case DataSource.ConnectionStatus.Disconnected:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Disconnected.  Attempting reconnect...");
                    break;
                default:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Unknown Connection Status");
                    break;
            }
        }

        public void PlayerLocationReceived(List<DataSource.Entity> players, DataSource.Entity userPlayer)
        {
            if (userPlayer != null && timer1.Enabled)
            {
                txtX.SetPropertyThreadSafe(() => txtX.Text, userPlayer.ix.ToString());
                txtY.SetPropertyThreadSafe(() => txtY.Text, userPlayer.iy.ToString());
                txtZ.SetPropertyThreadSafe(() => txtZ.Text, userPlayer.iz.ToString());
                txtRotation.SetPropertyThreadSafe(() => txtRotation.Text, ((int)Math.Floor((userPlayer.rotation * 180 / Math.PI) + 90)).ToString());
            }
        }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            DataSource.DS.ShutDown();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DataSource.DS.isConnected && timer1.Enabled)
            {
                DataSource.DS.GetPlayerLocation();
            }
        }

        private void txtTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtSeq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCoord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // only allow '-' at beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
            {
                e.Handled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Stop();
                btnUpdate.Text = "Start Updating";
            }
            else
            {
                timer1.Enabled = true;
                timer1.Start();
                btnUpdate.Text = "Stop Updating";
            }
        }

        private void btnWaypoint_Click(object sender, EventArgs e)
        {
            addPoint(sender, e);

            txtTime.Text = (Math.Round(Double.Parse(txtTime.Text) + (Double.Parse(txtInc.Text) / 60), 2)).ToString();
            txtSeq.Text = (int.Parse(txtSeq.Text) + 1).ToString();
        }

        private void dgvPoints_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if ((e.Exception) is FormatException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[e.RowIndex].ErrorText = "Error";
                view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error";

                e.ThrowException = false;
            }
        }

        private int topPartIndex = 0;
        private void addPoint(object sender, EventArgs e)
        {
            string id = "0";
            string seq = txtSeq.Text;
            bool topPart = true;

            if (sender.Equals(btnHouse))
            {
                id = "2";
            } 
            else if (sender.Equals(btnPortal))
            {
                id = "3";
            } 
            else if (sender.Equals(btnFight))
            {
                id = "5";
            } 
            else if (sender.Equals(btnSpotted))
            {
                id = "5";
            }
            else if (sender.Equals(btnDeath))
            {
                id = "1";
            }
            
            if (sender.Equals(btnWaypoint))
            {
                topPart = false;
            }
            else
            {
                topPartIndex++;
                seq = txtMSeq.Text;
                txtMSeq.Text = (int.Parse(txtMSeq.Text) + 1).ToString();
            }

            DataGridViewRow row = new DataGridViewRow();
            DataGridViewCell cell;
            cell = new DataGridViewTextBoxCell();
            cell.Value = seq;
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = txtX.Text;
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = txtY.Text;
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = txtZ.Text;
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = id;
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = txtTime.Text;
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = cmbPlayer.Text;
            row.Cells.Add(cell);

            if (topPart)
            {
                dgvPoints.Rows.Insert(topPartIndex - 1, row);
            }
            else
            {
                dgvPoints.Rows.Add(row);
                dgvPoints.FirstDisplayedScrollingRowIndex = dgvPoints.RowCount - 1;
            }

            lblDataSets.Text = dgvPoints.RowCount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "markers.json";
            sfd.DefaultExt = ".json";
            DialogResult result = sfd.ShowDialog();

            System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.OpenFile());

            foreach (DataGridViewRow row in dgvPoints.Rows) {
                sw.WriteLine(printRow(row));
            }

            sw.Close();
        }

        private string printRow(DataGridViewRow row)
        {
            if (row.Cells[0].Value == null || ((string) row.Cells[0].Value) == "" ||
                row.Cells[1].Value == null || ((string) row.Cells[1].Value) == "" ||
                row.Cells[2].Value == null || ((string) row.Cells[2].Value) == "" || 
                row.Cells[3].Value == null || ((string) row.Cells[3].Value) == "" ||
                row.Cells[4].Value == null || ((string) row.Cells[4].Value) == "" ||
                row.Cells[5].Value == null || ((string) row.Cells[5].Value) == "" ||
                row.Cells[6].Value == null || ((string) row.Cells[6].Value) == "")
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(150);
            sb.Append('{');

            sb.Append("\"seq\":"    + row.Cells[0].Value + ", ");
            sb.Append("\"x\":"      + row.Cells[1].Value + ", ");
            sb.Append("\"y\":"      + row.Cells[2].Value + ", ");
            sb.Append("\"z\":"      + row.Cells[3].Value + ", ");
            sb.Append("\"id\":"     + row.Cells[4].Value + ", ");
            sb.Append("\"time\":"   + row.Cells[5].Value + ", ");
            sb.Append("\"msg\":\""  + row.Cells[6].Value + "\" ");

            if (((string)row.Cells[7].Value) != "" && row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                sb.Append(", \"desc\":\"" + row.Cells[7].Value + "\" ");
            }

            sb.Append("},");
            return sb.ToString();
        }

        private void cmsPoints_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tsmiDeleteRow")
            {
                if (dgvPoints.SelectedCells.Count > 0)
                {
                    int index = dgvPoints.SelectedCells[0].RowIndex;

                    if (dgvPoints.RowCount - 1 > index)
                    {
                        dgvPoints.Rows.RemoveAt(index);
                        findNewSeq();
                    }
                }
            }
        }

        private void findNewSeq()
        {
            int seq = -1;
            int mseq = -1;
            foreach (DataGridViewRow row in dgvPoints.Rows) {
                if (row.Cells[4].Value != null)
                {

                    if (int.Parse(row.Cells[4].Value.ToString()) == 0)
                    {
                        int curSeq = int.Parse(row.Cells[0].Value.ToString());
                        if (seq < curSeq) seq = curSeq;
                    }
                    else
                    {
                        int curSeq = int.Parse(row.Cells[0].Value.ToString());
                        if (mseq < curSeq) mseq = curSeq;
                    }
                }
            }
            seq++; mseq++;
            txtSeq.Text = seq.ToString();
            txtMSeq.Text = mseq.ToString();
        }
    }

    public static class test
    {
        private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);
        public static Timer updateTimer;

        public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value)
        {
            if (updateTimer != null && updateTimer.Enabled == false)
            {
                return;
            }

            var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;

            if (propertyInfo == null ||
                !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
                @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null)
            {
                throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
            }

            if (@this.InvokeRequired)
            {
                @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetPropertyThreadSafe), new object[] { @this, property, value });
            }
            else
            {
                @this.GetType().InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, @this, new object[] { value });
            }
        }
    }
}
