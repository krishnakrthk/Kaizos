using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kaizos.Entities.Business; //reference to Business entities
using KaizosServiceLibrary.Model; //reference to Service entities


namespace KaizosServiceLibrary.Adapter
{
    class TariffAdapter
    {

        public BMasterServiceType ConvertStoB_MasterServiceType(SMasterServiceType sMasterServiceType)
        {
            BMasterServiceType bMasterServiceType = new BMasterServiceType();
            
            bMasterServiceType.ServiceTypeName = sMasterServiceType.ServiceTypeName;
            bMasterServiceType.Type = sMasterServiceType.Type;
            bMasterServiceType.Priority = sMasterServiceType.Priority;
            
            bMasterServiceType.isBulkShippingAvailable = BEnumFlag.No;

            if (sMasterServiceType.isBulkShippingAvailable == SEnumFlag.Yes)
                bMasterServiceType.isBulkShippingAvailable = BEnumFlag.Yes;

            return bMasterServiceType;
        }


        public SMasterServiceType ConvertBtoS_MasterServiceType(BMasterServiceType bMasterServiceType)
        {
            SMasterServiceType sMasterServiceType = new SMasterServiceType();

            sMasterServiceType.ServiceTypeName = sMasterServiceType.ServiceTypeName;
            sMasterServiceType.Type = sMasterServiceType.Type;
            sMasterServiceType.Priority = sMasterServiceType.Priority;

            sMasterServiceType.isBulkShippingAvailable = SEnumFlag.No;

            if (bMasterServiceType.isBulkShippingAvailable == BEnumFlag.Yes)
                sMasterServiceType.isBulkShippingAvailable = SEnumFlag.Yes;

            return sMasterServiceType;
        }


        /// <summary>
        /// Return Business to Service entity of FuelCharge.
        /// </summary>
        /// <param name="bFuelSurcharge"></param>
        /// <returns></returns>
        public SFuelSurcharge ConvertBtoS_FuelCharges(BFuelSurcharge bFuelSurcharge)
        {

            SFuelSurcharge sFuelSurcharge = new SFuelSurcharge();

            if (bFuelSurcharge != null)
            {
                sFuelSurcharge.ServiceID = bFuelSurcharge.ServiceID;
                sFuelSurcharge.TariffType = bFuelSurcharge.TariffType;
                sFuelSurcharge.KeyAccountReference = bFuelSurcharge.KeyAccountReference;
                sFuelSurcharge.MasterServiceName = bFuelSurcharge.MasterServiceName;
                sFuelSurcharge.StartDate = bFuelSurcharge.StartDate;
                sFuelSurcharge.LastUpdate = bFuelSurcharge.LastUpdate;
                sFuelSurcharge.ServiceName = bFuelSurcharge.ServiceName;
                sFuelSurcharge.Reference = bFuelSurcharge.Reference;
            }
            return sFuelSurcharge;
            
        }

        /// <summary>
        ///  Return list of Business to Service entity of FuelCharge.
        /// </summary>
        /// <param name="bFuelSurcharge"></param>
        /// <returns></returns>
        public List<SFuelSurcharge> ConvertBtoS_FuelCharges(List<BFuelSurcharge> bFuelSurcharge)
        {

            List<SFuelSurcharge> sFuelSurcharge = new List<SFuelSurcharge>();


            if (bFuelSurcharge != null)
            {
                for (int i = 0; i < bFuelSurcharge.Count; i++)
                {
                    sFuelSurcharge.Add(ConvertBtoS_FuelCharges(bFuelSurcharge[i]));
                }
            }

            return sFuelSurcharge;

        }

        public SFuelSurchargeParameter ConvertBtoS_FuelChargesParameters(BFuelSurchargeParameter bFuelSurchargeParameter)
        {
            SFuelSurchargeParameter sFuelSurchargeParameter = new SFuelSurchargeParameter();

            if (bFuelSurchargeParameter != null)
            {
                sFuelSurchargeParameter.ServiceID= bFuelSurchargeParameter.ServiceID;
                sFuelSurchargeParameter.ParameterDescription = bFuelSurchargeParameter.ParameterDescription;
                sFuelSurchargeParameter.ParameterValue = bFuelSurchargeParameter.ParameterValue;
            }
            return sFuelSurchargeParameter;
        }
        public List<SFuelSurchargeParameter> ConvertBtoS_FuelChargesParameters(List<BFuelSurchargeParameter> bFuelSurchargeParameter)
        {

            List<SFuelSurchargeParameter> sFuelSurchargeParameter = new List<SFuelSurchargeParameter>();


            if (bFuelSurchargeParameter != null)
            {
                for (int i = 0; i < bFuelSurchargeParameter.Count; i++)
                {
                    sFuelSurchargeParameter.Add(ConvertBtoS_FuelChargesParameters(bFuelSurchargeParameter[i]));
                }
            }

            return sFuelSurchargeParameter;

        }


        public BFuelSurchargeParameter ConvertStoB_FuelChargesParameters(SFuelSurchargeParameter sFuelSurchargeParameter)
        {
            BFuelSurchargeParameter bFuelSurchargeParameter = new BFuelSurchargeParameter();

            if (bFuelSurchargeParameter != null)
            {
                bFuelSurchargeParameter.ServiceID = sFuelSurchargeParameter.ServiceID;
                bFuelSurchargeParameter.ParameterDescription = sFuelSurchargeParameter.ParameterDescription;
                bFuelSurchargeParameter.ParameterValue = sFuelSurchargeParameter.ParameterValue;
            }
            return bFuelSurchargeParameter;
        }
        public List<BFuelSurchargeParameter> ConvertStoB_FuelChargesParameters(List<SFuelSurchargeParameter> sFuelSurchargeParameter)
        {

            List<BFuelSurchargeParameter> bFuelSurchargeParameter = new List<BFuelSurchargeParameter>();


            if (sFuelSurchargeParameter != null)
            {
                for (int i = 0; i < sFuelSurchargeParameter.Count; i++)
                {
                    bFuelSurchargeParameter.Add(ConvertStoB_FuelChargesParameters(sFuelSurchargeParameter[i]));
                }
            }
            return bFuelSurchargeParameter;
        }


        /**********************  after 13th December ****************************/

