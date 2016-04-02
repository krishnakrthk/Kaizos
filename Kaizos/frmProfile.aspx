<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProfile.aspx.cs" Inherits="Kaizos.frmProfile" MasterPageFile="~/Site.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class = "clsProfileMain">
        <fieldset id="Fieldset2" runat="server" class ="FieldSet">
            <legend>
                <asp:Label ID="Label3" runat="server" Text="Label">Profile Management</asp:Label>
            </legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Label">Profile Name*</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>    
            <div class="clsProfileButton">
                <asp:Button ID="Button1" runat="server" Text="Create" Height="21px" />
            </div>
            <div class = "clsProfileMandatory">
                <asp:Label ID="Label5" runat="server" Text="*Mandatory fields" Font-Size="Smaller"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
