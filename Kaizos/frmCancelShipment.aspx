<%@ Page Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmCancelShipment.aspx.cs" Inherits="Kaizos.frmCancelShipmentaspx"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="errorMsg" id="errorMsg" runat="server">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    
    <asp:CustomValidator ID="val_Shipment" runat="server" 
            ControlToValidate="txtShipmentReference" 
            EnableClientScript="False" 
            ValidateEmptyText="True"
            ValidationGroup="grpShipment"
            onservervalidate="val_Shipment_ServerValidate">
    </asp:CustomValidator>
</div>

<fieldset class="first">
    <label for="txtShipmentReference">
        <asp:Label ID="lblShipRef" runat="server"
            Text="<%$ Resources:LocalString, CancelReference %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtShipmentReference" runat="server"></asp:TextBox>
    
    <label for="txtShipmentReference">
        <asp:Label ID="lblResponsible" runat="server"  CssClass="clsLabelLeft"
        Text="<%$ Resources:LocalString, CancelResponsible %>"></asp:Label>
    </label>
    <div class="group">
        <asp:RadioButton ID="rdResponsible1" GroupName="rdResponsible" Checked="true"
            Text="<%$ Resources:LocalString, Customer %>" runat="server" /><br />
        <asp:RadioButton ID="rdResponsible2" GroupName="rdResponsible" 
            Text="<%$ Resources:LocalString, Carrier %>" runat="server" />
    </div>
</fieldset>
<div id="buttons">
    <asp:Button ID="btnCancel" runat="server" 
        Text="<%$ Resources:LocalString, CancelShipping %>"
        ValidationGroup="grpShipment" onclick="btnCancel_Click"/>
</div>
</asp:Content>
