<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmManualShipping.aspx.cs" Inherits="Kaizos.frmManualShipping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="errorMsg" ID="errorMsg1" runat="server">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valSingle" Text="<%$ Resources:LocalString, ValidationSingle %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMulti" Text="<%$ Resources:LocalString, ValidationMultiple %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valGreater" Text="<%$ Resources:LocalString, ValGreaterThan %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_Insurance" runat="server" 
            ControlToValidate="txtDeclaredValue" 
            EnableClientScript="False" 
            ValidateEmptyText="True"
            ValidationGroup="grpInsurance"
        onservervalidate="val_Insurance_ServerValidate" ></asp:CustomValidator>

    <asp:CustomValidator ID="val_SenderAddress" runat="server" 
            ControlToValidate="txtSenderCompany" 
            EnableClientScript="False" 
            ValidateEmptyText="True"
            ValidationGroup="grpSenderAddress" onservervalidate="val_SenderAddress_ServerValidate" 
            ></asp:CustomValidator>

    <asp:CustomValidator ID="val_ReturnAddress" runat="server" 
        ControlToValidate="txtReturnCompany" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpReturnAddress" onservervalidate="val_ReturnAddress_ServerValidate" 
        ></asp:CustomValidator>

    <asp:CustomValidator ID="CustomValidator1" runat="server" 
        ControlToValidate="txtRecipientCompanyName" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpRecipientAddress1" onservervalidate="CustomValidator1_ServerValidate" 
        ></asp:CustomValidator>
</div>


<fieldset id ="fsInsurance" runat="server" class="second">
    <legend id="lgInsurance" >
        <asp:Label ID="lblLegendInsurance" runat="server" Text="<%$ Resources:LocalString, Insurance %>"></asp:Label>
    </legend>
    <div class="fieldSection">
    <asp:CheckBox ID="chkInsurance" runat="server" 
                Text="<%$ Resources:LocalString, InsureThisShipment %>" 
            AutoPostBack="true" oncheckedchanged="chkInsurance_CheckedChanged" cssclass="checkBloc" TextAlign="Left"/>
    <br />
    <tr id="trInsurance1" runat="server" visible="false">
        <td><label for="txtDeclaredValue"><asp:Label runat="server" ID="lblDeclaredValue" Text="Declared Value" ></asp:Label></label></td>
        <td><asp:TextBox ID="txtDeclaredValue" runat="server" CssClass="tiny"></asp:TextBox>
            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="tiny"> </asp:DropDownList></td>
       
        <td><label for="txtDeclaredValue"><asp:Label runat="server" ID="lblPackagingType" Text="Packaging Type" ></asp:Label></label></td>
        <td><div class="inlineBlock">
            <asp:UpdatePanel ID="upPackage" runat="server">
                <ContentTemplate>
                <asp:DropDownList ID="ddlPackagingType" runat="server"
                onselectedindexchanged="ddlPackagingType_SelectedIndexChanged" 
                AutoPostBack="True" ></asp:DropDownList>
                <asp:TextBox ID="txtPackagingType" runat="server" Visible="false" CssClass="tiny"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        </td>
    </tr>
    </div>
    <div class="fieldSection">
    <tr id="trInsurance2" runat="server" visible="false">
        <td><label for="ddlClosedUsed"><asp:Label runat="server" ID="lblCloseUsed" Text="Closing Used" ></asp:Label></label></td>
        <td><asp:DropDownList ID="ddlClosedUsed" runat="server" ></asp:DropDownList></td>
        <td><label for="ddlPackageMaterial"><asp:Label runat="server" ID="lblPackageMaterial" Text="Material used in Packaging" ></asp:Label></label></td>
        <td><asp:DropDownList ID="ddlPackageMaterial" runat="server"></asp:DropDownList></td>
    </tr><br />

    <tr id="trInsurance3" runat="server" visible="false">
    <td></td>
    <td></td>
    <td><asp:CheckBox ID="chkInsuranceTermsAccept" runat="server" Text="<%$ Resources:LocalString, AcceptInsurance %>" TextAlign="Left"/> 
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" 
            Text="<%$ Resources:LocalString, TermsOfUse %>" Target="_blank"></asp:HyperLink>   
    </td>

    </tr>
    </div>
