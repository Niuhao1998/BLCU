<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="Admin_contact" %>

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
                <li><a href="../Admin/reservation.aspx" accesskey="1" title="">预约教室</a></li>
                <li><a href="../Admin/cancel.aspx" accesskey="2" title="">取消预约</a></li>
                <li><a href="../Admin/Cancel.aspx" accesskey="3" title="">审核消息</a></li>
               
                <li><a href="../Admin/classroom.aspx" accesskey="4" title="">可借教室</a></li>
                 
                <li class="current_page_item"><a href="#" accesskey="5" title="">管理员设置</a></li>
                <li><a href="../Admin/notice.aspx" accesskey="6" title="">公告栏</a></li>
                <li><a href="../Admin/longtime.aspx" accesskey="7" title="">长期预约</a></li>
                <li><a href="../Admin/user.aspx" accesskey="8" title="">用户管理</a></li>
            
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

                
			<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" ShowFooter="true" 
                OnRowCommand="GridView1_RowCommand" OnRowEditing="GridView1_RowEditing" 
                OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" 
                OnRowDeleting="GridView1_RowDeleting">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="姓名">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("姓名") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFirstName" Text='<%# Eval("姓名") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFirstNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="联系方式">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("联系方式") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSecondName" Text='<%# Eval("联系方式") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSecondNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="QQ">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("QQ") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtThirdName" Text='<%# Eval("QQ") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtThirdNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="微信">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("微信") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLastName" Text='<%# Eval("微信") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtLastNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/pic/修改.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" />
                                 <asp:ImageButton ImageUrl="~/pic/删除.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />

                            </ItemTemplate>

                            <EditItemTemplate>
                                  <asp:ImageButton ImageUrl="~/pic/保存.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                                  <asp:ImageButton ImageUrl="~/pic/取消.jpg" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />

                            </EditItemTemplate>

                            <FooterTemplate>
                                 <asp:ImageButton ImageUrl="~/pic/addnew.jpg" runat="server" CommandName="AddNew" ToolTip="AddNew" Width="20px" Height="20px" />

                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" Font-Size="Larger" />
                
				</asp:GridView>

                </div>
		<div id="featured">
			
            </div>
			<ul class="style1">
				<li class="first">
		
			</ul>
		
		
	</div>
</div>
   
      
    </form>

    <div style="display:none"><script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript' charset='gb2312'></script></div>

</body>
</html>
