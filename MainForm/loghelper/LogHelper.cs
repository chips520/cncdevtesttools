using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MainForm.loghelper
{
    public class LogHelper
    {
        //public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static readonly log4net.ILog logSendData = log4net.LogManager.GetLogger("logSd");

        public static readonly log4net.ILog logSys = log4net.LogManager.GetLogger("logSys");

        //public static readonly log4net.ILog logServerr = log4net.LogManager.GetLogger("logServer");

        public static void InitLog4Net()
        {
            string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            string configFilePath = assemblyDirPath + "//log4net.config";
            DOMConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));
        }

        //public static void WriteLog(string info)
        //{

        //    if (loginfo.IsInfoEnabled)
        //    {
        //        loginfo.Info(info);
        //    }
        //}

        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }

        public static void WriteSendDataLog(string info)
        {
            if (logSendData.IsInfoEnabled)
            {
                logSendData.Info(info);
            }
        }

        public static void WriteSysLog(string info)
        {
            if (logSys.IsInfoEnabled)
            {
                logSys.Info(info);
            }
        }

        //public static void WriteServerLog(string info, Exception ex)
        //{
        //    if (logServerr.IsInfoEnabled)
        //    {
        //        if (ex != null)
        //        {
        //            logServerr.Error(info, ex);
        //        }
        //        else
        //        {
        //            logServerr.Info(info);
        //        }
        //    }
        //}
    }
}
