<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmTariffValidity.aspx.cs" Inherits="Kaizos.frmTariffValidity" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <asp:ScriptManager ID="ScriptManager1" runat="server"> 
    </asp:ScriptManager>
   
<div class="errorMsg" id="errorMsg1" runat="server">
<asp:Label ID="lblSearchError" runat="server" Text=""></asp:Label>
</div>
<div class="errorMsg" id="errorMsg2" runat="server">
<asp:Label ID="lblDateValidation" runat="server"></asp:Label>
</div>

<fieldset class="first camouflageTable"> 
    <legend>
        <asp:Label ID="lblTariffValidity" runat="server" Text="<%$ Resources:LocalString, TariffFilter %>"  CssClass="clsLabelLeft"></asp:Label>
    </legend>
    
    <label for="txtEmail">
        <asp:Label ID="lblTariffType" runat="server"  Text="<%$ Resources:LocalString, TariffType %>" ></asp:Label>
    </label>
    <asp:RadioButtonList runat="server" ID="rblTariffType"  AutoPostBack="true"
        RepeatDirection="Vertical" 
        onselectedindexchanged="rblTariffType_SelectedIndexChanged">
        <asp:ListItem Text="<%$ Resources:LocalString, CarrierPublic %>"  Value="CarrierPublic" > </asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, Purchase %>"  Value="Purchase"></asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, KeyCustomerNegotiated %>"  Value="KeyCustomer"></asp:ListItem>
    </asp:RadioButtonList>
    
    <label for="txtEmail">
        <asp:Label ID="lblCarrier" runat="server" Text="<%$ Resources:LocalString, Carrier %>"  ></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCarrier" runat="server" ></asp:DropDownList>
 </fieldset>

<div id="buttons">
    <asp:Button ID="btnFilter" runat="server" Text="<%$ Resources:LocalString, Filter %>"  
            onclick="btnFilter_Click" />    
</div>

<asp:GridView ID="gvTariffRef" runat="server" 
    AlternatingRowStyle-CssClass="gridAlternate" AutoGenerateColumns="False" 
    DataKeyNames="TariffReference" HeaderStyle-CssClass="gridHeader" 
    onrowcancelingedit="gvTariffRef_RowCancelingEdit" 
    onrowediting="gvTariffRef_RowEditing" 
    onrowupdating="gvTariffRef_RowUpdating">
    <Columns>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, TariffReference %>">
            <ItemTemplate>
                <asp:Label ID="lblTariffRerence" runat="server" 
                    Text='<%#Eval("TariffReference").ToString() %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, StartDate %>">
            <ItemTemplate>
                <asp:Label ID="lblStartDate" runat="server" 
                    Text='<%# FormatDateString(Eval("StartDate").ToString()) %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtStartDate" runat="server" 
                    Text='<%# FormatDateString(Eval("StartDate").ToString()) %>'></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    CssClass="clsCalendar" TargetControlID="txtStartDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, EndDate %>">
            <ItemTemplate>
                <asp:Label ID="lblEndDate" runat="server" 
                    Text='<%# FormatDateString(Eval("EndDate").ToString()) %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEndDate" runat="server" 
                    Text='<%# FormatDateString(Eval("EndDate").ToString()) %>'></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    CssClass="clsCalendar" TargetControlID="txtEndDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </EditItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="<%$ Resources:LocalString, Archived %>">
            <ItemTemplate>
            <asp:CheckBox ID="cbArchived" runat="server" Checked='<%#Eval("Archived")%>' Enabled="false"/>
            </ItemTemplate>
            <EditItemTemplate>
            <asp:CheckBox ID="cbArchived" runat="server" Checked='<%#Eval("Archived")%>'/>                        
            </EditItemTemplate>
        </asp:TemplateField>



        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" 
                    CommandName="Edit" Text="<%$ Resources:LocalString, AllEdit %>" ></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="true" 
                    CommandName="Update" Text="<%$ Resources:LocalString, Update %>" ></asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" 
                    CommandName="Cancel" Text="<%$ Resources:LocalString, AllCancel %>" ></asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
