<%@ Page Language="C#" AutoEventWireup="true" Inherits="WebApplication.Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nova Scotia Nurses Union - Login</title>
	<link href="Styles/base.css" rel="stylesheet" type="text/css" />
    <link href="Styles/nsnu.css" rel="stylesheet" type="text/css" />
    <link href="Styles/ui-lightness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <!--[if gte IE 9]>
	  <style type="text/css">
		#header, .btn {
		   filter: none;
		}
	  </style>
	<![endif]-->
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-1.6.2.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>    
</head>
<body id="nsnu-login">
    <form id="form1" runat="server">
        <div id="container" class="clearfix">
			<div id="header" role="banner" class="clearfix">
	
		        <div class="masthead">
			        <div class="row clearfix">
				        <div class="grid_12">
					        <h1 class="branding ir"><a href="index.html">Nova Scotia Nurses Union</a></h1>
					    </div>
			        </div>
		        </div>
	
	        </div>
			
            <div id="content_wrap" class="content">
				<div class="row clearfix">
					<div class="grid_12">
						<div id="login_form">
							<h1>Administration Login</h1>
							<div class="wrapper-block clearfix">
							
								<label>Username<span class="required">&bull;</span></label>
								<asp:TextBox ID="tbUserName" runat="server" MaxLength="50"></asp:TextBox><br />
							    <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ControlToValidate="tbUserName" ErrorMessage="Username is a required field." SetFocusOnError="true" />
								<label>Password<span class="required">&bull;</span></label>
								<asp:TextBox ID="tbPassword" TextMode="Password" runat="server" MaxLength="20"></asp:TextBox><br />
							    <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="tbPassword" ErrorMessage="Password is a required field." SetFocusOnError="true" />
								<br />
								<asp:button ID="btnSubmit" CssClass="red-btn" runat="server" text="Login" OnClick="btnSubmit_onClick"/> <a id="lnkClear" class="clear-btn" href="#">Clear</a>
							
							</div>
						</div>
					</div>
				</div>
            </div>
			<div id="footer" role="contentinfo">
		
				<div class="attribution wrapper">	
					<div class="row clearfix">
						<div class="grid_12">
							<p>&copy; Nova Scotia Nurses Union 2013</p>
						</div>
					</div>
				</div>
		
			</div>
        </div>


	</form>
</body>
</html>

<script type="text/javascript">

    $(document).ready(function () {
        $("#lnkClear").click(function () {
            $("#tbUserName").val("");
            $("#tbPassword").val("");
        });
    });

</script>