</fieldset>

<asp:UpdatePanel ID="udpOutterUpdatePanel" runat="server"> 
             <ContentTemplate> 

<fieldset id="fsSender" runat="server" class="first manualShipAddress">
    <legend id="lgSender">
        <asp:Label ID="lblLegendSender" runat="server" Text="<%$ Resources:LocalString, Sender %>"></asp:Label>
    </legend>

    <asp:Button ID="btnPickAnAddress" runat="server" Text="<%$ Resources:LocalString, PickAnAddress %>" CssClass="pickAnAddress" />    
    <br />
    <label for="txtSenderCompany">
        <asp:Label ID="lblSCompanyName" runat="server" 
                Text="<%$ Resources:LocalString, CompanyName %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderCompany" runat="server"></asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblSName" runat="server" 
                Text="<%$ Resources:LocalString, Name %>" ></asp:Label>
    </label>
    <div class="group">
        <asp:DropDownList ID="ddlSenderCivility" runat="server" CssClass="tiny"> </asp:DropDownList><br />
        <asp:TextBox ID="txtSenderFirstName" runat="server" CssClass="tiny"></asp:TextBox>
        <asp:TextBox ID="txtSenderLastName" runat="server" CssClass="tiny"></asp:TextBox>
    </div>

    <label for="txtSenderCompany">
        <asp:Label ID="lblSPhoneNumber" runat="server" 
                Text="<%$ Resources:LocalString, PhoneNumber %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderPhone" runat="server"></asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblSEmail" runat="server" 
                Text="<%$ Resources:LocalString, EMail %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderEmail" runat="server"></asp:TextBox>
    
    <label for="txtSenderCompany" class="addressMore">
        <asp:Label ID="lblSAddress" runat="server" 
                Text="<%$ Resources:LocalString, Address %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderAddress1" runat="server" CssClass="addressMore"></asp:TextBox>

    <asp:TextBox ID="txtSenderAddress2" runat="server" CssClass="addressMore"></asp:TextBox>

    <asp:TextBox ID="txtSenderAddress3" runat="server" CssClass="addressMore"></asp:TextBox>
    

    <label for="txtSenderCompany">
        <asp:Label ID="lblSCity" runat="server" 
                Text="<%$ Resources:LocalString, City %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderCity" runat="server" Enabled="false"></asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblSZipCode" runat="server" 
                Text="<%$ Resources:LocalString, ZipCode %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderZipCode" runat="server" Enabled="false"></asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblSState" runat="server" 
                Text="<%$ Resources:LocalString, State %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderState" runat="server"></asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblCountry" runat="server" 
                Text="<%$ Resources:LocalString, Country %>" ></asp:Label>
    </label>
    <asp:DropDownList ID="ddlSenderCountry" runat="server" Enabled="false"> </asp:DropDownList>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblWishedShipDate" runat="server" 
                Text="<%$ Resources:LocalString, WishedShippingDate %>" ></asp:Label>
    </label>
    <div class="inlineBlock">
        <asp:UpdatePanel ID="upShpDateControl" runat="server"  >
            <ContentTemplate>
                <asp:TextBox ID="txtWishedShipDate" runat="server" CssClass="tiny"></asp:TextBox>
                <asp:CalendarExtender ID="calWishedDated"  Format="dd/MM/yyyy" runat="server" TargetControlID="txtWishedShipDate" CssClass="clsCalendar" >
                </asp:CalendarExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <label for="txtSenderCompany">
        <asp:Label ID="lblCollectionDeadline" runat="server" 
                Text="<%$ Resources:LocalString, CollectionDeadline %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtCollectionDeadLine" runat="server" ToolTip="<%$ Resources:LocalString, ExpectedTimeFormat %>" CssClass="tiny" >
                                </asp:TextBox>
    
    <label for="txtSenderCompany">
        <asp:Label ID="lblSCondition" runat="server" 
                Text="<%$ Resources:LocalString, Conditions %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtSenderCondition" runat="server" TextMode="MultiLine" CssClass="smallArea"></asp:TextBox>
    
    <label for="txtSenderCompany"><asp:Label ID="lblCarrier" runat="server" visible="false"
                Text="<%$ Resources:LocalString, Carrier %>" ></asp:Label></label>
    <asp:DropDownList ID="ddlCarrier" runat="server" Visible="false"> </asp:DropDownList>
    
    <asp:Button ID="btnSenderAddress" runat="server" 
                Text="<%$ Resources:LocalString, AddToAddressBook %>"  CssClass="pickAnAddress" 
                ValidationGroup="grpSenderAddress" onclick="btnSenderAddress_Click" />