        private BEnumTariffType GetTariffType(SEnumTariffType sEnumTariffType)
        {
            BEnumTariffType bEnumTariffType = BEnumTariffType.CarrierPublic; //Just defaulted

            if (SEnumTariffType.CarrierPublic == sEnumTariffType)
                bEnumTariffType = BEnumTariffType.CarrierPublic;
            else if (SEnumTariffType.Purchase == sEnumTariffType)
                bEnumTariffType = BEnumTariffType.Purchase;
            else if (SEnumTariffType.Negotiated == sEnumTariffType)
                bEnumTariffType = BEnumTariffType.Negotiated;

            return bEnumTariffType;
        }

        public BTariffMaster ConvertStoB_TariffMaster(STariffMaster sTariffMaster)
        {
            BTariffMaster bTariffMaster = new BTariffMaster();

            bTariffMaster.CarrierName       =   sTariffMaster.CarrierName;
            bTariffMaster.TariffReference   =   sTariffMaster.TariffReference;
            bTariffMaster.TariffType        =   GetTariffType(sTariffMaster.TariffType);
            bTariffMaster.ContainerType     =   sTariffMaster.ContainerType;
            bTariffMaster.KeyUserReference  =   sTariffMaster.KeyUserReference;
            bTariffMaster.StartDate         =   sTariffMaster.StartDate;
            bTariffMaster.EndDate           =   sTariffMaster.EndDate;

            return bTariffMaster;
        }

        public STariffCreationAcknowledgement ConvertBtoS_TariffCreationAck(BTariffCreationAcknowledgement bTariffCreationAck)
        {
            STariffCreationAcknowledgement sTariffCreationAck = new STariffCreationAcknowledgement();
            sTariffCreationAck.CreationStatus   = bTariffCreationAck.CreationStatus;
            sTariffCreationAck.Message          = bTariffCreationAck.Message;
            sTariffCreationAck.TariffReference  = bTariffCreationAck.TariffReference;
            return sTariffCreationAck;
        }

        public BTariffCalculationRule ConvertStoB_TariffCalculationRule(STariffCalculationRule sTariffCalculationRule)
        {
            BTariffCalculationRule bTariffCalculationRule = new BTariffCalculationRule();
            bTariffCalculationRule.ServiceTypeCode  = sTariffCalculationRule.ServiceTypeCode.Trim();
            bTariffCalculationRule.ZoneList         = sTariffCalculationRule.ZoneList.Trim();
            bTariffCalculationRule.GrossMargin      = sTariffCalculationRule.GrossMargin.Trim();
            return bTariffCalculationRule;
        }
        public List<BTariffCalculationRule> ConvertStoB_TariffCalculationRule(List<STariffCalculationRule> sTariffCalculationRule)
        {
            List<BTariffCalculationRule> bTariffCalculationRule = new List<BTariffCalculationRule>();

            if (sTariffCalculationRule != null)
            {
                for (int i = 0; i < sTariffCalculationRule.Count; i++)
                {
                    bTariffCalculationRule.Add(ConvertStoB_TariffCalculationRule(sTariffCalculationRule[i]));
                }
            }
            return bTariffCalculationRule;
        }


        public STariffCalculationRule ConvertBtoS_TariffCalculationRule(BTariffCalculationRule bTariffCalculationRule)
        {
            STariffCalculationRule sTariffCalculationRule = new STariffCalculationRule();
            sTariffCalculationRule.ServiceTypeCode  = bTariffCalculationRule.ServiceTypeCode.Trim();
            sTariffCalculationRule.ZoneList         = bTariffCalculationRule.ZoneList.Trim();
            sTariffCalculationRule.GrossMargin      = bTariffCalculationRule.GrossMargin.Trim();
            return sTariffCalculationRule;
        }

        public List<STariffCalculationRule> ConvertBtoS_TariffCalculationRule(List<BTariffCalculationRule> bTariffCalculationRule)
        {
            List<STariffCalculationRule> sTariffCalculationRule = new List<STariffCalculationRule>();

            if (bTariffCalculationRule != null)
            {
                for (int i = 0; i < bTariffCalculationRule.Count; i++)
                {
                    sTariffCalculationRule.Add(ConvertBtoS_TariffCalculationRule(bTariffCalculationRule[i]));
                }
            }
            return sTariffCalculationRule;
        }


        public SFileImportStatus ConvertBtoS_FileImportStatus(BFileImportStatus bFileImportStatus)
        {
            SFileImportStatus sFileImportStatus = new SFileImportStatus();
            sFileImportStatus.RowNumber         = bFileImportStatus.RowNumber;
            sFileImportStatus.FieldName         = bFileImportStatus.FieldName;
            sFileImportStatus.ErrorDescription  = bFileImportStatus.ErrorDescription;
            return sFileImportStatus;
        }

        public List<SFileImportStatus> ConvertBtoS_FileImportStatus(List<BFileImportStatus> bFileImportStatus)
        {
            List<SFileImportStatus> sFileImportStatus = new List<SFileImportStatus>();

            if (bFileImportStatus != null)
            {
                for (int i = 0; i < bFileImportStatus.Count; i++)
                {
                    sFileImportStatus.Add(ConvertBtoS_FileImportStatus(bFileImportStatus[i]));
                }
            }
            return sFileImportStatus;
        }

        public STariffReferenceList ConvertBtoS_TariffReferenceList(BTariffReferenceList bTariffReferenceList)
        {
            STariffReferenceList sTariffReferenceList = new STariffReferenceList();

            sTariffReferenceList.TariffReference    = bTariffReferenceList.TariffReference;
            sTariffReferenceList.StartDate          = bTariffReferenceList.StartDate;
            sTariffReferenceList.EndDate            = bTariffReferenceList.EndDate;
            sTariffReferenceList.Archived           = bTariffReferenceList.Archived;
            
            return sTariffReferenceList;
        }

        public List<STariffReferenceList> ConvertBtoS_TariffReferenceList(List<BTariffReferenceList> bTariffReferenceList)
        {
            List<STariffReferenceList> sTariffReferenceList = new List<STariffReferenceList>();

            if (bTariffReferenceList != null)
            {
                for (int i = 0; i < bTariffReferenceList.Count; i++)
                {
                    sTariffReferenceList.Add(ConvertBtoS_TariffReferenceList(bTariffReferenceList[i]));
                }
            }
            return sTariffReferenceList;
        }

        public BZone ConvertStoB_Zone(SZone sZone)
        {
            BZone bZone = new BZone();
            bZone.ZoneID                    = sZone.ZoneID;
            bZone.TariffReference           = sZone.TariffReference;
            bZone.ZoneName                  = sZone.ZoneName;
            bZone.Direction                 = sZone.Direction;
            bZone.MasterServiceName         = sZone.MasterServiceName;
            bZone.GeographicalCoverage      = sZone.GeographicalCoverage;
            bZone.CoverageList              = sZone.CoverageList;
            bZone.CountryCode               = sZone.CountryCode;

            return bZone;
        }

