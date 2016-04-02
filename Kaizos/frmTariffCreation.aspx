<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmTariffCreation.aspx.cs" Inherits="Kaizos.frmTariffCreation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
    </asp:ScriptManager>

	<div class="errorMsg" id="errorMsg" runat="server">
		<asp:Label ID="lblTariffRefErrMsg" runat="server" Text=""></asp:Label>
		<asp:Label ID="lblContainerTypeErrMsg" runat="server" Text=""></asp:Label>
		<asp:Label ID="lblStartDateErrMsg" runat="server" Text=""></asp:Label>
		<asp:Label ID="lblDateValidation" runat="server" Text=""></asp:Label>
		<asp:Label ID="lblKeyUserErrMsg" runat="server" Text=""></asp:Label>
	</div>

<fieldset class="fourth camouflageTable"> 
    <legend>
        <asp:Label ID="lblTariffCreation" runat="server" Text="<%$ Resources:LocalString, TariffCreation %>"></asp:Label>
    </legend>
    <label for="ddlCarrier">
        <asp:Label ID="lblCarrier" runat="server" Text="<%$ Resources:LocalString, Carrier %>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCarrier" runat="server"></asp:DropDownList>
    <br />
    <label for="txtTariffReference">
        <asp:Label ID="lblTariffReference" runat="server" Text="<%$ Resources:LocalString, TariffReference %>"></asp:Label> *
    </label>
	<asp:TextBox ID="txtTariffReference" runat="server"></asp:TextBox>  
	<br />
    <label for="rblTariffType">
        <asp:Label ID="lblTariffType" runat="server" Text="<%$ Resources:LocalString, TariffType %>" ></asp:Label> *
    </label>
    <asp:RadioButtonList runat="server" ID="rblTariffType" 
        RepeatDirection="Vertical" AutoPostBack="True" 
        onselectedindexchanged="rblTariffType_SelectedIndexChanged">
        <asp:ListItem Text="<%$ Resources:LocalString, CarrierPublic %>"            Value="CarrierPublic" > </asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, Purchase %>"                 Value="Purchase"></asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, KeyCustomerNegotiated %>"    Value="KeyCustomer"></asp:ListItem>
    </asp:RadioButtonList>

    <tr id="trKeyUserRef1" runat="server">
        <td class="camouflageTable">
         <label for="lblKeyUserReference">
            <asp:Label ID="lblKeyUserReference" runat="server" Text="<%$ Resources:LocalString, KeyUserReference %>"  ></asp:Label> *
         </label>  
        </td>
        <td class="camouflageTable">
         <asp:TextBox ID="txtKeyUserReference" runat="server"></asp:TextBox>  
        </td>
    </tr>
	<br />
    <label for="cblContainerType">
        <asp:Label ID="lblContainerType" runat="server" Text="<%$ Resources:LocalString, ContainerType %>" ></asp:Label> *
    </label>
    <asp:CheckBoxList ID="cblContainerType" runat="server" RepeatDirection="Vertical">
        <asp:ListItem>Letter</asp:ListItem>
        <asp:ListItem>Parcel</asp:ListItem>
        <asp:ListItem>Pallet</asp:ListItem>
    </asp:CheckBoxList>    
    <br />
    <label for="txtStartDate">
        <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:LocalString, StartDate %>" ></asp:Label> *
    </label>
    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox> 
    <asp:CalendarExtender ID="calenderStartDate" runat="server" TargetControlID="txtStartDate" Format="dd/MM/yyyy" CssClass="clsCalendar"></asp:CalendarExtender>
    
    <br />
    <label for="txtEndDate">
        <asp:Label ID="lblEndDate" runat="server" Text="<%$ Resources:LocalString, EndDate %>" ></asp:Label> *
    </label>
    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>  
    <asp:CalendarExtender ID="calenderEndDate" runat="server" TargetControlID="txtEndDate" Format="dd/MM/yyyy" CssClass="clsCalendar"></asp:CalendarExtender>
    
    
  
</fieldset>

<div id="buttons">
    <asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:LocalString, Create %>"  onclick="btnCreate_Click" />
</div>

</asp:Content>
