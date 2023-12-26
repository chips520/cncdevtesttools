using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.cfg
{
    /// <summary>
    /// 量仪采集时的多路串口服务器信息
    /// 一个串口服务器有3路ip，对应3台量仪
    /// 量仪配置信息格式按照多路的来配置，端口列表
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SerialIpInfo
    {
        [JsonProperty(PropertyName = "ip")]
        public string Ip
        {
            get;
            set;
        }


        //[JsonProperty(PropertyName = "ports")]
        //public List<int> Ports
        //{
        //    get;
        //    set;
        //}
        [JsonProperty(PropertyName = "port")]
        public int Port
        {
            get;
            set;
        }

        public override string ToString()
        {
            //if (Ports != null)
            //{
            StringBuilder sb = new StringBuilder();
            sb.Append("ip: ").Append(Ip);
            sb.Append(" port: ").Append(Port);
            //foreach (int port in Ports)
            //{
            //    sb.Append(port);
            //    sb.Append(";");
            //}
            return sb.ToString();
            //}
            //else
            //{
            //    return Ip;
            //}
        }
    }
}
