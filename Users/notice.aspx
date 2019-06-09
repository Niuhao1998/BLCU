<%@ Page Language="C#" AutoEventWireup="true" CodeFile="notice.aspx.cs" Inherits="Users_notice" %>

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
    
<div id="page" class="container">

	<div id="header">
		<div id="logo">
			<img src="../images/my.png" alt="" />
			<h1><a href="#">牛昊</a></h1>
		</div>
		<div id="menu">
            <ul>
                <li><a href="../Users/reservation.aspx" accesskey="1" title="">预约教室</a></li>
                <li><a href="../Users/cancel.aspx" accesskey="2" title="">取消预约</a></li>
                <li><a href="../Users/classroom.aspx" accesskey="3" title="">可借教室</a></li>
                <li><a href="../Users/contact.aspx" accesskey="4" title="">联系管理员</a></li>
                <li class="current_page_item"><a href="#" accesskey="5" title="">公告栏</a></li>
                <li><a href="../Users/longtime.aspx" accesskey="6" title="">长期预约</a></li>

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
	<form id="form1" runat="server">
    
		<div id="welcome">
			
			</div>
			<div class="title">
				
		</div>
		<div id="featured">
			<div class="title">
                	<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="579px">
                    <Columns>
                        <asp:BoundField DataField="标题" HeaderText="标 题" />
                        <asp:BoundField DataField="发布时间" HeaderText="发布时间" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkselect" Text="链接" runat="server"  onclick="lnkselect_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" Font-Size="Larger" />
                
				</asp:GridView>
			</div>
			<ul class="style1">
				<li class="first">
		
			</ul>
		</div>
		
	</div>
</div>
        <div>
        </div>
    </form>
    <div style="display:none"><script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript' charset='gb2312'></script></div>

</body>
</html>
