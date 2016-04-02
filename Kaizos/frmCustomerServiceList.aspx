<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerServiceList.aspx.cs" Inherits="Kaizos.frmCustomerServiceList" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gvCSList" runat="server" AutoGenerateColumns="false"
               AllowSorting="True" CaptionAlign="Top" HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow"
            onrowcommand="gvCSList_RowCommand" 
            onrowupdated="gvCSList_RowUpdated" 
            onrowupdating="gvCSList_RowUpdating">
               <Columns>
               <asp:TemplateField>
                   <ItemTemplate>
                       <asp:Button ID="dlRow" runat="server" Text="<%$ Resources:LocalString, AllEdit%>" CommandName="GoEdit"/>
                   </ItemTemplate>
                </asp:TemplateField>
                  <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, FranchiseListCompanyName%>"><ControlStyle Width="10%" /></asp:BoundField>
                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, FranchiseListLoginID%>" ><ControlStyle Width="10%" /></asp:BoundField>
                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, FranchiseListContactName%>" ><ControlStyle Width="10%" /></asp:BoundField>
                   <asp:BoundField DataField="Language" HeaderText="<%$ Resources:LocalString, FranchiseListLanguage%>" ><ControlStyle Width="10%" /></asp:BoundField>
               </Columns>
    </asp:GridView>
</asp:Content>


<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .clsGridRow
        {}
    </style>
</asp:Content>