<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmTariffDelayInterrogation.aspx.cs" Inherits="Kaizos.frmTariffDelayInterrogation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="errorMsg simulTool" id="errorMsg1" runat="server">
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valLess" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valMaxAllowed" Text="<%$ Resources:LocalString, MaxAllowedShipment %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valPossitive" Text="<%$ Resources:LocalString, ValPositive %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valGreater" Text="<%$ Resources:LocalString, ValGreaterThan %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valBetween" Text="<%$ Resources:LocalString, ValBetween %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valWeekend" Text="<%$ Resources:LocalString, ValWeekend %>" runat ="server" Visible= "false"></asp:Label>
    <asp:CustomValidator ID="val_Shipment" runat="server" 
        ControlToValidate="txtContentType" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpShipment" 
        onservervalidate="val_Shipment_ServerValidate">
    </asp:CustomValidator>
</div>

<div class="errorMsg simulTool" id="errorMsg2" runat="server">
    <asp:Label ID="lblGridValudation" Text="" runat ="server" Visible= "false" ></asp:Label>
</div>   

<fieldset id ="Customer" runat="server" class="first sender"  >
    <legend id="lgSender" >
        <asp:Label ID="lblLegendSender" runat="server" CssClass="clsLabelLeft"
            Text="<%$ Resources:LocalString, Sender %>"></asp:Label>
    </legend>
    <label for="ddlCountry">
        <asp:Label ID="lblSenderCountry" runat="server"  CssClass="clsLabelLeft"
        Text="<%$ Resources:LocalString, Country %>"></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" >
        </asp:DropDownList><br />

    <label for="ddlCarrier">
        <asp:Label ID="lblZipCodeCity" runat="server"  CssClass="clsLabelLeft"
        Text="<%$ Resources:LocalString, ZipCodeCity %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtCity" runat="server" 
        ToolTip="<%$ Resources:LocalString, City %>" AutoPostBack="True" 
        ontextchanged="txtCity_TextChanged" CssClass="tiny"></asp:TextBox>
    <asp:TextBox ID="txtZip" runat="server"  
        ToolTip="<%$ Resources:LocalString, ZipCode %>" 
        AutoPostBack="True" ontextchanged="txtZip_TextChanged" CssClass="tiny"></asp:TextBox>

        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
            TargetControlID="txtCity" UseContextKey="True"
            ServiceMethod="GetCompletionListCity" 
            MinimumPrefixLength="1"  
            FirstRowSelected="true"
            EnableCaching="false"       
            CompletionSetCount="10"
            CompletionInterval="1000"
            ShowOnlyCurrentWordInCompletionListItem="true"
            CompletionListCssClass="AutoExtender"
            CompletionListItemCssClass="AutoExtenderList"
            CompletionListHighlightedItemCssClass ="AutoExtenderHighlight"
            CompletionListElementID="divwidth">
            <Animations>
            <OnShow>
                <Sequence>
                    <OpacityAction Opacity="0" />
                    <HideAction Visible="true" />
                    <Parallel Duration=".4">
                        <FadeIn />
                    </Parallel>
                </Sequence>
            </OnShow>
            <OnHide>
                <Parallel Duration=".4">
                    <FadeOut />
                </Parallel>
            </OnHide>
        </Animations>
    </asp:AutoCompleteExtender>
    
    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
        TargetControlID="txtZip" UseContextKey="True"
        ServiceMethod="GetCompletionListZip" 
        MinimumPrefixLength="1"  
        FirstRowSelected="true"
        EnableCaching="false"       
        CompletionSetCount="10"
        CompletionInterval="1000"
        ShowOnlyCurrentWordInCompletionListItem="true"
        CompletionListCssClass="AutoExtender"
        CompletionListItemCssClass="AutoExtenderList"
        CompletionListHighlightedItemCssClass ="AutoExtenderHighlight"
        CompletionListElementID="divwidth1">
      <Animations>
        <OnShow>
            <Sequence>
                <OpacityAction Opacity="0" />
                <HideAction Visible="true" />
                <Parallel Duration=".4">
                    <FadeIn />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <Parallel Duration=".4">
                <FadeOut />
            </Parallel>
        </OnHide>
    </Animations>
    </asp:AutoCompleteExtender>
    
</fieldset>

