using MainForm.controller;
using MainForm.infos;
using MainForm.serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MainForm.serial.SerialResult;

namespace MainForm.model
{
    /// <summary>
    /// 
    /// </summary>
    public class DtModel : AbstractModel
    {
        public DtModel(MainController controller) : base(controller)
        {

        }

        public override bool Connected()
        {
            if (SeriesTcpClient != null)
            {
                IsConnected = SeriesTcpClient.ClientConnected;
            }
            return IsConnected;
        }
        private MeasuringInfo FindModel(string dtid)
        {
            if (tmpList != null)
            {
                // ip、 
                return tmpList.Find(c => c != null && c.DtId.ToString() == dtid);
            }
            return null;
        }
        public override void Listener_SerialDataChangeListener(object sender, EventArgs e)
        {
            if (sender != null && sender is SerialResult)
            {
                SerialResult sr = sender as SerialResult;
                if (sr.SerialClient != this.SeriesTcpClient)
                {
                    return;
                }

                if (sr.Result == ResultType.Success)
                {
                    string ncContent = sr.Data.ToString();
                    // 收到dt消息
                    if (!string.IsNullOrEmpty(ncContent))
                    {

                        Controler.PrintLog("msg dt Receive : " + ncContent, null);
                        // 发送到平台
                        //DT2&dtid=88
                        // 收到dt端已弹出界面的消息
                        if (ncContent.ToLower().StartsWith("ioemsg#show"))
                        {
                            string[] tmpA = ncContent.Split(new char[] { '#'});
                            if (tmpA != null && tmpA.Length == 3)
                            {
                                string dtid = tmpA[2];
                                MeasuringInfo tmpInfo = FindModel(dtid);
                                if (tmpInfo != null)
                                {
                                    Controler.SendNcSuccess(this, tmpInfo);
                                    tmpList.Remove(tmpInfo);
                                }
                            }
                            // ok 正常弹出
                            Controler.PrintLog(" ok 正常弹出" + ncContent, null);
                        }
                        else if (ncContent.ToLower().StartsWith("ioemsg#hide"))
                        {
                            // ok 正常弹出
                            Controler.PrintLog(" ok 正常隐藏" + ncContent, null);
                        }
                        else if (ncContent.ToLower().StartsWith("err#"))
                        {
                            // 弹出错误
                            Controler.PrintLog("fail 弹出错误" + ncContent, null);
                        }
                        else if (ncContent.ToLower().StartsWith("dt"))
                        {
                            // 返回DT
                            Controler.PrintLog("返回DT" + ncContent, null);
                            // 江森
                            //if (this.SeriesTcpClient.Ip == "10.126.221.243")
                            //{
                            //    Controler.PrintLog("win95 不发送关闭DT 因为系统收不到具体内容 " , null);
                            //}
                            //else
                            //{
                            SerialResult result = this.SeriesTcpClient.SendData("dtrecok");
                            if (result.Result == ResultType.Success_Send)
                            {
                                Controler.PrintLog("发送确认可关闭DT " + result.Msg, null);
                            }
                            else
                            {
                                Controler.PrintLog("发送确认可关闭DT 失败 " + result.Msg + result.Data, null);
                            }
                            // "DT" & index + 1 & "&dtid=" & dtid
                            string[] tmpA = ncContent.Split(new char[] { '&' });
                            if (tmpA != null && tmpA.Length == 2)
                            {
                                string dtidStr = tmpA[1];
                                string[] tmpB = ncContent.Split(new char[] { '=' });
                                //MeasuringInfo tmpInfo = FindModel(dtid);
                                if (tmpB != null && tmpB.Length == 2)
                                {
                                    string dtid = tmpB[1];

                                    ResultInfo rfInfo = new ResultInfo();
                                    rfInfo.ID = Convert.ToInt64(dtid);
                                    rfInfo.Reason = ncContent;
                                    Controler.addDtRs(rfInfo);
                                }
                            }
                            //}

                            // 发送给平台 zzz

                        }
                        //
                        // 解析数据
                       // AnalyzeData(ncContent, sr);
                        //PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    }
                }
                else if (sr.Result == ResultType.Success_Send)
                {
                    Controler.PrintLog("msg dt Success_Send: " + this.SeriesTcpClient.ToString(), null);
                    //if (tcpClientStateChangedHandler != null)
                    //{
                    //    tcpClientStateChangedHandler("", sr);
                    //}
                    //PrintLog(sr.SerialClient.ToString() + " " + sr.Msg, null);
                    // mainForm.SerialSendActived(sr.SerialClient, sr);
                    // PrintLog(sr.Msg);
                }
            }
        }
        List<MeasuringInfo> tmpList = new List<MeasuringInfo>();
        public override void Timer_Tick(object sender, EventArgs e)
        {
            Controler.PrintLog("time tick " , null);
            if (Connected())
            {


                Controler.PrintLog("time tick connected ", null);
                if (IsRun || !IsSendTimeEnable())
                {

                    Controler.PrintLog("time tick connected 159 line ", null);
                    return;
                }

                Controler.PrintLog("time tick connected 161 line ", null);
                IsRun = true;
                MeasuringInfo ncInfo = null;
                try
                {
                    // 队列中取一个任务
                    if (Queue.TryDequeue(out ncInfo))
                    {

                        Controler.PrintLog("time tick connected 172 line ", null);
                        if (ncInfo != null)
                        {
                            //MeasuringInfo nc;
                            //Dictionary.TryRemove(ncInfo.Ip, out nc);
                            // 下载nc程序，临时保存
                            //string content = GetNcContent(ncInfo);
                            if (SeriesTcpClient != null)
                            {
                                // 发送DT消息到机床端
                                SerialResult r = SeriesTcpClient.SendData(ncInfo.Data);
                                if (r.Result == SerialResult.ResultType.Success_Send)
                                {
                                    // 送到DT端了
                                    r.Info = ncInfo;
                                    // Controler.SendNcSuccess(this, ncInfo);
                                    tmpList.Add(ncInfo);
                                }
                                else
                                {
                                    // 未发送成功的，返给平台消息
                                    r.Info = ncInfo;
                                    Controler.SendNcFail(this, ncInfo);
                                    //
                                    // Queue.Enqueue(ncInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                // 这里断开，fanuc handler 
                ///EthDisconnect();
                IsRun = false;
            }
        }
    }
}
