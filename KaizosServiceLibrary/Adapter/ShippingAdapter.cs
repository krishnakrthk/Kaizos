using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaizos.Entities.Business;
using KaizosServiceLibrary.Model;

namespace KaizosServiceLibrary.Adapter
{
    class ShippingAdapter
    {
        /// <summary>
        /// Convert Service Entity To Business Entity - ShipmentOrder
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BShipmentOrder ConvertStoB_ShipmentOrder(SShipmentOrder sShipingOrder)
        {
            BShipmentOrder bShipingOrder        = new BShipmentOrder();
            bShipingOrder.AccountNo             = sShipingOrder.AccountNo;
            bShipingOrder.CalculatedDeliveryTime = sShipingOrder.CalculatedDeliveryTime;
            bShipingOrder.CalculatedShipDate    = sShipingOrder.CalculatedShipDate;
            bShipingOrder.CancelResponsible     = sShipingOrder.CancelResponsible;
            bShipingOrder.Carrier               = sShipingOrder.Carrier;
            bShipingOrder.ChosenPreference       = (BEnumShipPreference)sShipingOrder.ChosenPreference;
            bShipingOrder.ClosingMateiral       = sShipingOrder.ClosingMateiral;
            bShipingOrder.CurrencyType          = sShipingOrder.CurrencyType;
            bShipingOrder.CustomerInternalReference = sShipingOrder.CustomerInternalReference;
            bShipingOrder.CustomsValue         = sShipingOrder.CustomsValue;
            bShipingOrder.DeclaredValue         = sShipingOrder.DeclaredValue;
            bShipingOrder.FreightAmount         = sShipingOrder.FreightAmount;
            bShipingOrder.FuelCharge            = sShipingOrder.FuelCharge;
            bShipingOrder.Insured               = (BEnumFlag)sShipingOrder.Insured;
            bShipingOrder.Options               = sShipingOrder.Options;
            bShipingOrder.OptionsCharges        = sShipingOrder.OptionsCharges;
            bShipingOrder.OrderCreationTime     = sShipingOrder.OrderCreationTime;
            bShipingOrder.OrderType             = (BEnumOrderType)sShipingOrder.OrderType;
            bShipingOrder.PackageMaterial       = sShipingOrder.PackageMaterial;
            bShipingOrder.PackageType           = sShipingOrder.PackageType;
            bShipingOrder.PaymentType           = (BEnumPaymentType)sShipingOrder.PaymentType;
            bShipingOrder.RecipientAddress1     = sShipingOrder.RecipientAddress1;
            bShipingOrder.RecipientAddress2     = sShipingOrder.RecipientAddress2;
            bShipingOrder.RecipientAddress3     = sShipingOrder.RecipientAddress3;
            bShipingOrder.RecipientCity         = sShipingOrder.RecipientCity;
            bShipingOrder.RecipientComments     = sShipingOrder.RecipientComments;
            bShipingOrder.RecipientCompany      = sShipingOrder.RecipientCompany;
            bShipingOrder.RecipientCountry      = sShipingOrder.RecipientCountry;
            bShipingOrder.RecipientDeliveryDeadLine = sShipingOrder.RecipientDeliveryDeadLine;
            bShipingOrder.RecipientEmail        = sShipingOrder.RecipientEmail;
            bShipingOrder.RecipientName         = sShipingOrder.RecipientName;
            bShipingOrder.RecipientNotification = (BEnumFlag)sShipingOrder.RecipientNotification;
            bShipingOrder.RecipientPhone        = sShipingOrder.RecipientPhone;
            bShipingOrder.RecipientState        = sShipingOrder.RecipientState;
            bShipingOrder.RecipientType         = (BEnumAddressType)sShipingOrder.RecipientType;
            bShipingOrder.RecipientZipCode      = sShipingOrder.RecipientZipCode;
            bShipingOrder.ReturnAddress1        = sShipingOrder.ReturnAddress1;
            bShipingOrder.ReturnAddress2        = sShipingOrder.ReturnAddress2;
            bShipingOrder.ReturnAddress3        = sShipingOrder.ReturnAddress3;
            bShipingOrder.ReturnCity            = sShipingOrder.ReturnCity;
            bShipingOrder.ReturnCompany         = sShipingOrder.ReturnCompany;
            bShipingOrder.ReturnCountry         = sShipingOrder.ReturnCountry;
            bShipingOrder.ReturnEmail = sShipingOrder.ReturnEmail;
            bShipingOrder.ReturnName            = sShipingOrder.ReturnName;
            bShipingOrder.ReturnPhone           = sShipingOrder.ReturnPhone;
            bShipingOrder.ReturnState           = sShipingOrder.ReturnState;
            bShipingOrder.ReturnZipCode         = sShipingOrder.ReturnZipCode;
            bShipingOrder.SameReturnAddress     = (BEnumFlag)sShipingOrder.SameReturnAddress;
            bShipingOrder.SenderAddress1        = sShipingOrder.SenderAddress1;
            bShipingOrder.SenderAddress2        = sShipingOrder.SenderAddress2;
            bShipingOrder.SenderAddress3        = sShipingOrder.SenderAddress3;
            bShipingOrder.SenderCity            = sShipingOrder.SenderCity;
            bShipingOrder.SenderCollectDeadLine = sShipingOrder.SenderCollectDeadLine;
            bShipingOrder.SenderComments        = sShipingOrder.SenderComments;
            bShipingOrder.SenderCompany         = sShipingOrder.SenderCompany;
            bShipingOrder.SenderCountry         = sShipingOrder.SenderCountry;
            bShipingOrder.SenderEmail           = sShipingOrder.SenderEmail;
            bShipingOrder.SenderName            = sShipingOrder.SenderName;
            bShipingOrder.SenderNotification    = (BEnumFlag)sShipingOrder.SenderNotification;
            bShipingOrder.SenderPhone           = sShipingOrder.SenderPhone;
            bShipingOrder.SenderState           = sShipingOrder.SenderState;
            bShipingOrder.SenderZipCode         = sShipingOrder.SenderZipCode;
            bShipingOrder.ShipDetail            = ConvertStoB_ShipmentDetailsList(sShipingOrder.ShipDetail);
            bShipingOrder.ShipGroupID           = sShipingOrder.ShipGroupID;
            bShipingOrder.ShipReference         = sShipingOrder.ShipReference;
            bShipingOrder.ShipDateTime = sShipingOrder.ShipDateTime;
            bShipingOrder.Status                = sShipingOrder.Status;
            bShipingOrder.TaxableWeight         = sShipingOrder.TaxableWeight;
            bShipingOrder.TotalAmount           = sShipingOrder.TotalAmount;
            bShipingOrder.TotalWeight           = sShipingOrder.TotalWeight;
            bShipingOrder.UODCount              = sShipingOrder.UODCount;
            bShipingOrder.WishedShipDate        = sShipingOrder.WishedShipDate;
            bShipingOrder.ContainerType         = sShipingOrder.ContainerType;
            bShipingOrder.Surcharge = sShipingOrder.Surcharge;
            bShipingOrder.SurchargeDescription = sShipingOrder.SurchargeDescription;
             /***********[KS15MAR12]**********/
            bShipingOrder.CarrierService=sShipingOrder.CarrierService;
            /***********[KS23APR12]**********/
            bShipingOrder.CalculatedInsuranceAmount = sShipingOrder.CalculatedInsuranceAmount;

            bShipingOrder.ShipmentResult = ConvertStoB_ShipmentResult(sShipingOrder.ShipmentResult); 
            return bShipingOrder;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - ShipmentOrder
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SShipmentOrder ConvertBtoS_ShipmentOrder(BShipmentOrder bShipingOrder)
        {
            SShipmentOrder sShipingOrder        = new SShipmentOrder();
            sShipingOrder.AccountNo             = bShipingOrder.AccountNo;
            sShipingOrder.CalculatedDeliveryTime = bShipingOrder.CalculatedDeliveryTime;
            sShipingOrder.CalculatedShipDate    = bShipingOrder.CalculatedShipDate;
            sShipingOrder.CancelResponsible     = bShipingOrder.CancelResponsible;
            sShipingOrder.Carrier               = bShipingOrder.Carrier;
            sShipingOrder.ChosenPreference       = (SEnumShipPreference)bShipingOrder.ChosenPreference;
            sShipingOrder.ClosingMateiral       = bShipingOrder.ClosingMateiral;
            sShipingOrder.CurrencyType          = bShipingOrder.CurrencyType;
            sShipingOrder.CustomerInternalReference = bShipingOrder.CustomerInternalReference;
            sShipingOrder.CustomsValue         = bShipingOrder.CustomsValue;
            sShipingOrder.DeclaredValue         = bShipingOrder.DeclaredValue;
            sShipingOrder.FreightAmount         = bShipingOrder.FreightAmount;
            sShipingOrder.FuelCharge            = bShipingOrder.FuelCharge;
            sShipingOrder.Insured               = (SEnumFlag)bShipingOrder.Insured;
            sShipingOrder.Options               = bShipingOrder.Options;
            sShipingOrder.OptionsCharges        = bShipingOrder.OptionsCharges;
            sShipingOrder.OrderCreationTime     = bShipingOrder.OrderCreationTime;
            sShipingOrder.OrderType             = (SEnumOrderType)bShipingOrder.OrderType;
            sShipingOrder.PackageMaterial       = bShipingOrder.PackageMaterial;
            sShipingOrder.PackageType           = bShipingOrder.PackageType;
            sShipingOrder.PaymentType           = (SEnumPaymentType)bShipingOrder.PaymentType;
            sShipingOrder.RecipientAddress1     = bShipingOrder.RecipientAddress1;
            sShipingOrder.RecipientAddress2     = bShipingOrder.RecipientAddress2;
            sShipingOrder.RecipientAddress3     = bShipingOrder.RecipientAddress3;
            sShipingOrder.RecipientCity         = bShipingOrder.RecipientCity;
            sShipingOrder.RecipientComments     = bShipingOrder.RecipientComments;
            sShipingOrder.RecipientCompany      = bShipingOrder.RecipientCompany;
            sShipingOrder.RecipientCountry      = bShipingOrder.RecipientCountry;
            sShipingOrder.RecipientDeliveryDeadLine = bShipingOrder.RecipientDeliveryDeadLine;
            sShipingOrder.RecipientEmail        = bShipingOrder.RecipientEmail;
            sShipingOrder.RecipientName         = bShipingOrder.RecipientName;
            sShipingOrder.RecipientNotification = (SEnumFlag)bShipingOrder.RecipientNotification;
            sShipingOrder.RecipientPhone        = bShipingOrder.RecipientPhone;
            sShipingOrder.RecipientState        = bShipingOrder.RecipientState;
            sShipingOrder.RecipientType         = (SEnumAddressType)bShipingOrder.RecipientType;
            sShipingOrder.RecipientZipCode      = bShipingOrder.RecipientZipCode;
            sShipingOrder.ReturnAddress1        = bShipingOrder.ReturnAddress1;
            sShipingOrder.ReturnAddress2        = bShipingOrder.ReturnAddress2;
            sShipingOrder.ReturnAddress3        = bShipingOrder.ReturnAddress3;
            sShipingOrder.ReturnCity            = bShipingOrder.ReturnCity;
            sShipingOrder.ReturnCompany         = bShipingOrder.ReturnCompany;
            sShipingOrder.ReturnCountry         = bShipingOrder.ReturnCountry;
            sShipingOrder.ReturnEmail = bShipingOrder.ReturnEmail;
            sShipingOrder.ReturnName            = bShipingOrder.ReturnName;
            sShipingOrder.ReturnPhone           = bShipingOrder.ReturnPhone;
            sShipingOrder.ReturnState           = bShipingOrder.ReturnState;
            sShipingOrder.ReturnZipCode         = bShipingOrder.ReturnZipCode;
            sShipingOrder.SameReturnAddress     = (SEnumFlag)bShipingOrder.SameReturnAddress;
            sShipingOrder.SenderAddress1        = bShipingOrder.SenderAddress1;
            sShipingOrder.SenderAddress2        = bShipingOrder.SenderAddress2;
            sShipingOrder.SenderAddress3        = bShipingOrder.SenderAddress3;
            sShipingOrder.SenderCity            = bShipingOrder.SenderCity;
            sShipingOrder.SenderCollectDeadLine = bShipingOrder.SenderCollectDeadLine;
            sShipingOrder.SenderComments        = bShipingOrder.SenderComments;
            sShipingOrder.SenderCompany         = bShipingOrder.SenderCompany;
            sShipingOrder.SenderCountry         = bShipingOrder.SenderCountry;
            sShipingOrder.SenderEmail           = bShipingOrder.SenderEmail;
            sShipingOrder.SenderName            = bShipingOrder.SenderName;
            sShipingOrder.SenderNotification    = (SEnumFlag)bShipingOrder.SenderNotification;
            sShipingOrder.SenderPhone           = bShipingOrder.SenderPhone;
            sShipingOrder.SenderState           = bShipingOrder.SenderState;
            sShipingOrder.SenderZipCode         = bShipingOrder.SenderZipCode;

            sShipingOrder.ShipDetail            = ConvertBtoS_ShipmentDetailsList(bShipingOrder.ShipDetail);

            sShipingOrder.ShipGroupID           = bShipingOrder.ShipGroupID;
            sShipingOrder.ShipReference         = bShipingOrder.ShipReference;
            sShipingOrder.ShipDateTime = bShipingOrder.ShipDateTime;
            sShipingOrder.Status                = bShipingOrder.Status;
            sShipingOrder.TaxableWeight         = bShipingOrder.TaxableWeight;
            sShipingOrder.TotalAmount           = bShipingOrder.TotalAmount;
            sShipingOrder.TotalWeight           = bShipingOrder.TotalWeight;
            sShipingOrder.UODCount              = bShipingOrder.UODCount;
            sShipingOrder.WishedShipDate        = bShipingOrder.WishedShipDate;
            sShipingOrder.ContainerType         = bShipingOrder.ContainerType;
            sShipingOrder.Surcharge = bShipingOrder.Surcharge;
            sShipingOrder.SurchargeDescription = bShipingOrder.SurchargeDescription;
            /***********[KS15MAR12]**********/
            sShipingOrder.CarrierService = bShipingOrder.CarrierService;
            /***********[KS23APR12]**********/
            sShipingOrder.CalculatedInsuranceAmount = bShipingOrder.CalculatedInsuranceAmount;

            sShipingOrder.ShipmentResult = ConvertBtoS_ShipmentResult(bShipingOrder.ShipmentResult); 
            return sShipingOrder;
        }


        /// <summary>
        /// Convert Service Entity to Business Entity - List of Shipment order
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BShipmentOrder> ConvertStoB_ShipmentOrdersList(List<SShipmentOrder> sShipmentOrder)
        {
            List<BShipmentOrder> bShipmentOrder = new List<BShipmentOrder>();
            for (int i = 0; i < sShipmentOrder.Count; i++)
            {
                bShipmentOrder.Add(ConvertStoB_ShipmentOrder(sShipmentOrder[i]));
            }
            return bShipmentOrder;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of Shipment Order
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<SShipmentOrder> ConvertBtoS_ShipmentOrderList(List<BShipmentOrder> bShipmentOrder)
        {
            List<SShipmentOrder> sShipmentOrder = new List<SShipmentOrder>();
            for (int i = 0; i < bShipmentOrder.Count; i++)
            {
                sShipmentOrder.Add(ConvertBtoS_ShipmentOrder(bShipmentOrder[i]));
            }
            return sShipmentOrder;
        }


        /// <summary>
        /// Convert Service Entity To Business Entity - ShipmentDetail
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BShipmentDetails ConvertStoB_ShipmentDetails(SShipmentDetails sShipmentDetails)
        {
            BShipmentDetails bShipmentDetails   = new BShipmentDetails();
            bShipmentDetails.Container          = sShipmentDetails.Container;
            bShipmentDetails.ContentType        = sShipmentDetails.ContentType;
            bShipmentDetails.DimensionUnit      = sShipmentDetails.DimensionUnit;
            bShipmentDetails.Height             = sShipmentDetails.Height;
            bShipmentDetails.Length             = sShipmentDetails.Length;
            bShipmentDetails.ParcelNo           = sShipmentDetails.ParcelNo;
            bShipmentDetails.ShippingReference  = sShipmentDetails.ShippingReference;
            bShipmentDetails.TaxableWeight      = sShipmentDetails.TaxableWeight;
            bShipmentDetails.TrackingNo         = sShipmentDetails.TrackingNo;
            bShipmentDetails.Weight             = sShipmentDetails.Weight;
            bShipmentDetails.WeightUnit         = sShipmentDetails.WeightUnit;
            bShipmentDetails.Width              = sShipmentDetails.Width;
            return bShipmentDetails;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - ShipmentDetail
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SShipmentDetails ConvertBtoS_ShipmentDetails(BShipmentDetails bShipmentDetails)
        {
            SShipmentDetails sShipmentDetails   = new SShipmentDetails();
            sShipmentDetails.Container          = bShipmentDetails.Container;
            sShipmentDetails.ContentType        = bShipmentDetails.ContentType;
            sShipmentDetails.DimensionUnit      = bShipmentDetails.DimensionUnit;
            sShipmentDetails.Height             = bShipmentDetails.Height;
            sShipmentDetails.Length             = bShipmentDetails.Length;
            sShipmentDetails.ParcelNo           = bShipmentDetails.ParcelNo;
            sShipmentDetails.ShippingReference  = bShipmentDetails.ShippingReference;
            sShipmentDetails.TaxableWeight      = bShipmentDetails.TaxableWeight;
            sShipmentDetails.TrackingNo         = bShipmentDetails.TrackingNo;
            sShipmentDetails.Weight             = bShipmentDetails.Weight;
            sShipmentDetails.WeightUnit         = bShipmentDetails.WeightUnit;
            sShipmentDetails.Width              = bShipmentDetails.Width;

            return sShipmentDetails;

        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of ShipmentDetail
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BShipmentDetails> ConvertStoB_ShipmentDetailsList(List<SShipmentDetails> sShipmentDetails)
        {
            List<BShipmentDetails> bShipmentDetails = new List<BShipmentDetails>();
            for (int i = 0; i < sShipmentDetails.Count; i++)
            {
                bShipmentDetails.Add(ConvertStoB_ShipmentDetails(sShipmentDetails[i]));
            }
            return bShipmentDetails;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of ShipmentDetail
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<SShipmentDetails> ConvertBtoS_ShipmentDetailsList(List<BShipmentDetails> bShipmentDetails)
        {
            List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
            for (int i = 0; i < bShipmentDetails.Count; i++)
            {
                sShipmentDetails.Add(ConvertBtoS_ShipmentDetails(bShipmentDetails[i]));
            }
            return sShipmentDetails;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - ShipmentOrderTemp
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BShipmentOrderTemp ConvertStoB_ShipmentOrderTemp(SShipmentOrderTemp sShipmentOrderTemp)
        {
            BShipmentOrderTemp bShipmentOrderTemp   = new BShipmentOrderTemp();
            bShipmentOrderTemp.AccountNo            = sShipmentOrderTemp.AccountNo;
            bShipmentOrderTemp.OpenGroup            = (BEnumFlag)sShipmentOrderTemp.OpenGroup;
            bShipmentOrderTemp.SessionID            = sShipmentOrderTemp.SessionID;
            bShipmentOrderTemp.ShipGroupID          = sShipmentOrderTemp.ShipGroupID;
            bShipmentOrderTemp.ShipReference        = sShipmentOrderTemp.ShipReference;
            return bShipmentOrderTemp;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - ShipmentOrderTemp
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SShipmentOrderTemp ConvertBtoS_ShipmentOrderTemp(BShipmentOrderTemp bShipmentOrderTemp)
        {
            SShipmentOrderTemp sShipmentOrderTemp   = new SShipmentOrderTemp();
            sShipmentOrderTemp.AccountNo            = bShipmentOrderTemp.AccountNo;
            sShipmentOrderTemp.OpenGroup            = (SEnumFlag)bShipmentOrderTemp.OpenGroup;
            sShipmentOrderTemp.SessionID            = bShipmentOrderTemp.SessionID;
            sShipmentOrderTemp.ShipGroupID          = bShipmentOrderTemp.ShipGroupID;
            sShipmentOrderTemp.ShipReference        = bShipmentOrderTemp.ShipReference;
            return sShipmentOrderTemp;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - ShipmentQuotation
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BShipmentQuotation ConvertStoB_ShipmentQuotation(SShipmentQuotation sShipmentQuotation)
        {
            BShipmentQuotation bShipmentQuotation   = new BShipmentQuotation();
            bShipmentQuotation.CarrierName          = sShipmentQuotation.CarrierName;
            bShipmentQuotation.CarrierPriority      = sShipmentQuotation.CarrierPriority;
            bShipmentQuotation.CarrierType          = sShipmentQuotation.CarrierType;
            bShipmentQuotation.DeliveryDate         = sShipmentQuotation.DeliveryDate;
            bShipmentQuotation.CalculatedTariff     = sShipmentQuotation.CalculatedTariff;
            bShipmentQuotation.FuelSurcharge        = sShipmentQuotation.FuelSurcharge;
            bShipmentQuotation.Information          = sShipmentQuotation.Information;
            bShipmentQuotation.Options              = sShipmentQuotation.Options;
            bShipmentQuotation.OptionsDescription   = sShipmentQuotation.OptionsDescription;
            bShipmentQuotation.ServiceName          = sShipmentQuotation.ServiceName;
            bShipmentQuotation.ShippingDate         = sShipmentQuotation.ShippingDate;
            bShipmentQuotation.Surcharge            = sShipmentQuotation.Surcharge;
            bShipmentQuotation.SurchargeDescription = sShipmentQuotation.SurchargeDescription;
            /***********[KS15MAR12]**********/
            bShipmentQuotation.InfoType=sShipmentQuotation.InfoType;
     

            return bShipmentQuotation;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - ShipmentQuotation
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SShipmentQuotation ConvertBtoS_ShipmentQuotation(BShipmentQuotation bShipmentQuotation)
        {
            SShipmentQuotation sShipmentQuotation = new SShipmentQuotation();
            sShipmentQuotation.CarrierName      = bShipmentQuotation.CarrierName;
            sShipmentQuotation.CarrierPriority  = bShipmentQuotation.CarrierPriority;
            sShipmentQuotation.CarrierType      = bShipmentQuotation.CarrierType;
            sShipmentQuotation.DeliveryDate     = bShipmentQuotation.DeliveryDate;
            sShipmentQuotation.CalculatedTariff = bShipmentQuotation.CalculatedTariff;
            sShipmentQuotation.FuelSurcharge    = bShipmentQuotation.FuelSurcharge;
            sShipmentQuotation.Information      = bShipmentQuotation.Information;
            sShipmentQuotation.Options          = bShipmentQuotation.Options;
            sShipmentQuotation.OptionsDescription = bShipmentQuotation.OptionsDescription;
            sShipmentQuotation.ServiceName      = bShipmentQuotation.ServiceName;
            sShipmentQuotation.ShippingDate = bShipmentQuotation.ShippingDate;
            sShipmentQuotation.Surcharge        = bShipmentQuotation.Surcharge;
            sShipmentQuotation.SurchargeDescription = bShipmentQuotation.SurchargeDescription;
            /***********[KS15MAR12]**********/
            sShipmentQuotation.InfoType = bShipmentQuotation.InfoType;
            return sShipmentQuotation;

        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BShipmentQuotation> ConvertStoB_ShipmentQuotationList(List<SShipmentQuotation> sShipmentQuotation)
        {
            List<BShipmentQuotation> bShipmentQuotation = new List<BShipmentQuotation>();
            for (int i = 0; i < sShipmentQuotation.Count; i++)
            {
                bShipmentQuotation.Add(ConvertStoB_ShipmentQuotation(sShipmentQuotation[i]));
            }
            return bShipmentQuotation;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<SShipmentQuotation> ConvertBtoS_ShipmentQuotationList(List<BShipmentQuotation> bShipmentQuotation)
        {
            List<SShipmentQuotation> sShipmentQuotation = new List<SShipmentQuotation>();
            for (int i = 0; i < bShipmentQuotation.Count; i++)
            {
                sShipmentQuotation.Add(ConvertBtoS_ShipmentQuotation(bShipmentQuotation[i]));
            }
            return sShipmentQuotation;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - TariffName
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BTariffName ConvertStoB_TariffName(STariffName sTariffName)
        {
            BTariffName bTariffName = new BTariffName();
            bTariffName.AffectedTariff = sTariffName.AffectedTariff;
            return bTariffName;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public STariffName ConvertBtoS_TariffName(BTariffName bTariffName)
        {
            STariffName sTariffName = new STariffName();
            sTariffName.AffectedTariff = bTariffName.AffectedTariff;
            return sTariffName;

        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BTariffName> ConvertStoB_TariffNameList(List<STariffName> sTariffName)
        {
            List<BTariffName> bTariffName = new List<BTariffName>();
            for (int i = 0; i < sTariffName.Count; i++)
            {
                bTariffName.Add(ConvertStoB_TariffName(sTariffName[i]));
            }
            return bTariffName;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<STariffName> ConvertBtoS_TariffNameList(List<BTariffName> bTariffName)
        {
            List<STariffName> sTariffName = new List<STariffName>();
            for (int i = 0; i < bTariffName.Count; i++)
            {
                sTariffName.Add(ConvertBtoS_TariffName(bTariffName[i]));
            }
            return sTariffName;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - TariffName
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BTariffInfo ConvertStoB_TariffInfo(STariffInfo sTariffDetail)
        {
            BTariffInfo bTariffDetail = new BTariffInfo();
            bTariffDetail.TariffName = sTariffDetail.TariffName;
            bTariffDetail.TariffTable = sTariffDetail.TariffTable;
            bTariffDetail.TariffType = sTariffDetail.TariffType;
            return bTariffDetail;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public STariffInfo ConvertBtoS_TariffInfo(BTariffInfo bTariffDetail)
        {
            STariffInfo sTariffDetail = new STariffInfo();
            sTariffDetail.TariffName= bTariffDetail.TariffName;
            sTariffDetail.TariffTable = bTariffDetail.TariffTable;
            sTariffDetail.TariffType = bTariffDetail.TariffType;

            return sTariffDetail;

        }

        /// <summary>
        /// Convert Service Entity to Business Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<BTariffInfo> ConvertStoB_TariffInfoList(List<STariffInfo> sTariffDetail)
        {
            List<BTariffInfo> bTariffDetail = new List<BTariffInfo>();
            for (int i = 0; i < sTariffDetail.Count; i++)
            {
                bTariffDetail.Add(ConvertStoB_TariffInfo(sTariffDetail[i]));
            }
            return bTariffDetail;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - List of Tariff Name
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public List<STariffInfo> ConvertBtoS_TariffDetailList(List<BTariffInfo> bTariffDetail)
        {
            List<STariffInfo> sTariffDetail = new List<STariffInfo>();
            for (int i = 0; i < bTariffDetail.Count; i++)
            {
                sTariffDetail.Add(ConvertBtoS_TariffInfo(bTariffDetail[i]));
            }
            return sTariffDetail;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - PaymentInfo
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BPaymentInfo ConvertStoB_PaymentInfo(SPaymentInfo sPaymentInfo)
        {
            BPaymentInfo bPaymentInfo = new BPaymentInfo();
            bPaymentInfo.AdminMailID = sPaymentInfo.AdminMailID;
            bPaymentInfo.AdminPhone = sPaymentInfo.AdminPhone;
            bPaymentInfo.AvailableCreditLimit = sPaymentInfo.AvailableCreditLimit;
            bPaymentInfo.PaymentMethod = sPaymentInfo.PaymentMethod;
            bPaymentInfo.UserMailId = sPaymentInfo.PaymentMethod;
            return bPaymentInfo;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - PaymentInfo
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SPaymentInfo ConvertBtoS_PaymentInfo(BPaymentInfo bPaymentInfo)
        {
            SPaymentInfo sPaymentInfo = new SPaymentInfo();
            sPaymentInfo.AdminMailID = bPaymentInfo.AdminMailID;
            sPaymentInfo.AdminPhone = bPaymentInfo.AdminPhone;
            sPaymentInfo.AvailableCreditLimit = bPaymentInfo.AvailableCreditLimit;
            sPaymentInfo.PaymentMethod = bPaymentInfo.PaymentMethod;
            sPaymentInfo.UserMailId = bPaymentInfo.UserMailId;
            return sPaymentInfo;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - Shipment Result
        /// </summary>
        /// <param name="sToS"></param>
        /// <returns></returns>
        public BShipmentResult ConvertStoB_ShipmentResult(SShipmentResult sShipmentResult)
        {
            BShipmentResult bShipmentResult = new BShipmentResult();
            bShipmentResult.FeasibilityError = sShipmentResult.FeasibilityError;


            bShipmentResult.LabelError = sShipmentResult.LabelError;
            bShipmentResult.ManifestError = sShipmentResult.ManifestError;
            bShipmentResult.OtherError = sShipmentResult.OtherError;


            bShipmentResult.isFeasibility = BEnumFlag.No;
            if (sShipmentResult.isFeasibility == SEnumFlag.Yes)
                bShipmentResult.isFeasibility = BEnumFlag.Yes;

            bShipmentResult.isLabelGenerated = BEnumFlag.No;
            if (sShipmentResult.isLabelGenerated == SEnumFlag.Yes)
                bShipmentResult.isLabelGenerated = BEnumFlag.Yes;

            bShipmentResult.isManifestGenerated = BEnumFlag.No;
            if (sShipmentResult.isManifestGenerated == SEnumFlag.Yes)
                bShipmentResult.isManifestGenerated = BEnumFlag.Yes;

            bShipmentResult.isOther = BEnumFlag.No;
            if (sShipmentResult.isOther == SEnumFlag.Yes)
                bShipmentResult.isOther = BEnumFlag.Yes;

            return bShipmentResult;
        }

        /// <summary>
        /// Convert Business Entity to Service Entity - PaymentInfo
        /// </summary>
        /// <param name="bToS"></param>
        /// <returns></returns>
        public SShipmentResult ConvertBtoS_ShipmentResult(BShipmentResult bShipmentResult)
        {
            SShipmentResult sShipmentResult = new SShipmentResult();
            sShipmentResult.FeasibilityError = bShipmentResult.FeasibilityError;
            sShipmentResult.LabelError  = bShipmentResult.LabelError;
            sShipmentResult.ManifestError  = bShipmentResult.ManifestError;
            sShipmentResult.OtherError  = bShipmentResult.OtherError;

            sShipmentResult.isFeasibility = SEnumFlag.No;
            if (bShipmentResult.isFeasibility == BEnumFlag.Yes)
                sShipmentResult.isFeasibility = SEnumFlag.Yes;

            sShipmentResult.isLabelGenerated = SEnumFlag.No;
            if (bShipmentResult.isLabelGenerated == BEnumFlag.Yes)
                sShipmentResult.isLabelGenerated = SEnumFlag.Yes;

            sShipmentResult.isManifestGenerated = SEnumFlag.No;
            if (bShipmentResult.isManifestGenerated == BEnumFlag.Yes)
                sShipmentResult.isManifestGenerated = SEnumFlag.Yes;

            sShipmentResult.isOther = SEnumFlag.No;
            if (bShipmentResult.isOther == BEnumFlag.Yes)
                sShipmentResult.isOther = SEnumFlag.Yes;

            return sShipmentResult;

        }

    
    }

}
