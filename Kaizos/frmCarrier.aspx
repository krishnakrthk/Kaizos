<%@ Page Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmCarrier.aspx.cs" Inherits="Kaizos.frmCarrier" culture="auto"  uiculture="auto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="divSummaryCarrier">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="ValLessThan" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>
    <asp:HiddenField ID="hdnMode" runat="server" />
</div>
              
<asp:CustomValidator ID="val_Carrier" runat="server" 
            ControlToValidate="txtCarrierName" 
            EnableClientScript="False" 
            ValidateEmptyText="True"
            CssClass="clsErrorMessage" onservervalidate="val_Carrier_ServerValidate">
</asp:CustomValidator>

<fieldset id="Fieldset2" runat="server" class="third">
    <legend>
        <asp:Label ID="lblModuleHeading" runat="server" 
            Text="<%$ Resources:LocalString, CarrierCreation %>" >
            </asp:Label>
    </legend>               
    <label for="txtCarrierName">
        <asp:Label ID="lblCarrierName" runat="server" Text="<%$ Resources:LocalString, CarrierNameWithSpace %>">*</asp:Label> 
    </label>
    <asp:TextBox ID="txtCarrierName" runat="server" MaxLength="60" ></asp:TextBox>
    <br />
    <asp:CheckBox ID="chkReferencedCarrier" 
         Text="<%$ Resources:LocalString, ReferencedCarrier %>" runat="server" CssClass="checkBloc" TextAlign="Left"/> 
    <br />
    <label for="txtClaimResolutionDelays">
        <asp:Label ID="lblClaimResoulutionDelay" runat="server" 
             Text="<%$ Resources:LocalString, ClaimResoulutionDelay %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtClaimResolutionDelays" runat="server"></asp:TextBox>
   
    <tr id="trActive" runat="server" >
        <td></td>
        <td>
            <asp:CheckBox ID="chkActive" Text="<%$ Resources:LocalString, Active %>" runat="server" CssClass="checkBloc" TextAlign="Left"/> 
        </td>        
    </tr>
   
</fieldset>
<div id="buttons">
    <asp:Button ID="btnCreate" runat="server" 
            Text="<%$ Resources:LocalString, Create %>"
            onclick="btnCreate_Click"  /> 
</div>

<asp:ScriptManager ID="ScriptManager1" runat="server" />
        
</asp:Content>
