<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user.aspx.cs" Inherits="Admin_user" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="" />
<meta name="description" content="" />
<link href="http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900" rel="stylesheet" />
<link href="../bootstrap.min.css" rel="stylesheet" type="text/css" media="all" />
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
			<h1><a href="#">牛昊</a></h1>
		</div>
		<div id="menu">
            <ul>
                <li><a href="../Admin/reservation.aspx" accesskey="1" title="">预约教室</a></li>
                <li><a href="../Admin/Cancel.aspx" accesskey="2" title="">取消预约</a></li>
                 <li ><a href="#" accesskey="3" title="">审核消息</a></li>
                <li><a href="../Admin/classroom.aspx" accesskey="4" title="">可借教室</a></li>
                <li><a href="../Admin/contact.aspx" accesskey="5" title="">管理员设置</a></li>
                <li><a href="../Admin/notice.aspx" accesskey="6" title="">公告管理</a></li>
                <li><a href="../Admin/longtime.aspx" accesskey="7" title="">长期预约</a></li>
                <li class="current_page_item"><a href="../Admin/user.aspx" accesskey="8" title="">用户管理</a></li>
            </ul>
		</div>
	</div>
	<div id="main">
	
	<div>
	<img src="../images/xxkx.jpg" width="20%" style="float:left"/> 
		<h1>学院共享资源管理系统</h1>
		</div>
		<div id="banner">
			
		</div>
	
		<div id="welcome">
			
			
			</div>
        <div>  
                <asp:Label ID="Label1" runat="server" Text="学号"></asp:Label>
                <asp:TextBox ID="TextBox5" runat="server" Width="106px" Height="17px"></asp:TextBox>
               
                <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
                <asp:TextBox ID="TextBox6" runat="server" Width="116px"></asp:TextBox>
                联系方式<asp:TextBox ID="TextBox7" runat="server" Width="147px"></asp:TextBox>
                <br />
                <br />
                学校<asp:TextBox ID="TextBox8" runat="server" Width="106px"></asp:TextBox>
&nbsp;班级 
                <asp:TextBox ID="TextBox9" runat="server" Width="102px"></asp:TextBox>
&nbsp;专业 
                <asp:TextBox ID="TextBox10" runat="server" Width="125px"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="添加" Width="60px" />
			    <br />
                <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" Text="批量添加" OnClick="Button2_Click" />
           
			
			    &nbsp;<asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="确定" />
                <br />
           
			
			    <br />
               
            </div>
        <div>
<asp:GridView ID="GridView1" runat="server"  cssclass="table table-striped table-bordered" Width="777px" Height="205px"  OnRowDeleting="GridView1_RowDeleting">
                    <Columns>
                        
                        <asp:CommandField ButtonType="Button" HeaderText="操作" ShowDeleteButton="True" />

                       <%-- <asp:BoundField DataField="学号" ReadOnly="true" HeaderText="学号" ControlStyle-BackColor="#065482" HtmlEncode ="false"   >
<ControlStyle BackColor="#065482"></ControlStyle>
                        </asp:BoundField>--%>
                    </Columns>
      <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" Font-Size="Larger" />
                
                </asp:GridView>
        </div>
		<div id="featured">
            
            <div> 
            </div>
			<div class="title">
			    
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
