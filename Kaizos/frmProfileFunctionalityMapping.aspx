<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProfileFunctionalityMapping.aspx.cs" Inherits="Kaizos.frmProfileFunctionalityMapping" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gv_ProfileMapping" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" AllowSorting="True" CaptionAlign="Left">
                
                <Columns>
                    <asp:BoundField DataField="FunctionalName" HeaderText="<%$ Resources:LocalString, ProfileFunctionality%>">
                       </asp:BoundField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, PMN1%>" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkN1" runat="server" Checked = '<%#Eval("CheckN1") %>'/>
                    </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, PMN2%>" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkN2" runat="server" Checked = '<%#Eval("CheckN2")%>'/>
                    </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, PMCS%>" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkCS" runat="server" Checked = '<%#Eval("CheckCustomerService")%>'/>
                    </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, PMRF%>" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkRF" runat="server" Checked = '<%#Eval("CheckReferent")%>'/>
                    </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, PMAZ%>" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAZ" runat="server" Checked = '<%#Eval("CheckAuthorize")%>'/>
                    </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

    </asp:GridView>
    <asp:Button ID="btnUpdate" runat="server" Text="<%$ Resources:LocalString, AllUpdate%>" onclick="btnUpdate_Click" />
</asp:Content>



<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .clsGridRow
        {}
    </style>
</asp:Content>



