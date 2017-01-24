<%@ Page Language="C#" MasterPageFile="~/Nsnu.master" AutoEventWireup="true" CodeBehind="ViewVersion.aspx.cs" Inherits="WebApplication.Admin.ViewVersion" %>


<asp:Content ID="cMainContent" ContentPlaceHolderID="cphMainContent" Runat="Server">
	<div class="section page-heading row clearfix">
	    <div class="grid_12">
			<h1>Version History</h1>
		</div>
    </div>
	<div class="section row clearfix">			
		<div class="main grid_12">
			<asp:Literal runat="server" ID="litVersionData" />
		</div>
	</div>
</asp:Content>
