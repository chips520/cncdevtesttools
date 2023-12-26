using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainForm.cbeans
{
    public class CLabel : Label
    {
        public CLabel():base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            Graphics graphics = CreateGraphics();
            string label_str = Text;
            string str = label_str;
            SizeF sizeF = graphics.MeasureString(label_str, Font);
            while (sizeF.Width >= Width - 10)
            {
                str = label_str.Substring(0, str.Length - 1);
                sizeF = graphics.MeasureString(str, Font);
                Text = str + "...";
            }
        }
    }
}
