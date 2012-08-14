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
using System.Net;

using Newtonsoft.Json;

using DropNet;
using DropNet.Authenticators;

namespace UHC_Tracker
{
    public partial class UHCWaypoint : Form, DataSource.DSHandlerPlayerLocation, DataSource.DSHandlerConnectionStatusChanged
    {
        private EditTime dlgEditTime;
        public UHCWaypoint()
        {
            InitializeComponent();

            dlgEditTime = new EditTime();
            dlgEditTime.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            test.updateTimer = timer1;

            DataSource.DS.InitializeConnectionStatusChanged(this);
            DataSource.DS.InitializePlayerLocation(this);
            DataSource.DS.Connect("localhost", 50191);

            cmbPlayer.SelectedIndex = 0;
            cmdEpisode.SelectedIndex = 0;
        }

        public void ConnectionStatusChanged(DataSource.ConnectionStatus newStatus)
        {
            switch (newStatus)
            {
                case DataSource.ConnectionStatus.Idle:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Not Connected");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
                    break;
                case DataSource.ConnectionStatus.Connecting:
                    timer1.Enabled = false;
                    timer1.Start();
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connecting");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, false);
                    break;
                case DataSource.ConnectionStatus.Connected:
                    timer1.Enabled = true;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connected");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, false);
                    break;
                case DataSource.ConnectionStatus.Refused:
                    timer1.Enabled = false;
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connection refused");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
                    break;
                case DataSource.ConnectionStatus.Disconnected:
                    timer1.Enabled = false;
                    timer1.Stop();
                    lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Disconnected");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                    btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
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

        public static void txtTime_KeyPress(object sender, KeyPressEventArgs e)
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

            addTime(int.Parse(txtInc.Text));
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
                seq = txtMSeq.Text;
                txtMSeq.Text = (int.Parse(txtMSeq.Text) + 1).ToString();
            }

            double time = int.Parse(txtMins.Text) + (double.Parse(txtSecs.Text) / 60);
            double offset = int.Parse(txtOffMins.Text) + (double.Parse(txtOffSecs.Text) / 60);
            if (chkOffNeg.Checked)
            {
                offset = -offset;
            }
            time -= offset;
            

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("seq", seq);
            data.Add("x", txtX.Text);
            data.Add("y", txtY.Text);
            data.Add("z", txtZ.Text);
            data.Add("id", id.ToString());
            data.Add("time", Math.Round(time, 2).ToString());
            data.Add("msg", cmbPlayer.Text);

            addRow(createRow(data), topPart);

            lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = cmbPlayer.Text + ".json";
            sfd.DefaultExt = ".json";
            DialogResult result = sfd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }


