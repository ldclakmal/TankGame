namespace ClientApplication
{
    partial class GUI
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
            this.btnSend = new MetroFramework.Controls.MetroButton();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.txtDes = new System.Windows.Forms.TextBox();
            this.btnJoin = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(695, 63);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "SEND";
            this.btnSend.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnSend.UseSelectable = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtCmd
            // 
            this.txtCmd.Location = new System.Drawing.Point(104, 66);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(585, 20);
            this.txtCmd.TabIndex = 3;
            // 
            // txtDes
            // 
            this.txtDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDes.Location = new System.Drawing.Point(23, 95);
            this.txtDes.Multiline = true;
            this.txtDes.Name = "txtDes";
            this.txtDes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDes.Size = new System.Drawing.Size(747, 253);
            this.txtDes.TabIndex = 4;
            // 
            // btnJoin
            // 
            this.btnJoin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnJoin.Location = new System.Drawing.Point(23, 63);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(75, 23);
            this.btnJoin.TabIndex = 5;
            this.btnJoin.Text = "JOIN";
            this.btnJoin.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnJoin.UseSelectable = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 371);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtDes);
            this.Controls.Add(this.txtCmd);
            this.Controls.Add(this.btnSend);
            this.Name = "GUI";
            this.Text = "Tank Game";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnSend;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.TextBox txtDes;
        private MetroFramework.Controls.MetroButton btnJoin;
    }
}