<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerServiceUpdate.aspx.cs" Inherits="Kaizos.frmCustomerServiceUpdate" MasterPageFile="~/Site.master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="clsCustomerServiceUpdateMain">

    <fieldset id="Fieldset1" runat="server" class ="FieldSet">
        <legend>
            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:LocalString, CSUpdateUser%>"></asp:Label>
        </legend>
        <table>
            <tr>
                <td class="clsLabelRight">
                    <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, CSUpdateEmail%>"></asp:Label> <br />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
                </td>
                <td class="clsLabelRight">
                    <asp:Label ID="lblNewPassword" runat="server" Text="<%$ Resources:LocalString, CSUpdateNewPassword%>"></asp:Label> <br />
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
                        MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <td class="clsLabelRight">
                    <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:LocalString, CSUpdateLanguage%>"></asp:Label> <br />
                </td>
                <td>
                    <asp:DropDownList ID="ddlLanguage" runat="server"> </asp:DropDownList>
                </td>

                <td class="clsLabelRight">
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="<%$ Resources:LocalString, CSUpdateConfirmPassword%>"></asp:Label> <br />
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                        MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox><br />
                </td>
            </tr>
        </table>

    <div class="clsCustomerServiceUpdateButton">
        <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>" Height="21px" 
            onclick="btnSubmit_Click" ValidationGroup="grpCustomerServiceUpdate" />
        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>" Height="21px" 
            onclick="btnCancel_Click" />
    </div>

    <div class="clsCustomerServiceUpdateMandatory">
        <asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>" Font-Size="Smaller"></asp:Label>
    </div>
</fieldset>
</div>
<div class="divSummaryCustomerServiceUpdate">
<table>
<tr><td>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>

<asp:CustomValidator ID="val_CustomerServiceUpdate" runat="server" 
                ControlToValidate="txtEmail" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpCustomerServiceUpdate" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_CustomerServiceUpdate_ServerValidate"></asp:CustomValidator>
</td>
</tr>
</table>
</div>
</asp:Content>    