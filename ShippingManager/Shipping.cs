using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;
using Kaizos.Entities.Business;
using KaizosEntities;
using Kaizos.Components.CarrierManager;


namespace Kaizos.Components.ShippingManager
{
    public class ShippingHandler
    {
        /// <summary>
        /// Insert shipment order and details  (Use case : 2.1)
        /// </summary>
        /// <param name="bshipmentOrder"></param>
        /// <param name="bshipmentQuotation"></param>
        /// <param name="strSessionID"></param>
        /// <returns>
        /// 0 =&gt; Success
        /// 1 =&gt; Fail
        /// 2 =&gt; Failed due to shipment reference already exist
        /// </returns>
        public int CreateSingleShipment(BShipmentOrder bshipmentOrder, BShipmentQuotation bshipmentQuotation, string strSessionID)
        {

            float ConvertedLength = 0;
            float ConvertedWidth = 0;
            float ConvertedHeight = 0;


            int iGroupID;
            List<BShipmentDetails> bShipmentDetails = new List<BShipmentDetails>();
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

            /* Create or Get Group Id  and insert an entry in SHIPMENT_ORDER_TEMP*/
            ObjectParameter objParamGroupID = new ObjectParameter("GROUP_ID", typeof(Int32));
            System.Nullable<int> iReturnValue = context.uSP_SHIPMENT_ORDER_TEMP_INSERT(bshipmentOrder.ShipReference, bshipmentOrder.AccountNo, strSessionID, objParamGroupID).SingleOrDefault();
            iGroupID = Convert.ToInt32(objParamGroupID.Value);

            float Freight = (float)bshipmentQuotation.CalculatedTariff - (float)bshipmentQuotation.FuelSurcharge;
            /* Create an entry in SHIPMENT_ORDER table for the reference*/
            System.Nullable<int> iReturnValue1 = context.uSP_SHIPMENT_ORDER_INSERT(bshipmentOrder.ShipReference,
                                                                                        bshipmentOrder.SenderCountry,
                                                                                        bshipmentOrder.SenderZipCode,
                                                                                        bshipmentOrder.SenderCity,
                                                                                        bshipmentOrder.RecipientCountry,
                                                                                        bshipmentOrder.RecipientZipCode,
                                                                                        bshipmentOrder.RecipientCity,
                                                                                        bshipmentOrder.RecipientType.ToString(),
                                                                                        bshipmentOrder.Options,
                                                                                        bshipmentOrder.OptionsCharges,
                                                                                        bshipmentOrder.ShipDateTime,
                                                                                        bshipmentOrder.UODCount,
                                                                                        (decimal)bshipmentOrder.TotalWeight,
                                                                                        bshipmentQuotation.ServiceName,
                                                                                        DateTime.Now,
                                                                                        bshipmentQuotation.DeliveryDate,
                                                                                        bshipmentOrder.CalculatedDeliveryTime, //    "15:00",
                                                                                        (decimal)bshipmentOrder.TaxableWeight,
                                                                                        (decimal)Freight,
                                                                                        (decimal)bshipmentQuotation.FuelSurcharge,
                                                                                        bshipmentOrder.ChosenPreference.ToString(),
                                                                                        bshipmentOrder.AccountNo,
                                                                                        (decimal)bshipmentQuotation.CalculatedTariff,
                                                                                        "M",
                                                                                        iGroupID,
                                                                                        bshipmentQuotation.CarrierName).SingleOrDefault();

            bShipmentDetails = bshipmentOrder.ShipDetail.ToList();

            /*Insert entry in SHIPMENT_ORDER_DETAIL table for UOD's*/
            for (int i = 0; i < bShipmentDetails.Count; i++)
            {

                ConvertedLength = ConvertToCM(bShipmentDetails[i].Length, bShipmentDetails[i].DimensionUnit);
                ConvertedWidth = ConvertToCM(bShipmentDetails[i].Width, bShipmentDetails[i].DimensionUnit);
                ConvertedHeight = ConvertToCM(bShipmentDetails[i].Height, bShipmentDetails[i].DimensionUnit);


                float TaxableWeight = GetTaxableWeight(bshipmentQuotation.CarrierName, bshipmentOrder.SenderCountry, bshipmentOrder.RecipientCountry, bshipmentOrder.RecipientZipCode,
                                                                                                bShipmentDetails[i].Weight,
                                                                                                ConvertedHeight, //   bShipmentDetails[i].Height,
                                                                                                ConvertedWidth,  // bShipmentDetails[i].Width,
                                                                                                ConvertedLength); // bShipmentDetails[i].Length);

                System.Nullable<int> iReturnValue2 = context.uSP_SHIPMENT_ORDER_DETAIL_INSERT(bshipmentOrder.ShipReference,
                                                                                                bShipmentDetails[i].ParcelNo,
                                                                                                bShipmentDetails[i].ContentType,
                                                                                                bShipmentDetails[i].Container,
                                                                                                (decimal)bShipmentDetails[i].Weight,
                                                                                                bShipmentDetails[i].WeightUnit,
                                                                                                (decimal)bShipmentDetails[i].Length,
                                                                                                (decimal)bShipmentDetails[i].Width,
                                                                                                (decimal)bShipmentDetails[i].Height,
                                                                                                bShipmentDetails[i].DimensionUnit,
                                                                                                (decimal)TaxableWeight).SingleOrDefault();

            }


            return (int)iReturnValue;
        }

        protected float ConvertToCM(float Dimension, string DimensionUnit)
        {
            float result = 0;
            result = Dimension;
            if (DimensionUnit.Equals("in"))
            {
                result = result * 2.54f;
            }
            return result;
        }

        /// <summary>
        /// Get Affected tariff name for the user
        /// </summary>
        /// <param name="strAccountNo"></param>
        /// <returns>
        /// record set
        /// </returns>
        //public List<BTariffName> GetAffectedTariffName(string strAccountNo)
        //{
        //    List<BTariffName> bTariffName = new List<BTariffName>();

        //    KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        //    var TariffName = context.uSP_GET_AFFECTED_TARIFF(strAccountNo).ToList();

