namespace UHC_Tracker
{
    partial class EditTime
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkNegTime = new System.Windows.Forms.CheckBox();
            this.txtMins = new System.Windows.Forms.TextBox();
            this.txtSecs = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 38);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkNegTime
            // 
            this.chkNegTime.AutoSize = true;
            this.chkNegTime.Location = new System.Drawing.Point(88, 13);
            this.chkNegTime.Name = "chkNegTime";
            this.chkNegTime.Size = new System.Drawing.Size(69, 17);
            this.chkNegTime.TabIndex = 2;
            this.chkNegTime.Text = "Negative";
            this.chkNegTime.UseVisualStyleBackColor = true;
            // 
            // txtMins
            // 
            this.txtMins.Location = new System.Drawing.Point(25, 12);
            this.txtMins.MaxLength = 3;
            this.txtMins.Name = "txtMins";
            this.txtMins.Size = new System.Drawing.Size(31, 20);
            this.txtMins.TabIndex = 47;
            this.txtMins.Text = "0";
            this.txtMins.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMins.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMins_KeyPress);
            // 
            // txtSecs
            // 
            this.txtSecs.Location = new System.Drawing.Point(61, 12);
            this.txtSecs.MaxLength = 2;
            this.txtSecs.Name = "txtSecs";
            this.txtSecs.Size = new System.Drawing.Size(21, 20);
            this.txtSecs.TabIndex = 48;
            this.txtSecs.Text = "00";
            this.txtSecs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSecs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMins_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(54, 14);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(10, 13);
            this.label16.TabIndex = 49;
            this.label16.Text = ":";
            // 
            // EditTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 72);
            this.ControlBox = false;
            this.Controls.Add(this.txtMins);
            this.Controls.Add(this.txtSecs);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.chkNegTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditTime";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Time";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkNegTime;
        private System.Windows.Forms.TextBox txtMins;
        private System.Windows.Forms.TextBox txtSecs;
        private System.Windows.Forms.Label label16;
    }
}