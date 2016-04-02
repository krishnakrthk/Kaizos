<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerService.aspx.cs" Inherits="Kaizos.frmCustomerService" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<fieldset id="Fieldset1" runat="server" class ="first">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, CustomerServiceCustomerService%>"></asp:Label>
    </legend>
    <div class="fieldBloc">
        <label for="txtCustomerName">
            <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, CustomerServiceCustomerName%>"></asp:Label>
        </label>
        <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="100"></asp:TextBox>
    </div>
    
    <div class="fieldBloc">
        <label for="txtEmail">
            <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, CustomerServiceEmail%>"></asp:Label>
        </label>
        <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    </div>
    
    <div class="fieldBloc">
        <label for="ddlLanguage">
            <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:LocalString, CustomerServiceLanguage%>"></asp:Label>
        </label>
        <asp:DropDownList ID="ddlLanguage" runat="server"></asp:DropDownList>
    </div>
      
</fieldset>  

<div id="buttons">
    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
    <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>" onclick="btnSubmit_Click"  ValidationGroup = "grpCustomerService" />
    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click" />
</div>

<div class="divSummaryCustomerService">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_CustomerService" runat="server" 
        ControlToValidate="txtCustomerName" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpCustomerService" CssClass="clsErrorMessage" onservervalidate="val_CustomerService_ServerValidate" >
    </asp:CustomValidator>
</div>
</asp:Content>