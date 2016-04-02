<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAuthorizedUpdate.aspx.cs" Inherits="Kaizos.frmAuthorizedUpdate" MasterPageFile="~/NewSite.master" validateRequest="false"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="divSummaryAZU">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, ValidationMaxAllowed %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLength %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNotAvailable" Text="<%$ Resources:LocalString, valNotAvailable %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_AZU" runat="server" 
        ControlToValidate="txtEmail" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpConfirmPage" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_AZU_ServerValidate">
    </asp:CustomValidator>
</div>

<fieldset id="Fieldset1" runat="server" class ="first">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, AuthorizedUser%>"></asp:Label>
    </legend>
    <label for="txtCompanyName">
        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, AddressBookListCompanyName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtEmail">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, AZUEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" ></asp:TextBox>
    
    <label for="txtPhoneNo">
        <asp:Label ID="lblPhoneNo" runat="server" Text="<%$ Resources:LocalString, AZUPhoneNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="20" ></asp:TextBox>
    
    <asp:CheckBox ID="chkDelete" runat="server" text = "<%$ Resources:LocalString, AZUDeleteUser%>" TextAlign="Left"/>
</fieldset>

<div id="buttons">
    <asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
    <asp:Button ID="btnUpdate" runat="server" Text="<%$ Resources:LocalString, AllUpdate%>"
            ValidationGroup="grpConfirmPage" onclick="btnUpdate_Click" />
</div>
</asp:Content>