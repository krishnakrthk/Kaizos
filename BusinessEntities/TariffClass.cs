using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{
    public class BZone
    {
        public int ZoneID { get; set; }
        public string TariffReference { get; set; }
        public string ZoneName { get; set; }
        public string Direction { get; set; }
        public string MasterServiceName { get; set; }
        public string GeographicalCoverage { get; set; }
        public string CountryCode { get; set; }
        public string CoverageList { get; set; }
    }

    public class BZoneSearchDetails
    {
        public int      ZoneID { get; set; }
        public string   CarrierName { get; set; }
        public string   ZoneName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string   Direction { get; set; }
    }

    public class BTariffCreationAcknowledgement
    {
        public string   TariffReference { get; set; }
        public int      CreationStatus  { get; set; }
        public string   Message         { get; set; }
    }

    public class BTariffMaster
    {
        public string CarrierName { get; set; }
        public string TariffReference { get; set; }
        public BEnumTariffType TariffType { get; set; }
        public string ContainerType { get; set; }
        public string KeyUserReference { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BTariffMasterImport
    {
        public string TariffReference { get; set; }
        public string TariffTable { get; set; }
        public List<BTariffCalculationRule> CalculationRule { get; set; }
    }

    public class BTariffDetail
    {
        public float WeightFrom { get; set; }
        public float WeightTo { get; set; }
        public string Zone { get; set; }
        public float PurchaseAmount { get; set; }
        public float CalculatedAmount { get; set; }
        public string ServiceCode { get; set; }
    }

    public class PublicTariff
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ShipType { get; set; }
        public string ServiceType { get; set; }
        public float WeightUpTo30 { get; set; }
        public float WeightAbove30 { get; set; }
    }

    public class AffectedTariff
    {
        public string AccountNo { get; set; }
        public string AssignedTariff { get; set; }
    }

    public class SpecialTariff
    {
        public string AccountNo { get; set; }
        public float WeightFrom { get; set; }
        public float WeightTo { get; set; }
        public string ShipCountry { get; set; }
        public string DeliveryCountry { get; set; }
        public string MasterServiceType { get; set; }
        public string TariffReference { get; set; }
        public float Margin { get; set; }
        public string Carrier { get; set; }
    }

    public class SurchargeMaster
    {
        public string SurchargeCode { get; set; }
        public string SurchargeDescription { get; set; }
        public BEnumSurchargeType SurchargeType { get; set; }
        public string MasterType { get; set; }
        public string Comments { get; set; }
    }

    public class SurchargeDetails
    {
        public string SurchageCode { get; set; }
        public string TariffReference { get; set; }
        public string ServiceName { get; set; }
        public int ParameterCount { get; set; }
        public string ParameterValues { get; set; }
        public string ParameterDescription { get; set; }
    }

    public class BDeliveryDelay
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Delay { get; set; }
    }

    public class BFuelSurcharge
    {
        public int ServiceID { get; set; }
        public string TariffType { get; set; }
        public string KeyAccountReference { get; set; }
        public string MasterServiceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string ServiceName { get; set; }
        public string Reference { get; set; }
    }

    public class BFuelSurchargeParameter
    {
        public int ServiceID { get; set; }
        public string ParameterDescription { get; set; }
        public string ParameterValue { get; set; }
        public int ParameterCount { get; set; }
    }

    public class BMasterServiceType
    {
        public string ServiceTypeName { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public BEnumFlag isBulkShippingAvailable { get; set; }
    }

    public class BTariffCalculationRule
    {
        public string ServiceTypeCode { get; set; }
        public string ZoneList { get; set; }
        public string GrossMargin { get; set; }
    }


    public class BTariffReferenceList
    {
        public string   TariffReference { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Archived { get; set; }
    }

    /**********************  after 20th December ****************************/

    public class BSurchargeMaster
    {
        public string SurchargeCode { get; set; }
        public string SurchargeDescription { get; set; }
        public BEnumSurchargeType SurchargeType { get; set; }
        public string MasterServiceName { get; set; }
        public string Comments { get; set; }
        public string Active { get; set; }
    }

    public class BSurchargeDetails
    {
        public int ServiceID { get; set; }
        public string SurchageCode { get; set; }
        public string TariffReference { get; set; }
        public string ServiceName { get; set; }
        public string ParamID { get; set; }
        public decimal ParamValue { get; set; }
    }

    public class BPublicTariff
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public BEnumPublicTariffType ShipType { get; set; }
        public string MasterServiceName { get; set; }
        public float WeightUpTo30 { get; set; }
        public float WeightAbove30 { get; set; }
    }

    public class BPublicTariffSearchResult
    {
        // Redesigned table and public tariff screen as per YL request during DEMO [15FEB12RM]
        public string Caption { get; set; }
        public string Name  { get; set; }
        public BEnumPublicTariffType ShipType { get; set; }
        public string MasterServiceName { get; set; }
        public int MinWt { get; set; }
        public int MaxWt { get; set; }
        public string WtRangeCaption { get; set; }
        public float Dom { get; set; }
        public float Eur { get; set; }
        public float Int { get; set; }
        public int DispOrder { get; set; }

        //public string Origin { get; set; }
        //public BEnumPublicTariffType ShipType { get; set; }
        //public string MasterServiceName { get; set; }
        //public string WeightRange { get; set; }
        //public float Domestic { get; set; }
        //public float Europe { get; set; }
        //public float International { get; set; }
        //public string Destination { get; set; }
        //public string Name { get; set; }
    }
    
    /********** after 19th Jan HN **********************/

    public class BSimulationHeader
    {
        public string AccountNo { get; set; }
        public string AssignedTariff { get; set; }
        public string SimulationID { get; set; }
        public DateTime Valid { get; set; }
        public float WeightIncrement { get; set; }  //Changed datatype 16FEB12HN
        public float WeightLimit { get; set; }  //Changed datatype 16FEB12HN
    }

    public class BSimulationSurchargeDiscount
    {
        public string AccountNo { get; set; }
        public string SimulationID { get; set; }
        public string CarrierName { get; set; }
        public float SafetyDiscount { get; set; }
        public float FuelDiscount { get; set; }
    }

    public class BSimulationTariff
    {
        public string AccountNo { get; set; }
        public string WeightRange { get; set; }
        public string ShipCountry { get; set; }
        public string DeliveryCountry { get; set; }
        public string MasterServiceName { get; set; }
        public float AverageWeight { get; set; }
        public float ADV { get; set; }
        public float PurchaseFreight { get; set; }
        public float PurchaseTariff { get; set; }
        public string PurchaseCarrier { get; set; }
        public float PublicDiscount { get; set; }
        public string PublicCarrier { get; set; }
        public float PublicFreight { get; set; }
        public float PublicTariff { get; set; }
        public float PublicTurnOver { get; set; }
        public float SaleMargin { get; set; }
        public float SaleFreight { get; set; }
        public float SaleTariff { get; set; }
        public float SaleTurnOver { get; set; }
        public float SaleGrossMargin { get; set; }
        public float ComparisonSaleTariff { get; set; }
        public float ComparisonMargin { get; set; }
        public string SimulationID { get; set; }
        public string PublicSurcharge { get; set; }
        public string ContainerType { get; set; }
    }

    public class BSimulationTariffBased
    {
        public string AccountNo { get; set; }
        public string SimulationID { get; set; }
        public string CarrierName { get; set; }
        public string TariffReference { get; set; }
        public string Assigned { get; set; }
        public string TariffType { get; set; }
    }

    public class BSimulationList
    {
        public string TextField { get; set; }
        public string DataField { get; set; }
    }

    public class BSimulationSubTotal
    {
        public string AccountNo { get; set; }
        public string SimulationID { get; set; }
        public float SubTotalWeight { get; set; }
        public float SubTotalADV { get; set; }
        public float SubTotalFreight { get; set; }
        public float SubTotalPurchase { get; set; }
        public float SubTotalCurrentDiscount { get; set; }
        public float SubTotalPublic { get; set; }
        public float SubTotalCurrentSale { get; set; }
        public float SubTotalTurnOver { get; set; }
        public float SubTotalGrossMargin { get; set; }
        public float SubTotalSalesFretTariff { get; set; }
        public float SubTotalProposedSalesTariff { get; set; }
        public float SubTotalSalesTurnOver { get; set; }
        public float SubTotalSalesGrossMargin { get; set; }
        public float SubtotalCompare { get; set; }
        public float SubTotalCompareMargin { get; set; }

    }
}
