using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace MainForm.tools
{
    public class MyTools
    {
        /// <summary>
        /// 读取文本内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadContent(string filePath)
        {
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs);

                string content = reader.ReadToEnd();
                reader.Close();
                fs.Close();
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadContent " + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 保存文本内容
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SaveContent(string savePath, string content)
        {
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite);
                writer = new StreamWriter(fs);
                writer.Write(content);
                writer.Flush();
                writer.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveContent " + ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return false;
        }

        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (!string.IsNullOrEmpty(strInfo) && strInfo.ToLower() == "success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
