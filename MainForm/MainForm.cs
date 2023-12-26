using MainForm.cbeans.listbox;
using MainForm.cfg;
using MainForm.controller;
using MainForm.loghelper;
using MainForm.serial;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainForm.controller.MainController;

namespace MainForm
{
    public partial class MainForm : CBaseForm
    {
        private MainController controller;
        public MainForm()
        {
            InitializeComponent();
            this.cbtClose.Click += new System.EventHandler(this.cCloseButton1_Click);
        }
        public override void cCloseButton1_Click(object sender, EventArgs e)
        {
           //if (eth_Conn) eth_disconnect();
            Application.Exit();
        }
        private void btStartListenerClick(object sender, EventArgs e)
        {
            if (controller != null)
            {
                if (controller.ClientList == null || controller.ClientList.Count == 0)
                {
                    richTextBox1.AppendText("请点击初始化串口信息，如果仍不对，请查看配置文件是否已存在。");
                    richTextBox1.AppendText("\n");
                    return;
                }
                //listBox1.Items.Clear();
                //// ip列表初始一下
                //int idx = 1;
                //foreach (SerialTcpClient client in controller.ClientList)
                //{
                //    string no = string.Format("{0:D4}", idx);
                //    CListBoxIpItem item = new CListBoxIpItem() { serialTcpClient = client, IP = client.Ip, Port = client.Port, No = no, Type = 0 };
                //    idx++;
                //    listBox1.Items.Add(item);
                //}
                ListBoxDataBind();
                controller.BeginConnect();

                controller.StartTimer();
            }
        }

        private void ListBoxDataBind()
        {
            listBox1.DataSource = null;
            //listBox1.Items.Clear();
            // ip列表初始一下
            int idx = 1;
            List<CListBoxIpItem> list = new List<CListBoxIpItem>();
            foreach (SerialTcpClient client in controller.ClientList)
            {
                string no = string.Format("{0:D4}", idx);
                CListBoxIpItem item = new CListBoxIpItem() { serialTcpClient = client, IP = client.Ip, Port = client.Port, No = no, Type = 0 };
                idx++;
                list.Add(item);
            }
            HashSet<CListBoxIpItem> hs = new HashSet<CListBoxIpItem>(list);
            BindingSource bs = new BindingSource();
            bs.DataSource = hs;
            listBox1.DataSource = bs;
            listBox1.DisplayMember = "Name";
        }

        private void btCloseListenerClick(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = true;
            this.btWinformRun.Enabled = true;
            this.btCloseListener.Enabled = false;
            if (controller != null)
            {
                controller.DisConnect();
                //controller.StopTimer();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = GetTitle();
                InitState();
                //controller = new MainController();
                //string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
                //string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
                //string ipFilePath = assemblyDirPath + "/cfg/syscfg.cfg";
                //SysCfgInfo sysCfgInfo = null;
                //using (StreamReader sr = new StreamReader(ipFilePath, Encoding.UTF8))
                //{
                //    string str = sr.ReadToEnd();// ReadLine();
                //    // PrintLog("js Url ===========  : " + URL_Str, true);
                //    sysCfgInfo = JsonConvert.DeserializeObject<SysCfgInfo>(str);

                //}
                //if (sysCfgInfo == null)
                //{
                //    controller.PrintLog("sys cfg is null:", null, true);
                //    return;
                //}
                //controller.PrintLog(sysCfgInfo.ToString(), null, true);
                ////controller = new MainController();
                //controller.SysCfg = sysCfgInfo;

                //controller.TcpClientChangedListener += Controller_TcpClientChangedListener;
                //controller.SerialDataChangeListener += Controller_SerialDataChangeListener;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               // PrintLog("form load", ex, true);
            }
        }
       // delegate void RefreshUIMsg(object data, SerialTcpClient serialTcpClient);
        private void Controller_TcpClientChangedListener(object data, SerialResult serialResult)
        {
            if (this.InvokeRequired)
            {
                TcpClientStateChangedHandler rf = new TcpClientStateChangedHandler(Controller_TcpClientChangedListener);
                this.BeginInvoke(rf, new object[] { data, serialResult });
                return;
            }
            else
            {
                RefreshUIMsg(data, serialResult);
            }
        }

