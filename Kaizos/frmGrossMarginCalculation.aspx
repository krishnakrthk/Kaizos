<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmGrossMarginCalculation.aspx.cs" Inherits="Kaizos.frmGrossMarginCalculation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="errorMsg simulTool" ID="errorMsg1" runat="server" >
    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valSelection" Text="<%$ Resources:LocalString, ValidationSelection %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valCalLess" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="ValPositive" Text="<%$ Resources:LocalString, ValPositive %>" runat ="server" Visible= "false"></asp:Label>
    <asp:Label ID="valGreater" Text="<%$ Resources:LocalString, ValGreaterThan %>" runat ="server" Visible= "false"></asp:Label>
    
    <asp:CustomValidator ID="val_Header" runat="server" 
        ControlToValidate="txtWeightIncrement" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpHeader" 
        onservervalidate="val_Header_ServerValidate">
    </asp:CustomValidator>
</div>

<div class="errorMsg simulTool" ID="errorMsg2" runat="server">
    <asp:CustomValidator ID="val_Calculation" runat="server" 
        ControlToValidate="ddlSimulationID" 
        EnableClientScript="False" 
        ValidateEmptyText="True"
        ValidationGroup="grpRefresh"
        onservervalidate="val_Calculation_ServerValidate" >
    </asp:CustomValidator>
    
    <asp:Label ID="lblAlready" runat="server" Visible="false"  Text="<%$ Resources:LocalString, SMValAlreadyExists %>"></asp:Label>
</div>

<fieldset class="second simul">
    <legend>
    </legend>
    <div class="fieldSection">
    <label for="ddlCustomerID">
        <asp:Label ID="lblCustomerID" runat="server"  
            Text="<%$ Resources:LocalString, CutomerID %>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlCustomerID" runat="server"
                onselectedindexchanged="ddlCustomerID_SelectedIndexChanged" 
                AutoPostBack="True">
    </asp:DropDownList>
    
    <label for="ddlCustomerID">
        <asp:Label ID="lblDefaultTariff" runat="server"  
            Text="<%$ Resources:LocalString, DefaultPublic %>"></asp:Label>
    </label>
    <asp:DropDownList ID="ddlDefaultTariff" runat="server"> </asp:DropDownList>
    
    <label for="ddlCustomerID">
        <asp:Label ID="lblValidUntil" runat="server" 
            Text="<%$ Resources:LocalString, TariffValid %>"></asp:Label>
    </label>
    <div class="inlineBlock">
    <asp:UpdatePanel ID="upValidDateControl" runat="server"  >
        <ContentTemplate>
            <asp:TextBox ID="txtValidDate" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="calValidDate" runat="server" TargetControlID="txtValidDate" Format="dd/MM/yyyy">
            </asp:CalendarExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
    <div class="fieldSection">
        <div class="fieldBloc">
            <br />
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalString, Save %>" onclick="btnSave_Click" cssclass="altButton" /> 
            <br />
            <asp:Button ID="btnExport" runat="server" 
                Text="<%$ Resources:LocalString, ExportCSV %>" onclick="btnExport_Click" cssclass="altButton" />    
            <br />
            <asp:Button ID="btnAssign" runat="server" 
                Text="<%$ Resources:LocalString, AffectMargin %>" 
                onclick="btnAssign_Click" cssclass="altButton"/> 
        </div>
    </div>
</fieldset>


<fieldset class="first sender simul">
    <legend>
        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, SubTotalBlock %>"></asp:Label>
    </legend>
    <label for="ddlCustomerID">
        <asp:Label ID="lblWeightIncrement" runat="server"  CssClass="clsLabelLeft"
            Text="<%$ Resources:LocalString, WeightIncrement %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtWeightIncrement" class="tiny" runat="server" ></asp:TextBox>
    
    <label for="ddlCustomerID">
        <asp:Label ID="lblWeightLimit" runat="server"  CssClass="clsLabelLeft"
            Text="<%$ Resources:LocalString, WeightLimit %>"></asp:Label>
    </label>
    <asp:TextBox ID="txtWeighLimit" class="tiny" runat="server"></asp:TextBox>
    
    <div class="fieldBloc">
            <asp:Button ID="btnAddBlock" runat="server"  CssClass="altButton" Text="<%$ Resources:LocalString, AddBlock %>" 
                onclick="btnAddBlock_Click" ValidationGroup="grpHeader"/>
    </div>
</fieldset>

