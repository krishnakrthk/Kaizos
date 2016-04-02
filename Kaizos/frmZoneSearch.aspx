<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmZoneSearch.aspx.cs" Inherits="Kaizos.frmZoneSearch" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="errorMsg"><asp:Label ID="lblSearchResult" runat="server" Text="<%$ Resources:LocalString, SearchResult %>" Visible="false"></asp:Label></div>

<fieldset class="third"> 
    <legend>
        <asp:Label ID="lblZoning" runat="server" Text="Zone Search" CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <label for="txtTariffReference">
        <asp:Label ID="lblTariffReferencE" runat="server" Text="<%$ Resources:LocalString, TariffReference %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtTariffReference" runat="server" Text =""></asp:TextBox>
    <div class="fieldBloc">
        <asp:Button id="btnSearch" runat="server" Text="<%$ Resources:LocalString, Search %>"  onclick="btnSearch_Click"  />    
    </div>
</fieldset>

<asp:GridView ID="gvSearch" runat="server"                  AutoGenerateColumns="False" 
            HeaderStyle-CssClass="gridHeader"               RowStyle-CssClass="gridRow"
            AlternatingRowStyle-CssClass="gridAlternate"    DataKeyNames="ZoneID" 
            onrowcommand="gvSearch_RowCommand">
<Columns>
    <asp:BoundField DataField="ZoneID"      HeaderText="<%$ Resources:LocalString, Carrier      %>" Visible="false" />
    <asp:BoundField DataField="CarrierName" HeaderText="<%$ Resources:LocalString, Carrier      %>" />
    <asp:BoundField DataField="ZoneName"    HeaderText="<%$ Resources:LocalString, Zone         %>" />
            
    <asp:TemplateField>
        <ItemTemplate>
        <asp:Label ID="lblStartDate" runat="server" Text='<%# FormatDateString(Eval("StartDate").ToString()) %>' ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
            
    <asp:TemplateField>
        <ItemTemplate>
        <asp:Label ID="lblEndDate" runat="server" Text='<%# FormatDateString(Eval("EndDate").ToString()) %>' ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>


            
    <asp:BoundField DataField="Direction"   HeaderText="<%$ Resources:LocalString, Direction    %>" />

    <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton     runat="server"      ID="ibtEdit" Text="<%$ Resources:LocalString, AllEdit %>"  
                                AlternateText   ="Edit" 
                                ToolTip         ="<%$ Resources:LocalString, Edit %>"
                                CommandName     ="Edit" 
                                CommandArgument ='<%# Eval("ZoneID").ToString() %>' />
        </ItemTemplate>
    </asp:TemplateField>

</Columns>

</asp:GridView>

</asp:Content>
