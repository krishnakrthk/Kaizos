<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEndCustomer.aspx.cs" Inherits="Kaizos.frmEndCustomer"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="fr">
	<head id="Head1" runat="server">
		<title>Kaizos</title>
		
		<!-- Meta-data -->
		<meta charset='utf-8' />
		<meta name='description' content="Solution d'expédition" />
		<meta name='keywords' content="" />
		<meta name='author' content='IPS Europe' />
		<meta name='copyright' content='&copy; 2011 IPS Europe' />
		<meta name='language' content='fr' />
    		
		<!-- Design -->
		<link rel='shortcut icon' href='favicon.ico' />
		<link href='css/std.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' />
		<!--[if IE]><link href='css/ie.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
		<!--[if lte IE 7]><link href='css/ie7-.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
		<!--[if lte IE 8]><link href='css/ie8-.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
	</head>

<body class="home">
    <div id="container">
        <div id="head">
			<a id="logo" href="frmLogin.aspx"><img src="img/main/logo_kaizos.png" alt="Logo Kaizos" /></a>
            <a id="login" href="frmLogin.aspx">Login</a>
            <ul id="menu">
				<li><a href="index.htm">Accueil</a></li>
				<li><a href="services.htm">Services</a></li>
				<li><a href="help.htm">Conseils</a>
					<ul>
						<li><a href="help.htm#open_account">Créer votre compte</a></li>
						<li><a href="help.htm#invoicing_payment">Facturation et paiement</a></li>
						<li><a href="help.htm#insurance">Assurance</a></li>
						<li><a href="help.htm#tips">Conseils utiles</a></li>
					</ul>
				</li>
				<li><a href="contact.htm">Contact</a></li>
			</ul>
			<div id="head_actions">
				<div>
					<a href='demo.htm'>Démo<br />Application</a>
					<a href='register.htm'>Créer compte<br />& expédier</a>
				</div>
			</div>
        </div>

        <h3 class="clsLabelLeft" > <asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LocalString, EndCustomerTitle%>"/></h3>
           
   <div id="content">
       <form id="Form1" runat="server" class="form3">
           <asp:ScriptManager ID="ScriptManager1" runat="server" />
           <div id="column1">

        <fieldset runat="server" class ="first">
            <legend>
                <asp:Label ID="lblCompany" runat="server" Text="<%$ Resources:LocalString, EndCustomerCompany%>"></asp:Label>
            </legend>
            
            <label for="txtCompanyName"><asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, EndCustomerCompanyName%>"></asp:Label> </label> 
            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="60"></asp:TextBox>
            
            <label for="txtName"><asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, EndCustomerName%>"></asp:Label> </label>
            <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
            
            <label for="ddlFunction"><asp:Label ID="lblFunction" runat="server" Text="<%$ Resources:LocalString, EndCustomerFunction%>"></asp:Label> </label>
            <asp:DropDownList ID="ddlFunction" runat="server"> </asp:DropDownList>
            
            <label for="txtEmail"><asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, EndCustomerEmailLogin%>"></asp:Label> </label>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
            
            <label for="txtEmailConfirmation"><asp:Label ID="lblEmailConfirmation" runat="server" Text="<%$ Resources:LocalString, EndCustomerEmailConfirmation%>"></asp:Label> </label> 
            <asp:TextBox ID="txtEmailConfirmation" runat="server" MaxLength="60"></asp:TextBox>
            
            <label for="txtPassword"><asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:LocalString, EndCustomerPassword%>"></asp:Label> </label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox>    
            
            <label for="txtConfirmPassword"><asp:Label ID="lblConfirmPassword" runat="server" Text="<%$ Resources:LocalString, EndCustomerConfirmPassword%>"></asp:Label> </label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                            MaxLength="60" ToolTip="<%$ Resources:LocalString, AllPasswordRule%>"></asp:TextBox> 
            
            <label for="txtPhoneNumber"><asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, EndCustomerPhoneNumber%>"></asp:Label> </label>
            <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
            
            <label for="ddlCountry"><asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, EndCustomerCountry%>"></asp:Label></label>
            <asp:DropDownList ID="ddlCountry" runat="server"> </asp:DropDownList>
            
            <label for="txtHQZipcode"><asp:Label ID="lbHQZipcode" runat="server" Text="<%$ Resources:LocalString, EndCustomerHQZipcode%>"></asp:Label></label>
            <asp:TextBox ID="txtHQZipcode" runat="server" MaxLength="12"></asp:TextBox>
            
        </fieldset>
        
        <fieldset id="Fieldset2" runat="server" class="first">
            <legend>
                <asp:Label ID="lblInvoiceAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerInvoideAddress%>"></asp:Label>
            </legend>
            
            <label for="txtContactName"><asp:Label ID="lblContactName" runat="server" Text="<%$ Resources:LocalString, EndCustomerContactName %>"></asp:Label></label>
            <asp:TextBox ID="txtContactName" runat="server" MaxLength="100"></asp:TextBox>
           
            <label for="txtInvoicePhoneNumber"><asp:Label ID="lblInvoicePhoneNumber" runat="server" Text="<%$ Resources:LocalString, EndCustomerPhoneNumber%>"></asp:Label></label>
            <asp:TextBox ID="txtInvoicePhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
            
            <label for="txtInvoiceFaxNo"><asp:Label ID="lblFaxNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerFaxNo%>"></asp:Label></label>
            <asp:TextBox ID="txtInvoiceFaxNo" runat="server" MaxLength="20"></asp:TextBox>
            
            <label for="txtVatNo"><asp:Label ID="lblVatNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerVatNo%>"></asp:Label></label>
            <asp:TextBox ID="txtVatNo" runat="server" MaxLength="30"></asp:TextBox>
            
            <label for="txtSiretNo"><asp:Label ID="lblSiretNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerSiretNo%>"></asp:Label></label>
            <asp:TextBox ID="txtSiretNo" runat="server" MaxLength="30"></asp:TextBox>
            
            <label for="txtAddress1"><asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress1%>"></asp:Label></label>
            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox>
            
            <label for="txtAddress2"><asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress2%>"></asp:Label></label>
            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox>
            
            <label for="txtAddress3"><asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress3%>"></asp:Label></label>
            <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox>
            
            <label for="txtZipcode"><asp:Label ID="lblZipcode" runat="server" Text="<%$ Resources:LocalString, EndCustomerZipcode%>"></asp:Label></label>
            <asp:TextBox ID="txtZipcode" runat="server" MaxLength="12"></asp:TextBox>
            
            <label for="txtCity"><asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, EndCustomerCity%>"></asp:Label></label>
            <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
            
            <label for="ddlInvoiceCountry"><asp:Label ID="lblInvoiceCountry" runat="server" Text="<%$ Resources:LocalString, EndCustomerCountry%>"></asp:Label></label>
            <asp:DropDownList ID="ddlInvoiceCountry" runat="server"> </asp:DropDownList>
            
        </fieldset>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
      <fieldset id="Fieldset3" runat="server" class="first">
            <legend>
                <asp:Label ID="lblShipping" runat="server" Text="<%$ Resources:LocalString, EndCustomerShipping%>"></asp:Label>
            </legend>
            <label for="chkUseInvoiceAddress"><asp:Label ID="lblShippingAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerShippingAddress%>"></asp:Label></label>
            <asp:CheckBox ID="chkUseInvoiceAddress" runat="server" text = "<%$ Resources:LocalString, EndCustomerUseInvoiceAddress%>"/>
            
            <label for="chkUserReturnAddress"><asp:Label ID="lblReturnAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerReturnAddress%>"></asp:Label></label>
            <asp:CheckBox ID="chkUserReturnAddress" runat="server" text = "<%$ Resources:LocalString, EndCustomerUseInvoiceAddress%>"/>
            
            <label for="chkEnableShippingPreferance"><asp:Label ID="lblShippingPreference" runat="server" Text="<%$ Resources:LocalString,SelectShippingReferenceEnable%>"></asp:Label></label>
            <asp:CheckBox ID="chkEnableShippingPreferance" runat="server" 
                            text = "<%$ Resources:LocalString, SelectShippingReference%>" 
                            AutoPostBack="True" 
                            oncheckedchanged="chkEnableShippingPreferance_CheckedChanged"/>
            
            <tr id="trSelectShippingReference" runat="server">
                    <td>
                        <label for="rlShippingPreference"><asp:Label ID="lblSelectShippingReference" runat="server" Text="<%$ Resources:LocalString, SelectShippingReference%>"></asp:Label></label>
                    </td>

                    <td>                     
                       <asp:ReorderList ID="rlShippingPreference" runat="server" clientIdmode="AutoID"   
                                    DragHandleAlignment="Left" 
                                    ItemInsertLocation="Beginning"
                                    DataKeyField="Id"
                                    AllowReorder="true" PostBackOnReorder="false">                              
                            <ItemTemplate> 
                                    <asp:Label ID="lblShippingPreferenceType" runat="server" Text='<%# Eval("ShippingPreferenceType") %>'></asp:Label>                  
                            </ItemTemplate> 
                       </asp:ReorderList>                  
                    </td>
                </tr>
                
                <label for="ddlShipNamedCarrier"><asp:Label ID="lblShipNamedCarrier" runat="server" Text="<%$ Resources:LocalString, EndCustUpdateNamedCarrier%>"></asp:Label></label>
                <asp:DropDownList ID="ddlShipNamedCarrier" runat="server"></asp:DropDownList>                    
               
        </fieldset>
      </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="Fieldset4" runat="server" class="first">
            <legend>
                <asp:Label ID="lblPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustomerPaymentMethod%>"></asp:Label>
            </legend>
            
            <asp:UpdatePanel id="upatepanel1" runat="server">
                <ContentTemplate>
                
                <label for="lblCredit"><asp:Label ID="lblCurrentPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustomerCurrentPaymentMethod%>"></asp:Label></label>
                <asp:Label ID="lblCredit" runat="server" Text="<%$ Resources:LocalString, EndCustomerCreditCard%>"></asp:Label>
                
                <label for="chkDeferredPayment"><asp:Label ID="lblDeferredPayment" runat="server" Text="<%$ Resources:LocalString, EndCustomerRequestDeferredPayment%>"></asp:Label></label>
                <asp:CheckBox ID="chkDeferredPayment" runat="server" 
                            AutoPostBack="True" oncheckedchanged="chkDeferredPayment_CheckedChanged"/>
                
                <label for="txtTransportBudget"><asp:Label ID="lblTransportBudget" runat="server" Text="<%$ Resources:LocalString, EndCustomerTransportBudget%>"></asp:Label></label>
                <asp:TextBox ID="txtTransportBudget" runat="server"></asp:TextBox>
                
                <label for="txtDepositAmount"><asp:Label ID="lblDepositAmount" runat="server" Text="<%$ Resources:LocalString, EndCustomerDepositAmount%>"></asp:Label></label> 
                <asp:TextBox ID="txtDepositAmount" runat="server"></asp:TextBox>
                
                </ContentTemplate>
            </asp:UpdatePanel>
    </fieldset>

