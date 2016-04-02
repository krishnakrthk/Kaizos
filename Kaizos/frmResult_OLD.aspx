<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="frmResult.aspx.cs" Inherits="Kaizos.WebForm1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="divDeliveryDelayImportHeader">
<fieldset>
<legend><asp:Label ID="lblResult" runat="server" Text="Result" CssClass="clsLabelLeft"></asp:Label> </legend>
<table>
    <tr class="clsLabelCentre">
    <td>
        <asp:Label runat="server" ID="lblDisplayMsg" Text=""></asp:Label>
    </td>
    </tr>
    <tr>
    <td>
    <asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" ></asp:LinkButton>
    </td>
    </tr>
</table>
</fieldset>

</asp:Content>
