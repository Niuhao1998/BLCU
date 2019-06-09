<%@ Page Language="C#" AutoEventWireup="true" CodeFile="notice.aspx.cs" Inherits="Admin_notice" %>

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
                 <li><a href="../Admin/Review.aspx" accesskey="3" title="">审核消息</a></li>
               
                <li><a href="../Admin/classroom.aspx" accesskey="4" title="">可借教室</a></li>
                <li><a href="../Admin/contact.aspx" accesskey="5" title="">管理员设置</a></li>
                <li class="current_page_item"><a href="#" accesskey="6" title="">公告栏</a></li>
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
				
		</div>
		<div id="featured">
			<div class="title">                    
                    <div class="row">                        
                 <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                        <h3>标题</h3>
                     <input class="form-control" name="Reason" id="Reason" />
                 </div>
             </div>
                <asp:FileUpload ID="fUpload" runat="server" />
                <asp:Button ID="btClick" runat="server" OnClick="btClick_Click" Text="上传" />
                   <asp:Label ID="lbText" runat="server"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" ShowFooter="false" 
                OnRowEditing="GridView1_RowEditing"
                OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating"
                OnRowDeleting="GridView1_RowDeleting" Width="593px">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="标题">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("标题") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFirstName" Text='<%# Eval("标题") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFirstNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="发布时间">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("发布时间") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSecondName" Text='<%# Eval("发布时间") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSecondNameFooter" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="链接">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("链接") %>' runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtThirdName" Text='<%# Eval("链接") %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtThirdNameFooter" runat="server" />
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

                           </asp:TemplateField>
                    </Columns>
                <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" Font-Size="Larger" />
                
				</asp:GridView>

            </div>
   
            </div>
            <div>

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
