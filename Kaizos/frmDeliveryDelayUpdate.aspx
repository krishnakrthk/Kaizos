<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmDeliveryDelayUpdate.aspx.cs" Inherits="Kaizos.frmDeliveryDelayUpdate" SmartNavigation="True" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:CustomValidator ID="val_DeliveryDelayUpdate" runat="server"
        ControlToValidate   ="txtDelay"
        EnableClientScript  ="False"
        ValidateEmptyText   ="True"
        ValidationGroup     ="grpDeliveryDelayUpdate" 
        CssClass            ="clsErrorMessage" onservervalidate="val_DeliveryDelayUpdate_ServerValidate">
</asp:CustomValidator>
<asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber  %>" runat ="server" Visible= "false"></asp:Label>

    
<fieldset class="first">
    <legend>
        <asp:Label ID="lblCaption" runat="server" Text="<%$ Resources:LocalString, DeliveryDelay %>" CssClass="tableCellLeftAlignment"></asp:Label>
    </legend>
    <label for="rblTariffType">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalString, Source %>"></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlOrigin" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlOrigin_SelectedIndexChanged" >
    </asp:DropDownList>
    
    <label for="rblTariffType">
        <asp:Label ID="Label5" runat="server" 
                                Text="<%$ Resources:LocalString, Destination %>"></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlDestination_SelectedIndexChanged" >
    </asp:DropDownList>
    
    <label for="rblTariffType">
        <asp:Label ID="lblDeliveryDelay" runat="server" 
                            Text="<%$ Resources:LocalString, DeliveryDelay %>"></asp:Label>*
    </label>
    <asp:TextBox ID="txtDelay" runat="server" Text=""></asp:TextBox>  
    
</fieldset>

<asp:GridView ID="gvDeliveryDelay" runat="server"  
        AutoGenerateColumns="False" 
        HeaderStyle-CssClass="gridHeader"    
        onrowcommand="gvDeliveryDelay_RowCommand" 
        onrowediting="gvDeliveryDelay_RowEditing" >
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton runat="server" ID="imgEdit" ImageUrl="~/Image/left_arrow.png" CommandName="MyEdit"/>
            </ItemTemplate>
        </asp:TemplateField>
                    
        <asp:BoundField DataField="Origin" HeaderText="Origin"   SortExpression="Origin" />
        <asp:BoundField DataField="Destination" HeaderText="Destination" SortExpression="Destination"/>
        <asp:BoundField DataField="Delay" HeaderText="Delay" />
    </Columns>
</asp:GridView>

<div id="buttons">
    <asp:Button ID="btnUpdate" runat="server" 
        Text="<%$ Resources:LocalString, Update %>" onclick="btnUpdate_Click" 
        ValidationGroup="grpDeliveryDelayUpdate" />
    
    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalString, Back %>" 
        onclick="btnBack_Click"  />
</div>

</asp:Content>
