<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="frmCustomer.aspx.cs" Inherits="Kaizos.frmCustomer"MasterPageFile="~/NewSite.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="OptionCtrl" Namespace="VikServerControl" TagPrefix="cc1" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
    </asp:ScriptManager>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
<ContentTemplate>
<div class="divSummaryFranchise">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, ValidationMaxAllowed %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_Customer" runat="server" 
                    ControlToValidate="txtCompanyName" 
                    EnableClientScript="False" 
                    ValidateEmptyText="True"
                    ValidationGroup="grpCustomer" 
                    CssClass="clsErrorMessage" onservervalidate="val_Franchise_ServerValidate">
    </asp:CustomValidator>
</div>

<fieldset id="Fieldset1" runat="server" class="first">
    <legend>
        <asp:Label ID="lblAccount" runat="server" Text="<%$ Resources:LocalString, CustomerAccount %>"></asp:Label>
    </legend>

    <label for="txtCompanyName">
            <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, CustomerCompanyName %>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtName">
            <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, ReferentUserName%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtEmail">
            <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, CustomerEmail%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtCustomerConfirmationEmail">
            <asp:Label ID="lblCustomerConfirmationEmail" runat="server" Text="<%$ Resources:LocalString, CustomerConfirmationEmail%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtCustomerConfirmationEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtPhoneNumber">
            <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, CustomerPhoneNumber%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
     
    <label for="txtHQZipcode">
            <asp:Label ID="lblHQZipcode" runat="server" Text="<%$ Resources:LocalString, CustomerHQZipcode%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtHQZipcode" runat="server" MaxLength="12"></asp:TextBox>
    
    <label for="ddlCountry">
            <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, CustomerCountry%>"></asp:Label> 
    </label>
    <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
    
    <label for="lbCarrier">
            <asp:Label ID="lblCarrier" runat="server" Text="<%$ Resources:LocalString, CarrierNameWithSpace%>"></asp:Label> 
    </label>
    <asp:ListBox ID="lbCarrier" runat="server" 
                              SelectionMode="Multiple"></asp:ListBox>
    <div class="group">
        <asp:CheckBox ID="chkKeyAccount" runat="server"  
            Text = "<%$ Resources:LocalString, CustomerKeyAccount%>" AutoPostBack="True" TextAlign="left"
            oncheckedchanged="chkKeyAccount_CheckedChanged"/>
    </div>
</fieldset>
</ContentTemplate>
</asp:UpdatePanel>

<fieldset id="Fieldset2" runat="server"  class="first">
    <legend>
        <asp:Label ID="lblInvoiceAddress" runat="server" Text="<%$ Resources:LocalString, CustomerKeyInvoicingAddress%>"></asp:Label>
    </legend>
    
    <label for="txtInvoiceName">
            <asp:Label ID="lblInvoiceName" runat="server" Text="<%$ Resources:LocalString, PresidentName%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtInvoiceName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="OptionDropDownList1">
            <asp:Label ID="lblIndustry" runat="server" Text="<%$ Resources:LocalString, CustomerIndustry%>"></asp:Label> 
    </label>
    <cc1:OptionDropDownList id="OptionDropDownList1" runat="server" ></cc1:OptionDropDownList>
    
    <label for="txtLegalForm">
            <asp:Label ID="lblLegalForm" runat="server" Text="<%$ Resources:LocalString, CustomerLegalForm%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtLegalForm" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtManPower">
            <asp:Label ID="lblManPower" runat="server" Text="<%$ Resources:LocalString, CustomerManPower%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtManPower" runat="server" MaxLength="6"></asp:TextBox>
    
    <label for="txtPaymentDelay">
            <asp:Label ID="lblPaymentDelay" runat="server" Text="<%$ Resources:LocalString, CustomerPaymentDelay%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtPaymentDelay" runat="server" ></asp:TextBox>
    
    <label for="ddlCustomerCategory">
            <asp:Label ID="lblCustomerCategory" runat="server" Text="<%$ Resources:LocalString, CustomerKeyCustomerCategory%>"></asp:Label> 
    </label>
    <asp:DropDownList ID="ddlCustomerCategory" runat="server"></asp:DropDownList>
    
    <label for="ddInsuranceMethod">
            <asp:Label ID="lblInsuranceMethod" runat="server" Text="<%$ Resources:LocalString, CustomeInsuranceMethod%>"></asp:Label> 
    </label>
    <asp:DropDownList ID="ddInsuranceMethod" runat="server"></asp:DropDownList>
    
    <label for="txtFirmDate">
            <asp:Label ID="lblFirmDate" runat="server" Text="<%$ Resources:LocalString, CustomerFirmCreationDate%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtFirmDate" runat="server"></asp:TextBox>
    <asp:CalendarExtender ID="calenderFirmDate" runat="server" TargetControlID="txtFirmDate" Format="dd/MM/yyyy" CssClass="clsCalendar"></asp:CalendarExtender>
    
    <div class="group">
        <asp:CheckBox ID="chkFictiveAcoount" runat="server"  
                Text = "<%$ Resources:LocalString, EndCustUpdateActiveAccount%>" TextAlign="left" Checked="true" />
    </div>
</fieldset>                                   

<div id="buttons">
    <asp:Button ID="btnSubmit" runat="server" 
        Text="<%$ Resources:LocalString, AllSubmit%>" onclick="btnSubmit_Click" ValidationGroup="grpCustomer" />
    <asp:Button ID="btnCancel" runat="server" 
        Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click" />
    <asp:Label ID="lblMandatoryField" runat="server" Font-Size="Smaller" Text="<%$ Resources:LocalString, AllMandatoryfield%>"></asp:Label>
</div>

   <asp:Button ID="btnTarget" runat="server" Text=",," Visible="false" /> 
   <div>
    <asp:ModalPopupExtender
        CancelControlID="btnCancelModalPopup" 
        runat="server" 
        PopupControlID="pnlErrorDisplay" 
        id="ModalPopupExtender1" 
        TargetControlID="btnTarget"
        BackgroundCssClass="ErrorModalBackground" PopupDragHandleControlID="pnlErrorDisplay" /> 

    <asp:Panel ID="pnlErrorDisplay" runat="server" Height="180px" Width="400px" style="display:none" CssClass="ErrorModalWindow"> 
    <center>
    
     <table width="100%">
        <tr>
        <td align="left">
        
           <asp:Label ID="lblDBError" runat="server"></asp:Label> 
          
        </td>
        </tr>        
    </table>
    <br />
      <asp:Button ID="btnCancelModalPopup" runat="server" Text="<%$ Resources:LocalString, Close%>"  /> 
    </center>
        
</asp:Panel> 
</div>
</asp:Content>