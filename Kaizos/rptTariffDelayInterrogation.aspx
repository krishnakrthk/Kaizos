<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="rptTariffDelayInterrogation.aspx.cs" Inherits="Kaizos.rptTariffDelayInterrogation1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    function goBack() {
        window.history.back()
    }
</script>
<table class="tariff_table" >
    <tr class="gridHeader">
        <th colspan="8"> <asp:Label runat="server" ID="lblExp"  Text="<%$ Resources:LocalString, Express %>" /></th>
    </tr>
    <tr class="gridHeader">
        <td runat="server" id="td1"><asp:Label runat="server" ID="Label1"  Text="<%$ Resources:LocalString, CarrierType %>" /></td>
        <td><asp:Label runat="server" ID="Label2"  Text="<%$ Resources:LocalString, Carrier %>" /></td>
        <td><asp:Label runat="server" ID="Label3"  Text="<%$ Resources:LocalString, Service %>" /></td>
        <td class="hidden"><asp:Label runat="server" ID="Label4"  Text="<%$ Resources:LocalString, Shipping %>" /></td>
        <td><asp:Label runat="server" ID="Label5"  Text="<%$ Resources:LocalString, Info %>" /></td>
        <td><asp:Label runat="server" ID="Label6"  Text="<%$ Resources:LocalString, Delivery %>" /></td>
        <td><asp:Label runat="server" ID="Label7"  Text="<%$ Resources:LocalString, TariffDutyFree %>" /></td>
        <td>&nbsp;</td>
    </tr>
    <tr runat="server" id="trExp" visible="false">
        <td runat="server" id="td2"><asp:Label runat="server" ID="Label8"  Text="<%$ Resources:LocalString, YourCarrier %>" /></td>
        <td><asp:Label runat="server" ID="lblKeyCarrier"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblKeyService"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblKeyShipping"  Text="" /></td>
        <td class="hidden"><asp:Label runat="server" ID="lblKeyInfo"  Text="<%$ Resources:LocalString, ShipingInfo %>" /></td>
        <td><asp:Label runat="server" ID="lblKeyDelivery"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblKeyTariff"  Text="" /></td>
        <td><asp:RadioButton ID="rdCarrierSelection1" GroupName="rdCarrierSelection"  runat="server" /></td>
    </tr>

     <tr>
        <td runat="server" id="td3"><asp:Label runat="server" ID="Label9"  Text="<%$ Resources:LocalString, ProposedCarrier %>" /></td>
        <td><asp:Label runat="server" ID="lblCarrier"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblService"  Text="" /></td>
        <td class="hidden"><asp:Label runat="server" ID="lblShipping"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblInfo"  Text="<%$ Resources:LocalString, ShipingInfo %>" /></td>
        <td><asp:Label runat="server" ID="lblDelivery"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblTariff"  Text="" /></td>
        <td><asp:RadioButton ID="rdCarrierSelection2" GroupName="rdCarrierSelection" runat="server" /></td>
    </tr>
</table>

