using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{
    public class MasterService
    {
        public string ServiceType { get; set; }
        public BEnumPriority  Priority { get; set; }
        public BEnumFlag BulkShipping { get; set; }
    }

    public class BCarrierMaster
    {
        public int CarrierID { get; set; }
        public string CarrierName { get; set; }
        public BEnumFlag ReferencedCarrier { get; set; }
        public BEnumFlag Active { get; set; }
        public int ClaimDelay { get; set; }
        public string CustomerAccountNoLevel { get; set; }
    }

    public class BCarrierService
    {
        public int ServiceID { get; set; }
        public string CarrierName { get; set; }
        public string MasterServiceName { get; set; }
        public BEnumPriority Priority { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string DeliveryDelayTable { get; set; }
        public string DeliveryDeadLine { get; set; }
        public BEnumFlag Active { get; set; }
        public BEnumFlag KeyCustomerService { get; set; }


        /***********[KS15MAR12]**********/
        public string Information { get; set; }
        public string InfoType { get; set; }
        public string CarrierServiceCode { get; set; }


    }


    //Introduced following entities for KCI0001,KCI0002,KCI0003 [14FEB12RM]
    
    /// <summary>
    /// To return carrier document strings
    /// </summary>
    public class BCarrierProcessResult
    {

        public string Carrier { get; set; }
        public string Output { get; set; }
        public byte[] Result { get; set; }

    }

    /// <summary>
    /// Carrier related parameters
    /// </summary>
    public class BCarriercredentials
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string AppIdFeasability { get; set; }
        public string CountryVersion { get; set; }
        public string CurrencyVersion { get; set; }
        public string PostcodeMaskVersion { get; set; }
        public string TownGroupVersion { get; set; }
        public string ServiceVersion { get; set; }
        public string OptionVersion { get; set; }
        public string RateID { get; set; }
        public string OriginalTownGroup { get; set; }
        public string DestTownGroup { get; set; }
        public string AccountNO { get; set; } //navilibe account no
        public string ServiceType { get; set; }
        public string AppIdLabel { get; set; }
        public string Appverersion { get; set; }
        public string GroupCode { get; set; }
        public string Providance { get; set; }
        public string VATSender { get; set; }
        public string VATReciver { get; set; }
        public string PaymentId { get; set; }
        public string Deliveryinstruction { get; set; }
        public string Description { get; set; }
        public string TrackingType { get; set; }
        public string ExtratrackingDetail1 { get; set; }
        public string ExtratrackingDetail2 { get; set; }
        public string ExtratrackingDetail3 { get; set; }
        public string ExtratrackingDetail4 { get; set; }
        public string ExtratrackingDetail5 { get; set; }
        public string ExtratrackingDetail6 { get; set; }
        public string ContactID { get; set; }
        public string NContactID { get; set; }
        public string EContactID { get; set; }
        public string CustomerID { get; set; }
        public string CountryISOCode { get; set; }
        public string ProductCode { get; set; }
        public string ConsigneeRef { get; set; }
        public string CustomerRefNumber { get; set; }
        public string ServiceandAdditional { get; set; }
        public string ServiceInformation { get; set; }
        public string T852 { get; set; }
        public string GLSoutboundDepot { get; set; }
        public string ConstantZero { get; set; }
        public string Service { get; set; }

    }

    //Introduced following entities [29FEB12KS]

    public class BCarrierOutput
    {
        public string ShippingReference { get; set; }
        public string Carrier { get; set; }
        public string CommubicationNumber { get; set; }
        public byte[] LabelByte { get; set; }
        public string Label { get; set; }
        public string Manifest { get; set; }
        public string Invoice { get; set; }
    }


}
