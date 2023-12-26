using MainForm.controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IOEDTService
{
    public partial class Service1 : ServiceBase
    {
        private MainController mainControl;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (mainControl == null)
            {
                mainControl = new MainController();
            }
            mainControl.PrintLog("OnStart ", null);
            mainControl.InitSerialTcpClient();
            mainControl.BeginConnect();

            mainControl.StartTimer();
        }

        protected override void OnStop()
        {
            mainControl.PrintLog("OnStop ", null);
            mainControl.StopTimer();
        }
    }
}
