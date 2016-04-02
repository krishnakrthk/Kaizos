<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmTariffImportation.aspx.cs" Inherits="Kaizos.frmTariffImportation" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="errorMsg"  ID="errorMsg1" runat="server">
	<asp:CustomValidator ID="val_Shipment" runat="server"
				ControlToValidate   ="FileUpload1"
				EnableClientScript  ="False"
				ValidateEmptyText   ="True"
				ValidationGroup     ="grpTariffImport" 
				onservervalidate    ="val_Shipment_ServerValidate">
	</asp:CustomValidator>
</div>
<asp:Label ID="valInvalidExtension" Text="<%$ Resources:LocalString, ValidationFileExt %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valFileSize" Text="<%$ Resources:LocalString, ValidationFileSize %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>

<div class="errorMsg"  ID="errorMsg2" runat="server">
	<asp:CustomValidator ID="val_Rule" runat="server"
			ControlToValidate   ="FileUpload1"
			EnableClientScript  ="False"
			ValidateEmptyText   ="True"
			ValidationGroup     ="grpTariffRule"
			onservervalidate    ="val_Rule_ServerValidate">
	</asp:CustomValidator>
</div>
<asp:Label ID="val_Numeric"     Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="val_lessthanN"   Text="<%$ Resources:LocalString, NumberLessThenN %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="val_Empty"       Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>

<fieldset class="third"> 
    <legend>
        <asp:Label ID="lblZoning" runat="server" Text="Tariff Import" CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <label for="ddlCarrier">
        <asp:Label ID="lblCarrier" runat="server" Text="<%$ Resources:LocalString, Carrier %>"></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlCarrier" runat="server" 
        ontextchanged="ddlCarrier_TextChanged" AutoPostBack="true">
    </asp:DropDownList>
    
    <label for="ddlTariffReference">
        <asp:Label ID="lblTariffRef" runat="server" Text="<%$ Resources:LocalString, TariffReference %>" ></asp:Label> *
    </label>
    <asp:DropDownList ID="ddlTariffReference" runat="server"></asp:DropDownList>
    
    <label for="FileUpload1">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalString, TariffFile %>" ></asp:Label> *
    </label>
    <asp:FileUpload ID="FileUpload1" runat="server"  />
    <asp:Label id="lblMax" runat="server" Text="Max 10mb"></asp:Label>
    
</fieldset>

<fieldset class="third"> 
    <legend>
        <asp:Label id="lblCalculationRule" class="clsLabelLeft" runat="server" Text="<%$ Resources:LocalString, CalculationRules %>" ></asp:Label>
    </legend>
    <label for="txtServiceType">
        <asp:Label ID="lbl1" runat="server" Text="<%$ Resources:LocalString, ServiceTypeCode %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtServiceType" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtZoneList">
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, ZoneList %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtZoneList" runat="server" MaxLength="10"></asp:TextBox>
    
    <label for="txtZoneList">
        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalString, GrossMargin %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtGrossMargin" runat="server"></asp:TextBox>
    
    <asp:GridView ID="gv_Calculation" runat="server" AutoGenerateColumns="False"
        AllowSorting="True" CaptionAlign="Top"
        CellPadding="0" Width="100%"
        HeaderStyle-CssClass="gridHeader"
        AlternatingRowStyle-CssClass="gridAlternate"
        RowStyle-CssClass="gridRow" onrowcommand="gv_Calculation_RowCommand" 
        onrowdeleting="gv_Calculation_RowDeleting" >
        <Columns>
            <asp:BoundField DataField="ServiceTypeCode" HeaderText="<%$ Resources:LocalString, ServiceTypeCode %>" ><ControlStyle Width="10%" /></asp:BoundField>
            <asp:BoundField DataField="ZoneList" HeaderText="<%$ Resources:LocalString, ZoneList %>" ><ControlStyle Width="10%" /></asp:BoundField>
            <asp:BoundField DataField="GrossMargin" HeaderText="<%$ Resources:LocalString, GrossMargin %>" ><ControlStyle Width="10%" /></asp:BoundField>
                    
            <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ImageUrl="~/Image/delete.png" CommandName="Delete" ID="dlRow" runat="server"/>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>

            <HeaderStyle CssClass="gridHeader" />
            <AlternatingRowStyle CssClass="gridAlternate" />
            <RowStyle CssClass="gridRow" />
            <PagerStyle CssClass="clsGridPagerRow" />
    </asp:GridView>
</fieldset>
<div id="buttons">
    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryfield%>" Font-Size="Smaller"></asp:Label>
    <asp:Button ID="btnAddLine" runat="server" Text="<%$ Resources:LocalString, AddLine %>" 
                onclick="btnAddLine_Click" ValidationGroup="grpTariffRule" />

    <asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:LocalString, AddressBookImportImport %>"  onclick="btnCreate_Click"  ValidationGroup="grpTariffImport" />
</div>

</asp:Content>
