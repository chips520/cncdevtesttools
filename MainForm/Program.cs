using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MainForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process process = getRunningProcess();
            if (process != null)
            {
                try
                {
                    HandleRunningInstance(process);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return;
            }
            Application.Run(new MainForm());
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;//1.normal   2.minimized  3.maximized
        private static void HandleRunningInstance(Process process)
        {
            ShowWindowAsync(process.MainWindowHandle, WS_SHOWNORMAL);
            SetForegroundWindow(process.MainWindowHandle);
        }
        private static Process getRunningProcess()
        {
            Process current = Process.GetCurrentProcess();
            Process[] cs = Process.GetProcessesByName(current.ProcessName);
            foreach (Process p in cs)
            {
                //MessageBox.Show("process ============== " + p.ProcessName + ":" + p.Id + ":" + current.Id);
                if (p.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return p;
                    }
                }
            }
            return null;
        }
    }
}
