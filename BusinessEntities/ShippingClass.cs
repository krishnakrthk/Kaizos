using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{

    /// <summary>
    /// To collect order details of the current shipment
    /// </summary>
    public class BShipmentOrder
    {
        public string ShipReference { get; set; }
        public BEnumAddressType RecipientType { get; set; }
        public DateTime WishedShipDate { get; set; }
        public int UODCount { get; set; }
        public float TotalWeight { get; set; }
        public BEnumFlag Insured { get; set; }
        public float DeclaredValue { get; set; }
        public string CurrencyType { get; set; }
        public string PackageType { get; set; }
        public string PackageMaterial { get; set; }
        public string ClosingMateiral { get; set; }
        public string SenderCompany { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderEmail { get; set; }
        public string SenderAddress1 { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderAddress3 { get; set; }
        public string SenderCity { get; set; }
        public string SenderZipCode { get; set; }
        public string SenderState { get; set; }
        public string SenderCountry { get; set; }
        public string SenderCollectDeadLine { get; set; }
        public string SenderComments { get; set; }
        public BEnumFlag SameReturnAddress { get; set; }
        public string ReturnCompany { get; set; }
        public string ReturnName { get; set; }
        public string ReturnPhone { get; set; }
        public string ReturnEmail { get; set; }
        public string ReturnAddress1 { get; set; }
        public string ReturnAddress2 { get; set; }
        public string ReturnAddress3 { get; set; }
        public string ReturnCity { get; set; }
        public string ReturnZipCode { get; set; }
        public string ReturnState { get; set; }
        public string ReturnCountry { get; set; }
        public string RecipientCompany { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientAddress1 { get; set; }
        public string RecipientAddress2 { get; set; }
        public string RecipientAddress3 { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientZipCode { get; set; }
        public string RecipientState { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientDeliveryDeadLine { get; set; }
        public string RecipientComments { get; set; }
        public string CustomerInternalReference { get; set; }
        public float CustomsValue { get; set; }
        public BEnumFlag SenderNotification { get; set; }
        public BEnumFlag RecipientNotification { get; set; }
        public string Carrier { get; set; }
        public DateTime OrderCreationTime { get; set; }
        public DateTime ShipDateTime { get; set; }
        public string Status { get; set; }
        public DateTime CalculatedShipDate { get; set; }
        public string CalculatedDeliveryTime { get; set; }
        public float TaxableWeight { get; set; }
        public float FreightAmount { get; set; }
        public float FuelCharge { get; set; }
        public BEnumShipPreference ChosenPreference { get; set; }
        public string AccountNo { get; set; }
        public string Options { get; set; }
        public string OptionsCharges { get; set; }
        public float TotalAmount { get; set; }
        public BEnumOrderType OrderType { get; set; }
        public BEnumPaymentType PaymentType { get; set; }
        public string CancelResponsible { get; set; }
        public List<BShipmentDetails> ShipDetail { get; set; }
        public BShipmentResult ShipmentResult { get; set; }
        public int ShipGroupID { get; set; }
        public string ContainerType { get; set; }
        public string Surcharge { get; set; }
        public string SurchargeDescription { get; set; }

        /***********[KS15MAR12]**********/
        public string CarrierService { get; set; }
        /***********[KS23APR12]**********/
        public string CalculatedInsuranceAmount { get; set; }
    }

    /// <summary>
    /// To collect detail of the of a despatch unit
    /// </summary>
    public class BShipmentDetails
    {
        public string ShippingReference { get; set; }
        public int ParcelNo { get; set; }
        public string ContentType { get; set; }
        public string Container { get; set; }
        public float Weight { get; set; }
        public string WeightUnit { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string DimensionUnit { get; set; }
        public float TaxableWeight { get; set; }
        public string TrackingNo { get; set; }
    }

    /// <summary>
    /// To group the shipments for a transaction
    /// </summary>
    public class BShipmentOrderTemp
    {
        public string AccountNo { get; set; }
        public string ShipReference { get; set; }
        public int ShipGroupID { get; set; }
        public string SessionID { get; set; }
        public BEnumFlag OpenGroup { get; set; }
    }

    /// <summary>
    /// Use to retrieve/send information on Quote/Delay.
    /// </summary>
    public class BShipmentQuotation
    {
        public string CarrierPriority { get; set; }
        public string CarrierType { get; set; }
        public string CarrierName { get; set; }
        public string ServiceName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Information { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double CalculatedTariff { get; set; }
        public double FuelSurcharge { get; set; }
        public string SurchargeDescription { get; set; }
        public string Surcharge { get; set; }
        public string OptionsDescription { get; set; }
        public string Options { get; set; }


        /***********[KS15MAR12]**********/
        
        public string InfoType { get; set; }


    }

    /// <summary>
    /// Use to retrieve/send information from Tariff name
    /// </summary>
    public class BTariffName
    {
        public string AffectedTariff { get; set; }
    }

    ///<summary>
    /// Tariff table information of a carrier
    ///</summary>
    public class BTariffInfo
    {
        public string TariffName { get; set; }
        public string TariffType { get; set; }
        public string TariffTable { get; set; }
    }

    ///<summary>
    ///Object to retrieve payment methods for the user
    ///</summary>
    public class BPaymentInfo
    {
        public string PaymentMethod { get; set; }
        public float AvailableCreditLimit { get; set; }
        public string AdminMailID { get; set; }
        public string AdminPhone { get; set; }
        public string UserMailId { get; set; }
    }

    public class BShipmentResult
    {
        public string LabelError { get; set; }
        public BEnumFlag isLabelGenerated { get; set; }
        public string ManifestError { get; set; }
        public BEnumFlag isManifestGenerated { get; set; }
        public string FeasibilityError { get; set; }
        public BEnumFlag isFeasibility { get; set; }
        public string OtherError { get; set; }
        public BEnumFlag isOther { get; set; }
    }

    #region Needs to be removed after implementation
    
    /// <summary>
    /// To collect order details of the current shipment
    /// </summary>
    public class ShipmentOrder
    {
        public string ShipReference { get; set; }
        public BEnumAddressType RecipientType { get; set; }
        public DateTime WishedShipDate { get; set; }
        public int UODCount { get; set; }
        public float TotalWeight { get; set; }
        public BEnumFlag Insured { get; set; }
        public float DeclaredValue { get; set; }
        public string CurrencyType { get; set; }
        public string PackageType { get; set; }
        public string PackageMaterial { get; set; }
        public string ClosingMateiral { get; set; }
        public string SenderCompany { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderEmail { get; set; }
        public string SenderAddress1 { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderAddress3 { get; set; }
        public string SenderCity { get; set; }
        public string SenderZipCode { get; set; }
        public string SenderState { get; set; }
        public string SenderCountry { get; set; }
        public string SenderCollectDeadLine { get; set; }
        public string SenderComments { get; set; }
        public BEnumFlag SameReturnAddress { get; set; }
        public string ReturnCompany { get; set; }
        public string ReturnName { get; set; }
        public string ReturnPhone { get; set; }
        public string ReturnEmail { get; set; }
        public string ReturnAddress1 { get; set; }
        public string ReturnAddress2 { get; set; }
        public string ReturnAddress3 { get; set; }
        public string ReturnCity { get; set; }
        public string ReturnZipCode { get; set; }
        public string ReturnState { get; set; }
        public string ReturnCountry { get; set; }
        public string RecipientCompany { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientAddress1 { get; set; }
        public string RecipientAddress2 { get; set; }
        public string RecipientAddress3 { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientZipCode { get; set; }
        public string RecipientState { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientDeliveryDeadLine { get; set; }
        public string RecipientComments { get; set; }
        public string CustomerInternalReference { get; set; }
        public float CustomsValue { get; set; }
        public BEnumFlag SenderNotification { get; set; }
        public BEnumFlag RecipientNotification { get; set; }
        public string Carrier { get; set; }
        public DateTime OrderCreationTime { get; set; }
        public DateTime ShipDateTime { get; set; }
        public string Status { get; set; }
        public DateTime CalculatedShipDate { get; set; }
        public string CalculatedDeliveryTime { get; set; }
        public float TaxableWeight { get; set; }
        public float FreightAmount { get; set; }
        public float FuelCharge { get; set; }
        public BEnumShipPreference ChosenPreference { get; set; }
        public string AccountNo { get; set; }
        public string Options { get; set; }
        public string OptionsCharges { get; set; }
        public float TotalAmount { get; set; }
        public BEnumOrderType OrderType { get; set; }
        public BEnumPaymentType PaymentType { get; set; }
        public string CancelResponsible { get; set; }
        public List<BShipmentDetails> ShipDetail { get; set; }
        public int ShipGroupID { get; set; }
        public string ContainerType { get; set; }
        public string Surcharge { get; set; }
        public string SurchargeDescription { get; set; }
    }

    /// <summary>
    /// To collect detail of the of a despatch unit
    /// </summary>
    public class ShipmentDetails
    {
        public string ShippingReference { get; set; }
        public int ParcelNo { get; set; }
        public string ContentType { get; set; }
        public string Container { get; set; }
        public float Weight { get; set; }
        public string WeightUnit { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string DimensionUnit { get; set; }
        public float TaxableWeight { get; set; }
        public string TrackingNo { get; set; }
    }

    /// <summary>
    /// To group the shipments for a transaction
    /// </summary>
    public class ShipmentOrderTemp
    {
        public string AccountNo { get; set; }
        public string ShipReference { get; set; }
        public int ShipGroupID { get; set; }
        public string SessionID { get; set; }
        public BEnumFlag OpenGroup { get; set; }
    }

    /// <summary>
    /// Use to retrieve/send information on Quote/Delay.
    /// </summary>
    public class ShipmentQuotation
    {
        public string CarrierPriority { get; set; }
        public string CarrierType { get; set; }
        public string CarrierName { get; set; }
        public string ServiceName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Information { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double CalculatedTariff { get; set; }
        public double FuelSurcharge { get; set; }
        public string SurchargeDescription { get; set; }
        public string Surcharge { get; set; }
        public string OptionsDescription { get; set; }
        public string Options { get; set; }

    }
    #endregion

}
