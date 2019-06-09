<%@ Page Language="C#" AutoEventWireup="true" CodeFile="classroom.aspx.cs" Inherits="Admin_classroom" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="" />
<meta name="description" content="" />
<title>学院共享资源管理系统</title>
<link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="../css/bootstrap-datepicker.css" rel="stylesheet"/>	
<link href="../css/default.css" rel="stylesheet" type="text/css"/>
<link href="../css/fonts.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
<script src="../script/jquery.min.js"></script>
<script src="../script/bootstrap.js"></script>
<script src="../script/bootstrap-datepicker.js"></script>
<script src="../script/Controls.js"></script>
<div id="page">
  <div id="header">
    <div id="logo">
    	<img src="../images/my.png" alt="logo"/>
		<h3><a href="#">牛昊</a></h3>
    </div>
    <div id="menu">
            <ul>
                <li ><a href="../Admin/reservation.aspx" accesskey="1" title="">预约教室</a></li>
                <li><a href="../Admin/Cancel.aspx" accesskey="2" title="">取消预约</a></li>
                <li class="current_page_item"><a href="#" accesskey="3" title="">可借教室</a></li>
                <li><a href="../Admin/contact.aspx" accesskey="4" title="">联系管理员</a></li>
                <li><a href="../Admin/contact.aspx" accesskey="5" title="">管理员设置</a></li>
                <li><a href="../Admin/notice.aspx" accesskey="6" title="">公告栏</a></li>
                <li><a href="../Admin/longtime.aspx" accesskey="7" title="">长期预约</a></li>
                <li><a href="../Admin/user.aspx" accesskey="8" title="">用户管理</a></li>
            
            </ul>
		</div>
	</div>
