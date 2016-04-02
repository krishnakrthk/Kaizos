<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFranshise.aspx.cs" Inherits="Kaizos.frmFranshise" MasterPageFile="~/NewSite.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="divSummaryFranchise">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, ValidationMaxAllowed %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_Franchise" runat="server" 
        ControlToValidate="txtCompanyName" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpFranchise" 
        CssClass="clsErrorMessage" onservervalidate="val_Franchise_ServerValidate">
    </asp:CustomValidator>
    <asp:ModalPopupExtender runat="server" 
                            ID="ModalPopupExtender2" 
                            TargetControlID="btnCarrierAccount" 
                            PopupControlID="Panel5" 
                            BackgroundCssClass="modalBackground"                        
                            DropShadow="false"/> 

    <asp:Panel ID="Panel5" runat="server" CssClass="customerModelWindow"> 
        <asp:UpdatePanel ID="UpdatePanel4" runat="Server" UpdateMode="Conditional"> 
            <ContentTemplate> 
                    <asp:GridView ID="gvCarrierAccountReference" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" 
                        AllowSorting="True" CaptionAlign="Left" 
                            onrowcommand="gvCarrierAccountReference_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, FranchisePPCarrierName %>">
                            <ItemTemplate>
                                <asp:Label ID="lblCarrierName" runat="server" Text='<%#Eval("CarrierName").ToString()%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, FranchisePPAccountRef %>">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAccountReference" runat="server"></asp:TextBox>                
                            </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </ContentTemplate>       
        </asp:UpdatePanel> 
        <asp:Button ID="btnPPSubmit" runat="server" 
            Text="<%$ Resources:LocalString, AllSubmit%>" onclick="btnPPSubmit_Click" CommandName="MyEdit"/>
    </asp:Panel>
</div>

<fieldset id="Fieldset2" runat="server" class ="second">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, Franchisecompany%>">Company</asp:Label>
    </legend>
    <label for="txtName">
        <asp:Label ID="lblcompanyName" runat="server" Text="<%$ Resources:LocalString, FranchiseCompanyName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, FranchiseAddress1%>"></asp:Label>                    
    </label>
    <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox>                    
    
    <label for="txtName">
        <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, FranchiseName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, FranchiseAddress2%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblLegalForm" runat="server" Text="<%$ Resources:LocalString, FranchiseLegalForm%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtLegalForm" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, FranchiseAddress3%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblCommercialName" runat="server" Text="<%$ Resources:LocalString, FranchiseCommercialName%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCommercialName" runat="server" MaxLength="60">Kaizos</asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblZipCode" runat="server" Text="<%$ Resources:LocalString, FranchiseZipcode%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtZipCode" runat="server" MaxLength="12"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblManPower" runat="server" Text="<%$ Resources:LocalString, FranchiseManPower%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtManPower" runat="server" MaxLength="6"></asp:TextBox>                
    
    <label for="txtName">
        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, FranchiseCity%>"></asp:Label>                    
    </label>
    <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, FranchiseEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, FranchiseCountry%>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" 
         onselectedindexchanged="ddlCountry_SelectedIndexChanged"> </asp:DropDownList>
    
    <label for="txtName">
        <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, FranchisePhoneNumber%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>                    
    
    <label for="txtName">
        <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:LocalString, FranchiseLanguage%>"></asp:Label> 
    </label>
    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="True" 
         onselectedindexchanged="ddlLanguage_SelectedIndexChanged"> </asp:DropDownList>
    
    <label for="txtName">
        <asp:Label ID="lblFaxNumber" runat="server" Text="<%$ Resources:LocalString, FranchiseFaxNumber%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtFaxNumber" runat="server" MaxLength="20"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblChalandiseZone" runat="server" Text="<%$ Resources:LocalString, FranchiseChalandiseZone%>"></asp:Label>
    </label>
    <asp:TextBox ID="rtxtChalandiseZone" runat="server" TextMode="MultiLine" 
        MaxLength="5000" ToolTip="ISO Code + ZIPCODE with Comma seperator (Ex: FR45000,FR69001)" CssClass="smallArea"></asp:TextBox>                    
    
    <label for="txtName">
        <asp:Label ID="lblSiretNo" runat="server" Text="<%$ Resources:LocalString, FranchiseSiretNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtSiretNo" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblFirmCreateDate" runat="server" 
                            Text="<%$ Resources:LocalString, FranchiseFirmCreateDate%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtValidDate" runat="server"></asp:TextBox>
    <asp:CalendarExtender ID="calValidDate" runat="server" TargetControlID="txtValidDate" 
        CssClass="clsCalendar" Format="dd/MM/yyyy">
    </asp:CalendarExtender>
    
    <label for="txtName">
        <asp:Label ID="lblVatNo" runat="server" Text="<%$ Resources:LocalString, FranchiseVatNo%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtVatNo" runat="server" MaxLength="30"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblPaymentDelay" runat="server" Text="<%$ Resources:LocalString, FranchisePaymentDelay%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtPaymentDelay" runat="server"></asp:TextBox>
    
    <label for="txtName">
        <asp:Label ID="lblCarrierAccountReference" runat="server" Text="<%$ Resources:LocalString, FranchiseCarrierReference%>"></asp:Label>
    </label>
    <div class="fieldBloc">
        <asp:Button ID="btnCarrierAccount" runat="server" Text="<%$ Resources:LocalString, Edit%>" onclick="btnCarrierAccount_Click" />    
    </div>

</fieldset>

<div id="buttons">
    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>" Font-Size="Smaller"></asp:Label>
    <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>" 
        onclick="btnSubmit_Click" ValidationGroup="grpFranchise" />
    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click" />
</div>

</asp:Content>