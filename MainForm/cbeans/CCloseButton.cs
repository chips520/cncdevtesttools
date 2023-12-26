using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainForm.cbeans
{
    public class CCloseButton : Button
    {
        public int CBtType
        {
            get;
            set;
        }
        public int CBtStates
        {
            get;
            set;
        }
        public CCloseButton() : base()
        {
            this.SetStyle(
                ControlStyles.UserPaint |  //控件自行绘制，而不使用操作系统的绘制
                ControlStyles.AllPaintingInWmPaint | //忽略擦出的消息，减少闪烁。
                ControlStyles.OptimizedDoubleBuffer |//在缓冲区上绘制，不直接绘制到屏幕上，减少闪烁。
                ControlStyles.ResizeRedraw | //控件大小发生变化时，重绘。                  
                ControlStyles.SupportsTransparentBackColor, true);//支持透明背景颜色
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Drawing2D.SmoothingMode oldMode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (CBtType == 0) // x
            {
                e.Graphics.DrawLine(System.Drawing.Pens.White, 2, 2, Width - 2, Height - 2);
                e.Graphics.DrawLine(System.Drawing.Pens.White, 2, Height - 2, Width - 2, 2);
            }
            else if (CBtType == 1) // max
            {
                if (CBtStates == 1)
                {
                    e.Graphics.DrawRectangle(System.Drawing.Pens.White, 6, 2, Width - 8, Height - 8);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 10, 54, 119)), 4, 4, Width - 8, Height - 8);
                    e.Graphics.DrawRectangle(System.Drawing.Pens.White, 4, 4, Width - 8, Height - 8);
                }
                else
                    e.Graphics.DrawRectangle(System.Drawing.Pens.White, 2, 2, Width - 4, Height - 4);
            }
            else if (CBtType == 2) // min
            {
                e.Graphics.DrawLine(System.Drawing.Pens.White, 2, Height - 4, Width - 4, Height - 4);
            }
            else if (CBtType == 3)
            {

            }
            e.Graphics.SmoothingMode = oldMode;
        }
    }
}
