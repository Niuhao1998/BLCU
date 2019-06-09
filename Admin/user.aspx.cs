using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.IO;
using System.Windows.Forms;3
using System.Threading;
public partial class Admin_user : System.Web.UI.Page
{
    public string mypath = "无内容";
    string mystr = "Data Source=DESKTOP-C2ETVMC;Initial Catalog=ClassroomRent;Integrated Security=True";
    protected void Page_Load(object sender, EventArgs e)
    {  
        SqlConnection myconn = new SqlConnection();
        myconn.ConnectionString = mystr;
        myconn.Open();
        string mysql = "select USERid as 学号, password as 密码, contact as 联系方式, college as 学校, grade as 班级 , major as 专业 from users";
        DataSet myds = new DataSet();
//GridViewButtonColumn col_btn_insert = new GridViewButtonColumn();
        //col_btn_insert.HeaderText = "增加";
        //col_btn_insert.Text = "增加";//加上这两个就能显示
        //col_btn_insert.UseColumnTextForButtonValue = true;//
        //GridView1.Columns.Add(col_btn_insert);
        SqlDataAdapter myda = new SqlDataAdapter(mysql, myconn);
        myda.Fill(myds, "user");
        GridView1.DataSource = myds.Tables["user"];
        GridView1.DataBind();
        myconn.Close();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {   
        SqlConnection myconn = new SqlConnection();
        myconn.ConnectionString = mystr;
        myconn.Open();
        DataSet myds = new DataSet();
        string mysql = "insert into users values('" + TextBox5.Text.ToString() + "','" + TextBox6.Text.ToString() + "','" + TextBox7.Text.ToString() + "','" + TextBox8.Text.ToString() + "','" + TextBox9.Text.ToString() + "','" + TextBox10.Text.ToString() + "')";      
        SqlCommand com = new SqlCommand(mysql, myconn);
        com.ExecuteNonQuery();
        string mysql1 = "select USERid as 学号, password as 密码, contact as 联系方式, college as 学校, grade as 班级 , major as 专业 from users";
        SqlDataAdapter myda = new SqlDataAdapter(mysql1, myconn);
        myda.Fill(myds, "user");
        GridView1.DataSource = myds.Tables["user"];
        GridView1.DataBind();
        myconn.Close();
    }
   
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e) {
        SqlConnection myconn = new SqlConnection();
        myconn.ConnectionString = mystr;
        DataSet myds = new DataSet();
        myconn.Open();
        string mysql = "delete from users where userid=@First";
        SqlCommand SqlCmd = new SqlCommand(mysql, myconn);
        SqlCmd.Parameters.AddWithValue("@First", ((GridView1.Rows[e.RowIndex].Cells[1].Text.Trim())));       
        SqlCmd.ExecuteNonQuery();   
        string mysql1 = "select USERid as 学号, password as 密码, contact as 联系方式, college as 学校, grade as 班级 , major as 专业 from users";
        SqlDataAdapter myda = new SqlDataAdapter(mysql1, myconn);
        myda.Fill(myds, "user");
        GridView1.DataSource = myds.Tables["user"];
        GridView1.DataBind();
        myconn.Close();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {       Thread InvokeThread = new Thread(new ThreadStart(InvokeMethod));
            InvokeThread.SetApartmentState(ApartmentState.STA);
            InvokeThread.Start();
            InvokeThread.Join();
            TextBox11.Text = mypath;                                       
       
    }
    protected void readexcel(string fileroad ) {
        string id,password,contact,college,grade,major;
        DataTable ExcelTable;
        DataSet ds = new DataSet();
        FileInfo file = new FileInfo(fileroad);
        string extension = file.Extension;
        string ConnectionString;
        switch (extension)                          // 连接字符串
        {
            case ".xls":
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileroad + ";Extended Properties='Excel 8.0;HDR=no;IMEX=1;'";
                break;
            case ".xlsx":
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileroad + ";Extended Properties='Excel 12.0;HDR=no;IMEX=1;'";
                break;
            default:
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileroad + ";Extended Properties='Excel 8.0;HDR=no;IMEX=1;'";
                break;
        }
        OleDbConnection objConn = new OleDbConnection(ConnectionString);
        objConn.Open();
        DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

        string tableName = schemaTable.Rows[0][2].ToString().Trim();//获取 Excel 的表名，默认值是sheet1
        string strSql = "select * from [" + tableName + "]";

        OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
        OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
        myData.Fill(ds, tableName);
        ExcelTable = ds.Tables[tableName];
        int iColums = ExcelTable.Columns.Count;//列数
        int iRows = ExcelTable.Rows.Count;//行数
         string[,] storedata = new string[iRows, iColums];

        ArrayList list = new ArrayList();
        SqlConnection myconn = new SqlConnection();
        myconn.ConnectionString = mystr;
        myconn.Open();
        DataSet myds = new DataSet();
        for (int i = 1; i < ExcelTable.Rows.Count; i++)
        {
            //SupermarketVO vo = new SupermarketVO();
           id = ExcelTable.Rows[i][0].ToString();                    
           password = ExcelTable.Rows[i][1].ToString();               
           contact = ExcelTable.Rows[i][2].ToString();
           college = ExcelTable.Rows[i][3].ToString();
           grade = ExcelTable.Rows[i][4].ToString();
           major = ExcelTable.Rows[i][5].ToString();
           string mysql = "insert into users values('" + id + "','" + password + "','" + contact + "','" + college + "','" +grade + "','" +major + "')";
           SqlCommand com = new SqlCommand(mysql, myconn);
           com.ExecuteNonQuery();
        }
        string mysql1 = "select USERid as 学号, password as 密码, contact as 联系方式, college as 学校, grade as 班级 , major as 专业 from users";
        SqlDataAdapter myda = new SqlDataAdapter(mysql1, myconn);
        myda.Fill(myds, "user");
        GridView1.DataSource = myds.Tables["user"];
        GridView1.DataBind();
        myconn.Close();
        objConn.Close();
}
    private void InvokeMethod()
    {        
            OpenFileDialog InvokeDialog = new OpenFileDialog();
 
            if (InvokeDialog.ShowDialog() == DialogResult.OK)
            {
                mypath = System.IO.Path.GetFullPath(InvokeDialog.FileName).ToString();
                //mypath = File.ReadAllText(InvokeDialog.FileName);
            }           
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
         readexcel(TextBox11.Text.ToString());
    }
}
