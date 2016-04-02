<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTos.aspx.cs" Inherits="Kaizos.frmTos" MasterPageFile="~/NewSite.master" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<fieldset id="Fieldset2" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, TOS%>"></asp:Label>
    </legend>
    <div class="fieldBloc">
        <asp:Label ID="lblTos" runat="server" Text="<%$ Resources:LocalString, TOSTos%>"></asp:Label>
        <asp:TextBox ID="txtTos" runat="server" TextMode="MultiLine" MaxLength="100000"></asp:TextBox>
    </div>
</fieldset>
<div id = "buttons">
    <asp:Label ID="lblMandatory" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>"></asp:Label>
    <asp:Button ID="btnUpdate" runat="server" onclick="Button1_Click" Text="<%$ Resources:LocalString, AllUpdate%>" ValidationGroup="grpTos" />
</div>

<div class="divSummaryTos">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed100000" Text="<%$ Resources:LocalString, valMaxAllowed100000 %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLength %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valTosSame" Text="<%$ Resources:LocalString, ValidationShouldSame %>" runat ="server" Visible= "false"></asp:Label>
                    

    <asp:CustomValidator ID="val_Tos" runat="server" ControlToValidate="txtTos" EnableClientScript="False" 
    ValidateEmptyText="True" ValidationGroup="grpTos" CssClass="clsErrorMessage" 
    onservervalidate="val_Login_ServerValidate"></asp:CustomValidator>
</div>
</asp:Content>

