<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmZoneCreationUpdate.aspx.cs" Inherits="Kaizos.frmZoneCreationUpdate" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="errorMsg" id="errorMsg" runat ="server">
    <asp:CustomValidator ID="val_Shipment" runat="server"
            ControlToValidate   ="txtTariffReference"
            EnableClientScript  ="False"
            ValidateEmptyText   ="True"
            ValidationGroup     ="grpShipment"  
            onservervalidate    ="val_Shipment_ServerValidate">
    </asp:CustomValidator>
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNotExist" Text="<%$ Resources:LocalString, ValidationNotExist %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlreadyExist" Text="<%$ Resources:LocalString, ValidationAlreadyExist %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valzone" Text="<%$ Resources:LocalString,valzone %>" runat="server" Visible= "false" ></asp:Label>
</div>

<fieldset class="third camouflageTable"> 
    <legend>
        <asp:Label ID="lblZoning" runat="server" Text="Zoning" CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <label for="txtTariffReference">
        <asp:Label ID="lblTariffReferencE" runat="server" Text="<%$ Resources:LocalString, TariffReference %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtTariffReference" runat="server" Text ="" MaxLength="30"  autocomplete = "off"
        ontextchanged="txtTariffReference_TextChanged" ></asp:TextBox>
    
    <label for="txtZoneName">
        <asp:Label ID="lblZone" runat="server" Text="<%$ Resources:LocalString, ZoneName %>"></asp:Label>*
    </label>
    <asp:TextBox ID="txtZoneName" runat="server" Text ="" MaxLength="8"  autocomplete = "off"></asp:TextBox>
    
    <label for="cblDirection">
        <asp:Label ID="lblDirection" runat="server" Text="<%$ Resources:LocalString, Direction %>"></asp:Label>*
    </label>
    <asp:CheckBoxList ID="cblDirection" runat="server" RepeatDirection="Vertical">
        <asp:ListItem>Export</asp:ListItem>
        <asp:ListItem>Import</asp:ListItem>
        <asp:ListItem>Third Party</asp:ListItem>
    </asp:CheckBoxList>
    
    <label for="cblServiceType">
        <asp:Label ID="lblServiceType" runat="server" Text="<%$ Resources:LocalString, MasterServiceName %>" ></asp:Label>*
    </label>
    <asp:CheckBoxList ID="cblServiceType" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
    
    <label for="rblGeographical">
        <asp:Label ID="lblGeographical" runat="server" Text="<%$ Resources:LocalString, GeographicalCoverage %>" ></asp:Label>
    </label>
    <asp:RadioButtonList runat="server" ID="rblGeographical" 
            RepeatDirection="Vertical" AutoPostBack="True" 
            onselectedindexchanged="rblGeographical_SelectedIndexChanged">
        <asp:ListItem Text="<%$ Resources:LocalString, ZoneCreateCountry %>" Value="Country" ></asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, ZoneCreateZipcode %>" Value="ZipCode"></asp:ListItem>
    </asp:RadioButtonList>
    
    <label for="ddlCountry"><asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, Country %>" Visible="false" ></asp:Label></label>
    <asp:DropDownList ID="ddlCountry" runat="server" Visible="false"></asp:DropDownList>
    
    <label for="txtCountryZip"><asp:Label ID="lblZipCode" runat="server" Text="<%$ Resources:LocalString, CountryZipCode %>" ></asp:Label></label>
    <asp:TextBox ID="txtCountryZip" runat="server" TextMode="MultiLine" MaxLength="500000"></asp:TextBox>
    
</fieldset>

<div id="buttons">
    *<asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, Mandatory %>"  CssClass="clsLabelLeft"></asp:Label>
    <asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:LocalString, Create %>" 
            onclick="btnCreate_Click" ValidationGroup="grpShipment"/>
</div>

</asp:Content>
