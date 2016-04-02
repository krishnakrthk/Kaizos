<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTOSShow.aspx.cs" Inherits="Kaizos.frmTOSShow" MasterPageFile="~/NewSite.master" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<fieldset id="Fieldset2" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, TOS%>"></asp:Label>
    </legend>
    <asp:Label ID="lblTos" runat="server"></asp:Label>
</fieldset>
<div id = "buttons">
    <asp:Button ID="btnBack" runat="server" 
        Text="<%$ Resources:LocalString, AllBack%>" onclick="btnBack_Click" 
        PostBackUrl="frmTosAccept.aspx" />
</div>
</asp:Content>