<fieldset id ="Fieldset1" runat="server" class="first sender"  >
    <legend class="clsLegend" id="Legend1" >
        <asp:Label ID="lgRecipient" runat="server" CssClass="clsLabelLeft"
            Text="<%$ Resources:LocalString, Recipient %>"></asp:Label>
    </legend>    
    <label for="ddlRCountry">
        <asp:Label ID="lblRCountry" runat="server"  
        Text="<%$ Resources:LocalString, Country %>"></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlRCountry" runat="server" AutoPostBack="True"></asp:DropDownList>
    <br />
    <label for="txtRZipCode">
        <asp:Label ID="lblRZipCode" runat="server"  
        Text="<%$ Resources:LocalString, ZipCodeCity %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtRCity" runat="server" ToolTip="<%$ Resources:LocalString, City %>"
            AutoPostBack="True" OnTextChanged="txtRCity_TextChanged" CssClass="tiny"></asp:TextBox>
        <asp:TextBox ID="txtRZipCode" runat="server" ToolTip="<%$ Resources:LocalString, ZipCode %>"
            AutoPostBack="True" OnTextChanged="txtRZipCode_TextChanged" CssClass="tiny"></asp:TextBox>
        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtRCity"
            UseContextKey="True" ServiceMethod="GetCompletionListRCity" MinimumPrefixLength="1"
            FirstRowSelected="true" EnableCaching="false" CompletionSetCount="10" CompletionInterval="1000"
            ShowOnlyCurrentWordInCompletionListItem="true" CompletionListCssClass="AutoExtender"
            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
            CompletionListElementID="divwidth1">
            <Animations><OnShow>
        <Sequence>
            <OpacityAction Opacity="0" />
            <HideAction Visible="true" />
            <Parallel Duration=".4">
                <FadeIn />
            </Parallel>
        </Sequence>
    </OnShow>
    <OnHide>
        <Parallel Duration=".4">
            <FadeOut />
        </Parallel>
    </OnHide>
            </Animations>
        </asp:AutoCompleteExtender>
        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="txtRZipCode"
            UseContextKey="True" ServiceMethod="GetCompletionListRZip" MinimumPrefixLength="1"
            FirstRowSelected="true" EnableCaching="false" CompletionSetCount="10" CompletionInterval="1000"
            ShowOnlyCurrentWordInCompletionListItem="true" CompletionListCssClass="AutoExtender"
            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
            CompletionListElementID="divwidth1">
            <Animations>
    <OnShow>
        <Sequence>
            <OpacityAction Opacity="0" />
            <HideAction Visible="true" />
            <Parallel Duration=".4">
                <FadeIn />
            </Parallel>
        </Sequence>
    </OnShow>
    <OnHide>
        <Parallel Duration=".4">
            <FadeOut />
        </Parallel>
    </OnHide>
            </Animations>
        </asp:AutoCompleteExtender>
    <br />
    <label for="rdAddressType1">
        <asp:Label ID="lblAddressType" runat="server"  
        Text="<%$ Resources:LocalString, AddressType %>"></asp:Label>*
    </label>
    <div class="group">
        <asp:RadioButton ID="rdAddressType1" GroupName="rdAddressType" Checked="true" Text="<%$ Resources:LocalString, Commercial %>" runat="server" />
        <br />
        <asp:RadioButton ID="rdAddressType2" GroupName="rdAddressType" Text="<%$ Resources:LocalString, Residential %>" runat="server" />
    </div>
</fieldset>

