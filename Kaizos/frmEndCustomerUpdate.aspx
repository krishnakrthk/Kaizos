<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEndCustomerUpdate.aspx.cs" Inherits="Kaizos.frmEndCustomerUpdate" MasterPageFile="~/NewSite.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="OptionCtrl" Namespace="VikServerControl" TagPrefix="cc1" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="divSummaryEndCustomerUpdate">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLess100" Text="<%$ Resources:LocalString, valLess100 %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
    
    <asp:CustomValidator ID="val_EndCustomerUpdate" runat="server" 
        ControlToValidate="txtEmail" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpEndCustomerUpdate" 
        CssClass="clsErrorMessage" 
        onservervalidate="val_EndCustomerUpdate_ServerValidate">
    </asp:CustomValidator>
</div>

<fieldset id="Fieldset1" runat="server" class ="first">
    <legend>
        <asp:Label ID="lblUser" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateUser%>"></asp:Label>
    </legend>
    <label for="txtEmail">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateEmail%>"></asp:Label>  
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="optStatusEnable">
        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateStatus%>"></asp:Label>  
    </label>
    <div class="fieldGroup">
        <asp:RadioButton ID="optStatusEnable" runat="server" 
            Text="<%$ Resources:LocalString, EndCustUpdateStatusEnable%>" 
            GroupName="grpAccStatus" />
        <asp:RadioButton ID="optStatusDisable" runat="server" 
            Text="<%$ Resources:LocalString, EndCustUpdateStatusDisable%>" 
            GroupName="grpAccStatus" />
        <asp:RadioButton ID="optStatusArchieve" runat="server" 
            Text="<%$ Resources:LocalString, EndCustUpdateStatusArchieve%>" 
            GroupName="grpAccStatus" />
    </div>
    <div class="fieldBloc">
        <asp:Button ID="btnGet" runat="server" 
             Text="<%$ Resources:LocalString, AllGet%>" onclick="btnGet_Click" class="altButton"/>
    </div>

    <label for="txtPassword">
        <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:LocalString, EndCustUpdatePassword%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtPassword" runat="server" MaxLength="60" 
         TextMode="Password" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>
    
    <label for="txtConfirmPassword">
        <asp:Label ID="lblPasswordConfirmation" runat="server" Text="<%$ Resources:LocalString, EndCustUpdatePasswordConfirm%>"></asp:Label> 
    </label>
    <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="60" 
         TextMode="Password" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>
    
    <label for="ddlCommercialAttach">
        <asp:Label ID="txtCommercialAttach" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvCommercialAttach%>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCommercialAttach" runat="server"></asp:DropDownList>                        
    
    <label for="txtHQZipcode">
        <asp:Label ID="lblHQZipcode" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvHQZipcode%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtHQZipcode" runat="server" MaxLength="12"></asp:TextBox>
    
    <label for="txtTurnover">
        <asp:Label ID="lblTurnover" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvTurnOver%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtTurnover" runat="server" Enabled="False"></asp:TextBox>
    
    <label for="txtGrossMargin">
        <asp:Label ID="lblGrossMargin" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvGrossMargin%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtGrossMargin" runat="server" Enabled="False"></asp:TextBox>
    
    <label for="txtADV">
        <asp:Label ID="lblADV" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvADV%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtADV" runat="server" Enabled="False"></asp:TextBox>
    
    <label for="ddlCustomerCategory">
        <asp:Label ID="lblCustomerCategory" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvCustomerCategory%>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCustomerCategory" runat="server"></asp:DropDownList>
</fieldset>

<fieldset id="Fieldset2" runat="server" class ="first">
    <legend>
        <asp:Label ID="lblInvoiceAddress" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvoiceAddress%>">Invoicing Address</asp:Label>
    </legend>
    <label for="txtContact">
        <asp:Label ID="lblContact" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvContact%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtContact" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtCompanyName">
        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvCompanyName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
    
    <label for="txtPhoneNumber">
        <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvPhoneNumber%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
    
    <label for="txtFaxNo">
        <asp:Label ID="lblFaxNo" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvFaxNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20"></asp:TextBox>
    
    <label for="txtAddress1">
        <asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvAddress1%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtAddress2">
        <asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvAddress2%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtAddress3">
        <asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvAddress3%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtZipcode">
        <asp:Label ID="lblZipcode" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvZipcode%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtZipcode" runat="server" MaxLength="12"></asp:TextBox>
    
    <label for="txtCity">
        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvCity%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="ddlCountry">
        <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvCountry%>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
    
    <label for="txtVatNo">
        <asp:Label ID="lblVatNo" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvVatNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtVatNo" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtSiretNo">
        <asp:Label ID="lblSiretNo" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvSiretNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtSiretNo" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="ddlIndustry">
        <asp:Label ID="lblIndustry" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvIndustry%>"></asp:Label>
    </label>
    <cc1:OptionDropDownList id="ddlIndustry" runat="server" CssClass="clsgrey"></cc1:OptionDropDownList>
    
    <label for="txtLegalForm">
        <asp:Label ID="lblLegalForm" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvLegalForm%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtLegalForm" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtManPower">
        <asp:Label ID="lblManPower" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInvManPower%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtManPower" runat="server" MaxLength="30"></asp:TextBox>
