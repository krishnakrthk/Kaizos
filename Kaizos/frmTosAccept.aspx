<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTosAccept.aspx.cs" Inherits="Kaizos.frmTosAccept" MasterPageFile="~/NewSite.master" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<fieldset id="Fieldset2" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, TOS%>"></asp:Label>
    </legend>
    <asp:Label ID="lblTos" runat="server" Text="<%$ Resources:LocalString, lblTosAccept1%>"></asp:Label>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="frmTosShow.aspx" >
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalString, lblTosAccept2%>"></asp:Label>
    </asp:HyperLink> <br />
    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, lblTosAccept3%>"></asp:Label>
</fieldset>
<div id = "buttons">
    <asp:Button ID="btnAccept" runat="server" Text="<%$ Resources:LocalString, AllAccept%>" onclick="btnAccept_Click" />
</div>
</asp:Content>