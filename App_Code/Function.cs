using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
/// <summary>
/// 实现一些经常需要重复使用的方法，如CheckLogin()
/// </summary>
public class Function
{
    public static string Conn = "Data Source=DESKTOP-C2ETVMC;Initial Catalog=ClassroomRent;Integrated Security=True";
	public Function()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public void CheckLogin() //以下代码检测用户登录参数是否正确
    {

       
    }
    public SqlConnection CreateConnection()
    {
        SqlConnection myconn = new SqlConnection();
        myconn.ConnectionString = Conn;
        return myconn;
    }
    //绑定下拉列表
    public void DataSourceBind(string sql, DropDownList e,string name)
    {
        SqlConnection myconn = CreateConnection();
        myconn.Open();
        SqlCommand cmd = new SqlCommand(sql, myconn);
        SqlDataReader sdr = cmd.ExecuteReader();
        e.DataSource = sdr;
        e.DataTextField = name;//文本内容
        e.DataValueField = name; //数据源字段
        e.DataBind();
        sdr.Close();
        myconn.Close();
    }
    //注册时判断登录名是否在数据库中存在
        public bool IsUsernameExists(string username,string sql)
        {
            SqlConnection myconn = new SqlConnection();
            SqlCommand mycmd = new SqlCommand();
            bool b;
            int num = this.Rownum(sql);
            if (num > 0)
            {
                b = true;
            }
            else
            {
                b = false;
            }
            return b;            
        }
        //返回符合条件的数值的行数
        public int Rownum(string sql)
        {	//sql参数指出SQL语句
            int i = 0;
            string mystr = Conn;
            SqlConnection myconn = new SqlConnection();
            myconn.ConnectionString = mystr;
            myconn.Open();
            SqlCommand mycmd = new SqlCommand(sql, myconn);
            SqlDataReader myreader = mycmd.ExecuteReader();
            while (myreader.Read())		//循环读取信息
                i++;
            myconn.Close();
            return i;			//返回读取的行数
        }
        //返回读取的唯一行的唯一字段，第一个字段值
        public string Returnafield(string sql)
        {	//sql指出SQL语句
            string fn;
            string mystr = Conn; ;
            SqlConnection myconn = new SqlConnection();
            myconn.ConnectionString = mystr;
            myconn.Open();
            SqlCommand mycmd = new SqlCommand(sql, myconn);
            SqlDataReader myreader = mycmd.ExecuteReader();
            myreader.Read();
            fn = myreader[0].ToString().Trim();
            myconn.Close();
            return fn;		//返回读取的数据
        }
        //进行更新，删除操作
        public int ExecuteNonQuery(string sql)
        {
            string mystr = Conn;
            SqlConnection myconn = new SqlConnection();
            myconn.ConnectionString = mystr;
            myconn.Open();
            SqlCommand mycmd = new SqlCommand(sql, myconn);
            int num = mycmd.ExecuteNonQuery();
            myconn.Close();
            return num;
        }
        //通过SqlDataAdapter读取数据
        public DataSet ExecuteQuery(string sql, string tname)
        {
            string mystr = Conn;
            SqlConnection myconn = new SqlConnection();
            myconn.ConnectionString = mystr;
            myconn.Open();
            SqlDataAdapter myda = new SqlDataAdapter(sql, myconn);
            DataSet myds = new DataSet();
            myda.Fill(myds, tname);
            myconn.Close();
            return myds;
        }
        //public void dataview(string sql)
        //{
        //    SqlConnection myconn = CreateConnection();
        //    myconn.Open();
        //    SqlCommand cmd = new SqlCommand(sql, myconn);
        //    da
        //    reader.Close();
        //    myconn.Close();
        //}
        //执行Select语句，返回聚合函数结果
        public string ExecuteAggregateQuery(string sql)
        {
            string jg;
            string mystr = Conn;
            SqlConnection myconn = new SqlConnection();
            myconn.ConnectionString = mystr;
            myconn.Open();
            SqlCommand mycmd = new SqlCommand();
            mycmd.CommandText = sql;
            mycmd.Connection = myconn;
            jg = mycmd.ExecuteScalar().ToString();
            myconn.Close();
            return jg;
        }
        public DataSet getData(string sql)
        { 

            DataSet set = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
            adapter.Fill(set);
            return set;
        }

}