<fieldset class="first sender simul">
    <legend>
        <asp:Label ID="lblSimulationID2" runat="server"  CssClass="clsLabelRight"
                Text="<%$ Resources:LocalString, SimulationID %>"></asp:Label>
    </legend>
    <asp:Label ID="lblSimulationID" runat="server"  CssClass="clsLabelRight"
                Text="<%$ Resources:LocalString, SimulationID %>"></asp:Label>
    <asp:DropDownList ID="ddlSimulationID" runat="server" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlSimulationID_SelectedIndexChanged"> </asp:DropDownList> 
    
    <div class="fieldBloc">
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:LocalString, Refresh %>"    
            onclick="btnRefresh_Click" ValidationGroup="grpRefresh" />
    </div>
</fieldset>

<div class="simul_smalltables">
    <asp:Label ID="lblCarrier" runat="server" Text="<%$ Resources:LocalString, TariffBased %>"></asp:Label>
    <asp:GridView ID="gvCarrierTariff" runat="server" AutoGenerateColumns="False" 
            onrowdatabound="gvCarrierTariff_RowDataBound" >
        <Columns>
        <asp:TemplateField >
            <ItemTemplate>
                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("CarrierName").ToString() %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
                                
            <asp:TemplateField HeaderText="<%$ Resources:LocalString, PurchaseTariff %>" >
            <ItemTemplate>
                <asp:DropDownList ID="ddlPurchaseTariffList" runat="server"  ></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, PublicTariff %>"  >
            <ItemTemplate>
                <asp:DropDownList ID="ddlPublicTariffList" runat="server" ></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <asp:Label ID="lblTest" runat="server"></asp:Label>
 
    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, SurchargeDiscount %>"></asp:Label>
    
    <asp:GridView ID="gvCustomerDiscount" runat="server" AutoGenerateColumns="False" 
        onrowdatabound="gvCustomerDiscount_RowDataBound" 
        RowStyle-VerticalAlign="Bottom">
        <Columns>

            <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="lblCustomerServiceName" runat="server" Text='<%#Eval("CarrierName").ToString() %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
                                
            <asp:TemplateField HeaderText="<%$ Resources:LocalString, Fuel %>"  >
            <ItemTemplate>
                <asp:TextBox ID="txtFuelDiscount" runat="server" ></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, Safety %>">
            <ItemTemplate>
                <asp:TextBox ID="txtSafetyDiscount" runat="server"  ></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
            
    </asp:GridView>
</div>
<asp:GridView ID="gvCalcualtion" runat="server" AutoGenerateColumns="False" 
    onrowdatabound="gvCalcualtion_RowDataBound" 
    ShowFooter="True" onrowcreated="gvCalcualtion_RowCreated" class="simul_table" >
    <Columns>
    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMWeight %>" HeaderStyle-CssClass="columnZero">
        <ItemTemplate>
            <asp:Label ID="lblWeightRangeCol1" runat="server" Text='<%#Eval("gvWeight").ToString()%>'></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
                                
    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMShippingCountry %>" HeaderStyle-CssClass="firstColumns" >
        <ItemTemplate>
            <asp:DropDownList ID="ddlShippingCountry" runat="server"  ></asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDeliveryCountry %>" HeaderStyle-CssClass="firstColumns"  >
        <ItemTemplate>
            <asp:DropDownList ID="ddlDeliveryCountry" runat="server" ></asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMasterServiceName %>" HeaderStyle-CssClass="firstColumns"  >
        <ItemTemplate>
            <asp:DropDownList ID="ddlMasterServiceName" runat="server" ></asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMAverageWeight %>" HeaderStyle-CssClass="secondColumns"  >
        <ItemTemplate>
            <asp:TextBox ID="txtAverageWeight" Text='<%#Eval("gvAverageWeight").ToString() %>' runat="server"  CssClass="clsLabelCenter"></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMADV %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
            <asp:TextBox ID="txtAdv"  Text='<%#Eval("gvADV").ToString() %>' runat="server" ></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMFreightPurchase %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
            <asp:Label ID="lblFreightPurchase"  Text='<%#MyFormat(Eval("gvFreightPurchase").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPurchaseTariff %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
            <asp:Label ID="lblPurchaseTariff"  Text='<%#MyFormat(Eval("gvPurchaseTariff").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCarriers %>" HeaderStyle-CssClass="thirdColumns" >
        <ItemTemplate>
            <asp:Label ID="lblCarriers"  Text='<%#Eval("gvCarrier").ToString() %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDiscount %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:TextBox ID="txtDiscount"  Text='<%#MyFormat(Eval("gvDiscount").ToString()) %>' runat="server" ></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicCarrier %>" HeaderStyle-CssClass="thirdColumns">
        <ItemTemplate>
                <asp:DropDownList ID="dllPublicCarrier" runat="server" CssClass="medium"></asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicTariff%>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:TextBox ID="txtPublicFreight"  Text='<%#MyFormat(Eval("gvPublicTariff").ToString()) %>' runat="server" ></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentSaleTariff %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblCurrentSaleTariff"  Text='<%#MyFormat(Eval("gvCurrentSaleTariff").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
                    
    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentTurnOver %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblCurrentTurnOver"  Text='<%# MyFormat(Eval("gvCurrentTurnOver").ToString())  %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMargin %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:TextBox ID="txtGrossMarign"  Text='<%# MyFormat1(Eval("gvGrossMargin").ToString()) %>' runat="server" ></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>


    <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesFreight%>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblSalesFretTariff"  Text='<%#MyFormat(Eval("gvSalesFretTariff").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTariff %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblProposedSalesTariff"  Text='<%#MyFormat(Eval("gvProposedSalesTariff").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTurnOver %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblSalesTurnOver"  Text='<%#MyFormat(Eval("gvSalesTurnOver").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMGrossMargin %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblSalesGrossMargin"  Text='<%#MyFormat(Eval("gvSalesGrossMargin").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareSales %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblComparisonSales"  Text='<%#MyFormat(Eval("gvComparisonSales").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareMargin %>" HeaderStyle-CssClass="secondColumns" >
        <ItemTemplate>
                <asp:Label ID="lblComparisonMargin"  Text='<%#MyFormat(Eval("gvComparisonMargin").ToString()) %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareMargin %>" Visible="false">
        <ItemTemplate>
                <asp:Label ID="lblPublicSurcharge"  Text='<%#Eval("gvPublicSurcharge").ToString() %>' runat="server" ></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

    </Columns>