<fieldset  id ="Fieldset3" runat="server" class="second"  >
    <legend class="clsLegend" id="Legend3" >
        <asp:Label ID="lgShipmentDetails" runat="server" CssClass="clsLabelLeft"
            Text="<%$ Resources:LocalString, ShippingDetails %>"></asp:Label>
    </legend>
    <div class="fieldSection">    
    <label for="upShpDateControl">
        <asp:Label ID="lblShipDate" runat="server" 
                Text="<%$ Resources:LocalString, ShippingDate %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtShipingDate" runat="server" CssClass="tiny"></asp:TextBox>
    <asp:CalendarExtender ID="calShipingDate" runat="server" TargetControlID="txtShipingDate"
        Format="dd/MM/yyyy" CssClass="clsCalendar">
    </asp:CalendarExtender>
    
    <label for="txtParcelNo">
        <asp:Label ID="lblParcelNumber" runat="server" 
             Text="<%$ Resources:LocalString, ParcelNumber %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtParcelNo" runat="server" Enabled="false" Visible="false" CssClass="disable"></asp:TextBox>
            <asp:TextBox ID="txtCount" runat="server" Text="1" CssClass="tiny"></asp:TextBox>
    
    <label for="txtContentType">
        <asp:Label ID="lblContentType" runat="server" 
                Text="<%$ Resources:LocalString, ContentType %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtContentType" runat="server" MaxLength="60" CssClass="tiny"></asp:TextBox>    
    <asp:HyperLink ID="hplProhibitedList" runat="server" 
        NavigateUrl="rptProhibitedList.aspx"    OnClientClick="rptProhibitedList.target =’blank’">
        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, ProhibitedProducts %>" ></asp:Label>
    </asp:HyperLink>
    
    <label for="ddlContainer">
        <asp:Label ID="lblContainer" runat="server" 
                Text="<%$ Resources:LocalString, Container %>" ></asp:Label>*
    </label>
    <asp:DropDownList ID="ddlContainer" runat="server" CssClass="tiny" ></asp:DropDownList>
    </div>
    <div class="fieldSection">
    <label for="txtContainerGrossWeight">
        <asp:Label ID="lblContainerGrossWeight" runat="server" 
                Text="<%$ Resources:LocalString, ContainerGrossWeight %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtContainerGrossWeight" runat="server" MaxLength="7" CssClass="tiny"></asp:TextBox>
    <asp:DropDownList ID="ddlWeightUnit" runat="server" CssClass="tiny" ></asp:DropDownList>
    
    <label for="txtLength">
        <asp:Label ID="lblLength" runat="server" 
                Text="<%$ Resources:LocalString, Length %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtLength" runat="server" MaxLength="6" CssClass="tiny" ></asp:TextBox>
    
    <label for="txtWidth">
        <asp:Label ID="lblWidht" runat="server" 
                Text="<%$ Resources:LocalString, Width %>" CssClass="tiny" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtWidth" runat="server" MaxLength="6" CssClass="tiny" ></asp:TextBox>
    
    <label for="txtHeight">
        <asp:Label ID="lblHeight" runat="server" 
                Text="<%$ Resources:LocalString, Height %>" ></asp:Label>*
    </label>
    <asp:TextBox ID="txtHeight" runat="server" MaxLength="6" CssClass="tiny" ></asp:TextBox>
    <asp:DropDownList ID="ddlDimensionUnit" runat="server" CssClass="tiny" >
        </asp:DropDownList>
        </div>
	<asp:Button ID="btnAddShipment" CssClass="add" runat="server" 
        Text="<%$ Resources:LocalString, AddShipment %>" 
        ValidationGroup="grpShipment" onclick="btnAddShipment_Click" />

    <asp:GridView ID="gv_Shipment" runat="server" AutoGenerateColumns="False"
        AllowSorting="True" CaptionAlign="Top"
        CellPadding="0"
        onrowcommand="gv_Shipment_RowCommand" 
        onrowdeleting="gv_Shipment_RowDeleting" 
        onrowcancelingedit="gv_Shipment_RowCancelingEdit" 
        onrowediting="gv_Shipment_RowEditing" 
        onrowupdating="gv_Shipment_RowUpdating" 
        onrowdatabound="gv_Shipment_RowDataBound"
        CssClass="shipmentsList">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ParcelNumber %>">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>                     
                </ItemTemplate>            
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ContentType %>">
                <ItemTemplate>
                    <asp:Label ID="lblContentType" runat="server" Text='<%#Eval("gvContent").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <asp:TextBox ID="txtContentType" runat="server" Text='<%#Eval("gvContent").ToString()%>'/>
                </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Container %>" >
                <ItemTemplate>
                    <asp:Label ID="lblContainer" runat="server" Text='<%#Eval("gvContainer").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <%--<asp:TextBox ID="txtContainer" runat="server" Text='<%#Eval("gvContainer").ToString()%>' />--%>
                        <asp:Label ID="lblContainer1" runat="server" Text='<%#Eval("gvContainer").ToString() %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlgvContainer" runat="server" >        </asp:DropDownList>
                </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Weight %>" >
                <ItemTemplate>
                    <asp:Label ID="lblWeight" runat="server" Text='<%#Eval("gvWeight").ToString() %>'></asp:Label>
                    <asp:Label ID="lblWeightUnit" runat="server" Text='<%#Eval("gvWeightUnit") %>'> </asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("gvWeight").ToString()%>'/>
                        <asp:Label ID="lblWeightUnit" runat="server" Text='<%#Eval("gvWeightUnit") %>'> </asp:Label>
                </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Length %>" >
                <ItemTemplate>
                    <asp:Label ID="lblLength" runat="server" Text='<%#Eval("gvLength").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <asp:TextBox ID="txtLength" runat="server" Text='<%#Eval("gvLength").ToString()%>'/>
                </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Width %>" >
                <ItemTemplate>
                    <asp:Label ID="lblWidth" runat="server" Text='<%#Eval("gvWidth").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <asp:TextBox ID="txtWidth" runat="server" Text='<%#Eval("gvWidth").ToString()%>'/>
                </EditItemTemplate>
                </asp:TemplateField>

                <%--<asp:BoundField DataField="gvContent" HeaderText="<%$ Resources:LocalString, ContentType %>" ><ControlStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="gvContainer" HeaderText="<%$ Resources:LocalString, Container %>" ><ControlStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="gvWeight" HeaderText="<%$ Resources:LocalString, Weight %>" ><ControlStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="gvLength" HeaderText="<%$ Resources:LocalString, Length %>" ><ControlStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="gvWidth" HeaderText="<%$ Resources:LocalString, Width %>" ><ControlStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="gvHeight" HeaderText="<%$ Resources:LocalString, Height %>" ><ControlStyle Width="10%" /></asp:BoundField>--%>
                <asp:BoundField DataField="gvDimensionUnit" HeaderText="" Visible="false"></asp:BoundField>
                <asp:BoundField DataField="gvWeightUnit" HeaderText=""  Visible="false"></asp:BoundField>

                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Height %>" >
                <ItemTemplate>
                        <asp:Label ID="lblHeight" runat="server" Text='<%#Eval("gvHeight").ToString() %>'></asp:Label>
                        <asp:Label ID="lblHeightUnit" runat="server" Text='<%#Eval("gvDimensionUnit") %>'> </asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                        <asp:TextBox ID="txtHeight" runat="server" Text='<%#Eval("gvHeight").ToString()%>'/>
                        <asp:Label ID="lblHeightUnit" runat="server" Text='<%#Eval("gvDimensionUnit") %>'> </asp:Label>
                </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Edit" Text="Edit"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="true" CommandName="Update" Text="Update"></asp:LinkButton>
                        <asp:LinkButton id="LinkButton3" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Image/delete.png" CommandName="Delete" ID="dlRow" runat="server" />
                </ItemTemplate>
                </asp:TemplateField>
            </Columns>

    </asp:GridView>
