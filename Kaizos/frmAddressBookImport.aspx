<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddressBookImport.aspx.cs" Inherits="Kaizos.frmAddressBookImport" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="second"> 
    <legend>
        <asp:Label ID="lblZoning" runat="server" Text="<%$ Resources:LocalString, AddressBookImport%>" CssClass="clsLabelLeft"></asp:Label> 
    </legend>
    <div class="fieldBloc">
        <label for="FileUpload1">
            <asp:Label ID="lblAddressBookFile" runat="server" Text="<%$ Resources:LocalString, AddressBookImportFile %>" ></asp:Label> *
        </label>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Label id="lblMax" runat="server" Text="<%$ Resources:LocalString, AddressBookImportMaxsize %>"></asp:Label>
        <asp:Button ID="btnCreate" runat="server" 
            Text="<%$ Resources:LocalString, AddressBookImportImport %>" 
            onclick="btnCreate_Click"  ValidationGroup="grpAddressBookImport"/>
    </div>
</fieldset>

<div class="divSummaryAddressBook">
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_AddressBookImport" runat="server" 
        ControlToValidate="FileUpload1" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpAddressBookImport" CssClass="clsErrorMessage" onservervalidate="val_AddressBookImport_ServerValidate" >
    </asp:CustomValidator>
</div>
</asp:Content>