        private void RefreshUIMsg(object data, SerialResult serialResult)
        {
            if (serialResult != null && serialResult.SerialClient != null)
            {
                SerialTcpClient client = serialResult.SerialClient;

                StringBuilder sb = new StringBuilder();
                sb.Append(client.Ip).Append(':').Append(client.Port).Append(' ').Append(serialResult.Result.ToString()).Append(' ');
                switch (serialResult.Result)
                {
                    case SerialResult.ResultType.Success:
                        sb.Append(' ').Append(data == null ? "" : data.ToString()).Append("\n");
                        break;
                    case SerialResult.ResultType.Success_Connect:
                        sb.Append("\n");
                        //listBox1.Refresh();
                        ListBoxDataBind();
                        break;
                    case SerialResult.ResultType.Success_CloseClient:
                        sb.Append("\n");
                        //listBox1.Refresh();
                        ListBoxDataBind();
                        break;
                    case SerialResult.ResultType.Success_Send:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Error_Ip:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Error_ReceiveCallBack:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Other:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Error:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Fail_Connect:
                        sb.Append("\n");
                        //listBox1.Refresh();
                        ListBoxDataBind();
                        break;
                    case SerialResult.ResultType.Error_Send:
                        sb.Append("\n");
                        break;
                    case SerialResult.ResultType.Exception:
                        sb.Append("\n");
                        break;
                }
                if (checkBox1.Checked)
                    richTextBox1.AppendText(sb.ToString());
            }
        }
        //ConcurrentBag<int> list = new ConcurrentBag<int>();
        //System.Timers.Timer timer1 = new System.Timers.Timer();
        //System.Timers.Timer timer2 = new System.Timers.Timer();
        private void button3_Click(object sender, EventArgs e)
        {
            //if (ckService.Checked)
            //{


            //    return;
            //}
            bool isService = radioButton2.Checked;

            bool isWinform = radioButton1.Checked;
            this.groupBox1.Enabled = false;
            if (isService)
            {
                this.btWinformRun.Enabled = false;
                //this.btStopServer.Enabled = true;
                StartServer();
                return;
            }
            if (controller == null)
            {
                try
                {
                   
                    controller = new MainController();
                    //string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
                    //string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
                    //string ipFilePath = assemblyDirPath + "/cfg/syscfg.cfg";
                    //SysCfgInfo sysCfgInfo = null;
                    //using (StreamReader sr = new StreamReader(ipFilePath, Encoding.UTF8))
                    //{
                    //    string str = sr.ReadToEnd();// ReadLine();
                    //                                // PrintLog("js Url ===========  : " + URL_Str, true);
                    //    sysCfgInfo = JsonConvert.DeserializeObject<SysCfgInfo>(str);

                    //}
                    //if (sysCfgInfo == null)
                    //{
                    //    controller.PrintLog("sys cfg is null:", null, true);
                    //    return;
                    //}
                    //controller.PrintLog(sysCfgInfo.ToString(), null, true);
                    ////controller = new MainController();
                    //controller.SysCfg = sysCfgInfo;

                    controller.TcpClientChangedListener += Controller_TcpClientChangedListener;
                    //controller.SerialDataChangeListener += Controller_SerialDataChangeListener;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // PrintLog("form load", ex, true);
                }
            }
            if (controller != null)
            {                
                controller.InitSerialTcpClient();
                btStartListenerClick(null, e);
            }

            this.btWinformRun.Enabled = false;
            this.btCloseListener.Enabled = true;
            //ConcurrentBagWithPallel();
            //timer1.Interval = 500;
            //timer1.Elapsed += Timer1_Elapsed;
            //timer2.Interval = 500;
            //timer2.Elapsed += Timer1_Elapsed2;
            //timer1.Enabled = true;
            //timer2.Enabled = true;
        }

        //private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    ConcurrentBagWithPallel();
        //}
        //private void Timer1_Elapsed2(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    ConcurrentBagWithPallel2();
        //}
        //public void ConcurrentBagWithPallel2()
        //{
        //    Console.WriteLine("add  ConcurrentBag'----- ");
        //    //Parallel.For(0, 10, item =>
        //    //{
        //    //    list.Add(item);
        //    //});
        //    for (int i = 0; i < 10; i++)
        //    {
        //        //System.Console.WriteLine("ThreadWork1 producer: " + i);
        //        list.Add(i);
        //    }

