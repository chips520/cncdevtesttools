using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MainForm.serial
{
    public class SerialListener
    {
       

        private event EventHandler serialDataChangeListener;

        public event EventHandler SerialDataChangeListener
        {
            add { serialDataChangeListener += value; }
            remove { serialDataChangeListener -= value; }
        }

        public void FireSerialDataChangeListener(object obj)
        {
            if (this.serialDataChangeListener != null)
            {
                CSerialEventArg ce = new CSerialEventArg();
                this.serialDataChangeListener(obj, ce);
            }
        }
       
    }
}
