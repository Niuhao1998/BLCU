using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

public partial class Admin_Review : System.Web.UI.Page
{
    public string welcome;
    public string userid;
    string mystr = "Data Source='DESKTOP-C2ETVMC';Initial Catalog='ClassroomRent';Integrated Security='True'";
    string sql1, sql2, sql3;
    SqlConnection myconn = new SqlConnection();
    DataSet myds = new DataSet();
    SqlDataAdapter myda;
    SqlCommand com;
    int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        welcome = "当前申请情况";
        if (!IsPostBack)
        {
            myconn.ConnectionString = mystr;
            myconn.Open();
            sql1 = "select userID,date,starttime,endtime,reason,building,room,";
            sql1 += "(case checkstate when 1 then '已通过' when 0 then '未审核' when -1 then '未通过' end) checkstate";
            sql1 += " from BorrowLog where isdeleted = 0 and ";
            sql1 += "( date> CONVERT (nvarchar(12),GETDATE(),112) or (date = CONVERT (nvarchar(12),GETDATE(),112) and starttime>=convert(char(8),getdate(),108))) order by date, starttime, orderdate";
            myda = new SqlDataAdapter(sql1, myconn);
            myda.Fill(myds, "borrowlog");
            GridView1.DataSource = myds.Tables["borrowlog"];
            GridView1.DataBind();
            myconn.Close();
        }
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            Button check = row.Cells[8].FindControl("review") as Button;
            Button refuse = row.Cells[8].FindControl("refuse") as Button;
            string checkstate = row.Cells[7].Text.ToString();
            //int min = int.Parse(row.Cells[2].Text.ToString().Substring(3, 2));
            if (checkstate == "已通过")
            {
                check.Text = "取消审核";
            }
            else if (checkstate == "未审核")
            {
                check.Text = "通过审核";
            }
            else
            {
                check.Visible = false;
                refuse.Text = "取消拒绝";
            }
        }
    }
    protected void review_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((Button)sender).NamingContainer).RowIndex;
        string id = GridView1.Rows[Index].Cells[0].Text.ToString();
        string date = GridView1.Rows[Index].Cells[1].Text.ToString();
        string time = GridView1.Rows[Index].Cells[2].Text.ToString();
        string room = GridView1.Rows[Index].Cells[5].Text.ToString();
        Button check = GridView1.Rows[Index].Cells[8].FindControl("review") as Button;
        Button refuse = GridView1.Rows[Index].Cells[8].FindControl("refuse") as Button;
        refuse.Visible = false;
        sql2 = "update BorrowLog set checkstate= abs(checkstate -1) where ";
        sql2 += "userID = '" + id + "' and date = '" + date + "' ";
        sql2 += "and starttime = '" + time + "' and room = '" + room + "'";
        sql2 += "and isDeleted = 0";
        myconn.ConnectionString = mystr;
        myconn.Open();
        //如果存在借用同一时间段的情况，那么先借用的显示在前，当通过先借用的人的请求后，后借用的自动拒绝。
        //当取消审核先借用的人的请求后，后借用的人变为未通过。

        //确定当前该时间该教室是否有多个人预定
        string sql = "select count(*) from BorrowLog where ";
        sql += "userID != '" + id + "' and date = '" + date + "' ";
        sql += "and starttime = '" + time + "' and room = '" + room + "'";
        sql += "and isDeleted = 0 and checkstate != 1";
        com = new SqlCommand(sql, myconn);
        count = int.Parse(com.ExecuteScalar().ToString());
        if (count > 1) //如果有多个人预定
        {
            if (check.Text == "通过审核") //表示当前是通过该行的审核，那么其余执行的是拒绝操作
            {
                sql3 = "update BorrowLog set checkstate= -1 where ";
                sql3 += "userID != '" + id + "' and date = '" + date + "' ";
                sql3 += "and starttime = '" + time + "' and room = '" + room + "' ";
                sql3 += "and isDeleted = 0 and checkstate != 1";
            }
            else
            {
                sql3 = "update BorrowLog set checkstate= 0 where ";
                sql3 += "userID != '" + id + "' and date = '" + date + "' ";
                sql3 += "and starttime = '" + time + "' and room = '" + room + "' ";
                sql3 += "and isDeleted = 0 and checkstate != 1";
            }
        }
        updatedataset(sql2);



    }
    protected void refuse_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((Button)sender).NamingContainer).RowIndex;
        string id = GridView1.Rows[Index].Cells[0].Text.ToString();
        string date = GridView1.Rows[Index].Cells[1].Text.ToString();
        string time = GridView1.Rows[Index].Cells[2].Text.ToString();
        string room = GridView1.Rows[Index].Cells[5].Text.ToString();
        sql2 = "update BorrowLog set checkstate= -abs(checkstate + 1) where ";
        sql2 += "userID = '" + id + "' and date = '" + date + "' ";
        sql2 += "and starttime = '" + time + "' and room = '" + room + "'";
        sql2 += "and isDeleted = 0";
        myconn.ConnectionString = mystr;
        myconn.Open();
        updatedataset(sql2);
    }
    public void updatedataset(string sql)
    {
        com = new SqlCommand(sql, myconn);
        int i = 0;
        i = com.ExecuteNonQuery();
        Response.Write("<script>window.alert('sss！');</script>");
        sql1 = "update BorrowLog set checkstate= -1 where checkstate < -1";
        com = new SqlCommand(sql1, myconn);
        com.ExecuteNonQuery();
        sql2 = "update BorrowLog set checkstate= 1 where checkstate > 1";
        com = new SqlCommand(sql2, myconn);
        com.ExecuteNonQuery();
        if (count > 1)
        {
            com = new SqlCommand(sql3, myconn);
            com.ExecuteNonQuery();
        }
        if (i != 0)
        {
            myconn.Close();
            Response.Write("<script>window.alert('操作成功！');</script>");
            //Response.Write("<script>window.location='Review.aspx'</script>");
            Response.Redirect("Review.aspx");
            i = 0;

        }
        else { Response.Write("<script>window.alert('error！');</script>"); }
    }
}