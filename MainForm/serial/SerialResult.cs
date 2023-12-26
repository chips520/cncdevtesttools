using MainForm.infos;
using MainForm.serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainForm.serial
{
    public class SerialResult
    {
        public enum ResultType
        {
            /// <summary>
            /// ip格式错误
            /// </summary>
            Error_Ip = -3,
            /// <summary>
            /// 异常
            /// </summary>
            Exception = -2,
            /// <summary>
            /// 数据出错或无数据
            /// </summary>
            Error = -1,
            /// <summary>
            /// 无状态
            /// </summary>
            None = 0,
            /// <summary>
            /// 数据接收成功
            /// </summary>
            Success = 1,
            /// <summary>
            /// 关闭tcp成功
            /// </summary>
            Success_CloseClient = 2,
            /// <summary>
            /// 连接tcp成功
            /// </summary>
            Success_Connect = 3,
            Fail_Connect = -4,
            /// <summary>
            /// 其它信息
            /// </summary>
            Other = 4,
            /// <summary>
            /// 发送成功
            /// </summary>
            Success_Send = 5,
            /// <summary>
            /// 发送失败
            /// </summary>
            Error_Send = -5,
            /// <summary>
            /// 接收回调时的异常，不知道串口服务器为何会有此现像
            /// </summary>
            Error_ReceiveCallBack = -6
        }
        public ResultType Result
        {
            get;
            set;
        }
        public SerialTcpClient SerialClient
        {
            get;
            set;
        }

        public string Msg
        {
            get;
            set;
        }
        public virtual object Data
        {
            get;
            set;
        }

        public MeasuringInfo Info
        {
            get;
            set;
        }
    }
}