</fieldset>

<asp:UpdatePanel id="UpdatePanel2" runat="server">
    <ContentTemplate>
        <fieldset id="Fieldset3" runat="server" class ="first">
            <legend>
                <asp:Label ID="lblShipping" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateShipping%>"></asp:Label>
            </legend>
            <label for="chkInvoiceAddress">
                <asp:Label ID="lblShippingAddress" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateShippingAddress%>"></asp:Label> 
            </label>
            <asp:CheckBox ID="chkInvoiceAddress" runat="server" text = "<%$ Resources:LocalString, EndCustUpdateShippingInvoiceAddress%>"  CssClass="checkBloc"/>
            
            <label for="chkEnableShippingPreferance">
                <asp:Label ID="lblShippingPreference" runat="server" Text="<%$ Resources:LocalString,SelectShippingReferenceEnable%>"></asp:Label>
            </label>
            <asp:CheckBox ID="chkEnableShippingPreferance" runat="server" text = "<%$ Resources:LocalString, SelectShippingReference%>" AutoPostBack="True" 
                            oncheckedchanged="chkEnableShippingPreferance_CheckedChanged" CssClass="checkBloc"/> </td>
            <div class="inlineBlock">
                <tr id="trSelectShippingReference" runat="server">
                    <td> 
					    <label for="rlShippingPreference">
                            <asp:Label ID="lblSelectShippingReference" runat="server" Text="<%$ Resources:LocalString, SelectShippingReference%>"></asp:Label>
				        </label>
                    </td>
                    <td> 
                        <asp:ReorderList ID="rlShippingPreference" runat="server" 
                            clientIdmode="AutoID"   
					        DragHandleAlignment="Left" 
					        ItemInsertLocation="Beginning"
					        DataKeyField="Id"
					        AllowReorder="true"> 
					        <ItemTemplate> 
						        <asp:Label ID="lblShippingPreferenceType" runat="server" Text='<%# Eval("ShippingPreferenceType") %>'></asp:Label>            
					        </ItemTemplate> 
				        </asp:ReorderList>                 
                    </td>
                </tr>
            </div>
            <label for="ddlShipNamedCarrier">
                <asp:Label ID="lblShipNamedCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateNamedCarrier%>"></asp:Label> 
            </label>
            <asp:DropDownList ID="ddlShipNamedCarrier" runat="server"></asp:DropDownList> 
            
            <label for="ddlShipInsuredMethod">
                <asp:Label ID="lblShipInsuranceMethod" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInsuranceMethod%>"></asp:Label> 
            </label>
            <asp:DropDownList ID="ddlShipInsuredMethod" runat="server"></asp:DropDownList> 
        </fieldset>
    </ContentTemplate> 
</asp:UpdatePanel> 

<asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <fieldset id="Fieldset4" runat="server" class ="first">
            <legend>
                <asp:Label ID="lblPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustUpdatePaymentMethod%>"></asp:Label>
            </legend>
            
            <label for="optCreditCard">
                <asp:Label ID="lblCurrentPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCurrentPaymentMethod%>"></asp:Label> 
            </label>
            <div class="fieldGroup">
                <asp:RadioButton ID="optCreditCard" runat="server" 
                    text = "<%$ Resources:LocalString, EndCustUpdateCurrentCC%>" 
                    GroupName="grpOptEndCustomerUpdate" 
                    oncheckedchanged="optCreditCard_CheckedChanged" AutoPostBack="True"/><br />
                <asp:RadioButton ID="optDeferdPayment" runat="server" 
                    text = "<%$ Resources:LocalString, EndCustUpdateCurrentDP%>" 
                    GroupName="grpOptEndCustomerUpdate" AutoPostBack="True" 
                    oncheckedchanged="optDeferdPayment_CheckedChanged"/>
            </div>
            <label for="ddlDPType"><asp:Label ID="lblDPType" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCurrentDPType%>"></asp:Label></label>
            <asp:DropDownList ID="ddlDPType" runat="server"></asp:DropDownList>
            
            <label for="chkRequestDP"><asp:Label ID="lblRequestDP" runat="server" Visible="false"></asp:Label></label>
            <asp:CheckBox ID="chkReqestDP" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCurrentRequestDP%>"
                            oncheckedchanged="chkReqestDP_CheckedChanged" AutoPostBack="True" TextAlign="Left"/>
            
            <label for="txtManPtxtTranportBudgetower"><asp:Label ID="lblTransportBudget" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateTransportBudget%>"></asp:Label></label>
            <asp:TextBox ID="txtTranportBudget" runat="server"></asp:TextBox>
            
            <label for="txtDepositAmount"><asp:Label ID="lblDepositAmount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateDepostAmount%>"></asp:Label></label>
            <asp:TextBox ID="txtDepositAmount" runat="server"></asp:TextBox>
            
            <label for="txtPaymentDelay">
                <asp:Label ID="lblPaymentDelay" runat="server" Text="<%$ Resources:LocalString, EndCustUpdatePaymentDelay%>"></asp:Label> 
            </label>
            <asp:TextBox ID="txtPaymentDelay" runat="server"></asp:TextBox>
        </fieldset>
    </ContentTemplate> 
</asp:UpdatePanel> 

<asp:UpdatePanel id="Upatepanel3" runat="server">
    <ContentTemplate>
        <fieldset id="Fieldset5" runat="server" class ="first">
            <legend>
                <asp:Label ID="lblKeyAccount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateKeyAccount%>"></asp:Label>
            </legend>
            <asp:CheckBox ID="chkKeyAccount" runat="server"  AutoPostBack="true" 
                        Text="<%$ Resources:LocalString, EndCustUpdateISKeyAccount%>" 
                        oncheckedchanged="chkKeyAccount_CheckedChanged" TextAlign="Left"/>
                <tr id="trCarrier" runat="server" visible="false">
                    <td>
                        <label for="ddKeyCarrier">
                            <asp:Label ID="lblForCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateForCarrier%>"></asp:Label>  
                        </label>
                    </td>
                               
                     <td>  <asp:DropDownList ID="ddKeyCarrier" Visible ="false" runat="server"></asp:DropDownList> 
                           <asp:ListBox ID="lbCarrier" Rows="5" runat="server" Visible="false" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>              
            <label for="txtSubcription">
                <asp:Label ID="lblSubscription" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateSubscription%>"></asp:Label>  
            </label>
            <asp:TextBox ID="txtSubcription" runat="server"></asp:TextBox>
        </fieldset>
    </ContentTemplate> 
</asp:UpdatePanel>

<fieldset id="Fieldset8" runat="server" class ="first">
    <legend>
        <asp:Label ID="lblExtraInfor" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateExtraInfo%>"></asp:Label>
    </legend>
    <label for="txtExtraInfo">
        <asp:Label ID="lblExtra" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateExtraInfo%>"></asp:Label>  
    </label>
    <asp:TextBox ID="txtExtraInfo" runat="server"></asp:TextBox>
    <asp:CheckBox ID="chkActiveAccount" runat="server"  Text="<%$ Resources:LocalString, EndCustUpdateActiveAccount%>" TextAlign="Left"/>
</fieldset>    
    
<table>
    <thead>
        <tr>
            <th colspan="2">
                <asp:Label ID="lblParcel" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateParcels%>"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblParcelFr" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateFrance%>"></asp:Label>  
            </th>
            <th>
                <asp:Label ID="lblParcelEup" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateEurope%>"></asp:Label>  
            </th>
            <th>
                <asp:Label ID="lblParcelInt" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInternational%>"></asp:Label>  
            </th>
        </tr>
    </thead>
    <tr>
        <td rowspan="5">
            <asp:Label ID="lblParcelExpress" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateExpress%>"></asp:Label>  
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblParcelADV1" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateADV%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelADVFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelADVEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelADVInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelWPP" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateWpp%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelWPPFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelWPPEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelWPPInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCarrier%>"></asp:Label>  
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelCarrierFr" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelCarrierFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelCarrierEup" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelCarrierEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelCarrierInt" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelCarrierInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelDiscount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateDiscount%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelDiscountFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelDiscountEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelDiscountInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td rowspan="5">
            <asp:Label ID="lblParcelEconomy" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateEconomy%>"></asp:Label>  
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblParcelEconomyAdv" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateADV%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyAdvFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyAdvEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyAdvInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelEconomyWpp" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateWpp%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyWppFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyWppEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyWppInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelEconomyCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCarrier%>"></asp:Label>  
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelEconomyCarrierFr" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelEconomyCarrierFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelEconomyCarrierEup" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelEconomyCarrierEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlParcelEconomyCarrierInt" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtParcelEconomyCarrierInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblParcelEconomyDiscount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateDiscount%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyDiscountFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyDiscountEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtParcelEconomyDiscountInt" runat="server"></asp:TextBox>
        </td>

    </tr>