        public SZone ConvertBtoS_Zone(BZone bZone)
        {
            SZone sZone = new SZone();
            if (bZone != null)
            {
                sZone.ZoneID = bZone.ZoneID;
                sZone.TariffReference = bZone.TariffReference;
                sZone.ZoneName = bZone.ZoneName;
                sZone.Direction = bZone.Direction;
                sZone.MasterServiceName = bZone.MasterServiceName;
                sZone.GeographicalCoverage = bZone.GeographicalCoverage;
                sZone.CoverageList = bZone.CoverageList;
                sZone.CountryCode = bZone.CountryCode;
                

            }
            return sZone;
        }

        public List<SZone> ConvertBtoS_Zone(List<BZone> bZone)
        {
            List<SZone> sZoneList = new List<SZone>();

            if (bZone != null)
            {
                for (int i = 0; i < bZone.Count; i++)
                {
                    sZoneList.Add(ConvertBtoS_Zone(bZone[i]));
                }
            }
            return sZoneList;
        }

        public SZoneSearchDetails ConvertBtoS_ZoneSearchDetails(BZoneSearchDetails bZoneSearchDetails)
        {
            SZoneSearchDetails sZoneSearchDetails = new SZoneSearchDetails();
            sZoneSearchDetails.ZoneID       = bZoneSearchDetails.ZoneID;
            sZoneSearchDetails.CarrierName  = bZoneSearchDetails.CarrierName;
            sZoneSearchDetails.ZoneName     = bZoneSearchDetails.ZoneName;
            sZoneSearchDetails.Direction    = bZoneSearchDetails.Direction;
            sZoneSearchDetails.StartDate    = bZoneSearchDetails.StartDate;
            sZoneSearchDetails.EndDate      = bZoneSearchDetails.EndDate;

            return sZoneSearchDetails;
        }

        public List<SZoneSearchDetails> ConvertBtoS_ZoneSearchDetails(List<BZoneSearchDetails> bZoneSearchDetails)
        {
            List<SZoneSearchDetails> sZoneSearchDetails = new List<SZoneSearchDetails>();

            if (bZoneSearchDetails != null)
            {
                for (int i = 0; i < bZoneSearchDetails.Count; i++)
                {
                    sZoneSearchDetails.Add(ConvertBtoS_ZoneSearchDetails(bZoneSearchDetails[i]));
                }
            }
            return sZoneSearchDetails;
        }

        /**********************  after 20th December ****************************/

        public SSurchargeMaster ConvertBtoS_SurchargeMaster(BSurchargeMaster bSurchargeMaster)
        {
            SSurchargeMaster sSurchargeMaster = new SSurchargeMaster();
            sSurchargeMaster.SurchargeCode = bSurchargeMaster.SurchargeCode;
            sSurchargeMaster.SurchargeDescription = bSurchargeMaster.SurchargeDescription;
            sSurchargeMaster.SurchargeType = (bSurchargeMaster.SurchargeType == BEnumSurchargeType.O) ? SEnumSurchargeType.O : SEnumSurchargeType.S;
            sSurchargeMaster.MasterServiceName = bSurchargeMaster.MasterServiceName;
            sSurchargeMaster.Active = bSurchargeMaster.Active;

            return sSurchargeMaster;
        }

        public List<SSurchargeMaster> ConvertBtoS_SurchargeMaster(List<BSurchargeMaster> bSurchargeMaster)
        {
            List<SSurchargeMaster> sSurchargeMaster = new List<SSurchargeMaster>();

            if (bSurchargeMaster != null)
            {
                for (int i = 0; i < bSurchargeMaster.Count; i++)
                {
                    sSurchargeMaster.Add(ConvertBtoS_SurchargeMaster(bSurchargeMaster[i]));
                }
            }
            return sSurchargeMaster;
        }

        public SSurchargeDetails ConvertBtoS_SurchargeDetail(BSurchargeDetails bSurchargeDetails)
        {
            SSurchargeDetails sSurchargeDetails = new SSurchargeDetails();

            sSurchargeDetails.ServiceID = bSurchargeDetails.ServiceID;
            sSurchargeDetails.SurchageCode = bSurchargeDetails.SurchageCode;
            sSurchargeDetails.TariffReference = bSurchargeDetails.TariffReference;
            sSurchargeDetails.ServiceName = bSurchargeDetails.ServiceName;
            sSurchargeDetails.ParamID = bSurchargeDetails.ParamID;
            sSurchargeDetails.ParamValue = bSurchargeDetails.ParamValue;
            return sSurchargeDetails;
        }

        public List<SSurchargeDetails> ConvertBtoS_SurchargeDetail(List<BSurchargeDetails> bSurchargeDetails)
        {
            List<SSurchargeDetails> sSurchargeDetails = new List<SSurchargeDetails>();

            if (bSurchargeDetails != null)
            {
                for (int i = 0; i < bSurchargeDetails.Count; i++)
                {
                    sSurchargeDetails.Add(ConvertBtoS_SurchargeDetail(bSurchargeDetails[i]));
                }
            }
            return sSurchargeDetails;
        }

        public BSurchargeDetails ConvertStoB_SurchargeDetail(SSurchargeDetails sSurchargeDetails)
        {
            BSurchargeDetails bSurchargeDetails = new BSurchargeDetails();

            bSurchargeDetails.ServiceID = sSurchargeDetails.ServiceID;
            bSurchargeDetails.SurchageCode = sSurchargeDetails.SurchageCode;
            bSurchargeDetails.TariffReference = sSurchargeDetails.TariffReference;
            bSurchargeDetails.ServiceName = sSurchargeDetails.ServiceName;
            bSurchargeDetails.ParamID = sSurchargeDetails.ParamID;
            bSurchargeDetails.ParamValue = sSurchargeDetails.ParamValue;
            return bSurchargeDetails;
        }

        public List<BSurchargeDetails> ConvertStoB_SurchargeDetail(List<SSurchargeDetails> sSurchargeDetails)
        {
            List<BSurchargeDetails> bSurchargeDetails = new List<BSurchargeDetails>();

            if (sSurchargeDetails != null)
            {
                for (int i = 0; i < sSurchargeDetails.Count; i++)
                {
                    bSurchargeDetails.Add(ConvertStoB_SurchargeDetail(sSurchargeDetails[i]));
                }
            }
            return bSurchargeDetails;
        }