        //    for (int i = 0; i < TariffName.Count; i++)
        //    {
        //        BTariffName t = new BTariffName();
        //        t.AffectedTariff = TariffName[i].ToString();
        //        bTariffName.Add(t);
        //    }
        //    if (TariffName.Count == 0)
        //    {
        //        BTariffName t = new BTariffName();
        //        t.AffectedTariff = "NotAssigned";
        //        bTariffName.Add(t);
        //    }

        //    return bTariffName;

        //}

        /// <summary>
        /// Updates all flags and address details for the confirmed shipment
        /// </summary>
        /// <param name="bShipmentOrder"></param>
        /// <returns>
        /// 0 =&gt; Success
        /// 1 =&gt; Fail
        /// 2 =&gt; Shipment reference not available
        /// </returns>
        public int ConfirmShipment(BShipmentOrder bShipmentOrder)
        {
            int result = 0;
            string Insurance_value = "";
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            if (bShipmentOrder.Insured == BEnumFlag.Yes)
            {
                /* Update SHIPMENT_ORDER_TEMP*/
                var rr = context.uSP_GET_INSURANCE_RATE(bShipmentOrder.AccountNo, bShipmentOrder.RecipientCountry, bShipmentOrder.CarrierService, bShipmentOrder.Carrier).First();
                float rate = (float)rr.RATE;

                float ff = (rate * bShipmentOrder.DeclaredValue) / 100;
                if (ff < 5.0)
                {
                    if (rr.MIN_CHARGE > 0)
                        ff = (float)rr.MIN_CHARGE;
                }
                Insurance_value = ff.ToString().Replace(",", ".");
                if (bShipmentOrder.DeclaredValue == 0)
                {
                    Insurance_value = "0";
                }
            }
            else 
            {
                Insurance_value = "0";
            }
            System.Nullable<int> iReturnValue = context.uSP_SHIPMENT_ORDER_UPDATE(bShipmentOrder.ShipReference,
                                                                                        bShipmentOrder.Insured.ToString(),
                                                                                        (decimal)bShipmentOrder.DeclaredValue,
                                                                                        bShipmentOrder.CurrencyType,
                                                                                        bShipmentOrder.PackageType,
                                                                                        bShipmentOrder.PackageMaterial,
                                                                                        bShipmentOrder.ClosingMateiral,
                                                                                        bShipmentOrder.SenderCompany,
                                                                                        bShipmentOrder.SenderName,
                                                                                        bShipmentOrder.SenderPhone,
                                                                                        bShipmentOrder.SenderEmail,
                                                                                        bShipmentOrder.SenderAddress1,
                                                                                        bShipmentOrder.SenderAddress2,
                                                                                        bShipmentOrder.SenderAddress3,
                                                                                        bShipmentOrder.SenderState,
                                                                                        bShipmentOrder.SenderCollectDeadLine,
                                                                                        bShipmentOrder.SenderComments,
                                                                                        (bShipmentOrder.SameReturnAddress.ToString() == "Yes") ? "Y" : "N",
                                                                                        bShipmentOrder.ReturnCompany,
                                                                                        bShipmentOrder.ReturnName,
                                                                                        bShipmentOrder.ReturnPhone,
                                                                                        bShipmentOrder.ReturnAddress1,
                                                                                        bShipmentOrder.ReturnAddress2,
                                                                                        bShipmentOrder.ReturnAddress3,
                                                                                        bShipmentOrder.ReturnCity,
                                                                                        bShipmentOrder.ReturnZipCode,
                                                                                        bShipmentOrder.ReturnState,
                                                                                        bShipmentOrder.ReturnCountry,
                                                                                        bShipmentOrder.RecipientCompany,
                                                                                        bShipmentOrder.RecipientName,
                                                                                        bShipmentOrder.RecipientPhone,
                                                                                        bShipmentOrder.RecipientEmail,
                                                                                        bShipmentOrder.RecipientAddress1,
                                                                                        bShipmentOrder.RecipientAddress2,
                                                                                        bShipmentOrder.RecipientAddress3,
                                                                                        bShipmentOrder.RecipientState,
                                                                                        bShipmentOrder.RecipientDeliveryDeadLine,
                                                                                        bShipmentOrder.RecipientComments,
                                                                                        bShipmentOrder.CustomerInternalReference,
                                                                                        (decimal)bShipmentOrder.CustomsValue,
                                                                                        (bShipmentOrder.SenderNotification.ToString() == "Yes") ? "Y" : "N",
                                                                                        (bShipmentOrder.RecipientNotification.ToString() == "Yes") ? "Y" : "N",
                                                                                        bShipmentOrder.ShipDateTime,
                                                                                        bShipmentOrder.Status,
                                                                                        bShipmentOrder.SenderCity,
                                                                                        bShipmentOrder.SenderZipCode,
                                                                                        bShipmentOrder.RecipientCity,
                                                                                        bShipmentOrder.RecipientZipCode,
                                                                                        Insurance_value).SingleOrDefault();

            result = Convert.ToInt32(iReturnValue);


            return result;
        }

