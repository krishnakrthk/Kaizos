<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmResult.aspx.cs" Inherits="Kaizos.frmResult" MasterPageFile="~/NewSite.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="first">
        <legend>
            <asp:Label ID="lblResult" runat="server" Text="Result"></asp:Label> 
        </legend>
        <asp:Label runat="server" ID="lblDisplayMsg" Text=""></asp:Label><br /><br />
            <asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" ></asp:LinkButton>
                
    </fieldset>
</asp:Content>
