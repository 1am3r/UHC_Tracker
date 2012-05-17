namespace UHC_Tracker
{
    partial class UHCWaypoint
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
            this.components = new System.ComponentModel.Container();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRotation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDataSets = new System.Windows.Forms.Label();
            this.btnWaypoint = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnHouse = new System.Windows.Forms.Button();
            this.btnSpotted = new System.Windows.Forms.Button();
            this.btnFight = new System.Windows.Forms.Button();
            this.btnPortal = new System.Windows.Forms.Button();
            this.btnDeath = new System.Windows.Forms.Button();
            this.txtSeq = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.cmbPlayer = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dgvPoints = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsPoints = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMSeq = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).BeginInit();
            this.cmsPoints.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 350);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(73, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Disconnected";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(57, 32);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(50, 20);
            this.txtX.TabIndex = 2;
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCoord_KeyPress);
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(57, 58);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(50, 20);
            this.txtY.TabIndex = 3;
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCoord_KeyPress);
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(57, 84);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(50, 20);
            this.txtZ.TabIndex = 4;
            this.txtZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCoord_KeyPress);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Z:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rot:";
            // 
            // txtRotation
            // 
            this.txtRotation.Enabled = false;
            this.txtRotation.Location = new System.Drawing.Point(57, 110);
            this.txtRotation.Name = "txtRotation";
            this.txtRotation.Size = new System.Drawing.Size(50, 20);
            this.txtRotation.TabIndex = 5;
            this.txtRotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Player:";
            // 
            // txtInc
            // 
            this.txtInc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInc.Location = new System.Drawing.Point(600, 5);
            this.txtInc.MaxLength = 3;
            this.txtInc.Name = "txtInc";
            this.txtInc.Size = new System.Drawing.Size(34, 20);
            this.txtInc.TabIndex = 6;
            this.txtInc.Text = "15";
            this.txtInc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(512, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Time increment:";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(184, 32);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(57, 20);
            this.txtTime.TabIndex = 13;
            this.txtTime.Text = "0.0";
            this.txtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTime_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(113, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Time:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "DataSets:";
            // 
            // lblDataSets
            // 
            this.lblDataSets.AutoSize = true;
            this.lblDataSets.Location = new System.Drawing.Point(72, 195);
            this.lblDataSets.Name = "lblDataSets";
            this.lblDataSets.Size = new System.Drawing.Size(13, 13);
            this.lblDataSets.TabIndex = 16;
            this.lblDataSets.Text = "0";
            // 
            // btnWaypoint
            // 
            this.btnWaypoint.Location = new System.Drawing.Point(12, 169);
            this.btnWaypoint.Name = "btnWaypoint";
            this.btnWaypoint.Size = new System.Drawing.Size(95, 23);
            this.btnWaypoint.TabIndex = 17;
            this.btnWaypoint.Text = "Add Waypoint";
            this.btnWaypoint.UseVisualStyleBackColor = true;
            this.btnWaypoint.Click += new System.EventHandler(this.btnWaypoint_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(12, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save Points";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnHouse
            // 
            this.btnHouse.Location = new System.Drawing.Point(133, 169);
            this.btnHouse.Name = "btnHouse";
            this.btnHouse.Size = new System.Drawing.Size(95, 23);
            this.btnHouse.TabIndex = 19;
            this.btnHouse.Text = "Add House";
            this.btnHouse.UseVisualStyleBackColor = true;
            this.btnHouse.Click += new System.EventHandler(this.addPoint);
            // 
            // btnSpotted
            // 
            this.btnSpotted.Location = new System.Drawing.Point(436, 169);
            this.btnSpotted.Name = "btnSpotted";
            this.btnSpotted.Size = new System.Drawing.Size(95, 23);
            this.btnSpotted.TabIndex = 20;
            this.btnSpotted.Text = "Add Spotted";
            this.btnSpotted.UseVisualStyleBackColor = true;
            this.btnSpotted.Click += new System.EventHandler(this.addPoint);
            // 
            // btnFight
            // 
            this.btnFight.Location = new System.Drawing.Point(335, 169);
            this.btnFight.Name = "btnFight";
            this.btnFight.Size = new System.Drawing.Size(95, 23);
            this.btnFight.TabIndex = 21;
            this.btnFight.Text = "Add Fightpoint";
            this.btnFight.UseVisualStyleBackColor = true;
            this.btnFight.Click += new System.EventHandler(this.addPoint);
            // 
            // btnPortal
            // 
            this.btnPortal.Location = new System.Drawing.Point(234, 169);
            this.btnPortal.Name = "btnPortal";
            this.btnPortal.Size = new System.Drawing.Size(95, 23);
            this.btnPortal.TabIndex = 22;
            this.btnPortal.Text = "Add Portal";
            this.btnPortal.UseVisualStyleBackColor = true;
            this.btnPortal.Click += new System.EventHandler(this.addPoint);
            // 
            // btnDeath
            // 
            this.btnDeath.Location = new System.Drawing.Point(537, 169);
            this.btnDeath.Name = "btnDeath";
            this.btnDeath.Size = new System.Drawing.Size(95, 23);
            this.btnDeath.TabIndex = 23;
            this.btnDeath.Text = "Add Deathpoint";
            this.btnDeath.UseVisualStyleBackColor = true;
            this.btnDeath.Click += new System.EventHandler(this.addPoint);
            // 
            // txtSeq
            // 
            this.txtSeq.Location = new System.Drawing.Point(184, 58);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(57, 20);
            this.txtSeq.TabIndex = 25;
            this.txtSeq.Text = "0";
            this.txtSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSeq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeq_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(113, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Path Seq:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdate.Location = new System.Drawing.Point(544, 324);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 23);
            this.btnUpdate.TabIndex = 27;
            this.btnUpdate.Text = "Stop Updating";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // cmbPlayer
            // 
            this.cmbPlayer.FormattingEnabled = true;
            this.cmbPlayer.Items.AddRange(new object[] {
            "AnderZEL",
            "BdoubleO100",
            "Docm77",
            "Etho",
            "Guude",
            "just_defy",
            "kurtmac",
            "mcgamer",
            "Nebris",
            "Pakratt0013",
            "PauseUnpause",
            "VintageBeef",
            "W92Baj",
            "Zisteau"});
            this.cmbPlayer.Location = new System.Drawing.Point(57, 5);
            this.cmbPlayer.Name = "cmbPlayer";
            this.cmbPlayer.Size = new System.Drawing.Size(184, 21);
            this.cmbPlayer.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(633, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "s";
            // 
            // dgvPoints
            // 
            this.dgvPoints.AllowUserToResizeColumns = false;
            this.dgvPoints.AllowUserToResizeRows = false;
            this.dgvPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPoints.ColumnHeadersHeight = 18;
            this.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.clmDesc});
            this.dgvPoints.ContextMenuStrip = this.cmsPoints;
            this.dgvPoints.EnableHeadersVisualStyles = false;
            this.dgvPoints.Location = new System.Drawing.Point(12, 211);
            this.dgvPoints.MultiSelect = false;
            this.dgvPoints.Name = "dgvPoints";
            this.dgvPoints.RowHeadersVisible = false;
            this.dgvPoints.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPoints.RowTemplate.ReadOnly = true;
            this.dgvPoints.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPoints.Size = new System.Drawing.Size(620, 107);
            this.dgvPoints.TabIndex = 30;
            this.dgvPoints.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPoints_DataError);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Seq";
            this.Column1.MaxInputLength = 4;
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "X";
            this.Column2.MaxInputLength = 5;
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 60;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.Frozen = true;
            this.Column3.HeaderText = "Y";
            this.Column3.MaxInputLength = 5;
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.Frozen = true;
            this.Column4.HeaderText = "Z";
            this.Column4.MaxInputLength = 5;
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.Frozen = true;
            this.Column5.HeaderText = "ID";
            this.Column5.MaxInputLength = 2;
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.Width = 30;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.Frozen = true;
            this.Column6.HeaderText = "Time";
            this.Column6.MaxInputLength = 6;
            this.Column6.Name = "Column6";
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.Width = 60;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.Frozen = true;
            this.Column7.HeaderText = "Name";
            this.Column7.MaxInputLength = 20;
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column7.Width = 190;
            // 
            // clmDesc
            // 
            this.clmDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmDesc.HeaderText = "Desc";
            this.clmDesc.Name = "clmDesc";
            this.clmDesc.Width = 117;
            // 
            // cmsPoints
            // 
            this.cmsPoints.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDeleteRow});
            this.cmsPoints.Name = "cmsPoints";
            this.cmsPoints.Size = new System.Drawing.Size(153, 48);
            this.cmsPoints.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsPoints_ItemClicked);
            // 
            // tsmiDeleteRow
            // 
            this.tsmiDeleteRow.Name = "tsmiDeleteRow";
            this.tsmiDeleteRow.Size = new System.Drawing.Size(152, 22);
            this.tsmiDeleteRow.Text = "Delete Row";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(113, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Marker Seq:";
            // 
            // txtMSeq
            // 
            this.txtMSeq.Location = new System.Drawing.Point(184, 84);
            this.txtMSeq.Name = "txtMSeq";
            this.txtMSeq.Size = new System.Drawing.Size(57, 20);
            this.txtMSeq.TabIndex = 31;
            this.txtMSeq.Text = "0";
            this.txtMSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Increment Time";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UHCWaypoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 372);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtMSeq);
            this.Controls.Add(this.dgvPoints);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbPlayer);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.btnDeath);
            this.Controls.Add(this.btnPortal);
            this.Controls.Add(this.btnFight);
            this.Controls.Add(this.btnSpotted);
            this.Controls.Add(this.btnHouse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnWaypoint);
            this.Controls.Add(this.lblDataSets);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtInc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRotation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtZ);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.lblStatus);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(660, 1200);
            this.MinimumSize = new System.Drawing.Size(660, 410);
            this.Name = "UHCWaypoint";
            this.Text = "UHC Waypoint Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).EndInit();
            this.cmsPoints.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRotation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDataSets;
        private System.Windows.Forms.Button btnWaypoint;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnHouse;
        private System.Windows.Forms.Button btnSpotted;
        private System.Windows.Forms.Button btnFight;
        private System.Windows.Forms.Button btnPortal;
        private System.Windows.Forms.Button btnDeath;
        private System.Windows.Forms.TextBox txtSeq;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ComboBox cmbPlayer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvPoints;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDesc;
        private System.Windows.Forms.ContextMenuStrip cmsPoints;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteRow;
        private System.Windows.Forms.Button button1;
    }
}

