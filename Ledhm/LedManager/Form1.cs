using Dos.ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void 主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFrom mainFrom = new MainFrom();
            panel1.Controls.Clear();
            mainFrom.FormBorderStyle = FormBorderStyle.None;
            mainFrom.TopLevel = false;
            mainFrom.Parent = panel1;
            mainFrom.Show();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SzFrom szFrom = new SzFrom();
            panel1.Controls.Clear();
            szFrom.FormBorderStyle = FormBorderStyle.None;
            szFrom.TopLevel = false;
            szFrom.Parent = panel1;
            szFrom.Show();
        }

        private void 产品设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProFrom proform = new ProFrom();
            panel1.Controls.Clear();
            proform.FormBorderStyle = FormBorderStyle.None;
            proform.TopLevel = false;
            proform.Parent = panel1;
            proform.Show();
        }
    }
}
