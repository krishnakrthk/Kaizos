<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmFuelSurchargeManagement.aspx.cs" Inherits="Kaizos.frmFuelSurchargeManagement" %>
<%@ Import Namespace ="System.Data" %>
<%@ Import Namespace ="KaizosServiceInvokers.KaizosServiceReference" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"> 
</asp:ScriptManager>

<div class="errorMsg" id="errorMsg1" runat="server">
	<asp:CustomValidator ID="val_FuelSurcharge" runat="server"
			ControlToValidate   ="txtKeyActRef"
			EnableClientScript  ="False"
			ValidateEmptyText   ="True"
			ValidationGroup     ="grpFuelTariff" 
			onservervalidate="val_FuelSurcharge_ServerValidate" >
	</asp:CustomValidator>
</div>
<div class="errorMsg" id="errorMsg2" runat="server">
	<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valEmpty"  Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="lblSearchError" runat="server" Text=""></asp:Label>
</div>
<div class="errorMsg" id="errorMsg3" runat="server">
	<asp:CustomValidator ID="val_FuelParam" runat="server"
		ControlToValidate   ="txtKeyActRef"
		EnableClientScript  ="False"
		ValidateEmptyText   ="True"
		ValidationGroup     ="grpFuelParam" 
		onservervalidate    ="val_FuelParam_ServerValidate" >
	</asp:CustomValidator>

	<asp:Label ID="valParamEmpty" Text="<%$ Resources:LocalString, ParamValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valParamNumber" Text="<%$ Resources:LocalString, ParamValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
</div>
<div class="errorMsg" id="errorMsg4" runat="server">
	<asp:Label id="lblError" runat="server" Visible="false" />
</div>
<fieldset class="third camouflageTable">
    <legend>
        <asp:Label ID="lblFuelCharge" Text="<%$ Resources:LocalString, FuelSurcharge %>" runat="server" CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <label for="rblTariffType">
        <asp:Label ID="lblTariffType" runat="server"  Text ="Tariff Type"></asp:Label>
    </label>
    <asp:RadioButtonList runat="server" ID="rblTariffType" 
        RepeatDirection="Vertical" AutoPostBack="True"  
        onselectedindexchanged="rblTariffType_SelectedIndexChanged">
        <asp:ListItem Text="<%$ Resources:LocalString, FSCarrierPublic %>" Value="CarrierPublic" > </asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, FSKaizosPurchase %>" Value="KaizosPurchase"></asp:ListItem>
        <asp:ListItem Text="<%$ Resources:LocalString, FSKeyCustomerNegotiated %>" Value="KeyCustomer"></asp:ListItem>
    </asp:RadioButtonList>
   
    <label for="txtKeyActRef"><asp:Label ID="lblKeyActRef" runat="server" Text="Key Account Reference"></asp:Label></label>
    <asp:TextBox ID="txtKeyActRef" runat="server" Text="" ></asp:TextBox>
    
</fieldset>

<div id="buttons">
    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalString, FSSearch %>" onclick="btnSearch_Click" ValidationGroup="grpFuelTariff" /> 
</div>

<asp:UpdatePanel id="updatepanel2" runat="server" class="left">
        <ContentTemplate>
                    
            <asp:GridView ID="gvParameter" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="ServiceID"        HeaderStyle-CssClass="gridHeader" 
                        RowStyle-CssClass="gridRow"     
                AlternatingRowStyle-CssClass="gridAlternate" Visible="false" 
                onrowcreated="gvParameter_RowCreated" >
        <Columns>
            <asp:BoundField DataField="ServiceID" HeaderText="ServiceID"  > </asp:BoundField>
            <asp:BoundField DataField="ParameterDescription" HeaderText="<%$ Resources:LocalString, ParameterDescription %>"  SortExpression="ParameterDescription"> </asp:BoundField>
                <asp:TemplateField HeaderText="Value">
                <ItemStyle Width="40px" />
                <ItemTemplate>
                    <asp:TextBox runat="server" id="txtParamValue" Text='<%#Eval("ParameterValue").ToString() %>'  ></asp:TextBox>   
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:Button runat="server" id="btnUpdate" Text="Update"  ValidationGroup="grpFuelParam"
            onclick="btnUpdate_Click" Visible="false"></asp:Button> 
    <asp:Button runat="server" id="btnCancel" Text="Cancel"  Visible="false" 
        onclick="btnCancel_Click"></asp:Button>

</ContentTemplate>
</asp:UpdatePanel>

<asp:GridView ID="gvFuel" runat="server"    AutoGenerateColumns="False" 
                DataKeyNames="ServiceID"    HeaderStyle-CssClass="gridHeader" 
                RowStyle-CssClass="gridRow" AlternatingRowStyle-CssClass="gridAlternate" 
                onrowcancelingedit="gvFuel_RowCancelingEdit" 
                onrowediting="gvFuel_RowEditing" onrowcommand="gvFuel_RowCommand" 
                onrowupdating="gvFuel_RowUpdating" >
    <Columns>

        <asp:BoundField DataField="ServiceID" Visible="false" />

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, ReferenceCaption %>" >
        <ItemTemplate>
        <asp:Label ID="lblRefName" runat="server" Text='<%#Eval("Reference").ToString() %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="<%$ Resources:LocalString, CarrierName %>" >
        <ItemTemplate>
            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName").ToString() %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterServiceName %>" >
        <ItemTemplate>
            <asp:Label ID="lblMasterServiceName" runat="server" Text='<%#Eval("MasterServiceName").ToString() %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>

            
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, LastUpdate %>" >
        <ItemTemplate>
            <asp:Label ID="lblLastUpdate" runat="server" Text='<%# DateFormat(Eval("LastUpdate").ToString()) %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="<%$ Resources:LocalString, StartDate %>" >
        <ItemTemplate>
            <asp:Label ID="lblStartDate" runat="server" Text='<%# DateFormat(Eval("StartDate").ToString()) %>'></asp:Label>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit" Text="Edit"></asp:LinkButton>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtStartDate" runat="server" Text='<%# DateFormat(Eval("StartDate").ToString()) %>'></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="dd/MM/yyyy" CssClass="clsCalendar"></asp:CalendarExtender>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="true" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton id="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
        </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, EditParameter %>">
                <ItemStyle Width="40px" />
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="ibtEdit" Text="<%$ Resources:LocalString, FSParameter %>" AlternateText="Edit" ToolTip="<%$ Resources:LocalString, EditParameter %>"
                        CommandName="parameter" CommandArgument='<%#Eval("ServiceID").ToString() %>' />
                </ItemTemplate>
            </asp:TemplateField>

    </Columns>
</asp:GridView>

</ContentTemplate>
            
</asp:UpdatePanel>
            

<tr runat="server" id="row_updatebutton">
    <td class="clsLabelCentre">
                        
    </td>
                        
</tr>
   
</asp:Content>


