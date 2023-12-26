using MainForm.serial;
using MainForm.cfg;
using MainForm.loghelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static MainForm.serial.SerialResult;
using System.ComponentModel;
using System.Collections.Concurrent;
using MainForm.infos;
using System.Net;
using System.Timers;
using MainForm.model;

namespace MainForm.controller
{
    /// <summary>
    /// 这个是控制层 DT 通讯，类似dde 采集的形式，机床端是服务端，而这个是客户端
    /// 框架用量仪的框架，底层通迅比较适用，正好是客户端形式通讯
    /// </summary>
    public class MainController
    {
        private System.Timers.Timer timer = null;
        private System.Timers.Timer dataSendTimer = null;
        /// <summary>
        /// log 定时删除
        /// </summary>
        private System.Timers.Timer timerSystemMaintenance;

        SerialListener listener = new SerialListener();

        List<SerialTcpClient> clientList = new List<SerialTcpClient>();

        public delegate void TcpClientStateChangedHandler(object data, SerialResult serialResult);
        private TcpClientStateChangedHandler tcpClientStateChangedHandler;
        // 通讯model
        List<AbstractModel> modelList = new List<AbstractModel>();


        public MainController()
        {
            LogHelper.InitLog4Net();
            AddListener();
        }

        public void StartTimer()
        {
            PrintLog("======================mc start timer", null);
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 20000;// 60000;
                timer.Elapsed += Timer_Elapsed;
               
            }
            timer.Enabled = true;

            //
            if (dataSendTimer == null)
            {
                dataSendTimer = new System.Timers.Timer();
                //dataSendTimer.Interval = 1000 * 60 * 2;
                dataSendTimer.Interval = 10000;//
                //1000 * 30;
                dataSendTimer.Elapsed += DataSendTimer_Elapsed;
            }
            dataSendTimer.Enabled = true;


            // 定时删除日志
            if (timerSystemMaintenance == null)
            {
                timerSystemMaintenance = new System.Timers.Timer();
                // 240小时（10天）清一下日志 1小时检查一下,1小时定时器检查
                timerSystemMaintenance.Interval = 3600000;//)//1000 * 60 * 60 * 48
                timerSystemMaintenance.Elapsed += timerDelLogs_Elapsed; ;
            }
            //timerSystemMaintenance.Enabled = true;
            timerSystemMaintenance.Start();
        }

      