<table class="tariff_table">
    <tr class="gridHeader">
        <th colspan="8"> <asp:Label runat="server" ID="Label10"  Text="<%$ Resources:LocalString, Economy %>" /></th>
    </tr>
    <tr class="gridHeader">
        <td runat="server" id="td4"><asp:Label runat="server" ID="Label11"  Text="<%$ Resources:LocalString, CarrierType %>" /></td>
        <td><asp:Label runat="server" ID="Label12"  Text="<%$ Resources:LocalString, Carrier %>" /></td>
        <td><asp:Label runat="server" ID="Label13"  Text="<%$ Resources:LocalString, Service %>" /></td>
        <td class="hidden"><asp:Label runat="server" ID="Label14"  Text="<%$ Resources:LocalString, Shipping %>" /></td>
        <td><asp:Label runat="server" ID="Label15"  Text="<%$ Resources:LocalString, Info %>" /></td>
        <td><asp:Label runat="server" ID="Label16"  Text="<%$ Resources:LocalString, Delivery %>" /></td>
        <td><asp:Label runat="server" ID="Label17"  Text="<%$ Resources:LocalString, TariffDutyFree %>" /></td>
        <td>&nbsp;</td>
    </tr>
    <tr runat="server" id="trEco" visible="false" >
        <td runat="server" id="td5"><asp:Label runat="server" ID="Label18"  Text="<%$ Resources:LocalString, YourCarrier %>" /></td>
        <td><asp:Label runat="server" ID="lblEcoKeyCarrier"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoKeyService"  Text="" /></td>
        <td class="hidden"><asp:Label runat="server" ID="lblEcoKeyShipping"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoKeyInfo"  Text="<%$ Resources:LocalString, ShipingInfo %>" /></td>
        <td><asp:Label runat="server" ID="lblEcoKeyDelivery"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoKeyTariff"  Text="" /></td>
        <td><asp:RadioButton ID="rdCarrierSelection3" GroupName="rdCarrierSelection" runat="server" /></td>
    </tr> 
     <tr>
        <td runat="server" id="td6"><asp:Label runat="server" ID="Label25"  Text="<%$ Resources:LocalString, ProposedCarrier %>" /></td>
        <td><asp:Label runat="server" ID="lblEcoCarrier"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoService"  Text="" /></td>
        <td class="hidden"><asp:Label runat="server" ID="lblEcoShipping"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoInfo"  Text="<%$ Resources:LocalString, ShipingInfo %>" /></td>
        <td><asp:Label runat="server" ID="lblEcoDelivery"  Text="" /></td>
        <td><asp:Label runat="server" ID="lblEcoTariff"  Text="" /></td>
        <td><asp:RadioButton ID="rdCarrierSelection4" GroupName="rdCarrierSelection" runat="server" /></td>
    </tr>
</table>

<div id="buttons">
<asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalString, Back %>" 
                CssClass="Back" onclick="btnBack_Click1"   />

    <%--<input type="button" value="Back" onclick="goBack()" class="Back" />--%>
    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:LocalString, Reset %>" onclick="btnReset_Click" />
    <asp:Button ID="btnShipIt" runat="server" Text="<%$ Resources:LocalString, ShipIt %>" onclick="btnShipIt_Click" />
</div>


<asp:HiddenField runat="server" ID="hdKeyFuelCharge" />
<asp:HiddenField runat="server" ID="hdKeySurchargeDescription" />
<asp:HiddenField runat="server" ID="hdKeySurchargeValue" />
<asp:HiddenField runat="server" ID="hdKeyOptionsDescription" />
<asp:HiddenField runat="server" ID="hdKeyOptionsValue" />

<asp:HiddenField runat="server" ID="hdFuelCharge" />
<asp:HiddenField runat="server" ID="hdSurchargeDescription" />
<asp:HiddenField runat="server" ID="hdSurchargeValue" />
<asp:HiddenField runat="server" ID="hdOptionsDescription" />
<asp:HiddenField runat="server" ID="hdOptionsValue" />

<asp:HiddenField runat="server" ID="hdEcoKeyFuelCharge" />
<asp:HiddenField runat="server" ID="hdEcoKeySurchargeDescription" />
<asp:HiddenField runat="server" ID="hdEcoKeySurchargeValue" />
<asp:HiddenField runat="server" ID="hdEcoKeyOptionsDescription" />
<asp:HiddenField runat="server" ID="hdEcoKeyOptionsValue" />

<asp:HiddenField runat="server" ID="hdEcoFuelCharge" />
<asp:HiddenField runat="server" ID="hdEcoSurchargeDescription" />
<asp:HiddenField runat="server" ID="hdEcoSurchargeValue" />
<asp:HiddenField runat="server" ID="hdEcoOptionsDescription" />
<asp:HiddenField runat="server" ID="hdEcoOptionsValue" />


</asp:Content>
