﻿<%@ page language="VB" autoeventwireup="false" inherits="Login, App_Web_jtvwotiq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">




<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login to the security token service (STS)</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 125px;
        }
        .style3
        {
            font-size: large;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="style3">
    
        STS Provider </div>
    <p class="style3">
        &nbsp;</p>
    <asp:Label ID="Label3" runat="server" style="font-weight: 700" 
        Text="Login to the STS"></asp:Label>
&nbsp;<br />    
    
    <table class="style1">
        <tr>
            <td class="style2">
                <asp:Label ID="Label1" runat="server" Text="User name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="149px">password</asp:TextBox>
            </td>
        </tr>
    </table>
    <p>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
    </p>
    </form>
<p>
    
</body>
</html>