        private SEnumPublicTariffType ConvertBtoS_PublicTariffShipType(BEnumPublicTariffType bEnumPublicTariffType)
        {
            SEnumPublicTariffType sEnumPublicTariffType = new SEnumPublicTariffType();

            if (bEnumPublicTariffType == BEnumPublicTariffType.Domestic)
                sEnumPublicTariffType = SEnumPublicTariffType.Domestic;
            else if (bEnumPublicTariffType == BEnumPublicTariffType.Export)
                sEnumPublicTariffType = SEnumPublicTariffType.Export;
            else if (bEnumPublicTariffType == BEnumPublicTariffType.Import)
                sEnumPublicTariffType = SEnumPublicTariffType.Import;

            return sEnumPublicTariffType;
        }

        private BEnumPublicTariffType ConvertStoB_PublicTariffShipType(SEnumPublicTariffType sEnumPublicTariffType)
        {
            BEnumPublicTariffType bEnumPublicTariffType = new BEnumPublicTariffType();

            if (sEnumPublicTariffType == SEnumPublicTariffType.Domestic)
                bEnumPublicTariffType = BEnumPublicTariffType.Domestic;
            else if (sEnumPublicTariffType == SEnumPublicTariffType.Export)
                bEnumPublicTariffType = BEnumPublicTariffType.Export;
            else if (sEnumPublicTariffType == SEnumPublicTariffType.Import)
                bEnumPublicTariffType = BEnumPublicTariffType.Import;

            return bEnumPublicTariffType;
        }

        public SPublicTariffSearchResult ConvertBtoS_PublicTariffSearchResult(BPublicTariffSearchResult bPublicTariffSearchResult)
        {

            SPublicTariffSearchResult sPublicTariffSearchResult = new SPublicTariffSearchResult();

            sPublicTariffSearchResult.Caption           = bPublicTariffSearchResult.Caption;
            sPublicTariffSearchResult.Name              = bPublicTariffSearchResult.Name;
            sPublicTariffSearchResult.ShipType          = ConvertBtoS_PublicTariffShipType(bPublicTariffSearchResult.ShipType);
            sPublicTariffSearchResult.MasterServiceName = bPublicTariffSearchResult.MasterServiceName;
            sPublicTariffSearchResult.MinWt             = bPublicTariffSearchResult.MinWt;        
            sPublicTariffSearchResult.MaxWt             = bPublicTariffSearchResult.MaxWt;
            sPublicTariffSearchResult.WtRangeCaption    = bPublicTariffSearchResult.WtRangeCaption;
            sPublicTariffSearchResult.Dom               = bPublicTariffSearchResult.Dom;        
            sPublicTariffSearchResult.Eur               = bPublicTariffSearchResult.Eur;        
            sPublicTariffSearchResult.Int               = bPublicTariffSearchResult.Int;
            sPublicTariffSearchResult.DispOrder         = bPublicTariffSearchResult.DispOrder;        

            //sPublicTariffSearchResult.Origin = bPublicTariffSearchResult.Origin;
            //sPublicTariffSearchResult.ShipType = ConvertBtoS_PublicTariffShipType(bPublicTariffSearchResult.ShipType);
            //sPublicTariffSearchResult.MasterServiceName = bPublicTariffSearchResult.MasterServiceName;
            //sPublicTariffSearchResult.WeightRange = bPublicTariffSearchResult.WeightRange;
            //sPublicTariffSearchResult.Domestic = bPublicTariffSearchResult.Domestic;
            //sPublicTariffSearchResult.Europe = bPublicTariffSearchResult.Europe;
            //sPublicTariffSearchResult.International = bPublicTariffSearchResult.International;
            //sPublicTariffSearchResult.Destination = bPublicTariffSearchResult.Destination;
            //sPublicTariffSearchResult.Name = bPublicTariffSearchResult.Name;

            return sPublicTariffSearchResult;

        }

        public List<SPublicTariffSearchResult> ConvertBtoS_PublicTariffSearchResult(List<BPublicTariffSearchResult> bPublicTariffSearchResult)
        {
            List<SPublicTariffSearchResult> sPublicTariffSearchResult = new List<SPublicTariffSearchResult>();

            if (bPublicTariffSearchResult != null)
            {
                for (int i = 0; i < bPublicTariffSearchResult.Count; i++)
                {
                    sPublicTariffSearchResult.Add(ConvertBtoS_PublicTariffSearchResult(bPublicTariffSearchResult[i]));
                }
            }
            return sPublicTariffSearchResult;
        }

        public BPublicTariffSearchResult ConvertStoB_PublicTariffSearchResult(SPublicTariffSearchResult sPublicTariffSearchResult)
        {

            //Reconstructed for publictariff new table [15FEB12RM]
            BPublicTariffSearchResult bPublicTariffSearchResult = new BPublicTariffSearchResult();

            bPublicTariffSearchResult.Caption           = sPublicTariffSearchResult.Caption;
            bPublicTariffSearchResult.Name              = sPublicTariffSearchResult.Name;
            bPublicTariffSearchResult.ShipType          = ConvertStoB_PublicTariffShipType(sPublicTariffSearchResult.ShipType);
            bPublicTariffSearchResult.MasterServiceName = sPublicTariffSearchResult.MasterServiceName;
            bPublicTariffSearchResult.MinWt             = sPublicTariffSearchResult.MinWt;
            bPublicTariffSearchResult.MaxWt             = sPublicTariffSearchResult.MaxWt;
            bPublicTariffSearchResult.WtRangeCaption    = sPublicTariffSearchResult.WtRangeCaption;
            bPublicTariffSearchResult.Dom               = sPublicTariffSearchResult.Dom;
            bPublicTariffSearchResult.Eur               = sPublicTariffSearchResult.Eur;
            bPublicTariffSearchResult.Int               = sPublicTariffSearchResult.Int;
            bPublicTariffSearchResult.DispOrder         = sPublicTariffSearchResult.DispOrder;        

            //bPublicTariffSearchResult.Origin = sPublicTariffSearchResult.Origin;
            //bPublicTariffSearchResult.ShipType = ConvertStoB_PublicTariffShipType(sPublicTariffSearchResult.ShipType);
            //bPublicTariffSearchResult.MasterServiceName = sPublicTariffSearchResult.MasterServiceName;
            //bPublicTariffSearchResult.WeightRange = sPublicTariffSearchResult.WeightRange;
            //bPublicTariffSearchResult.Domestic = sPublicTariffSearchResult.Domestic;
            //bPublicTariffSearchResult.Europe = sPublicTariffSearchResult.Europe;
            //bPublicTariffSearchResult.International = sPublicTariffSearchResult.International;
            //bPublicTariffSearchResult.Destination = sPublicTariffSearchResult.Destination;
            //bPublicTariffSearchResult.Name = sPublicTariffSearchResult.Name;

            return bPublicTariffSearchResult;

        }

