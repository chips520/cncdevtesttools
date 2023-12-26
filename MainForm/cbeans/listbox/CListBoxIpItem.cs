using MainForm.serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.cbeans.listbox
{
    public class CListBoxIpItem
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string No
        {
            get;
            set;
        }

        /// <summary>
        /// 0: 日升量仪串口服务器
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int SysType
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public SerialTcpClient serialTcpClient
        {
            get;
            set;
        }
        public string Name
        {
            get { return ToString();  }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Type == 0)
            {
                sb.Append(No);
                sb.Append("-");
                sb.Append(IP);
                sb.Append(":");
                sb.Append(serialTcpClient.Port);
                //sb.Append("(日升量仪串服)");
            }
            else
            {

                sb.Append(No);
                sb.Append("-");
                sb.Append( IP);
            }
            if (serialTcpClient.ClientConnected)
            {
                sb.Append("在线");
            }
            return sb.ToString();
            
        }

        public bool IsConnected
        {
            get;
            set;
        }
    }
}