<div id="buttons">
    
    <asp:CheckBox ID="chkTOS" runat="server" text = ""/>
    <asp:LinkButton ID="HyperLink1" runat="server" NavigateUrl="frmTOSShow.aspx" >
        <asp:Label ID="lblTos" runat="server" Text="<%$ Resources:LocalString, EndCustomerTOS%>"></asp:Label>
    </asp:LinkButton>
    <asp:Button ID="btnSubmit" runat="server" 
        Text="<%$ Resources:LocalString, AllSubmit%>" onclick="btnSubmit_Click" ValidationGroup = "grpEndCustomer"/>
    <asp:Button ID="btnCancel" runat="server"  CssClass = "Cancel"
        Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click"/>
    
    <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>"></asp:Label>
                
                
    <asp:modalpopupextender 
	    id="lnkDelete_ModalPopupExtender" runat="server" 
	    okcontrolid="ButtonDeleleOkay" 
	    targetcontrolid="HyperLink1" popupcontrolid="DivDeleteConfirmation" 
	    backgroundcssclass="modalBackground"></asp:modalpopupextender>


    <asp:confirmbuttonextender id="lnkDelete_ConfirmButtonExtender" 
		runat="server" targetcontrolid="HyperLink1" enabled="True" 
		displaymodalpopupid="lnkDelete_ModalPopupExtender"></asp:confirmbuttonextender>

    <asp:panel class="customerModelWindow" id="DivDeleteConfirmation" style="display: none" runat="server">
        <asp:Label ID="Label1" Text="Information" runat="server"  CssClass="clsLabelHeader"/>
        <asp:Label ID="lblMessage" runat="server" />
        
        <input id="ButtonDeleleOkay" type="button" value="ok" class="clsMessgeButton"/> 
        
    </asp:panel>

    <div class="divSummaryAddressBook">
        <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
        <asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>

        <asp:CustomValidator ID="val_EndCustomer" runat="server" 
                ControlToValidate="txtEmail" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpEndCustomer" CssClass="clsErrorMessage" 
                onservervalidate="val_EndCustomer_ServerValidate">
        </asp:CustomValidator>

    </div>

</div> 
</div>
<div id="column2">
        <div class="white_bloc">
			<h3>CONTACTEZ-NOUS :</h3>
			<ul>
				<li>Tél : 01 30 15 78 29</li>
				<li>E-mail : <a href="mailto:commercial@kaizos.com">commercial@kaizos.com</a></li>
			</ul>
		</div>
		<div id="shipping_solution">THE BEST SHIPPING <br />SOLUTION FOR EACH<br />DESTINATION</div> 
</div>



</form>
</div>
</div><!-- container -->
    <hr class="break" />
	<div id="foot">
		<div id="foot_content">
			<a id="website" href="http://www.kaizos.com">www.kaizos.com</a>
			<div id="legal">
				<ul>
					<li><a href="mentions_legales.htm"> Mentions légales</a></li>
				</ul>
				<p>Copyright &reg; 2011 Kaizos. Tous droits réservés</p>
			</div>
		</div>
	</div>
</body>
</html>