using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

public partial class Admin_Cancel : System.Web.UI.Page, IHttpHandler, IRequiresSessionState
{
    public string welcome;
    public string userid;
    string mystr = "Data Source='DESKTOP-C2ETVMC';Initial Catalog='ClassroomRent';Integrated Security='True'";
    string sql1, sql2;
    SqlConnection myconn = new SqlConnection();
    DataSet myds = new DataSet();
    SqlDataAdapter myda;

    protected void Page_Load(object sender, EventArgs e)
    {
        welcome = "您的预约记录";
        userid = Session["userid"].ToString();
        if (!IsPostBack)
        {
            myconn.ConnectionString = mystr;
            myconn.Open();
            sql1 = "select userID,date,starttime,endtime,reason,building,room,";
            sql1 += "(case checkstate when 1 then '已通过' when 0 then '未通过' end) checkstate";
            sql1 += " from BorrowLog where IsDeleted=0 order by date desc";
            myda = new SqlDataAdapter(sql1, myconn);
            myda.Fill(myds, "borrowlog");
            GridView1.DataSource = myds.Tables["borrowlog"];
            GridView1.DataBind();
            myconn.Close();
        }


    }
    protected void GridView1_Load(object sender, EventArgs e)
    {
        DateTime date;
        int nowh = int.Parse(DateTime.Now.Hour.ToString());
        int nowm = int.Parse(DateTime.Now.Minute.ToString());
        foreach (GridViewRow row in GridView1.Rows)
        {
            Button cancelorder = row.Cells[8].FindControl("canceling") as Button;
            DateTime.TryParse(row.Cells[1].Text.ToString(), out date);
            int hour = int.Parse(row.Cells[2].Text.ToString().Substring(0, 2));
            int min = int.Parse(row.Cells[2].Text.ToString().Substring(3, 2));
            if (date > DateTime.Now.Date ||
                (date == DateTime.Now.Date && (hour > nowh || (hour == nowh && min > nowm))))
            {
                cancelorder.Visible = true;
            }
            else
            {
                cancelorder.Visible = false;
            }
        }
    }
    protected void canceling_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((Button)sender).NamingContainer).RowIndex;
        string id = GridView1.Rows[Index].Cells[0].Text.ToString();
        string date = GridView1.Rows[Index].Cells[1].Text.ToString();
        string time = GridView1.Rows[Index].Cells[2].Text.ToString();
        string room = GridView1.Rows[Index].Cells[5].Text.ToString();
        sql2 = "update BorrowLog set IsDeleted=1 where ";
        sql2 += "userID = '" + id + "' and date = '" + date + "' ";
        sql2 += "and starttime = '" + time + "' and room = '" + room + "'";
        myconn.ConnectionString = mystr;
        myconn.Open();

        SqlCommand com = new SqlCommand(sql2, myconn);
        int i = 0;
        i = com.ExecuteNonQuery();
        if (i != 0)
        {
            Response.Write("<script>window.alert('取消成功！');</script>");
            sql1 = "select * from BorrowLog where userID = '" + userid + "' and IsDeleted=0 order by date desc";
            myda = new SqlDataAdapter(sql1, myconn);
            myda.Fill(myds, "borrowlog");
            GridView1.DataSource = myds.Tables["borrowlog"];
            GridView1.DataBind();
            myconn.Close();

        }
        else { Response.Write("<script>window.alert('error！');</script>"); }

    }
}
