using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.Threading.Tasks;

namespace UNICLog
{
    public class ULog
    {
        /// <summary>
        /// 调用ini之前必须初始化的变量
        /// </summary>
        public TimeSpan todel_time = new TimeSpan();
        public string Path = Application.StartupPath;

        public enum cengci
        {
            error, debug
        }

        public static System.Threading.Timer del_tick_timer;

        public void INI(string path, TimeSpan time)
        {
            todel_time = time;
            Path = path;
            // int p = RunningInstance_int();
            Process process = RunningInstance();
            if (process != null)
            {
                if (MessageBox.Show("此应用程序已经启动！") == DialogResult.OK)
                {
                    WriteLog("已经启动");
                    System.Environment.Exit(1);

                }
                return;
            }
            else
            {

            }
            if (todel_time.TotalMilliseconds >= 1000)
                del_tick_timer = new System.Threading.Timer(call, null, todel_time, new TimeSpan(0));

        }
        ~ULog()
        {
            dispose();
        }
        public void dispose()
        {
            if (del_tick_timer != null)
                del_tick_timer.Change(-1, -1);
            del_tick_timer = null;
        }

        private void call(object state)
        {
            //把程序下所有创建时间早的都删除不是什么好主意。。
            //  DelAll(Path, todel_time);
        }
        public static object WriteLogObj = new object();


        public static void writeLogF(cengci c, string strContent)
        {
            WriteLog(strContent);//把所有的log写到一个文件，有助于判断

            lock (WriteLogObj)
            {
                string path = Application.StartupPath + "\\Log" + c.ToString() + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string strTemp = DateTime.Now.ToShortDateString();
                strTemp = strTemp.Replace('/', '-');
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path + "\\" + c.ToString() + strTemp + ".txt", true);
                sw.Write(DateTime.Now.ToString() + ":  " + strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }

        }
        public static void WriteLog(string strContent)
        {
            TempStrings.Add(strContent);
            if (TempStrings.Count > 100)
            {
                TempStrings.Clear();
            }

        }
        public static System.Threading.Timer Saver = new System.Threading.Timer(call_save, null, 1000, 1000);

        
        private static void call_save(object state)
        {
            lock (WriteLogObj)
            {
                WriteLogR(TempStrings);
                TempStrings.Clear();
            }

        }
        public static List<string> TempStrings = new List<string>();
        //写日志

        private static void WriteLogR(List<string> ob)
        {
            try {
                if (!System.IO.Directory.Exists(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2")))
                {
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2"));
                }
                string strTemp = DateTime.Now.ToShortDateString();
                strTemp = strTemp.Replace('/', '-');
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2") + "\\MachineLog" + strTemp + ".txt", true);
                //  Log.Trace(strContent);
                // lock (WriteLogObj)
                for (int i = 0; i < ob.Count; i++)
                {

                    sw.Write(DateTime.Now.ToString() + ":  " + ob[i] + "\r\n");

                }

                sw.Flush();
                sw.Close();

            }catch(Exception ex){
            
            }
                    }

        private static void WriteLogR(string strContent)
        {

            //  Log.Trace(strContent);
            // lock (WriteLogObj)
            {
                if (!System.IO.Directory.Exists(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2")))
                {
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2"));
                }
                string strTemp = DateTime.Now.ToShortDateString();
                strTemp = strTemp.Replace('/', '-');
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2") + "\\MachineLog" + strTemp + ".txt", true);
                sw.Write(DateTime.Now.ToString() + ":  " + strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
        }

        public static string webpaht = "";
        public static void WEb_WriteLog(string strContent)
        {
            //  Log.Trace(strContent);
            lock (WriteLogObj)
            {
                if (!System.IO.Directory.Exists(webpaht + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2")))
                {
                    System.IO.Directory.CreateDirectory(webpaht + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2"));
                }
                string strTemp = DateTime.Now.ToShortDateString();
                strTemp = strTemp.Replace('/', '-');
                System.IO.StreamWriter sw = new System.IO.StreamWriter(webpaht + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2") + "\\MachineLog" + strTemp + ".txt", true);
                sw.Write(DateTime.Now.ToString() + ":  " + strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
        }

        public static int RunningInstance_int()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            return
                processes.Length;
        }

        public static Process RunningInstance()
        {

            Process current = Process.GetCurrentProcess();

            WriteLog(current.ProcessName);
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop   through   the   running   processes   in   with   the   same   name   
            foreach (Process process in processes)
            {
                //Ignore   the   current   process   
                if (process.Id != current.Id)
                {
                    //Make   sure   that   the   process   is   running   from   the   exe   file.   
                    string pathtt = Assembly.GetExecutingAssembly().Location.Replace("/", "\\");

                    int las_dex = pathtt.LastIndexOf("\\");

                    if (las_dex > 0)
                        pathtt = pathtt.Substring(0, las_dex);

                    WriteLog(pathtt + "+++++++" + current.MainModule.FileName);
                    if (current.MainModule.FileName.StartsWith(pathtt))
                    {
                        //Return   the   other   process   instance.   
                        return process;
                    }
                }
            }
            //No   other   instance   was   found,   return   null. 
            return null;
        }
        private static void HandleRunningInstance(Process instance)
        {
            //Make   sure   the   window   is   not   minimized   or   maximized   
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            //Set   the   real   intance   to   foreground   window
            SetForegroundWindow(instance.MainWindowHandle);
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;

        public static void DelAll(string Path, TimeSpan pass)
        {
            lock (WriteLogObj)
            {
                try
                {

                    DirectoryInfo dirInfo = new DirectoryInfo(Path);
                    FileInfo[] files = dirInfo.GetFiles();   // 获取该目录下的所有文件
                    foreach (FileInfo file in files)
                    {
                        if (file.CreationTime.Add(pass) < DateTime.Now)
                        {
                            file.Delete();

                        }
                    }

                    DirectoryInfo[] directorys = dirInfo.GetDirectories();
                    foreach (DirectoryInfo d in directorys)
                    {
                        DelAll(d.FullName, pass);
                    }
                    directorys = dirInfo.GetDirectories();

                    files = dirInfo.GetFiles();
                    if (files.Length == 0 && directorys.Length == 0)//如果目录下没东西，删除所有的
                    {
                        if (dirInfo.CreationTime.Add(pass) < DateTime.Now)
                        {
                            dirInfo.Delete();
                        }
                    }


                }
                catch (Exception e)
                {


                }
            }
        }
        public static void WriteLogDirect(string strContent)
        {
            try
            {
                if (!System.IO.Directory.Exists(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2")))
                {
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2"));
                }
                string strTemp = DateTime.Now.ToShortDateString();
                strTemp = strTemp.Replace('/', '-');
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "\\Log" + "\\" + DateTime.Now.Year.ToString("d4") + DateTime.Now.Month.ToString("d2") + "\\MachineLog" + strTemp + ".txt", true);
                //  Log.Trace(strContent);
                // lock (WriteLogObj)
                sw.Write(DateTime.Now.ToString() + ":  " + strContent + "\r\n");

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
