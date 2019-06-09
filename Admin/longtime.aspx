<%@ Page Language="C#" AutoEventWireup="true" CodeFile="longtime.aspx.cs" Inherits="Admin_longtime" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="" />
<meta name="description" content="" />
     <link rel="stylesheet" href="../css/bootstrap.min.css"/>
        
 <link type="text/css" rel="stylesheet" href="../Date/test/jeDate-test.css"/>
    <link type="text/css" rel="stylesheet" href="../Date/skin/jedate.css"/>
<link href="http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900" rel="stylesheet" />
<link href="../default.css" rel="stylesheet" type="text/css" media="all" />
<link href="../fonts.css" rel="stylesheet" type="text/css" media="all" />


    <script type="text/javascript" src="../Date/src/jedate.js"></script>

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
				<li><a href="../Admin/notice.aspx" accesskey="6" title="">公告栏</a></li>
				<li class="current_page_item"><a href="../Users/notice.aspx" accesskey="7" title="">长期预约</a></li>
            <li><a href="../Admin/user.aspx" accesskey="8" title="">用户管理</a></li>
            
            </ul>
		</div>
	</div>
	<div id="main">
	
	<div>
	<img src="../images/xxkx.jpg" width="20%" style="float:left"/> 
		<h1>学院共享资源管理系统</h1>
		</div>
        
        <div class="container">
            
	     <form id="form1" runat="server" action="longtime.aspx" method="post" name="onTime">
                  <div class="row">
                  
            <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                <select class="form-control" name="campusSelect" id="campus">                    
                    <option value="1">校本部</option>                   
                </select>
                </div>
                       <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 form-group">
                  <select class="form-control" name="buildingSelect" id="building">                    
                    <option value="south">主南</option>                             
                  </select>
                           </div>

                     <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 form-group">

                <select class="form-control" name="roomSelect" id="room">                    
                    <option value="316">316</option>                             
                    <option value="312">312</option>                             
                  
                    </select>
                        
            </div>     
               </div>
             <div class="row">
                 <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                 <select class="form-control" name="Week" id="week">                    
                    <option value="周一">周一</option>                             
                    <option value="周二">周二</option>
                    <option value="周三">周三</option>
                    <option value="周四">周四</option>
                    <option value="周五">周五</option>
                    <option value="周六">周六</option>
                    <option value="周日">周日</option>
                             
                  
                    </select>

                 </div>
                 <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                
                <div class="jeinpbox"><input type="text" name="day" class="jeinput" id="test11" placeholder="YYYY-MM-DD"/></div>
                    
                </div>
                 <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                <div class="jeinpbox"><input type="text" name="hour" class="jeinput" id="test" placeholder="hh"/></div>
     
                     </div>
               
                 </div>
             <div class="row">
                 <div class="col-xs-5 col-sm-5 col-md-4 col-lg-2 form-group">
                        <input class="form-control" name="Reason" id="Reason" />
                 </div>
             </div>
              <button class="btn btn-success btn-default" onclick="document.mylogin.submit()">查询</button>

                 <div id="featured">
			    <div class="title">
			    </div>
			    <ul class="style1">
				    <li class="first"></li>
		
			    </ul>
		    </div>
             <div class="row">
             
                <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" OnDataBound="GridView1_Load">
                    <Columns>
                        <asp:BoundField DataField="building" HeaderText="教学楼" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                            </asp:BoundField>
                          <asp:BoundField DataField="room" HeaderText="教室" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                            </asp:BoundField>
                        <asp:BoundField DataField="date" HeaderText="日期" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                            </asp:BoundField>
                      
                         <asp:BoundField DataField="starttime" HeaderText="开始时间" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                            </asp:BoundField>
                         <asp:BoundField DataField="endtime" HeaderText="结束时间" HeaderStyle-BackColor="#065482" >
<HeaderStyle BackColor="#065482"></HeaderStyle>
                            </asp:BoundField>
                    </Columns>
                    <HeaderStyle BackColor="#065482" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" Wrap="False" />
                
                </asp:GridView>
                 </div>
             
         </form>
	    
            </div>
      
     	
	</div>
</div>
  
    <div style="display:none"><script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript' charset='gb2312'></script></div>
     <script type="text/javascript">
      jeDate("#test11",{
        format: "YYYY-MM-DD",
        multiPane:false,
        range:" 至 "
         });
      jeDate("#test",{
        format: "hh",
        multiPane:false,
        range:" 至 "
    });
    </script>

</body>
</html>