</fieldset>
    
<input id="Button2" type="button" style="display: none" runat="server" /> 

    <asp:ModalPopupExtender
        CancelControlID="btnCancelModalPopup" 
        runat="server" 
        PopupControlID="Panel2" 
        id="ModalPopupSender" 
        TargetControlID="Button2" /> 

    <asp:Panel ID="Panel2" runat="server" CssClass="customerModelWindow" style="display:none;"> 
        <asp:Label  ID="lblSenderMessage" runat="server"></asp:Label><br />
        <asp:Button ID="Button3" runat="server" Text="OK" /> 
    </asp:Panel> 

    <asp:ModalPopupExtender runat="server" 
                        ID="mpeThePopup" 
                        TargetControlID="btnPickAnAddress" 
                        PopupControlID="pnlModalPopUpPanel" 
                        BackgroundCssClass="modalBackground"                        
                        DropShadow="false"/> 

    <asp:Panel ID="pnlModalPopUpPanel" runat="server" CssClass="customerModelWindow addressSearch"> 

                    <asp:UpdatePanel ID="udpInnerUpdatePanel" runat="Server" UpdateMode="Conditional"> 
                        <ContentTemplate> 
                            <p> 
                                <%--<asp:DropDownList ID="ddlProducts" runat="server"></asp:DropDownList> --%>   
                                <asp:TextBox ID="txt1" runat="server" ></asp:TextBox>                            
                                &nbsp; 
                                <asp:Button ID="btnChooseProduct" runat="server"  Text="<%$ Resources:LocalString, Search%>" onclick="btnChooseProduct_Click"/> 
                                &nbsp; 
                                <asp:Button ID="Button1" runat="server" Text="Cancel" onclick="Button1_Click"  CssClass="Cancel"/> 
                                <br /><asp:Label ID="lblSError" runat="server" CssClass="clsErrorMessage" Visible="false"></asp:Label>

                                <div class="customerModelWindowinner">
                                <asp:GridView ID="gvAddressBookList" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="True" CaptionAlign="Left" 
                                    onrowcommand="gvAddressBookList_RowCommand"
                                    CssClass="manualShip_addressTable"  >
                                   <Columns>
                                   <asp:TemplateField>
                                        <ItemTemplate>
                                          <asp:Label ID="lblAddressID" runat="server" Text='<%#Eval("AddressID").ToString() %>' Visible = "false"></asp:Label>
                                          <asp:ImageButton runat="server" ID="imgEdit" ImageUrl="~/Image/left_arrow.png" CommandName="MyEdit"/>             
                                          <asp:Label ID="Label1" runat="server" Text='<%#Eval("Comments").ToString() %>' Visible = "false"></asp:Label>
                                         </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:BoundField DataField="AddressType" HeaderText="<%$ Resources:LocalString, AddressBookListAddressType %>"   />
                                   <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, AddressBookListCompanyName %>"   />
                                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, AddressBookListContactName %>"   />
                                   <asp:BoundField DataField="TelephoneNo" HeaderText="<%$ Resources:LocalString, AddressBookListTelNo %>"   />
                                   <asp:BoundField DataField="FaxNo" HeaderText="<%$ Resources:LocalString, AddressBookListFaxNo %>"   />
                                   <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:LocalString, AddressBookListAddress1 %>" />

                                   <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:LocalString, AddressBookListAddress2 %>"   />
                                   <asp:BoundField DataField="Address3" HeaderText="<%$ Resources:LocalString, AddressBookListAddress3 %>"   />
                                   <asp:BoundField DataField="Zipcode" HeaderText="<%$ Resources:LocalString, AddressBookListZipcode %>"   />
                                   <asp:BoundField DataField="City" HeaderText="<%$ Resources:LocalString, AddressBookListCity %>"   />
                                   <asp:BoundField DataField="State" HeaderText="<%$ Resources:LocalString, AddressBookListState %>" />
                    
                                   <asp:BoundField DataField="Country" HeaderText="<%$ Resources:LocalString, AddressBookListCountry %>"   />
                                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, AddressBookListEmail %>"   />
                                   <asp:BoundField DataField="LastPickupMondayToThursday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpMonThur %>"   />
                                   <asp:BoundField DataField="LastPickupFriday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpFriday %>"   />
                                   <asp:BoundField Visible="false" DataField="Comments" HeaderText="<%$ Resources:LocalString, AddressBookListComments %>" />
                                   <asp:BoundField Visible="false" DataField="ShipPreference" HeaderText="<%$ Resources:LocalString, AddressBookListShipPreference %>"   />
                                   
                                   </Columns>
                            </asp:GridView>
                            </div>
                        </ContentTemplate>       
                        <Triggers> 
                            <asp:AsyncPostBackTrigger ControlID="btnChooseProduct" EventName="Click" /> 
                        </Triggers>

                    </asp:UpdatePanel> 
                 </asp:Panel> 


  </ContentTemplate> 
        </asp:UpdatePanel> 

