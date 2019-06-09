using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Users_notice : System.Web.UI.Page
{
    public string welcome;
    public string userid;
    string mystr = "Data Source='DESKTOP-C2ETVMC';Initial Catalog='ClassroomRent';Integrated Security='True'";
    SqlConnection myconn = new SqlConnection();
    DataSet myds = new DataSet();
    SqlDataAdapter myda;
    string sql = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            myconn.ConnectionString = mystr;
            myconn.Open();
            sql = "select 标题,发布时间 from notice";
            myda = new SqlDataAdapter(sql, myconn);
            myda.Fill(myds);
            GridView1.DataSource = myds;
            GridView1.DataBind();

        }
    }

    protected void lnkselect_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
        string title = GridView1.Rows[Index].Cells[0].Text.ToString();
        string Time = GridView1.Rows[Index].Cells[1].Text.ToString();
        string[] RealTime = Time.Split(' ');

        string sql2 = "select 链接 from notice where( 标题='" + title + "' and 发布时间='" + RealTime[0] + "')";
        myconn.ConnectionString = mystr;
        myconn.Open();
        SqlCommand com = new SqlCommand(sql2, myconn);
        SqlDataAdapter linkData;
        linkData = new SqlDataAdapter(sql2, myconn);
        DataSet Ds = new DataSet();
        linkData.Fill(Ds);
        string lnk = Ds.Tables[0].Rows[0][0].ToString();
        lnk = lnk.Trim();
        Response.Redirect(lnk);
    }
}