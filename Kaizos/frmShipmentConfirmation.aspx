<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmShipmentConfirmation.aspx.cs" Inherits="Kaizos.frmShipmentConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
</asp:ScriptManager>

<asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="ShipReference" onrowdatabound="gvShipments_RowDataBound"
    ShowHeader="False" onrowcommand="gvShipments_RowCommand" CssClass="shippingConfirmation" >
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
            <table>
                <tr>
                    <th colspan="2">
                        <asp:Label runat="server" ID="Label11" Text ="<%$ Resources:LocalString, ShipingRef %>"/> 
                        <asp:Label runat="server" ID="lblShipReference" Text ='<%#  Eval("ShipReference").ToString() %>' ></asp:Label>
                    </th>
                    <th colspan="2">
                        <asp:Label runat="server" ID="Label13" Text ="<%$ Resources:LocalString, Status %>"/> 
                        <asp:Label runat="server" ID="Label15" Text ="OK"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td class="title"><asp:Label runat="server" ID="lblShipmentRef" Text ="<%$ Resources:LocalString, Service %>"></asp:Label></td>
                    <td><asp:Label ID="lblCarrier"  runat="server" Text ='<%#  Eval("CarrierService").ToString() %>'> </asp:Label> </td>
                    <td></td>
                    <td>
                        <asp:ImageButton ImageUrl="~/Image/Edit.png" CommandName="Edit1" ID="edRow" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ShipReference").ToString() %>' ToolTip="<%$ Resources:LocalString, Edit %>"/>
                        <asp:ImageButton ImageUrl="~/Image/Delete_Summary.png" CommandName="Delete1" ID="dlRow" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ShipReference").ToString() %>' ToolTip="<%$ Resources:LocalString, Delete %>"/>
                    </td>
                </tr>
                <tr>
                          <td class="title"><asp:Label runat="server" ID="Label49" Text ="<%$ Resources:LocalString, cname %>">></asp:Label></td>
                          <td><asp:Label ID="Label50"  runat="server" Text ='<%#  Eval("Carrier").ToString() %>' class="clsSummaryLabelLeft"> </asp:Label> </td>
                </tr>
                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label3" Text ="<%$ Resources:LocalString, WishedShippingDate %>"></asp:Label></td>
                    <td><asp:Label ID="Label4"  runat="server" Text ='<%#MyFormat(Eval("WishedShipDate").ToString()) %>'> </asp:Label> </td>
                    <td></td>
                    <td></td>
                </tr>
                
                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label7" Text ="<%$ Resources:LocalString, ShippingAddress %>"></asp:Label></td>
                    <td><asp:Label ID="Label8"  runat="server" Text ='<%#  Eval("SenderCompany").ToString() %>'> </asp:Label> </td>
                    <td class="title"><asp:Label runat="server" ID="Label9" Text ="<%$ Resources:LocalString, DeliveryAddress %>"></asp:Label></td>
                    <td><asp:Label ID="Label10"  runat="server" Text ='<%#  Eval("RecipientCompany").ToString() %>'> </asp:Label> </td>
                </tr>
                <tr>
                    <td></td>
                    <td ><asp:Label ID="Label12"  runat="server" Text ='<%#  MyName(Eval("SenderName").ToString()) %>'> </asp:Label> </td>
                    <td></td>
                    <td ><asp:Label ID="Label14"  runat="server" Text ='<%#  MyName(Eval("RecipientName").ToString()) %>'> </asp:Label> </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Label ID="Label16"  runat="server" Text ='<%#  Eval("SenderAddress1").ToString() %>'> </asp:Label> 
                    <asp:Label ID="Label2"  runat="server" Text ='<%#  Eval("SenderAddress2").ToString() %>'> </asp:Label>
                    <asp:Label ID="Label26"  runat="server" Text ='<%#  Eval("SenderAddress3").ToString() %>'> </asp:Label></td>
                    <td></td>
                    <td><asp:Label ID="Label18"  runat="server" Text ='<%#  Eval("RecipientAddress1").ToString() %>'> </asp:Label> 
                        <asp:Label ID="Label6"  runat="server" Text ='<%#  Eval("RecipientAddress2").ToString() %>'> </asp:Label>
                        <asp:Label ID="Label25"  runat="server" Text ='<%#  Eval("RecipientAddress3").ToString() %>'> </asp:Label></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Label ID="Label20"  runat="server" Text ='<%#  Eval("SenderZipCode").ToString()%>'> </asp:Label>
                        <asp:Label ID="Label23"  runat="server" Text ='<%#  Eval("SenderCity").ToString() %>'> </asp:Label> </td>
                    <td></td>
                    <td><asp:Label ID="Label22"  runat="server" Text ='<%#  Eval("RecipientZipCode").ToString()%>'> </asp:Label> 
                    <asp:Label ID="Label24"  runat="server" Text ='<%#  Eval("RecipientCity").ToString() %>'> </asp:Label> </td>
                </tr>

                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label30" Text ="<%$ Resources:LocalString, IndicativeDeliveryDate %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label19"  runat="server" Text ='<%# MyFormat(Eval("CalculatedShipDate").ToString()) %>' > </asp:Label>  
                        <asp:Label ID="Label46"  runat="server" Text ='<%# Eval("CalculatedDeliveryTime").ToString() %>' > </asp:Label> </td>
                </tr>
                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label17" Text ="<%$ Resources:LocalString, TotalParcel %>"></asp:Label></td>
                    <td ><asp:Label ID="Label27"  runat="server" Text ='<%#  Eval("UODCount").ToString() %>' > </asp:Label> </td>
                    <td class="title"><asp:Label ID="Label28"  runat="server" Text ="<%$ Resources:LocalString, TotalWeight %>"  > </asp:Label> 
                    <td ><asp:Label ID="Label29"  runat="server" Text ='<%# MyFormatDecimal (Eval("TotalWeight").ToString()) %>' > </asp:Label>
                    <asp:Label ID="lblWeightUnit1"  runat="server" Text ="" class="clsSummaryLabelLeft"> </asp:Label>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td class="title"><asp:Label ID="Label32"  runat="server" Text ="<%$ Resources:LocalString, TotalTax %>"  > </asp:Label> 
                    <td ><asp:Label ID="Label33"  runat="server" Text ='<%#  MyFormatDecimal(Eval("TaxableWeight").ToString()) %>' > </asp:Label>
                     <asp:Label ID="lblWeightUnit2"  runat="server" Text ="" class="clsSummaryLabelLeft"> </asp:Label>
                    </td>
                </tr>
                <tr>
                         
                    <td colspan="4">
                        <asp:GridView ID="gvShipmentDetail" runat="server" AutoGenerateColumns="False" class="shippingConfirmation_table">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ParcelNumber %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ParcelNo").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ContentType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ContentType").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Container %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("Container").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, Dimension %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("Length").ToString() %>'></asp:Label>*
                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Width").ToString() %>'></asp:Label>*
                                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("Height").ToString() %>'></asp:Label>
                                        <asp:Label ID="Label44" runat="server" Text='  <%#Eval("DimensionUnit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, ContainerGrossWeight %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%# MyFormatDecimal (Eval("Weight").ToString()) %>'></asp:Label>
                                        <asp:Label ID="Label45" runat="server" Text='  <%#Eval("WeightUnit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalString, TaxableWeight %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#MyFormatDecimal(Eval("TaxableWeight").ToString()) %>'></asp:Label>
                                     <asp:Label ID="Labe245" runat="server" Text='  <%#Eval("WeightUnit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                            
                        </asp:GridView>
                    </td>
                         
                </tr>

                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label21" Text ="<%$ Resources:LocalString, InsuranceDeclaredValue %>"></asp:Label></td>
                    <td ><asp:Label ID="Label31"  runat="server" Text ='<%#  MyFormatDecimal(Eval("DeclaredValue").ToString()) %>' > </asp:Label> 
                    <asp:Label ID="lblCurrency" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td class="title"><asp:Label ID="Label34"  runat="server" Text ="<%$ Resources:LocalString, CustomsValue %>"  > </asp:Label> 
                    <td ><asp:Label ID="Label35"  runat="server" Text ='<%#  MyFormatDecimal(Eval("CustomsValue").ToString()) %>' > </asp:Label> 
                    <asp:Label ID="Label51" runat="server" Text="EUR"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="title"><asp:Label runat="server" ID="Label47" Text ="<%$ Resources:LocalString, CalculatedInsurancevalue %>"></asp:Label></td>
					<td><asp:Label ID="Label48"  runat="server" Text ='<%#  MyFormatDecimal(Eval("CalculatedInsuranceAmount").ToString()) %>'> </asp:Label> 
                    <asp:Label ID="Label52" runat="server" Text="EUR"></asp:Label>
                    </td>
					<td class="title"><asp:Label ID="Label36"  runat="server" Text ="<%$ Resources:LocalString, CustomerInternalRef %>"  > </asp:Label> 
                    <td ><asp:Label ID="Label37"  runat="server" Text ='<%#  Eval("CustomerInternalReference").ToString() %>' > </asp:Label> </td>
                </tr>

                <tr>
                    <td class="title"><asp:Label ID="Label40"  runat="server" Text ="<%$ Resources:LocalString, Freight %>"  > </asp:Label> </td>
                    <td ><asp:Label ID="Label41"  runat="server" Text ='<%#  MyFormatDecimal(Eval("FreightAmount").ToString()) %>'   > </asp:Label> 
                    <asp:Label ID="Label53" runat="server" Text="EUR"></asp:Label>
                    </td>
                    <td class="title"><asp:Label ID="Label38"  runat="server" Text ="<%$ Resources:LocalString, FuelCharge %>"  > </asp:Label> 
                    <td ><asp:Label ID="Label39"  runat="server" Text ='<%# MyFormatDecimal(Eval("FuelCharge").ToString()) %>' > </asp:Label> 
                    <asp:Label ID="Label54" runat="server" Text="EUR"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td class="title"><asp:Label ID="Label42"  runat="server" Text ="<%$ Resources:LocalString, TotalAmount %>"  > </asp:Label> </td>
                    <td ><asp:Label ID="Label43"  runat="server" Text ='<%# MyFormatDecimal(Eval("TotalAmount").ToString()) %>'   > </asp:Label> 
                    <asp:Label ID="Label55" runat="server" Text="EUR"></asp:Label>
                    </td>
                </tr>

            </table>        
            </ItemTemplate>

        </asp:TemplateField>
    </Columns>
</asp:GridView>

<div id="buttons">
    <asp:Button ID="btnAddShipment" runat="server" Text="<%$ Resources:LocalString, AddShipment %>" onclick="btnAddShipment_Click"  /> 
    <asp:Button ID="btnSend" runat="server" Text="<%$ Resources:LocalString, Send %>" onclick="btnSend_Click" />
</div>

</asp:Content>
