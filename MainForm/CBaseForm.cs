using MainForm.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainForm
{
    public partial class CBaseForm : Form
    {
        public CBaseForm()
        {
            InitializeComponent();
        }

        private Color _backlineColor = Color.FromArgb(255, 10, 54, 119);
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap icon = Resources.ioeicon;//MainForm.Properties.Resources.logo;
            int imgW = icon.Width;
            int imgH = icon.Height;
            int offX = 16;
            int offY = 5;
            int gap = 4;
            int h = imgH + gap * 4;
            e.Graphics.FillRectangle(new SolidBrush(_backlineColor), 0, 0, Width, h);

            e.Graphics.DrawImage(icon, offX, gap * 2, imgW, imgH);

            Font fn = new Font("微软雅黑", 16);
            string s = Text;
            if (string.IsNullOrEmpty(s))
            {
                s = "机床联网软件";
            }
            SizeF size = e.Graphics.MeasureString(s, fn);
            e.Graphics.DrawString(s, fn, Brushes.White, new PointF(offX + gap * 2 + imgW, 6));

            base.OnPaint(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //  if (SkinMobile)
            {
                //释放鼠标焦点捕获
                Win32.ReleaseCapture();
                //向当前窗体发送拖动消息
                Win32.SendMessage(this.Handle, 0x0112, 0xF011, 0);
                base.OnMouseUp(e);
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 最小化动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbtMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 最大化动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbtMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.cbtMax.CBtStates = 0;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.cbtMax.CBtStates = 1;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void cCloseButton1_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        /// <summary>
        /// 自定义最小化，最大化，关闭按钮的动态布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_SizeChanged(object sender, EventArgs e)
        {
            int w = Width;
            int h = Height;
            int x = 0;
            int y = 0;
            x = cbtMin.Left;
            y = cbtMin.Top;
            int bw = cbtMin.Width;
            int bh = cbtMin.Height;
            int gap = 10;
            //x = Width - gap * 3 - bw * 3;
            x = Width - gap * 2 - bw * 2;
            this.cbtMin.Left = x;
            //x += bw + gap;
            //this.cbtMax.Left = x;
            x += bw + gap;
            this.cbtClose.Left = x;
        }

        public String GetTitle()
        {
            return @"DNC";
        }
    }
}