        //    Console.WriteLine("add  ConcurrentBag's count is {0} max {1}",list.Count, list.Max());
        //}
        //public void ConcurrentBagWithPallel()
        //{
        //    Console.WriteLine("==================  read ");
        //    //ConcurrentBag<int> list = new ConcurrentBag<int>();
        //    //Parallel.For(0, 10000, item =>
        //    //{
        //    //    list.Add(item);
        //    //});
        //    //Console.WriteLine("ConcurrentBag's count is {0}", list.Count());
        //    //int n = 0;
        //    //foreach (int i in list)
        //    //{
        //    //    if (n > 10)
        //    //        break;
        //    //    n++;
        //    //    //Console.WriteLine("Item[{0}] = {1}", n, i);
        //    //}
        //    // Console.WriteLine("ConcurrentBag's max item is {0}", list.Max());
        //    int idx = 0;
        //    //foreach (int ii in list)
        //    //{
        //    //   // Console.WriteLine(ii);
        //    //}
        //    while (!list.IsEmpty)
        //    {
        //        int i;
        //        //list.TryPeek(out i);
        //        list.TryTake(out i);
        //        //Console.WriteLine("a {0}",  i);
        //        //if (i > 9000)
        //        //{
        //        //    Console.WriteLine("xdx {0}", idx);
        //        //    break;
        //        //}
        //        //idx++;
        //        Console.WriteLine("x {0} {1}", idx, i);
        //        idx++;
        //    }
        //    // Console.WriteLine("xxxxx {0}", list.Max());
        //    Console.WriteLine("xxxxx  read ");
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            if (controller != null)
            {
                controller.StopTimer();
            }
        }

        public string GetTitle()
        {
            return @"数据采集软件";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                CListBoxIpItem item = listBox1.SelectedItem as CListBoxIpItem;
                if (item != null && item.serialTcpClient != null)
                {
                    if (!string.IsNullOrEmpty(textBox2.Text))
                        item.serialTcpClient.SendNc(textBox2.Text);
                }
                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == radioButton1 || sender == radioButton2)
            {
                // bool isService = radioButton2.Checked;

                // bool isWinform = radioButton1.Checked;

                //// this.btWinformRun.Enabled = isWinform;
                // this.btAutoConn.Enabled = isWinform;
                // this.btStartListener.Enabled = isWinform;
                // this.btCloseListener.Enabled = isWinform;
                // this.btDtSend.Enabled = isWinform;

                // this.btInstallService.Enabled = isService;
                // this.btUninstallService.Enabled = isService;

                InitState();
            }
        }

        private void InitState()
        {
            bool isService = radioButton2.Checked;

            bool isWinform = radioButton1.Checked;

            // this.btWinformRun.Enabled = isWinform;
            this.btAutoConn.Enabled = isWinform;
            this.btStartListener.Enabled = isWinform;
            this.btCloseListener.Enabled = isWinform;
            this.btDtSend.Enabled = isWinform;

            this.btInstallService.Enabled = isService;
            this.btUninstallService.Enabled = isService;
            this.btStopServer.Enabled = isService;
            if (isWinform)
            {

                this.btCloseListener.Enabled = false;
            }
            //if (isService)
            //{
            //    this.btStopServer.Enabled = false;
            //}
        }

        //================================== service 处理 ==============================================

        private void StartServer()
        {
            if (this.IsServiceExisted(serviceName))
                this.ServiceStart(serviceName);
            richTextBox1.AppendText("服务启动完成\n");
        }
        private void btStopServer_Click(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = true;
            this.btWinformRun.Enabled = true;
            //this.btStopServer.Enabled = false;

            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                richTextBox1.AppendText("服务已停止\n");
                //this.label1.Text = "服务已停止";
            }
            else
            {
                richTextBox1.AppendText("服务已停止\n");
                //this.label1.Text = "服务已停止";
            }
        }

        string serviceFilePath = Application.StartupPath + Path.DirectorySeparatorChar + "IOEDTService.exe";
        string serviceName = "IOEDTService";
        private void btInstallService_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                richTextBox1.AppendText("正在安装服务\n");
                if (this.IsServiceExisted(serviceName))
                {
                    richTextBox1.AppendText("服务安装完成，请启动服务\n");
                    this.Cursor = this.DefaultCursor;
                    return;
                }
                    //this.UninstallService(serviceFilePath);
                this.InstallService(serviceFilePath);

                richTextBox1.AppendText("服务安装完成，请启动服务\n");
               // this.label1.Text = "服务安装完成，请启动服务";
            }
            catch (Exception ex)
            {
                //this.label1.Text = "请以管理员身份运行程序";
                richTextBox1.AppendText("请以管理员身份运行程序" + ex.Message + "\n");
            }
            this.Cursor = this.DefaultCursor;
        }
        //判断服务是否存在
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }
        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
            }
        }

        private void btUninstallService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            richTextBox1.AppendText("正在卸载服务\n");
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                this.UninstallService(serviceFilePath);

                richTextBox1.AppendText("服务安装完成\n");
                //this.label1.Text = "服务卸载完成";
            }
            else
            {
                richTextBox1.AppendText("未安装服务，不需要卸载\n");
                //this.label1.Text = "未安装服务，不需要卸载";
            }
            this.Cursor = this.DefaultCursor;
        }

        //启动服务
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        //停止服务
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientExit();
        }
        private void clientExit()
        {
            Application.Exit();
        }

        internal class cbeans
        {
        }
    }
}
