﻿using System;
using System.Runtime.InteropServices;
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
using System.Globalization;

namespace UHC_Tracker
{
    public partial class UHCWaypoint : Form, DataSource.DSHandlerPlayerLocation, DataSource.DSHandlerConnectionStatusChanged, DataSource.DSHandlerWorldPath
    {
        private EditTime dlgEditTime;
        KeyboardHook hook = null;
        bool isClosing = false;
        private string autoSaveFile = null;

        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }


        private void resetAutoSaveFile()
        {
            tsslSaveState.Text = "Data not saved! AutoSave not enabled!";
            autoSaveFile = null;
            saveTimer.Stop();
        }

        private void pauseAutoSave()
        {
            autoSaveFile = null;
            saveTimer.Stop();
        }

        private void startAutoSave(string path)
        {
            if (chkAutoSave.Checked)
            {
                autoSaveFile = path;
                saveTimer.Start();
                tsslSaveState.Text = "Data autosave to file: " + autoSaveFile + " enabled";
            }
        }

        public UHCWaypoint()
        {
            InitializeComponent();

            dlgEditTime = new EditTime();
            dlgEditTime.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");

            resetAutoSaveFile();

            if (!IsRunningOnMono())
            {
                hook = new KeyboardHook();
                hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
                hook.RegisterHotKey(UHC_Tracker.ModifierKeys.Control, Keys.F12);
            }

            DataSource.DS.InitializeConnectionStatusChanged(this);
            DataSource.DS.InitializePlayerLocation(this);
            DataSource.DS.InitializeWorldPath(this);
            DataSource.DS.Connect(txtMCIp.Text, 50191);

            cmbEpisode.SelectedIndex = 0;
            cmbSeason.SelectedIndex = cmbSeason.Items.Count - 1;

            updateTimer.Start();

            tsslConnection.Alignment = ToolStripItemAlignment.Right;
            tsslConnection.Spring = true;
            tsslConnection.Anchor = AnchorStyles.Right;
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            btnWaypoint.PerformClick();
        }

