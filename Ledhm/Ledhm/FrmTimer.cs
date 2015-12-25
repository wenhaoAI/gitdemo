using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ledhm
{
    public partial class FrmTimer : Form
    {
        public FrmTimer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtpass.Text))
                {
                    string mima = txtpass.Text;
                    if (mima.Trim() == "huimen12345")
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("密码错误！");
                    }

                }
            }
            catch (Exception ex)
            {
                UNICLog.ULog.WriteLogDirect("密码~" + ex.Message);
            }
        }
    }
}
