<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddressBookList.aspx.cs" Inherits="Kaizos.frmAddressBookList" MasterPageFile="~/NewSite.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class ="divAddressBookList">
    <asp:GridView ID="gvAddressBookList" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" AllowSorting="True" CaptionAlign="Left">
               <Columns>
                   <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, AddressBookListCompanyName%>" />
                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, AddressBookListContactName%>" />
                   <asp:BoundField DataField="AddressType" HeaderText="<%$ Resources:LocalString, AddressBookListAddressType%>" />
                   <asp:BoundField DataField="TelephoneNo" HeaderText="<%$ Resources:LocalString, AddressBookListTelNo%>" />
                   <asp:BoundField DataField="FaxNo" HeaderText="<%$ Resources:LocalString, AddressBookListFaxNo%>" />
                   <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:LocalString, AddressBookListAddress1%>" />
                   <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:LocalString, AddressBookListAddress2%>" />
                   <asp:BoundField DataField="Address3" HeaderText="<%$ Resources:LocalString, AddressBookListAddress3%>" />
                   <asp:BoundField DataField="Zipcode" HeaderText="<%$ Resources:LocalString, AddressBookListZipcode%>" />
                   <asp:BoundField DataField="City" HeaderText="<%$ Resources:LocalString, AddressBookListCity%>" />
                   <asp:BoundField DataField="State" HeaderText="<%$ Resources:LocalString, AddressBookListState%>" />
                   <asp:BoundField DataField="Country" HeaderText="<%$ Resources:LocalString, AddressBookListCountry%>" />
                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, AddressBookListEmail%>" />
                   <asp:BoundField DataField="LastPickupMondayToThursday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpMonThur%>" />
                    <asp:BoundField DataField="LastPickupFriday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpFriday%>" />
                   <asp:BoundField DataField="Comments" HeaderText="<%$ Resources:LocalString, AddressBookListComments%>" />
                   <asp:BoundField DataField="ShipPreference" HeaderText="<%$ Resources:LocalString, AddressBookListShipPreference%>" />
                   <asp:BoundField DataField="AddressUsedFor" HeaderText="<%$ Resources:LocalString, AddressBookListAddressUsedFor%>" />
               </Columns>
    </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .clsGridRow
        {}
    </style>
</asp:Content>

