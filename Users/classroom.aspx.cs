using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;
public partial class Users_classroom : System.Web.UI.Page
{
    Function myFunc = new Function();
    public DataSet ds = new DataSet();
    public int BookDay = 7;   //可提前预约的天数，默认可提前7天预约
    public int NeedDay = 3;  //需要提前预约天数，默认必须提前3天预约
    public string userid = "";
    public string users = "";
    public void CheckLogin() //以下代码检测用户登录参数是否正确
    {

        //String StuId = (string)Session["StuId"];
        //DataSet dt = SqlHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [BookClass].[dbo].[UserInfo] WHERE StuId ='" + StuId + "'AND Type = 'Student'");
        //if (dt.Tables[0].Rows.Count == 0)
        //{
        //    Session.Abandon();
        //    Console.Write("您的参数有误，请尝试重新登录。");
        //    //  System.Threading.Thread.Sleep(10000); 
        //    Response.Redirect("login.aspx");
        //    return;
        //}
        //else
        //{
        //    DataSet dt1 = SqlHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [BookClass].[dbo].[UserInfo] WHERE StuId ='" + StuId + "'");
        //    label_stuNum.Text = dt1.Tables[0].Rows[0][1].ToString();
        //    label_user.Text = dt1.Tables[0].Rows[0][3].ToString();
        //}
    }

