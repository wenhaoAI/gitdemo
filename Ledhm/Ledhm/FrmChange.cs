using Dos.ORM;
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
    public partial class FrmChange : Form
    {
        public FrmChange()
        {
            InitializeComponent();
        }


        public DateTime dtm { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            dtm = dtp.Value;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

    }
}