<asp:UpdatePanel ID="UpdatePanel3" runat="server"> 
             <ContentTemplate> 
<fieldset id="Fieldset2" runat="server" class="first manualShipAddress">
    <legend id="Legend2">
        <asp:Label ID="lblLegendRecipient" runat="server" Text="<%$ Resources:LocalString, Recipient %>"></asp:Label>
    </legend>
    
    <asp:Button ID="btnRecipientPickAddress" runat="server" Text="<%$ Resources:LocalString, PickAnAddress %>" CssClass="pickAnAddress" />
    <br />
    <label for="txtReturnCompany">
        <asp:Label ID="lblReCompany" runat="server" 
                Text="<%$ Resources:LocalString, CompanyName %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientCompanyName" runat="server"></asp:TextBox>

    <label for="txtReturnCompany">
        <asp:Label ID="lblReName" runat="server" 
                Text="<%$ Resources:LocalString, Name %>" ></asp:Label>
    </label>
    <div class="group">
        <asp:DropDownList ID="ddlRecipientCivility" runat="server" CssClass="tiny"> </asp:DropDownList><br />
        <asp:TextBox ID="txtRecipientFirstName" runat="server" CssClass="tiny"></asp:TextBox>
        <asp:TextBox ID="txtRecipientLastName" runat="server" CssClass="tiny"></asp:TextBox>
    </div>

    <label for="txtReturnCompany">
        <asp:Label ID="lblRePhone" runat="server" 
                Text="<%$ Resources:LocalString, PhoneNumber %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientPhone" runat="server"></asp:TextBox>
        
    <label for="txtReturnCompany">
        <asp:Label ID="lblReEmail" runat="server" 
                Text="<%$ Resources:LocalString, EMail %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientEmail" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany" class="addressMore">
        <asp:Label ID="lblReAddress" runat="server" 
                Text="<%$ Resources:LocalString, Address %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientAddress1" runat="server" CssClass="addressMore"></asp:TextBox><br />
    
    <asp:TextBox ID="txtRecipientAddress2" runat="server" CssClass="addressMore"></asp:TextBox><br />
    
    <asp:TextBox ID="txtRecipientAddress3" runat="server" CssClass="addressMore"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReCity" runat="server" 
                Text="<%$ Resources:LocalString, City %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientCity" runat="server" Enabled="false"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReZip" runat="server" 
                Text="<%$ Resources:LocalString, ZipCode %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientZipCode" runat="server" Enabled="false"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReState" runat="server" 
                Text="<%$ Resources:LocalString, State %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientState" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReCountry" runat="server" 
                Text="<%$ Resources:LocalString, Country %>" ></asp:Label>
    </label>
    <asp:DropDownList ID="ddlRecipientCountry" runat="server" Enabled="false"> </asp:DropDownList>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReDelivery" runat="server" 
                Text="<%$ Resources:LocalString, DeliveryDeadLine %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientDeliveryDeadLine" runat="server" ToolTip="<%$ Resources:LocalString, ExpectedTimeFormat %>" CssClass="tiny" ></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblReConditions" runat="server" 
                Text="<%$ Resources:LocalString, Conditions %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtRecipientConditions" runat="server" TextMode="MultiLine" CssClass="smallArea" ></asp:TextBox>
    
    <asp:Button ID="btnRecipientAddToAddress" runat="server" Text="<%$ Resources:LocalString, AddToAddressBook %>"
                ValidationGroup="grpRecipientAddress1"  CssClass="pickAnAddress" 
                onclick="btnRecipientAddToAddress_Click"/>
