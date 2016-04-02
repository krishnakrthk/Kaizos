<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAuthorized.aspx.cs" Inherits="Kaizos.frmAuthorized" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="divSummaryAuthorized">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, ValidationMaxAllowed %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLength %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNotAvailable" Text="<%$ Resources:LocalString, valNotAvailable %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_Authorized" runat="server" 
        ControlToValidate="txtName" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpAuthorizedPage" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_ConfirmPage_ServerValidate">
    </asp:CustomValidator>
</div>

<fieldset id="Fieldset1" runat="server" class ="first">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, AuthorizedUser%>"></asp:Label>
    </legend>
    <label for="txtName">
        <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, AuthorizedName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtEmail">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, AuthorizedEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtPhoneNo">
        <asp:Label ID="lblPhoneNo" runat="server" Text="<%$ Resources:LocalString, AuthorizedPhoneNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="20"></asp:TextBox>
</fieldset>

<div id="buttons">
    <asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>"></asp:Label>
    <asp:Button ID="btnSubmit" runat="server" 
        Text="<%$ Resources:LocalString, AllSubmit%>" 
        ValidationGroup = "grpAuthorizedPage" onclick="btnSubmit_Click"/>
    <asp:Button ID="btnCancel" runat="server" 
        Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click"/>
</div>
</asp:Content>