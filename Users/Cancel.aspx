<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cancel.aspx.cs" Inherits="Users_Cancel" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="" />
<meta name="description" content="" />
<link href="http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900" rel="stylesheet" />
<link href="../default.css" rel="stylesheet" type="text/css" media="all" />
<link href="../fonts.css" rel="stylesheet" type="text/css" media="all" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
    
<div id="page" class="container">

	<div id="header">
		<div id="logo">
			<img src="../images/my.png" alt="" />
			<h1><a href="#"><% =Session["userid"] %></a></h1>
		</div>
		<div id="menu">
            <ul>
                <li><a href="../Users/reservation.aspx" accesskey="1" title="">预约教室</a></li>
                <li class="current_page_item"><a href="#" accesskey="2" title="">取消预约</a></li>
                <li><a href="../Users/classroom.aspx" accesskey="3" title="">可借教室</a></li>
                <li><a href="../Users/contact.aspx" accesskey="4" title="">联系管理员</a></li>
                <li><a href="../Users/notice.aspx" accesskey="5" title="">公告栏</a></li>
                <li><a href="../Users/longtime.aspx" accesskey="6" title="">长期预约</a></li>
            </ul>
		</div>
	</div>
	<div id="main">
	    <div>
	    <img src="../images/xxkx.jpg" width="20%" style="float:left"/> 
		    <h1>学院共享资源管理系统</h1>
		    <div id="banner">
		    </div>
	
		    <div id="welcome">
			    <p><%=welcome %></p>
               
		    </div>
		    <div class="title">
				
		        <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" OnDataBound="GridView1_Load">
                    <Columns>
                         
 
<%--                        <asp:BoundField DataField="Column0" HeaderText="Column0" />
 
                        <asp:BoundField DataField="Column1" HeaderText="Column1" />
 
                        <asp:BoundField DataField="Column2" HeaderText="Column2" />

                        <asp:CommandField ShowDeleteButton="True" />--%>
 
                        <asp:BoundField DataField="userID" HeaderText="借用人学号" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="date" ReadOnly="true" HeaderText="借用日期" ControlStyle-BackColor="#065482" HtmlEncode ="false" DataFormatString="{0:yyyy/MM/dd}"  >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="starttime" HeaderText="借用时间" ControlStyle-BackColor="#065482" >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="endtime" HeaderText="归还时间" ControlStyle-BackColor="#065482" >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="building" HeaderText="教学楼" ControlStyle-BackColor="#065482" >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="room" HeaderText="借用教室" ControlStyle-BackColor="#065482" >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="reason" HeaderText="借用事由" ControlStyle-BackColor="#065482" ControlStyle-BorderStyle="Solid" >
<ControlStyle BackColor="#065482" BorderStyle="Solid"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="checkstate" HeaderText="是否通过审核" ControlStyle-BackColor="#065482" ControlStyle-BorderStyle="Solid" >
<ControlStyle BackColor="#065482" BorderStyle="Solid"></ControlStyle>
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="canceling" runat="server" CausesValidation="False" CommandName="Edit" Text="取消预约" OnClientClick="return confirm('您确认要删除吗？')"  OnClick="canceling_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>
 
                     </Columns>
                    <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" />
                </asp:GridView>
				
		    </div>
		    <div id="featured">
			    <div class="title">
			    </div>
			    <ul class="style1">
				    <li class="first"></li>
		
			    </ul>
		    </div>
	    </div>
    </div>
</div>
        <div>
        </div>
    </form>
    <div style="display:none"><script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript' charset='gb2312'></script></div>

</body>
</html>
