<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFranchiseUpdateByAdmin.aspx.cs" Inherits="Kaizos.frmUserUpdateByAdmin" MasterPageFile="~/NewSite.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<fieldset id="Fieldset2" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminUser%>"></asp:Label>
    </legend>
    
    <label for="txtEmail">
        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminEmail%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEmail" runat="server" Enabled="False" MaxLength="60"></asp:TextBox>     
    
    <label for="txtLanguage">
        <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminLanguage%>"></asp:Label>     
     </label>
     <asp:TextBox ID="txtLanguage" runat="server" Enabled="False"></asp:TextBox>     
     <asp:Button ID="btnGet" runat="server" Text="<%$ Resources:LocalString, AllGet%>"
          onclick="btnGet_Click"  Visible = "false" />
</fieldset>

<fieldset id="Fieldset1" runat="server" class ="third">
    <legend>
        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminCompany%>"></asp:Label>
    </legend>
    
    <label for="txtCompanyName">
        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminCompanyName%>"></asp:Label>                            
    </label>
    <asp:TextBox ID="txtCompanyName" runat="server" Enabled="False" MaxLength="100"></asp:TextBox>  
    
    <label for="rtxtChalandiseZone">
        <asp:Label ID="lblChalandiseZone" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminChalandiseZone%>"></asp:Label>   
    </label>
    <asp:TextBox ID="rtxtChalandiseZone" runat="server" TextMode="MultiLine" 
         MaxLength="5000"></asp:TextBox> 
    
    <label for="fileUpload">
        <asp:Label ID="lblCreditRequest" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminCreditRequest%>"></asp:Label>     
    </label>
    <asp:FileUpload ID="fileUpload" runat="server" />
    <asp:Label id="lblMax" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminFileComment%>"></asp:Label><br/>      
    <asp:Label ID="lblCreditRequest1" runat="server" Text=""></asp:Label>
    
    <label for="optEnable">
        <asp:Label ID="lblAccountStatus" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminAccountStatus%>"></asp:Label> 
    </label>
    <div class="group">
    <asp:RadioButton ID="optEnable" runat="server" Text = "<%$ Resources:LocalString, FranchiseUpdateAdminEnabled%>" 
                        GroupName="OptFranchiseUpdate"/>
    <asp:RadioButton ID="optDisable" runat="server" Text = "<%$ Resources:LocalString, FranchiseUpdateAdminDisabled%>" 
                        GroupName="OptFranchiseUpdate"/>
    <asp:RadioButton ID="optArcheive" runat="server" Text = "<%$ Resources:LocalString, FranchiseUpdateAdminArchieve%>" 
                        GroupName="OptFranchiseUpdate"/>
                        </div>
</fieldset>

<fieldset id="Fieldset3" runat="server" class ="third">
    <legend>
        <asp:Label ID="lblMonthlyFees" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminMonthlyFees%>"></asp:Label>
    </legend>
    
    <label for="txtFrance">
        <asp:Label ID="lblFrance" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminFrance%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtFrance" runat="server"></asp:TextBox>
    
    <label for="txtInternational">
        <asp:Label ID="lblInternational" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminInternational%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtInternational" runat="server"></asp:TextBox>
    
    <label for="txtEurope">
        <asp:Label ID="lblEurope" runat="server" Text="<%$ Resources:LocalString, FranchiseUpdateAdminEurope%>"></asp:Label>
    </label>
    <asp:TextBox ID="txtEurope" runat="server"></asp:TextBox>
</fieldset>
 
<div id = "buttons">
    <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalString, AllSubmit%>" 
        onclick="btnSubmit_Click" ValidationGroup="grpFranchiseUpdateByAdmin" />
    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>" 
        onclick="btnCancel_Click" />
    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AllMandatoryField%>"></asp:Label>    
</div>

<div class="divSummaryFranchiseUpdateByAdmin">

    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLess100" Text="<%$ Resources:LocalString, valLess100 %>" runat ="server" Visible= "false"></asp:Label>


    <asp:CustomValidator ID="val_FranchiseUpdateByAdmin" runat="server" 
        ControlToValidate="txtEmail" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpFranchiseUpdateByAdmin" 
        CssClass="clsErrorMessage" onservervalidate="val_FranchiseUpdateByAdmin_ServerValidate">
    </asp:CustomValidator>

</div>

</asp:Content>