        public void StopTimer()
        {
            try
            {
                if (timer != null)
                {
                    timer.Enabled = false;
                }
                if (dataSendTimer != null)
                {
                    dataSendTimer.Enabled = false;
                }
                if (timerSystemMaintenance != null)
                {
                    timerSystemMaintenance.Enabled = false;
                }
                // 停止
                if (modelList != null)
                {
                    foreach (AbstractModel tmp in modelList)
                    {
                        if (tmp != null)
                            tmp.Stop();
                    }
                }

                foreach (SerialTcpClient client in clientList)
                {
                    if (client != null)
                    {
                        DisConnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
            }
           
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           
            try
            {
                if (clientList != null)
                {

                    foreach (SerialTcpClient client in clientList)
                    {
                        if (client != null && !client.ClientConnected)
                        {
                            BackgroundWorker worker = new BackgroundWorker();
                            worker.DoWork += Connect_DoWork;
                            worker.RunWorkerCompleted += Connect_RunWorkCompleted;
                            worker.RunWorkerAsync(client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("BeginConnect: ", ex);
            }
        }

        public event TcpClientStateChangedHandler TcpClientChangedListener
        {
            add { this.tcpClientStateChangedHandler += value; }
            remove { this.tcpClientStateChangedHandler -= value; }
        }
        private void LoadSysCfg()
        {
            try
            {

                string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
                string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
                string ipFilePath = assemblyDirPath + "/cfg/syscfg.cfg";
                SysCfgInfo sysCfgInfo = null;
                using (StreamReader sr = new StreamReader(ipFilePath, Encoding.UTF8))
                {
                    string str = sr.ReadToEnd();// ReadLine();
                                                // PrintLog("js Url ===========  : " + URL_Str, true);
                    PrintLog("sys cfg is :" + str, null, true);
                    sysCfgInfo = JsonConvert.DeserializeObject<SysCfgInfo>(str);

                }
                if (sysCfgInfo == null)
                {
                    PrintLog("sys cfg is null:", null, true);
                    return;
                }
                PrintLog(sysCfgInfo.ToString(), null, true);
                //controller = new MainController();
                SysCfg = sysCfgInfo;
                //controller.SerialDataChangeListener += Controller_SerialDataChangeListener;
            }
            catch (Exception ex)
            {
                PrintLog("loadcfg ex " + ex.Message, ex);//.WriteLine(ex.Message);
                // PrintLog("form load", ex, true);
            }
        }
        public void InitSerialTcpClient()
        {
            LoadSysCfg();

            if (SysCfg != null && SysCfg.InfoList != null)
            {
                foreach (SerialIpInfo info in SysCfg.InfoList)
                {
                    if (info == null)
                        continue;
                    string ip = info.Ip;
                    int port = info.Port;
                    SerialTcpClient client = AddTcpClient(ip, port);
                    AbstractModel model = FindModel(ip, port, client);
                    if (model == null)
                    {
                        model = new DtModel(this);
                        model.SeriesTcpClient = client;
                        Listener.SerialDataChangeListener += model.Listener_SerialDataChangeListener;
                        this.modelList.Add(model);
                    }
                }
            }
        }

        public SerialTcpClient AddTcpClient(string ip, int port)
        {
            //zzuser300
            SerialTcpClient tmpClient = clientList.Find(c => c.Ip == ip && c.Port == port);
            if (tmpClient == null)
            {
                tmpClient = new SerialTcpClient(listener, this) { Ip = ip, Port = port };
                clientList.Add(tmpClient);
                //if (timer == null)
                //{
                //    timer = new System.Timers.Timer();
                //    timer.Interval = 20000;// 60000;
                //    timer.Elapsed += Timer_Elapsed;
                //    timer.Start();
                //}
            }
            return tmpClient;
        }
        private AbstractModel FindModel(string ip, int port, SerialTcpClient client)
        {
            if (modelList != null)
            {
                // ip、 
                return modelList.Find(c => c.SeriesTcpClient != null && c.SeriesTcpClient == client);
            }
            return null;
        }

        public SysCfgInfo SysCfg
        {
            get;
            set;
        }

        public SerialListener Listener { get => listener; set => listener = value; }
        public List<SerialTcpClient> ClientList { get => clientList; set => clientList = value; }

        public void AddListener()
        {
            Listener.SerialDataChangeListener += Listener_SerialDataChangeListener;
        }

        private void Listener_SerialDataChangeListener(object sender, EventArgs e)
        {
            if (sender != null && sender is SerialResult)
            {
                SerialResult sr = sender as SerialResult;
                if (sr.Result == ResultType.Success)
                {
                    string ncContent = sr.Data.ToString();
                    // 收到串口服务器的nc内容
                    if (!string.IsNullOrEmpty(ncContent))
                    {
                        if (tcpClientStateChangedHandler != null)
                        {
                            tcpClientStateChangedHandler(ncContent, sr);
                        }

                        //"## 001 n update 0/295.0294/NG"
                        // 解析数据
                        //AnalyzeData(ncContent, sr);
                        PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    }
                }
                else if (sr.Result == ResultType.Success_Send)
                {
                    if (tcpClientStateChangedHandler != null)
                    {
                        tcpClientStateChangedHandler("", sr);
                    }
                    PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    // mainForm.SerialSendActived(sr.SerialClient, sr);
                    // PrintLog(sr.Msg);
                }
                else if (sr.Result == ResultType.Success_Connect || sr.Result == ResultType.Fail_Connect)
                {
                    if (sr.Result == ResultType.Success_Connect)
                    {
                        sr.SerialClient.ClientConnected = true;
                        sr.SerialClient.StartSocketListener();
                    }
                    else
                        sr.SerialClient.ClientConnected = false;
                    if (tcpClientStateChangedHandler != null)
                    {
                        tcpClientStateChangedHandler("", sr);
                    }
                    PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    //if (sr.Result == ResultType.Success_Connect)
                    //{
                    //    sr.SerialClient.StartSocketListener();
                    //}
                    //mainForm.ConnectedActived(sr.SerialClient, sr);
                }
                else if (sr.Result == ResultType.Success_CloseClient)
                {
                    sr.SerialClient.ClientConnected = false;
                    if (tcpClientStateChangedHandler != null)
                    {
                        tcpClientStateChangedHandler("", sr);
                    }
                    PrintLog(sr.SerialClient.Ip + " " + sr.Msg, null);
                    //CListBoxIpItem item = FindListItem(sr.SerialClient);
                    //if (item != null)
                    //{
                    //    item.IsConnected = false;
                    //    //RemoveIpItem(item);
                    //    mainForm.SerialClientCloseActived(sr.SerialClient, sr);
                    //    PrintLog(sr.SerialClient.ToString() + " offline disconn ");

                    //    // 自动重连
                    //    //BackgroundWorker worker = new BackgroundWorker();
                    //    //worker.DoWork += Worker_DoWork1;
                    //    //worker.RunWorkerCompleted += Worker_RunWorkerCompleted1;
                    //    //worker.RunWorkerAsync(item);
                    //}
                }
                else if (sr.Result == ResultType.Error_Send || sr.Result == ResultType.Error_ReceiveCallBack)
                {
                    // 断开重连

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += CheckOffLineDoWork;
                    worker.RunWorkerCompleted += CheckOffLineDoWorkCompleted;
                    worker.RunWorkerAsync(sr.SerialClient);
                    if (tcpClientStateChangedHandler != null)
                    {
                        tcpClientStateChangedHandler("", sr);
                    }
                    PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    //CListBoxIpItem item = FindListItem(sr.SerialClient);
                    //if (item != null)
                    //{
                    //    BackgroundWorker worker = new BackgroundWorker();
                    //    worker.DoWork += CheckOffLineDoWork;
                    //    worker.RunWorkerCompleted += Worker_RunWorkerCompleted1;
                    //    worker.RunWorkerAsync(item);
                    //}
                    //mainForm.SerialSendActived(sr.SerialClient, sr);
                    //PrintLog(sr.SerialClient.Ip + " " + sr.Msg);
                }
                else
                {
                    if (tcpClientStateChangedHandler != null)
                    {
                        tcpClientStateChangedHandler("", sr);
                    }
                    PrintLog(sr.SerialClient.ToString() + " " +sr.Msg, sr.Data is Exception ? sr.Data as Exception : null);
                    //if (sr.Result != ResultType.Error)
                    //    PrintLog(sr.SerialClient.Ip + " " + sr.Msg, sr.Data is Exception ? sr.Data as Exception : null);
                }
            }            
        }

        public void BeginConnect()
        {
            try
            {
                if (clientList != null)
                {

                    foreach (SerialTcpClient client in clientList)
                    {
                        if (client != null)
                        {
                            BackgroundWorker worker = new BackgroundWorker();
                            worker.DoWork += Connect_DoWork;
                            worker.RunWorkerCompleted += Connect_RunWorkCompleted;
                            worker.RunWorkerAsync(client);
                           // client.StartListener();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("BeginConnect: ", ex);
            }
        }

        public void DisConnect()
        {
            try
            {
                if (clientList != null)
                {
                    foreach (SerialTcpClient client in clientList)
                    {
                        if (client != null)
                        {
                            client.EndListener();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("DisConnect: ", ex);
            }
        }
        public bool BeginConnect(SerialTcpClient client)
        {
            if (client != null)
            {
                return client.StartListener();
            }
            return false;
        }

        private void Connect_RunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Connect_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null && e.Argument is SerialTcpClient)
            {
                SerialTcpClient srClient = e.Argument as SerialTcpClient;
                this.BeginConnect(srClient);
            }
            e.Result = "";
        }


        private void CheckOffLineDoWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
     
        private void CheckOffLineDoWork(object sender, DoWorkEventArgs e)
        {
            //MyTools.PingIpOrDomainName(client.Ip)
            if (e.Argument != null && e.Argument is SerialTcpClient)
            {
                SerialTcpClient srClient = e.Argument as SerialTcpClient;
                this.DisConnect(srClient);
            }
            e.Result = "";
        }

        public bool DisConnect(SerialTcpClient client)
        {
            if (client != null)
            {
                return client.EndListener();
            }
            return false;
        }

        public void PrintLog(string context, Exception e, bool isDebug = true)
        {
            if (isDebug)
            {
                if (e == null)
                {
                    LogHelper.WriteSysLog(context);
                }
                else
                {
                    LogHelper.WriteLog(context, e);
                }
                Console.WriteLine(context);
            }
        }


       // private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        // 采集的数据放到这个列表中
        private ConcurrentBag<MeasuringInfo> bag = new ConcurrentBag<MeasuringInfo>();
        private ConcurrentBag<MeasuringInfo> bagBak = new ConcurrentBag<MeasuringInfo>();



       

        /// <summary>
        /// 解析量仪数据，然后发送到平台
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sr"></param>
        //public void AnalyzeData(string content, SerialResult sr)
        //{
        //    //  //"## 001 n update 0/295.0294/NG"
        //    if (!string.IsNullOrEmpty(content))
        //    {
        //        string[] array = content.Split(new char[] {' '});
        //        if (array != null && array.Length > 0)
        //        {
        //            string tmp = array[array.Length - 1];
        //            string[] array2 = tmp.Split(new char[] {'/'});
        //            if (array2 != null && array2.Length == 3)
        //            {
        //                string value = array2[1];
        //                if (!string.IsNullOrEmpty(value))
        //                {
        //                    // 发送到平台
        //                    string ip = sr.SerialClient.Ip;
        //                    int port = sr.SerialClient.Port;
        //                    MeasuringInfo info = new MeasuringInfo() { Ip = ip, Port = port, Data = value };
        //                    // 发送格式...
        //                    bag.Add(info);
                            
        //                }
        //            }
        //        }
        //    }
        //}

        //=========url 先固定在代码中，原则上也可以放配置中，防止乱改，先代码中固定
        private string URL_Str = "http://116.62.225.115/api/";
        public String Url
        {
            get { return URL_Str; }// "http://116.62.225.115/api/";}
        }
        bool ncChecking = false;
        private HardwareInfo hardInfo = new HardwareInfo();
        private string hdId = string.Empty;

        //private ConcurrentBag<ResultInfo> bagDtRs = new ConcurrentBag<ResultInfo>();
        /// <summary>
        /// 发送DT Reason 列表，发到平台不成功，继续发
        /// </summary>
        private ConcurrentBag<ResultInfo> bagDtRsBak = new ConcurrentBag<ResultInfo>();
        /// <summary>
        ///  发送DT Reason 列表，发到平台不成功，继续发
        /// </summary>
        /// <returns></returns>
        private List<ResultInfo> Get2TResultInfoList()
        {
            List<ResultInfo> resultList = new List<ResultInfo>();
            if (ncInfoList != null)
            {
                
                // 之有未成功返回给平台的信息，再发送出去
                while (true)
                {
                    try
                    {
                        ResultInfo ncResultInfo = null;
                        if (bagDtRsBak.TryTake(out ncResultInfo))
                        {
                            if (ncResultInfo != null)
                            {
                                Console.WriteLine("ResultInfo.dtid : " + ncResultInfo.ID + " " + ncResultInfo.Result);
                                resultList.Add(ncResultInfo);
                            }
                        }
                        else
                        {
                            //Console.WriteLine("=== GetNcResultInfoList ncResultInfo TryTake false ==========");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("=== Get2TResultInfoList ResultInfo ex ==========" + ex.Message);
                    }
                }

              
            }
            return resultList;
        }

        public void addDtRs(ResultInfo info)
        {
            bagDtRsBak.Add(info);
        }
        /// <summary>
        /// 发送dt reason
        /// </summary>
        /// <param name="info"></param>
        private void SendDtRs(ResultInfo info)
        {
            string httpUrl = Url + "dtResult?reason=" + info.Reason;
            if (HttpPost(httpUrl, ""))
            {
                PrintLog("发送确认 dt reason ok " + httpUrl, null);
            }
            else
            {
                PrintLog("发送确认 dt reason fail " + httpUrl, null);
                bagDtRsBak.Add(info);
            }
        }
        /// <summary>
        /// 获取心跳 发送结果 dt reason ；dt 发送到机床结果； 获取DT 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataSendTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PrintLog("========dt time ncChecking" + ncChecking, null);
            if (ncChecking)
                return;
            // 虽是多线程，但要控制不要同时去处理nc传程心跳接口
            ncChecking = true;
            // zzz
            //HttpPost();
            try
            {

                //=====================================================================
                // 检查未发送的dtreason
                //=====================================================================
                List<ResultInfo> dtResults = Get2TResultInfoList();
                if (dtResults != null && dtResults.Count > 0)
                {
                    foreach (ResultInfo tmp in dtResults)
                    {
                        SendDtRs(tmp);
                    }
                }
                //=====================================================================
                if (string.IsNullOrEmpty(hdId))
                {
                    hdId = hardInfo.GetHardDiskID();
                }
                string cswc = "";
                string httpUrl = Url + "dtstate?sn=" + hdId;
                ////"http://192.168.1.126:8080/ioe/api/dncstate?sn="+hdId;
                //string ncResult = GetNcResultInfoList();

                // 先找到有没有需要返回结果的处理成功失败的信息，
                //ncInfoList 取出来，列表清空 trytake
                List<ResultInfo> resultList = GetResultInfoList();
                string ncResult = string.Empty;
                if (resultList != null && resultList.Count > 0)
                {
                    try
                    {
                        // 不能json,拼字符串
                        //ncResult = JsonConvert.SerializeObject(resultList);
                        foreach (ResultInfo tmpRs in resultList)
                        {
                            if (tmpRs != null)
                            {
                                ncResult = "id:" + tmpRs.ID + "result:" + tmpRs.Result;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("=== GetNcResultInfoList JsonConvert ex ==========" + ex.Message);
                    }
                }
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>> ncResult: " + ncResult);
                // 如果需要返回给平台信息的话，加上cswc,接口定义
                //if (!string.IsNullOrEmpty(ncResult))
                //{
                //    httpUrl = httpUrl + "&cswc=" + ncResult;
                //}

                //string response = HttpPostResponse(httpUrl,"");
                // HttpResponseResult httpResult = new HttpResponseResult();//
                HttpResponseResult httpResult = HttpPostResponse(httpUrl, "");
                if (!httpResult.IsSuccess)
                {
                    if (resultList != null && resultList.Count > 0)
                    {
                        try
                        {
                            foreach (ResultInfo tmp in resultList)
                            {
                                if (tmp != null)
                                {
                                    // 这里的ncresultinfo应该不会有重复的，每次取出来，原ConcurrentBag<NcInfo> ncInfoList 中也是去除的TryTake
                                    // 代码中确保两个list不要死循环了，取出来要清空
                                    //ncResultList 在GetNcResultInfoList中已清空了
                                    ncResultList.Add(tmp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("=== GetNcResultInfoList JsonConvert ex ==========" + ex.Message);
                        }
                    }
                    PrintLog("dt time http result not success " + ncChecking, null);
                    ncChecking = false;
                    return;
                }

                string response = httpResult.Response;
                PrintLog("response: " + response, null);
                if (!string.IsNullOrEmpty(response))
                {
                    NcStateInfo ncStateInfo = JsonConvert.DeserializeObject<NcStateInfo>(response);
                    if (ncStateInfo != null && ncStateInfo.NcList != null && ncStateInfo.NcList.Count > 0)
                    {
                        foreach (MeasuringInfo minfo in ncStateInfo.NcList)
                        {
                            string dtIp = minfo.Ip;
                            //string dtData = minfo.DtId;
                            minfo.Port = 14000;
                            minfo.Data = "{\"dtid\":" + minfo.DtId + "}";
                           // long dtid = 0L;
                            if (modelList != null)
                            {
                                // ip
                                AbstractModel model = modelList.Find(c => c.SeriesTcpClient != null && c.SeriesTcpClient.Ip == dtIp);
                                if (model != null)
                                {
                                    //MeasuringInfo measuringInfo = new MeasuringInfo() { Ip = dtIp, Port = 14000, Data = dtData, DtId = dtid };

                                    model.Queue.Enqueue(minfo);
                                    model.Start();
                                }
                            }
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {

            }
            ncChecking = false;
            //if (bag != null)
            //{
            //    MeasuringInfo info = null;
            //    int idx = 0;
            //    while (!bag.IsEmpty)
            //    {
            //        bag.TryTake(out info);
            //        if (info != null)
            //        {


            //        }
            //    }
            //    //foreach(int )
            //    // ljtj
            //    // 量仪自动提交质检数据
            //    //HttpPost(Url+"ljtj", )
            //}
        }
        public HttpResponseResult HttpPostResponse(string url, string param)
        {
            Stream writer = null;
            StreamReader responsestream = null;
            HttpResponseResult httpRs = new HttpResponseResult();
            try
            {
                string strURL = url;
                System.Net.HttpWebRequest request;
                System.GC.Collect();//垃圾回收
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.KeepAlive = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.Accept = "*/*";
                request.Timeout = 20000;
                request.ReadWriteTimeout = 20000;
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                request.ServicePoint.Expect100Continue = false;
                //Stream writer = null;
                try
                {
                    writer = request.GetRequestStream();
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();

                    // 读取返回数据
                    WebResponse response = request.GetResponse();
                    responsestream = new StreamReader(response.GetResponseStream());
                    string result = responsestream.ReadToEnd();
                    responsestream.Close();
                    response.Close();
                    httpRs.IsSuccess = true;
                    httpRs.Response = result;
                    PrintSendDataLog(url + param);
                    return httpRs;
                }
                catch (WebException ex)
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    writer = null;
                    if (responsestream != null)
                    {
                        responsestream.Close();
                    }
                    responsestream = null;
                    request = null;

                    PrintLog(url + param + "http post webex ", ex);
                    //return string.Empty;
                }
                catch (Exception ex)
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    writer = null;
                    if (responsestream != null)
                    {
                        responsestream.Close();
                    }
                    responsestream = null;
                    request = null;
                    PrintLog(url + param + "http post ex", ex);
                    //return string.Empty;
                }
            }
            catch (Exception ex)
            {
                PrintLog(url + param + "http post f1 ex", ex);
            }
            finally
            {
                try
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    writer = null;
                    if (responsestream != null)
                    {
                        responsestream.Close();
                    }
                    responsestream = null;
                }
                catch (Exception ex)
                {
                }
            }
            httpRs.IsSuccess = false;
            return httpRs;
        }
        // 线程安全列表
        private ConcurrentBag<MeasuringInfo> ncInfoList = new ConcurrentBag<MeasuringInfo>();
        // 线程安全列表
        private ConcurrentBag<ResultInfo> ncResultList = new ConcurrentBag<ResultInfo>();
        private List<ResultInfo> GetResultInfoList()
        {
            List<ResultInfo> resultList = new List<ResultInfo>();
            if (ncInfoList != null)
            {

                while (true)
                {
                    try
                    {
                        MeasuringInfo ncInfo = null;
                        if (ncInfoList.TryTake(out ncInfo))
                        {
                            if (ncInfo != null)
                            {
                                Console.WriteLine("dtid : " + ncInfo.DtId);
                                ResultInfo result = new ResultInfo();
                                result.Result = ncInfo.Result;
                                result.ID = ncInfo.DtId;
                                resultList.Add(result);
                            }
                        }
                        else
                        {
                            //Console.WriteLine("=== GetNcResultInfoList ncInfo TryTake false ==========");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("=== GetResultInfoList Info ex ==========" + ex.Message);
                    }
                }
                // 之有未成功返回给平台的信息，再发送出去
                while (true)
                {
                    try
                    {
                        ResultInfo ncResultInfo = null;
                        if (ncResultList.TryTake(out ncResultInfo))
                        {
                            if (ncResultInfo != null)
                            {
                                Console.WriteLine("ResultInfo.dtid : " + ncResultInfo.ID + " " + ncResultInfo.Result);
                                resultList.Add(ncResultInfo);
                            }
                        }
                        else
                        {
                            //Console.WriteLine("=== GetNcResultInfoList ncResultInfo TryTake false ==========");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("=== GetResultInfoList ResultInfo ex ==========" + ex.Message);
                    }
                }

                //if (resultList.Count > 0)
                //{
                //    try
                //    {
                //        return JsonConvert.SerializeObject(resultList);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("=== GetNcResultInfoList JsonConvert ex ==========" + ex.Message);
                //    }
                //}
            }
            return resultList;
        }
        public void SendNcSuccess(AbstractModel model, MeasuringInfo info)
        {
            // 发送给平台，是否成功
            info.Result = 1;
            StringBuilder sb = new StringBuilder();
            sb.Append("dtid: ").Append(info.DtId).Append(" ResultCode: ").Append(info.Result.ToString())
                .Append(" ip: ").Append(info.Ip)
                .Append(" port: ").Append(info.Port.ToString())
                .Append(" data: ").Append(info.Data);
            PrintLog(sb.ToString(), null);
            ncInfoList.Add(info);

        }

        /// <summary>
        /// nc 传输失败
        /// </summary>
        /// <param name="model"></param>
        /// <param name="info"></param>
        public void SendNcFail(AbstractModel model, MeasuringInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("dtid: ").Append(info.DtId).Append(" ResultCode: ").Append(info.Result.ToString())
                .Append(" ip: ").Append(info.Ip)
                .Append(" port: ").Append(info.Port.ToString())
                .Append(" data: ").Append(info.Data);
            PrintLog(sb.ToString(), null);
            ncInfoList.Add(info);
        }
        //========================== http 发送 =========================
        /// <summary>
        /// http请求的发送
        /// </summary>
        /// <param name="url"> 接口地址</param>
        /// <param name="param"> 发送内容</param>
        /// <returns></returns>
        public bool HttpPost(string url, string param)
        {
            try
            {
                //PrintLine("httpPost ", true);
                //PrintLine(param, true);
                //// 服务模式下开启发送
                //if (!isService)/// || isService)
                //{
                //    if (DebugTools.isOutSendData(OutFlag))
                //    {
                //        PrintSendDataLine(param, true);
                //    }
                //    return true;
                //}
                //PrintSendDataLog(param);
                //PrintLine("httpPost ", true);
                string strURL = url;
                System.Net.HttpWebRequest request;
                System.GC.Collect();//垃圾回收
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.KeepAlive = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.Accept = "*";
                request.Timeout = 20000;
                request.ReadWriteTimeout = 20000;
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                request.ServicePoint.Expect100Continue = false;
                Stream writer = null;
                try
                {
                    writer = request.GetRequestStream();
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    PrintSendDataLog(param);
                    return true;
                }
                catch (WebException ex)
                {

                    if (writer != null)
                    {
                        writer.Close();
                    }
                    writer = null;
                    request = null;
                    PrintLog("http post webex", ex);
                    return false;
                }
                catch (Exception ex)
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    request = null;
                    PrintLog("http post ex", ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                PrintLog("http post f1 ex", ex);
            }
            return false;
        }
        public void PrintSendDataLog(string content, Exception ex = null)
        {
            Console.WriteLine("print line : ==>>>>>  " + content);
            if (ex != null)
            {
                Console.WriteLine("print line exception: ==>>>>>  " + ex.Message);
                LogHelper.WriteLog(content, ex);
            }
            else
            {
                LogHelper.WriteSendDataLog(content);
            }
        }
        private void timerDelLogs_Elapsed(object sender, ElapsedEventArgs e)
        {
            //delLogs(@"//Log/LogInfo");
            delLogs(@"//Log/LogSDInfo");
            delLogs(@"//Log/LogSysInfo");
            delLogs(@"//Log/LogError");
        }

        private void delLogs(string pathName)
        {
            string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            string dirPath = assemblyDirPath + pathName;// "//Log/LogInfo";
            DirectoryInfo df = new DirectoryInfo(dirPath);
            LogHelper.WriteSysLog("delLogs " + pathName);
            if (df.Exists)
            {
                //PringLogSys("line 2446", true);
                FileInfo[] fis = df.GetFiles();
                if (fis != null)
                {
                    DateTime now = DateTime.Now;
                    foreach (FileInfo fi in fis)
                    {
                        LogHelper.WriteSysLog(fi.Name + " hours " + (now - fi.LastWriteTime).TotalHours);
                        // 10天前的日志删除
                        if ((now - fi.LastWriteTime).TotalHours >= 240)
                        {
                            try
                            {
                                fi.Delete();
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteLog("del log exception ", ex);
                            }
                        }
                    }
                }
            }
        }
        //======================================== end http ==================================
    }
}
