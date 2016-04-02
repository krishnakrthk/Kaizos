using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kaizos.Entities.Business;
          
namespace KaizosServiceLibrary.Model
{
    [DataContract]
    public class SMasterServiceType
    {
        [DataMember]
        public string ServiceTypeName { get; set; }
        
        [DataMember]
        public string Priority { get; set; }
        
        [DataMember]
        public string Type { get; set; }
        
        [DataMember]
        public SEnumFlag isBulkShippingAvailable { get; set; }

    }

    [DataContract]
    public class SFuelSurcharge
    {
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string TariffType { get; set; }
        [DataMember]
        public string KeyAccountReference { get; set; }
        [DataMember]
        public string MasterServiceName { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime LastUpdate { get; set; }
        [DataMember]
        public string ParameterDescription { get; set; }
        [DataMember]
        public string ParameterValue { get; set; }
        [DataMember]
        public int ParameterCount { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string Reference { get; set; }
    }

    [DataContract]
    public class SFuelSurchargeParameter
    {
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ParameterDescription { get; set; }
        [DataMember]
        public string ParameterValue { get; set; }
        [DataMember]
        public int ParameterCount { get; set; }
    }

    
    /**********************  after 13th December ****************************/

    [DataContract]
    public class STariffMaster
    {
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string TariffReference { get; set; }
        [DataMember]
        public SEnumTariffType TariffType { get; set; }
        [DataMember]
        public string ContainerType { get; set; }
        [DataMember]
        public string KeyUserReference { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
    }

    [DataContract]
    public class STariffCreationAcknowledgement
    {
        [DataMember]
        public string TariffReference { get; set; }
        [DataMember]
        public int CreationStatus { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class STariffCalculationRule
    {
        [DataMember]
        public string ServiceTypeCode { get; set; }
        [DataMember]
        public string ZoneList { get; set; }
        [DataMember]
        public string GrossMargin { get; set; }
    }

    [DataContract]
    public class SFileImportStatus
    {
        [DataMember]
        public int RowNumber { get; set; }
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string ErrorDescription { get; set; }
    }

    [DataContract]
    public class STariffReferenceList
    {
        [DataMember]
        public string TariffReference   { get; set; }
        [DataMember]
        public DateTime StartDate       { get; set; }
        [DataMember]
        public DateTime EndDate         { get; set; }
        [DataMember]
        public bool Archived            { get; set; }
    }

    [DataContract]
    public class SZone
    {
        [DataMember]
        public int ZoneID { get; set; }
        [DataMember]
        public string TariffReference       { get; set; }
        [DataMember]
        public string ZoneName              { get; set; }
        [DataMember]
        public string Direction             { get; set; }
        [DataMember]
        public string MasterServiceName     { get; set; }
        [DataMember]
        public string GeographicalCoverage  { get; set; }
        [DataMember]
        public string CountryCode           { get; set; }
        [DataMember]
        public string CoverageList          { get; set; }
    }

    [DataContract]
    public class SZoneSearchDetails
    {
        [DataMember]
        public int ZoneID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ZoneName { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public string Direction { get; set; }
    }


    /**********************  after 20th December ****************************/
    [DataContract]
    public class SSurchargeMaster
    {
        [DataMember]
        public string SurchargeCode { get; set; }
        [DataMember]
        public string SurchargeDescription { get; set; }
        [DataMember]
        public SEnumSurchargeType SurchargeType { get; set; }
        [DataMember]
        public string MasterServiceName { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string Active { get; set; }
    }

    [DataContract]
    public class SSurchargeDetails
    {
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string SurchageCode { get; set; }
        [DataMember]
        public string TariffReference { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ParamID { get; set; }
        [DataMember]
        public decimal ParamValue { get; set; }
    }


    //Recontructed below entity as per new table PUBLIC_TARIFF [15FEB12RM]
    [DataContract]
    public class SPublicTariffSearchResult
    {
        //[DataMember]
        //public string Origin { get; set; }
        //[DataMember]
        //public SEnumPublicTariffType ShipType { get; set; }
        //[DataMember]
        //public string MasterServiceName { get; set; }
        //[DataMember]
        //public string WeightRange { get; set; }
        //[DataMember]
        //public float Domestic { get; set; }
        //[DataMember]
        //public float Europe { get; set; }
        //[DataMember]
        //public float International { get; set; }
        //[DataMember]
        //public string Destination { get; set; }
        //[DataMember]
        //public string Name { get; set; }

        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public SEnumPublicTariffType ShipType { get; set; }
        [DataMember]
        public string MasterServiceName { get; set; }
        [DataMember]
        public int MinWt { get; set; }
        [DataMember]
        public int MaxWt { get; set; }
        [DataMember]
        public string WtRangeCaption { get; set; }
        [DataMember]
        public float Dom { get; set; }
        [DataMember]
        public float Eur { get; set; }
        [DataMember]
        public float Int { get; set; }
        [DataMember]
        public int DispOrder { get; set; }

    }

    /*************** 19th Jan HN *******************************/

    [DataContract]
    public class SSimulationHeader
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string AssignedTariff { get; set; }
        [DataMember]
        public string SimulationID { get; set; }
        [DataMember]
        public DateTime Valid { get; set; }
        [DataMember]
        public float WeightIncrement { get; set; } // converted data type 16FEB12HN
        [DataMember]
        public float WeightLimit { get; set; } // converted data type 16FEB12HN
    }

    [DataContract]
    public class SSimulationSurchargeDiscount
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string SimulationID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public float SafetyDiscount { get; set; }
        [DataMember]
        public float FuelDiscount { get; set; }
    }

    [DataContract]
    public class SSimulationTariffBased
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string SimulationID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string TariffReference { get; set; }
        [DataMember]
        public string Assigned { get; set; }
        [DataMember]
        public string TariffType { get; set; }
    }

    [DataContract]
    public class SSimulationTariff
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string WeightRange { get; set; }
        [DataMember]
        public string ShipCountry { get; set; }
        [DataMember]
        public string DeliveryCountry { get; set; }
        [DataMember]
        public string MasterServiceName { get; set; }
        [DataMember]
        public float AverageWeight { get; set; }
        [DataMember]
        public float ADV { get; set; }
        [DataMember]
        public float PurchaseFreight { get; set; }
        [DataMember]
        public float PurchaseTariff { get; set; }
        [DataMember]
        public string PurchaseCarrier { get; set; }
        [DataMember]
        public float PublicDiscount { get; set; }
        [DataMember]
        public string PublicCarrier { get; set; }
        [DataMember]
        public float PublicFreight { get; set; }
        [DataMember]
        public float PublicTariff { get; set; }
        [DataMember]
        public float PublicTurnOver { get; set; }
        [DataMember]
        public float SaleMargin { get; set; }
        [DataMember]
        public float SaleFreight { get; set; }
        [DataMember]
        public float SaleTariff { get; set; }
        [DataMember]
        public float SaleTurnOver { get; set; }
        [DataMember]
        public float SaleGrossMargin { get; set; }
        [DataMember]
        public float ComparisonSaleTariff { get; set; }
        [DataMember]
        public float ComparisonMargin { get; set; }
        [DataMember]
        public string SimulationID { get; set; }
        [DataMember]
        public string PublicSurcharge { get; set; }
        [DataMember]
        public string ContainerType { get; set; }
    }

    [DataContract]
    public class SSimulationList
    {
        [DataMember]
        public string TextField { get; set; }
        [DataMember]
        public string DataField { get; set; }
    }

    [DataContract]
    public class SSimulationSubTotal
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string SimulationID { get; set; }
        [DataMember]
        public float SubTotalWeight { get; set; }
        [DataMember]
        public float SubTotalADV { get; set; }
        [DataMember]
        public float SubTotalFreight { get; set; }
        [DataMember]
        public float SubTotalPurchase { get; set; }
        [DataMember]
        public float SubTotalCurrentDiscount { get; set; }
        [DataMember]
        public float SubTotalPublic { get; set; }
        [DataMember]
        public float SubTotalCurrentSale { get; set; }
        [DataMember]
        public float SubTotalTurnOver { get; set; }
        [DataMember]
        public float SubTotalGrossMargin { get; set; }
        [DataMember]
        public float SubTotalSalesFretTariff { get; set; }
        [DataMember]
        public float SubTotalProposedSalesTariff { get; set; }
        [DataMember]
        public float SubTotalSalesTurnOver { get; set; }
        [DataMember]
        public float SubTotalSalesGrossMargin { get; set; }
        [DataMember]
        public float SubtotalCompare { get; set; }
        [DataMember]
        public float SubTotalCompareMargin { get; set; }

    }
}