</fieldset>
<input id="Button6" type="button" style="display: none" runat="server" /> 

    <asp:ModalPopupExtender
        CancelControlID="btnCancelModalPopup" 
        runat="server" 
        PopupControlID="Panel4" 
        id="ModalPopupReceipent" 
        TargetControlID="Button6"
        BackgroundCssClass="modalBackground" /> 

    <asp:Panel ID="Panel4" runat="server" CssClass="customerModelWindow" style="display:none;"> 
        <asp:Label  ID="lblReceipentMessage" runat="server"></asp:Label><br />
        <asp:Button ID="Button8" runat="server" Text="OK" /> 
    </asp:Panel> 

    <asp:ModalPopupExtender runat="server" 
                        ID="ModalPopupExtender2" 
                        TargetControlID="btnRecipientPickAddress" 
                        PopupControlID="Panel5"                     
                        DropShadow="false"/> 

    <asp:Panel ID="Panel5" runat="server" CssClass="customerModelWindow addressSearch"> 

                    <asp:UpdatePanel ID="UpdatePanel4" runat="Server" UpdateMode="Conditional"> 
                        <ContentTemplate> 
                            <p> 
                                <%--<asp:DropDownList ID="ddlProducts" runat="server"></asp:DropDownList> --%>   
                                <asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>                            
                                &nbsp; 
                                <asp:Button ID="btnReAddress" runat="server" 
                                    Text="<%$ Resources:LocalString, Search%>" onclick="btnReAddress_Click"  /> 
                                &nbsp; 
                                <asp:Button ID="Button10" runat="server" Text="Cancel" onclick="Button1_Click" CssClass="Cancel" /> 
                                <br /> <asp:Label ID="lblReError" runat="server" CssClass="clsErrorMessage" Visible="false"></asp:Label>
                                
                                <div class="customerModelWindowinner">
                                <asp:GridView ID="gvAddressBookList_RE" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="True" CaptionAlign="Left" 
                                    onrowcommand="gvAddressBookList_RE_RowCommand" 
                                    CssClass="manualShip_addressTable" >
                                   <Columns>
                                   <asp:TemplateField>
                                        <ItemTemplate>
                                          <asp:Label ID="lblAddressID" runat="server" Text='<%#Eval("AddressID").ToString() %>' Visible = "false"></asp:Label>
                                          <asp:ImageButton runat="server" ID="imgEdit" ImageUrl="~/Image/left_arrow.png" CommandName="MyEdit"/>             
                                          <asp:Label ID="Label1" runat="server" Text='<%#Eval("Comments").ToString() %>' Visible = "false"></asp:Label>
                                         </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:BoundField DataField="AddressType" HeaderText="<%$ Resources:LocalString, AddressBookListAddressType %>"   />
                                   <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, AddressBookListCompanyName %>"   />
                                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, AddressBookListContactName %>"   />
                                   <asp:BoundField DataField="TelephoneNo" HeaderText="<%$ Resources:LocalString, AddressBookListTelNo %>"   />
                                   <asp:BoundField DataField="FaxNo" HeaderText="<%$ Resources:LocalString, AddressBookListFaxNo %>"   />
                                   <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:LocalString, AddressBookListAddress1 %>" />
                                   <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:LocalString, AddressBookListAddress2 %>"   />
                                   <asp:BoundField DataField="Address3" HeaderText="<%$ Resources:LocalString, AddressBookListAddress3 %>"   />
                                   <asp:BoundField DataField="Zipcode" HeaderText="<%$ Resources:LocalString, AddressBookListZipcode %>"   />
                                   <asp:BoundField DataField="City" HeaderText="<%$ Resources:LocalString, AddressBookListCity %>"   />
                                   <asp:BoundField DataField="State" HeaderText="<%$ Resources:LocalString, AddressBookListState %>" />                    
                                   <asp:BoundField DataField="Country" HeaderText="<%$ Resources:LocalString, AddressBookListCountry %>"   />
                                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, AddressBookListEmail %>"   />
                                   <asp:BoundField DataField="LastPickupMondayToThursday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpMonThur %>"   />
                                   <asp:BoundField DataField="LastPickupFriday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpFriday %>"   />
                                   <asp:BoundField Visible="false" DataField="Comments" HeaderText="<%$ Resources:LocalString, AddressBookListComments %>" />
                                   <asp:BoundField Visible="false" DataField="ShipPreference" HeaderText="<%$ Resources:LocalString, AddressBookListShipPreference %>"   />
                                   
                                   </Columns>
                            </asp:GridView>
                            </div>
                        </ContentTemplate>       
                        <Triggers> 
                            <asp:AsyncPostBackTrigger ControlID="btnRtAddress" EventName="Click" /> 
                        </Triggers>

                    </asp:UpdatePanel> 
                 </asp:Panel> 

   </ContentTemplate>
   </asp:UpdatePanel>


<asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
<ContentTemplate> 

<fieldset id="Fieldset1" runat="server" class="first manualShipAddress">
    <legend id="Legend1">
        <asp:Label ID="lblLegendReturn" runat="server" Text="<%$ Resources:LocalString, Return %>"></asp:Label>
    </legend>

    <asp:Button ID="btnRtPicKAnAddress" runat="server" Text="<%$ Resources:LocalString, PickAnAddress %>" CssClass="pickAnAddress"/>
    <br />
    <asp:CheckBox ID="chkUseSender" runat="server" 
                Text="<%$ Resources:LocalString, UseSenderAddress %>" AutoPostBack="True" 
                oncheckedchanged="chkUseSender_CheckedChanged" CssClass="smallArea" TextAlign="Left" ></asp:CheckBox>
        
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtCompany" runat="server" 
                Text="<%$ Resources:LocalString, CompanyName %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnCompany" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtName" runat="server" 
                Text="<%$ Resources:LocalString, Name %>" ></asp:Label>
    </label>
    <div class="group">
        <asp:DropDownList ID="ddlReturnCivility" runat="server" CssClass="tiny" > </asp:DropDownList><br />
        <asp:TextBox ID="txtReturnFirstName" runat="server" CssClass="tiny"></asp:TextBox>
        <asp:TextBox ID="txtReturnLastName" runat="server" CssClass="tiny"></asp:TextBox>
    </div>

    <label for="txtReturnCompany">
        <asp:Label ID="lblRtPhone" runat="server" 
                Text="<%$ Resources:LocalString, PhoneNumber %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnPhone" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtEmail" runat="server" 
                Text="<%$ Resources:LocalString, EMail %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnEmail" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany" class="addressMore">
        <asp:Label ID="lblRtAddress" runat="server" 
                Text="<%$ Resources:LocalString, Address %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnAddress1" runat="server" CssClass="addressMore"></asp:TextBox>

    <asp:TextBox ID="txtReturnAddress2" runat="server" CssClass="addressMore"></asp:TextBox>

    <asp:TextBox ID="txtReturnAddress3" runat="server" CssClass="addressMore"></asp:TextBox>
   

    <label for="txtReturnCompany">
        <asp:Label ID="lblRtCity" runat="server" 
                Text="<%$ Resources:LocalString, City %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnCity" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtZipCode" runat="server" 
                Text="<%$ Resources:LocalString, ZipCode %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnZipCode" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtState" runat="server" 
                Text="<%$ Resources:LocalString, State %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnState" runat="server"></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblRtCountry" runat="server" 
                Text="<%$ Resources:LocalString, Country %>" ></asp:Label>
    </label>
    <asp:DropDownList ID="ddlReturnCountry" runat="server"> </asp:DropDownList>
    
    <label for="txtReturnCompany">
        <asp:Label ID="lblDeliveryDeadLine" runat="server" 
                Text="<%$ Resources:LocalString, DeliveryDeadLine %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnDeliveryDeadline" runat="server" ToolTip="<%$ Resources:LocalString, ExpectedTimeFormat %>" CssClass="tiny" ></asp:TextBox>
    
    <label for="txtReturnCompany">
        <asp:Label ID="Label13" runat="server" 
                Text="<%$ Resources:LocalString, Conditions %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtReturnCondition" runat="server" TextMode="MultiLine" CssClass="smallArea" ></asp:TextBox>
    
    <asp:Button ID="btnRtAddAddress" runat="server" Text="<%$ Resources:LocalString, AddToAddressBook %>" 
                ValidationGroup= "grpReturnAddress" CssClass="pickAnAddress" 
                onclick="btnRtAddAddress_Click" />
