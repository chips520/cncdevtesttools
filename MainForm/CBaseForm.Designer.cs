using MainForm.cbeans;

namespace MainForm
{
    partial class CBaseForm
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
            this.cbtMin = new CCloseButton();
            this.cbtMax = new CCloseButton();
            this.cbtClose = new CCloseButton();
            this.SuspendLayout();
            // 
            // cbtMin
            // 
            this.cbtMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(54)))), ((int)(((byte)(119)))));
            this.cbtMin.CBtStates = 0;
            this.cbtMin.CBtType = 2;
            this.cbtMin.FlatAppearance.BorderSize = 0;
            this.cbtMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtMin.Location = new System.Drawing.Point(1036, 10);
            this.cbtMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbtMin.Name = "cbtMin";
            this.cbtMin.Size = new System.Drawing.Size(22, 24);
            this.cbtMin.TabIndex = 50;
            this.cbtMin.UseVisualStyleBackColor = false;
            this.cbtMin.Click += new System.EventHandler(this.cbtMin_Click);
            // 
            // cbtMax
            // 
            this.cbtMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(54)))), ((int)(((byte)(119)))));
            this.cbtMax.CBtStates = 0;
            this.cbtMax.CBtType = 1;
            this.cbtMax.FlatAppearance.BorderSize = 0;
            this.cbtMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtMax.Location = new System.Drawing.Point(1071, 10);
            this.cbtMax.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbtMax.Name = "cbtMax";
            this.cbtMax.Size = new System.Drawing.Size(22, 24);
            this.cbtMax.TabIndex = 50;
            this.cbtMax.UseVisualStyleBackColor = false;
            this.cbtMax.Click += new System.EventHandler(this.cbtMax_Click);
            // 
            // cbtClose
            // 
            this.cbtClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(54)))), ((int)(((byte)(119)))));
            this.cbtClose.CBtStates = 0;
            this.cbtClose.CBtType = 0;
            this.cbtClose.FlatAppearance.BorderSize = 0;
            this.cbtClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtClose.Location = new System.Drawing.Point(1108, 10);
            this.cbtClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbtClose.Name = "cbtClose";
            this.cbtClose.Size = new System.Drawing.Size(22, 24);
            this.cbtClose.TabIndex = 47;
            this.cbtClose.UseVisualStyleBackColor = false;
            // 
            // CBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Controls.Add(this.cbtMin);
            this.Controls.Add(this.cbtMax);
            this.Controls.Add(this.cbtClose);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "机床联网软件";
            this.SizeChanged += new System.EventHandler(this.bt_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public cbeans.CCloseButton cbtClose;
        public cbeans.CCloseButton cbtMax;
        public cbeans.CCloseButton cbtMin;
    }
}