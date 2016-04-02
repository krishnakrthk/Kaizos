<%@ Page Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmcarrierLabelDisplay.aspx.cs" Inherits="Kaizos.frmcarrierLabelDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:GridView ID="GridView1" runat="server" DataKeyNames="ShippingReference"  onrowcommand="GridView1_RowCommand">
    <Columns>
        <asp:TemplateField>
        <ItemTemplate>
            <asp:Button ID="dlRow" runat="server"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Text="Label" CommandName="GoEdit">
                       
            </asp:Button >
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
            <asp:Button ID="dlrow2" runat="server"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Text="Manifest" CommandName="GoEdit2">
        </asp:Button >
        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
    
<asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" 
        PostBackUrl="~/frmTariffDelayInterrogation.aspx" ></asp:LinkButton>
    
&nbsp;<br />
<br />
<asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" 
        Visible="False">LinkButton</asp:LinkButton>
</asp:Content>