        public List<BPublicTariffSearchResult> ConvertStoB_PublicTariffSearchResult(List<SPublicTariffSearchResult> sPublicTariffSearchResult)
        {
            List<BPublicTariffSearchResult> bPublicTariffSearchResult = new List<BPublicTariffSearchResult>();

            if (sPublicTariffSearchResult != null)
            {
                for (int i = 0; i < sPublicTariffSearchResult.Count; i++)
                {
                    bPublicTariffSearchResult.Add(ConvertStoB_PublicTariffSearchResult(sPublicTariffSearchResult[i]));
                }
            }
            return bPublicTariffSearchResult;
        }

        /***************** 26DEC11HN ***************/

        private SEnumTariffType GetTariffType1(BEnumTariffType bEnumTariffType)
        {
            SEnumTariffType sEnumTariffType = SEnumTariffType.CarrierPublic; //Just defaulted

            if (BEnumTariffType.CarrierPublic == bEnumTariffType)
                sEnumTariffType = SEnumTariffType.CarrierPublic;
            else if (BEnumTariffType.Purchase == bEnumTariffType)
                sEnumTariffType = SEnumTariffType.Purchase;
            else if (BEnumTariffType.Negotiated == bEnumTariffType)
                sEnumTariffType = SEnumTariffType.Negotiated;

            return sEnumTariffType;
        }

