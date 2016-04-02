<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEndCustomerLevel1.aspx.cs" Inherits="Kaizos.frmEndCustomerLevel1" MasterPageFile="~/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="MainHeader">
         <div class="MainTitle">
            <table width="100%">
            <tr>
                <td class="AppLogo" valign="middle"></td>
            </tr>
            </table>
        </div>
        <h3 class="clsLabelLeft"> <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LocalString, EndCustomerTitle%>"/></h3>
    </div>
    <div class = "clsEndCustomerLevelCompany">
        <fieldset id="Fieldset1" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="lblCompany" runat="server" Text="<%$ Resources:LocalString, EndCustomerCompany%>"></asp:Label>
            </legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, EndCustomerCompanyName%>"></asp:Label>  
                    </td>
                    <td>               
                        <asp:TextBox ID="txtCompanyName" runat="server" Enabled="False" MaxLength="60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblEmailConfirmation" runat="server" Text="<%$ Resources:LocalString, EndCustomerEmailConfirmation%>"></asp:Label>  
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailConfirmation" runat="server" Enabled="False" 
                            MaxLength="60"></asp:TextBox>
                    </td>
                </tr>
                </table>
        </fieldset>
    </div>
    <br />

    <div class="clsEndCustomerLevel1InvoiceAddress">
        <fieldset id="Fieldset2" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="lblInvoiceAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerInvoideAddress%>"></asp:Label>
            </legend>
            <table>
                <tr>
                    <td>        
                        <asp:Label ID="lblContactName" runat="server" Text="<%$ Resources:LocalString, EndCustomerContactName %>"></asp:Label>
                    </td>
                    <td>    
                        <asp:TextBox ID="txtContactName" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>    
                    <td>    <asp:Label ID="lblInvoicePhoneNumber" runat="server" Text="<%$ Resources:LocalString, EndCustomerPhoneNumber%>"></asp:Label> </td>
                    <td>    <asp:TextBox ID="txtInvoicePhoneNumber" runat="server" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>    
                    <td>    <asp:Label ID="lblFaxNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerFaxNo%>"></asp:Label> </td>
                    <td>    <asp:TextBox ID="txtInvoiceFaxNo" runat="server" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblVatNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerVatNo%>"></asp:Label></td>
                    <td>    <asp:TextBox ID="txtVatNo" runat="server" MaxLength="30"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblSiretNo" runat="server" Text="<%$ Resources:LocalString, EndCustomerSiretNo%>"></asp:Label></td>
                    <td>    <asp:TextBox ID="txtSiretNo" runat="server" MaxLength="30"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>   <asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress1%>"></asp:Label></td>
                    <td>    <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress2%>"></asp:Label></td>
                    <td>    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, EndCustomerAddress3%>"></asp:Label> </td>
                    <td>   <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblZipcode" runat="server" Text="<%$ Resources:LocalString, EndCustomerZipcode%>"></asp:Label> </td>
                    <td>    <asp:TextBox ID="txtZipcode" runat="server" MaxLength="12"></asp:TextBox></td>
                </tr> 
                <tr>               
                    <td>    <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, EndCustomerCity%>"></asp:Label> </td>
                    <td>    <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>    <asp:Label ID="lblInvoiceCountry" runat="server" Text="<%$ Resources:LocalString, EndCustomerCountry%>"></asp:Label> </td>
                    <td>   <asp:DropDownList ID="ddlInvoiceCountry" runat="server" AutoPostBack="True"> </asp:DropDownList></td>
                </tr>
            </table>
        </fieldset>
    </div>

    <div class="clsEndCustomerShipping">
        <fieldset id="Fieldset3" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="lblShipping" runat="server" Text="<%$ Resources:LocalString, EndCustomerShipping%>"></asp:Label>
            </legend>
            <table>
                <tr>
                    <td> <asp:Label ID="lblShippingAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerShippingAddress%>"></asp:Label> </td>
                    <td> <asp:CheckBox ID="chkUseInvoiceAddress" runat="server" text = "<%$ Resources:LocalString, EndCustomerUseInvoiceAddress%>"/> </td>
                </tr>
                <tr>
                    <td> <asp:Label ID="lblReturnAddress" runat="server" Text="<%$ Resources:LocalString, EndCustomerReturnAddress%>"></asp:Label>  </td>
                    <td> <asp:CheckBox ID="chkUserReturnAddress" runat="server" text = "<%$ Resources:LocalString, EndCustomerReturnAddress%>"/> </td>
                </tr>
                <tr>
                    <td> <asp:Label ID="lblShippingPreference" runat="server" Text="<%$ Resources:LocalString, EndCustomerShippingPreference%>"></asp:Label> </td>
                    <td> <asp:DropDownList ID="ddlShippingPreference" runat="server" AutoPostBack="True"> </asp:DropDownList> </td>
                </tr>
            </table>
        </fieldset>
    </div> 

    <br /><br />
    <div class="clsEndCustomerPayment">
        <fieldset id="Fieldset4" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="lblPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustomerPaymentMethod%>"></asp:Label>
            </legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCurrentPaymentMethod" runat="server" Text="<%$ Resources:LocalString, EndCustomerCurrentPaymentMethod%>"></asp:Label> 
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, EndCustomerCreditCard%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeferredPayment" runat="server" Text="<%$ Resources:LocalString, EndCustomerRequestDeferredPayment%>"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkDeferredPayment" runat="server" 
                            text = "<%$ Resources:LocalString, EndCustomerDeferredPayment%>" 
                            oncheckedchanged="chkDeferredPayment_CheckedChanged" AutoPostBack="True"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTransportBudget" runat="server" Text="<%$ Resources:LocalString, EndCustomerTransportBudget%>"></asp:Label> 
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransportBudget" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

    <div class="clsEndCustomerTos">
        <asp:CheckBox ID="chkTOS" runat="server" text = ""/>
        <asp:LinkButton ID="HyperLink1" runat="server" NavigateUrl="frmTOSShow.aspx" >
        <asp:Label ID="lblTos" runat="server" Text="<%$ Resources:LocalString, EndCustomerTOS%>"></asp:Label>
        </asp:LinkButton>
    </div>

                        <asp:modalpopupextender 
		                    id="lnkDelete_ModalPopupExtender" runat="server" 
		                    okcontrolid="ButtonDeleleOkay" 
		                    targetcontrolid="HyperLink1" popupcontrolid="DivDeleteConfirmation" 
		                    backgroundcssclass="modalBackground"></asp:modalpopupextender>
                         <asp:confirmbuttonextender id="lnkDelete_ConfirmButtonExtender" 
		                        runat="server" targetcontrolid="HyperLink1" enabled="True" 
		                        displaymodalpopupid="lnkDelete_ModalPopupExtender"></asp:confirmbuttonextender>



    <div class="clsEndCustomerButton">
        <asp:Button ID="btnSubmit" runat="server" 
            Text="<%$ Resources:LocalString, AllSubmit%>" 
            ValidationGroup = "grpEndCustomer" onclick="btnSubmit_Click"/>
        <asp:Button ID="btnCancel" runat="server"  CssClass = "Cancel"
            Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click"/>
    </div>

    <div class="clsEndCustomerMandatory">
        <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>"></asp:Label>
    </div>

 <asp:panel class="customerModelWindow" id="DivDeleteConfirmation" style="display: none" runat="server">
    <div class="TitlebarLeft">
        <asp:Label ID="Label1" Text="Information" runat="server"  CssClass="clsLabelHeader"/>
     </div>
        <center>
        <div class="customerDivModalWindow">
        <table >
            <tr>
                <td>
                      <asp:Label ID="lblMessage" runat="server" />
                </td>
            </tr>
        </table>
        </div>
        <table>
             <tr>
                <td align="right">
                    <input id="ButtonDeleleOkay" type="button" value="ok" class="clsMessgeButton"/> 
                </td>
            </tr>
        </table>
        </center>
</asp:panel>

<div class="divSummaryEndCustomerLevel">
<table>
<tr><td>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valShouldSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>

<asp:CustomValidator ID="val_EndCustomer" runat="server" 
                ControlToValidate="txtContactName" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpEndCustomer" CssClass="clsErrorMessage" 
        onservervalidate="val_EndCustomer_ServerValidate"></asp:CustomValidator>
</td>
</tr>
</table>
</div>

</asp:Content>