        /// <summary>
        /// To get all the confirmed order to ship for the session
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="strAccountNo"></param>
        /// <param name="strSessionID"></param>
        /// <param name="strStatus"></param>
        /// <returns>
        /// List of ShipmentOrders
        /// </returns>
        public List<BShipmentOrder> GetShipmentDetails(int iGroupID, string strSessionID, string strStatus, string strAccountNo)
        {
            List<BShipmentOrder> bShipmentOrder = new List<BShipmentOrder>();

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var ConfirmedShipments = context.uSP_GET_CONFIRMED_SHIPING_ORDERS(iGroupID, strSessionID, strAccountNo, strStatus).ToList();
            string strReference = "";
            foreach (var rec in ConfirmedShipments)
            {
                BShipmentOrder o = new BShipmentOrder();
                List<BShipmentDetails> bShipmentDetails = new List<BShipmentDetails>();
                BShipmentResult bShipmentResult = new BShipmentResult();
                o.CalculatedInsuranceAmount = rec.CALCULATED_INSURANCE_VALUE;
                if (strReference.Equals(rec.SHIP_REFERENCE.Trim()))
                {
                    continue;
                }
                strReference = rec.SHIP_REFERENCE.Trim();
                o.ShipReference = strReference;
                foreach (var detail in ConfirmedShipments)
                {
                    BShipmentDetails d = new BShipmentDetails();
                    if (detail.SHIP_REFERENCE.Trim().Equals(strReference))
                    {
                        d.Container = detail.CONTAINER.Trim();
                        d.ContentType = detail.CONTENT_TYPE.Trim();
                        d.DimensionUnit = detail.DIMENSION_UNIT.Trim();
                        d.Height = (float)detail.HEIGHT;
                        d.Length = (float)detail.LENGTH;
                        d.ParcelNo = detail.PARCEL_NO;
                        d.ShippingReference = detail.SHIP_REFERENCE;
                        d.TaxableWeight = (float)detail.UOD_TAXABLE_WEIGHT;
                        d.TrackingNo = "";
                        d.Weight = (float)detail.WEIGHT;
                        d.WeightUnit = detail.WEIGHT_UNIT.Trim();
                        d.Width = (float)detail.WIDTH;
                        bShipmentDetails.Add(d);
                    }
                }
                o.ShipDetail = bShipmentDetails;
                o.AccountNo = rec.ACCOUNT_NO.Trim();
                o.CalculatedDeliveryTime = rec.CALCULATED_DELIVERY_TIME;
                o.CalculatedShipDate = Convert.ToDateTime(rec.CALCULATED_SHIP_DATE);
                o.CancelResponsible = rec.CANCEL_RESPONSIBLE.Trim();
                o.CarrierService = rec.CARRIER;
                o.Carrier = rec.CARRIER_NAME;

                if (rec.CHOSEN_PREFERENCE == BEnumShipPreference.Fastest.ToString())
                    o.ChosenPreference = BEnumShipPreference.Fastest;
                else if (rec.CHOSEN_PREFERENCE == BEnumShipPreference.MostCompetitive.ToString())
                    o.ChosenPreference = BEnumShipPreference.MostCompetitive;
                else
                    o.ChosenPreference = BEnumShipPreference.NamedCarrier;

                o.ClosingMateiral = rec.CLOSING_MATERIAL;
                o.ContainerType = "";
                o.CurrencyType = rec.CURRENCY_TYPE;
                o.CustomerInternalReference = rec.CUST_INTERNAL_REFERENCE;
                o.CustomsValue = (float)rec.CUSTOMS_VALUE;
                o.DeclaredValue = (float)rec.DECLARED_VALUE;
                o.FreightAmount = (float)rec.FREIGHT_AMOUNT;
                o.FuelCharge = (float)rec.FUEL_CHARGE;
                o.Insured = (rec.INSURED == "Y") ? BEnumFlag.Yes : BEnumFlag.No;
                o.Options = rec.OPTIONS;
                o.OptionsCharges = rec.OPTIONS_CHARGES;
                o.OrderCreationTime = Convert.ToDateTime(rec.ORDER_CREATION_TIME);
                o.OrderType = (rec.ORDER_TYPE == "M") ? BEnumOrderType.Manual : BEnumOrderType.Import;
                o.PackageMaterial = rec.PACKAGE_MATERIAL;
                o.PackageType = rec.PACKAGE_TYPE;
                o.PaymentType = BEnumPaymentType.DeferredPayment;
                o.RecipientAddress1 = rec.RECIPIENT_ADDRESS1;
                o.RecipientAddress2 = rec.RECIPIENT_ADDRESS2;
                o.RecipientAddress3 = rec.RECIPIENT_ADDRESS3;
                o.RecipientCity = rec.RECIPIENT_CITY;
                o.RecipientComments = rec.RECIPIENT_COMMENTS;
                o.RecipientCompany = rec.RECIPIENT_COMPANY;
                o.RecipientCountry = rec.RECIPIENT_COUNTRY;
                o.RecipientDeliveryDeadLine = rec.RECIPIENT_DEL_DEADLINE;
                o.RecipientEmail = rec.RECIPIENT_EMAIL;
                o.RecipientName = rec.RECIPIENT_NAME;
                o.RecipientNotification = (rec.RECIPIENT_NOTIFICATION == "Y") ? BEnumFlag.Yes : BEnumFlag.No;
                o.RecipientPhone = rec.RECIPIENT_PHONE;
                o.RecipientState = rec.RECIPIENT_STATE;
                o.RecipientType = (rec.RECIPIENT_TYPE.Equals("Company")) ? BEnumAddressType.Company : BEnumAddressType.Residential;
                o.RecipientZipCode = rec.RECIPIENT_ZIP;
                o.ReturnAddress1 = rec.RETURN_ADDRESS1;
                o.ReturnAddress2 = rec.RETURN_ADDRESS2;
                o.ReturnAddress3 = rec.RETURN_ADDRESS3;
                o.ReturnCity = rec.RETURN_CITY;
                o.ReturnCompany = rec.RETURN_COMPANY;
                o.ReturnCountry = rec.RETURN_COUNTRY;
                o.ReturnName = rec.RETURN_NAME;
                o.ReturnPhone = rec.RETURN_PHONE;
                o.ReturnState = rec.RETURN_STATE;
                o.ReturnZipCode = rec.RETURN_ZIP;
                o.SameReturnAddress = (rec.SAME_RETURN_ADDRESS.Equals("Y")) ? BEnumFlag.Yes : BEnumFlag.No;
                o.SenderAddress1 = rec.SENDER_ADDRESS1;
                o.SenderAddress2 = rec.SENDER_ADDRESS2;
                o.SenderAddress3 = rec.SENDER_ADDRESS3;
                o.SenderCity = rec.SENDER_CITY;
                o.SenderCollectDeadLine = rec.SENDER_COLLECT_DEADLINE;
                o.SenderComments = rec.SENDER_COMMENTS;
                o.SenderCompany = rec.SENDER_COMPANY;
                o.SenderCountry = rec.SENDER_COUNTRY;
                o.SenderEmail = rec.SENDER_EMAIL;
                o.SenderName = rec.SENDER_NAME;
                o.SenderNotification = (rec.SENDER_NOTIFICATION.Equals("Y")) ? BEnumFlag.Yes : BEnumFlag.No;
                o.SenderPhone = rec.SENDER_PHONE;
                o.SenderState = rec.SENDER_STATE;
                o.SenderZipCode = rec.SENDER_ZIP;
                o.ShipDateTime = Convert.ToDateTime(rec.SHIPPING_TIME);
                o.ShipGroupID = rec.GROUP_ID;
                o.Status = rec.STATUS;
                o.Surcharge = rec.SURCHARGE;
                o.SurchargeDescription = rec.SURCHARGE_DESCRIPTION;
                o.TaxableWeight = (float)rec.TAXABLE_WEIGHT;
                o.TotalAmount = (float)rec.TOTAL_AMOUNT;
                o.TotalWeight = (float)rec.TOTAL_WEIGHT;
                o.UODCount = rec.UOD_COUNT;
                o.WishedShipDate = Convert.ToDateTime(rec.WISHED_SHIP_DATE);

                bShipmentResult.LabelError = "";
                bShipmentResult.isLabelGenerated = BEnumFlag.No;
                bShipmentResult.ManifestError = "";
                bShipmentResult.isManifestGenerated = BEnumFlag.No;
                bShipmentResult.FeasibilityError = "";
                bShipmentResult.isFeasibility = BEnumFlag.No;
                bShipmentResult.OtherError = "";
                bShipmentResult.isOther = BEnumFlag.No;

                o.ShipmentResult = bShipmentResult;

                bShipmentOrder.Add(o);
            }


            return bShipmentOrder;

        }