</asp:GridView>

<asp:GridView ID="gvOuterList" runat="server" AutoGenerateColumns="false" 
                onrowdatabound="gvOuterList_RowDataBound" 
                ShowHeader ="false" 
				class="simul_table outer"
                >
        <Columns>
        <asp:TemplateField Visible="false">
        <ItemTemplate>
            <asp:Label ID="lblID" runat="server" Text='<%#Eval("DataField").ToString() %>' Visible="false"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
         <div class="simul_table_header">
            <asp:Label ID="lblListRef" runat="server" Text='<%$ Resources:LocalString, Reference %>'></asp:Label>
            <asp:Label ID="lblID1" runat="server" Text='<%#Eval("DataField").ToString() %>'></asp:Label> 
            <asp:Label ID="Label4" runat="server" Text='<%$ Resources:LocalString, SMShippingCountry %>' ></asp:Label> 
            <asp:Label ID="lblGHShipcountry" runat="server"></asp:Label> 
            <asp:Label ID="Label6" runat="server" Text='<%$ Resources:LocalString, SMDeliveryCountry %>' ></asp:Label> 
            <asp:Label ID="lblGHDeliverycountry" runat="server"></asp:Label> 
            <asp:Label ID="Label8" runat="server" Text='<%$ Resources:LocalString, SMMasterServiceName %>' ></asp:Label> 
            <asp:Label ID="lblGHMasterService" runat="server"></asp:Label> 
        </div>
        <asp:GridView ID="gvCalculationList" runat="server" AutoGenerateColumns="False"             
            ShowFooter="True" onrowdatabound="gvCalculationList_RowDataBound" CssClass="inside_table" >
                            <Columns>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMWeight %>" >
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightRangeC1" runat="server" Text='<%#Eval("gvWeight").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                   
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMShippingCountry %>" HeaderStyle-CssClass="firstColumns">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlShippingCountryC2" runat="server"  Enabled="false"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDeliveryCountry %>" HeaderStyle-CssClass="firstColumns">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlDeliveryCountryC2" runat="server" Enabled="false" ></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMasterServiceName %>" HeaderStyle-CssClass="firstColumns">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMasterServiceNameC3" runat="server" Enabled="false" ></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMAverageWeight %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAverageWeightC4" Text='<%#Eval("gvAverageWeight").ToString() %>' runat="server"  Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMADV %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:TextBox ID="txtAdvC5"  Text='<%#Eval("gvADV").ToString() %>' runat="server" Enabled="false" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMFreightPurchase %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblFreightPurchaseC6"  Text='<%#Eval("gvFreightPurchase").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPurchaseTariff %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblPurchaseTariffC7"  Text='<%#Eval("gvPurchaseTariff").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                                  <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCarriers %>" HeaderStyle-CssClass="thirdColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblCarriersC8"  Text='<%#Eval("gvCarrier").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDiscount %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:TextBox ID="txtDiscountC9"  Text='<%#Eval("gvDiscount").ToString() %>' runat="server" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicCarrier %>" HeaderStyle-CssClass="thirdColumns"  >
                                <ItemTemplate>
                                     <%--<asp:DropDownList ID="dllPublicCarrierC10" runat="server" Enabled="false"></asp:DropDownList>--%> 
                                      <asp:TextBox ID="txtPublicCarrierC10"  Text='<%#Eval("gvPublicCarrier").ToString() %>' runat="server" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicTariff%>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:TextBox ID="txtPublicFreightC11"  Text='<%#Eval("gvPublicTariff").ToString() %>' runat="server" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentSaleTariff %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblCurrentSaleTariffC12"  Text='<%#Eval("gvCurrentSaleTariff").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                    
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentTurnOver %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblCurrentTurnOverC13"  Text='<%#Eval("gvCurrentTurnOver").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMargin %>"  HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:TextBox ID="txtGrossMarignC14"  Text='<%#Eval("gvGrossMargin").ToString() %>' runat="server" Enabled="false" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>


                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesFreight%>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblSalesFretTariffC15"  Text='<%#Eval("gvSalesFretTariff").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTariff %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblProposedSalesTariffC16"  Text='<%#Eval("gvProposedSalesTariff").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTurnOver %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblSalesTurnOverC17"  Text='<%#Eval("gvSalesTurnOver").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMGrossMargin %>" HeaderStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblSalesGrossMarginC18"  Text='<%#Eval("gvSalesGrossMargin").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareSales %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblComparisonSalesC19"  Text='<%#Eval("gvComparisonSales").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareMargin %>" HeaderStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblComparisonMarginC20"  Text='<%#Eval("gvComparisonMargin").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            </Columns>
                            
           </asp:GridView>
           
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>

        </asp:GridView>

    <asp:GridView ID="gvGrossTotal" runat="server" AutoGenerateColumns="False" 
                  ShowHeader ="false" CssClass="grossTotal" >
                            <Columns>
                            
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMWeight %>" ItemStyle-CssClass="columnZero" >
                                <ItemTemplate>
                                <asp:Label ID="lblTotalWeightRange" runat="server" Text='<%$ Resources:LocalString, Total %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMShippingCountry %>" ItemStyle-CssClass="firstColumns"   >
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalOrigin" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDeliveryCountry %>" ItemStyle-CssClass="firstColumns"   >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalDestination" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMasterServiceName %>" ItemStyle-CssClass="firstColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalMaster" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMAverageWeight %>" ItemStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAverageWeight" Text='<%#Eval("gvAverageWeight").ToString() %>' runat="server"  CssClass="clsLabelCenter"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMADV %>" ItemStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalAdv"  Text='<%#Eval("gvADV").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMFreightPurchase %>" ItemStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalFreightPurchase"  Text='<%#MyFormat(Eval("gvFreightPurchase").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPurchaseTariff %>" ItemStyle-CssClass="secondColumns"  >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalPurchaseTariff"  Text='<%#MyFormat(Eval("gvPurchaseTariff").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                                  <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCarriers %>" ItemStyle-CssClass="thirdColumns2">
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalCarrier" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMDiscount %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalDiscount"  Text='<%#MyFormat(Eval("gvDiscount").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicCarrier %>" ItemStyle-CssClass="thirdColumns2" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalPublicCarrier" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMPublicTariff%>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalPublicFreight"  Text='<%#MyFormat(Eval("gvPublicTariff").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentSaleTariff %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lbltotalCurrentSaleTariff"  Text='<%#MyFormat(Eval("gvCurrentSaleTariff").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                    
                            <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCurrentTurnOver %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalCurrentTurnOver"  Text='<%# MyFormat(Eval("gvCurrentTurnOver").ToString())  %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMMargin %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalGrossMarign"  Text='<%# MyFormat1(Eval("gvGrossMargin").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesFreight%>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalSalesFretTariff"  Text='<%#MyFormat(Eval("gvSalesFretTariff").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTariff %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalProposedSalesTariff"  Text='<%#MyFormat(Eval("gvProposedSalesTariff").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMSalesTurnOver %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalSalesTurnOver"  Text='<%#MyFormat(Eval("gvSalesTurnOver").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMGrossMargin %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalSalesGrossMargin"  Text='<%#MyFormat(Eval("gvSalesGrossMargin").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareSales %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalComparisonSales"  Text='<%#MyFormat(Eval("gvComparisonSales").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareMargin %>" ItemStyle-CssClass="secondColumns" >
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalComparisonMargin"  Text='<%#MyFormat(Eval("gvComparisonMargin").ToString()) %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="<%$ Resources:LocalString, SMCompareMargin %>"  Visible="false">
                                <ItemTemplate>
                                     <asp:Label ID="lblTotalPublicSurcharge"  Text='<%#Eval("gvPublicSurcharge").ToString() %>' runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            </Columns>
                </asp:GridView>

</asp:Content>