</fieldset>

    
<input id="Button4" type="button" style="display: none" runat="server" /> 

    <asp:ModalPopupExtender
        CancelControlID="Button5" 
        runat="server" 
        PopupControlID="Panel3" 
        id="ModalPopupReturn" 
        TargetControlID="Button4"
        BackgroundCssClass="modalBackground" /> 

    <asp:Panel ID="Panel3" runat="server" CssClass="customerModelWindow" style="display:none;"> 
        <asp:Label  ID="lblRetrunMessage" runat="server"></asp:Label><br />
        <asp:Button ID="Button5" runat="server" Text="OK" /> 
    </asp:Panel> 

    <asp:ModalPopupExtender runat="server" 
                        ID="ModalPopupExtender3" 
                        TargetControlID="btnRtPicKAnAddress" 
                        PopupControlID="pnlModalPopUpPanel1" 
                        BackgroundCssClass="modalBackground"                        
                        DropShadow="false"/> 

    <asp:Panel ID="pnlModalPopUpPanel1" runat="server" CssClass="customerModelWindow addressSearch"> 

                    <asp:UpdatePanel ID="UpdatePanel2" runat="Server" UpdateMode="Conditional"> 
                        <ContentTemplate> 
                            <p> 
                                <%--<asp:DropDownList ID="ddlProducts" runat="server"></asp:DropDownList> --%>   
                                <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>                            
                                &nbsp; 
                                <asp:Button ID="btnRtAddress" runat="server" 
                                    Text="<%$ Resources:LocalString, Search%>" onclick="btnRtAddress_Click"  /> 
                                &nbsp; 
                                <asp:Button ID="Button7" runat="server" Text="Cancel" onclick="Button1_Click" CssClass="Cancel" /> 
                                <br /> <asp:Label ID="lblRtError" runat="server" CssClass="clsErrorMessage" Visible="false"></asp:Label>
                                
                                <div class="customerModelWindowinner">
                                <asp:GridView ID="gvAddressBookList_RT" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="True" CaptionAlign="Left" 
                                    onrowcommand="gvAddressBookList_RT_RowCommand"
                                    CssClass="manualShip_addressTable" >
                                   <Columns>
                                   <asp:TemplateField>
                                        <ItemTemplate>
                                          <asp:Label ID="lblAddressID" runat="server" Text='<%#Eval("AddressID").ToString() %>' Visible = "false"></asp:Label>
                                          <asp:ImageButton runat="server" ID="imgEdit" ImageUrl="~/Image/left_arrow.png" CommandName="MyEdit"/>             
                                          <asp:Label ID="Label1" runat="server" Text='<%#Eval("Comments").ToString() %>' Visible = "false"></asp:Label>
                                         </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:BoundField DataField="AddressType" HeaderText="<%$ Resources:LocalString, AddressBookListAddressType %>"   />
                                   <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:LocalString, AddressBookListCompanyName %>"   />
                                   <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalString, AddressBookListContactName %>"   />
                                   <asp:BoundField DataField="TelephoneNo" HeaderText="<%$ Resources:LocalString, AddressBookListTelNo %>"   />
                                   <asp:BoundField DataField="FaxNo" HeaderText="<%$ Resources:LocalString, AddressBookListFaxNo %>"   />
                                   <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:LocalString, AddressBookListAddress1 %>" />

                                   <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:LocalString, AddressBookListAddress2 %>"   />
                                   <asp:BoundField DataField="Address3" HeaderText="<%$ Resources:LocalString, AddressBookListAddress3 %>"   />
                                   <asp:BoundField DataField="Zipcode" HeaderText="<%$ Resources:LocalString, AddressBookListZipcode %>"   />
                                   <asp:BoundField DataField="City" HeaderText="<%$ Resources:LocalString, AddressBookListCity %>"   />
                                   <asp:BoundField DataField="State" HeaderText="<%$ Resources:LocalString, AddressBookListState %>" />
                    
                                   <asp:BoundField DataField="Country" HeaderText="<%$ Resources:LocalString, AddressBookListCountry %>"   />
                                   <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalString, AddressBookListEmail %>"   />
                                   <asp:BoundField DataField="LastPickupMondayToThursday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpMonThur %>"   />
                                   <asp:BoundField DataField="LastPickupFriday" HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpFriday %>"   />
                                   <asp:BoundField Visible="false" DataField="Comments" HeaderText="<%$ Resources:LocalString, AddressBookListComments %>" />
                                   <asp:BoundField Visible="false" DataField="ShipPreference" HeaderText="<%$ Resources:LocalString, AddressBookListShipPreference %>"   />
                                   
                                   </Columns>
                            </asp:GridView>
                            </div>
                        </ContentTemplate>       
                        <Triggers> 
                            <asp:AsyncPostBackTrigger ControlID="btnRtAddress" EventName="Click" /> 
                        </Triggers>

                    </asp:UpdatePanel> 
                 </asp:Panel> 

   </ContentTemplate>
   </asp:UpdatePanel>