        /// <summary>
        /// To get all the Order information for the given shipment reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        /// Order information
        /// </returns>
        public BShipmentOrder GetOrderInformation(string ShipmentReference)
        {
            BShipmentOrder bShipmentOrder = new BShipmentOrder();
            BShipmentResult bShipmentResult = new BShipmentResult();

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var ConfirmedShipments = context.uSP_GET_SHIPMENT_INFO(ShipmentReference).ToList();
            string strReference = "";
            foreach (var rec in ConfirmedShipments)
            {
                List<BShipmentDetails> bShipmentDetails = new List<BShipmentDetails>();
                strReference = rec.SHIP_REFERENCE.Trim();
                bShipmentOrder.ShipReference = strReference;
                foreach (var detail in ConfirmedShipments)
                {
                    BShipmentDetails d = new BShipmentDetails();
                    if (detail.SHIP_REFERENCE.Trim().Equals(strReference))
                    {
                        d.Container = detail.CONTAINER.Trim();
                        d.ContentType = detail.CONTENT_TYPE.Trim();
                        d.DimensionUnit = detail.DIMENSION_UNIT.Trim();
                        d.Height = (float)detail.HEIGHT;
                        d.Length = (float)detail.LENGTH;
                        d.ParcelNo = detail.PARCEL_NO;
                        d.ShippingReference = detail.SHIP_REFERENCE;
                        d.TaxableWeight = (float)detail.TAXABLE_WEIGHT;
                        d.TrackingNo = "";
                        d.Weight = (float)detail.WEIGHT;
                        d.WeightUnit = detail.WEIGHT_UNIT.Trim();
                        d.Width = (float)detail.WIDTH;
                        bShipmentDetails.Add(d);
                    }
                }
                bShipmentOrder.ShipDetail = bShipmentDetails;
                bShipmentOrder.AccountNo = rec.ACCOUNT_NO.Trim();
                bShipmentOrder.CalculatedDeliveryTime = rec.CALCULATED_DELIVERY_TIME;
                bShipmentOrder.CalculatedShipDate = Convert.ToDateTime(rec.CALCULATED_SHIP_DATE);
                bShipmentOrder.CancelResponsible = rec.CANCEL_RESPONSIBLE.Trim();
                bShipmentOrder.Carrier = rec.CARRIER.Trim();

                if (rec.CHOSEN_PREFERENCE == BEnumShipPreference.Fastest.ToString())
                    bShipmentOrder.ChosenPreference = BEnumShipPreference.Fastest;
                else if (rec.CHOSEN_PREFERENCE == BEnumShipPreference.MostCompetitive.ToString())
                    bShipmentOrder.ChosenPreference = BEnumShipPreference.MostCompetitive;
                else
                    bShipmentOrder.ChosenPreference = BEnumShipPreference.NamedCarrier;

                bShipmentOrder.ClosingMateiral = rec.CLOSING_MATERIAL;
                bShipmentOrder.ContainerType = "";
                bShipmentOrder.CurrencyType = rec.CURRENCY_TYPE;
                bShipmentOrder.CustomerInternalReference = rec.CUST_INTERNAL_REFERENCE;
                bShipmentOrder.CustomsValue = (float)rec.CUSTOMS_VALUE;
                bShipmentOrder.DeclaredValue = (float)rec.DECLARED_VALUE;
                bShipmentOrder.FreightAmount = (float)rec.FREIGHT_AMOUNT;
                bShipmentOrder.FuelCharge = (float)rec.FUEL_CHARGE;
                bShipmentOrder.Insured = (rec.INSURED == "Y") ? BEnumFlag.Yes : BEnumFlag.No;
                bShipmentOrder.Options = rec.OPTIONS;
                bShipmentOrder.OptionsCharges = rec.OPTIONS_CHARGES;
                bShipmentOrder.OrderCreationTime = Convert.ToDateTime(rec.ORDER_CREATION_TIME);
                bShipmentOrder.OrderType = (rec.ORDER_TYPE == "M") ? BEnumOrderType.Manual : BEnumOrderType.Import;
                bShipmentOrder.PackageMaterial = rec.PACKAGE_MATERIAL;
                bShipmentOrder.PackageType = rec.PACKAGE_TYPE;
                bShipmentOrder.PaymentType = BEnumPaymentType.DeferredPayment;
                bShipmentOrder.RecipientAddress1 = rec.RECIPIENT_ADDRESS1;
                bShipmentOrder.RecipientAddress2 = rec.RECIPIENT_ADDRESS2;
                bShipmentOrder.RecipientAddress3 = rec.RECIPIENT_ADDRESS3;
                bShipmentOrder.RecipientCity = rec.RECIPIENT_CITY;
                bShipmentOrder.RecipientComments = rec.RECIPIENT_COMMENTS;
                bShipmentOrder.RecipientCompany = rec.RECIPIENT_COMPANY;
                bShipmentOrder.RecipientCountry = rec.RECIPIENT_COUNTRY;
                bShipmentOrder.RecipientDeliveryDeadLine = rec.RECIPIENT_DEL_DEADLINE;
                bShipmentOrder.RecipientEmail = rec.RECIPIENT_EMAIL;
                bShipmentOrder.RecipientName = rec.RECIPIENT_NAME;
                bShipmentOrder.RecipientNotification = (rec.RECIPIENT_NOTIFICATION == "Y") ? BEnumFlag.Yes : BEnumFlag.No;
                bShipmentOrder.RecipientPhone = rec.RECIPIENT_PHONE;
                bShipmentOrder.RecipientState = rec.RECIPIENT_STATE;
                bShipmentOrder.RecipientType = (rec.RECIPIENT_TYPE.Equals("Company")) ? BEnumAddressType.Company : BEnumAddressType.Residential;
                bShipmentOrder.RecipientZipCode = rec.RECIPIENT_ZIP;
                bShipmentOrder.ReturnAddress1 = rec.RETURN_ADDRESS1;
                bShipmentOrder.ReturnAddress2 = rec.RETURN_ADDRESS2;
                bShipmentOrder.ReturnAddress3 = rec.RETURN_ADDRESS3;
                bShipmentOrder.ReturnCity = rec.RETURN_CITY;
                bShipmentOrder.ReturnCompany = rec.RETURN_COMPANY;
                bShipmentOrder.ReturnCountry = rec.RETURN_COUNTRY;
                bShipmentOrder.ReturnName = rec.RETURN_NAME;
                bShipmentOrder.ReturnPhone = rec.RETURN_PHONE;
                bShipmentOrder.ReturnState = rec.RETURN_STATE;
                bShipmentOrder.ReturnZipCode = rec.RETURN_ZIP;
                bShipmentOrder.SameReturnAddress = (rec.SAME_RETURN_ADDRESS.Equals("Y")) ? BEnumFlag.Yes : BEnumFlag.No;
                bShipmentOrder.SenderAddress1 = rec.SENDER_ADDRESS1;
                bShipmentOrder.SenderAddress2 = rec.SENDER_ADDRESS2;
                bShipmentOrder.SenderAddress3 = rec.SENDER_ADDRESS3;
                bShipmentOrder.SenderCity = rec.SENDER_CITY;
                bShipmentOrder.SenderCollectDeadLine = rec.SENDER_COLLECT_DEADLINE;
                bShipmentOrder.SenderComments = rec.SENDER_COMMENTS;
                bShipmentOrder.SenderCompany = rec.SENDER_COMPANY;
                bShipmentOrder.SenderCountry = rec.SENDER_COUNTRY;
                bShipmentOrder.SenderEmail = rec.SENDER_EMAIL;
                bShipmentOrder.SenderName = rec.SENDER_NAME;
                bShipmentOrder.SenderNotification = (rec.SENDER_NOTIFICATION.Equals("Y")) ? BEnumFlag.Yes : BEnumFlag.No;
                bShipmentOrder.SenderPhone = rec.SENDER_PHONE;
                bShipmentOrder.SenderState = rec.SENDER_STATE;
                bShipmentOrder.SenderZipCode = rec.SENDER_ZIP;
                bShipmentOrder.ShipDateTime = Convert.ToDateTime(rec.SHIPPING_TIME);
                bShipmentOrder.ShipGroupID = rec.GROUP_ID;
                bShipmentOrder.Status = rec.STATUS;
                bShipmentOrder.Surcharge = rec.SURCHARGE;
                bShipmentOrder.SurchargeDescription = rec.SURCHARGE_DESCRIPTION;
                bShipmentOrder.TaxableWeight = (float)rec.TAXABLE_WEIGHT;
                bShipmentOrder.TotalAmount = (float)rec.TOTAL_AMOUNT;
                bShipmentOrder.TotalWeight = (float)rec.TOTAL_WEIGHT;
                bShipmentOrder.UODCount = rec.UOD_COUNT;
                bShipmentOrder.WishedShipDate = Convert.ToDateTime(rec.WISHED_SHIP_DATE);

                bShipmentResult.LabelError = "";
                bShipmentResult.isLabelGenerated = BEnumFlag.No;
                bShipmentResult.ManifestError = "";
                bShipmentResult.isManifestGenerated = BEnumFlag.No;
                bShipmentResult.FeasibilityError = "";
                bShipmentResult.isFeasibility = BEnumFlag.No;
                bShipmentResult.OtherError = "";
                bShipmentResult.isOther = BEnumFlag.No;

                bShipmentOrder.ShipmentResult = bShipmentResult;

            }


            return bShipmentOrder;

        }