</fieldset>

<fieldset  id ="Fieldset2" runat="server" class="first"  >
    <legend class="clsLegend" id="Legend2" >
        <asp:Label ID="lgOptions" runat="server"
            Text="<%$ Resources:LocalString, Option %>"></asp:Label>
    </legend>
    <%--  <asp:CheckBox ID="CheckBox1" runat="server" Text="Options"/>--%>
    <div class="camouflageTable2">
        <asp:CheckBoxList ID="cblOptions" runat="server" RepeatDirection="Vertical" TextAlign="Left"></asp:CheckBoxList>
    </div>
</fieldset>

<fieldset  id ="Fieldset4" runat="server" class="first"  >
    <legend class="clsLegend" id="Legend4" >
        <asp:Label ID="Label1" runat="server" 
            Text="<%$ Resources:LocalString, Total %>"></asp:Label>
    </legend>    
    <label for="txtTotalParcelNo">
        <asp:Label ID="lblTotalParcel" runat="server" 
                Text="<%$ Resources:LocalString, ParcelCount %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtTotalParcelNo" runat="server" Text="0"  Enabled="false" CssClass="tiny"></asp:TextBox>
    
    <label for="txtTotalGrossWeight">
        <asp:Label ID="Label3" runat="server" 
                Text="<%$ Resources:LocalString, GrossWeight %>" ></asp:Label>
    </label>
    <asp:TextBox ID="txtTotalGrossWeight" runat="server" Text="0"  Enabled="false" CssClass="tiny"></asp:TextBox>
</fieldset>

<div id="buttons">
    <asp:Button ID="btnGetQuote" runat="server" Text="<%$ Resources:LocalString, GetQuote %>" 
        onclick="btnGetQuote_Click" />
</div>    
<asp:ModalPopupExtender
    CancelControlID="btnCancelModalPopup" 
    runat="server" 
    PopupControlID="Panel1" 
    id="ModalPopupExtender1" 
    TargetControlID="hplProhibitedList"
    BackgroundCssClass="modalBackground" /> 

<asp:Panel ID="Panel1" runat="server" CssClass="customerModelWindow" style="display:none;"> 
    <asp:GridView ID="gvProhibitedList" runat="server" AutoGenerateColumns="False" >
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ProhibitedProducts%>" 
                        ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblProhibitedList" runat="server" Text='<%#Eval("ComboText").ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
                                
            </Columns>
    </asp:GridView>
    <asp:Button ID="btnCancelModalPopup" runat="server" Text="<%$ Resources:LocalString, Close%>" /> 
</asp:Panel> 

</asp:Content>