</table>
   
<table>
    <thead>
        <tr>
            <th colspan="2">
                <asp:Label ID="lblPallet" runat="server" Text="<%$ Resources:LocalString, EndCustUpdatePallets%>"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblPalletFr" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateFrance%>"></asp:Label>  
            </th>
            <th>
                <asp:Label ID="lblPalletEup" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateEurope%>"></asp:Label>  
            </th>
            <th>
                <asp:Label ID="lblPalletIn" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateInternational%>"></asp:Label>  
            </th>
        </tr>
    </thead>
    <tr>
        <td rowspan="5">
            <asp:Label ID="lblPalletExpress" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateExpress%>"></asp:Label>  
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPalletExpressADV" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateADV%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressADVFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressADVEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressADVInt" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPalletExpressWpp" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateWpp%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressWppFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressWppEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressWppInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletExpressCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCarrier%>"></asp:Label>  
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletExpressCarrierFr" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletExpressCarrierFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletExpressCarrierEup" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletExpressCarrierEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletExpressCarrierInt" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletExpressCarrierInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletExpressDiscount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateDiscount%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressDiscountFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressDiscountEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletExpressDiscountInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td rowspan="5">
            <asp:Label ID="lblPalletEconomy" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateEconomy%>"></asp:Label>  
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletEconomyAdv" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateADV%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyAdvFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyAdvEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyAdvInt" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletEconomyWpp" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateWpp%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyWppFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyWppEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyWppInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletEconomyCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateCarrier%>"></asp:Label>  
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletEconomyCarrierFr" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletEconomyCarrierFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletEconomyCarrierEup" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletEconomyCarrierEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList ID="ddlPalletEconomyCarrierInt" runat="server"></asp:DropDownList><br />
            <asp:TextBox ID="txtPalletEconomyCarrierInt" runat="server"></asp:TextBox>
        </td>

    </tr>

    <tr>
        <td>
            <asp:Label ID="lblPalletEconomyDiscount" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateDiscount%>"></asp:Label>  
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyDiscountFr" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyDiscountEup" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtPalletEconomyDiscountInt" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>

<div id="buttons">
    <asp:CheckBox ID="chkTOS" runat="server" text = ""/>
    <asp:LinkButton ID="HyperLink1" runat="server" NavigateUrl="frmTOSShow.aspx" >
    <asp:Label ID="lblTos" runat="server" Text="<%$ Resources:LocalString, EndCustomerTOS%>"></asp:Label>
    </asp:LinkButton>
    <asp:Button ID="btnSubmit" runat="server" 
        Text="<%$ Resources:LocalString, EndCustSubmit%>" 
        ValidationGroup="grpEndCustomerUpdate" onclick="btnSubmit_Click"/>
    <asp:Button ID="btnCance" runat="server" 
        Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCance_Click" CssClass="Cancel"/>
    <asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>"></asp:Label>
</div>

<asp:modalpopupextender id="lnkDelete_ModalPopupExtender" runat="server" okcontrolid="ButtonDeleleOkay" 
	targetcontrolid="HyperLink1" popupcontrolid="DivDeleteConfirmation" backgroundcssclass="modalBackground">
</asp:modalpopupextender>

<asp:confirmbuttonextender id="lnkDelete_ConfirmButtonExtender" runat="server" targetcontrolid="HyperLink1" enabled="True" 
	displaymodalpopupid="lnkDelete_ModalPopupExtender">
</asp:confirmbuttonextender>

<asp:panel class="customerModelWindow" id="DivDeleteConfirmation" style="display: none" runat="server">
    <asp:Label ID="Label1" Text="Information" runat="server"  CssClass="clsLabelHeader"/>
    <asp:Label ID="lblMessage" runat="server" />
     
    <input id="ButtonDeleleOkay" type="button" value="ok" class="clsMessgeButton"/> 
</asp:panel>

</asp:Content>