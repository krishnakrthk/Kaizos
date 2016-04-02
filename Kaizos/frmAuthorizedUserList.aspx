<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAuthorizedUserList.aspx.cs" Inherits="Kaizos.frmAuthorizedUserList" MasterPageFile="~/NewSite.master" Title="Kaizos - Authorized User List"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:GridView ID="gv_AZL" runat="server" AutoGenerateColumns="false" DataKeyNames="AccountNo" 
        HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" 
        SortedAscendingHeaderStyle-CssClass="gridHeader" 
        SortedAscendingCellStyle-CssClass="gridRow"
        onrowcommand="gv_AZL_RowCommand" onrowupdated="gv_AZL_RowUpdated" 
        onrowupdating="gv_AZL_RowUpdating" AllowSorting="true" 
        onrowcreated="gv_AZL_RowCreated" onsorting="gv_AZL_Sorting" 
        onrowdatabound="gv_AZL_RowDataBound" >
        <Columns>
            <asp:BoundField HeaderText="" Visible="false"> </asp:BoundField>
            <asp:BoundField DataField="ContactName" SortExpression="ContactName" HeaderText="<%$ Resources:LocalString, AZLName %>"></asp:BoundField>
            <asp:BoundField DataField="EmailId" HeaderText="<%$ Resources:LocalString, AZLLoginID%>"></asp:BoundField>
            <asp:BoundField DataField="TelephoneNo" HeaderText="<%$ Resources:LocalString, AZLPhoneNo%>"></asp:BoundField>
            <asp:TemplateField  Visible="true">
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgEdit" ImageUrl="~/Image/user.jpg"  Visible="true"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="dlRow" runat="server" Text="<%$ Resources:LocalString, AllEdit%>" CommandName="GoEdit"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserType" HeaderText="" Visible="true"> </asp:BoundField>
        </Columns>
</asp:GridView>
</asp:Content>



