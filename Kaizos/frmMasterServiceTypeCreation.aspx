<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmMasterServiceTypeCreation.aspx.cs" Inherits="Kaizos.frmMasterServiceTypeCreation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
<div class="errorMsg" id="errorMsg" runat="server">
	<asp:CustomValidator ID="val_MasterServiceYype" runat="server"
		ControlToValidate   ="txtServiceTypeName"
		EnableClientScript  ="False"
		ValidateEmptyText   ="True"
		ValidationGroup     ="grpMasterServiceCreation" 
		onservervalidate="val_MasterServiceYype_ServerValidate"> 
	</asp:CustomValidator>
</div>
<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valNotFound" Text="<%$ Resources:LocalString, ValidationNotFound %>" runat ="server" Visible= "false"></asp:Label>
    
<fieldset class="third camouflageTable">
    <legend>
        <asp:Label ID="lblCaption" runat="server" 
            Text="<%$ Resources:LocalString, ServiceTypeName %>"></asp:Label>
    </legend>  
    
    <label for="txtEmail">
        <asp:Label ID="lblServiceTypeName" runat="server" 
                Text="<%$ Resources:LocalString, ServiceTypeName %>"></asp:Label>*
    </label>
    <asp:TextBox ID="txtServiceTypeName" runat="server" Text="" MaxLength="30"></asp:TextBox>  
                
    <label for="txtEmail">
        <asp:Label ID="lblPriority" runat="server" 
                Text="<%$ Resources:LocalString, Priority %>"></asp:Label>*  
    </label>
    <asp:RadioButtonList runat="server" ID="rblPriority" RepeatDirection="Vertical">
        <asp:ListItem Text="Express" Value="Express" > </asp:ListItem>
        <asp:ListItem Text="Economy" Value="Economy"></asp:ListItem>
    </asp:RadioButtonList>
        
    <label for="txtEmail">
        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:LocalString, Type %>"></asp:Label>*
    </label>
    <asp:RadioButtonList runat="server" ID="rblType" RepeatDirection="Vertical">
        <asp:ListItem Text="Road" Value="Road" > </asp:ListItem>
        <asp:ListItem Text="Air" Value="Air"></asp:ListItem>
    </asp:RadioButtonList>  
        
    <label for="txtEmail">
        <asp:Label ID="lblBulkShipping" runat="server" 
                Text="<%$ Resources:LocalString, IsBulkShipping %>"></asp:Label>*
    </label>
    <asp:RadioButtonList runat="server" ID="rblBulkShipping" RepeatDirection="Vertical">
        <asp:ListItem Text="Yes" Value="Y" > </asp:ListItem>
        <asp:ListItem Text="No" Value="N"></asp:ListItem>
    </asp:RadioButtonList>            
    
</fieldset>
<div id="buttons">
    <asp:Label ID="lblMandatoryCaption" runat="server" Text="*Mandatory Fields"></asp:Label>
    <asp:Button ID="btnNext" runat="server" 
                Text="<%$ Resources:LocalString, Next %>" onclick="btnNext_Click" 
                ValidationGroup="grpMasterServiceCreation" />
</div>

    
</asp:Content>
