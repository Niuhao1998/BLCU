using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Admin_notice : System.Web.UI.Page
{
    public string welcome;
    public string userid;
    string mystr = "Data Source='DESKTOP-C2ETVMC';Initial Catalog='ClassroomRent';Integrated Security='True'";
    string sql1, sql2;
    SqlConnection myconn = new SqlConnection();
    DataSet myds = new DataSet();
    SqlDataAdapter myda;
    string title = "无标题";
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
        sql1 = "select * from notice";

        myda = new SqlDataAdapter(sql1, myconn);
        myda.Fill(myds);
        GridView1.DataSource = myds;
        GridView1.DataBind();
        myconn.Close();
    }

    protected void btClick_Click(object sender, EventArgs e)
    {
        bool fileOK = false;
        //指定路径
        string path = Server.MapPath("~/");
        //文件上传控件中如果已经包含文件
        if (fUpload.HasFile)
        {
            //得到文件的后缀
            string fileExtension = System.IO.Path.GetExtension(fUpload.FileName).ToLower();
            //允许的文件后缀
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" ,".py",".c",".cpp",".pdf",".txt",
            ".xls",".html",".java",".docx"};
            //看包含的文件是否是被允许的文件后缀
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    //如果是，标志位置为真
                    fileOK = true;
                }
            }
        }
        if (Request.Params["Reason"] != null)
        {
            title = Request.Params["Reason"];
        }
        if (fileOK)
        {
            try
            {
                //文件另存在服务器指定目录下
                path += "Users/notice/";

                myconn.ConnectionString = mystr;
                myconn.Open();
                
                string date;
                date = DateTime.Now.ToLocalTime().ToString();
                
                Random fileRandom = new Random();
                string fileExtension = System.IO.Path.GetExtension(fUpload.FileName).ToLower();
                string fileName = System.DateTime.Now.ToFileTime() + Convert.ToInt32(fileRandom.NextDouble()).ToString() + fileExtension;
                fUpload.PostedFile.SaveAs(path + fileName);
                sql1 = "Insert into notice values('" + title + "' , '" + date + "' ," +"'./notice/"+fileName+"')";
                SqlCommand cmd = new SqlCommand(sql1, myconn);
                cmd.ExecuteNonQuery();
                lbText.Text = "文件上传成功";
            }
            catch (Exception ex)
            {
                lbText.Text = "文件上传失败！";
            }
        }
        else
        {
            lbText.Text = "只能上传gif、png、jpeg或者jpg图像文件！";
            lbText.ForeColor = System.Drawing.Color.Red;
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
                string query = "update notice set 标题=@First,发布时间=@Second,链接=@Third where 链接=@Third";
                SqlCommand SqlCmd = new SqlCommand(query, SqlCon);
                SqlCmd.Parameters.AddWithValue("@First", ((GridView1.Rows[e.RowIndex].FindControl("txtFirstName")) as TextBox).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Second", ((GridView1.Rows[e.RowIndex].FindControl("txtSecondName")) as TextBox).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Third", ((GridView1.Rows[e.RowIndex].FindControl("txtThirdName")) as TextBox).Text.Trim());
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
                string query = "delete from notice where 标题=@First and 发布时间=@Second and 链接=@Third";
                SqlCommand SqlCmd = new SqlCommand(query, SqlCon);
                SqlCmd.Parameters.AddWithValue("@First", ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[0].Controls[1]).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Second", ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[1].Controls[1]).Text.Trim());
                SqlCmd.Parameters.AddWithValue("@Third", ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[2].Controls[1]).Text.Trim()); ;
                

                string path = Server.MapPath("~\\");
                string temp= ((System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].Cells[2].Controls[1]).Text.Trim();
                temp = temp.Substring(2);
                path = path +"/Users/"+temp;
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch(System.IO.IOException ex)
                    {

                    }
                }
                SqlCmd.ExecuteNonQuery();
                showGridview();

            }
        }
        catch (Exception ex)
        {

        }

    }
}