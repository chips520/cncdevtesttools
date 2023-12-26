
using MainForm.controller;
using MainForm.loghelper;
using MainForm.serial;
using MainForm.tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MainForm.serial.SerialResult;

namespace MainForm.serial
{
    /// <summary>
    /// DT 通讯
    /// 服务监听客户端
    /// 客户端方式，串口设备作为服务器，好处理随处可以运行，串口监听部分需要注意，如果其它地方运行，都开着可能会都能收到串口数据
    /// </summary>
    public class SerialTcpClient
    {
        private int port = 13000;
        private string ip;
        private TcpClient tcpClient;
        private SerialListener listener;
        private ManualResetEvent timeOutObject = new ManualResetEvent(false);
        //private MainControler mainControler;
        private byte[] buffer = new byte[2048];
        private Socket socket;

        public bool ClientConnected
        {
            get;
            set;
        }

        public bool IsDataSubContract
        {
            get;
            set;
        }

        private StringBuilder sb;
        public StringBuilder GetStringBuilder()
        {
            if (sb == null)
                sb = new StringBuilder();
            return sb;
        }
        public SerialTcpClient(SerialListener listener, MainController mainControler)
        {
            this.Listener = listener;
            //this.mainControler = mainControler;
        }

   

        //public bool IsConnected()
        //{
        //    if (tcpClient != null && socket != null)
        //    {
        //       // if (isClientConnected(tcpClient))
        //        // if (socket.Poll(-1, SelectMode.SelectRead))
        //        {
        //            SendNc("test");
        //            return true;
        //        }

        //        //return tcpClient.Connected;
        //    }
        //    return false;
        //}

        //public bool isClientConnected(TcpClient ClientSocket)
        //{
        //    IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

        //    TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

        //    foreach (TcpConnectionInformation c in tcpConnections)
        //    {
        //        TcpState stateOfConnection = c.State;

        //        if (c.LocalEndPoint.Equals(ClientSocket.Client.LocalEndPoint) && c.RemoteEndPoint.Equals(ClientSocket.Client.RemoteEndPoint))
        //        {
        //            if (stateOfConnection == TcpState.Established)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }

        //    }

        //    return false;
        //}

        public int Port { get => port; set => port = value; }
        public string Ip { get => ip; set => ip = value; }
        public SerialListener Listener
        {
            get => listener;
            set => listener = value;
        }

        public bool StartListener()
        {

            // 只处理发起连接，连接是否成功走异步通知
            Connecting();
            //if (Connecting())
            //{
            //    //socket = tcpClient.Client;
            //    //socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            //    Console.WriteLine("============= StartListener ================ " + true);
            //    return true;
            //}
            Console.WriteLine("============ StartListener ================= " + false);
            return false;
           // return false;
        }

        public bool StartSocketListener()
        {
            if (this.tcpClient != null && this.tcpClient.Connected)
            {
                socket = tcpClient.Client;
                socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
                Console.WriteLine("============== StartSocketListener =============== " + this.ip + ":" + port + " -- " +  socket.LocalEndPoint.ToString()+ " " + true);
                return true;
            }
            Console.WriteLine("=============== StartSocketListener ============== " + false);
            return false;
            // return false;
        }

        private bool Connecting()
        {
            // 监听初始化 tcp
            if (tcpClient == null)
            {
                tcpClient = new TcpClient();
            }
            try
            {
                if (!UtilityTools.IPCheck(this.Ip) || !System.Net.IPAddress.TryParse(this.Ip, out System.Net.IPAddress address))
                {
                    if (Listener != null)
                        Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Error_Ip, "ip address error " + this.Ip + " port " + Port));
                    //if (mainControler != null)
                    //{
                    //    mainControler.PrintLog("ip address error " + this.Ip + " port " + Port);
                    //}
                    // Controler.PrintLine("ip address error " + this.IpStr, true);
                    return false;
                }

