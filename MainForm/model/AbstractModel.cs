using MainForm.controller;
using MainForm.infos;
using MainForm.serial;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MainForm.model
{
    public abstract class AbstractModel
    {
        private MainController controler;

        protected Timer timer;

        public AbstractModel(MainController controler)
        {
            this.controler = controler;
        }
        public abstract void Listener_SerialDataChangeListener(object sender, EventArgs e);

        private ConcurrentQueue<MeasuringInfo> queue = new ConcurrentQueue<MeasuringInfo>();

        public ConcurrentQueue<MeasuringInfo> Queue
        {
            get => queue; set => queue = value;
        }

        // 会存在多线程访问，所以用ncinfo.id作为key判断是否已经在处理，如已在处理，则不加入队例queue
        private ConcurrentDictionary<string, MeasuringInfo> dictionary = new ConcurrentDictionary<string, MeasuringInfo>();

        public ConcurrentDictionary<string, MeasuringInfo> Dictionary
        {
            get => dictionary;
            set => dictionary = value;
        }
        public abstract void Timer_Tick(object sender, EventArgs e);
        /// <summary>
        /// 连接机床
        /// </summary>
        /// <returns></returns>
        public abstract bool Connected();
        /// <summary>
        /// 是否通讯连接
        /// </summary>
        public bool IsConnected
        {
            get; set;
        }
        public MainController Controler
        {
            get
            {
                return controler;
            }
        }

        public SerialTcpClient SeriesTcpClient
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是正在运行
        /// </summary>
        public bool IsRun
        {
            get; set;
        }
     

        public void Start()
        {
            if (timer == null)
            {
                timer = new Timer();
                // 从队列中取传程序任务
                timer.Interval = 6000;
                //   Controler.PringLogSys("ttm " + timer.Interval, true);
                timer.Elapsed += Timer_Tick;
            }
            //IsRun = false;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            if (!timer.Enabled)
            {
                IsRun = false;
                timer.Enabled = true;
            }
        }
        /// <summary>
        /// 停止传输或断开连接
        /// </summary>
        public virtual void Stop()
        {
            IsRun = true;
            if (timer != null)
            {
                timer.Enabled = false;
            }
        }

        DateTime lastTime;
        public bool IsSendTimeEnable()
        {
            DateTime nn = DateTime.Now;
            // 前一次时间
            if (lastTime == null)
            {
                lastTime = nn;
                return true;
            }
            else
            {
                TimeSpan ts = nn - lastTime;
                if (Math.Abs(Math.Round(ts.TotalSeconds)) >= 120)
                {
                    
                    lastTime = nn;
                    return true;
                }
            }
            return false;
        }
    }
}
