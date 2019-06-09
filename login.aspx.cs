using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
public partial class login : System.Web.UI.Page
{
    public string Conn = "Data Source=DESKTOP-C2ETVMC;Initial Catalog=ClassroomRent;Integrated Security=True";
    public string id = "";
    public string pass = "";
    public string Reinput_text = "";
    public string choose = "";
    public bool flag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Unnamed1_Click(sender, e);
        }
    }


    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        if (Request.Params["id"] != null)
        {
            id = Request.Params["id"].ToString();
        }
        if (Request.Params["password"] != null)
        {
            pass = Request.Params["password"].ToString();
        }
        if (Request.Params["choose"] != null)
        {
            choose = Request.Params["choose"].ToString();
        }
        if (id == "" || pass == "")
        {
            if (flag == false)
            {
                flag = true;
            }
            else
            {
                Response.Write("<script>alert('用户名或密码不能为空')</script>");
            }
        }
        else
        {
            if (choose == "user")
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Conn;
                string sql = "select * from users where userID= '" + id + "' and password='" + pass + "'";
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        conn.Close();
                        Session["userid"] = id;
                        Session["password"] = pass;
                        //重定向
                        Response.Redirect("Users/reservation.aspx");

                        //Server.Transfer();
                    }
                    else
                    {
                        Response.Write("<script>alert('用户名或密码错误')</script>");
                    }
                }
                catch
                {
                    Response.Write("wrong!");
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Conn;
                string sql = "select * from Admin where ID= '" + id + "' and 密码='" + pass + "'";
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        conn.Close();
                        Session["userid"] = id;
                        Session["password"] = pass;
                        //重定向
                        Response.Redirect("Admin/reservation.aspx");

                        //Server.Transfer();
                    }
                    else
                    {
                        Response.Write("<script>alert('用户名或密码错误')</script>");
                    }
                }
                catch
                {
                    Response.Write("wrong!");
                }
            }
        }
    }
}