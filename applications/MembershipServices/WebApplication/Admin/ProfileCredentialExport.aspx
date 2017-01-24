<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProfileCredentialExport.aspx.cs" Inherits="WebApplication.Admin.ProfileCredentialExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnGeneratePasswords" runat="server" OnClick="btnGeneratePasswords_onClick" Text="Generate Passwords"/><br/>
    <asp:Button ID="btnEncryptPasswords" runat="server" OnClick="btnEncryptPasswords_onClick" Text="btnEncryptPasswords" /><br/>
    
    </div>
    </form>
</body>
</html>