<fieldset id="Fieldset3" runat="server" class="first">
    <legend id="Legend3">
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, Incoterm %>"></asp:Label>
    </legend>
    <label><asp:Label ID="Label4" runat="server" 
                Text="<%$ Resources:LocalString, IncotermDetails %>" ></asp:Label></label>
</fieldset>

<fieldset id="Fieldset4" runat="server" class="first">
    <legend id="Legend4">
        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalString, AdditionalDetail %>"></asp:Label>
    </legend>
    <label for="txtCustomerInternalRef">
        <asp:Label ID="lblCustInternalRef" runat="server" 
                Text="<%$ Resources:LocalString, CustomerInternalRef %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtCustomerInternalRef" runat="server"></asp:TextBox>
    
    <label for="txtCustomerInternalRef"><asp:Label ID="lblCustomsValue" runat="server" 
                Text="<%$ Resources:LocalString, CustomsValue  %>" Visible="false"></asp:Label></label>
    <asp:TextBox ID="txtCustomsValue" runat="server" Visible="false"></asp:TextBox>

    <asp:CheckBox ID="chkSenderNotification" runat="server" 
                    Text="<%$ Resources:LocalString, SenderNotification %>" CssClass="checkBloc" TextAlign="Left"></asp:CheckBox>
    <br />
    <asp:CheckBox ID="chkRecipientNotification" runat="server" 
                Text="<%$ Resources:LocalString, RecipientNotification %>" CssClass="checkBloc" TextAlign="Left"></asp:CheckBox>
    <br />
    <asp:CheckBox ID="chkAcceptShipingCodition" runat="server" 
                Text="<%$ Resources:LocalString, AcceptShipingCondition %>" CssClass="checkBloc" TextAlign="Left"></asp:CheckBox>
    
</fieldset>

<asp:Button ID="btnSubmit" runat="server" 
     Text="<%$ Resources:LocalString, ManualSubmit %>" onclick="btnSubmit_Click" ValidationGroup="grpInsurance"/>
<asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalString, Back %>" 
                CssClass="Back" onclick="btnBack_Click"  />
<input id="dummy" type="button" style="display: none" runat="server" /> 
    <asp:ModalPopupExtender
        CancelControlID="btnCancelModalPopup" 
        runat="server" 
        PopupControlID="Panel1" 
        id="ModalPopupExtender1" 
        TargetControlID="dummy"
        BackgroundCssClass="modalBackground" /> 

    <asp:Panel ID="Panel1" runat="server" CssClass="customerModelWindow" style="display:none;"> 
        <asp:Label  ID="lblMessage" runat="server"></asp:Label>
        
        <asp:Button ID="btnCancelModalPopup" runat="server" Text="OK" /> 
    </asp:Panel> 

</asp:Content>
