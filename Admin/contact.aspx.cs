using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class Admin_contact : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            showGridview();
        }
    }
    protected void showGridview()
    {
        myconn.ConnectionString = mystr;
        myconn.Open();
        sql1 = "select * from contact_Admin";

        myda = new SqlDataAdapter(sql1, myconn);
        myda.Fill(myds);
        GridView1.DataSource = myds;
        GridView1.DataBind();
        myconn.Close();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("AddNew"))
            {
                using (SqlConnection SqlCon = new SqlConnection(mystr))
                {
                    SqlCon.Open();
                    string query = "insert into contact_Admin (姓名,联系方式,QQ,微信) values (@First,@Second,@Third,@Last)";
                    SqlCommand SqlCmd = new SqlCommand(query, SqlCon);
                    SqlCmd.Parameters.AddWithValue("@First", ((GridView1.FooterRow.FindControl("txtFirstNameFooter")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Second", ((GridView1.FooterRow.FindControl("txtSecondNameFooter")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Third", ((GridView1.FooterRow.FindControl("txtThirdNameFooter")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Last", ((GridView1.FooterRow.FindControl("txtLastNameFooter")) as TextBox).Text.Trim());
                    SqlCmd.ExecuteNonQuery();
                    showGridview();
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;

        showGridview();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;

        showGridview();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
                using (SqlConnection SqlCon = new SqlConnection(mystr))
                {
                    SqlCon.Open();
                    string query = "update contact_Admin set 姓名=@First,联系方式=@Second,QQ=@Third,微信=@Last where QQ=@Third";
                    SqlCommand SqlCmd = new SqlCommand(query, SqlCon);
                    SqlCmd.Parameters.AddWithValue("@First", ((GridView1.Rows[e.RowIndex].FindControl("txtFirstName")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Second", ((GridView1.Rows[e.RowIndex].FindControl("txtSecondName")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Third", ((GridView1.Rows[e.RowIndex].FindControl("txtThirdName")) as TextBox).Text.Trim());
                    SqlCmd.Parameters.AddWithValue("@Last", ((GridView1.Rows[e.RowIndex].FindControl("txtLastName")) as TextBox).Text.Trim());
                    SqlCmd.ExecuteNonQuery();
                    showGridview();
                
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            using (SqlConnection SqlCon = new SqlConnection(mystr))
            {
                SqlCon.Open();
                string query = "delete from contact_Admin where 姓名=@First and 联系方式=@Second and QQ=@Third and 微信=@Last";
                SqlCommand SqlCmd = new SqlCommand(query, SqlCon);
                SqlCmd.Parameters.AddWithValue("@First", ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[0].Controls[1]).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Second",((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[1].Controls[1]).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Third", ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[2].Controls[1]).Text.Trim()); ;
                SqlCmd.Parameters.AddWithValue("@Last",  ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[3].Controls[1]).Text.Trim()); ;
                SqlCmd.ExecuteNonQuery();
                showGridview();

            }
        }
        catch (Exception ex)
        {

        }

    }
}