        public void ConnectionStatusChanged(DataSource.ConnectionStatus newStatus)
        {
            if (!this.isClosing)
            {
                switch (newStatus)
                {
                    case DataSource.ConnectionStatus.Idle:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Not Connected");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
                        break;
                    case DataSource.ConnectionStatus.Connecting:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connecting");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, false);
                        break;
                    case DataSource.ConnectionStatus.Connected:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connected");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, false);
                        break;
                    case DataSource.ConnectionStatus.Refused:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Connection\nrefused");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
                        break;
                    case DataSource.ConnectionStatus.Disconnected:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Disconnected");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Text, "Connect");
                        btnConnect.SetPropertyThreadSafe(() => btnConnect.Enabled, true);
                        this.Invoke(new Action(() => { MessageBox.Show(this, "Disconnected!", "Disconnected", MessageBoxButtons.OK); }));
                        break;
                    default:
                        lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, "Unknown Connection Status");
                        break;
                }
            }
        }

        public void PlayerLocationReceived(List<DataSource.Entity> players, DataSource.Entity userPlayer)
        {
            if (userPlayer != null && updateTimer.Enabled)
            {
                txtX.SetPropertyThreadSafe(() => txtX.Text, userPlayer.ix.ToString());
                txtY.SetPropertyThreadSafe(() => txtY.Text, userPlayer.iy.ToString());
                txtZ.SetPropertyThreadSafe(() => txtZ.Text, userPlayer.iz.ToString());
                txtRotation.SetPropertyThreadSafe(() => txtRotation.Text, ((int)Math.Floor((userPlayer.rotation * 180 / Math.PI) + 90)).ToString());
            }
        }

        public void WorldPathReceived(string worldPath, int dimension, string worldName, int spawnX, int spawnY, int spawnZ)
        {
            txtDim.SetPropertyThreadSafe(() => txtDim.Text, dimension.ToString());
        }

        delegate void CallClickHandlerCallback(Button btn);
        private void CallClickHandler(Button btn)
        {
            if (btn.InvokeRequired) {
                CallClickHandlerCallback cb = new CallClickHandlerCallback(CallClickHandler);
                this.Invoke(cb, new object[] { btn });
            } else {
                btn.PerformClick();
            }
        }

        delegate void CallAddPointCallback(Button btn, string message);
        private void CallAddPoint(Button btn, string message = "")
        {
            if (btn.InvokeRequired)
            {
                CallAddPointCallback cb = new CallAddPointCallback(CallAddPoint);
                this.Invoke(cb, new object[] { btn, message });
            }
            else
            {
                addPoint(btn, new EventArgs(), message);
            }
        }

        public string CommandReceived(byte command, string playerName, string desc = "")
        {
            if (playerName.ToLower() == txtMCName.Text.ToLower())
            {
                string response = "Got Point (" + txtX.Text + "," + txtY.Text + "," + txtZ.Text + ") Type: ";

                switch (command)
                {
                    case DataSource.DS.SAVE_WAYPOINT:
                        CallClickHandler(btnWaypoint);
                        response += "Waypoint";
                        break;
                    case DataSource.DS.SAVE_HOUSE_POINT:
                        CallAddPoint(btnHouse, desc);
                        response += "'House' with description: " + desc;
                        break;
                    case DataSource.DS.SAVE_PORTAL_POINT:
                        CallAddPoint(btnPortal, desc);
                        response += "'Portal' with description: " + desc;
                        break;
                    case DataSource.DS.SAVE_FIGHT_POINT:
                        CallAddPoint(btnFight, desc);
                        response += "'Fight' with description: " + desc;
                        break;
                    case DataSource.DS.SAVE_DEATH_POINT:
                        CallAddPoint(btnDeath, desc);
                        response += "'Death' with description: " + desc;
                        break;
                    case DataSource.DS.SAVE_SPOT_POINT:
                        CallAddPoint(btnSpotted, desc);
                        response += "'Spot' with description: " + desc;
                        break;
                    case DataSource.DS.SAVE_HEAR_POINT:
                        CallAddPoint(btnHear, desc);
                        response += "'Hear' with description: " + desc;
                        break;
                    default:
                        response = "";
                        break;
                }

                return response;
            } else {
                return null;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateTimer.Stop();
            isClosing = true;
            DataSource.DS.ShutDown();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DataSource.DS.isConnected && updateTimer.Enabled)
            {
                DataSource.DS.GetPlayerLocation(txtMCName.Text);
                DataSource.DS.GetWorldPath(txtMCName.Text);
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
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;

                // Merker für Bedingungen
                bool bOk = true;

                // wenn das Eingabezeichen ein kulturunabhängiges Dezimaltrennzeichen ist
                if (e.KeyChar.ToString() == NumberFormatInfo.InvariantInfo.NumberDecimalSeparator)
                {
                    // überprüfen, ob in der Textbox schon ein Dezimaltrennzeichen steht,
                    // um Doppelungen zu vermeiden
                    bOk = (tb.Text.IndexOf(NumberFormatInfo.InvariantInfo.NumberDecimalSeparator) < 0);
                }

                // wenn das Eingabezeichen ein führendes Minus ist
                if (e.KeyChar.ToString() == NumberFormatInfo.InvariantInfo.NegativeSign)
                {
                    // überprüfen, ob in der Textbox schon ein führendes Minus steht,
                    // um Doppelungen zu vermeiden
                    bOk = ((tb.SelectionStart == 0) && (tb.Text.IndexOf(NumberFormatInfo.InvariantInfo.NegativeSign) < 0));
                }

                if (bOk)
                {
                    // Liste mit erlaubten Zeichen, in diesem Fall
                    // führendes Minus, Zahlen, Dezimaltrennzeichen, Backspace
                    string sChars = NumberFormatInfo.InvariantInfo.NegativeSign + "0123456789" + NumberFormatInfo.InvariantInfo.NumberDecimalSeparator + "\b";

                    // testen, ob das Zeichen erlaubt ist (in der Liste vorhanden ist)
                    e.Handled = (sChars.IndexOf(e.KeyChar) < 0);
                }
                else
                {
                    // sonst Eingabezeichen verwerfen
                    e.Handled = true;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (updateTimer.Enabled)
            {
                updateTimer.Stop();
                btnUpdate.Text = "Start Updating";
            }
            else
            {
                updateTimer.Start();
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
            addPoint(sender, e, "");
        }

        private void addPoint(object sender, EventArgs e, string message = null)
        {
            string id = "0";
            string seq = txtSeq.Text;
            bool topPart = true;
            string desc = "";

            if (sender.Equals(btnWaypoint))
            {
                topPart = false;
            }
            else
            {
                if (message == null) {
                    AddDesc descFrm = new AddDesc();
                    descFrm.ShowDialog();
                    desc = descFrm.desc;
                } else {
                    desc = message;
                }

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
                    id = "6";
                }
                else if (sender.Equals(btnHear))
                {
                    id = "7";
                }
                else if (sender.Equals(btnDeath))
                {
                    id = "1";
                }
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
            data.Add("dim", txtDim.Text);
            data.Add("msg", cmbPlayer.Text);
            data.Add("desc", desc);

            addRow(createRow(data), topPart);

            lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
        }

        private readonly Object saveFileLock = new Object();
        private void doSave(System.IO.StreamWriter sw)
        {
            lock (saveFileLock)
            {
                JsonTextWriter json = new JsonTextWriter(sw);
                json.Formatting = Formatting.Indented;
                json.WriteStartArray();
                JsonSerializer jsonSer = new JsonSerializer();
                jsonSer.Formatting = Formatting.None;
                jsonSer.NullValueHandling = NullValueHandling.Ignore;

                lock (dgvPoints)
                {
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
                }
                json.WriteEndArray();
                json.Close();
            }
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

            pauseAutoSave();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.OpenFile());
            doSave(sw);
            sw.Close();
            startAutoSave(sfd.FileName);
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
            sb.Append("\"time\":" + row.Cells[5].Value + ", ");
            sb.Append("\"dim\":" + row.Cells[6].Value + ", ");
            sb.Append("\"msg\":\""  + row.Cells[7].Value + "\" ");

            if (((string)row.Cells[8].Value) != "" && row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                sb.Append(", \"desc\":\"" + row.Cells[8].Value + "\" ");
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
                row.Cells[6].Value == null || ((string)row.Cells[6].Value) == "" ||
                row.Cells[7].Value == null || ((string)row.Cells[7].Value) == "")
            {
                return null;
            }

            if (row.Index < topPartIndex)
            {
                rd.seq = row.Index;
            }
            else
            {
                rd.seq = row.Index - topPartIndex;
            }


            // rd.seq = int.Parse(row.Cells[0].Value.ToString());
            rd.x = int.Parse(row.Cells[1].Value.ToString());
            rd.y = int.Parse(row.Cells[2].Value.ToString());
            rd.z = int.Parse(row.Cells[3].Value.ToString());
            rd.id = int.Parse(row.Cells[4].Value.ToString());
            rd.time = double.Parse(row.Cells[5].Value.ToString());
            rd.dim = int.Parse(row.Cells[6].Value.ToString());
            rd.msg = row.Cells[7].Value.ToString();

            if (row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                if (row.Cells[8].Value == null)
                {
                    row.Cells[8].Value = "";
                }
                rd.desc = row.Cells[8].Value.ToString();
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
                row.Cells[6].Value == null || ((string)row.Cells[6].Value) == "" ||
                row.Cells[7].Value == null || ((string)row.Cells[7].Value) == "")
            {
                return null;
            }

            data.Add("seq",  row.Cells[0].Value.ToString());
            data.Add("x",    row.Cells[1].Value.ToString());
            data.Add("y",    row.Cells[2].Value.ToString());
            data.Add("z",    row.Cells[3].Value.ToString());
            data.Add("id",   row.Cells[4].Value.ToString());
            data.Add("time", row.Cells[5].Value.ToString());
            data.Add("dim", row.Cells[6].Value.ToString());
            data.Add("msg",  row.Cells[7].Value.ToString());

            if (((string)row.Cells[8].Value) != "" && row.Cells[4].Value != null && int.Parse(row.Cells[4].Value.ToString()) != 0)
            {
                data.Add("desc", row.Cells[8].Value.ToString());
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
            try
            {
                cell.Value = data["dim"];
            }
            catch (Exception)
            {
                cell.Value = "0";
            }
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
                lock (dgvPoints)
                {
                    dgvPoints.Rows.Insert(topPartIndex, row);
                }
                topPartIndex++;
            }
            else
            {
                lock (dgvPoints)
                {
                    dgvPoints.Rows.Add(row);
                    dgvPoints.FirstDisplayedScrollingRowIndex = dgvPoints.RowCount - 1;
                }
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
            lock (dgvPoints)
            {
                if (dgvPoints.Rows.Count > 1)
                {
                    DialogResult result = MessageBox.Show("Do you really want to discard all entrys?", "Are you sure?", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        topPartIndex = 0;
                        dgvPoints.Rows.Clear();
                        lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
                        resetAutoSaveFile();
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!DataSource.DS.isConnected)
            {
                DataSource.DS.Connect(txtMCIp.Text, 50191);
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
                    lock (dgvPoints)
                    {
                        dgvPoints.Rows.Clear();
                    }
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

            resetAutoSaveFile();

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

            resetAutoSaveFile();

            try
            {
                WebClient dataClient = new WebClient();
                System.IO.Stream dataStrm = dataClient.OpenRead("http://46.38.243.240/uhc" + cmbSeason.Text + "/data/" + cmbPlayer.Text + ".json");

                topPartIndex = 0;
                lock (dgvPoints)
                {
                    dgvPoints.Rows.Clear();
                }
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
            lock (dgvPoints)
            {
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
            }
            json.WriteEndArray();

            string timeStr = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            sw.Flush();
            data.Position = 0;

            DropNetClient dropClient = new DropNetClient("q7sr7mpetc5kbei", "50jcxh2o3ofhkpg", "iguo6ia89lojpei", "bz82sp8r218pi3q");
            dropClient.UseSandbox = true;
            DropNet.Models.MetaData mdata =  dropClient.UploadFile("/", cmbPlayer.Text + ".json." + timeStr, data);

            json.Close();
            if (mdata != null)
            {
                MessageBox.Show("Data successfully uploaded");
            }
            else
            {
                MessageBox.Show("Uploading data failed!");
            }
        }

        private void tsmidEditTime_Click(object sender, EventArgs e)
        {
            if (dlgEditTime.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lock (dgvPoints)
                {
                    foreach (DataGridViewRow row in dgvPoints.SelectedRows)
                    {
                        double time = double.Parse(row.Cells[5].Value.ToString());
                        time += dlgEditTime.time;
                        row.Cells[5].Value = Math.Round(time, 2).ToString();
                    }
                }
            }
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvPoints.SelectedRows.Count > 0)
            {
                lock (dgvPoints)
                {
                    foreach (DataGridViewRow row in dgvPoints.SelectedRows)
                    {
                        if (dgvPoints.RowCount - 1 > 0)
                        {
                            dgvPoints.Rows.Remove(row);
                        }
                    }
                }

                findNewSeq();
                lblDataSets.Text = (dgvPoints.RowCount - 1).ToString();
            }
        }

        private void btnLoadPlayerlist_Click(object sender, EventArgs e)
        {
            cmbPlayer.Items.Clear();
            try
            {
                WebClient dataClient = new WebClient();
                System.IO.Stream dataStrm = dataClient.OpenRead("http://46.38.243.240/uhc/players.json");
                System.IO.StreamReader sr = new System.IO.StreamReader(dataStrm);
                JsonTextReader json = new JsonTextReader(sr);
                JsonSerializer jsonSer = new JsonSerializer();
                List<IDictionary<string, string>> data = jsonSer.Deserialize<List<IDictionary<string, string>>>(json);
                json.Close();
                sr.Close();

                foreach (IDictionary<string, string> d in data)
                {
                    if (d.ContainsKey("name")) {
                        string name = d["name"];
                        cmbPlayer.Items.Add(name);
                    }
                }
                cmbPlayer.SelectedIndex = 0;

                foreach (IDictionary<string, string> d in data)
                {
                    if (d.ContainsKey("season"))
                    {
                        string season = d["season"];
                        cmbSeason.Items.Add(season);
                    }
                }
                cmbSeason.SelectedIndex = cmbSeason.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't download data! ex:" + ex.Message);
                return;
            }
        }

        private void cmdUseStart_Click(object sender, EventArgs e)
        {
            double time = 0;
            time = cmbEpisode.SelectedIndex * 20;
            time -= double.Parse(txtStartMins.Text) + (double.Parse(txtStartSecs.Text) / 60);

            if (time < 0)
            {
                chkOffNeg.CheckState = CheckState.Unchecked;
                time = -time;
            }
            else
            {
                chkOffNeg.CheckState = CheckState.Checked;
            }

            double mins = Math.Floor(time);
            txtOffMins.Text = ((int)mins).ToString();
            txtOffSecs.Text = (Math.Round((time - mins) * 60, 0)).ToString("0#");
        }

        private void cmdUseEnd_Click(object sender, EventArgs e)
        {
            double time = 0;
            time = (cmbEpisode.SelectedIndex + 1) * 20;
            time -= double.Parse(txtEndMins.Text) + (double.Parse(txtEndSecs.Text) / 60);

            if (time < 0)
            {
                chkOffNeg.CheckState = CheckState.Unchecked;
                time = -time;
            }
            else
            {
                chkOffNeg.CheckState = CheckState.Checked;
            }

            double mins = Math.Floor(time);
            txtOffMins.Text = ((int)mins).ToString();
            txtOffSecs.Text = (Math.Round((time - mins) * 60, 0)).ToString("0#");
        }

        private void teleportToHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPoints.SelectedRows.Count == 1)
            {
                double x = double.Parse(dgvPoints.SelectedRows[0].Cells[1].Value.ToString());
                double y = double.Parse(dgvPoints.SelectedRows[0].Cells[2].Value.ToString());
                double z = double.Parse(dgvPoints.SelectedRows[0].Cells[3].Value.ToString());

                DataSource.DS.SetPlayerLocation(txtMCName.Text, (int) -x, (int) y, (int) z);
            }
        }

        private void saveTimer_Tick(object sender, EventArgs e)
        {
            if (chkAutoSave.Checked && autoSaveFile != null)
            {
                tsslSaveState.Text = "Autosaving data to " + autoSaveFile;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(autoSaveFile);
                doSave(sw);
                sw.Close();
                tsslSaveState.Text = System.DateTime.Now.ToLongTimeString() + ": Autosaving successful";
            }
        }

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            tsslConnection.Text = lblStatus.Text;
        }

        private void label23_DoubleClick(object sender, EventArgs e)
        {
            if (!txtMCIp.Enabled) {
                DataSource.DS.doReconnect = false;
                DataSource.DS.Disconnect();
            } else {
                DataSource.DS.doReconnect = true;
            }
            txtMCIp.Enabled = !txtMCIp.Enabled;
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            ((TextBox) sender).SelectAll();
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
        public int dim { get; set; }
        public string msg { get; set; }
        public string desc { get; set; }
    }

    public static class test
    {
        private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);

        public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value)
        {
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
        
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

}
