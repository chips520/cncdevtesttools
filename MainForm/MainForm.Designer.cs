namespace MainForm
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btStartListener = new System.Windows.Forms.Button();
            this.btCloseListener = new System.Windows.Forms.Button();
            this.btWinformRun = new System.Windows.Forms.Button();
            this.btAutoConn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btDtSend = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btInstallService = new System.Windows.Forms.Button();
            this.btUninstallService = new System.Windows.Forms.Button();
            this.btStopServer = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbtClose
            // 
            this.cbtClose.FlatAppearance.BorderSize = 0;
            this.cbtClose.Location = new System.Drawing.Point(871, 6);
            // 
            // cbtMax
            // 
            this.cbtMax.FlatAppearance.BorderSize = 0;
            // 
            // cbtMin
            // 
            this.cbtMin.FlatAppearance.BorderSize = 0;
            this.cbtMin.Location = new System.Drawing.Point(753, 11);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(465, 102);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(409, 407);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // btStartListener
            // 
            this.btStartListener.Location = new System.Drawing.Point(178, 109);
            this.btStartListener.Margin = new System.Windows.Forms.Padding(2);
            this.btStartListener.Name = "btStartListener";
            this.btStartListener.Size = new System.Drawing.Size(116, 34);
            this.btStartListener.TabIndex = 1;
            this.btStartListener.Text = "btStartListener";
            this.btStartListener.UseVisualStyleBackColor = true;
            this.btStartListener.Click += new System.EventHandler(this.btStartListenerClick);
            // 
            // btCloseListener
            // 
            this.btCloseListener.Location = new System.Drawing.Point(316, 109);
            this.btCloseListener.Margin = new System.Windows.Forms.Padding(2);
            this.btCloseListener.Name = "btCloseListener";
            this.btCloseListener.Size = new System.Drawing.Size(116, 34);
            this.btCloseListener.TabIndex = 2;
            this.btCloseListener.Text = "关闭连接";
            this.btCloseListener.UseVisualStyleBackColor = true;
            this.btCloseListener.Click += new System.EventHandler(this.btCloseListenerClick);
            // 
            // btWinformRun
            // 
            this.btWinformRun.Location = new System.Drawing.Point(27, 109);
            this.btWinformRun.Margin = new System.Windows.Forms.Padding(2);
            this.btWinformRun.Name = "btWinformRun";
            this.btWinformRun.Size = new System.Drawing.Size(134, 34);
            this.btWinformRun.TabIndex = 3;
            this.btWinformRun.Text = "初始化并运行";
            this.btWinformRun.UseVisualStyleBackColor = true;
            this.btWinformRun.Click += new System.EventHandler(this.button3_Click);
            // 
            // btAutoConn
            // 
            this.btAutoConn.Location = new System.Drawing.Point(27, 155);
            this.btAutoConn.Margin = new System.Windows.Forms.Padding(2);
            this.btAutoConn.Name = "btAutoConn";
            this.btAutoConn.Size = new System.Drawing.Size(134, 34);
            this.btAutoConn.TabIndex = 4;
            this.btAutoConn.Text = "关闭自动连接";
            this.btAutoConn.UseVisualStyleBackColor = true;
            this.btAutoConn.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(465, 64);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(112, 22);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "是否输出消息显示";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(27, 289);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(372, 220);
            this.listBox1.TabIndex = 7;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 258);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "量仪串口设备列表：";
            // 
            // btDtSend
            // 
            this.btDtSend.Location = new System.Drawing.Point(178, 155);
            this.btDtSend.Margin = new System.Windows.Forms.Padding(2);
            this.btDtSend.Name = "btDtSend";
            this.btDtSend.Size = new System.Drawing.Size(116, 35);
            this.btDtSend.TabIndex = 51;
            this.btDtSend.Text = "DT发送";
            this.btDtSend.UseVisualStyleBackColor = true;
            this.btDtSend.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(316, 164);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(117, 21);
            this.textBox2.TabIndex = 52;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(20, 22);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(95, 16);
            this.radioButton1.TabIndex = 54;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "窗口程序运行";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(152, 22);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(95, 16);
            this.radioButton2.TabIndex = 55;
            this.radioButton2.Text = "系统服务运行";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(27, 49);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(405, 55);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "运行方式";
            // 
            // btInstallService
            // 
            this.btInstallService.Location = new System.Drawing.Point(27, 205);
            this.btInstallService.Margin = new System.Windows.Forms.Padding(2);
            this.btInstallService.Name = "btInstallService";
            this.btInstallService.Size = new System.Drawing.Size(134, 33);
            this.btInstallService.TabIndex = 57;
            this.btInstallService.Text = "安装服务";
            this.btInstallService.UseVisualStyleBackColor = true;
            this.btInstallService.Click += new System.EventHandler(this.btInstallService_Click);
            // 
            // btUninstallService
            // 
            this.btUninstallService.Location = new System.Drawing.Point(178, 205);
            this.btUninstallService.Margin = new System.Windows.Forms.Padding(2);
            this.btUninstallService.Name = "btUninstallService";
            this.btUninstallService.Size = new System.Drawing.Size(116, 33);
            this.btUninstallService.TabIndex = 58;
            this.btUninstallService.Text = "御载服务";
            this.btUninstallService.UseVisualStyleBackColor = true;
            this.btUninstallService.Click += new System.EventHandler(this.btUninstallService_Click);
            // 
            // btStopServer
            // 
            this.btStopServer.Location = new System.Drawing.Point(316, 206);
            this.btStopServer.Margin = new System.Windows.Forms.Padding(2);
            this.btStopServer.Name = "btStopServer";
            this.btStopServer.Size = new System.Drawing.Size(116, 32);
            this.btStopServer.TabIndex = 59;
            this.btStopServer.Text = "停止服务";
            this.btStopServer.UseVisualStyleBackColor = true;
            this.btStopServer.Click += new System.EventHandler(this.btStopServer_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "DT服务";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 538);
            this.Controls.Add(this.btStopServer);
            this.Controls.Add(this.btUninstallService);
            this.Controls.Add(this.btInstallService);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btDtSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btAutoConn);
            this.Controls.Add(this.btWinformRun);
            this.Controls.Add(this.btCloseListener);
            this.Controls.Add(this.btStartListener);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.richTextBox1, 0);
            this.Controls.SetChildIndex(this.btStartListener, 0);
            this.Controls.SetChildIndex(this.btCloseListener, 0);
            this.Controls.SetChildIndex(this.btWinformRun, 0);
            this.Controls.SetChildIndex(this.btAutoConn, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.listBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbtClose, 0);
            this.Controls.SetChildIndex(this.cbtMax, 0);
            this.Controls.SetChildIndex(this.cbtMin, 0);
            this.Controls.SetChildIndex(this.btDtSend, 0);
            this.Controls.SetChildIndex(this.textBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.btInstallService, 0);
            this.Controls.SetChildIndex(this.btUninstallService, 0);
            this.Controls.SetChildIndex(this.btStopServer, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btStartListener;
        private System.Windows.Forms.Button btCloseListener;
        private System.Windows.Forms.Button btWinformRun;
        private System.Windows.Forms.Button btAutoConn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btDtSend;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btInstallService;
        private System.Windows.Forms.Button btUninstallService;
        private System.Windows.Forms.Button btStopServer;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
    }
}