        /// <summary>
        /// To get all the Payment method and infor of the user
        /// </summary>
        /// <param name="strAccountNo"></param>
        /// <param name="strUserType"></param>
        /// <returns>
        /// Payment method and admin id
        /// </returns>
        public BPaymentInfo GetPaymentMethodAndInfo(string strAccountNo, string strUserType)
        {
            BPaymentInfo bPaymentInfo = new BPaymentInfo();

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

            ObjectParameter objParamAdminMail = new ObjectParameter("ADMIN_MAIL", typeof(String));
            ObjectParameter objParamAdminPhone = new ObjectParameter("ADMIN_PHONE", typeof(String));
            ObjectParameter objParamAvailableCredit = new ObjectParameter("AVAILABLE_CREDIT", typeof(Decimal));
            ObjectParameter objParamPaymentMethod = new ObjectParameter("PAYMENT_METHOD", typeof(String));
            ObjectParameter objParamUserMail = new ObjectParameter("USER_MAIL", typeof(String));

            System.Nullable<int> iReturnValue1 = context.uSP_GET_PAYMENT_INFO(strAccountNo, strUserType, objParamAdminMail, objParamAdminPhone, objParamAvailableCredit, objParamPaymentMethod, objParamUserMail).SingleOrDefault();

            string paymentMethod = objParamPaymentMethod.Value.ToString();
            if (paymentMethod.Trim().Equals(""))
            {
                bPaymentInfo.AdminMailID = objParamAdminMail.Value.ToString();
                bPaymentInfo.AdminPhone = objParamAdminPhone.Value.ToString();
                bPaymentInfo.AvailableCreditLimit = 0;
                bPaymentInfo.PaymentMethod = objParamPaymentMethod.Value.ToString();
                bPaymentInfo.UserMailId = objParamUserMail.Value.ToString();

            }
            else
            {
                bPaymentInfo.AdminMailID = objParamAdminMail.Value.ToString();
                bPaymentInfo.AdminPhone = objParamAdminPhone.Value.ToString();
                bPaymentInfo.AvailableCreditLimit = (float)Convert.ToDecimal(objParamAvailableCredit.Value);
                bPaymentInfo.PaymentMethod = objParamPaymentMethod.Value.ToString();
                bPaymentInfo.UserMailId = objParamUserMail.Value.ToString();
            }

            return bPaymentInfo;
        }

