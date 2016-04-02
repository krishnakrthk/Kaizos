using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class rptTariffDelayInterrogation1 : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));
        KaizosServiceAgent proxy = new KaizosServiceAgent();
        SShipmentOrder sShipmentOrder = new SShipmentOrder();
        List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
        List<SShipmentQuotation> sShipmentQuotation = new List<SShipmentQuotation>();
        string sessionID;
        string DeliveryTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = KaizosSession.Current.SessionID;
            sShipmentOrder = fillOrder();
            sShipmentDetails = sShipmentOrder.ShipDetail.ToList();
            
            if (!Page.IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmTariffDelayInterrogation").ToString();
                //rdCarrierSelection1.Checked = true;
                try
                {

                    string strContainerType = "";
                    for (int i = 0; i < sShipmentDetails.Count; i++)
                    {
                        if (!(strContainerType.Contains(sShipmentDetails[i].Container)))
                            strContainerType = strContainerType + sShipmentDetails[i].Container + ",";
                    }

                    strContainerType = strContainerType.Substring(0, strContainerType.Length - 1).Trim();
                    sShipmentOrder.ContainerType = strContainerType;

                    if ((KaizosSession.Current.checking == 0) || (KaizosSession.Current.checking == 10))
                    {
                        sShipmentQuotation = proxy.GetQuote(sShipmentOrder).ToList();
                        KaizosSession.Current.sshipmentquotation = sShipmentQuotation;
                    }
                    else if (KaizosSession.Current.checking == 1)
                    {
                        sShipmentQuotation = KaizosSession.Current.sshipmentquotation;
                    }

                   

                    sShipmentQuotation = sShipmentQuotation.OrderByDescending(s => s.CalculatedTariff).ThenBy(s => s.DeliveryDate).ToList();

                    if (sShipmentQuotation.Count == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmTariffDelayInterrogation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "NoCarrierSelected").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        # region looping into Retrived carriers

                        string[] strSurchargeDescription;
                        string[] strSurchargeValue;
                        string strToolTip = "";
                        string[] strOptionsDescription;
                        string[] strOptionsValue;


                        foreach (var q in sShipmentQuotation)
                        {
                            float CalculatedTariff = 0;
                            float Options = 0;
                            float Surcharge = 0;
                            float Fuel = 0;

                            strSurchargeDescription = q.SurchargeDescription.ToString().Split(',');
                            strSurchargeValue = q.Surcharge.ToString().Split(',');

                            strToolTip = strToolTip + "Freight : " + q.CalculatedTariff.ToString("####0.00") + " + ";
                            strToolTip = strToolTip + "\nFuel Surcharge : " + q.FuelSurcharge.ToString("####0.00");
                            Fuel = (float)q.FuelSurcharge;

                            if (strSurchargeDescription.Length > 0)
                            {
                                strToolTip = strToolTip + Environment.NewLine + "\nSurcharges : ";
                            }

                            for (int i = 0; i < strSurchargeDescription.Length; i++)
                            {
                                strToolTip = strToolTip + strSurchargeDescription[i].Trim() + " = " + strSurchargeValue[i].Trim() + " + ";
                                Surcharge += (float)Convert.ToDecimal(strSurchargeValue[i]);
                            }
                            if (q.Options.ToString() == "")
                            {
 
                            }
                            else
                            {
                                strOptionsDescription = q.OptionsDescription.ToString().Split(',');
                                strOptionsValue = q.Options.ToString().Split(',');


                                if (strOptionsDescription.Length > 0)
                                {
                                    strToolTip = strToolTip + Environment.NewLine + "\nOptions : ";
                                }

                                for (int i = 0; i < strOptionsDescription.Length; i++)
                                {
                                    strToolTip = strToolTip + strOptionsDescription[i].Trim() + " = " + strOptionsValue[i].Trim() + " + ";
                                    Options += (float)Convert.ToDecimal(strOptionsValue[i]);
                                }
                            }
                            strToolTip = strToolTip.Substring(0, (strToolTip.Length - 2));

                            CalculatedTariff = (float)q.CalculatedTariff;
                            CalculatedTariff += Fuel + Options + Surcharge;
                            //NR - 20/04/12 bug 1357
                            CalculatedTariff = (float)System.Math.Round(CalculatedTariff, 2);
                            
                            if (q.CarrierPriority.ToString().ToUpper().Equals("EXP"))
                            {
                                if (q.CarrierType.ToString().Equals("K"))
                                {
                                    //NR - 04/05/2012 - bug#1357
                                    rdCarrierSelection1.Checked = true;

                                    trExp.Visible = true;
                                    lblKeyCarrier.Text = q.CarrierName.ToString();
                                    lblKeyDelivery.Text = q.DeliveryDate.ToString("dd/MM/yyyy HH:mm");
                                    //lblKeyTariff.Text = q.CalculatedTariff.ToString();
                                    lblKeyTariff.Text = CalculatedTariff.ToString();
                                    lblKeyService.Text = q.ServiceName.ToString();
                                    lblKeyTariff.ToolTip = strToolTip;
                                    lblKeyShipping.Text = q.ShippingDate.ToString("dd/MM/yyyy");
                                    hdKeyFuelCharge.Value = q.FuelSurcharge.ToString();
                                    hdKeyOptionsDescription.Value = q.OptionsDescription.ToString();
                                    hdKeyOptionsValue.Value = q.Options.ToString();
                                    hdKeySurchargeDescription.Value = q.SurchargeDescription.ToString();
                                    hdKeySurchargeValue.Value = q.Surcharge.ToString();
                                    strToolTip = "";
                                }
                                else
                                {
                                    //NR - 04/05/2012 - bug#1357
                                    rdCarrierSelection2.Checked = true;

                                    td1.Visible = false;
                                    td2.Visible = false;
                                    td3.Visible = false;
                                    td4.Visible = false;
                                    td5.Visible = false;
                                    td6.Visible = false;

                                    lblCarrier.Text = q.CarrierName.ToString();
                                    lblDelivery.Text = q.DeliveryDate.ToString("dd/MM/yyyy HH:mm");
                                    //lblTariff.Text = q.CalculatedTariff.ToString();
                                    lblTariff.Text = CalculatedTariff.ToString();
                                    lblService.Text = q.ServiceName.ToString();
                                    lblTariff.ToolTip = strToolTip;
                                    lblShipping.Text = q.ShippingDate.ToString("dd/MM/yyyy");
                                    hdFuelCharge.Value = q.FuelSurcharge.ToString();
                                    hdOptionsDescription.Value = q.OptionsDescription.ToString();
                                    hdOptionsValue.Value = q.Options.ToString();
                                    hdSurchargeDescription.Value = q.SurchargeDescription.ToString();
                                    hdSurchargeValue.Value = q.Surcharge.ToString();
                                    strToolTip = "";
                                }
                            }
                            else
                            {
                                if (q.CarrierType.ToString().Equals("K"))
                                {
                                    trEco.Visible = true;
                                    lblEcoKeyCarrier.Text = q.CarrierName.ToString();
                                    lblEcoKeyDelivery.Text = q.DeliveryDate.ToString("dd/MM/yyyy HH:mm");
                                    //lblEcoKeyTariff.Text = q.CalculatedTariff.ToString();
                                    lblEcoKeyTariff.Text = CalculatedTariff.ToString();
                                    lblEcoKeyService.Text = q.ServiceName.ToString();
                                    lblEcoKeyTariff.ToolTip = strToolTip;
                                    lblEcoKeyShipping.Text = q.ShippingDate.ToString("dd/MM/yyyy");
                                    hdEcoKeyFuelCharge.Value = q.FuelSurcharge.ToString();
                                    hdEcoKeyOptionsDescription.Value = q.OptionsDescription.ToString();
                                    hdEcoKeyOptionsValue.Value = q.Options.ToString();
                                    hdEcoKeySurchargeDescription.Value = q.SurchargeDescription.ToString();
                                    hdEcoKeySurchargeValue.Value = q.Surcharge.ToString();
                                    strToolTip = "";
                                }
                                else
                                {
                                    td1.Visible = false;
                                    td2.Visible = false;
                                    td3.Visible = false;
                                    td4.Visible = false;
                                    td5.Visible = false;
                                    td6.Visible = false;

                                    lblEcoCarrier.Text = q.CarrierName.ToString();
                                    lblEcoDelivery.Text = q.DeliveryDate.ToString("dd/MM/yyyy HH:mm");
                                    //lblEcoTariff.Text = q.CalculatedTariff.ToString();
                                    lblEcoTariff.Text = CalculatedTariff.ToString();
                                    lblEcoService.Text = q.ServiceName.ToString();
                                    lblEcoTariff.ToolTip = strToolTip;
                                    lblEcoShipping.Text = q.ShippingDate.ToString("dd/MM/yyyy");
                                    hdEcoFuelCharge.Value = q.FuelSurcharge.ToString();
                                    hdEcoOptionsDescription.Value = q.OptionsDescription.ToString();
                                    hdEcoOptionsValue.Value = q.Options.ToString();
                                    hdEcoSurchargeDescription.Value = q.SurchargeDescription.ToString();
                                    hdEcoSurchargeValue.Value = q.Surcharge.ToString();
                                    strToolTip = "";
                                }
                            }
                        }

                        #endregion looping into Retrived carriers
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmTariffDelayInterrogation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualCarrierSelection").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "PageLoad", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmTariffDelayInterrogation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualCarrierSelection").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
        }

        private SShipmentOrder fillOrder()
        {
            SShipmentOrder sShipmentOrder = new SShipmentOrder();
            SShipmentResult sShipmentResult = new SShipmentResult();


            sShipmentOrder.AccountNo = KaizosSession.Current.AccountNo; // "123";
            sShipmentOrder.RecipientCountry = KaizosSession.Current.RecipientCountryCode;   // "AD";

            sShipmentOrder.RecipientType = KaizosSession.Current.RecipientAddressType; //KaizosSession.Current. SEnumAddressType.Company;
            sShipmentOrder.RecipientZipCode = KaizosSession.Current.RecipientZip; //"R Zip";
            sShipmentOrder.SenderCountry = KaizosSession.Current.SenderCountryCode; //"FR";
            sShipmentOrder.SenderZipCode = KaizosSession.Current.SenderZip; //"S Zip";
            sShipmentOrder.ShipDetail = KaizosSession.Current.ShipmentDetail.ToArray();
            sShipmentOrder.TotalWeight = KaizosSession.Current.GrossWeight;
            sShipmentOrder.UODCount = KaizosSession.Current.ParcelCount;
            sShipmentOrder.ShipDateTime = KaizosSession.Current.ShippingDate;
            sShipmentOrder.RecipientCity = KaizosSession.Current.RecipientCity;
            sShipmentOrder.SenderCity = KaizosSession.Current.SenderCity;


            sShipmentOrder.CalculatedDeliveryTime = "";  // "DL:TM";
            sShipmentOrder.CalculatedShipDate = DateTime.Now;
            sShipmentOrder.CancelResponsible = "";   //"CancelRes";
            sShipmentOrder.Carrier = "";     //"Carrier";
            sShipmentOrder.ChosenPreference = SEnumShipPreference.Fastest;
            sShipmentOrder.ClosingMateiral = "";     //"Closing Material";
            sShipmentOrder.CurrencyType = "";       //"CUR";
            sShipmentOrder.CustomerInternalReference = "";  //"Internal";
            sShipmentOrder.CustomsValue = 0;
            sShipmentOrder.DeclaredValue = 0;
            sShipmentOrder.FreightAmount = 0;
            sShipmentOrder.FuelCharge = 0;
            sShipmentOrder.Insured = SEnumFlag.Yes;
            sShipmentOrder.Options = KaizosSession.Current.Options;          //"Options";
            sShipmentOrder.OptionsCharges = "";      //"Opt Charges";
            sShipmentOrder.OrderCreationTime = DateTime.Now;
            sShipmentOrder.OrderType = SEnumOrderType.Import;
            sShipmentOrder.PackageMaterial = "";    //"Package";
            sShipmentOrder.PackageType = "";        // "Pack Type";
            sShipmentOrder.PaymentType = SEnumPaymentType.CreditCard;
            sShipmentOrder.RecipientAddress1 = "";  //"R A1";
            sShipmentOrder.RecipientAddress2 = "";  //"R A2";
            sShipmentOrder.RecipientAddress3 = "";  //"R A3";
            sShipmentOrder.RecipientComments = "";   //" R Comment";
            sShipmentOrder.RecipientCompany = "";   //"R Comp";
            sShipmentOrder.RecipientDeliveryDeadLine = "";  //"DL:DL";
            sShipmentOrder.RecipientEmail = "";     //"R Email";
            sShipmentOrder.RecipientName = "";      //"R Name";
            sShipmentOrder.RecipientNotification = SEnumFlag.No;
            sShipmentOrder.RecipientPhone = "";     //"R Phone";
            sShipmentOrder.RecipientState = "";     //"R State";
            sShipmentOrder.ReturnAddress1 = "";     //"RT A1";
            sShipmentOrder.ReturnAddress2 = "";     //"RT A2";
            sShipmentOrder.ReturnAddress3 = "";     //"RT A3";
            sShipmentOrder.ReturnCity = "";         //"RT City";
            sShipmentOrder.ReturnCompany = "";      //"TR Company";
            sShipmentOrder.ReturnCountry = "";      //"RT Country";
            sShipmentOrder.ReturnName = "";         //"RT Name";
            sShipmentOrder.ReturnPhone = "";        //"RT Phone";
            sShipmentOrder.ReturnState = "";        //"RT State";
            sShipmentOrder.ReturnZipCode = "";      //"RT Zip";
            sShipmentOrder.SameReturnAddress = SEnumFlag.Yes;
            sShipmentOrder.SenderAddress1 = "";     //"S A1";
            sShipmentOrder.SenderAddress2 = "";     //"S A2";
            sShipmentOrder.SenderAddress3 = "";     //"S A3";
            sShipmentOrder.SenderCollectDeadLine = "";  //"CL:DL";
            sShipmentOrder.SenderComments = "";         //"S Comment";
            sShipmentOrder.SenderCompany = "";          //"S Company";
            sShipmentOrder.SenderEmail = "";        //"S Email";
            sShipmentOrder.SenderName = "";         //"S Name";
            sShipmentOrder.SenderNotification = SEnumFlag.Yes;
            sShipmentOrder.SenderPhone = "";    //"S Phone";
            sShipmentOrder.SenderState = "";        //"S State";
            sShipmentOrder.ShipGroupID = 0; //1111;
            sShipmentOrder.ShipReference = "";  //"ShipRef";
            sShipmentOrder.Status = ""; //"Status";
            sShipmentOrder.TaxableWeight = 0; //4;
            sShipmentOrder.TotalAmount = 0; //5;
            sShipmentOrder.WishedShipDate = DateTime.Now;
            sShipmentOrder.ContainerType = "";

            sShipmentResult.LabelError = "";
            sShipmentResult.isLabelGenerated = SEnumFlag.No;
            sShipmentResult.ManifestError = "";
            sShipmentResult.isManifestGenerated = SEnumFlag.No;
            sShipmentResult.FeasibilityError = "";
            sShipmentResult.isFeasibility = SEnumFlag.No;
            sShipmentResult.OtherError = "";
            sShipmentResult.isOther = SEnumFlag.No;

            sShipmentOrder.ShipmentResult = sShipmentResult;

            return sShipmentOrder;

        }

        protected void btnShipIt_Click(object sender, EventArgs e)
        {
            try
            {
                /* Generate Shipment reference */
                SNextcounter sNextCounter = new SNextcounter();
                SShipmentQuotation sSelectedCarrier = new SShipmentQuotation();
                string strShipmentRef;
                sNextCounter = proxy.GetNextCounter("SHIPING_REF", "SYSTEM", 1);
                strShipmentRef = sNextCounter.NextCounter.ToString();
                sShipmentOrder.ShipReference = strShipmentRef;
                sSelectedCarrier = getSelectedCarrier();
                sShipmentOrder.CalculatedShipDate = sSelectedCarrier.DeliveryDate;

                sShipmentOrder.CalculatedDeliveryTime = DeliveryTime;

                proxy.CreateSingleShipment(sShipmentOrder, sSelectedCarrier, sessionID);

                KaizosSession.Current.ShipReference = strShipmentRef;
                KaizosSession.Current.CarrierName = sSelectedCarrier.CarrierName;
                KaizosSession.Current.ServiceName = sSelectedCarrier.ServiceName;

                Response.Redirect("frmManualShipping.aspx");
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "rptTariffDelayInterrogation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipCreation").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnShipIt_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "rptTariffDelayInterrogation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipCreation").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmTariffDelayInterrogation.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTariffDelayInterrogation.aspx");
        }

        private SShipmentQuotation getSelectedCarrier()
        {
            string format = "dd/MM/yyyy";
            SShipmentQuotation SelectedCarrier = new SShipmentQuotation();
            string TempDate = "";

            if (rdCarrierSelection1.Checked)
            {
                SelectedCarrier.CalculatedTariff = Convert.ToDouble(lblKeyTariff.Text);
                SelectedCarrier.CarrierName = lblKeyCarrier.Text;
                SelectedCarrier.CarrierPriority = "EXP";
                SelectedCarrier.CarrierType = "KeyCustomer";
                //SelectedCarrier.DeliveryDate = Convert.ToDateTime(lblKeyDelivery.Text);
                DeliveryTime = lblDelivery.Text.Trim().Substring(11);
                TempDate = lblKeyDelivery.Text.Trim().Substring(1, 10);
                SelectedCarrier.DeliveryDate = DateTime.ParseExact(TempDate, format, CultureInfo.InvariantCulture);
                SelectedCarrier.FuelSurcharge = Convert.ToDouble(hdKeyFuelCharge.Value);
                SelectedCarrier.Information = lblKeyInfo.Text;
                SelectedCarrier.Options = hdKeyOptionsValue.Value.Trim();
                SelectedCarrier.OptionsDescription = hdKeyOptionsDescription.Value.Trim();
                SelectedCarrier.ServiceName = lblKeyService.Text.Trim();
                SelectedCarrier.ShippingDate = DateTime.ParseExact(lblKeyShipping.Text, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblKeyShipping.Text.Trim());
                SelectedCarrier.Surcharge = hdKeySurchargeValue.Value.Trim();
                SelectedCarrier.SurchargeDescription = hdKeySurchargeDescription.Value.Trim();
            }
            else if (rdCarrierSelection2.Checked)
            {
                SelectedCarrier.CalculatedTariff = Convert.ToDouble(lblTariff.Text);
                SelectedCarrier.CarrierName = lblCarrier.Text;
                SelectedCarrier.CarrierPriority = "EXP";
                SelectedCarrier.CarrierType = "";
                DeliveryTime = lblDelivery.Text.Trim().Substring(11);
                TempDate = lblDelivery.Text.Trim().Substring(0, 10);
                SelectedCarrier.DeliveryDate = DateTime.ParseExact(TempDate, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblDelivery.Text);
                SelectedCarrier.FuelSurcharge = Convert.ToDouble(hdFuelCharge.Value);
                SelectedCarrier.Information = lblInfo.Text;
                SelectedCarrier.Options = hdOptionsValue.Value.Trim();
                SelectedCarrier.OptionsDescription = hdOptionsDescription.Value.Trim();
                SelectedCarrier.ServiceName = lblService.Text.Trim();
                SelectedCarrier.ShippingDate = DateTime.ParseExact(lblShipping.Text, format, CultureInfo.InvariantCulture); // Convert.ToDateTime(lblShipping.Text.Trim());
                SelectedCarrier.Surcharge = hdSurchargeValue.Value.Trim();
                SelectedCarrier.SurchargeDescription = hdSurchargeDescription.Value.Trim();
            }
            else if (rdCarrierSelection3.Checked)
            {
                SelectedCarrier.CalculatedTariff = Convert.ToDouble(lblEcoKeyTariff.Text);
                SelectedCarrier.CarrierName = lblEcoKeyCarrier.Text;
                SelectedCarrier.CarrierPriority = "ECO";
                SelectedCarrier.CarrierType = "KeyCustomer";
                DeliveryTime = lblDelivery.Text.Trim().Substring(11);
                TempDate = lblDelivery.Text.Trim().Substring(0, 10);
                SelectedCarrier.DeliveryDate = DateTime.ParseExact(TempDate, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblEcoKeyDelivery.Text);
                SelectedCarrier.FuelSurcharge = Convert.ToDouble(hdEcoKeyFuelCharge.Value);
                SelectedCarrier.Information = lblEcoKeyInfo.Text;
                SelectedCarrier.Options = hdEcoKeyOptionsValue.Value.Trim();
                SelectedCarrier.OptionsDescription = hdEcoKeyOptionsDescription.Value.Trim();
                SelectedCarrier.ServiceName = lblEcoKeyService.Text.Trim();
                SelectedCarrier.ShippingDate = DateTime.ParseExact(lblEcoKeyShipping.Text, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblEcoKeyShipping.Text.Trim());
                SelectedCarrier.Surcharge = hdEcoKeySurchargeValue.Value.Trim();
                SelectedCarrier.SurchargeDescription = hdEcoKeySurchargeDescription.Value.Trim();
            }
            else if (rdCarrierSelection4.Checked)
            {
                SelectedCarrier.CalculatedTariff = Convert.ToDouble(lblEcoTariff.Text);
                SelectedCarrier.CarrierName = lblEcoCarrier.Text;
                SelectedCarrier.CarrierPriority = "ECO";
                SelectedCarrier.CarrierType = "";
                DeliveryTime = lblDelivery.Text.Trim().Substring(11);
                TempDate = lblDelivery.Text.Trim().Substring(0, 10);
                SelectedCarrier.DeliveryDate = DateTime.ParseExact(TempDate, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblEcoDelivery.Text);
                SelectedCarrier.FuelSurcharge = Convert.ToDouble(hdEcoFuelCharge.Value);
                SelectedCarrier.Information = lblEcoInfo.Text;
                SelectedCarrier.Options = hdEcoOptionsValue.Value.Trim();
                SelectedCarrier.OptionsDescription = hdEcoOptionsDescription.Value.Trim();
                SelectedCarrier.ServiceName = lblEcoService.Text.Trim();
                SelectedCarrier.ShippingDate = DateTime.ParseExact(lblEcoShipping.Text, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(lblEcoShipping.Text.Trim());
                SelectedCarrier.Surcharge = hdEcoSurchargeValue.Value.Trim();
                SelectedCarrier.SurchargeDescription = hdEcoSurchargeDescription.Value.Trim();
            }

            return SelectedCarrier;
        }

        protected void btnBack_Click1(object sender, EventArgs e)
        {
            Server.Transfer("frmTariffDelayInterrogation.aspx", false);
        }

    }
}