        public BTariffMaster ConvertStoB_TariffMaster1(STariffMaster sTariffMaster)
        {
            BTariffMaster bTariffMaster = new BTariffMaster();

            bTariffMaster.CarrierName = sTariffMaster.CarrierName;
            bTariffMaster.TariffReference = sTariffMaster.TariffReference;
            bTariffMaster.TariffType = GetTariffType(sTariffMaster.TariffType);
            bTariffMaster.ContainerType = sTariffMaster.ContainerType;
            bTariffMaster.KeyUserReference = sTariffMaster.KeyUserReference;
            bTariffMaster.StartDate = sTariffMaster.StartDate;
            bTariffMaster.EndDate = sTariffMaster.EndDate;

            return bTariffMaster;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BTariffMaster> ConvertStoB_TariffMasterList(List<STariffMaster> sTariffMaster)
        {
            List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();
            for (int i = 0; i < sTariffMaster.Count; i++)
            {
                bTariffMaster.Add(ConvertStoB_TariffMaster1(sTariffMaster[i]));
            }
            return bTariffMaster;
        }

        public STariffMaster ConvertBtoS_TariffMaster(BTariffMaster bTariffMaster)
        {
            STariffMaster sTariffMaster = new STariffMaster();

            sTariffMaster.CarrierName = bTariffMaster.CarrierName;
            sTariffMaster.TariffReference = bTariffMaster.TariffReference;
            sTariffMaster.TariffType = GetTariffType1(bTariffMaster.TariffType);
            sTariffMaster.ContainerType = bTariffMaster.ContainerType;
            sTariffMaster.KeyUserReference = bTariffMaster.KeyUserReference;
            sTariffMaster.StartDate = bTariffMaster.StartDate;
            sTariffMaster.EndDate = bTariffMaster.EndDate;

            return sTariffMaster;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<STariffMaster> ConvertBtoS_TariffMaster(List<BTariffMaster> bTariffMaster)
        {
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();
            for (int i = 0; i < bTariffMaster.Count; i++)
            {
                sTariffMaster.Add(ConvertBtoS_TariffMaster(bTariffMaster[i]));
            }
            return sTariffMaster;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation Header
        /// </summary>
        /// <param name="sSimulationHeader"></param>
        /// <returns></returns>
        public BSimulationHeader ConvertStoB_SimulationHeader(SSimulationHeader sSimulationHeader)
        {
            BSimulationHeader bSimulationHeader = new BSimulationHeader();
            bSimulationHeader.AccountNo = sSimulationHeader.AccountNo;
            bSimulationHeader.AssignedTariff = sSimulationHeader.AssignedTariff;
            bSimulationHeader.SimulationID = sSimulationHeader.SimulationID;
            bSimulationHeader.Valid = sSimulationHeader.Valid;
            bSimulationHeader.WeightIncrement = sSimulationHeader.WeightIncrement;
            bSimulationHeader.WeightLimit = sSimulationHeader.WeightLimit;
            return bSimulationHeader;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Header
        /// </summary>
        /// <param name="bSimulationHeader"></param>
        /// <returns></returns>
        public SSimulationHeader ConvertBtoS_SimulationHeader(BSimulationHeader bSimulationHeader)
        {
            SSimulationHeader sSimulationHeader = new SSimulationHeader();

            sSimulationHeader.AccountNo = bSimulationHeader.AccountNo;
            sSimulationHeader.AssignedTariff = bSimulationHeader.AssignedTariff;
            sSimulationHeader.SimulationID = bSimulationHeader.SimulationID;
            sSimulationHeader.Valid = bSimulationHeader.Valid;
            sSimulationHeader.WeightIncrement = bSimulationHeader.WeightIncrement;
            sSimulationHeader.WeightLimit = bSimulationHeader.WeightLimit;

            return sSimulationHeader;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation Surcharge Discount
        /// </summary>
        /// <param name="sSimulationHeader"></param>
        /// <returns></returns>
        public BSimulationSurchargeDiscount ConvertStoB_SimulationSurchargeDiscount(SSimulationSurchargeDiscount sSimulationSurchargeDiscount)
        {
            BSimulationSurchargeDiscount bSimulationSurchargeDiscount = new BSimulationSurchargeDiscount();

            bSimulationSurchargeDiscount.AccountNo = sSimulationSurchargeDiscount.AccountNo;
            bSimulationSurchargeDiscount.CarrierName = sSimulationSurchargeDiscount.CarrierName;
            bSimulationSurchargeDiscount.FuelDiscount = sSimulationSurchargeDiscount.FuelDiscount;
            bSimulationSurchargeDiscount.SimulationID = sSimulationSurchargeDiscount.SimulationID;
            bSimulationSurchargeDiscount.SafetyDiscount = sSimulationSurchargeDiscount.SafetyDiscount;

            return bSimulationSurchargeDiscount;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Simulation Surcharge Discount
        /// </summary>
        /// <param name="sSimulationSurchargeDiscount"></param>
        /// <returns></returns>
        public List<BSimulationSurchargeDiscount> ConvertStoB_SimulationSurchargeDiscount(List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount)
        {
            List<BSimulationSurchargeDiscount> bSimulationSurchargeDiscount = new List<BSimulationSurchargeDiscount>();
            for (int i = 0; i < sSimulationSurchargeDiscount.Count; i++)
            {
                bSimulationSurchargeDiscount.Add(ConvertStoB_SimulationSurchargeDiscount(sSimulationSurchargeDiscount[i]));
            }
            return bSimulationSurchargeDiscount;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Surcharge Discount
        /// </summary>
        /// <param name="BSimulationSurchargeDiscount"></param>
        /// <returns></returns>
        public SSimulationSurchargeDiscount ConvertBtoS_SimulationSurchargeDiscount(BSimulationSurchargeDiscount bSimulationSurchargeDiscount)
        {
            SSimulationSurchargeDiscount sSimulationSurchargeDiscount = new SSimulationSurchargeDiscount();

            sSimulationSurchargeDiscount.AccountNo = bSimulationSurchargeDiscount.AccountNo;
            sSimulationSurchargeDiscount.CarrierName = bSimulationSurchargeDiscount.CarrierName;
            sSimulationSurchargeDiscount.FuelDiscount = bSimulationSurchargeDiscount.FuelDiscount;
            sSimulationSurchargeDiscount.SimulationID = bSimulationSurchargeDiscount.SimulationID;
            sSimulationSurchargeDiscount.SafetyDiscount = bSimulationSurchargeDiscount.SafetyDiscount;

            return sSimulationSurchargeDiscount;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - List Simulation Surcharge Discount
        /// </summary>
        /// <param name="BSimulationSurchargeDiscount"></param>
        /// <returns></returns>
        public List<SSimulationSurchargeDiscount> ConvertBtoS_SimulationSurchargeDiscount(List<BSimulationSurchargeDiscount> bSimulationSurchargeDiscount)
        {
            List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount = new List<SSimulationSurchargeDiscount>();
            for (int i = 0; i < bSimulationSurchargeDiscount.Count; i++)
            {
                sSimulationSurchargeDiscount.Add(ConvertBtoS_SimulationSurchargeDiscount(bSimulationSurchargeDiscount[i]));
            }
            return sSimulationSurchargeDiscount;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation Tariff 
        /// </summary>
        /// <param name="sSimulationTariff"></param>
        /// <returns></returns>
        public BSimulationTariff ConvertStoB_SimulationTariff(SSimulationTariff sSimulationTariff)
        {
            BSimulationTariff bSimulationTariff = new BSimulationTariff();

            bSimulationTariff.AccountNo = sSimulationTariff.AccountNo;
            bSimulationTariff.ADV = sSimulationTariff.ADV;
            bSimulationTariff.AverageWeight = sSimulationTariff.AverageWeight;
            bSimulationTariff.ComparisonMargin = sSimulationTariff.ComparisonMargin;
            bSimulationTariff.ComparisonSaleTariff = sSimulationTariff.ComparisonSaleTariff;
            bSimulationTariff.DeliveryCountry = sSimulationTariff.DeliveryCountry;
            bSimulationTariff.MasterServiceName = sSimulationTariff.MasterServiceName;
            bSimulationTariff.PublicCarrier = sSimulationTariff.PublicCarrier;
            bSimulationTariff.PublicDiscount = sSimulationTariff.PublicDiscount;
            bSimulationTariff.PublicFreight = sSimulationTariff.PublicFreight;
            bSimulationTariff.PublicTariff = sSimulationTariff.PublicTariff;
            bSimulationTariff.PublicTurnOver = sSimulationTariff.PublicTurnOver;
            bSimulationTariff.PurchaseCarrier = sSimulationTariff.PurchaseCarrier;
            bSimulationTariff.PurchaseFreight = sSimulationTariff.PurchaseFreight;
            bSimulationTariff.PurchaseTariff = sSimulationTariff.PurchaseTariff;
            bSimulationTariff.SaleFreight = sSimulationTariff.SaleFreight;
            bSimulationTariff.SaleGrossMargin = sSimulationTariff.SaleGrossMargin;
            bSimulationTariff.SaleMargin = sSimulationTariff.SaleMargin;
            bSimulationTariff.SaleTariff = sSimulationTariff.SaleTariff;
            bSimulationTariff.SaleTurnOver = sSimulationTariff.SaleTurnOver;
            bSimulationTariff.ShipCountry = sSimulationTariff.ShipCountry;
            bSimulationTariff.SimulationID = sSimulationTariff.SimulationID;
            bSimulationTariff.WeightRange = sSimulationTariff.WeightRange;
            bSimulationTariff.PublicSurcharge = sSimulationTariff.PublicSurcharge;
            bSimulationTariff.ContainerType = sSimulationTariff.ContainerType;

            return bSimulationTariff;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Simulation Taruff based
        /// </summary>
        /// <param name="sSimulationTariff"></param>
        /// <returns></returns>
        public List<BSimulationTariff> ConvertStoB_SimulationTariff(List<SSimulationTariff> sSimulationTariff)
        {
            List<BSimulationTariff> bSimulationTariff = new List<BSimulationTariff>();
            for (int i = 0; i < sSimulationTariff.Count; i++)
            {
                bSimulationTariff.Add(ConvertStoB_SimulationTariff(sSimulationTariff[i]));
            }
            return bSimulationTariff;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Tariff Based
        /// </summary>
        /// <param name="bSimulationTariff"></param>
        /// <returns></returns>
        public SSimulationTariff ConvertBtoS_SimulationTariff(BSimulationTariff bSimulationTariff)
        {
            SSimulationTariff sSimulationTariff = new SSimulationTariff();

            sSimulationTariff.AccountNo = bSimulationTariff.AccountNo;
            sSimulationTariff.ADV = bSimulationTariff.ADV;
            sSimulationTariff.AverageWeight = bSimulationTariff.AverageWeight;
            sSimulationTariff.ComparisonMargin = bSimulationTariff.ComparisonMargin;
            sSimulationTariff.ComparisonSaleTariff = bSimulationTariff.ComparisonSaleTariff;
            sSimulationTariff.DeliveryCountry = bSimulationTariff.DeliveryCountry;
            sSimulationTariff.MasterServiceName = bSimulationTariff.MasterServiceName;
            sSimulationTariff.PublicCarrier = bSimulationTariff.PublicCarrier;
            sSimulationTariff.PublicDiscount = bSimulationTariff.PublicDiscount;
            sSimulationTariff.PublicFreight = bSimulationTariff.PublicFreight;
            sSimulationTariff.PublicTariff = bSimulationTariff.PublicTariff;
            sSimulationTariff.PublicTurnOver = bSimulationTariff.PublicTurnOver;
            sSimulationTariff.PurchaseCarrier = bSimulationTariff.PurchaseCarrier;
            sSimulationTariff.PurchaseFreight = bSimulationTariff.PurchaseFreight;
            sSimulationTariff.PurchaseTariff = bSimulationTariff.PurchaseTariff;
            sSimulationTariff.SaleFreight = bSimulationTariff.SaleFreight;
            sSimulationTariff.SaleGrossMargin = bSimulationTariff.SaleGrossMargin;
            sSimulationTariff.SaleMargin = bSimulationTariff.SaleMargin;
            sSimulationTariff.SaleTariff = bSimulationTariff.SaleTariff;
            sSimulationTariff.SaleTurnOver = bSimulationTariff.SaleTurnOver;
            sSimulationTariff.ShipCountry = bSimulationTariff.ShipCountry;
            sSimulationTariff.SimulationID = bSimulationTariff.SimulationID;
            sSimulationTariff.WeightRange = bSimulationTariff.WeightRange;
            sSimulationTariff.PublicSurcharge = bSimulationTariff.PublicSurcharge;
            sSimulationTariff.ContainerType = bSimulationTariff.ContainerType;

            return sSimulationTariff;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - List Tariff Based
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public List<SSimulationTariff> ConvertBtoS_SimulationTariff(List<BSimulationTariff> bSimulationTariff)
        {
            List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
            for (int i = 0; i < bSimulationTariff.Count; i++)
            {
                sSimulationTariff.Add(ConvertBtoS_SimulationTariff(bSimulationTariff[i]));
            }
            return sSimulationTariff;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation Tariff Based
        /// </summary>
        /// <param name="sSimulationTariffBased"></param>
        /// <returns></returns>
        public BSimulationTariffBased ConvertStoB_SimulationTariffBased(SSimulationTariffBased sSimulationTariffBased)
        {
            BSimulationTariffBased bSimulationTariffBased = new BSimulationTariffBased();
            bSimulationTariffBased.Assigned = sSimulationTariffBased.Assigned;
            bSimulationTariffBased.CarrierName = sSimulationTariffBased.CarrierName;
            bSimulationTariffBased.SimulationID = sSimulationTariffBased.SimulationID;
            bSimulationTariffBased.TariffReference = sSimulationTariffBased.TariffReference;
            bSimulationTariffBased.TariffType = sSimulationTariffBased.TariffType;
            bSimulationTariffBased.AccountNo = sSimulationTariffBased.AccountNo;
            return bSimulationTariffBased;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Simulation Taruff based
        /// </summary>
        /// <param name="sSimulationTariffBased"></param>
        /// <returns></returns>
        public List<BSimulationTariffBased> ConvertStoB_SimulationTariffBased(List<SSimulationTariffBased> sSimulationTariffBased)
        {
            List<BSimulationTariffBased> bSimulationTariffBased = new List<BSimulationTariffBased>();
            for (int i = 0; i < sSimulationTariffBased.Count; i++)
            {
                bSimulationTariffBased.Add(ConvertStoB_SimulationTariffBased(sSimulationTariffBased[i]));
            }
            return bSimulationTariffBased;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Tariff Based
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public SSimulationTariffBased ConvertBtoS_SimulationTariffBased(BSimulationTariffBased bSimulationTariffBased)
        {
            SSimulationTariffBased sSimulationTariffBased = new SSimulationTariffBased();
            sSimulationTariffBased.Assigned = bSimulationTariffBased.Assigned;
            sSimulationTariffBased.CarrierName = bSimulationTariffBased.CarrierName;
            sSimulationTariffBased.SimulationID = bSimulationTariffBased.SimulationID;
            sSimulationTariffBased.TariffReference = bSimulationTariffBased.TariffReference;
            sSimulationTariffBased.TariffType = bSimulationTariffBased.TariffType;
            sSimulationTariffBased.AccountNo = bSimulationTariffBased.AccountNo;

            return sSimulationTariffBased;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - List Tariff Based
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public List<SSimulationTariffBased> ConvertBtoS_SimulationTariffBased(List<BSimulationTariffBased> bSimulationTariffBased)
        {
            List<SSimulationTariffBased> sSimulationTariffBased = new List<SSimulationTariffBased>();
            for (int i = 0; i < bSimulationTariffBased.Count; i++)
            {
                sSimulationTariffBased.Add(ConvertBtoS_SimulationTariffBased(bSimulationTariffBased[i]));
            }
            return sSimulationTariffBased;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation ID List
        /// </summary>
        /// <param name="sSimulationTariffBased"></param>
        /// <returns></returns>
        public BSimulationList ConvertStoB_SimulationList(SSimulationList sSimulationList)
        {
            BSimulationList bSimulationList = new BSimulationList();
            bSimulationList.DataField = sSimulationList.DataField;
            bSimulationList.TextField = sSimulationList.TextField;

            return bSimulationList;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - Simulation ID List
        /// </summary>
        /// <param name="sSimulationTariffBased"></param>
        /// <returns></returns>
        public List<BSimulationList> ConvertStoB_SimulationList(List<SSimulationList> sSimulationList)
        {
            List<BSimulationList> bSimulationList = new List<BSimulationList>();
            for (int i = 0; i < sSimulationList.Count; i++)
            {
                bSimulationList.Add(ConvertStoB_SimulationList(sSimulationList[i]));
            }
            return bSimulationList;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Tariff Based
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public SSimulationList ConvertBtoS_SimulationList(BSimulationList bSimulationList)
        {
            SSimulationList sSimulationList = new SSimulationList();
            sSimulationList.DataField = bSimulationList.DataField;
            sSimulationList.TextField = bSimulationList.TextField;

            return sSimulationList;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation ID List
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public List<SSimulationList> ConvertBtoS_SimulationList(List<BSimulationList> bSimulationList)
        {
            List<SSimulationList> sSimulationList = new List<SSimulationList>();
            for (int i = 0; i < bSimulationList.Count; i++)
            {
                sSimulationList.Add(ConvertBtoS_SimulationList(bSimulationList[i]));
            }
            return sSimulationList;
        }


        /// <summary>
        /// Convert Business Entity to Service Entity - Simulation Sub Total
        /// </summary>
        /// <param name="sSimulationSubTotal"></param>
        /// <returns></returns>
        public BSimulationSubTotal ConvertStoB_SimulationSubTotal(SSimulationSubTotal sSimulationSubTotal)
        {
            BSimulationSubTotal bSimulationSubTotal = new BSimulationSubTotal();
            bSimulationSubTotal.AccountNo = sSimulationSubTotal.AccountNo;
            bSimulationSubTotal.SimulationID = sSimulationSubTotal.SimulationID;
            bSimulationSubTotal.SubTotalADV = sSimulationSubTotal.SubTotalADV;
            bSimulationSubTotal.SubtotalCompare = sSimulationSubTotal.SubtotalCompare;
            bSimulationSubTotal.SubTotalCompareMargin = sSimulationSubTotal.SubTotalCompareMargin;
            bSimulationSubTotal.SubTotalCurrentDiscount = sSimulationSubTotal.SubTotalCurrentDiscount;
            bSimulationSubTotal.SubTotalCurrentSale = sSimulationSubTotal.SubTotalCurrentSale;
            bSimulationSubTotal.SubTotalFreight = sSimulationSubTotal.SubTotalFreight;
            bSimulationSubTotal.SubTotalGrossMargin = sSimulationSubTotal.SubTotalGrossMargin;
            bSimulationSubTotal.SubTotalProposedSalesTariff = sSimulationSubTotal.SubTotalProposedSalesTariff;
            bSimulationSubTotal.SubTotalPublic = sSimulationSubTotal.SubTotalPublic;
            bSimulationSubTotal.SubTotalPurchase = sSimulationSubTotal.SubTotalPurchase;
            bSimulationSubTotal.SubTotalSalesFretTariff = sSimulationSubTotal.SubTotalSalesFretTariff;
            bSimulationSubTotal.SubTotalSalesGrossMargin = sSimulationSubTotal.SubTotalSalesGrossMargin;
            bSimulationSubTotal.SubTotalSalesTurnOver = sSimulationSubTotal.SubTotalSalesTurnOver;
            bSimulationSubTotal.SubTotalTurnOver = sSimulationSubTotal.SubTotalTurnOver;
            bSimulationSubTotal.SubTotalWeight = sSimulationSubTotal.SubTotalWeight;


            return bSimulationSubTotal;
        }

        /// <summary>
        /// Convert Service Entity to Business Entity - Simulation Sub total 
        /// </summary>
        /// <param name="sSimulationSubTotal"></param>
        /// <returns></returns>
        public List<BSimulationSubTotal> ConvertStoB_SimulationSubTotal(List<SSimulationSubTotal> sSimulationSubTotal)
        {
            List<BSimulationSubTotal> bSimulationSubTotal = new List<BSimulationSubTotal>();
            for (int i = 0; i < sSimulationSubTotal.Count; i++)
            {
                bSimulationSubTotal.Add(ConvertStoB_SimulationSubTotal(sSimulationSubTotal[i]));
            }
            return bSimulationSubTotal;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Sub Total
        /// </summary>
        /// <param name="bSimulationSubTotal"></param>
        /// <returns></returns>
        public SSimulationSubTotal ConvertBtoS_SimulationSubTotal(BSimulationSubTotal bSimulationSubTotal)
        {
            SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();

            sSimulationSubTotal.AccountNo = bSimulationSubTotal.AccountNo;
            sSimulationSubTotal.SimulationID = bSimulationSubTotal.SimulationID;
            sSimulationSubTotal.SubTotalADV = bSimulationSubTotal.SubTotalADV;
            sSimulationSubTotal.SubtotalCompare = bSimulationSubTotal.SubtotalCompare;
            sSimulationSubTotal.SubTotalCompareMargin = bSimulationSubTotal.SubTotalCompareMargin;
            sSimulationSubTotal.SubTotalCurrentDiscount = bSimulationSubTotal.SubTotalCurrentDiscount;
            sSimulationSubTotal.SubTotalCurrentSale = bSimulationSubTotal.SubTotalCurrentSale;
            sSimulationSubTotal.SubTotalFreight = bSimulationSubTotal.SubTotalFreight;
            sSimulationSubTotal.SubTotalGrossMargin = bSimulationSubTotal.SubTotalGrossMargin;
            sSimulationSubTotal.SubTotalProposedSalesTariff = bSimulationSubTotal.SubTotalProposedSalesTariff;
            sSimulationSubTotal.SubTotalPublic = bSimulationSubTotal.SubTotalPublic;
            sSimulationSubTotal.SubTotalPurchase = bSimulationSubTotal.SubTotalPurchase;
            sSimulationSubTotal.SubTotalSalesFretTariff = bSimulationSubTotal.SubTotalSalesFretTariff;
            sSimulationSubTotal.SubTotalSalesGrossMargin = bSimulationSubTotal.SubTotalSalesGrossMargin;
            sSimulationSubTotal.SubTotalSalesTurnOver = bSimulationSubTotal.SubTotalSalesTurnOver;
            sSimulationSubTotal.SubTotalTurnOver = bSimulationSubTotal.SubTotalTurnOver;
            sSimulationSubTotal.SubTotalWeight = bSimulationSubTotal.SubTotalWeight;

            return sSimulationSubTotal;
        }

        /// <summary>
        /// Convert  Service Entity to Business Entity - Simulation Sub Total
        /// </summary>
        /// <param name="bSimulationSubTotal"></param>
        /// <returns></returns>
        public List<SSimulationSubTotal> ConvertBtoS_SimulationSubTotal(List<BSimulationSubTotal> bSimulationSubTotal)
        {
            List<SSimulationSubTotal> sSimulationSubTotal = new List<SSimulationSubTotal>();
            for (int i = 0; i < bSimulationSubTotal.Count; i++)
            {
                sSimulationSubTotal.Add(ConvertBtoS_SimulationSubTotal(bSimulationSubTotal[i]));
            }
            return sSimulationSubTotal;
        }



    
    
    }
}