    protected void Log_Off(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("login.aspx");
    }
    protected void OpenReservation_Click(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>showFloat();</script>");
        LinkButton lbtn = (LinkButton)sender;
        string ID = lbtn.CommandArgument;
        Response.Write("<script>alert(借教室失败'" + ID.ToString() + "'</script>");

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["userid"] = "1";
        this.Button1.Attributes.Add("OnClick", "return  checkItems()"); //给button添加js的onclick事件，在客户端判断文本框的属性是否正确 
        this.ReserveButton.Attributes.Add("OnClick", "return checkInput()");
        if (Session["userid"].ToString() != null)
        {
            userid = Session["userid"].ToString().Trim();
            string sql = "select contact from users where userId ='" + userid + "'";
            ds = myFunc.getData(sql);
            users = userid + " " + ds.Tables[0].Rows[0]["contact"].ToString().Trim();
            ds.Clear();
        }
        //以下代码用于获取管理员设置的教室可提前预约的天数和需要提前预约的天数
        //DataSet dtSystem = SqlHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [ClassroomRent].[dbo].[classr]");
        //if (dtSystem.Tables[0].Rows[0][1] != null)    //如果有设置则为设置的值
        //{
        //    BookDay = Convert.ToInt32(dtSystem.Tables[0].Rows[0][1].ToString());
        //}
        //if (dtSystem.Tables[0].Rows[0][2] != null)    //如果有设置则为设置的值
        //{
        //    //NeedDay = Convert.ToInt32(dtSystem.Tables[0].Rows[0][2].ToString());
        //    NeedDay = Convert.ToInt32(dtSystem.Tables[0].Rows[0][1].ToString());
        //}
        if (!this.IsPostBack)
        {

            //绑定校区  
            string sql_campus = "select campus from Campus";
            string campus_name = "campus";
            myFunc.DataSourceBind(sql_campus, this.DropDownListCampus, campus_name);
            //绑定教学楼  
            this.DropDownListBuilding.Items.Add("--请选择--");
            //string sql_building = "select buildingName from Buildings where campus ='"+this.DropDownListCompus.SelectedValue.ToString().Trim()+"'";
            //string sql_building = "select buildingName from Buildings";
            //string building_name = "buildingName";
            //myFunc.DataSourceBind(sql_building, this.DropDownListBuilding, building_name);
            //绑定教室
            //string sql_room = "select room from classrooms where campus = '"+this.DropDownListCompus.SelectedValue.ToString().Trim()+"'and building='" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "'";
            //绑定开始时间
            string sql_start = "select startTime from Time";
            string start_name = "startTime";
            myFunc.DataSourceBind(sql_start, this.DropDownListStart, start_name);
            //绑定结束时间
            string sql_end = "select endTime from Time";
            string end_name = "endTime";
            myFunc.DataSourceBind(sql_end, this.DropDownListEnd, end_name);

        }

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (CheckTime() == 0)
        {

        }
        else
        {
            GridView_Binding();
        }

    }
    protected void GridView_Binding()
    {
        SqlConnection myconn = myFunc.CreateConnection();
        myconn.Open();
        DateTime date = DateTime.Now;
        if (Request.Form["chooseDate"] != "选择日期")
        {
            date = Convert.ToDateTime(Request.Form["chooseDate"].ToString().Trim());
        }
        string sql = "select * from classrooms where campus='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "' and building='" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "'";
        if (Request.Form["roomCheckBox"] != null)
        {
            sql += " and room = '" + this.DropDownListRoom.SelectedValue.ToString().Trim() + "'";
        }
        sql += " and room not in ( select room from BorrowLog where campus='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "' and date = '" + date + "' and building='" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "' and ( starttime <= '" + GetHour(this.DropDownListStart.SelectedValue.ToString().Trim()) + ":00' and endtime > '" + GetHour(this.DropDownListStart.SelectedValue.ToString().Trim()) + ":00') or ( starttime < '" + GetHour(this.DropDownListEnd.SelectedValue.ToString().Trim()) + ":00' and endtime >='" + GetHour(this.DropDownListEnd.SelectedValue.ToString().Trim()) + ":00' ) )";
        SqlDataAdapter da = new SqlDataAdapter(sql, myconn);
        da.Fill(ds);
        this.GridView1.DataSource = ds;
        this.GridView1.DataBind();
        int num = this.GridView1.Rows.Count;
        if (num == 0)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('抱歉，该时间段的教室已被占用！')", true);
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                this.GridView1.Rows[i].Cells[4].Text = this.DropDownListStart.SelectedValue.ToString() + "--" + this.DropDownListEnd.SelectedValue.ToString();
            }
            myconn.Close();
        }

    }
    public int IsBorrowed()
    {
        string mysql = "SELECT * FROM BORROWLOG WHERE CAMPUS ='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "'AND BUILDING = '" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "' AND CLASSROOM = '" + this.DropDownListRoom.SelectedValue.ToString().Trim() + "' AND DATE ='" + Request.Params["date"] + "' AND STARTTIME = '" + this.DropDownListStart.SelectedValue.ToString().Trim() + "' AND ENDTIME='" + this.DropDownListEnd.SelectedValue.ToString().Trim() + "'";
        int num = myFunc.Rownum(mysql);
        if (num != 0) return 1;//教室在borrowlog中存在 表示已被借
        return 0;


    }
    public string getUsers()
    {
        return users;
    }
    public void ReserveButton_Click(object sender, EventArgs e)
    {
        //Response.Write("<script>alert(借教室失败'" + "mmp".ToString() + "'</script>");
        string campus, building, room, startTime, endTime, phoneNumber, reason, userId, checkState, sql;
        DateTime date = DateTime.Now;
        phoneNumber = this.TextContact.Text.ToString().Trim();
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('"+phoneNumber+"');</script>");
        reason = this.TextReason.Text.ToString().Trim();
        campus = this.DropDownListCampus.SelectedItem.Text.ToString().Trim();
        building = this.DropDownListBuilding.SelectedItem.Text.ToString().Trim();
        room = this.HiddenField1.Value;
        if (Request.Form["chooseDate"] != "选择日期")
        {
            date = Convert.ToDateTime(Request.Form["chooseDate"].ToString().Trim());
        }
        startTime = this.DropDownListStart.SelectedItem.Text.ToString().Trim();
        endTime = this.DropDownListEnd.SelectedItem.Text.ToString().Trim();
        userId = this.TextUserID.Text.ToString().Trim();
        checkState = "待审核";
        try
        {
            sql = "insert into BorrowLog(campus,building,room,date,starttime,endtime,reason,userID,checkstate) values('" + campus + "','" + building + "','" + room + "','" + date + "'," + GetHour(startTime) + "," + GetHour(endTime) + ",'" + reason + "','" + userId + "','" + checkState + "')";
            myFunc.ExecuteNonQuery(sql);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "myscript", "<script>checkInput()</script>");
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('抱歉，教室借用失败！')", true);
        }
        //if (phoneNumber == null || campus == null || building == null || room == null || reason == null || userid == null || username == null)
        //{
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('请完善信息！')", true);
        //}
        //else
        //{

        //}

        //return 1;

    }
    protected void DropDownListBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        //更改时触发级联事件
        string sql_room = "select room from classrooms where campus ='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "' and building = '" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "'";
        string room_name = "room";
        myFunc.DataSourceBind(sql_room, this.DropDownListRoom, room_name);

    }
    protected void DropDownListCompus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DropDownListCampus.SelectedValue.ToString().Trim() == "--请选择")
        {
            this.DropDownListBuilding.SelectedValue = "--请选择--";

        }
        else
        {
            string sql_building = "select buildingName from Buildings where campus='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "'";
            string room_name = "buildingName";
            myFunc.DataSourceBind(sql_building, this.DropDownListBuilding, room_name);
            this.DropDownListBuilding_SelectedIndexChanged(sender, e);

        }

    }
    public int Room_Bind()
    {
        string sql_room = "select room from classrooms where campus='" + this.DropDownListCampus.SelectedValue.ToString().Trim() + "' and building='" + this.DropDownListBuilding.SelectedValue.ToString().Trim() + "'"; ;
        string room_name = "room";
        myFunc.DataSourceBind(sql_room, this.DropDownListRoom, room_name);
        if (this.DropDownListBuilding.SelectedValue.ToString().Trim() == "--请选择--")
        {
            Response.Write("<script>windows.alert('请选择教学楼')</script>");
        }
        else
        {


        }
        return 1;
    }
    public int release_Room()
    {
        this.DropDownListRoom.ClearSelection();
        return 1;
    }

    public int GetHour(string t)
    {
        string s = "";
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i] != ':') s = s + t[i];
            else break;
        }
        return Convert.ToInt32(s);
    }
    protected int CheckTime()
    {
        int start, end;
        start = GetHour(this.DropDownListStart.SelectedValue.ToString());
        end = GetHour(this.DropDownListEnd.SelectedValue.ToString());
        string date = this.Request.Form["chooseDate"].ToString();
        if (date == null)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert(' 请选择');</script>");
        }
        if (end <= start)
        {
            this.DropDownListEnd.SelectedIndex = 0;
            this.DropDownListStart.SelectedIndex = 0;
            return 0;
        }
        return 1;
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "getRoom")
        {
            Response.Write("<script>alert(借教室失败'" + e.CommandArgument.ToString() + "'</script>");
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.GridView1.DataBind();
    }
}