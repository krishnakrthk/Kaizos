<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerServiceUpdateAdmin.aspx.cs" Inherits="Kaizos.frmCustomerServiceUpdateAdmin"MasterPageFile="~/NewSite.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<fieldset id="Fieldset1" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, CSUser %>"></asp:Label>
    </legend>
    <div class="fieldBloc">
        <label for="txtEmail">
            <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, CSEmail %>"></asp:Label>
        </label>
        <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    </div><br />
    
    <div class="fieldBloc">
        <label for="txtCompanyName">
            <asp:Label ID="lblAccountStatus" runat="server" Text="<%$ Resources:LocalString, CSAccountStatus %>"></asp:Label>
        </label>
        <asp:RadioButton ID="optEnabled" runat="server" Text="<%$ Resources:LocalString, CSAccountStatusE%>" GroupName="grpAccountStatus" />
        <asp:RadioButton ID="optDisabled" runat="server" Text="<%$ Resources:LocalString, CSAccountStatusD%>" 
                        GroupName="grpAccountStatus" />
        <asp:RadioButton ID="optArchi" runat="server" Text="<%$ Resources:LocalString, CSAccountStatusA%>" 
                        GroupName="grpAccountStatus" AutoPostBack="True" />
        <asp:RadioButton ID="optBeingCreated" runat="server" Text="<%$ Resources:LocalString, CSAccountStatusB %>" 
                        GroupName="grpAccountStatus" />
    </div> 
</fieldset>

<div id="buttons">
    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
    <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>" 
            ValidationGroup="grpCustomerServiceUpdateByAdmin" onclick="btnSubmit_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>"
            onclick="btnCancel_Click" />
</div>

<div class="divSummaryCustomerServiceUpdateByAdmin">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_CustomerServiceUpdateByAdmin" runat="server" 
        ControlToValidate="txtEmail" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpCustomerServiceUpdateByAdmin" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_CustomerServiceUpdateByAdmin_ServerValidate" >
    </asp:CustomValidator>
</div>
</asp:Content>