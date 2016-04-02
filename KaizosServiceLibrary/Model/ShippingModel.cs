using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
    [DataContract]
    public class SShipmentOrder
    {
        [DataMember]
        public string ShipReference { get; set; }
        [DataMember]
        public SEnumAddressType RecipientType { get; set; }
        [DataMember]
        public DateTime WishedShipDate { get; set; }
        [DataMember]
        public int UODCount { get; set; }
        [DataMember]
        public float TotalWeight { get; set; }
        [DataMember]
        public SEnumFlag Insured { get; set; }
        [DataMember]
        public float DeclaredValue { get; set; }
        [DataMember]
        public string CurrencyType { get; set; }
        [DataMember]
        public string PackageType { get; set; }
        [DataMember]
        public string PackageMaterial { get; set; }
        [DataMember]
        public string ClosingMateiral { get; set; }
        [DataMember]
        public string SenderCompany { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderEmail { get; set; }
        [DataMember]
        public string SenderAddress1 { get; set; }
        [DataMember]
        public string SenderAddress2 { get; set; }
        [DataMember]
        public string SenderAddress3 { get; set; }
        [DataMember]
        public string SenderCity { get; set; }
        [DataMember]
        public string SenderZipCode { get; set; }
        [DataMember]
        public string SenderState { get; set; }
        [DataMember]
        public string SenderCountry { get; set; }
        [DataMember]
        public string SenderCollectDeadLine { get; set; }
        [DataMember]
        public string SenderComments { get; set; }
        [DataMember]
        public SEnumFlag SameReturnAddress { get; set; }
        [DataMember]
        public string ReturnCompany { get; set; }
        [DataMember]
        public string ReturnName { get; set; }
        [DataMember]
        public string ReturnPhone { get; set; }
        [DataMember]
        public string ReturnEmail { get; set; }
        [DataMember]
        public string ReturnAddress1 { get; set; }
        [DataMember]
        public string ReturnAddress2 { get; set; }
        [DataMember]
        public string ReturnAddress3 { get; set; }
        [DataMember]
        public string ReturnCity { get; set; }
        [DataMember]
        public string ReturnZipCode { get; set; }
        [DataMember]
        public string ReturnState { get; set; }
        [DataMember]
        public string ReturnCountry { get; set; }
        [DataMember]
        public string RecipientCompany { get; set; }
        [DataMember]
        public string RecipientName { get; set; }
        [DataMember]
        public string RecipientPhone { get; set; }
        [DataMember]
        public string RecipientEmail { get; set; }
        [DataMember]
        public string RecipientAddress1 { get; set; }
        [DataMember]
        public string RecipientAddress2 { get; set; }
        [DataMember]
        public string RecipientAddress3 { get; set; }
        [DataMember]
        public string RecipientCity { get; set; }
        [DataMember]
        public string RecipientZipCode { get; set; }
        [DataMember]
        public string RecipientState { get; set; }
        [DataMember]
        public string RecipientCountry { get; set; }
        [DataMember]
        public string RecipientDeliveryDeadLine { get; set; }
        [DataMember]
        public string RecipientComments { get; set; }
        [DataMember]
        public string CustomerInternalReference { get; set; }
        [DataMember]
        public float CustomsValue { get; set; }
        [DataMember]
        public SEnumFlag SenderNotification { get; set; }
        [DataMember]
        public SEnumFlag RecipientNotification { get; set; }
        [DataMember]
        public string Carrier { get; set; }
        [DataMember]
        public DateTime OrderCreationTime { get; set; }
        [DataMember]
        public DateTime ShipDateTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime CalculatedShipDate { get; set; }
        [DataMember]
        public string CalculatedDeliveryTime { get; set; }
        [DataMember]
        public float TaxableWeight { get; set; }
        [DataMember]
        public float FreightAmount { get; set; }
        [DataMember]
        public float FuelCharge { get; set; }
        [DataMember]
        public SEnumShipPreference ChosenPreference { get; set; }
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string Options { get; set; }
        [DataMember]
        public string OptionsCharges { get; set; }
        [DataMember]
        public float TotalAmount { get; set; }
        [DataMember]
        public SEnumOrderType OrderType { get; set; }
        [DataMember]
        public SEnumPaymentType PaymentType { get; set; }
        [DataMember]
        public string CancelResponsible { get; set; }
        [DataMember]
        public List<SShipmentDetails> ShipDetail { get; set; }
        [DataMember]
        public int ShipGroupID { get; set; }
        [DataMember]
        public string ContainerType { get; set; }
        [DataMember]
        public string Surcharge { get; set; }
        [DataMember]
        public string SurchargeDescription { get; set; }
        [DataMember]
        public SShipmentResult ShipmentResult { get; set; }

        /***********[KS15MAR12]**********/
        [DataMember]
        public string CarrierService { get; set; }
        /***********[KS23APR12]**********/
        [DataMember]
        public string CalculatedInsuranceAmount { get; set; }
    }

    [DataContract]
    public class SShipmentDetails
    {
        [DataMember]
        public string ShippingReference { get; set; }
        [DataMember]
        public int ParcelNo { get; set; }
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public string Container { get; set; }
        [DataMember]
        public float Weight { get; set; }
        [DataMember]
        public string WeightUnit { get; set; }
        [DataMember]
        public float Length { get; set; }
        [DataMember]
        public float Width { get; set; }
        [DataMember]
        public float Height { get; set; }
        [DataMember]
        public string DimensionUnit { get; set; }
        [DataMember]
        public float TaxableWeight { get; set; }
        [DataMember]
        public string TrackingNo { get; set; }
    }

    [DataContract]
    public class SShipmentOrderTemp
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string ShipReference { get; set; }
        [DataMember]
        public int ShipGroupID { get; set; }
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public SEnumFlag OpenGroup { get; set; }
    }

    [DataContract]
    public class SShipmentQuotation
    {
        [DataMember]
        public string CarrierPriority { get; set; }
        [DataMember]
        public string CarrierType { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public DateTime ShippingDate { get; set; }
        [DataMember]
        public string Information { get; set; }
        [DataMember]
        public DateTime DeliveryDate { get; set; }
        [DataMember]
        public double CalculatedTariff { get; set; }
        [DataMember]
        public double FuelSurcharge { get; set; }
        [DataMember]
        public string SurchargeDescription { get; set; }
        [DataMember]
        public string Surcharge { get; set; }
        [DataMember]
        public string OptionsDescription { get; set; }
        [DataMember]
        public string Options { get; set; }

        /***********[KS15MAR12]**********/
        [DataMember]
        public string InfoType { get; set; }


    }

    [DataContract]
    public class STariffName
    {
        [DataMember]
        public string AffectedTariff { get; set; }
    }

    [DataContract]
    public class STariffInfo
    {
        [DataMember]
        public string TariffName { get; set; }
        [DataMember]
        public string TariffType { get; set; }
        [DataMember]
        public string TariffTable { get; set; }
    }

    [DataContract]
    public class SPaymentInfo
    {
        [DataMember]
        public string PaymentMethod { get; set; }
        [DataMember]
        public float AvailableCreditLimit { get; set; }
        [DataMember]
        public string AdminMailID { get; set; }
        [DataMember]
        public string AdminPhone { get; set; }
        [DataMember]
        public string UserMailId { get; set; }
    }

    [DataContract]
    public class SShipmentResult
    {
        [DataMember]
        public string LabelError { get; set; }
        [DataMember]
        public SEnumFlag isLabelGenerated { get; set; }
        [DataMember]
        public string ManifestError { get; set; }
        [DataMember]
        public SEnumFlag isManifestGenerated { get; set; }
        [DataMember]
        public string FeasibilityError { get; set; }
        [DataMember]
        public SEnumFlag isFeasibility { get; set; }
        [DataMember]
        public string OtherError { get; set; }
        [DataMember]
        public SEnumFlag isOther { get; set; }

    }
}
  