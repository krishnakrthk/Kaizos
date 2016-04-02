<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="frmInsuranceBroker.aspx.cs" Inherits="Kaizos.frmInsuranceBroker"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<div class="clsCarrierMain">
 <fieldset id="Fieldset2" runat="server">
                <legend>
                    <asp:Label ID="lblModuleHeading" runat="server" 
                        Text="<%$ Resources:LocalString,  InsuranceBrokerDetails  %>" >
                       </asp:Label>
                </legend>               
<br />
    <br />
    <br />
    <table>
    <tr>
    
    <td class="clsLabelRight">
       <asp:Label ID="lblInsurancebrokeremailaddress" runat="server" Text="<%$ Resources:LocalString, Insurancebrokeremailaddress %>">*</asp:Label> 
    </td>
    <td>
     <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    </td>
    </tr>
   <tr>
    <td class="clsButton" colspan="2">
    <asp:Button ID="btnSubmit" runat="server" 
                            Text="<%$ Resources:LocalString, ManualSubmit %>" 
            Height="21px" onclick="btnSubmit_Click" ValidationGroup="grpBrokerEmail" /> 
    </td>
    </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    </fieldset>
</div>

<div class="divSummaryAddressBook">
<table>
<tr><td>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="ValLessThan" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>


<asp:CustomValidator ID="val_BrokerEmailId" runat="server" 
                ControlToValidate="txtEmail" 
                EnableClientScript="False" 
                ValidateEmptyText="True"
                ValidationGroup="grpBrokerEmail" CssClass="clsErrorMessage" onservervalidate="val_Broker_EmailID" 
             ></asp:CustomValidator>
</td>
</tr>
</table>
</div>
</asp:Content>
