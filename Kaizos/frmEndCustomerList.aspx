<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEndCustomerList.aspx.cs" Inherits="Kaizos.frmEndCustomerList" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gvFranchiseList" runat="server" AutoGenerateColumns="false"
               AllowSorting="True" CaptionAlign="Top" 
        HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" 
        onrowcommand="gvFranchiseList_RowCommand" 
        onrowupdated="gvFranchiseList_RowUpdated" 
        onrowupdating="gvFranchiseList_RowUpdating">
               <Columns>
               <asp:TemplateField>
                   <ItemTemplate>
                       <asp:Button ID="dlsimulate" runat="server" Text="<%$ Resources:LocalString, FRListGrossMargin%>" CommandName="GoSimulate"/>
                   </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField>
                   <ItemTemplate>
                       <asp:Button ID="dlRow" runat="server" Text="<%$ Resources:LocalString, AllEdit%>" CommandName="GoEdit"/>
                   </ItemTemplate>
                </asp:TemplateField>
                  <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, FranchiseListCompanyName%>"></asp:BoundField>
                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, FranchiseListLoginID%>" ></asp:BoundField>
                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, FranchiseListContactName%>" ></asp:BoundField>
                   <asp:BoundField DataField="Language" HeaderText="<%$ Resources:LocalString, FranchiseListLanguage%>" ></asp:BoundField>
               </Columns>
    </asp:GridView>
</asp:Content>


<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .clsGridRow
        {}
    </style>
</asp:Content>