</div>
	<div id="main">
	    <div class="title">
	        <img src="../images/xxkx.jpg" width="10%" style="float:left"/> 
		    <h1>学院共享资源管理系统</h1>
	    </div>        
           <div class="container">
            <form id="form2" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
            <div class="row">
                <div class="col1 form-group">
                    <asp:DropDownList cssclass="form-control" name="campusSelect" ID="DropDownListCampus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCompus_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col2 form-group">
                    <asp:DropDownList cssclass="form-control" name="buildingSelect" ID="DropDownListBuilding" runat="server" DataTextField="buildingName" DataValueField="buildingName" OnSelectedIndexChanged="DropDownListBuilding_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="col3 form-group">
                    <asp:DropDownList  cssclass="form-control" name="roomSelect" ID="DropDownListRoom" runat="server" DataTextField="room" DataValueField="room" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
             </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownListBuilding"/>
                    <asp:AsyncPostBackTrigger ControlID="DropDownListRoom"/>
                    <%--<asp:AsyncPostBackTrigger ControlID="roomCheckBox"/>--%>
               </Triggers>
            </asp:UpdatePanel>
            <div class="row">
                 <div class="col1 form-group">
                    <div class="input-append date">
                    	<span class="add-on date-add">
                       		<span class="arrow"></span>
	                    </span>
                	    <input class="form-control1" name="chooseDate" id="date" type="text" value="选择日期" /></div>
                    </div>
                <div class="col2 form-group">
                     <asp:DropDownList  cssclass="form-control" name="Start_Time" ID="DropDownListStart" runat="server" DataTextField="开始时间" DataValueField="开始时间">
                     </asp:DropDownList>
                </div>
                <div class="col3 form-group">
                    <asp:DropDownList  cssclass="form-control" name="End_Time" ID="DropDownListEnd" runat="server" DataTextField="结束时间" DataValueField="结束时间">
                    </asp:DropDownList>
                </div> 
             </div>  
             <div class="row1">
                 <div>
                    <input name="roomCheckBox" id="roomCheck" type="checkbox" onchange="if (this.checked){alert('asf') <%=Room_Bind()%>}else{<%=release_Room() %>>}"/>是否将教室号加入查询条件<br/>
                    <%--<asp:CheckBox  style="left:10px" ID="CheckBox1" runat="server" Width="20px" ViewStateMode="Disabled" AutoPostBack="True" />--%>
                 </div>
             </div>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <div class="search_button" >
                    <asp:Button cssclass="btn btn-success btn-default" ID="Button1"  runat="server" text="查询" Font-Size="14px" Width="60px" OnClick="Button1_Click"/>
                </div>
                <hr/>
            <div class="row">
                <table class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th >校区</th>
                            <th >教学楼</th>
                            <th >教室</th>
                            <th >类型</th>
                            <th >时间段</th>
                        </tr>
                    </thead>
                </table>
                <div id="reservation" style="border-radius:20%;border:solid 1px #555555;background:#fff;padding:10px;width:500px;height:400px;z-index:1001; position: absolute; display:none;top:20%; left:40%;"> 
                    <div style="padding:3px 15px 3px 15px;text-align:center;vertical-align:middle;" > 
                    <div> 
                      <label>校区：</label> 
                      <asp:TextBox ID="TextCampus" runat="server" ReadOnly="True"></asp:TextBox> 
                    </div> 
                    <div> 
                      <label>教学楼：</label> 
                      <asp:TextBox ID="TextBuilding" runat="server" ReadOnly="True"></asp:TextBox> 
                    </div> 
                    <div> 
                      <label>教室：</label> 
                      <asp:TextBox ID="TextRoom" runat="server" ReadOnly="True"></asp:TextBox> 
                    </div> 
                    <div> 
                      <label>学号：</label> 
                      <asp:TextBox ID="TextUserID" runat="server" ReadOnly="True" ></asp:TextBox> 
                    </div> 
                    <div> 
                      <label>联系方式：</label>
                      <asp:TextBox ID="TextContact" runat="server" > </asp:TextBox> 
                    </div> 
                    <div> 
                      <label >原因：</label>
                      <asp:TextBox ID="TextReason" runat="server" width="300px" Height="100px"> </asp:TextBox> 
                    </div> 
                    <br/> 
                    <div style="text-align:center"> &nbsp; &nbsp;
                        <asp:Button CssClass="btn btn-success btn-default" ID="ReserveButton" runat="server" Text=" 预 约 " Font-Size="14px" Width="60px" OnClick="ReserveButton_Click" />&nbsp; 
                        <%--<input class="btn btn-success btn-default" id="Button2" type="button" value=" 预 约 " onclick="Reservation()" style=" Font-Size:14px;Width:60px;bottom:10px;" />--%>
                        <input class="btn btn-success btn-default" id="CancelButton" type="button" value=" 取 消 " onclick="ShowNo()" style=" Font-Size:14px;Width:60px;bottom:10px;" /> 
                    </div> 
                </div> 
                </div>
                <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%> 
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:GridView style="float:left" CssClass="table table-striped table-bordered" ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="1055px" CaptionAlign="Bottom" ClientIDMode="Static" ShowHeader="False" BorderColor="#555555" BorderStyle="Solid"  OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5">
                        <EditRowStyle  />
                        <HeaderStyle HorizontalAlign="Center"  />
                        
                        <Columns>
                            <asp:BoundField HeaderText="校区" DataField="campus">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle BorderStyle="Solid" HorizontalAlign="Center" Width="20%" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="教学楼" DataField="building" >
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="教室" DataField="room" >
                            
                            <HeaderStyle HorizontalAlign="Center" BorderStyle="Solid" Width="20%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="类型" DataField="type" >
                            
                            <HeaderStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="时间段">
                            <ControlStyle  Width="167px" />
                            <HeaderStyle Width="20%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                            </asp:BoundField>
                            
                        </Columns>
                        <PagerTemplate>
                        <table  style="width:100%;font-size: 12px;">
                        <tr>
                            <td style="text-align: center">
                                <%#((GridView)Container.NamingContainer).PageIndex+1 %>
                                /
                                <%#((GridView)Container.NamingContainer).PageCount %>
                                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                    Enable="<%#((GridView)Container.NamingContainer).PageIndex!=0 %>">首页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="prev"
                                    CommandName="Page" Enable="<%#((GridView)Container.NamingContainer).PageIndex!=0 %>">上一页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                    Enable="<%#((GridView)Container.NamingContainer).PageIndex!=((GridView)Container.NamingContainer).PageCount-1 %>">下一页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                    Enable="<%#((GridView)Container.NamingContainer).PageIndex!=((GridView)Container.NamingContainer).PageCount-1 %>">尾页</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </PagerTemplate>
                        <SelectedRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:GridView>
		</ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Button1" />
                    <asp:AsyncPostBackTrigger ControlID="ReserveButton" />
                   
                </Triggers>
            </asp:UpdatePanel>
         </div>
            </form>
           </div>
    </div>
<script type="text/JavaScript">
    var baseDatesDisabled = ['01.01, 2017', '04.17, 2017', '05.01, 2017', '06.05, 2017', '07.14, 2017', '08.15, 2017', '11.01, 2017', '11.11, 2017', '12.25, 2017'];
    $(".input-append").datepicker({
        format: 'yyyy-mm-dd',
        language: 'cn',
        orientation: 'bottom',
        autoclose: true,
        datesDisabled: baseDatesDisabled
    });
    function showFloat(room)                    //根据屏幕的大小显示两个层 
    {
        //var range = getRange();
        document.getElementById("reservation").style.display = "block";
        document.getElementById("TextCampus").value = document.getElementById("DropDownListCampus").value;
        document.getElementById("TextBuilding").value = document.getElementById("DropDownListBuilding").value;
        //alert(document.getElementsByClassName("reservation").attributes["id"].value);
        document.getElementById("TextRoom").value = room;
        document.getElementById("HiddenField1").value = room;
        var str = "<%=getUsers()%>";
        document.getElementById("TextUserID").value = str.split(" ")[0];
        document.getElementById("TextContact").value = str.split(" ")[1];


    }
    </script>
<div style="display:none"><script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript' charset='gb2312'></script></div>

</body>
</html>
