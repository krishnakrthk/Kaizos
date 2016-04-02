<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmPublicTariff.aspx.cs" Inherits="Kaizos.frmPublicTariff" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:CustomValidator ID="val_PublicTariff" runat="server"
    ControlToValidate   ="ddlTariffReference"
    EnableClientScript  ="False"
    ValidateEmptyText   ="True"
    ValidationGroup     ="grpPublicTariff" 
    onservervalidate="val_PublicTariff_ServerValidate" > </asp:CustomValidator>

<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, PublicTariffValidation %>" runat ="server" Visible= "false"></asp:Label>

<fieldset class="first"> 
    <label for="ddlTariffReference">
        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, TariffType %>" ></asp:Label> *
    </label>
    <asp:DropDownList ID="ddlTariffReference" runat="server" AutoPostBack="True" 
         onselectedindexchanged="ddlTariffReference_SelectedIndexChanged">
    </asp:DropDownList>
</fieldset>

<asp:GridView ID="gv_PublicTariff" runat="server" AutoGenerateColumns="False"
    AllowSorting="True"     
    CaptionAlign="Top"
    onrowdatabound="gv_PublicTariff_RowDataBound" 
    onrowcreated="gv_PublicTariff_RowCreated"
	class="PublicTariff"
    >
    <Columns>

        <asp:TemplateField  HeaderText="<%$ Resources:LocalString, AddressBookListCountry %>">
            <ItemTemplate>
                <asp:label id="lblOriginCaption" runat="server" Text='<%# FormatCaption(Eval("Caption").ToString()) %>'></asp:label>
            </ItemTemplate>
        </asp:TemplateField>                    
                    
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, ShipType %>">
            <ItemTemplate>
                <asp:label id="lblShipType" runat="server" Text='<%#Eval("ShipType").ToString() %>'></asp:label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterServiceName %>">
            <ItemTemplate>
                <asp:label id="lblMasterServiceName" runat="server" Text='<%#Eval("MasterServiceName").ToString() %>'></asp:label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, PTWeightRanges %>">
            <ItemTemplate>
                <asp:label id="lblWeightRange" runat="server" Text='<%#Eval("WtRangeCaption").ToString() %>'></asp:label>
            </ItemTemplate>
			<ItemStyle CssClass="weight"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, Domestic %>" >
            <ItemTemplate>
                <asp:TextBox ID="txtDomestic" runat="server"  Text='<%#Eval("Dom").ToString() %>' CssClass="tableCellCentreAlignment"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, Europe %>" >
            <ItemTemplate>
                <asp:TextBox ID="txtEurope" runat="server"  Text='<%#Eval("Eur").ToString() %>' CssClass="tableCellCentreAlignment"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, International %>" >
            <ItemTemplate>
                <asp:TextBox ID="txtInternational" runat="server"  Text='<%#Eval("Int").ToString() %>' CssClass="tableCellCentreAlignment"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="MinWt" ItemStyle-CssClass="hiddencol">
            <ItemTemplate>
                <asp:TextBox ID="txtMinWt" runat="server"  Text='<%#Eval("MinWt").ToString() %>' CssClass="tableCellCentreAlignment"></asp:TextBox>
            </ItemTemplate>

			<ItemStyle CssClass="hiddencol"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="MaxWt" ItemStyle-CssClass="hiddencol">
            <ItemTemplate>
                <asp:TextBox ID="txtMaxWt" runat="server"  Text='<%#Eval("MaxWt").ToString() %>' CssClass="tableCellCentreAlignment"></asp:TextBox>
            </ItemTemplate>

			<ItemStyle CssClass="hiddencol"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField  HeaderText="" ItemStyle-CssClass="hiddencol">
            <ItemTemplate>
                <asp:label id="lblOrigin" runat="server" Text='<%#Eval("Caption").ToString() %>'></asp:label>
            </ItemTemplate>

			<ItemStyle CssClass="hiddencol"></ItemStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" ValidationGroup="grpPublicTariff"  />

</asp:Content>