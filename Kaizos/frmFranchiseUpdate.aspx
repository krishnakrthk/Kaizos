<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFranchiseUpdate.aspx.cs" Inherits="Kaizos.frmFranchiseUpdate" MasterPageFile="~/NewSite.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<div class="divSummaryFranchiseUpdate">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, ValidationMaxAllowed %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
    <asp:CustomValidator ID="val_FranchiseUpdate" runat="server" 
        ControlToValidate="txtEmail" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpFranchiseUpdate" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_FranchiseUpdate_ServerValidate" >
    </asp:CustomValidator>
</div>

<fieldset id="Fieldset1" runat="server" class ="second">
    <legend>
        <asp:Label ID="Label20" runat="server" Text="Label">User</asp:Label>
    </legend>
    <label for="txtEmail">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtEmail">
        <asp:Label ID="lblNewPassword" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateNewPassword%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
         MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>
    
    <label for="txtEmail">
        <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateLanguage%>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="True"> </asp:DropDownList>
    
    <label for="txtEmail">
        <asp:Label ID="lblConfirmPassword" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateConfirmPassword%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
         MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>

    <asp:Button ID="btnLoad" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateLoad%>"
         onclick="btnLoad_Click" />

</fieldset>

<fieldset id="Fieldset2" runat="server" class ="second">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateCompany%>"></asp:Label>
    </legend>
    
    <label for="txtCompanyName">
        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateCompanyName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtAddress1">
        <asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAddress1%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtAddress2">
        <asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAddress2%>"></asp:Label>          
    </label>
    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtLegalForm">
        <asp:Label ID="lblLegalForm" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateLegalForm%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtLegalForm" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtAddress3">
        <asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAddress3%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox>         
    
    <label for="txtCommercialName">
        <asp:Label ID="lblCommercialName" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateCommercialName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCommercialName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtZipcode">
        <asp:Label ID="lblZipcode" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateZipcode%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtZipcode" runat="server">12</asp:TextBox>
    
    <label for="txtManPower">
        <asp:Label ID="lblManPower" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateManPower%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtManPower" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtCity">
        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateCity%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtcEmail">
        <asp:Label ID="lblcEmail" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdatelblEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtcEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="ddlCountry">
        <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateCountry%>"></asp:Label>    
    </label>
    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True"> </asp:DropDownList>
    
    <label for="txtPhoneNumber">
        <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdatePhoneNumber%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
    
    <label for="rtxtChalandiseZone">
        <asp:Label ID="lblChalandiseZone" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateChalandiseZone%>"></asp:Label>
    </label>
    <asp:TextBox ID="rtxtChalandiseZone" runat="server" TextMode="MultiLine" ReadOnly="True" class="smallArea"></asp:TextBox>
    
    <label for="txtFaxNumber">
        <asp:Label ID="lblFaxNumber" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateFaxNumber%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtFaxNumber" runat="server" MaxLength="20"></asp:TextBox>
    
    <label for="txtSiretNo">
        <asp:Label ID="lblSiretNo" runat="server" Text="<%$ Resources:LocalString, FranchiseSiretNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtSiretNo" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtVatNo">
        <asp:Label ID="lblVatNo" runat="server" Text="<%$ Resources:LocalString, FranchiseVatNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtVatNo" runat="server" MaxLength="30"></asp:TextBox>
</fieldset>

<div id="buttons">
   <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
   <asp:Button ID="bbtSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>"
        onclick="bbtSubmit_Click" ValidationGroup="grpFranchiseUpdate" />
    <asp:Button ID="bbtCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>"
        onclick="bbtCancel_Click" />
</div>

</asp:Content>