        /// <summary>
        /// To get all the Deferred payment process
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <param name="ClosedReference"></param>
        /// <param name="fTotalAmount"></param>
        /// <returns>
        /// Updates the closed shipments and deduct total amount from available limit 
        /// </returns>
        public int DeferredPayment(List<string> ClosedReference, string AccountNo, float fTotalAmount)
        {
            int result = 0;
            try
            {
                string strClosedReference = "";//"''";
                for (int i = 0; i < ClosedReference.Count; i++)
                {
                    strClosedReference += ClosedReference[i] + "', '";
                }

                strClosedReference = strClosedReference.Substring(0, (strClosedReference.Length - 4));
                strClosedReference = "'" + strClosedReference + "'";

                KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

                System.Nullable<int> iReturnValue1 = context.uSP_DEFERRED_PAYMENT(strClosedReference, AccountNo, (Decimal)fTotalAmount).SingleOrDefault();

            }
            catch (Exception error)
            {


            }
            return result;
        }

        /// <summary>
        /// Delete the shipment by passing shipment reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        ///  0 => Success
        ///  1 => Fail
        ///  2 => Failed die to shipment reference not available
        /// </returns>
        public int DeleteShipment(string ShipmentReference)
        {
            int result = 0;

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            System.Nullable<int> iReturnValue1 = context.uSP_SHIPMENT_ORDER_DELETE(ShipmentReference).SingleOrDefault();
            result = (Int32)iReturnValue1;
            return result;
        }

        /// <summary>
        /// Cancel the shipment by passing shipment reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        ///  0 => Success
        ///  1 => Fail
        ///  2 => Failed die to shipment reference not available
        /// </returns>
        public int CancelShipment(string ShipmentReference)
        {
            int result = 0;

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            System.Nullable<int> iReturnValue1 = context.uSP_SHIPMENT_ORDER_CANCEL(ShipmentReference).SingleOrDefault();
            result = (Int32)iReturnValue1;
            return result;
        }

