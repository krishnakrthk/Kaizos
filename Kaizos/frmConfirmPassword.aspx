<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmConfirmPassword.aspx.cs" Inherits="Kaizos.frmConfirmPassword" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head2" runat="server">
    <title></title>
    <link href="~/CSS/Kaizos.css" rel="stylesheet" type="text/css" />
</head>

<body class="clsSiteMain">
    <form id="Form1" runat="server">
     <div class="MainHeader">
         <div class="MainTitle">
            <table width="100%">
            <tr>
                <td class="AppLogo" valign="middle"></td>
            </tr>
            
            </table>
        </div>
        </div>
    <div class="clsConfirmPassword">

        <fieldset id="Fieldset1" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="lblPWDConfirmPassword" runat="server" Text="<%$ Resources:LocalString, ConfirmPWDConfirmPassword%>"></asp:Label>
            </legend>
            <table>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:LocalString, ConfirmPWDPassword%>"></asp:Label> <br /><br />
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" 
                        ValidationGroup="grpValConfirmPassword" TextMode="Password" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox><br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="<%$ Resources:LocalString, ConfirmPWDPasswordConfirmation%>" ></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" 
                        ValidationGroup="grpValConfirmPassword" TextMode="Password" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>
                </td>
            </tr>
        </table>

            <div class="clsButton">
                <asp:Button ID="btnConfirmPassword" runat="server" Text="<%$ Resources:LocalString, AllUpdate%>" Height="21px" 
                    onclick="Button1_Click" ValidationGroup="grpConfirmPage" />
            </div>

            <div class="clsMandatory">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
            </div>
        </fieldset>
    </div>

<div class="divSummaryConfirmPassword">
<table>
<tr><td>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLength %>" runat ="server" Visible= "false"></asp:Label>

<asp:CustomValidator ID="val_ConfirmPasword" runat="server" 
                ControlToValidate="txtConfirmPassword" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpConfirmPage" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_ConfirmPage_ServerValidate"></asp:CustomValidator>
</td>
</tr>
</table>
</div>
    </form>
</body>
</html>
