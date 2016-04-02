<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMain.aspx.cs" Inherits="Kaizos.frmMain" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head2" runat="server">
    <title></title>
    <link href="~/CSS/Kaizos.css" rel="stylesheet" type="text/css" />
</head>

<body class = "clsMainBody">
    <form id="Form1" runat="server">
        <div class="title">
            <h1 class = "clsMainH"> Kaizos</h1>
            <hr/>
        </div>

        <div class="clsMain">
            <table>
                <tr>
                    <td class="clsLabelRight">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="frmLogin.aspx" >
                            <asp:Label ID="lblLogin" runat="server" Text="Login"></asp:Label>
                        </asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="clsLabelRight">
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="frmEndCustomer.aspx" >
                            <asp:Label ID="lblCreateAccount" runat="server" Text="Create an Account"></asp:Label>        
                        </asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
        <hr>
    </form>
</body>
</html>