        /// <summary>
        /// Use to receive bulk of order in a file and process internaly one by one using CreateSingleShipment() + GetQoute()   (Use case : 2.2)
        /// </summary>
        /// <param name="shipmentFile"></param>
        /// <returns>
        /// Row 0 => Number of success + Success in FieldName
        /// Row 0 => Number of Fail + Failed in FieldName 
        /// in all other positive row number will have Row ID + Field Name + Error description
        /// </returns>
        public BFileImportStatus CreateBulkShippment(byte[] shipmentFile)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Propose optimal carrier for the shipment with quote
        /// </summary>
        /// <param name="bShipmentOrder">Shipment Order information</param>
        /// <returns>
        /// List of carriers with quote
        /// </returns>
        public List<BShipmentQuotation> GetQuote(BShipmentOrder bShipmentOrder)
        {

            List<BShipmentQuotation> bShipmentQuotation = new List<BShipmentQuotation>();
            List<BTariffInfo> bTariffDetail = new List<BTariffInfo>();

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            #region Priority
            string strPriority = "";
            string strCarrier = "";
            string strService = "";
            string strAssignedTariff = "";
            string strTariffReference = "";
            string strTariffTable = "";
            string strTariffType = ""; // K => Key type + N => Normal tariff
            string strDeliveryDelay = "";

            // From object needs to stored
            string strCustomerType = BEnumCustomerType.KeyCustomer.ToString();

            strAssignedTariff = getAssignedTariffMarigin(bShipmentOrder.AccountNo);

            var Priority = context.uSP_GET_PRIORITY().ToList();

            for (int pCount = 0; pCount < Priority.Count; pCount++)
            {
                strPriority = Priority[pCount].MASTER_PRIORITY.ToString();
                var Carrier = context.uSP_GET_CARRIER_SERVICE_LIST(strPriority).ToList();

                #region service
                foreach (var service in Carrier)
                {
                    BTariffInfo td = new BTariffInfo();
                    BShipmentQuotation q = new BShipmentQuotation();
                    BCarrierProcessResult bCarrierProcessResult = new BCarrierProcessResult();
                    strCarrier = service.CARRIER_NAME;
                    strService = service.SERVICE_NAME;
                    strDeliveryDelay = service.DELIVERY_DELAY;

                    BShipmentOrder Order = new BShipmentOrder();

                    CarrierHandler oCarrierHandler = new CarrierHandler();
                    bShipmentOrder.Carrier = strService;

                    Order = oCarrierHandler.CarrierProcess(bShipmentOrder, BEnumCarrierProcess.Feasable, out bCarrierProcessResult);

                    // If any failure in Carrier communication (exceptions) its treated as not feasable.
                    if (Order.ShipmentResult.isFeasibility == BEnumFlag.No)
                    {
                        continue;
                    }

                    ObjectParameter objParamTariffReference = new ObjectParameter("TARIFF_REFERENCE", typeof(String));
                    ObjectParameter objParamTariffTable = new ObjectParameter("TARIFF_TABLE", typeof(String));
                    ObjectParameter objParamTariffType = new ObjectParameter("TARIFF_TYPE", typeof(String));

                    System.Nullable<int> iReturnValue1 = context.uSP_GET_TARIFF_DETAILS(strCarrier, bShipmentOrder.AccountNo, bShipmentOrder.ContainerType, objParamTariffReference, objParamTariffTable, objParamTariffType).SingleOrDefault();
                    strTariffReference = objParamTariffReference.Value.ToString();
                    strTariffTable = objParamTariffTable.Value.ToString();
                    strTariffType = objParamTariffType.Value.ToString();

                    //23FEB12HN To skip the carrier which is having no Tariff for the requirement.
                    if (strTariffReference.Equals(""))
                        continue;

                    td.TariffName = objParamTariffReference.Value.ToString();
                    td.TariffTable = objParamTariffTable.Value.ToString();
                    td.TariffType = objParamTariffTable.Value.ToString();
                    decimal f = 12345678.78m;  //17FEB12HN

                    ObjectParameter objCalculatedTariff = new ObjectParameter("CALCULATED_TARIFF", f);
                    ObjectParameter objDeliveryDelay = new ObjectParameter("DELIVERY_DELAY", typeof(Int32));
                    ObjectParameter objMargin = new ObjectParameter("MARGIN", f);
                    ObjectParameter objFuelSurcharge = new ObjectParameter("FUEL_SURCHARGE", f);
                    ObjectParameter objOptionsValue = new ObjectParameter("OPTION_VALUES", typeof(String));
                    ObjectParameter objSurchargeValue = new ObjectParameter("SURCHARGE_VALUES", typeof(String));
                    ObjectParameter objOptionsDescriptions = new ObjectParameter("OPTION_DESCRIPTION", typeof(String));
                    ObjectParameter objSurchargeDescriptions = new ObjectParameter("SURCHARGE_DESCRIPTION", typeof(String));
                    ObjectParameter objDeliveryDate = new ObjectParameter("DELIVERY_DATE", typeof(DateTime));
                    ObjectParameter objOptionsTotal = new ObjectParameter("OPTION_TOTAL", f);
                    ObjectParameter objSurchargeTotal = new ObjectParameter("SURCHARGE_TOTAL", f);
                    double Total = 0f;
                    if (strCarrier.Trim().Equals("GLS"))
                    {

                        foreach (BShipmentDetails ss in bShipmentOrder.ShipDetail)
                        {
                            System.Nullable<int> iReturnValue2 = context.uSP_GET_TARIFF_AND_DELIVERY_DETAILS(strService, strAssignedTariff, strTariffReference, bShipmentOrder.SenderCountry, bShipmentOrder.RecipientCountry, bShipmentOrder.RecipientZipCode, bShipmentOrder.ShipDateTime, strTariffTable, strDeliveryDelay, (decimal)ss.Weight, strCustomerType, bShipmentOrder.AccountNo, 2, 3, 2, bShipmentOrder.ContainerType, bShipmentOrder.UODCount, "R", "", objCalculatedTariff, objDeliveryDelay, objMargin, objFuelSurcharge, objOptionsValue, objSurchargeValue, objOptionsDescriptions, objSurchargeDescriptions, objDeliveryDate).SingleOrDefault();
                            if (isNumericValidation(objCalculatedTariff.Value.ToString(), System.Globalization.NumberStyles.Float))
                            {
                                Total = Total + float.Parse(objCalculatedTariff.Value.ToString());
                            }


                        }
                    }
                    else
                    {
                        System.Nullable<int> iReturnValue2 = context.uSP_GET_TARIFF_AND_DELIVERY_DETAILS(strService, strAssignedTariff, strTariffReference, bShipmentOrder.SenderCountry, bShipmentOrder.RecipientCountry, bShipmentOrder.RecipientZipCode, bShipmentOrder.ShipDateTime, strTariffTable, strDeliveryDelay, (decimal)bShipmentOrder.TotalWeight, strCustomerType, bShipmentOrder.AccountNo, 2, 3, 2, bShipmentOrder.ContainerType, bShipmentOrder.UODCount, "R", "", objCalculatedTariff, objDeliveryDelay, objMargin, objFuelSurcharge, objOptionsValue, objSurchargeValue, objOptionsDescriptions, objSurchargeDescriptions, objDeliveryDate).SingleOrDefault();
                    }
                    System.Nullable<int> SS = context.uSP_GET_OPTIONCHARGE(strTariffReference, bShipmentOrder.UODCount, (decimal)bShipmentOrder.TotalWeight, 2, 2, 2, bShipmentOrder.Options/*"'Deliver on saturday','Deliver in Relais Colis'"*/, objOptionsValue, objOptionsDescriptions, objOptionsTotal).SingleOrDefault();
                    string container_type = bShipmentOrder.ShipDetail[0].Container;
                    foreach (BShipmentDetails bo in bShipmentOrder.ShipDetail)
                    {
                        if (bo.Container == "Pallet")
                        {
                            container_type = "PALLET";
                        }
                    }
                    string resedenttype = "";
                    if (bShipmentOrder.RecipientType == BEnumAddressType.Residential)
                    {
                        resedenttype = "RES";
                    }


                    System.Nullable<int> SS1 = context.GET_SURCHANGE_SHIPPING(strTariffReference, strCarrier, bShipmentOrder.SenderCountry, bShipmentOrder.RecipientCountry, bShipmentOrder.ShipReference, bShipmentOrder.UODCount, (decimal)bShipmentOrder.TotalWeight, 1, 1, 1, container_type, bShipmentOrder.SenderCity, bShipmentOrder.SenderZipCode, bShipmentOrder.RecipientCity, bShipmentOrder.RecipientZipCode, strCarrier, strService, resedenttype, objSurchargeValue, objSurchargeDescriptions, objSurchargeTotal).SingleOrDefault();

                    bool isTariffAvailable = true;



                    if (isNumericValidation(objCalculatedTariff.Value.ToString(), System.Globalization.NumberStyles.Float))
                    {
                        if (strCarrier.Trim().Equals("GLS"))
                        {
                            q.CalculatedTariff = Total;
                        }
                        else
                            q.CalculatedTariff = float.Parse(objCalculatedTariff.Value.ToString());
                    }
                    else
                    {
                        q.CalculatedTariff = 0f;
                        isTariffAvailable = false;
                    }

                    //q.FuelSurcharge = Convert.ToDouble(objFuelSurcharge.Value);
                    if (isNumericValidation(objFuelSurcharge.Value.ToString(), System.Globalization.NumberStyles.Float))
                    {
                        q.FuelSurcharge = float.Parse(objFuelSurcharge.Value.ToString());
                    }
                    else
                    {
                        q.FuelSurcharge = 0f;
                    }


                    q.CarrierName = strCarrier;
                    q.CarrierPriority = strPriority;
                    q.CarrierType = strTariffType;
                    string DlDate = objDeliveryDate.Value.ToString();
                    q.DeliveryDate = Convert.ToDateTime(DlDate);
                    //q.FuelSurcharge = Convert.ToDouble(objFuelSurcharge.Value);
                    if (objOptionsValue.Value.ToString().Trim().Equals(""))
                    {
                        q.Options = "";
                        q.OptionsDescription = "";
                    }
                    else
                    {
                        q.Options = objOptionsValue.Value.ToString();
                        q.OptionsDescription = objOptionsDescriptions.Value.ToString();
                    }
                    q.ServiceName = strService;
                    //q.ShippingDate = Convert.ToDateTime(objDeliveryDate.Value.ToString());
                    q.ShippingDate = bShipmentOrder.ShipDateTime;

                    q.Surcharge = objSurchargeValue.Value.ToString();

                    q.SurchargeDescription = objSurchargeDescriptions.Value.ToString();


                    float CalculatedTariff = 0;
                    if (isTariffAvailable)
                    {
                        if (strCarrier.Trim().Equals("TNT") || strCarrier.Trim().Equals("TNT INT"))
                        {
                            CalculatedTariff = (float)Convert.ToDouble(objCalculatedTariff.Value);
                        }
                        else if (strCarrier.Trim().Equals("GLS"))
                        {
                            CalculatedTariff = (float)Total + (float)Convert.ToDouble(objSurchargeTotal.Value);//(float)Convert.ToDouble(objSurchargeTotal.Value);
                        }
                        ObjectParameter objFuelCharge = new ObjectParameter("RESULT", f); //07FEB12HN

                        System.Nullable<int> iReturnValue3 = context.uSP_GET_FUEL_SURCHARGE(0, strService, "", "", "", (decimal)CalculatedTariff, objFuelCharge).SingleOrDefault();

                        q.FuelSurcharge = (float)Convert.ToDecimal(objFuelCharge.Value.ToString());
                        bShipmentQuotation.Add(q);

                    }

                }
                #endregion service
            }
            #endregion Priority
            return bShipmentQuotation;

        }

