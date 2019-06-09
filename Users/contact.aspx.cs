using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class Users_contact : System.Web.UI.Page
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
            sql = "select * from contact_Admin";
            myda = new SqlDataAdapter(sql, myconn);
            myda.Fill(myds);
            GridView1.DataSource = myds;
            GridView1.DataBind();

        }
    }
}