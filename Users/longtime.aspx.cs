using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class Users_longtime : System.Web.UI.Page
{
    public string userid = "";
    public string Conn = "Data Source=DESKTOP-C2ETVMC;Initial Catalog=ClassroomRent;Integrated Security=True";
    public string Campus = "";
    public string Building = "";
    public string room = "";
    public string Week = "";
    public string day = "";
    public string Start_Day = "";
    public string End_Day = "";
    public string hour = "";
    public string Start_hour = "";
    public string End_hour = "";
    public string reason = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        userid = Session["userid"].ToString();
        if (IsPostBack)
        {
            Clicked(sender, e);
        }
        //  Response.Write("123");

        //temp();
    }
    protected void GridView1_Load(object sender, EventArgs e)
    {


    }
 
    string CaculateWeekDay(int y, int m, int d)
    {
        if (m == 1) m = 13;
        if (m == 2) m = 14;
        int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
        string weekstr = "";
        switch (week)
        {
            case 0: weekstr = "周一"; break;
            case 1: weekstr = "周二"; break;
            case 2: weekstr = "周三"; break;
            case 3: weekstr = "周四"; break;
            case 4: weekstr = "周五"; break;
            case 5: weekstr = "周六"; break;
            case 6: weekstr = "周日"; break;
        }

        return weekstr;
    }
    protected List<string> Find_all_day(string day,string start,string end)
    {
        DateTime dt1 = DateTime.Parse(start);
        DateTime dt2 = DateTime.Parse(end);
        List<string> date=new List<string>();
        for(DateTime d = dt1; d < dt2; d = d.AddDays(1))
        {
            string temp = d.ToString();
            string[] temp1 = temp.Split(' ');
            string[] str_date = temp1[0].Split('/');
            string temp2 = CaculateWeekDay(int.Parse(str_date[0]), int.Parse(str_date[1]), int.Parse(str_date[2]));
            if (day == CaculateWeekDay(int.Parse(str_date[0]), int.Parse(str_date[1]), int.Parse(str_date[2])))
            {
                date.Add(temp);
            }
        }
        return date;
    }
    protected void Clicked(object sender, EventArgs e)
    {
        
        if (Request.Params["campusSelect"] != null)
        {
            Campus = Request.Params["campusSelect"];
         
        }
        if (Request.Params["buildingSelect"] != null)
        {
            Building = Request.Params["buildingSelect"];
        }
        if (Request.Params["roomSelect"] != null)
        {
            room = Request.Params["roomSelect"];
        }
        if (Request.Params["Week"] != null)
        {
            Week = Request.Params["Week"];
        }
        if (Request.Params["day"] != null)
        {
            day = Request.Params["day"];
        }
        if (Request.Params["hour"] != null)
        {
            hour = Request.Params["hour"];
        }
        if (Request.Params["Reason"] != null)
        {
            reason = Request.Params["Reason"];
        }
        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = Conn;
        string sql = "";
        try
        {
            string[] Day_array = day.Split('至');
            string[] hour_array = hour.Split('至');
            Start_hour = hour_array[0].Trim();
            End_hour = hour_array[1];
            List<string> total = Find_all_day(Week, Day_array[0], Day_array[1]);
            DataSet myds = new DataSet();
            SqlDataAdapter myda;

            conn.Open();
            foreach(var data in total)
            {
                string[] temp = data.Split(' ');
                string day = temp[0].Replace("/", "-");
                sql = "select * from BorrowLog where building='"+Building+"' and room="+room
                    +" and date='"+day+"' and starttime='"+Start_hour+":00 '"+"and endtime='"+End_hour+":00'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                myda = new SqlDataAdapter(sql, conn);
                myda.Fill(myds);
            }
            if (myds.Tables[0].Rows.Count == 0)
            {
                //执行预约程序
                foreach (var data in total)
                {
                    string[] temp = data.Split(' ');
                    string day = temp[0].Replace("/", "-");
                    string sql2 = "insert into Borrowlog values('" + day + "','" + Start_hour + ":00'," + "1,'" + Building + "'," + room +
                        "," + userid + ",'"+reason+"',"+0+",'"+End_hour+":00',0)";
                    SqlCommand cmd = new SqlCommand(sql2, conn);
                    cmd.ExecuteNonQuery();
                    
                }
                Response.Write("<script>alert('成功预约')</script>");

            }
            else
            {
                Response.Write("<script>alert('该选定时间段内有被预定的教室')</script>");
                GridView1.DataSource = myds;
                GridView1.DataBind();
            }
        }
        catch
        {


        }
    }
  
}