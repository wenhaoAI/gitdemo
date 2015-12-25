using Displays;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledhm
{

       

    public enum AppState
    {
        Starting, Started, Stoping, Stoped, Error
    }
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, StringBuilder lParam);
        const int WM_GETTEXT = 0x000D;


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        DbSession dbsession = new DbSession("MySqlConn");

    
        Displays.LEDDisplay LEDDisplay;
        int flag = -1;//班次状态
        int teep = 0;//期望产量
        int type =1;
        int recordcount = 0;
        
        //int thtotal = 0;
        
        DateTime thtime;
        double cycle = 1;
        string bott = "";
        string pro = "";
        OleDbConnection connection;
        OleDbDataAdapter command;
        string qp = ConfigurationManager.AppSettings["qp"];
        public Form1()
        {
            InitializeComponent();
        }
    
        private bool sendtoled() {
            LEDDisplay.TNetParam NetParam;
            // 网络参数
            NetParam.IPAddr = ConfigurationManager.AppSettings["ipaddress"];
            NetParam.Password = "";
            NetParam.UDPPort = 27694;
            // 显示屏参数
            LEDDisplay = new LEDDisplay(); // 新建显示屏
            LEDDisplay.bUse = false;
            LEDDisplay.SCL2008 = true;
            LEDDisplay.DevID = short.Parse( ConfigurationManager.AppSettings["ledid"].ToString());
            LEDDisplay.Net = false;
            LEDDisplay.NetParam = NetParam;
            LEDDisplay.Timeout = 2;
            LEDDisplay.Retry = 4;
            LEDDisplay.Width = 128;
            LEDDisplay.Height = 128;        	

            bool bOK = false;
            if(!LEDDisplay.bUse){
                bOK = LEDDisplay.Open();
                if(bOK){                  
                    //DisplayController.SCL2008.TextInfoType TextPro;
                    //TextPro.Left_ = 4046;
                    //TextPro.Top_ = 4;
                    //TextPro.Width_ = 50;
                    //TextPro.Height_ = 15;
                    //TextPro.Color = 0x00FF00;
                    //TextPro.ASCFont = 1;
                    //TextPro.HZFont = 1;
                    //TextPro.XPos = 0;
                    //TextPro.YPos = 0;
                  
                    //DisplayController.SCL2008.TextInfoType TextTeep;
                    //TextTeep.Left_ = 4050;
                    //TextTeep.Top_ = 30;
                    //TextTeep.Width_ = 46;
                    //TextTeep.Height_ = 15;
                    //TextTeep.Color = 0x00FF00;
                    //TextTeep.ASCFont = 1;
                    //TextTeep.HZFont = 1;
                    //TextTeep.XPos = 0;
                    //TextTeep.YPos = 0;
                    DisplayController.SCL2008.TextInfoType TextGood;
                    TextGood.Left_ = 4050;
                    TextGood.Top_ = 48;
                    TextGood.Width_ = 46;
                    TextGood.Height_ = 15;
                    TextGood.Color = 0x00FF00;
                    TextGood.ASCFont = 1;
                    TextGood.HZFont = 1;
                    TextGood.XPos = 0;
                    TextGood.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextCap;
                    TextCap.Left_ = 4050;
                    TextCap.Top_ = 70;
                    TextCap.Width_ = 46;
                    TextCap.Height_ = 15;
                    TextCap.Color = 0x00FF00;
                    TextCap.ASCFont = 1;
                    TextCap.HZFont = 1;
                    TextCap.XPos = 0;
                    TextCap.YPos = 0;

                    //DisplayController.SCL2008.TextInfoType TextBott;
                    //TextBott.Left_ = 4050;
                    //TextBott.Top_ = 90;
                    //TextBott.Width_ = 46;
                    //TextBott.Height_ = 15;
                    //TextBott.Color = 0x00FF00;
                    //TextBott.ASCFont = 1;
                    //TextBott.HZFont = 1;
                    //TextBott.XPos = 0;
                    //TextBott.YPos = 0;

                    //DisplayController.SCL2008.TextInfoType TextTime;
                    //TextTime.Left_ = 4050;
                    //TextTime.Top_ = 122;
                    //TextTime.Width_ = 46;
                    //TextTime.Height_ = 15;
                    //TextTime.Color = 0x00FF00;
                    //TextTime.ASCFont = 1;
                    //TextTime.HZFont = 1;
                    //TextTime.XPos = 0;
                    //TextTime.YPos = 0;

                  //  string[] strs = count.Split(',');
                    if(LEDDisplay.bUse){
                      //  bOK = LEDDisplay.ShowString(LEDDisplay.DevID, TextPro, proname) && LEDDisplay.ShowString(LEDDisplay.DevID, TextTeep,  teep.ToString()) && LEDDisplay.ShowString(LEDDisplay.DevID, TextGood,  strs[2].ToString())&&LEDDisplay.ShowString(LEDDisplay.DevID,TextCap,(teep-int.Parse(strs[2])).ToString());
                        UNICLog.ULog.WriteLog(recordcount.ToString() + "recordcount");
                        bOK = LEDDisplay.ShowString(LEDDisplay.DevID, TextGood, recordcount.ToString().PadLeft(7, ' ')) && LEDDisplay.ShowString(LEDDisplay.DevID, TextCap, (teep - recordcount).ToString().PadLeft(7, ' '));
                        
                    }
                   bOK =  LEDDisplay.Close();
                }
            }

            return bOK;
        }

        private void sendpro(string proname,string thqp) { 
             LEDDisplay.TNetParam NetParam;
            // 网络参数
            NetParam.IPAddr = ConfigurationManager.AppSettings["ipaddress"];
            NetParam.Password = "";
            NetParam.UDPPort = 27694;
            // 显示屏参数
            LEDDisplay = new LEDDisplay(); // 新建显示屏
            LEDDisplay.bUse = false;
            LEDDisplay.SCL2008 = true;
            LEDDisplay.DevID = short.Parse( ConfigurationManager.AppSettings["ledid"].ToString());
            LEDDisplay.Net = false;
            LEDDisplay.NetParam = NetParam;
            LEDDisplay.Timeout = 2;
            LEDDisplay.Retry = 4;
            LEDDisplay.Width = 128;
            LEDDisplay.Height = 128;        	

            bool bOK = false;

            if (!LEDDisplay.bUse)
            {
                #region
                bOK = LEDDisplay.Open();
                if (bOK)
                {
                    DisplayController.SCL2008.TextInfoType Textqp;
                    Textqp.Left_ = 3970;
                    Textqp.Top_ = 4;
                    Textqp.Width_ = 30;
                    Textqp.Height_ = 15;
                    Textqp.Color = 0x00FF00;
                    Textqp.ASCFont =3;
                    Textqp.HZFont = 2;
                    Textqp.XPos = 0;
                    Textqp.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextPro;
                    TextPro.Left_ = 4000;
                    TextPro.Top_ = 4;
                    TextPro.Width_ = 80;
                    TextPro.Height_ = 15;
                    TextPro.Color = 0x00FF00;
                    TextPro.ASCFont = 1;
                    TextPro.HZFont = 1;
                    TextPro.XPos = 0;
                    TextPro.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextTeep;
                    TextTeep.Left_ = 4050;
                    TextTeep.Top_ = 30;
                    TextTeep.Width_ = 46;
                    TextTeep.Height_ = 15;
                    TextTeep.Color = 0x00FF00;
                    TextTeep.ASCFont = 1;
                    TextTeep.HZFont = 1;
                    TextTeep.XPos = 0;
                    TextTeep.YPos = 0;


                    DisplayController.SCL2008.TextInfoType TextBott;
                    TextBott.Left_ = 4050;
                    TextBott.Top_ = 93;
                    TextBott.Width_ = 46;
                    TextBott.Height_ = 15;
                    TextBott.Color = 0x00FF00;
                    TextBott.ASCFont = 1;
                    TextBott.HZFont = 1;
                    TextBott.XPos = 0;
                    TextBott.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextTime;
                    TextTime.Left_ = 4050;
                    TextTime.Top_ = 113;
                    TextTime.Width_ = 46;
                    TextTime.Height_ = 15;
                    TextTime.Color = 0x00FF00;
                    TextTime.ASCFont = 1;
                    TextTime.HZFont = 1;
                    TextTime.XPos = 0;
                    TextTime.YPos = 0;


                    if (LEDDisplay.bUse)
                    {
                        
                        bOK = LEDDisplay.ShowString(LEDDisplay.DevID, TextPro, proname.PadLeft(12,' '));
                        bOK= LEDDisplay.ShowString(LEDDisplay.DevID, Textqp, thqp.ToUpper().PadLeft(4, ' '));
                       
                        teep =(int) Math.Round(((System.DateTime.Now - thtime).TotalSeconds)/cycle);
                        //string oae =teep .ToString().PadLeft(7,' ');
                        //bott = bott.PadLeft(7,' ');
                        UNICLog.ULog.WriteLog(teep+"teep"+thtime.ToString());
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextTeep, teep.ToString().PadLeft(7, ' '));
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextBott, bott.PadLeft(6, ' '));
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextTime, cycle.ToString().PadLeft(7, ' '));
                        
                    
                    }
                    bOK = LEDDisplay.Close();
                #endregion
                }
            }
        }

        private void sendstop() {
            LEDDisplay.TNetParam NetParam;
            // 网络参数
            NetParam.IPAddr = ConfigurationManager.AppSettings["ipaddress"];
            NetParam.Password = "";
            NetParam.UDPPort = 27694;
            // 显示屏参数
            LEDDisplay = new LEDDisplay(); // 新建显示屏
            LEDDisplay.bUse = false;
            LEDDisplay.SCL2008 = true;
            LEDDisplay.DevID = short.Parse(ConfigurationManager.AppSettings["ledid"].ToString());
            LEDDisplay.Net = false;
            LEDDisplay.NetParam = NetParam;
            LEDDisplay.Timeout = 2;
            LEDDisplay.Retry = 4;
            LEDDisplay.Width = 128;
            LEDDisplay.Height = 128;

            bool bOK = false;
            if (!LEDDisplay.bUse)
            {
                bOK = LEDDisplay.Open();
                if (bOK)
                {
                     DisplayController.SCL2008.TextInfoType Textqp;
                    Textqp.Left_ = 3970;
                    Textqp.Top_ = 4;
                    Textqp.Width_ = 20;
                    Textqp.Height_ = 15;
                    Textqp.Color = 0x00FF00;
                    Textqp.ASCFont = 1;
                    Textqp.HZFont = 1;
                    Textqp.XPos = 0;
                    Textqp.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextPro;
                    TextPro.Left_ = 3996;
                    TextPro.Top_ = 4;
                    TextPro.Width_ = 100;
                    TextPro.Height_ = 15;
                    TextPro.Color = 0x00FF00;
                    TextPro.ASCFont = 1;
                    TextPro.HZFont = 1;
                    TextPro.XPos = 0;
                    TextPro.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextTeep;
                    TextTeep.Left_ = 4050;
                    TextTeep.Top_ = 30;
                    TextTeep.Width_ = 46;
                    TextTeep.Height_ = 15;
                    TextTeep.Color = 0x00FF00;
                    TextTeep.ASCFont = 1;
                    TextTeep.HZFont = 1;
                    TextTeep.XPos = 0;
                    TextTeep.YPos = 0;


                    DisplayController.SCL2008.TextInfoType TextBott;
                    TextBott.Left_ = 4050;
                    TextBott.Top_ = 93;
                    TextBott.Width_ = 46;
                    TextBott.Height_ = 15;
                    TextBott.Color = 0x00FF00;
                    TextBott.ASCFont = 1;
                    TextBott.HZFont = 1;
                    TextBott.XPos = 0;
                    TextBott.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextTime;
                    TextTime.Left_ = 4050;
                    TextTime.Top_ = 113;
                    TextTime.Width_ = 46;
                    TextTime.Height_ = 15;
                    TextTime.Color = 0x00FF00;
                    TextTime.ASCFont = 1;
                    TextTime.HZFont = 1;
                    TextTime.XPos = 0;
                    TextTime.YPos = 0;


                    DisplayController.SCL2008.TextInfoType TextGood;
                    TextGood.Left_ = 4050;
                    TextGood.Top_ = 48;
                    TextGood.Width_ = 46;
                    TextGood.Height_ = 15;
                    TextGood.Color = 0x00FF00;
                    TextGood.ASCFont = 1;
                    TextGood.HZFont = 1;
                    TextGood.XPos = 0;
                    TextGood.YPos = 0;

                    DisplayController.SCL2008.TextInfoType TextCap;
                    TextCap.Left_ = 4050;
                    TextCap.Top_ = 70;
                    TextCap.Width_ = 46;
                    TextCap.Height_ = 15;
                    TextCap.Color = 0x00FF00;
                    TextCap.ASCFont = 1;
                    TextCap.HZFont = 1;
                    TextCap.XPos = 0;
                    TextCap.YPos = 0;

                    if (LEDDisplay.bUse)
                    {
                        LEDDisplay.ShowString(LEDDisplay.DevID,Textqp,qp.PadLeft(4,' '));
                        LEDDisplay.ShowString(LEDDisplay.DevID,TextPro,"计划停产".PadLeft(12,' '));
                        LEDDisplay.ShowString(LEDDisplay.DevID,TextTeep,"       ");
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextGood, "       ");
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextCap, "       ");
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextBott, "       ");
                        LEDDisplay.ShowString(LEDDisplay.DevID, TextTime, "       ");

                    }
                    bOK = LEDDisplay.Close();
                }
            }
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
          
            this.WindowState = FormWindowState.Minimized;
            DateTime dte = DateTime.Now;
            drawbmp();

        }


        System.Timers.Timer tbmp = new System.Timers.Timer(30000);
        private void drawbmp()
        {
            
            tbmp.Elapsed += new System.Timers.ElapsedEventHandler(thbmp);
            tbmp.AutoReset = true;
            tbmp.Enabled = true;
        }

        private void thbmp(object sender, System.Timers.ElapsedEventArgs e)
        {
            tbmp.Enabled = false;
            try
            {
                DateTime dt = System.DateTime.Now;

                int thshift = getshift(dt);

                getmsg();
                if(!string.IsNullOrWhiteSpace(pro)){
                    getini(pro, true, thshift, dt); 
                }
               
            }
            catch (Exception ex) {
                UNICLog.ULog.WriteLogDirect(ex.Message);
            }

            tbmp.Enabled = true;
        }
        public void getmsg()
        {
           
            string lpszParentClass = "Tfrmmain"; //整个窗口的类名  
            string lpszParentWindow = null; //窗口标题  
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr EdithWnd = new IntPtr(0);
            IntPtr ip = new IntPtr(0);
            IntPtr hwndCalc = FindWindow(lpszParentClass, lpszParentWindow);
            //UNICLog.ULog.WriteLog("start");
            IntPtr asdf = FindWindow(null,"Piston Measurement System");
            if (asdf != IntPtr.Zero)
            {
              
                IntPtr hwnd3 = FindWindowEx(asdf, 0, "TTabbedNotebook", null);
                if (hwnd3 != IntPtr.Zero)
                {
                  
                    IntPtr hwnd4 = FindWindowEx(hwnd3, 0, "TTabPage", "Configuration");
                    if (hwnd4 != IntPtr.Zero)
                    {
                      
                        IntPtr hwnd5 = FindWindowEx(hwnd4, 0, "TEdit", null);
                        if (hwnd5 != IntPtr.Zero)
                        {

                            StringBuilder strB = new StringBuilder(100);
                            SendMessage(hwnd5, WM_GETTEXT, new IntPtr(255), strB);

                            string a = strB.ToString().Trim();
                            pro = a; 
                        }
                    }

                }
            }
            else {
              
            }
         }

        delegate void FunDelegate(string strtotal,string strgood,string strbad);

        private void FunStart(string stotal,string sgood,string sbad)
        {
            // 要调用的方法的委托
            FunDelegate funDelegate = new FunDelegate(Fun);

            /*========================================================
             * 使用this.BeginInvoke方法
             * （也可以使用this.Invoke()方法）
            ========================================================*/

            // this.BeginInvoke(被调用的方法的委托，要传递的参数[Object数组])
            IAsyncResult aResult = this.BeginInvoke(funDelegate, stotal,sgood,sbad);

            // 用于等待异步操作完成(-1表示无限期等待)
            aResult.AsyncWaitHandle.WaitOne(-1);

            // 使用this.EndInvoke方法获得返回值
         //   string str = (string)this.EndInvoke(aResult);
          //  MessageBox.Show(str.ToString());
        }

        private void Fun(string total,string good,string bad)
        {
            txtgood.Text = good;
           
            //txt.Text = (string)datetime;
            //return "委托的返回值";
        }

        public void getini(string hname,bool boo,int tshift,DateTime dtime) {
               
                int cc= getcount(hname,dtime);
                int total=cc;
                int good = 0;
                int bad = 0;
                if(cc>0){
                    good = cc;
//                    dtime = dtime.AddHours(-7);
                    string thshift = dtime.AddHours(-7).Year + "-" + dtime.AddHours(-7).Month + "-" + dtime.AddHours(-7).Day + tshift;
                    if (type == 1)
                    {
                        #region
                        if (boo)
                        {
                            if (dbsession.FromSql(string.Format("select * from led_count where qp='{0}' and shift='{1}' and proname='{2}'", qp, thshift, pro)).ToDataTable().Rows.Count < 1)
                            {

                                if (tshift == 0)
                                {
                                    DateTime thime = new DateTime(dtime.AddHours(-7).Year, dtime.AddHours(-7).Month, dtime.AddHours(-7).Day, 07, 00, 00);
                                    string te = thime.ToString("yyyyMMddHHmmss");
                                    dbsession.FromSql(string.Format("insert into led_count (qp,oldtotal,oldgood,oldbad,shift,proname,dtime) values('{0}',{1},{2},{3},'{4}','{5}','{6}')", qp, total, good, bad, thshift, pro, te)).ExecuteNonQuery();
                                }
                                else
                                {
                                    DateTime thime = new DateTime(dtime.AddHours(-7).Year, dtime.AddHours(-7).Month, dtime.AddHours(-7).Day, 19, 00, 00);
                                //    UNICLog.ULog.WriteLog(thtime.ToString());
                                    string te = thime.ToString("yyyyMMddHHmmss");
                                    dbsession.FromSql(string.Format("insert into led_count (qp,oldtotal,oldgood,oldbad,shift,proname,dtime) values('{0}',{1},{2},{3},'{4}','{5}','{6}')", qp, total, good, bad, thshift, pro, te)).ExecuteNonQuery();
                                }

                            }
                            if (tshift == 1)
                            {
                                if (dtime.Hour > 18)
                                {
                                    dbsession.FromSql(string.Format("update led_count set total={0}-oldtotal,good={1}-oldgood,bad={2}-oldbad where qp='{3}' and shift='{4}' and proname='{5}'", total, good, bad, qp, thshift, pro)).ExecuteNonQuery();
                                }
                                else
                                {
                                    dbsession.FromSql(string.Format("update led_count set total=total+{0},good=good+{1},bad=bad+{2} where qp='{3}' and shift='{4}' and proname='{5}'", total, good, bad, qp, thshift, pro)).ExecuteNonQuery();
                                }
                            }
                            else if (tshift == 0)
                            {
                                dbsession.FromSql(string.Format("update led_count set total={0}-oldtotal,good={1}-oldgood,bad={2}-oldbad where qp='{3}' and shift='{4}' and proname='{5}'", total, good, bad, qp, thshift, pro)).ExecuteNonQuery();
                            }
                        }
        
                
                }
                
                    DataTable dtable = dbsession.FromSql(string.Format("select * from led_count where qp='{0}' and shift='{1}' and proname='{2}'", qp, thshift,pro)).ToDataTable();
                    if (dtable.Rows.Count > 0)
                    {
                        string nowtotal = dtable.Rows[0]["total"].ToString();
                        string nowgood = dtable.Rows[0]["good"].ToString();
                        string nowbad = dtable.Rows[0]["bad"].ToString();
                        FunStart(nowtotal, nowgood, nowbad);
                        recordcount = int.Parse(nowgood);
                    }
                    #endregion
             
                }
                else
                {
                   
                }
           
        }
        private string ContentValue(string Section, string key,string fpath)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, fpath);
            return temp.ToString();
        }

        public int getshift(DateTime dtime) {
            int temp = -1;
            dtime = dtime.AddHours(-7);
            if(dtime.Hour>-1&&dtime.Hour<12){
                temp = 0;
                
            }
            else {
                temp = 1;
            }
            return temp;
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            FrmTimer frt = new FrmTimer();
            frt.ShowDialog();
            if (frt.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                FrmChange fmc = new FrmChange();
                fmc.ShowDialog();
                if (fmc.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //thtime = fmc.dtm;
                    DateTime de = fmc.dtm;
                    de = de.AddHours(-7);

                    dbsession.FromSql(string.Format("update led_count set  dtime='{0}' where qp='{1}' and shift='{2}' and proname='{3}' ",fmc.dtm,qp,de.Year+"-"+de.Month+"-"+de.Day+flag,pro)).ExecuteNonQuery();
                }
            }

            //getmsg();
            //getcount(pro);

        }

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text == "shut down")
            {
                FrmTimer frt = new FrmTimer();
                frt.ShowDialog();
                if (frt.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        tbmp.Enabled = false;
                        tbmp.Dispose();
                        tbmp.Close();
                        sendstop();
                        
                    
                    }
                    catch (Exception ex) {
                        UNICLog.ULog.WriteLogDirect(ex.Message);
                    }
                    button1.Text = "working";
                }

            }
            else {
                
                drawbmp();
                button1.Text = "shut down";
            }
            
        }

        private int getcount(string proname,DateTime dtm) {
            int tt = 0;
               
                double db = dtm.ToOADate();
                int fdd = (int)Math.Floor(db);

                string strTableName = @"C:\" + proname + @"\" + fdd + @".DBF";
                string tablepath = @"C://" + proname + @"//";
                
                if (File.Exists(strTableName))
                {
                    string strConn = string.Format(@"Provider=VFPOLEDB.1;Data Source={0}", tablepath);
                    
                     //  string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;User ID=Admin;Password=;", tablepath);
                    
                        
                   
                    UNICLog.ULog.WriteLog(strConn);
                    using (connection = new OleDbConnection(strConn))
                    {
                        DataSet ds = new DataSet();
                        
                        try {
                          //  connection.Open();

                            string strSql = @"select count(*) from " + fdd + ".DBF  where JUDGE='O'";
                            UNICLog.ULog.WriteLog(strSql + "---" + strTableName);
                            //string st = "select * from " + strTableName;
                            //需要 using System.Data.Odbc;
                          //  OdbcCommand cmm = new OdbcCommand(strSql,connection);
                            OleDbCommand cmm = new OleDbCommand(strSql,connection);
                            UNICLog.ULog.WriteLog("before open");
                            cmm.Connection.Open();
                            UNICLog.ULog.WriteLog("before scalar");
                            object ot = cmm.ExecuteScalar();
                            if(ot!=null){
                                tt = int.Parse(ot.ToString());
                            }
                            cmm.Connection.Close();
                            cmm.Connection.Dispose();
                            cmm.Dispose();
                            UNICLog.ULog.WriteLog("end---"+tt);
                        }catch( Exception ex){
                            UNICLog.ULog.WriteLogDirect("oledb"+ex.Message);
                            try {
                                UNICLog.ULog.WriteLog("restart");
                                Application.Restart(); }
                            catch(Exception exr){
                                UNICLog.ULog.WriteLogDirect(exr.Message+"restart");
                            }

                        }
                  
                  }
                }
                else {
                    UNICLog.ULog.WriteLog("无数据");
                }
            return tt;
            
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UNICLog.ULog.WriteLog("is closing");
        }
    
    }


}