                tcpClient.BeginConnect(Ip, Port, new AsyncCallback(ConnectCallBack), tcpClient);
                timeOutObject.WaitOne(6000, true);
                Console.WriteLine("============================= xxxxxxxxxxxxxxxxxxx " );
                return tcpClient.Connected;
            }
            catch (Exception ex)
            {
                if (Listener != null)
                    Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Exception, "ip address error " + this.Ip + " port " + Port, ex));
                //if (mainControler != null)
                //    mainControler.PrintLog("ip address error " + this.Ip + " port " + Port, ex);
                // Controler.PrintLine("ip address TryParse error " + this.IpStr, true);
                return false;
            }
            //tcpClient.BeginConnect(Ip, Port, new AsyncCallback(ConnectCallBack), tcpClient);
            //timeOutObject.WaitOne(6000, true);

            //return tcpClient.Connected;
            return false;
        }

        private void ConnectCallBack(IAsyncResult asyncresult)
        {
            try
            {
              //  IsEthConnected = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                   
                    if (Listener != null)
                        Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Success_Connect, "connected " + this.Ip + " port " + Port));
                    //if (mainControler != null)
                    //    mainControler.PrintLog("connected " + this.Ip + " port " + Port);
                    //IsEthConnected = true;
                    // Controler.PrintLine("connect :" + SendAndRec("connect ok", false), true);
                }
            }
            catch (Exception ex)
            {
                //  IsEthConnected = false;
                //Controler.PrintLine("ConnectCallback ex: ", true, ex);
                if (Listener != null)
                    Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Fail_Connect, "ConnectCallback error " + this.Ip + " port " + Port, ex));
                //if (mainControler != null)
                //    mainControler.PrintLog("ConnectCallback error " + this.Ip + " port " + Port, ex);
            }
            finally
            {
                timeOutObject.Set();
            }
        }

        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                if (socket != null)
                {
                    int length = socket.EndReceive(asyncResult);
                    socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
                    if (length == 0)
                    {
                        //socket.Send
                        EndListener();
                        if (Listener != null)
                            Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Error, "receive length " + length));
                        return;
                    }

                    byte[] data = new byte[length];
                    Array.Copy(buffer, 0, data, 0, length);
                    string msg = string.Empty;
                    msg = Encoding.UTF8.GetString(data);
                    //try
                    //{
                    //    //LogHelper.WriteSysLog("zzdefault "+Encoding.Default.GetString(data));
                    //    //LogHelper.WriteSysLog("zzUTF8 " + Encoding.UTF8.GetString(data));
                    //    //LogHelper.WriteSysLog("zzDefault " + Encoding.Default.GetString(data));
                    //    //Console.WriteLine(Encoding.Unicode.GetString(data));
                    //    //Console.WriteLine(Encoding.Default.GetString(data));
                    //    //Console.WriteLine(Encoding.UTF8.GetString(data));
                    //}
                    //catch (Exception ex)
                    //{

                    //}


                    if (Listener != null)
                        Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Success,
                            "receive [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]" + msg + Environment.NewLine, msg));
                    //if (mainControler != null)
                    //    mainControler.PrintLog("receive [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]" + msg + Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                if (Listener != null)
                    Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Error_ReceiveCallBack, this.ip + " Error_ReceiveCallBack ", ex));
                //if (mainControler != null)
                //    mainControler.PrintLog("receive " , ex);
            }
        }

        public SerialResult SendData(string content)
        {
            if (tcpClient == null)
            {
                return GetSerialResult(ResultType.Error_Send, "send fail ");
            }
            if (tcpClient != null && socket != null)
            {
                try
                {
                    byte[] sendBytes = Encoding.UTF8.GetBytes(content);
                    int sendLen = socket.Send(sendBytes, 0, sendBytes.Length, SocketFlags.None);
                    //SerialResult result = GetSerialResult(ResultType.Success_Send, "sendLen ： " + sendLen);
                    //if (Listener != null)
                    //    Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Success_Send, "sendLen ： " + sendLen));
                    return  GetSerialResult(ResultType.Success_Send, "sendLen ： " + sendLen);
                }
                catch (Exception ex)
                {
                    //if (Listener != null)
                    //    Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Error_Send, "send ex ： " + ex.Message, ex));

                    return GetSerialResult(ResultType.Error_Send, "send ex ： " + ex.Message, ex);
                }
            }

            return GetSerialResult(ResultType.Error_Send, "send fail ");
        }

        public void SendNc(string content)
        {
            if (tcpClient == null)
            {
                return;
            }
            //if (tcpClient.Connected)
            {
                try
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += DoWork_SendNc; ;
                    worker.RunWorkerCompleted += RunWorkerCompleted_SendNc; ;
                    worker.RunWorkerAsync(content);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void RunWorkerCompleted_SendNc(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void DoWork_SendNc(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is string)
            {
                string content = e.Argument as string;
                //if (string.IsNullOrEmpty(content))
                //{
                //    e.Result = "error";
                //    return;
                //}
                if (tcpClient == null)
                {
                    e.Result = "#error";
                    return;
                }
                if (tcpClient != null && socket != null)
                {
                    try
                    {
                        byte[] sendBytes = Encoding.ASCII.GetBytes(content);
                        int sendLen = socket.Send(sendBytes, 0, sendBytes.Length, SocketFlags.None);
                        if (Listener != null)
                            Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Success_Send, "sendLen ： " + sendLen));
                    }
                    catch (Exception ex)
                    {
                        if (Listener != null)
                            Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Error_Send, "send ex ： " + ex.Message, ex));
                    }
                }
            }
        }

        public bool EndListener()
        {
            if (tcpClient == null)
                return true;
            if (tcpClient.Connected)
            {
                try
                {
                    if (socket != null)
                    {
                        // socket.EndReceive(new AsyncCallback(MyAsyncCallback));
                        socket.Disconnect(true);
                        socket.Close();
                        socket = null;
                    }
                    tcpClient.Close();
                }
                catch (Exception ex)
                {
                    if (Listener != null)
                        Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Exception, "EndListener ex: " + this.Ip + " port " + Port, ex));
                    //if (mainControler != null)
                    //    mainControler.PrintLog("EndListener ex: " + this.Ip + " port " + Port, ex);
                }
            }
            tcpClient = null;
            if (Listener != null)
                Listener.FireSerialDataChangeListener(GetSerialResult(ResultType.Success_CloseClient, "EndListener " + this.Ip + " port " + Port));
            //if (mainControler != null)
            //    mainControler.PrintLog("EndListener " + this.Ip + " port " + Port);
            return true;
        }
        private  void MyAsyncCallback(IAsyncResult ar)
        {
            Console.WriteLine("异步调用");
            Console.ReadLine();
        }

        private SerialResult GetSerialResult(SerialResult.ResultType type, string msg, object data = null)
        {
            return new SerialResult() {Result = type, Msg = msg, Data = data, SerialClient = this };
        }

        public override string ToString()
        {
           
            StringBuilder sb = new StringBuilder();

            sb.Append(" ip: ").Append(Ip);
            sb.Append(" port: ").Append(port);

            return sb.ToString();
        }
    }
}
