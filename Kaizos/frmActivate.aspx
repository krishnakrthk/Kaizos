<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmActivate.aspx.cs" Inherits="Kaizos.frmActivate" %>

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
                <asp:Label ID="lblPWDOldPasswordPassword" runat="server" Text="<%$ Resources:LocalString, ActivatePassword%>"></asp:Label>
            </legend>
            <table>
            <tr>
                <td>
                    <asp:Label ID="lblNewPassword" runat="server" Text="<%$ Resources:LocalString, ActivateNewPassword%>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="60" 
                        TextMode="Password" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>
                </td>
            </tr>
        </table>

            <div class="clsButton">
                <asp:Button ID="btnActivatePassword" runat="server" 
                    Text="<%$ Resources:LocalString, AllActivate%>" 
                    ValidationGroup = "grpActivatePage" onclick="btnActivatePassword_Click"/>
            </div>

            <div class="clsMandatory">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
            </div>
        </fieldset>
    </div>

<div class="divSummaryActivatePassword">
<table>
<tr><td>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLength %>" runat ="server" Visible= "false"></asp:Label>

<asp:CustomValidator ID="val_ActivatePasword" runat="server" 
                ControlToValidate="txtNewPassword" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpActivatePage" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_ActivatePasword_ServerValidate" ></asp:CustomValidator>
</td>
</tr>
</table>
</div>
    </form>
</body>
</html>
