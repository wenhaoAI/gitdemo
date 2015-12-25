using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using UNICLog;

namespace UnicTest
{
    public partial class Form1 : Form
    {
        public static ULog ulo = new ULog();
        public Form1()
        {
            InitializeComponent();
            ulo.Path = Application.StartupPath + @"/Log/";
            ulo.todel_time = new TimeSpan(24, 0, 0, 0, 0);
            ulo.INI(ulo.Path,ulo.todel_time);
            ULog.WriteLog("chengxu");
        }
    }
}
