using Dos.ORM;
using LedManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedManager
{
    class ClassUtils
    {
        DbSession dbsession = new DbSession("MySqlConn");
        
        #region
        ///<summary>
        ///TreeView刷新添加节点
        ///</summary>
        ///<param name="view">需要刷新的TreeView</param> 
        /// <param name="str">数据库语句</param> 
        public  void refreshTreeView(TreeView view,string str) {
           if(view.Nodes.Count>0){
               view.Nodes[0].Nodes.Clear();
           }
            

            DataTable dt = dbsession.FromSql(str).ToDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dt.Rows[i][0].ToString();
                if(dt.Rows[i][1].ToString()=="0"){
                    tn.ForeColor = Color.Green;

                }
                else if (dt.Rows[i][1].ToString() == "1")
                {
                    tn.ForeColor = Color.Red;
                }else{
                    tn.ForeColor = Color.Gray;
                }

                view.Nodes[0].Nodes.Add(tn);
            }
            view.Nodes[0].ForeColor = Color.Green;
            view.Font = new System.Drawing.Font(view.Font.FontFamily,14);
            view.ExpandAll();
        }
        #endregion
        
    
    }
}
