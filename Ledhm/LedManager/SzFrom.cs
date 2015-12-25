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
    public partial class SzFrom : Form
    {

        DbSession dbsession = new DbSession("MySqlConn");

        public SzFrom()
        {
            InitializeComponent();
            new ClassUtils().refreshTreeView(treeView1,"select qp,status from led");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(treeView1.SelectedNode.Text!="所有生产线"){
                label2.Text = treeView1.SelectedNode.Text;
                DataTable dt = dbsession.FromSql(string.Format("select * from led where qp='{0}'",treeView1.SelectedNode.Text)).ToDataTable();
                textcom.Text = dt.Rows[0][3].ToString();
                textip.Text = dt.Rows[0][2].ToString();
                textpro.Text = dt.Rows[0][4].ToString();
                if(dt.Rows[0][5].ToString()=="0"){
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                }
                else if (dt.Rows[0][5].ToString() == "1")
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;                
                }else{
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;                        
                }
            
            }
        }
    }
}
