<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登陆</title>
      <link rel="stylesheet" href="./css/bootstrap.min.css"/>
        
    <link rel="stylesheet" href="../css/normalize.css"/>
	<link rel="stylesheet" href="../css/common.css"/>
	<link rel="stylesheet" href="../css/main.css"/>
    <link href="../fonts.css" rel="stylesheet" type="text/css" media="all" />

</head>
<body>
    <header>
    <div class="logo">
			<img src="../pic/logo.jpg" alt="北语信科logo" class="logo-img"/>
			<h1 class="vertical-center">学院共享资源管理系统</h1>
		</div>
        </header>
    	<div class="login-container">
		<section class="img-display vertical-center">
			<img src="../pic/library.png" alt=""/>
			<img src="../pic/books1.png" alt=""/>
			<img src="../pic/seat.png" alt=""/>
			<img src="../pic/computer.png" alt=""/>
		</section>
		<section class="login vertical-center">            
			<form action="login.aspx" name="mylogin" method="post" id="mylogin" runat="server">
                <div><select class="form-control" style="width:313px;" name="choose">
                    <option value="user" >用户端</option>
                    <option value="admin">管理员端</option>
                     </select></div>
                 <div><span>学号：</span><input type="text" id="login_num" maxlength="12" name="id"/></div>
				<div><span>密码：</span><input type="password" id="login_pwd" name="password"/></div>
				<button class="login-btn"  onclick="document.mylogin.submit()" >Login</button>
			</form>
		</section>
	</div>  
</body>
</html>