            System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.OpenFile());
            JsonTextWriter json = new JsonTextWriter(sw);
            json.Formatting = Formatting.Indented;
            json.WriteStartArray();
            JsonSerializer jsonSer = new JsonSerializer();
            jsonSer.Formatting = Formatting.None;
            jsonSer.NullValueHandling = NullValueHandling.Ignore;
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                //IDictionary<string, string> d = convertRow(row);
                RowData d = createRowObject(row);
                if (d != null)
                {
                    StringBuilder sb = new StringBuilder();
                    System.IO.StringWriter swL = new System.IO.StringWriter(sb);
                    jsonSer.Serialize(swL, d);

                    json.Formatting = Formatting.Indented;
                    json.WriteRawValue(sb.ToString());
                }
            }
            json.WriteEndArray();
            json.Close();
            sw.Close();


            //System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.OpenFile());

            //foreach (DataGridViewRow row in dgvPoints.Rows) {
            //    sw.WriteLine(printRow(row));
            //}

            //sw.Close();
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

        private RowData createRowObject(DataGridViewRow row)
        {
            RowData rd = new RowData();

            if (row.Cells[0].Value == null || ((string)row.Cells[0].Value) == "" ||
                row.Cells[1].Value == null || ((string)row.Cells[1].Value) == "" ||
                row.Cells[2].Value == null || ((string)row.Cells[2].Value) == "" ||
                row.Cells[3].Value == null || ((string)row.Cells[3].Value) == "" ||
                row.Cells[4].Value == null || ((string)row.Cells[4].Value) == "" ||
                row.Cells[5].Value == null || ((string)row.Cells[5].Value) == "" ||
                row.Cells[6].Value == null || ((string)row.Cells[6].Value) == "")
            {
                return null;
            }

            rd.seq = int.Parse(row.Cells[0].Value.ToString());
            rd.x = int.Parse(row.Cells[1].Value.ToString());
            rd.y = int.Parse(row.Cells[2].Value.ToString());
            rd.z = int.Parse(row.Cells[3].Value.ToString());
            rd.id = int.Parse(row.Cells[4].Value.ToString());
            rd.time = double.Parse(row.Cells[5].Value.ToString());
            rd.msg = row.Cells[6].Value.ToString();

            if (row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                if (row.Cells[7].Value == null)
                {
                    row.Cells[7].Value = "";
                }
                rd.desc = row.Cells[7].Value.ToString();
            }

            return rd;
        }

        private IDictionary<string, string> convertRow(DataGridViewRow row)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (row.Cells[0].Value == null || ((string)row.Cells[0].Value) == "" ||
                row.Cells[1].Value == null || ((string)row.Cells[1].Value) == "" ||
                row.Cells[2].Value == null || ((string)row.Cells[2].Value) == "" ||
                row.Cells[3].Value == null || ((string)row.Cells[3].Value) == "" ||
                row.Cells[4].Value == null || ((string)row.Cells[4].Value) == "" ||
                row.Cells[5].Value == null || ((string)row.Cells[5].Value) == "" ||
                row.Cells[6].Value == null || ((string)row.Cells[6].Value) == "")
            {
                return null;
            }

            data.Add("seq",  row.Cells[0].Value.ToString());
            data.Add("x",    row.Cells[1].Value.ToString());
            data.Add("y",    row.Cells[2].Value.ToString());
            data.Add("z",    row.Cells[3].Value.ToString());
            data.Add("id",   row.Cells[4].Value.ToString());
            data.Add("time", row.Cells[5].Value.ToString());
            data.Add("msg",  row.Cells[6].Value.ToString());

            if (((string)row.Cells[7].Value) != "" && row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                data.Add("desc", row.Cells[7].Value.ToString());
            }
            

            return data;
        }

        private DataGridViewRow createRow(IDictionary<string, string> data)
        {
            DataGridViewRow row = new DataGridViewRow();

            DataGridViewCell cell;
            cell = new DataGridViewTextBoxCell();
            cell.Value = data["seq"];
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = data["x"];
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = data["y"];
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = data["z"];
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = data["id"];
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = data["time"];
            row.Cells.Add(cell);
            cell = new DataGridViewTextBoxCell();
            cell.Value = data["msg"];
            row.Cells.Add(cell);

            if (data.ContainsKey("desc"))
            {
                cell = new DataGridViewTextBoxCell();
                cell.Value = data["desc"];
                row.Cells.Add(cell);
            }

            return row;
        }

        private int topPartIndex = 0;
        private void addRow(DataGridViewRow row, bool topPart)
        {
            if (topPart)
            {
                if (topPartIndex > dgvPoints.Rows.Count)
                {
                    topPartIndex = dgvPoints.Rows.Count + 1;
                }
                dgvPoints.Rows.Insert(topPartIndex, row);
                topPartIndex++;
            }
            else
            {
                dgvPoints.Rows.Add(row);
                dgvPoints.FirstDisplayedScrollingRowIndex = dgvPoints.RowCount - 1;
            }
        }

        private void findNewSeq()
        {
            topPartIndex = 0;
            int seq = -1;
            int mseq = -1;
            double time = 0;
            foreach (DataGridViewRow row in dgvPoints.Rows) {
                if (row.Cells[0].Value != null && row.Cells[4].Value != null && row.Cells[5].Value != null)
                {
                    double curTime = double.Parse(row.Cells[5].Value.ToString());
                    if (time < curTime) time = curTime;

                    if (int.Parse(row.Cells[4].Value.ToString()) == 0)
                    {
                        int curSeq = int.Parse(row.Cells[0].Value.ToString());
                        if (seq < curSeq) seq = curSeq;
                    }
                    else
                    {
                        int curSeq = int.Parse(row.Cells[0].Value.ToString());
                        if (mseq < curSeq) mseq = curSeq;
                        topPartIndex++;
                    }
                }
            }
            seq++; mseq++;
            txtSeq.Text = seq.ToString();
            txtMSeq.Text = mseq.ToString();

            double offset = double.Parse(txtOffMins.Text) + (double.Parse(txtOffSecs.Text) / 60);
            offset = (chkOffNeg.Checked ? -offset : offset);
            time += offset;

            double mins = Math.Floor(time);
            txtMins.Text = ((int) mins).ToString();
            txtSecs.Text = (Math.Round((time - mins) * 60, 0)).ToString("0#");
            addTime(int.Parse(txtInc.Text));
        }

        private void addTime(int secs)
        {
            double time = Double.Parse(txtMins.Text) + (Double.Parse(txtSecs.Text) / 60);
            time += ((double) secs) / 60;
            double mins = Math.Floor(time);
            txtMins.Text = mins.ToString();
            txtSecs.Text = (Math.Round((time - mins) * 60, 0)).ToString("0#");
        }

        private void btnIncrement_Click(object sender, EventArgs e)
        {
            addTime(int.Parse(txtInc.Text));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgvPoints.Rows.Count > 1)
            {
                DialogResult result = MessageBox.Show("Do you really want to discard all entrys?", "Are you sure?", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    topPartIndex = 0;
                    dgvPoints.Rows.Clear();
                    lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!DataSource.DS.isConnected)
            {
                DataSource.DS.Connect("localhost", 50191);
            }
            
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (dgvPoints.Rows.Count > 1)
            {
                DialogResult result = MessageBox.Show("Do you really want to discard all entrys?", "Are you sure?", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    topPartIndex = 0;
                    dgvPoints.Rows.Clear();
                    lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
                }
                else
                {
                    return;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = cmbPlayer.Text + ".json";
            ofd.DefaultExt = ".json";
            DialogResult ofdResult = ofd.ShowDialog();

            if (ofdResult == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            System.IO.StreamReader sr = new System.IO.StreamReader(ofd.OpenFile());
            JsonTextReader json = new JsonTextReader(sr);
            JsonSerializer jsonSer = new JsonSerializer();
            List<IDictionary<string, string>> data = jsonSer.Deserialize<List<IDictionary<string, string>>>(json);
            json.Close();
            sr.Close();

            foreach (IDictionary<string, string> d in data)
            {
                DataGridViewRow row = createRow(d);
                int id = int.Parse(d["id"]);
                if (id != 0)
                {
                    addRow(row, true);
                }
                else
                {
                    addRow(row, false);
                }
            }

            findNewSeq();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (dgvPoints.Rows.Count > 1)
            {
                DialogResult result = MessageBox.Show("Do you really want to discard all entrys?", "Are you sure?", MessageBoxButtons.YesNo);
                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                WebClient dataClient = new WebClient();
                System.IO.Stream dataStrm = dataClient.OpenRead("http://88.198.183.184/uhc7/data/" + cmbPlayer.Text + ".json");

                topPartIndex = 0;
                dgvPoints.Rows.Clear();
                lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();

                System.IO.StreamReader sr = new System.IO.StreamReader(dataStrm);
                JsonTextReader json = new JsonTextReader(sr);
                JsonSerializer jsonSer = new JsonSerializer();
                List<IDictionary<string, string>> data = jsonSer.Deserialize<List<IDictionary<string, string>>>(json);
                json.Close();
                sr.Close();

                foreach (IDictionary<string, string> d in data)
                {
                    DataGridViewRow row = createRow(d);
                    int id = int.Parse(d["id"]);
                    if (id != 0)
                    {
                        addRow(row, true);
                    }
                    else
                    {
                        addRow(row, false);
                    }
                }

                findNewSeq();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't download data! ex:" + ex.Message);
                return;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            System.IO.Stream data = new System.IO.MemoryStream(64 * 1024);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(data);
            JsonTextWriter json = new JsonTextWriter(sw);
            json.Formatting = Formatting.Indented;
            json.WriteStartArray();
            JsonSerializer jsonSer = new JsonSerializer();
            jsonSer.Formatting = Formatting.None;
            jsonSer.NullValueHandling = NullValueHandling.Ignore;
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                //IDictionary<string, string> d = convertRow(row);
                RowData d = createRowObject(row);
                if (d != null)
                {
                    StringBuilder sb = new StringBuilder();
                    System.IO.StringWriter swL = new System.IO.StringWriter(sb);
                    jsonSer.Serialize(swL, d);

                    json.Formatting = Formatting.Indented;
                    json.WriteRawValue(sb.ToString());
                }
            }
            json.WriteEndArray();

            string timeStr = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            sw.Flush();
            data.Position = 0;

            DropNetClient dropClient = new DropNetClient("q7sr7mpetc5kbei", "50jcxh2o3ofhkpg", "iguo6ia89lojpei", "bz82sp8r218pi3q");
            dropClient.UseSandbox = true;
            DropNet.Models.MetaData mdata =  dropClient.UploadFile("/", cmbPlayer.Text + ".json." + timeStr, data);

            json.Close();
        }

        private void tsmidEditTime_Click(object sender, EventArgs e)
        {
            if (dlgEditTime.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (DataGridViewRow row in dgvPoints.SelectedRows)
                {
                    double time = double.Parse(row.Cells[5].Value.ToString());
                    time += dlgEditTime.time;
                    row.Cells[5].Value = Math.Round(time, 2).ToString();
                }
            }
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvPoints.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvPoints.SelectedRows)
                {
                    if (dgvPoints.RowCount - 1 > 0)
                    {
                        dgvPoints.Rows.Remove(row);
                    }
                }

                findNewSeq();
                lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
            }
        }
    }

    public class RowData
    {
        public int seq { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z  { get; set; }
        public int id { get; set; }
        public double time { get; set; }
        public string msg { get; set; }
        public string desc { get; set; }
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
