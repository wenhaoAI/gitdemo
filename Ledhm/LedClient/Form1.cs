using HPSocketCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LedClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HPSocketCS.TcpClient client = new HPSocketCS.TcpClient();
        private void Form1_Load(object sender, EventArgs e)
        {
            try {
                client.OnPrepareConnect += new TcpClientEvent.OnPrepareConnectEventHandler(OnPrepareConnect);
                client.OnConnect += new TcpClientEvent.OnConnectEventHandler(OnConnect);
                client.OnSend += new TcpClientEvent.OnSendEventHandler(OnSend);
                client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(OnReceive);
                client.OnClose += new TcpClientEvent.OnCloseEventHandler(OnClose);
                client.OnError += new TcpClientEvent.OnErrorEventHandler(OnError);
                
            }catch(Exception ex){
              
            
            }
        }

        private HandleResult OnPrepareConnect(TcpClient sender, uint socket)
        {
            return HandleResult.Ok;
        }

        private HandleResult OnConnect(TcpClient sender)
        {
            return HandleResult.Ok;
        }

        private HandleResult OnSend(TcpClient sender, IntPtr pData, int length)
        {
            return HandleResult.Ok;
        }

        private HandleResult OnReceive(TcpClient sender, IntPtr pData, int length)
        {
            return HandleResult.Ok;
        }

        private HandleResult OnClose(TcpClient sender)
        {
            return HandleResult.Ok;
        }

        private HandleResult OnError(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            return HandleResult.Ok;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string ip = ConfigurationManager.AppSettings["server"];
            //ushort port = ushort.Parse( ConfigurationManager.AppSettings["port"]);
            //try {
            //    client.Connetion(ip,port,false);
            //}catch(Exception ex){
            //    MessageBox.Show(ex.Message);
            //}   
        
        
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string ip = ConfigurationManager.AppSettings["server"];
            ushort port = ushort.Parse(ConfigurationManager.AppSettings["port"]);
            try
            {
                client.Connetion(ip, port, false);
                drawbmp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        System.Timers.Timer tbmp = new System.Timers.Timer(20000);
        private void drawbmp()
        {
            tbmp.Elapsed += new System.Timers.ElapsedEventHandler(thbmp);
            tbmp.AutoReset = true;
            tbmp.Enabled = true;
        }

        private void thbmp(object sender, System.Timers.ElapsedEventArgs e)
        {
            Random ran = new Random();

            int aa = ran.Next(100000);
            int bb = ran.Next(100000);
            int cc = ran.Next(100000);
            string a = aa+","+bb+","+cc;
            byte[] byes = Encoding.Default.GetBytes(a);
            client.Send(byes,byes.Length);
        }
        public void getmsg() {
            OleDbConnectionStringBuilder cb = new OleDbConnectionStringBuilder();
            cb.Provider = "LCPI.IBProvider";
            cb.Add("Location", @"192.168.1.208:e:\keyu.gdb");
            cb.Add("User ID", "sysdba");
            cb.Add("Password", "masterkey");
            cb.Add("ctype", "win1251");
            OleDbConnection con = new OleDbConnection(cb.ToString());
            con.Open();
            OleDbTransaction trans = con.BeginTransaction();

            OleDbCommand cmd = new OleDbCommand("select count(*) from tablename", con, trans);

            DataTable dt = cmd.ExecuteReader().GetSchemaTable();
            trans.Commit();
            con.Close();
        
        }

    }
}
