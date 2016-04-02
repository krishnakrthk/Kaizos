<%@ Page Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmCarrierList.aspx.cs" Inherits="Kaizos.frmCarrierList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:GridView ID="gvCarrier" runat="server" AutoGenerateColumns="false" DataKeyNames="CarrierID" 
               AllowSorting="True" CaptionAlign="Top" 
        HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow"
            onrowcommand="gvCarriers_RowCommand" >
              <Columns>
                   <asp:TemplateField>
                   <ItemTemplate>
                       <asp:Button ID="dlRow" runat="server" Text="<%$ Resources:LocalString, AllEdit%>" CommandName="GoEdit"/>
                   </ItemTemplate>
                </asp:TemplateField>
                  <asp:BoundField DataField="CarrierName" HeaderText="<%$ Resources:LocalString, CarrierNameWithSpace %>"></asp:BoundField>             
                  <asp:TemplateField HeaderText="<%$ Resources:LocalString, ReferencedCarrier %>" > 
                  <ItemTemplate>
                      <asp:CheckBox ID="chkReferencedCarrier" runat="server" Text=""  Checked='<%# Eval("ReferencedCarrier") %>' Enabled="false"/>
                  </ItemTemplate>
                  </asp:TemplateField>

                   <asp:BoundField DataField="ClaimDelay" HeaderText="<%$ Resources:LocalString, ClaimResoulutionDelay%>" ></asp:BoundField>
                  
                  <asp:TemplateField HeaderText="<%$ Resources:LocalString, Active%>" >
                  <ItemTemplate>
                      <asp:CheckBox ID="chkActive"  runat="server" Text=""   Checked='<%# Eval("Active") %>' Enabled="false"/>
                  </ItemTemplate>
                  </asp:TemplateField>
               </Columns>
    </asp:GridView>
</asp:Content>
