<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmTariffOptionsAndSurcharge.aspx.cs" Inherits="Kaizos.frmTariffOptionsAndSurcharge" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"> 
    </asp:ScriptManager>

<div class="errorMsg" id="errorMsg_surcharge" runat="server">
    <asp:CustomValidator ID="val_surcharge" runat="server"
            ControlToValidate   ="txtTariffRef"
            EnableClientScript  ="False"
            ValidateEmptyText   ="True"
            ValidationGroup     ="grpSurcharge" 
            onservervalidate    ="val_Shipment_ServerValidate"> </asp:CustomValidator>

     <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
     <asp:Label ID="valNotFound" Text="<%$ Resources:LocalString, ValidationNotFound %>" runat ="server" Visible= "false"></asp:Label>
</div>

<fieldset class="third"> 
    <legend>
        <asp:Label ID="lblTariffOption" runat="server" Text="<%$ Resources:LocalString, OptionsSurchargeFilter %>"  CssClass="clsLabelLeft"></asp:Label>
    </legend>
    <div class="fieldBloc">
        <label for="ddlCarrier">
            <asp:Label ID="lblTariffRef" runat="server"  Text="<%$ Resources:LocalString, TariffReference %>" ></asp:Label>
        </label>
        <asp:TextBox ID="txtTariffRef" runat="server"></asp:TextBox>
    
        <asp:Button ID="btnSearch" runat="server"  Text="<%$ Resources:LocalString, Search %>" 
                            onclick="btnSearch_Click"           ValidationGroup="grpSurcharge" />
    </div>
</fieldset>
            <asp:UpdatePanel id="updatepanel2" runat="server" class="left">
                    <ContentTemplate>
                    
                    <asp:GridView ID="gvSurchargeDetail" runat="server" AutoGenerateColumns          ="False" 
                                    DataKeyNames        ="ServiceID"    HeaderStyle-CssClass         ="gridHeader" 
                                    RowStyle-CssClass   ="gridRow"      AlternatingRowStyle-CssClass ="gridAlternate" 
                                    Visible             ="false" >
                    <Columns>

                      <asp:BoundField DataField="TariffReference"  HeaderText="<%$ Resources:LocalString, OptionParamTariffRef %>" ControlStyle-Width="0"> </asp:BoundField>
                      <asp:BoundField DataField="SurchageCode"     HeaderText="<%$ Resources:LocalString, OptionParamSurchargeCode %>" ControlStyle-Width="0"> </asp:BoundField>
                       <asp:BoundField DataField="ServiceName"      HeaderText="<%$ Resources:LocalString, OptionParamServiceName %>"> </asp:BoundField>
                       <asp:BoundField DataField="ParamID"          HeaderText="<%$ Resources:LocalString, ParamID %>"> </asp:BoundField>

                       <asp:TemplateField HeaderText="<%$ Resources:LocalString, ParamValue %>">
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="txtParamValue" Text='<%#Eval("ParamValue").ToString() %>'  ></asp:TextBox>   
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                   <asp:Button runat="server" id="btnUpdate" Text="<%$ Resources:LocalString, Update %>" Visible="false" 
                   onclick="btnUpdate_Click" ValidationGroup="grpOptionParam" ></asp:Button> 

                   <asp:Button runat="server" id="btnCancel" Text="<%$ Resources:LocalString, AllCancel %>" Visible="false" onclick="btnCancel_Click" ></asp:Button> 
				   <div class="errorMsg" id="errorMsg_OptionParam" runat="server">
						<asp:CustomValidator ID="val_OptionParam" runat="server"
							ControlToValidate   ="txtTariffRef"
							EnableClientScript  ="False"
							ValidateEmptyText   ="True"
							ValidationGroup     ="grpOptionParam" 
							onservervalidate="val_OptionParam_ServerValidate" 
							>
						</asp:CustomValidator>
					</div>

                </ContentTemplate>
                </asp:UpdatePanel>


<div class="divOptionSurchargeGrid">
     <asp:UpdatePanel id="upatepanel1" runat="server">
                <ContentTemplate>
                
                <asp:GridView ID="gvSurchargeMaster" runat="server"    AutoGenerateColumns="False" 
                                DataKeyNames="SurchargeCode"    HeaderStyle-CssClass="gridHeader" 
                                RowStyle-CssClass="gridRow" 
                        AlternatingRowStyle-CssClass="gridAlternate" 
                        onrowediting="gvSurchargeMaster_RowEditing" 
                        onrowcommand="gvSurchargeMaster_RowCommand" 
                        onrowcancelingedit="gvSurchargeMaster_RowCancelingEdit" 
                        onrowdatabound="gvSurchargeMaster_RowDataBound" 
                        onrowupdating="gvSurchargeMaster_RowUpdating" onrowcreated="gvSurchargeMaster_RowCreated" 
                                >
                    <Columns>

                    <asp:BoundField DataField="ServiceID" Visible="false" />

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterSurchargeCode %>"  HeaderStyle-CssClass="hiddencol"  ItemStyle-CssClass="hiddencol">
                    <ItemTemplate>
                    <asp:Label ID="lblRefName" runat="server" Text='<%#Eval("SurchargeCode").ToString() %>' ></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterSurchargeLabel %>" >
                    <ItemTemplate>
                     <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("SurchargeDescription").ToString() %>'></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterSurchargeType %>" >
                    <ItemTemplate>
                     <asp:Label ID="lblMasterSurchargeType" runat="server" Text='<%# SetSurchargeType(Eval("SurchargeType").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, MasterServiceName %>" >
                    <ItemTemplate>
                        <asp:Label ID="lblMasterServiceList" runat="server" Text='<%#Eval("MasterServiceName").ToString()%>'></asp:Label>
                        <asp:CheckBoxList id="cblMasterServieName" runat="server" DataValueField="ComboText" DataTextField="ComboText" Enabled="false" TextAlign="Left"> </asp:CheckBoxList>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lblMasterServiceList" runat="server" Text='<%#Eval("MasterServiceName").ToString()%>' ></asp:Label>
                        <asp:CheckBoxList id="cblMasterServieName" runat="server" DataValueField="ComboText" DataTextField="ComboText" EnableViewState="true" TextAlign="Left"></asp:CheckBoxList>
                    </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, Active %>" >
                    <ItemTemplate>
                     <asp:Label ID="lblMasterSurchargeActive" runat="server" Text='<%#Eval("Active").ToString() %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                     <asp:CheckBox ID="cbActive" runat="server" Checked='<%#IsActive(Eval("Active").ToString())%>' />
                    </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, EditParameter %>">
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="ibtEdit" Text="Parameter" AlternateText="Edit" ToolTip="<%$ Resources:LocalString, EditParameter %>"
                                    CommandName="parameter" CommandArgument='<%#Eval("SurchargeCode").ToString() %>' />
                            </ItemTemplate>
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
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
            </asp:UpdatePanel>
</div>
 
</asp:Content>