        ///<summary>
        ///For getting assigned tariff for the given account
        /// </summary>
        /// <param name="strAccountNo"></param>
        /// <returns>
        /// Assigned tariff type
        /// </returns>
        private string getAssignedTariffMarigin(string strAccountNo)
        {
            string strAssignedTariffMargin = "";
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            ObjectParameter objAssignedTariffMargin = new ObjectParameter("ASSIGNED_TARIFF", typeof(String));

            System.Nullable<int> iReturnValue = context.uSP_GET_ASSIGNED_TARIFF(strAccountNo, objAssignedTariffMargin).SingleOrDefault();

            strAssignedTariffMargin = objAssignedTariffMargin.Value.ToString();

            return strAssignedTariffMargin;
        }

        /// <summary>
        /// Carrier Process
        /// </summary>
        /// <param name="paymentDetails"></param>
        ///<returns>
        ///
        /// </returns> 
        public void TransferToBankGateway(PaymentDetails paymentDetails)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Carrier Process
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        /// 
        /// </returns>
        public int CarrierProcess(List<BShipmentOrder> bShipmentOrder)
        {
            int result = 0;


            return result;
        }


        /*********************[21FEB12KS]************************/
        public string getBrokerEmailId()
        {
            string result = "";
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            result = context.uSP_GET_INSURANCE_BROKER().SingleOrDefault();
            return result;
        }

        public int UpdateBrokerEmailId(string Email)
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            return (int)context.uSP_INSURANCE_BROKER_UPDATE(Email).SingleOrDefault();
        }

        /************ [24FEB12HN] ************************/
        /// <summary>
        /// To get Taxable weight for the given Weight and Dimension for the Carrier
        /// </summary>
        /// <param name="CarrierName"></param>
        /// <param name="Origin"></param>
        /// <param name="Destination"></param>
        /// <param name="Zipcode"></param>
        /// <param name="Weight"></param>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public float GetTaxableWeight(string CarrierName, string Origin, string Destination, string Zipcode, float Weight, float Height, float Width, float Length)
        {
            float result = 0;

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            
            decimal f = 12345678.78m;  //17FEB12HN

            ObjectParameter objTaxableWeight = new ObjectParameter("TAXABLE_WEIGHT", f);

            System.Nullable<int> iReturnValue1 = context.uSP_GET_TAXABLE_WEIGHT(CarrierName, Origin, Destination, Zipcode, (decimal)Weight, (decimal)Height, (decimal)Width, (decimal)Length, objTaxableWeight).SingleOrDefault();
            
            result = (float) Convert.ToDouble(objTaxableWeight.Value);

            return result;
        }

        /// <summary>
        /// To update Carrier details for the given reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <param name="TrackingNumber"></param>
        /// <param name="ParameterValues"></param>
        /// <returns></returns>
        public int ShipmentDetailUpdate(string ShipmentReference, string TrackingNumber, string ParameterValues)
        {
            int result = 0;

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

            System.Nullable<int> iReturnValue1 = context.uSP_SHIPMENT_DETAIL_UPDATE(ShipmentReference,TrackingNumber,ParameterValues).SingleOrDefault();

            result = (int)iReturnValue1;

            return result;
        }


        protected bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

    }
}
