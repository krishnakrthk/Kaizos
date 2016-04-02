<%@ Page Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmDeliveryDelayImport.aspx.cs" Inherits="Kaizos.frmDeliveryDelayImport" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="third"> 
    <legend>
        <asp:Label ID="lblDelayImport" runat="server" Text="<%$ Resources:LocalString, CarrierServiceDelayImport %>"  CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <label for="FileUpload1">
        <asp:Label ID="lblTariffReferencE" runat="server" Text="<%$ Resources:LocalString, TariffReference %>" ></asp:Label>
    </label>
    <asp:FileUpload ID="FileUpload1"  runat="server"/>
</fieldset>

<div id="buttons">
    <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:LocalString, Upload %>" onclick="btnUpload_Click" />
</div>

</asp:Content>
