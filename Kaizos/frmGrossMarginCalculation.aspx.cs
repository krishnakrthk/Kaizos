using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmGrossMarginCalculation : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));
        KaizosServiceAgent proxy = new KaizosServiceAgent();


        #region Declaraions
        int irow = 0;
        int iLoop = 0;
        string strSelectedOrigin = "";
        string strSelectedDestination = "";
        string strSelectedMasterType = "";
        string strSelectedPublicCarrier = "";
        float fSubTotalWeight = 0.0f;
        float fSubTotalADV = 0.0f;
        float fSubTotalFreight = 0.0f;
        float fSubTotalPublic = 0.0f;
        float fSubTotalPurchase = 0.0f;
        float fSubTotalTurnOver = 0.0f;
        float fSubTotalCurrentDiscount = 0.0f;
        float fSubTotalCurrentSale = 0.0f;
        float fSubTotalGrossMargin = 0.0f;
        float fSubTotalSalesFretTariff = 0.0f;
        float fSubTotalProposedSalesTariff = 0.0f;
        float fSubTotalSalesTurnOver = 0.0f;
        float fSubTotalSalesGrossMargin = 0.0f;
        float fSubtotalCompare = 0.0f;
        float fSubTotalCompareMargin = 0.0f;
        string PublicSurcharge = "";

        float fNet = 0.0f;
        float fPub = 0.0f;

        List<GridDetail> vwGridDetail;
        List<PublicDiscount> publicDiscountDB = new List<PublicDiscount>();
        List<SSimulationTariffBased> sSimulationTariffBased = new List<SSimulationTariffBased>();

        #endregion

        [Serializable]
        protected class GridDetail
        {
            public string gvWeight { get; set; }
            public string gvDestinationCountry { get; set; }
            public string gvOriginCountry { get; set; }
            public string gvAverageWeight { get; set; }
            public string gvMasterServiceName { get; set; }
            public string gvADV { get; set; }
            public float gvFreightPurchase { get; set; }
            public float gvPurchaseTariff { get; set; }
            public string gvCarrier { get; set; }
            public float gvDiscount { get; set; }
            public string gvPublicCarrier { get; set; }
            public float gvPublicTariff { get; set; }
            public string gvPublicSurcharge { get; set; }
            public float gvCurrentSaleTariff { get; set; }
            public float gvCurrentTurnOver { get; set; }
            public float gvGrossMargin { get; set; }
            public float gvSalesFretTariff { get; set; }
            public float gvProposedSalesTariff { get; set; }
            public float gvSalesTurnOver { get; set; }
            public float gvSalesGrossMargin { get; set; }
            public float gvComparisonSales { get; set; }
            public float gvComparisonMargin { get; set; }
            public bool gvIsPublicTariffUpdated { get; set; }
            public bool gvIsDiscountUpdated { get; set; }
            public float gvOldPublicTariff { get; set; }
            public float gvActualPublicTariff { get; set; }
            public float gvFuelSurcharge { get; set; }
        }

        protected class PublicDiscount
        {
            public string CarrierName { get; set; }
            public string FuelDiscount { get; set; }
            public string SafetyDiscount { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
			errorMsg1.Attributes["style"] = "display: none;";
            errorMsg2.Attributes["style"] = "display: none;";
			List<string> StringList = new List<string>();
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();
            SKeyValue sKeyValue = new SKeyValue();
            sKeyValue = proxy.GetValueFromParameter("MAX_ROW_PER_BLOCK");
            ViewState["MaxRow"] = sKeyValue.Value;

            if (!(Page.IsPostBack))
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmGrossMarginCalculation").ToString();
                try
                {

                    StringList = proxy.GetPublicTariffNames().ToList();
                    ddlDefaultTariff.DataSource = StringList;
                    ddlDefaultTariff.DataBind();

                    //StringList = proxy.GetAssignedUsers(KaizosSession.Current.AccountNo);
                    List<SUserID> UserIDList = new List<SUserID>();
                    //UserIDList = proxy.GetAssignedUsers("1");
                    UserIDList = proxy.GetAssignedUsers(KaizosSession.Current.AccountNo);
                    ddlCustomerID.DataSource = UserIDList;
                    ddlCustomerID.DataTextField = "UserName";
                    ddlCustomerID.DataValueField = "AccountNo";
                    ddlCustomerID.DataBind();
                    ddlCustomerID.Items.Insert(0, new ListItem(String.Empty, " "));


                    sTariffMaster = proxy.GetTariffAssignedCarrierNames("PURCHASE");
                    gvCarrierTariff.DataSource = sTariffMaster;
                    gvCarrierTariff.DataBind();


                    //Carrier list must be taken from someother SP later.

                    sTariffMaster = proxy.GetAllCarrierNames();

                    //Just to fill default
                    foreach (var s in sTariffMaster)
                    {
                        PublicDiscount p = new PublicDiscount();
                        p.CarrierName = s.CarrierName;
                        p.FuelDiscount = "0";
                        p.SafetyDiscount = "0";
                        publicDiscountDB.Add(p);
                    }

                    gvCustomerDiscount.DataSource = sTariffMaster;
                    gvCustomerDiscount.DataBind();
                    vwGridDetail = new List<GridDetail>();
                    // ViewState["vwGridDetail"] = vwGridDetail;
                    ViewState["NewBlock"] = false;
                    ViewState["EditBlock"] = false;
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SimulationTool").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "PageLoad", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SimulationTool").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }

            }
            else
            {
                //
            }

            //ViewState["vwGridDetail"] = vwGridDetail;
        }

        protected void gvCarrierTariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label lblServiceList = (Label)e.Row.FindControl("lblServiceName");

                    // Purchase Tariff
                    sTariffMaster = proxy.GetCarrierTariffNames("PURCHASE", lblServiceList.Text);
                    DropDownList ddlList = e.Row.FindControl("ddlPurchaseTariffList") as DropDownList;

                    ddlList.DataSource = sTariffMaster;
                    ddlList.DataTextField = "TariffReference";
                    ddlList.DataBind();
                    ddlList.SelectedValue = GetSelectedTariffBased(lblServiceList.Text, "Purchase");

                    // Public Tariff

                    sTariffMaster = proxy.GetCarrierTariffNames(SEnumTariffType.CarrierPublic.ToString(), lblServiceList.Text);

                    ddlList = e.Row.FindControl("ddlPublicTariffList") as DropDownList;

                    ddlList.DataSource = sTariffMaster;
                    ddlList.DataTextField = "TariffReference";
                    ddlList.DataBind();
                    ddlList.SelectedValue = GetSelectedTariffBased(lblServiceList.Text, "Public");
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SimulationToolCarrierTariffLoad").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "gvCarrierTariff_RowDataBound", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SimulationToolCarrierTariffLoad").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }

            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            if (IsValid)
            {
                #region Declarations

                List<STariffMaster> sTariffMaster = new List<STariffMaster>();
                sTariffMaster = GetSelectedCarrierTariff();

                List<STariffMaster> sTariffMaster1 = new List<STariffMaster>();

                float AvgWeight = 0.0f;
                float ADV = 0.0f;
                string MasterServiceName = "";
                string Destination = "";
                string Origin = "";
                string AssignedTariff = ddlDefaultTariff.SelectedValue.Trim();
                float PublicTariff = 0f;
                string PublicCarrier = "";
                float PublicDiscount = 0f;
                float fSalesFreightTariff = 0f;
                float fFreightPurchase = 0f;
                float fSalesGrossMargin = 0f;
                //30JAN11HN to calculate Surcharges for sale tariff
                float fSalesFuel = 0f;
                float fSalesSurcharge = 0f;
                string PublicSelectedTariff = "";
                float PublicFuel = 0f;
                bool isPublicTariffChanged = false;
                SSurchargeDetails PublicSurcharge = new SSurchargeDetails();


                List<SShipmentQuotation> PurchaseValue = new List<SShipmentQuotation>();
                List<SShipmentQuotation> PublicValue = new List<SShipmentQuotation>();
                List<SShipmentQuotation> SaleValue = new List<SShipmentQuotation>();
                List<GridDetail> gridDetail = new List<GridDetail>();

                #endregion

                vwGridDetail = (List<GridDetail>)ViewState["vwGridDetail"];

                int i = 0;
                foreach (GridViewRow gr in gvCalcualtion.Rows)
                {
                    List<SShipmentQuotation> sShipmentQuotation = new List<SShipmentQuotation>();
                    List<SShipmentQuotation> sShipmentQuotation2 = new List<SShipmentQuotation>();
                    GridDetail gd = new GridDetail();

                    SShipmentQuotation q = new SShipmentQuotation();
                    SShipmentQuotation publicq = new SShipmentQuotation();
                    SShipmentQuotation saleq = new SShipmentQuotation();

                    List<STariffReferenceList> PublicTariffReference = new List<STariffReferenceList>();

                    if (strSelectedDestination.Equals(""))
                    {
                        strSelectedDestination = ((DropDownList)gr.Cells[1].FindControl("ddlDeliveryCountry")).SelectedValue;
                        strSelectedOrigin = ((DropDownList)gr.Cells[2].FindControl("ddlShippingCountry")).SelectedValue; ;
                        MasterServiceName = ((DropDownList)gr.Cells[3].FindControl("ddlMasterServiceName")).SelectedValue;
                    }

                    Destination = strSelectedDestination.Substring(0, 2);
                    Origin = strSelectedOrigin.Substring(0, 2);
                    AvgWeight = (float)Convert.ToDouble(((TextBox)gr.Cells[4].FindControl("txtAverageWeight")).Text);
                    gd.gvAverageWeight = AvgWeight.ToString();

                    string strADV = ((TextBox)gr.Cells[4].FindControl("txtAdv")).Text.Trim();
                    if (strADV.Equals(""))
                    {
                        ADV = 0;
                    }
                    else
                    {
                        ADV = (float)Convert.ToDouble(strADV);
                    }
                    gd.gvADV = ADV.ToString();

                    #region 1.  Purchase Tariff Calculations
                    try
                    {
                        sShipmentQuotation = proxy.GetQuoteForTool(sTariffMaster, AssignedTariff, "Parcel", Origin, Destination, MasterServiceName.Trim(), AvgWeight);
                        sShipmentQuotation = sShipmentQuotation.OrderBy(c => c.CalculatedTariff).ToList();
                        if (sShipmentQuotation.Count == 0)
                        {
                            q.CalculatedTariff = 0;
                            q.CarrierName = "-";
                            q.CarrierPriority = "";
                            q.CarrierType = "";
                            q.DeliveryDate = DateTime.Now;
                            q.FuelSurcharge = 0;
                            q.Options = "-";
                            q.OptionsDescription = "-";
                            q.ServiceName = "-";
                            q.ShippingDate = DateTime.Now;
                            q.Surcharge = "0";
                            q.SurchargeDescription = "-";
                            fFreightPurchase = 0;
                        }
                        else
                        {
                            q.CalculatedTariff = sShipmentQuotation[0].CalculatedTariff;
                            q.CarrierName = sShipmentQuotation[0].CarrierName;
                            q.CarrierPriority = "";
                            q.CarrierType = "";
                            q.DeliveryDate = DateTime.Now;
                            q.FuelSurcharge = sShipmentQuotation[0].FuelSurcharge;
                            q.Options = sShipmentQuotation[0].Options;
                            q.OptionsDescription = sShipmentQuotation[0].OptionsDescription;
                            q.ServiceName = "";
                            q.ShippingDate = DateTime.Now;
                            q.Surcharge = sShipmentQuotation[0].Surcharge;
                            q.SurchargeDescription = sShipmentQuotation[0].SurchargeDescription;
                            fFreightPurchase = (float)sShipmentQuotation[0].CalculatedTariff;
                        }
                        PurchaseValue.Add(q);
                    }
                    /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                    catch (FaultException<SGeneralFault> ex)
                    {
                        string ErrorDetails = ex.Detail.Details;
                        string MethodName = ex.Detail.Issue;

                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPurchase").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    catch (Exception error)
                    {
                        /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnRefresh_Click", ErrorLog.ExtractError(error));
                            logger.Debug(ErrMsg);

                            KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                            KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPurchase").ToString();
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }

                    #endregion

                    #region 2. Public tariff calculations

                    STariffMaster t = new STariffMaster();

                    // 2.1 Retaining manual entry while refresing after edit 
                    // Condition to retain : Change only in Public Tariff value and Discount

                    gd.gvIsPublicTariffUpdated = false;
                    gd.gvIsDiscountUpdated = false;

                    // Old values
                    string OldCarrier = vwGridDetail[i].gvCarrier;
                    string OldOrigin = vwGridDetail[i].gvOriginCountry;
                    string OldDestination = vwGridDetail[i].gvDestinationCountry;
                    string OldMasterService = vwGridDetail[i].gvMasterServiceName;
                    string OldPublicCarrier = vwGridDetail[i].gvPublicCarrier;
                    string OldWeight = vwGridDetail[i].gvAverageWeight.ToString();
                    string OldPublicTariff = vwGridDetail[i].gvPublicTariff.ToString();
                    string OldDiscount = vwGridDetail[i].gvDiscount.ToString();

                    //New values
                    string NewCarrier = ((Label)gr.Cells[8].FindControl("lblCarriers")).Text;
                    string NewOrigin = strSelectedOrigin;
                    string NewDestination = strSelectedDestination;
                    string NewMasterService = MasterServiceName;
                    string NewPublicCarrier = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue;

                    string NewWeight = ((TextBox)gr.Cells[4].FindControl("txtAverageWeight")).Text;
                    string NewPublicTariff = ((TextBox)gr.Cells[11].FindControl("txtPublicFreight")).Text;
                    string NewDiscount = ((TextBox)gr.Cells[9].FindControl("txtDiscount")).Text;

                    if (OldCarrier.Equals(NewCarrier) && OldOrigin.Equals(NewOrigin) &&
                            OldDestination.Equals(NewDestination) && OldPublicCarrier.Equals(NewPublicCarrier) &&
                            OldWeight.Equals(NewWeight) && OldMasterService.Equals(NewMasterService))
                    {
                        publicq.CalculatedTariff = Convert.ToDouble(((TextBox)gr.Cells[11].FindControl("txtPublicFreight")).Text);
                        publicq.CarrierName = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue;
                        publicq.CarrierPriority = "";
                        publicq.CarrierType = "";
                        publicq.DeliveryDate = DateTime.Now;
                        publicq.FuelSurcharge = vwGridDetail[i].gvFuelSurcharge;// sShipmentQuotation[0].FuelSurcharge;
                        publicq.Options = ""; // sShipmentQuotation[0].Options;
                        publicq.OptionsDescription = ""; // sShipmentQuotation[0].OptionsDescription;
                        publicq.ServiceName = "";
                        publicq.ShippingDate = DateTime.Now;
                        publicq.Surcharge = vwGridDetail[i].gvPublicSurcharge.ToString(); // sShipmentQuotation[0].Surcharge;
                        publicq.SurchargeDescription = ""; // sShipmentQuotation[0].SurchargeDescription;
                        if (!(Convert.ToDecimal(OldPublicTariff) == Convert.ToDecimal(NewPublicTariff))) //if (!(OldPublicTariff.Equals(NewPublicTariff)))
                        {
                            isPublicTariffChanged = true;
                            gd.gvIsPublicTariffUpdated = true;
                            gd.gvOldPublicTariff = (float)Convert.ToDouble(OldPublicTariff);
                            gd.gvActualPublicTariff = (float)Convert.ToDouble(vwGridDetail[i].gvActualPublicTariff.ToString());
                            PublicTariffReference = proxy.GetTariffReference(((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue, SEnumTariffType.CarrierPublic.ToString());
                            publicq.FuelSurcharge = proxy.GetFuelSurcharge(PublicTariffReference[0].TariffReference, NewOrigin, NewDestination,
                                                 (float)Convert.ToDouble(NewWeight), NewMasterService.Trim(), (float)publicq.CalculatedTariff);
                        }
                        else if (!(Convert.ToDecimal(OldDiscount) == Convert.ToDecimal(NewDiscount))) //else if (!(OldDiscount.Equals(NewDiscount)))
                        {
                            PublicTariffReference = proxy.GetTariffReference(((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue, SEnumTariffType.CarrierPublic.ToString());
                            t.CarrierName = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue;
                            t.TariffReference = PublicTariffReference[0].TariffReference;
                            t.TariffType = SEnumTariffType.CarrierPublic;
                            sTariffMaster1.Add(t);

                            sShipmentQuotation2 = proxy.GetQuoteForTool(sTariffMaster1, AssignedTariff, "Parcel", Origin, Destination, MasterServiceName.Trim(), AvgWeight);
                            sShipmentQuotation2 = sShipmentQuotation2.OrderBy(c => c.CalculatedTariff).ToList();
                            publicq.CalculatedTariff = sShipmentQuotation2[0].CalculatedTariff;
                            gd.gvActualPublicTariff = (float)sShipmentQuotation2[0].CalculatedTariff;

                            //publicq.CalculatedTariff = (float)Convert.ToDouble(vwGridDetail[i].gvActualPublicTariff.ToString());
                            //gd.gvActualPublicTariff = (float)Convert.ToDouble(vwGridDetail[i].gvActualPublicTariff.ToString());
                            gd.gvIsDiscountUpdated = true;
                        }
                        else
                        {
                            gd.gvIsPublicTariffUpdated = false;
                            gd.gvActualPublicTariff = (float)Convert.ToDouble(vwGridDetail[i].gvActualPublicTariff.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            PublicTariffReference = proxy.GetTariffReference(((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue, SEnumTariffType.CarrierPublic.ToString());
                            if (PublicTariffReference.Count == 0)
                            {
                                //Fill Default values and continue for next loop
                                publicq.CalculatedTariff = 0;
                                publicq.CarrierName = "-";
                                publicq.CarrierPriority = "";
                                publicq.CarrierType = "";
                                publicq.DeliveryDate = DateTime.Now;
                                publicq.FuelSurcharge = 0;
                                publicq.Options = "0";
                                publicq.OptionsDescription = "-";
                                publicq.ServiceName = "-";
                                publicq.ShippingDate = DateTime.Now;
                                publicq.Surcharge = "0";
                                publicq.SurchargeDescription = "-";

                            }
                            else
                            {
                                t.CarrierName = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue;
                                t.TariffReference = PublicTariffReference[0].TariffReference;
                                t.TariffType = SEnumTariffType.CarrierPublic;
                                sTariffMaster1.Add(t);

                                sShipmentQuotation2 = proxy.GetQuoteForTool(sTariffMaster1, AssignedTariff, "Parcel", Origin, Destination, MasterServiceName.Trim(), AvgWeight);
                                sShipmentQuotation2 = sShipmentQuotation2.OrderBy(c => c.CalculatedTariff).ToList();
                                if (sShipmentQuotation2.Count == 0)
                                {
                                    publicq.CalculatedTariff = 0;
                                    publicq.CarrierName = "-";
                                    publicq.CarrierPriority = "";
                                    publicq.CarrierType = "";
                                    publicq.DeliveryDate = DateTime.Now;
                                    publicq.FuelSurcharge = 0;
                                    publicq.Options = "0";
                                    publicq.OptionsDescription = "-";
                                    publicq.ServiceName = "-";
                                    publicq.ShippingDate = DateTime.Now;
                                    publicq.Surcharge = "0";
                                    publicq.SurchargeDescription = "-";
                                }
                                else
                                {
                                    publicq.CalculatedTariff = sShipmentQuotation2[0].CalculatedTariff;
                                    publicq.CarrierName = sShipmentQuotation2[0].CarrierName;
                                    publicq.CarrierPriority = "";
                                    publicq.CarrierType = "";
                                    publicq.DeliveryDate = DateTime.Now;
                                    publicq.FuelSurcharge = sShipmentQuotation2[0].FuelSurcharge;
                                    publicq.Options = sShipmentQuotation2[0].Options;
                                    publicq.OptionsDescription = sShipmentQuotation2[0].OptionsDescription;
                                    publicq.ServiceName = "";
                                    publicq.ShippingDate = DateTime.Now;
                                    publicq.Surcharge = sShipmentQuotation2[0].Surcharge;
                                    publicq.SurchargeDescription = sShipmentQuotation2[0].SurchargeDescription;
                                    gd.gvActualPublicTariff = (float)sShipmentQuotation2[0].CalculatedTariff;
                                }
                            }
                        }
                        /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                        catch (FaultException<SGeneralFault> ex)
                        {
                            string ErrorDetails = ex.Detail.Details;
                            string MethodName = ex.Detail.Issue;

                            string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                            logger.Debug(ErrMsg);

                            KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                            KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPublic").ToString();
                            Server.Transfer("frmResult.aspx", false);
                        }
                        catch (Exception error)
                        {
                            /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                            if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                            {
                                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnRefresh_Click", ErrorLog.ExtractError(error));
                                logger.Debug(ErrMsg);

                                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPublic").ToString();
                                Server.Transfer("frmResult.aspx", false);
                            }
                        }
                    }


                    PublicValue.Add(publicq);

                    PublicDiscount = (float)Convert.ToDouble(((TextBox)gr.Cells[9].FindControl("txtDiscount")).Text);
                    PublicCarrier = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue.Trim();
                    strSelectedPublicCarrier = PublicCarrier;
                    gd.gvDiscount = PublicDiscount;
                    gd.gvPublicCarrier = PublicCarrier;
                    gd.gvGrossMargin = (float)Convert.ToDouble(((TextBox)gr.Cells[14].FindControl("txtGrossMarign")).Text);

                    #endregion

                    #region 3. Sales Tariffs
                    try
                    {
                        fSalesGrossMargin = (float)Convert.ToDouble(((TextBox)gr.Cells[14].FindControl("txtGrossMarign")).Text);
                        fSalesFreightTariff = fFreightPurchase * (1 + (fSalesGrossMargin / 100));

                        gd.gvGrossMargin = fSalesGrossMargin;
                        gd.gvSalesFretTariff = fSalesFreightTariff;

                        if (sShipmentQuotation.Count == 0)
                        {
                            saleq.CalculatedTariff = 0;
                            saleq.CarrierName = "-";
                            saleq.CarrierPriority = "";
                            saleq.CarrierType = "";
                            saleq.DeliveryDate = DateTime.Now;
                            saleq.FuelSurcharge = 0;
                            saleq.Options = "-";
                            saleq.OptionsDescription = "-";
                            saleq.ServiceName = "-";
                            saleq.ShippingDate = DateTime.Now;
                            saleq.Surcharge = "0";
                            saleq.SurchargeDescription = "-";
                            fSalesFuel = 0;
                            fSalesSurcharge = 0;
                        }
                        else
                        {
                            PublicSelectedTariff = GetSelectedPublicCarrierTariff(sShipmentQuotation[0].CarrierName);
                            PublicSurcharge = proxy.GetSimulationSurcharge(PublicSelectedTariff, Origin, Destination, AvgWeight, MasterServiceName.Trim());
                            fSalesFreightTariff = fSalesFreightTariff + (float)Convert.ToDecimal(PublicSurcharge.ParamID);

                            PublicFuel = proxy.GetFuelSurcharge(PublicSelectedTariff, Origin, Destination, AvgWeight, MasterServiceName.Trim(), fSalesFreightTariff);

                            saleq.CalculatedTariff = sShipmentQuotation[0].CalculatedTariff;
                            saleq.CarrierName = sShipmentQuotation[0].CarrierName;
                            saleq.CarrierPriority = "";
                            saleq.CarrierType = "";
                            saleq.DeliveryDate = DateTime.Now;
                            saleq.FuelSurcharge = PublicFuel;
                            saleq.Options = "";
                            saleq.OptionsDescription = "";
                            saleq.ServiceName = "";
                            saleq.ShippingDate = DateTime.Now;
                            saleq.Surcharge = PublicSurcharge.ParamID;
                            saleq.SurchargeDescription = PublicSurcharge.SurchageCode;
                            fSalesFuel = PublicFuel;
                            fSalesSurcharge = (float)Convert.ToDouble(PublicSurcharge.ParamID); // Taken value in ParamID as SP returns string
                        }

                        SaleValue.Add(saleq);

                    }
                    /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                    catch (FaultException<SGeneralFault> ex)
                    {
                        string ErrorDetails = ex.Detail.Details;
                        string MethodName = ex.Detail.Issue;

                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPurchase").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    catch (Exception error)
                    {
                        /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnRefresh_Click", ErrorLog.ExtractError(error));
                            logger.Debug(ErrMsg);

                            KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                            KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationPurchase").ToString();
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }

                    #endregion

                    i++;
                    gridDetail.Add(gd);
                }
                BindCalculatedGrid(PurchaseValue, PublicValue, SaleValue, strSelectedOrigin, strSelectedDestination, MasterServiceName, gridDetail);
                ViewState["EditBlock"] = true;
                GetTotal();
            }
        }

        protected List<STariffMaster> GetSelectedCarrierTariff()
        {
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();

            foreach (GridViewRow gr in gvCarrierTariff.Rows)
            {
                STariffMaster t = new STariffMaster();
                t.CarrierName = ((Label)gr.Cells[0].FindControl("lblServiceName")).Text;
                t.TariffReference = ((DropDownList)gr.Cells[1].FindControl("ddlPurchaseTariffList")).SelectedValue;
                t.TariffType = SEnumTariffType.Purchase;
                sTariffMaster.Add(t);
            }
            return sTariffMaster;
        }

        //30JAN11HN
        protected string GetSelectedPublicCarrierTariff(string Carrier)
        {
            string result = "";
            string strCurrentCarrier = "";

            foreach (GridViewRow gr in gvCarrierTariff.Rows)
            {
                strCurrentCarrier = ((Label)gr.Cells[0].FindControl("lblServiceName")).Text.Trim();
                if (strCurrentCarrier.Equals(Carrier.Trim()))
                {
                    result = ((DropDownList)gr.Cells[1].FindControl("ddlPublicTariffList")).SelectedValue;
                }
            }
            return result;
        }

        protected void gvCalcualtion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                List<SCountryTable> sCountryTable = new List<SCountryTable>();
                List<SComboText> sComboText = new List<SComboText>();
                SComboTableField sComboTableField = new SComboTableField();
                List<STariffMaster> sTariffMaster = new List<STariffMaster>();
                List<GridDetail> vwGridDetailLocal;
                vwGridDetailLocal = (List<GridDetail>)ViewState["vwGridDetail"];

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //To fill Carrier Names
                    DropDownList ddlCarrList = e.Row.FindControl("dllPublicCarrier") as DropDownList;
                    sTariffMaster = proxy.GetAllCarrierNames();

                    ddlCarrList.DataSource = sTariffMaster;
                    ddlCarrList.DataTextField = "CarrierName";
                    if (!(strSelectedOrigin.Equals("")))
                    {
                        ddlCarrList.SelectedValue = vwGridDetail[iLoop].gvPublicCarrier;
                        iLoop++;
                    }

                    ddlCarrList.DataBind();

                    if (irow == 0)
                    {
                        sCountryTable = proxy.FillCountryCombo().ToList();

                        // Shipping Country
                        DropDownList ddlList = e.Row.FindControl("ddlShippingCountry") as DropDownList;
                        sTariffMaster = proxy.GetAllCarrierNames();

                        ddlList.DataSource = sCountryTable;
                        ddlList.DataTextField = "CodeName";
                        ddlList.DataValueField = "CountryCode";

                        ddlList.DataBind();
                        ddlList.SelectedValue = strSelectedOrigin;

                        // Delivery Country
                        ddlList = e.Row.FindControl("ddlDeliveryCountry") as DropDownList;
                        ddlList.DataSource = sCountryTable;
                        ddlList.DataTextField = "CodeName";
                        ddlList.DataValueField = "CountryCode";
                        ddlList.DataBind();
                        ddlList.SelectedValue = strSelectedDestination;

                        //To fill Master Service Names
                        sComboTableField.FieldName = "MASTER_SERVICE_NAME";
                        sComboTableField.TableName = "MASTER_SERVICE";
                        sComboText = proxy.FillCombo(sComboTableField).ToList();
                        ddlList = e.Row.FindControl("ddlMasterServiceName") as DropDownList;
                        ddlList.DataSource = sComboText;
                        ddlList.DataTextField = "ComboText";
                        if (!(strSelectedMasterType.Equals("")))
                            ddlList.SelectedValue = strSelectedMasterType;
                        ddlList.DataBind();
                        irow++;

                    }
                    else
                    {
                        DropDownList ddlList = e.Row.FindControl("ddlShippingCountry") as DropDownList;
                        ddlList.Visible = false;
                        ddlList = e.Row.FindControl("ddlDeliveryCountry") as DropDownList;
                        ddlList.Visible = false;
                        ddlList = e.Row.FindControl("ddlMasterServiceName") as DropDownList;
                        ddlList.Visible = false;

                    }

                    e.Row.Cells[1].Style.Add("border-top-width", "0px");
                    e.Row.Cells[1].Style.Add("border-bottom-width", "0px");
                    e.Row.Cells[2].Style.Add("border-top-width", "0px");
                    e.Row.Cells[2].Style.Add("border-bottom-width", "0px");
                    e.Row.Cells[3].Style.Add("border-top-width", "0px");
                    e.Row.Cells[3].Style.Add("border-bottom-width", "0px");
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].Text = "Sub Total";
                    e.Row.Cells[4].Text = fSubTotalWeight.ToString("F");
                    e.Row.Cells[5].Text = fSubTotalADV.ToString("F");
                    e.Row.Cells[6].Text = fSubTotalFreight.ToString("F");
                    e.Row.Cells[7].Text = fSubTotalPurchase.ToString("F");
                    e.Row.Cells[9].Text = fSubTotalCurrentDiscount.ToString("F");
                    e.Row.Cells[11].Text = fSubTotalPublic.ToString("F");
                    e.Row.Cells[12].Text = fSubTotalCurrentSale.ToString("F");
                    e.Row.Cells[13].Text = fSubTotalTurnOver.ToString("F");
                    e.Row.Cells[14].Text = fSubTotalGrossMargin.ToString("F");
                    e.Row.Cells[15].Text = fSubTotalSalesFretTariff.ToString("F");
                    e.Row.Cells[16].Text = fSubTotalProposedSalesTariff.ToString("F");
                    e.Row.Cells[17].Text = fSubTotalSalesTurnOver.ToString("F");
                    e.Row.Cells[18].Text = fSubTotalSalesGrossMargin.ToString("F");
                    e.Row.Cells[19].Text = fSubtotalCompare.ToString("F");
                    e.Row.Cells[20].Text = fSubTotalCompareMargin.ToString("F");
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "gvCalcualtion_RowDataBound", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnAddBlock_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                //LoadFullList();

                List<GridDetail> gridDetail = new List<GridDetail>();
                //int iIncrement = Convert.ToInt32(txtWeightIncrement.Text);
                //int iMaxWeight = Convert.ToInt32(txtWeighLimit.Text);
                float iIncrement = (float)Convert.ToDecimal(txtWeightIncrement.Text);
                float iMaxWeight = (float)Convert.ToDecimal(txtWeighLimit.Text);
                float iLower = 0;
                float iUpper = 0;
                float fAverageWeight = 0;
                string strWeight = "";
                while (iUpper < iMaxWeight)
                {
                    GridDetail g = new GridDetail();

                    if (iUpper == 0)
                        iLower = 0;
                    else
                    {
                        //iLower = iUpper + 1;
                        iLower = iUpper; // SDD change Bug ID 1134 - 06FEB12
                    }

                    iUpper = iUpper + iIncrement;
                    iLower = (float)Math.Round(iLower, 2);
                    iUpper = (float)Math.Round(iUpper, 2);
                    fAverageWeight = (float)((iLower + iUpper) / 2);


                    strWeight = iLower.ToString() + "-" + iUpper.ToString();

                    g.gvWeight = strWeight;
                    g.gvOriginCountry = "";
                    g.gvDestinationCountry = "";
                    g.gvAverageWeight = fAverageWeight.ToString();
                    g.gvMasterServiceName = "";
                    g.gvFreightPurchase = 0;
                    g.gvADV = "1";
                    g.gvFreightPurchase = 0;
                    g.gvPurchaseTariff = 0;
                    g.gvCarrier = "";
                    g.gvDiscount = 0f;
                    g.gvPublicTariff = 0f;
                    g.gvPublicCarrier = "";
                    g.gvCurrentSaleTariff = 0f;
                    g.gvCurrentTurnOver = 0f;
                    g.gvPublicSurcharge = "0";

                    gridDetail.Add(g);
                }

                gvCalcualtion.DataSource = gridDetail;
                gvCalcualtion.DataBind();
                ViewState["vwGridDetail"] = gridDetail.ToList();
                ViewState["NewBlock"] = true;
                //ddlSimulationID.SelectedIndex = 0;

                //gvOuterList.DataSource = null;
                //gvOuterList.DataBind();
                ////LoadFullList();
                //List<SSimulationList> sSimulationList = new List<SSimulationList>();
                //sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
                //gvOuterList.DataSource = sSimulationList;
                //gvOuterList.DataBind();
                ddlCustomerID_SelectedIndexChanged(this, EventArgs.Empty);

            }
        }

        protected void BindCalculatedGrid(List<SShipmentQuotation> PurchaseQuotation, List<SShipmentQuotation> PublicQuotation, List<SShipmentQuotation> SaleQuotation, string SelectedOrigin, string SelectedDestination, string SelectedMasterType, List<GridDetail> GridDetailFromScreen)
        {
            try
            {
                List<GridDetail> gridDetail = new List<GridDetail>();
                PublicDiscount publicDiscount = new PublicDiscount();
                List<STariffReferenceList> PublicTariffReference = new List<STariffReferenceList>();

                //int iIncrement = Convert.ToInt32(txtWeightIncrement.Text);
                //int iMaxWeight = Convert.ToInt32(txtWeighLimit.Text);

                float iIncrement = (float)Convert.ToDecimal(txtWeightIncrement.Text);
                float iMaxWeight = (float)Convert.ToDecimal(txtWeighLimit.Text);

                float iLower = 0;
                float iUpper = 0;
                float fAverageWeight = 0;
                float fADV = 0;
                float fCalculatedTariff = 0;
                float fPublicTariff = 0;
                float fPurchase = 0;
                float fCurrentDiscountPercentage = 0;
                float fCurrentDiscount = 0;
                float fPublicSurcharge = 0;
                float fPublicSurchargeDiscount = 0;
                float fPublicFuelSurcharge = 0;
                float fPublicFuelSurchargeDiscount = 0;
                float fSaleGrossMargin = 0;
                float fCurrentSaleTariff = 0;
                float fSalesTurnOver = 0;
                float fProposedSalesTariff = 0;
                float fSalesFretTariff = 0;
                float fCompare = 0;
                float fCompareMargin = 0;

                string strWeight = "";
                int i = 0;
                while (iUpper < iMaxWeight)
                {
                    GridDetail g = new GridDetail();

                    if (iUpper == 0)
                        iLower = 0;
                    else
                        iLower = iUpper;

                    iUpper = iUpper + iIncrement;
                    iLower = (float)Math.Round(iLower, 2);
                    iUpper = (float)Math.Round(iUpper, 2);
                    fAverageWeight = (float)((iLower + iUpper) / 2);

                    strWeight = iLower.ToString() + "-" + iUpper.ToString();
                    g.gvWeight = strWeight;
                    g.gvOriginCountry = SelectedOrigin;
                    g.gvDestinationCountry = SelectedDestination;
                    g.gvAverageWeight = GridDetailFromScreen[i].gvAverageWeight;  //fAverageWeight.ToString();
                    g.gvMasterServiceName = SelectedMasterType;
                    g.gvADV = GridDetailFromScreen[i].gvADV;

                    // Purchase tariff 
                    g.gvFreightPurchase = (float)PurchaseQuotation[i].CalculatedTariff;
                    fPurchase = (float)(PurchaseQuotation[i].CalculatedTariff + PurchaseQuotation[i].FuelSurcharge +
                                                        Convert.ToDouble(PurchaseQuotation[i].Surcharge));
                    g.gvPurchaseTariff = fPurchase;
                    g.gvCarrier = PurchaseQuotation[i].CarrierName;

                    // Current Budget based on Public tariff
                    g.gvDiscount = GridDetailFromScreen[i].gvDiscount;
                    g.gvPublicTariff = (float)PublicQuotation[i].CalculatedTariff;
                    g.gvPublicCarrier = GridDetailFromScreen[i].gvPublicCarrier;

                    g.gvCurrentTurnOver = 0f;
                    fADV = (float)Convert.ToDouble(GridDetailFromScreen[i].gvADV);
                    fPublicTariff = (float)Convert.ToDouble(PublicQuotation[i].CalculatedTariff);


                    //Current Sale tariff Calculation
                    fPublicSurcharge = GetSurcharge(PublicQuotation[i].Surcharge);
                    publicDiscount = GetPublicDiscounts(PublicQuotation[i].CarrierName);
                    fPublicFuelSurchargeDiscount = (float)Convert.ToDouble(publicDiscount.FuelDiscount);
                    fPublicSurchargeDiscount = (float)Convert.ToDouble(publicDiscount.SafetyDiscount);
                    fPublicSurcharge = fPublicSurcharge * (1 - (fPublicSurchargeDiscount / 100));

                    fPublicFuelSurcharge = (float)PublicQuotation[i].FuelSurcharge;
                    fPublicFuelSurcharge = fPublicFuelSurcharge * (1 - (fPublicFuelSurchargeDiscount / 100));

                    fCurrentDiscount = (float)Convert.ToDouble(GridDetailFromScreen[i].gvDiscount);

                    if (GridDetailFromScreen[i].gvIsPublicTariffUpdated)
                    {
                        float Difference = GridDetailFromScreen[i].gvActualPublicTariff - fPublicTariff;
                        fCurrentDiscount = (Difference / GridDetailFromScreen[i].gvActualPublicTariff) * 100;
                        fCurrentSaleTariff = (GridDetailFromScreen[i].gvActualPublicTariff - fCurrentDiscount) + fPublicSurcharge + fPublicFuelSurcharge;
                        g.gvDiscount = fCurrentDiscount;
                        //g.gvActualPublicTariff = GridDetailFromScreen[i].gvActualPublicTariff;
                    }
                    else if (GridDetailFromScreen[i].gvIsDiscountUpdated)
                    {
                        fCurrentDiscount = fPublicTariff * (fCurrentDiscount / 100);
                        PublicTariffReference = proxy.GetTariffReference(PublicQuotation[i].CarrierName, SEnumTariffType.CarrierPublic.ToString());
                        fPublicTariff = (fPublicTariff - fCurrentDiscount);
                        fPublicFuelSurcharge = proxy.GetFuelSurcharge(PublicTariffReference[0].TariffReference, SelectedOrigin, strSelectedDestination,
                                             (float)Convert.ToDouble(fAverageWeight), SelectedMasterType.Trim(), fPublicTariff + fPublicSurcharge);
                        fPublicFuelSurcharge = fPublicFuelSurcharge * (1 - (fPublicFuelSurchargeDiscount / 100));
                        //fPublicFuelSurcharge = (fPublicTariff + fPublicSurcharge) * (fPublicFuelSurcharge /100); 
                        fCurrentSaleTariff = fPublicTariff + fPublicSurcharge + fPublicFuelSurcharge;
                    }
                    else
                    {
                        //if ((fPublicTariff > 0) && (fPublicTariff != GridDetailFromScreen[i].gvActualPublicTariff))
                        if (fPublicTariff > 0)
                        {
                            PublicTariffReference = proxy.GetTariffReference(PublicQuotation[i].CarrierName, SEnumTariffType.CarrierPublic.ToString());
                            fPublicFuelSurcharge = proxy.GetFuelSurcharge(PublicTariffReference[0].TariffReference, SelectedOrigin, strSelectedDestination,
                                                 (float)Convert.ToDouble(fAverageWeight), SelectedMasterType.Trim(), fPublicTariff + fPublicSurcharge);
                            fPublicFuelSurcharge = fPublicFuelSurcharge * (1 - (fPublicFuelSurchargeDiscount / 100));
                            //fPublicFuelSurcharge = (fPublicTariff + fPublicSurcharge) * (fPublicFuelSurcharge /100); 
                            fCurrentSaleTariff = fPublicTariff + fPublicSurcharge + fPublicFuelSurcharge;
                        }
                        else
                        {
                            //fPublicFuelSurcharge = (fPublicTariff + fPublicSurcharge) * (fPublicFuelSurcharge /100); 
                            fCurrentSaleTariff = fPublicTariff + fPublicSurcharge + fPublicFuelSurcharge;
                        }
                    }
                    g.gvActualPublicTariff = GridDetailFromScreen[i].gvActualPublicTariff;
                    g.gvPublicTariff = fPublicTariff;
                    g.gvCurrentSaleTariff = fCurrentSaleTariff;
                    g.gvPublicSurcharge = PublicQuotation[i].Surcharge;
                    g.gvFuelSurcharge = fPublicFuelSurcharge;

                    fPub = fPub + (GridDetailFromScreen[i].gvActualPublicTariff * fADV);
                    fNet = fNet + (fPublicTariff * fADV);

                    //Turn Over Calculation
                    g.gvCurrentTurnOver = fADV * fCurrentSaleTariff * 252;

                    // Sale Tariffs
                    fSalesFretTariff = GridDetailFromScreen[i].gvSalesFretTariff;
                    g.gvSalesFretTariff = fSalesFretTariff;
                    g.gvGrossMargin = GridDetailFromScreen[i].gvGrossMargin;

                    //30JAN12HN
                    //fProposedSalesTariff = GridDetailFromScreen[i].gvSalesFretTariff +
                    //                            fPublicSurcharge + (float)PublicQuotation[i].FuelSurcharge;
                    //g.gvProposedSalesTariff = fProposedSalesTariff;

                    fProposedSalesTariff = fSalesFretTariff + (float)Convert.ToDouble(SaleQuotation[i].Surcharge)
                                                + (float)SaleQuotation[i].FuelSurcharge;
                    g.gvProposedSalesTariff = fProposedSalesTariff;

                    fSalesTurnOver = fADV * fProposedSalesTariff * 252;
                    g.gvSalesTurnOver = fSalesTurnOver;

                    fSaleGrossMargin = (fProposedSalesTariff - fPurchase) * fADV * 252;
                    g.gvSalesGrossMargin = fSaleGrossMargin;

                    //Comparison
                    fCompare = ((fProposedSalesTariff - fCurrentSaleTariff) / fCurrentSaleTariff) * 100;
                    fCompareMargin = (fSaleGrossMargin / fSalesTurnOver) * 100;
                    g.gvComparisonSales = fCompare;
                    g.gvComparisonMargin = fCompareMargin;

                    //Subtotal Calculations
                    fAverageWeight = (float)Convert.ToDouble(GridDetailFromScreen[i].gvAverageWeight);
                    fCalculatedTariff = (float)Convert.ToDouble(PurchaseQuotation[i].CalculatedTariff);

                    fSubTotalWeight = fSubTotalWeight + (fADV * fAverageWeight);
                    fSubTotalADV = fSubTotalADV + fADV;
                    fSubTotalFreight = fSubTotalFreight + (fADV * fCalculatedTariff);
                    fSubTotalPurchase = fSubTotalPurchase + (fADV * fPurchase);

                    fSubTotalPublic = fSubTotalPublic + (fADV * fPublicTariff);
                    fSubTotalCurrentSale = fSubTotalCurrentSale + (fADV * fCurrentSaleTariff);
                    fSubTotalTurnOver = fSubTotalTurnOver + g.gvCurrentTurnOver;

                    fSubTotalSalesFretTariff = fSubTotalSalesFretTariff + (fADV * fSalesFretTariff);
                    fSubTotalProposedSalesTariff = fSubTotalProposedSalesTariff + (fADV * fProposedSalesTariff);
                    fSubTotalSalesTurnOver = fSubTotalSalesTurnOver + fSalesTurnOver;
                    fSubTotalSalesGrossMargin = fSubTotalSalesGrossMargin + fSaleGrossMargin;

                    gridDetail.Add(g);
                    i++;
                }

                fSubTotalWeight = fSubTotalWeight / fSubTotalADV;
                fSubTotalFreight = fSubTotalFreight / fSubTotalADV;
                fSubTotalPublic = fSubTotalPublic / fSubTotalADV;
                fSubTotalCurrentSale = fSubTotalCurrentSale / fSubTotalADV;

                //fSubTotalCurrentDiscount = (fSubTotalPublic - fSubTotalCurrentSale) / (fSubTotalPublic * 100);
                fSubTotalCurrentDiscount = 1 - ((fNet / fSubTotalADV) / (fPub / fSubTotalADV));

                fSubTotalProposedSalesTariff = fSubTotalProposedSalesTariff / fSubTotalADV;
                fSubTotalGrossMargin = (fSubTotalProposedSalesTariff - fSubTotalPurchase) / (fSubTotalPurchase * 100);
                fSubTotalSalesFretTariff = fSubTotalSalesFretTariff / fSubTotalADV;

                fSubtotalCompare = (fSubTotalProposedSalesTariff - fSubTotalCurrentSale) / fSubTotalCurrentSale * 100;
                fSubTotalCompareMargin = (fSubTotalSalesTurnOver - fSubTotalTurnOver) / fSubTotalTurnOver * 100;

                strSelectedDestination = SelectedDestination;
                strSelectedOrigin = SelectedOrigin;
                strSelectedMasterType = SelectedMasterType;

                gridDetail = ReduceDecimals(gridDetail);

                vwGridDetail = gridDetail;
                ViewState["vwGridDetail"] = vwGridDetail;

                gvCalcualtion.DataSource = gridDetail;
                gvCalcualtion.DataBind();
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "BindCalculatedGrid", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }

        protected string MyFormat(string input)
        {
            return Convert.ToDouble(input).ToString("#.00");
        }

        protected string MyFormat1(string input)
        {
            return Convert.ToDouble(input).ToString("#.000");
        }

        protected List<GridDetail> ReduceDecimals(List<GridDetail> Input)
        {
            List<GridDetail> result = new List<GridDetail>();

            foreach (var i in Input)
            {
                GridDetail g = new GridDetail();
                g.gvActualPublicTariff = (float)Math.Round(i.gvActualPublicTariff, 2);
                g.gvADV = i.gvADV;
                g.gvAverageWeight = i.gvAverageWeight;
                g.gvCarrier = i.gvCarrier;

                g.gvComparisonMargin = ConvertToNumber(i.gvComparisonMargin, 2);

                g.gvComparisonSales = ConvertToNumber(i.gvComparisonSales, 2);     //(float)Math.Round(i.gvComparisonSales);
                g.gvCurrentSaleTariff = ConvertToNumber(i.gvCurrentSaleTariff, 2);   //(float)Math.Round(i.gvCurrentSaleTariff, 2);
                g.gvCurrentTurnOver = ConvertToNumber(i.gvCurrentTurnOver, 2);   //(float)Math.Round(i.gvCurrentTurnOver, 2);
                g.gvDestinationCountry = i.gvDestinationCountry;
                g.gvDiscount = ConvertToNumber(i.gvDiscount, 2);          // (float)Math.Round(i.gvDiscount, 2);
                g.gvFreightPurchase = ConvertToNumber(i.gvFreightPurchase, 2); //(float)Math.Round(i.gvFreightPurchase, 2);
                g.gvFuelSurcharge = ConvertToNumber(i.gvFuelSurcharge, 2);   //(float)Math.Round(i.gvFuelSurcharge, 2);
                g.gvGrossMargin = ConvertToNumber(i.gvGrossMargin, 3);   //(float)Math.Round(i.gvGrossMargin, 3);
                g.gvIsDiscountUpdated = i.gvIsDiscountUpdated;
                g.gvIsPublicTariffUpdated = i.gvIsPublicTariffUpdated;
                g.gvMasterServiceName = i.gvMasterServiceName;
                g.gvOldPublicTariff = ConvertToNumber(i.gvOldPublicTariff, 2); //(float)Math.Round(i.gvOldPublicTariff, 2);
                g.gvOriginCountry = i.gvOriginCountry;
                g.gvProposedSalesTariff = ConvertToNumber(i.gvProposedSalesTariff, 2);   //(float)Math.Round(i.gvProposedSalesTariff, 2);
                g.gvPublicCarrier = i.gvPublicCarrier;
                g.gvPublicSurcharge = i.gvPublicSurcharge;
                g.gvPublicTariff = ConvertToNumber(i.gvPublicTariff, 2); //(float)Math.Round(i.gvPublicTariff, 2);
                g.gvPurchaseTariff = ConvertToNumber(i.gvPurchaseTariff, 2); // (float)Math.Round(i.gvPurchaseTariff, 2);
                g.gvSalesFretTariff = ConvertToNumber(i.gvSalesFretTariff, 2); //(float)Math.Round(i.gvSalesFretTariff, 2);
                g.gvSalesGrossMargin = ConvertToNumber(i.gvSalesGrossMargin, 2); //(float)Math.Round(i.gvSalesGrossMargin, 2);
                g.gvSalesTurnOver = ConvertToNumber(i.gvSalesTurnOver, 2); //(float)Math.Round(i.gvSalesTurnOver, 2);
                g.gvWeight = i.gvWeight;

                result.Add(g);

            }


            return result;

        }

        protected float ConvertToNumber(float Value, int DecimalPlaces)
        {
            float result = 0;

            result = float.IsNaN(Value) ? 0 : (float)Math.Round(Value, DecimalPlaces);

            //if (float.IsNaN(Value))
            //{
            //    result = 0;
            //}
            //else
            //{
            //    result = (float)Math.Round(Value, 2);
            //}

            return result;

        }

        protected void gvCustomerDiscount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PublicDiscount Discount = new PublicDiscount();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblServiceList = (Label)e.Row.FindControl("lblCustomerServiceName");

                TextBox txtBox = e.Row.FindControl("txtFuelDiscount") as TextBox;
                Discount = GetPublicDiscount(lblServiceList.Text);
                if (Discount.FuelDiscount.Equals(""))
                {
                    txtBox.Text = "0";
                    txtBox = e.Row.FindControl("txtSafetyDiscount") as TextBox;
                    txtBox.Text = "0";
                }
                else
                {
                    txtBox.Text = Discount.FuelDiscount;
                    txtBox = e.Row.FindControl("txtSafetyDiscount") as TextBox;
                    txtBox.Text = Discount.SafetyDiscount;
                }

            }
        }

        protected float GetSurcharge(string SurchargeString)
        {
            float result = 0f;
            string[] strValue = SurchargeString.Split(',');
            for (int i = 0; i < strValue.Length; i++)
            {
                result += (float)Convert.ToDouble(strValue[i]);
            }
            return result;
        }

        protected PublicDiscount GetPublicDiscounts(string CarrierName)
        {
            PublicDiscount publicDiscount = new PublicDiscount();
            string CurrentCarrier = "";
            foreach (GridViewRow gr in gvCustomerDiscount.Rows)
            {
                CurrentCarrier = ((Label)gr.Cells[0].FindControl("lblCustomerServiceName")).Text.Trim();
                if (CurrentCarrier.Equals(CarrierName.Trim()))
                {
                    publicDiscount.CarrierName = CurrentCarrier;
                    publicDiscount.FuelDiscount = ((TextBox)gr.Cells[1].FindControl("txtFuelDiscount")).Text;
                    publicDiscount.SafetyDiscount = ((TextBox)gr.Cells[2].FindControl("txtSafetyDiscount")).Text;
                }
            }

            return publicDiscount;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iResult = 0;
            try
            {
                string strSimulationID = "";
                bool isNewBlock = (bool)ViewState["NewBlock"];
                bool isAlreadyExist = false;
                int i = 0;
                string ShipCountry = "";
                string DeliveryCountry = "";
                string MasterService = "";

                if (isNewBlock)
                {
                    foreach (GridViewRow gr in gvCalcualtion.Rows)
                    {
                        SSimulationTariff s = new SSimulationTariff();
                        if (i == 0)
                        {
                            ShipCountry = ((DropDownList)gr.Cells[1].FindControl("ddlShippingCountry")).SelectedValue;
                            DeliveryCountry = ((DropDownList)gr.Cells[2].FindControl("ddlDeliveryCountry")).SelectedValue;
                            MasterService = ((DropDownList)gr.Cells[3].FindControl("ddlMasterServiceName")).SelectedValue;
                            i++;
                        }
                    }

                    isAlreadyExist = proxy.isAlreadyExist(ShipCountry, DeliveryCountry, MasterService, ddlCustomerID.SelectedValue.Trim());
                }

                if (isAlreadyExist)
                {
                    lblAlready.Visible = true;
                }
                else
                {
                    lblAlready.Visible = false;

                    #region 0.Generate Simulation ID

                    SNextcounter sNextCounter = new SNextcounter();

                    if (isNewBlock)
                    {
                        sNextCounter = proxy.GetNextCounter("SIMULATION_REF", "SYSTEM", 1);
                        strSimulationID = sNextCounter.NextCounter.ToString();
                    }
                    else
                    {
                        strSimulationID = ddlSimulationID.SelectedValue.Trim();
                        int result = proxy.DeleteSimulationID(strSimulationID);
                    }

                    #endregion

                    #region 1. Default Tariff
                    try
                    {
                        string format = "dd/MM/yyyy";
                        SSimulationHeader sSimulationHeader = new SSimulationHeader();
                        sSimulationHeader.AccountNo = ddlCustomerID.SelectedValue.Trim();
                        sSimulationHeader.AssignedTariff = ddlDefaultTariff.SelectedValue.Trim();
                        //sSimulationHeader.Valid = Convert.ToDateTime(txtValidDate.Text);
                        sSimulationHeader.Valid = DateTime.ParseExact(txtValidDate.Text, format, CultureInfo.InvariantCulture);
                        //sSimulationHeader.WeightIncrement = Convert.ToInt32(txtWeightIncrement.Text);
                        //sSimulationHeader.WeightLimit = Convert.ToInt32(txtWeighLimit.Text);

                        // 14FEB12HN converted int to float.
                        sSimulationHeader.WeightIncrement = (float)Convert.ToDecimal(txtWeightIncrement.Text);
                        sSimulationHeader.WeightLimit = (float)Convert.ToDecimal(txtWeighLimit.Text);

                        sSimulationHeader.SimulationID = strSimulationID;

                        iResult = proxy.SimulationHeaderInsert(sSimulationHeader);

                    }
                    catch (Exception error)
                    {
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string userName = User.Identity.Name;
                            string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                            //logger.Debug(errorMessage);

                            //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                            //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                            //Server.Transfer("frmResult.aspx", false);
                        }
                    }

                    #endregion

                    #region 2. Tariff Based
                    try
                    {
                        List<SSimulationTariffBased> sSimulationTariffBased = new List<SSimulationTariffBased>();

                        foreach (GridViewRow gr in gvCarrierTariff.Rows)
                        {
                            SSimulationTariffBased purchase = new SSimulationTariffBased();
                            purchase.TariffReference = ((DropDownList)gr.Cells[1].FindControl("ddlPurchaseTariffList")).SelectedValue;
                            purchase.AccountNo = ddlCustomerID.SelectedValue.Trim();
                            purchase.Assigned = "Y";
                            purchase.CarrierName = ((Label)gr.Cells[0].FindControl("lblServiceName")).Text;
                            purchase.SimulationID = strSimulationID;
                            purchase.TariffType = "Purchase";
                            sSimulationTariffBased.Add(purchase);

                            SSimulationTariffBased pub = new SSimulationTariffBased();
                            pub.TariffReference = ((DropDownList)gr.Cells[2].FindControl("ddlPublicTariffList")).SelectedValue;
                            pub.AccountNo = ddlCustomerID.SelectedValue.Trim();
                            pub.Assigned = "Y";
                            pub.CarrierName = ((Label)gr.Cells[0].FindControl("lblServiceName")).Text;
                            pub.SimulationID = strSimulationID;
                            pub.TariffType = "Public";
                            sSimulationTariffBased.Add(pub);
                        }

                        iResult = proxy.SimulationTariffBased(sSimulationTariffBased);

                    }
                    catch (Exception error)
                    {
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string userName = User.Identity.Name;
                            string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                            //logger.Debug(errorMessage);

                            //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                            //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                            //Server.Transfer("frmResult.aspx", false);
                        }
                    }

                    #endregion

                    #region 3.Surcharge Discount
                    try
                    {
                        List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount = new List<SSimulationSurchargeDiscount>();

                        foreach (GridViewRow gr in gvCustomerDiscount.Rows)
                        {
                            SSimulationSurchargeDiscount s = new SSimulationSurchargeDiscount();
                            s.AccountNo = ddlCustomerID.SelectedValue;
                            s.CarrierName = ((Label)gr.Cells[0].FindControl("lblCustomerServiceName")).Text;
                            s.FuelDiscount = (float)Convert.ToDouble(((TextBox)gr.Cells[1].FindControl("txtFuelDiscount")).Text);
                            s.SafetyDiscount = (float)Convert.ToDouble(((TextBox)gr.Cells[2].FindControl("txtSafetyDiscount")).Text);
                            s.SimulationID = strSimulationID;
                            sSimulationSurchargeDiscount.Add(s);
                        }

                        iResult = proxy.SimulationSurchargeDiscount(sSimulationSurchargeDiscount);

                    }
                    catch (Exception error)
                    {
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string userName = User.Identity.Name;
                            string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                            //logger.Debug(errorMessage);

                            //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                            //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                            //Server.Transfer("frmResult.aspx", false);
                        }
                    }


                    #endregion

                    #region 4.Simulation Calculation

                    try
                    {
                        List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
                        SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();

                        i = 0;

                        foreach (GridViewRow gr in gvCalcualtion.Rows)
                        {
                            SSimulationTariff s = new SSimulationTariff();
                            if (i == 0)
                            {
                                ShipCountry = ((DropDownList)gr.Cells[1].FindControl("ddlShippingCountry")).SelectedValue;
                                DeliveryCountry = ((DropDownList)gr.Cells[2].FindControl("ddlDeliveryCountry")).SelectedValue;
                                MasterService = ((DropDownList)gr.Cells[3].FindControl("ddlMasterServiceName")).SelectedValue;
                                i++;
                            }
                            s.AccountNo = ddlCustomerID.SelectedValue;
                            s.SimulationID = strSimulationID;
                            s.WeightRange = ((Label)gr.Cells[0].FindControl("lblWeightRangeCol1")).Text;
                            s.ShipCountry = ShipCountry;
                            s.DeliveryCountry = DeliveryCountry;
                            s.MasterServiceName = MasterService;
                            s.AverageWeight = (float)Convert.ToDouble(((TextBox)gr.Cells[4].FindControl("txtAverageWeight")).Text);
                            s.ADV = (float)Convert.ToDouble(((TextBox)gr.Cells[5].FindControl("txtAdv")).Text);
                            s.PurchaseFreight = (float)Convert.ToDouble(((Label)gr.Cells[6].FindControl("lblFreightPurchase")).Text);
                            s.PurchaseTariff = (float)Convert.ToDouble(((Label)gr.Cells[7].FindControl("lblPurchaseTariff")).Text);
                            s.PurchaseCarrier = ((Label)gr.Cells[8].FindControl("lblCarriers")).Text;
                            s.PublicDiscount = (float)Convert.ToDouble(((TextBox)gr.Cells[9].FindControl("txtDiscount")).Text);
                            s.PublicCarrier = ((DropDownList)gr.Cells[10].FindControl("dllPublicCarrier")).SelectedValue;
                            s.PublicFreight = (float)Convert.ToDouble(((TextBox)gr.Cells[11].FindControl("txtPublicFreight")).Text);
                            s.PublicTariff = (float)Convert.ToDouble(((Label)gr.Cells[12].FindControl("lblCurrentSaleTariff")).Text);
                            s.PublicTurnOver = (float)Convert.ToDouble(((Label)gr.Cells[13].FindControl("lblCurrentTurnOver")).Text);
                            s.SaleMargin = (float)Convert.ToDouble(((TextBox)gr.Cells[14].FindControl("txtGrossMarign")).Text);
                            s.SaleFreight = (float)Convert.ToDouble(((Label)gr.Cells[15].FindControl("lblSalesFretTariff")).Text);
                            s.SaleTariff = (float)Convert.ToDouble(((Label)gr.Cells[16].FindControl("lblProposedSalesTariff")).Text);
                            s.SaleTurnOver = (float)Convert.ToDouble(((Label)gr.Cells[17].FindControl("lblSalesTurnOver")).Text);
                            s.SaleGrossMargin = (float)Convert.ToDouble(((Label)gr.Cells[18].FindControl("lblSalesGrossMargin")).Text);
                            s.ComparisonSaleTariff = (float)Convert.ToDouble(((Label)gr.Cells[19].FindControl("lblComparisonSales")).Text);
                            s.ComparisonMargin = (float)Convert.ToDouble(((Label)gr.Cells[20].FindControl("lblComparisonMargin")).Text);
                            s.PublicSurcharge = ((Label)gr.Cells[21].FindControl("lblPublicSurcharge")).Text;

                            sSimulationTariff.Add(s);
                        }

                        iResult = proxy.SimulationTariff(sSimulationTariff);

                    }
                    catch (Exception error)
                    {
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string userName = User.Identity.Name;
                            string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                            //logger.Debug(errorMessage);

                            //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                            //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                            //Server.Transfer("frmResult.aspx", false);
                        }
                    }



                    #endregion

                    #region 5. Sub total Block
                    try
                    {

                        SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();
                        sSimulationSubTotal.AccountNo = ddlCustomerID.SelectedValue;
                        sSimulationSubTotal.SimulationID = strSimulationID;
                        sSimulationSubTotal.SubTotalWeight = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[4].Text);
                        sSimulationSubTotal.SubTotalADV = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[5].Text);
                        sSimulationSubTotal.SubTotalFreight = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[6].Text);
                        sSimulationSubTotal.SubTotalPurchase = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[7].Text);
                        sSimulationSubTotal.SubTotalCurrentDiscount = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[9].Text);
                        sSimulationSubTotal.SubTotalPublic = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[11].Text);
                        sSimulationSubTotal.SubTotalCurrentSale = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[12].Text);
                        sSimulationSubTotal.SubTotalTurnOver = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[13].Text);
                        sSimulationSubTotal.SubTotalGrossMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[14].Text);
                        sSimulationSubTotal.SubTotalSalesFretTariff = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[15].Text);
                        sSimulationSubTotal.SubTotalProposedSalesTariff = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[16].Text);
                        sSimulationSubTotal.SubTotalSalesTurnOver = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[17].Text);
                        sSimulationSubTotal.SubTotalSalesGrossMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[18].Text);
                        sSimulationSubTotal.SubtotalCompare = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[19].Text);
                        sSimulationSubTotal.SubTotalCompareMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[20].Text);

                        iResult = proxy.SimulationSubTotal(sSimulationSubTotal);

                    }
                    catch (Exception error)
                    {
                        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                        {
                            string userName = User.Identity.Name;
                            string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                            //logger.Debug(errorMessage);

                            //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                            //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                            //Server.Transfer("frmResult.aspx", false);
                        }
                    }


                    #endregion

                    #region 6. Reload Simulation Drop down list
                    LoadSimulationList();
                    ddlSimulationID.SelectedValue = strSimulationID;
                    #endregion

                    ViewState["NewBlock"] = false;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolSave").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnSave_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolSave").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void ddlCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["EditBlock"] = false;
            if (ddlCustomerID.SelectedValue.Trim().Equals(""))
            {
                ClearAll();
            }
            else
            {
                LoadSimulationList();

                List<SSimulationList> sSimulationList = new List<SSimulationList>();

                try
                {
                    sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
                }

                 /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "ddlCustomerID_SelectedIndexChanged", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                        //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }

                gvOuterList.DataSource = sSimulationList;
                gvOuterList.DataBind();

                if (sSimulationList.Count > 0)
                {
                    List<GridDetail> Total = new List<GridDetail>();
                    GetTotal();
                }

            }

        }

        protected void GetTotal()
        {
            try
            {
                List<GridDetail> Total = new List<GridDetail>();
                GridDetail gridDetail = new GridDetail();

                List<SSimulationSubTotal> sSimulationGrossTotal = new List<SSimulationSubTotal>();
                sSimulationGrossTotal = proxy.GetSimulationGrossTotal(ddlSimulationID.SelectedValue.Trim(), ddlCustomerID.SelectedValue.Trim());

                SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();

                bool isEditBlock1 = (bool)ViewState["EditBlock"];
                if (isEditBlock1)
                {

                    sSimulationSubTotal.SubTotalWeight = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[4].Text.Trim());
                    //e.Row.Cells[4].Text = fSubTotalWeight.ToString("F");
                    sSimulationSubTotal.SubTotalADV = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[5].Text.Trim());
                    //e.Row.Cells[5].Text = fSubTotalADV.ToString("F");
                    sSimulationSubTotal.SubTotalFreight = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[6].Text.Trim());
                    //e.Row.Cells[6].Text = fSubTotalFreight.ToString("F");
                    sSimulationSubTotal.SubTotalPurchase = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[7].Text.Trim());
                    //e.Row.Cells[7].Text = fSubTotalPurchase.ToString("F");
                    sSimulationSubTotal.SubTotalCurrentDiscount = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[9].Text.Trim());
                    //e.Row.Cells[9].Text = fSubTotalCurrentDiscount.ToString("F");
                    sSimulationSubTotal.SubTotalPublic = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[11].Text.Trim());
                    //e.Row.Cells[11].Text = fSubTotalPublic.ToString("F");
                    sSimulationSubTotal.SubTotalCurrentSale = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[12].Text.Trim());
                    //e.Row.Cells[12].Text = fSubTotalCurrentSale.ToString("F");
                    sSimulationSubTotal.SubTotalTurnOver = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[13].Text.Trim());
                    //e.Row.Cells[13].Text = fSubTotalTurnOver.ToString("F");
                    sSimulationSubTotal.SubTotalGrossMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[14].Text.Trim());
                    //e.Row.Cells[14].Text = fSubTotalGrossMargin.ToString("F");
                    sSimulationSubTotal.SubTotalSalesFretTariff = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[15].Text.Trim());
                    //e.Row.Cells[15].Text = fSubTotalSalesFretTariff.ToString("F");
                    sSimulationSubTotal.SubTotalProposedSalesTariff = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[16].Text.Trim());
                    //e.Row.Cells[16].Text = fSubTotalProposedSalesTariff.ToString("F");
                    sSimulationSubTotal.SubTotalSalesTurnOver = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[17].Text.Trim());
                    //e.Row.Cells[17].Text = fSubTotalSalesTurnOver.ToString("F");
                    sSimulationSubTotal.SubTotalSalesGrossMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[18].Text.Trim());
                    //e.Row.Cells[18].Text = fSubTotalSalesGrossMargin.ToString("F");
                    sSimulationSubTotal.SubtotalCompare = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[19].Text.Trim());
                    //e.Row.Cells[19].Text = fSubtotalCompare.ToString("F");
                    sSimulationSubTotal.SubTotalCompareMargin = (float)Convert.ToDouble(gvCalcualtion.FooterRow.Cells[20].Text.Trim());
                    //e.Row.Cells[20].Text = fSubTotalCompareMargin.ToString("F");
                }
                else
                {
                    sSimulationSubTotal = proxy.GetSimulationSubTotal(ddlSimulationID.SelectedValue.Trim());
                }

                float fCalculateAvgWeight5 = CalculateAvgWeight5(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateADV6 = CalculateADV6(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateFreightPurchase7 = CalculateFreightPurchase7(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculatePurchaseTariff8 = CalculatePurchaseTariff8(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculatePublicTariff12 = CalculatePublicTariff12(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculatePublicSales13 = CalculatePublicSales13(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateCurrentDiscount10 = CalculateCurrentDiscount10(fCalculatePublicTariff12, fCalculatePublicSales13);

                float fCalculatePublicTurnover14 = CalculatePublicTurnover14(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateSalesTariff17 = CalculateSalesTariff17(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateTurnover18 = CalculateTurnover18(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateSalesMargin15 = CalculateSalesMargin15(fCalculatePurchaseTariff8, fCalculateSalesTariff17);

                float fCalculateSalesFret16 = CalculateSalesFret16(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateGrossMargin19 = CalculateGrossMargin19(sSimulationGrossTotal, sSimulationSubTotal);

                float fCalculateCompareSales20 = CalculateCompareSales20(fCalculateSalesTariff17, fCalculatePublicSales13);

                float fCalculateCompareMargin21 = CalculateCompareMargin21(fCalculateTurnover18, fCalculatePublicTurnover14);

                #region Assigning to Class

                gridDetail.gvActualPublicTariff = 0;
                gridDetail.gvADV = fCalculateADV6.ToString();  //sSimulationGrossTotal[0].SubTotalADV.ToString(); //"6";
                gridDetail.gvAverageWeight = Math.Round(fCalculateAvgWeight5, 2).ToString();   //sSimulationGrossTotal[0].SubTotalWeight.ToString();  //"5";
                gridDetail.gvCarrier = "";
                gridDetail.gvComparisonMargin = fCalculateCompareMargin21;  // sSimulationGrossTotal[0].SubTotalCompareMargin;// 21;
                gridDetail.gvComparisonSales = fCalculateCompareSales20;    // sSimulationGrossTotal[0].SubtotalCompare;  //20;
                gridDetail.gvCurrentSaleTariff = fCalculatePublicSales13; //sSimulationGrossTotal[0].SubTotalCurrentSale; // 13;
                gridDetail.gvCurrentTurnOver = fCalculatePublicTurnover14;   // sSimulationGrossTotal[0].SubTotalTurnOver;// 14;
                gridDetail.gvDestinationCountry = "";
                gridDetail.gvDiscount = fCalculateCurrentDiscount10;// sSimulationGrossTotal[0].SubTotalCurrentDiscount; // 10;
                gridDetail.gvFreightPurchase = fCalculateFreightPurchase7; // sSimulationGrossTotal[0].SubTotalFreight; // 7;
                gridDetail.gvFuelSurcharge = 0;
                gridDetail.gvGrossMargin = fCalculateSalesMargin15; // sSimulationGrossTotal[0].SubTotalGrossMargin;   // 15;
                gridDetail.gvIsDiscountUpdated = false;
                gridDetail.gvIsPublicTariffUpdated = false;
                gridDetail.gvMasterServiceName = "";
                gridDetail.gvOldPublicTariff = 0;
                gridDetail.gvOriginCountry = "";
                gridDetail.gvProposedSalesTariff = fCalculateSalesTariff17; // sSimulationGrossTotal[0].SubTotalProposedSalesTariff; // 17;
                gridDetail.gvPublicCarrier = "";
                gridDetail.gvPublicSurcharge = "";
                gridDetail.gvPublicTariff = fCalculatePublicTariff12;   // sSimulationGrossTotal[0].SubTotalPublic; // 12;
                gridDetail.gvPurchaseTariff = fCalculatePurchaseTariff8;// sSimulationGrossTotal[0].SubTotalPurchase;   // 8;
                gridDetail.gvSalesFretTariff = fCalculateSalesFret16;       // sSimulationGrossTotal[0].SubTotalSalesFretTariff; // 16;
                gridDetail.gvSalesGrossMargin = fCalculateGrossMargin19;    // sSimulationGrossTotal[0].SubTotalGrossMargin;  // 19;
                gridDetail.gvSalesTurnOver = fCalculateTurnover18;  // sSimulationGrossTotal[0].SubTotalSalesTurnOver;   // 18;
                gridDetail.gvWeight = "";

                #endregion

                Total.Add(gridDetail);

                gvGrossTotal.DataSource = Total;
                gvGrossTotal.DataBind();
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolGetTotal").ToString(), ddlCustomerID.SelectedValue.Trim());
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "GetTotal", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }

        protected float CalculateAvgWeight5(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            // =(F19*G19+F26*G26)/(G19+G26)  F=> Weight ; G=> ADV
            float result = 0;
            float fTotalAdv = 0;

            float AvgWeightF = 0;
            float ADVG = 0;
            bool isEditBlock = (bool)ViewState["EditBlock"];

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    AvgWeightF = i.SubTotalWeight;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    AvgWeightF = DBSubTotal.SubTotalWeight;
                    ADVG = DBSubTotal.SubTotalADV;
                }
                result += AvgWeightF * ADVG;
                fTotalAdv += ADVG;
            }

            result = result / fTotalAdv;

            return result;
        }

        protected float CalculateADV6(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=G19+G26  = G=> ADV
            float result = 0;
            float ADVG = 0;
            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    ADVG = DBSubTotal.SubTotalADV;
                }
                result += ADVG;
            }

            return result;
        }

        protected float CalculateFreightPurchase7(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=(H19*F19+H26*F26)/(F19+F26)   H=> Freight Purchase ; F=> Weight

            float result = 0;
            float Weight = 0;
            float AvgWeightF = 0;
            float PurchaseH = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    AvgWeightF = i.SubTotalWeight;
                    PurchaseH = i.SubTotalFreight;
                }
                else
                {
                    AvgWeightF = DBSubTotal.SubTotalWeight;
                    PurchaseH = DBSubTotal.SubTotalFreight;
                }
                result += AvgWeightF * PurchaseH;
                Weight += AvgWeightF;
            }
            result = result / Weight;

            return result;
        }

        protected float CalculatePurchaseTariff8(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=(I19*G19+I26*G26)/(G19+G26) I=> Purchase Tariff; G=> ADV

            float result = 0;

            float fTotalAdv = 0;

            float PurchaseTariffI = 0;
            float ADVG = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    PurchaseTariffI = i.SubTotalPurchase;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    PurchaseTariffI = DBSubTotal.SubTotalPurchase;
                    ADVG = DBSubTotal.SubTotalADV;
                }
                result += PurchaseTariffI * ADVG;
                fTotalAdv += ADVG;
            }

            result = result / fTotalAdv;

            return result;
        }

        protected float CalculatePublicTariff12(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=(G19*M19+G26*M26)/(G19+G26)      G=> ADV ; M=> Current public Tariff
            float result = 0;

            float fTotalAdv = 0;

            float PublicTariffM = 0;
            float ADVG = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    PublicTariffM = i.SubTotalPublic;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    PublicTariffM = DBSubTotal.SubTotalPublic;
                    ADVG = DBSubTotal.SubTotalADV;
                }

                result += PublicTariffM * ADVG;
                fTotalAdv += ADVG;
            }

            result = result / fTotalAdv;

            return result;
        }

        protected float CalculatePublicSales13(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=(G19*N19+G26*N26)/G28    G=> ADV ; N=> Current Sales Tariff

            float result = 0;

            float fTotalAdv = 0;

            float CurrentSalesTariffN = 0;
            float ADVG = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    CurrentSalesTariffN = i.SubTotalCurrentSale;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    CurrentSalesTariffN = DBSubTotal.SubTotalCurrentSale;
                    ADVG = DBSubTotal.SubTotalADV;
                }
                result += CurrentSalesTariffN * ADVG;
                fTotalAdv += ADVG;
            }

            result = result / fTotalAdv;

            return result;
        }

        protected float CalculateCurrentDiscount10(float CurrentPublic, float CurrentSale)
        {
            // =(M28-N28)/M28*100  M=> Current Public ; N=> Current Sale

            float result = 0;

            result = (CurrentPublic - CurrentSale) / CurrentPublic * 100;

            return result;
        }

        protected float CalculatePublicTurnover14(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            // =O19+O26     O=> Public Turnover
            float result = 0;

            float PublicTurnoverO = 0;
            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    PublicTurnoverO = i.SubTotalTurnOver;
                }
                else
                {
                    PublicTurnoverO = DBSubTotal.SubTotalTurnOver;
                }
                result += PublicTurnoverO;
            }

            return result;
        }

        protected float CalculateSalesMargin15(float PurchaseTariff, float SaleTariff)
        {
            //=(R28-I28)/I28*100    R=>Sales Tariff   ; I=> PurchaseTariff
            float result = 0;

            result = (SaleTariff - PurchaseTariff) / PurchaseTariff * 100;

            return result;
        }

        protected float CalculateSalesFret16(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            // =(Q19 + Q26)/G28   Q=> Sales Fret Tariff; G=> ADV
            float result = 0;
            float fTotalAdv = 0;
            float ADVG = 0;

            float SalesFretQ = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    SalesFretQ = i.SubTotalSalesFretTariff;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    SalesFretQ = DBSubTotal.SubTotalSalesFretTariff;
                    ADVG = DBSubTotal.SubTotalADV;
                }

                result += SalesFretQ;
                fTotalAdv += ADVG;
            }
            result = result / fTotalAdv;
            return result;
        }

        protected float CalculateSalesTariff17(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=(G19*R19+G26*R26)/G28    G=> ADV ; R=> Sales Tariff

            float result = 0;

            float fTotalAdv = 0;

            float SalesTariffR = 0;
            float ADVG = 0;

            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    SalesTariffR = i.SubTotalProposedSalesTariff;
                    ADVG = i.SubTotalADV;
                }
                else
                {
                    SalesTariffR = DBSubTotal.SubTotalProposedSalesTariff;
                    ADVG = DBSubTotal.SubTotalADV;
                }

                result += SalesTariffR * ADVG;
                fTotalAdv += ADVG;
            }

            result = result / fTotalAdv;

            return result;
        }

        protected float CalculateTurnover18(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=S19+S26      S=>Sale Turnover
            float result = 0;

            float SalesTurnoverS = 0;
            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    SalesTurnoverS = i.SubTotalSalesTurnOver;
                }
                else
                {
                    SalesTurnoverS = DBSubTotal.SubTotalSalesTurnOver;
                }
                result += SalesTurnoverS;
            }

            return result;
        }

        protected float CalculateGrossMargin19(List<SSimulationSubTotal> SubTotal, SSimulationSubTotal DBSubTotal)
        {
            //=T19+T26   T=> Gross Margin
            float result = 0;

            float SalesGrossMarginS = 0;
            foreach (var i in SubTotal)
            {
                if (!(ddlSimulationID.SelectedValue.Equals(i.SimulationID)))
                {
                    SalesGrossMarginS = i.SubTotalSalesGrossMargin;
                }
                else
                {
                    SalesGrossMarginS = DBSubTotal.SubTotalSalesGrossMargin;
                }
                result += SalesGrossMarginS;
            }

            return result;
        }

        protected float CalculateCompareSales20(float SalesTariff, float PublicTariff)
        {
            //=(R28-N28)/N28*100
            float result = 0;

            result = (SalesTariff - PublicTariff) / PublicTariff * 100;

            return result;
        }

        protected float CalculateCompareMargin21(float SalesTurnover, float CurrentTurnover)
        {
            //=(S28-O28)/O28*100  S=> sales Turnover ; O=> Current Turnover
            float result = 0;

            result = (SalesTurnover - CurrentTurnover) / CurrentTurnover * 100;

            return result;
        }

        protected void ddlSimulationID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSimulationID = ddlSimulationID.SelectedValue.Trim();
            try
            {
                LoadCalculationBlock(strSimulationID);
                List<SSimulationList> sSimulationList = new List<SSimulationList>();
                sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
                gvOuterList.DataSource = sSimulationList;
                gvOuterList.DataBind();
                GetTotal();
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolSimulationLoad").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "ddlSimulationID_SelectedIndexChanged", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolSimulationLoad").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected PublicDiscount GetPublicDiscount(string CarrierName)
        {
            PublicDiscount publicDiscount = new PublicDiscount();
            bool isValuefilled = false;
            foreach (var item in publicDiscountDB)
            {
                if (item.CarrierName.Equals(CarrierName))
                {
                    publicDiscount.FuelDiscount = item.FuelDiscount;
                    publicDiscount.SafetyDiscount = item.SafetyDiscount;
                    isValuefilled = true;
                }
            }
            if (!isValuefilled)
            {
                publicDiscount.FuelDiscount = "0";
                publicDiscount.SafetyDiscount = "0";

            }
            return publicDiscount;

        }

        protected string GetSelectedTariffBased(string CarrierName, string CarrierType)
        {
            string result = "N/A";

            foreach (var item in sSimulationTariffBased)
            {
                if ((item.CarrierName.Equals(CarrierName)) && (item.TariffType.Equals(CarrierType)))
                {
                    result = item.TariffReference;
                }

            }

            return result;

        }

        protected void LoadSimulationList()
        {
            List<SSimulationList> sSimulationList = new List<SSimulationList>();
            sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
            ddlSimulationID.DataSource = sSimulationList;
            ddlSimulationID.DataTextField = "TextField";
            ddlSimulationID.DataValueField = "DataField";
            ddlSimulationID.DataBind();
            ddlSimulationID.Items.Insert(0, new ListItem(String.Empty, " "));
        }

        protected void LoadCalculationBlock(string strSimulationID)
        {

            #region  1. Header

            SSimulationHeader sSimulationHeader = new SSimulationHeader();
            sSimulationHeader = proxy.GetSimulationHeader(strSimulationID);

            if (sSimulationHeader.SimulationID.Equals(""))
            {
                txtValidDate.Text = "";
            }
            else
            {
                txtValidDate.Text = sSimulationHeader.Valid.ToString("dd/MM/yyyy");
                txtWeighLimit.Text = sSimulationHeader.WeightLimit.ToString();
                txtWeightIncrement.Text = sSimulationHeader.WeightIncrement.ToString();
                ddlDefaultTariff.SelectedValue = sSimulationHeader.AssignedTariff;
            }
            #endregion

            #region 2. Surcharge Discount

            List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount = new List<SSimulationSurchargeDiscount>();

            sSimulationSurchargeDiscount = proxy.GetSimulationSurchargeDiscount(strSimulationID);

            foreach (var s in sSimulationSurchargeDiscount)
            {
                PublicDiscount p = new PublicDiscount();
                p.CarrierName = s.CarrierName;
                p.FuelDiscount = s.FuelDiscount.ToString();
                p.SafetyDiscount = s.SafetyDiscount.ToString();
                publicDiscountDB.Add(p);
            }

            List<STariffMaster> sTariffMaster = new List<STariffMaster>();
            sTariffMaster = proxy.GetAllCarrierNames();
            gvCustomerDiscount.DataSource = sTariffMaster;
            gvCustomerDiscount.DataBind();


            #endregion

            #region 3. Purchase Tariff Selection

            sSimulationTariffBased = proxy.GetSimulationTariffBased(strSimulationID);

            List<STariffMaster> sTariffBased = new List<STariffMaster>();
            sTariffBased = proxy.GetTariffAssignedCarrierNames("PURCHASE");
            gvCarrierTariff.DataSource = sTariffBased;
            gvCarrierTariff.DataBind();

            #endregion

            #region 4. Calculation Grid

            List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
            sSimulationTariff = proxy.GetSimulationTariff(strSimulationID);
            List<GridDetail> gridDetail = new List<GridDetail>();

            foreach (var s in sSimulationTariff)
            {
                GridDetail g = new GridDetail();

                //g.gvActualPublicTariff
                g.gvADV = s.ADV.ToString();
                g.gvAverageWeight = s.AverageWeight.ToString();
                g.gvCarrier = s.PurchaseCarrier;
                g.gvComparisonMargin = (float)s.ComparisonMargin;
                g.gvComparisonSales = (float)s.ComparisonSaleTariff;
                g.gvCurrentSaleTariff = (float)s.PublicTariff;
                g.gvCurrentTurnOver = (float)s.PublicTurnOver;
                g.gvDiscount = (float)s.PublicDiscount;
                g.gvFreightPurchase = (float)s.PurchaseFreight;
                g.gvGrossMargin = (float)s.SaleMargin;
                g.gvMasterServiceName = s.MasterServiceName;
                g.gvOriginCountry = s.ShipCountry;
                g.gvProposedSalesTariff = s.SaleTariff;
                g.gvPublicCarrier = s.PublicCarrier;
                //g.gvIsPublicTariffUpdated
                g.gvPublicSurcharge = s.PublicSurcharge;
                //g.gvOldPublicTariff
                //g.gvPublicSurcharge
                g.gvPublicTariff = s.PublicFreight;
                g.gvPurchaseTariff = s.PurchaseTariff;
                g.gvSalesFretTariff = s.SaleFreight;
                g.gvSalesGrossMargin = s.SaleGrossMargin;
                g.gvSalesTurnOver = s.SaleTurnOver;
                g.gvDestinationCountry = s.DeliveryCountry;
                g.gvWeight = s.WeightRange;
                strSelectedOrigin = s.ShipCountry.Trim();
                strSelectedDestination = s.DeliveryCountry.Trim();
                strSelectedMasterType = s.MasterServiceName.Trim();

                gridDetail.Add(g);
            }

            #endregion

            #region 5. Grid Sub Total

            SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();
            sSimulationSubTotal = proxy.GetSimulationSubTotal(strSimulationID);
            fSubTotalADV = sSimulationSubTotal.SubTotalADV;
            fSubtotalCompare = sSimulationSubTotal.SubtotalCompare;
            fSubTotalCompareMargin = sSimulationSubTotal.SubTotalCompareMargin;
            fSubTotalCurrentDiscount = sSimulationSubTotal.SubTotalCurrentDiscount;
            fSubTotalCurrentSale = sSimulationSubTotal.SubTotalCurrentSale;
            fSubTotalFreight = sSimulationSubTotal.SubTotalFreight;
            fSubTotalGrossMargin = sSimulationSubTotal.SubTotalGrossMargin;
            fSubTotalProposedSalesTariff = sSimulationSubTotal.SubTotalProposedSalesTariff;
            fSubTotalPublic = sSimulationSubTotal.SubTotalPublic;
            fSubTotalPurchase = sSimulationSubTotal.SubTotalPurchase;
            fSubTotalSalesFretTariff = sSimulationSubTotal.SubTotalSalesFretTariff;
            fSubTotalSalesGrossMargin = sSimulationSubTotal.SubTotalSalesGrossMargin;
            fSubTotalSalesTurnOver = sSimulationSubTotal.SubTotalSalesTurnOver;
            fSubTotalTurnOver = sSimulationSubTotal.SubTotalTurnOver;
            fSubTotalWeight = sSimulationSubTotal.SubTotalWeight;

            #endregion

            ViewState["vwGridDetail"] = gridDetail.ToList();
            ViewState["NewBlock"] = false;
            vwGridDetail = (List<GridDetail>)ViewState["vwGridDetail"];

            gvCalcualtion.DataSource = gridDetail;
            gvCalcualtion.DataBind();

        }

        protected void gvCalculationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<SCountryTable> sCountryTable = new List<SCountryTable>();
            List<SComboText> sComboText = new List<SComboText>();
            SComboTableField sComboTableField = new SComboTableField();
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (irow == 0)
                {
                    sCountryTable = proxy.FillCountryCombo().ToList();

                    // Shipping Country
                    DropDownList ddlList = e.Row.FindControl("ddlShippingCountryC2") as DropDownList;
                    sTariffMaster = proxy.GetAllCarrierNames();

                    ddlList.DataSource = sCountryTable;
                    ddlList.DataTextField = "CodeName";
                    ddlList.DataValueField = "CountryCode";

                    ddlList.DataBind();
                    ddlList.SelectedValue = strSelectedOrigin;

                    // Delivery Country
                    ddlList = e.Row.FindControl("ddlDeliveryCountryC2") as DropDownList;
                    ddlList.DataSource = sCountryTable;
                    ddlList.DataTextField = "CodeName";
                    ddlList.DataValueField = "CountryCode";
                    ddlList.DataBind();
                    ddlList.SelectedValue = strSelectedDestination;

                    //To fill Master Service Names
                    sComboTableField.FieldName = "MASTER_SERVICE_NAME";
                    sComboTableField.TableName = "MASTER_SERVICE";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();
                    ddlList = e.Row.FindControl("ddlMasterServiceNameC3") as DropDownList;
                    ddlList.DataSource = sComboText;
                    ddlList.DataTextField = "ComboText";
                    if (!(strSelectedMasterType.Equals("")))
                        ddlList.SelectedValue = strSelectedMasterType;
                    ddlList.DataBind();
                    irow++;

                }
                else
                {
                    DropDownList ddlList = e.Row.FindControl("ddlShippingCountryC2") as DropDownList;
                    ddlList.Visible = false;
                    ddlList = e.Row.FindControl("ddlDeliveryCountryC2") as DropDownList;
                    ddlList.Visible = false;
                    ddlList = e.Row.FindControl("ddlMasterServiceNameC3") as DropDownList;
                    ddlList.Visible = false;

                }

                e.Row.Cells[1].Style.Add("border-top-width", "0px");
                e.Row.Cells[1].Style.Add("border-bottom-width", "0px");
                e.Row.Cells[2].Style.Add("border-top-width", "0px");
                e.Row.Cells[2].Style.Add("border-bottom-width", "0px");
                e.Row.Cells[3].Style.Add("border-top-width", "0px");
                e.Row.Cells[3].Style.Add("border-bottom-width", "0px");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Sub Total";
                e.Row.Cells[4].Text = fSubTotalWeight.ToString("F");
                e.Row.Cells[5].Text = fSubTotalADV.ToString("F");
                e.Row.Cells[6].Text = fSubTotalFreight.ToString("F");
                e.Row.Cells[7].Text = fSubTotalPurchase.ToString("F");
                e.Row.Cells[9].Text = fSubTotalCurrentDiscount.ToString("F");
                e.Row.Cells[11].Text = fSubTotalPublic.ToString("F");
                e.Row.Cells[12].Text = fSubTotalCurrentSale.ToString("F");
                e.Row.Cells[13].Text = fSubTotalTurnOver.ToString("F");
                e.Row.Cells[14].Text = fSubTotalGrossMargin.ToString("F");
                e.Row.Cells[15].Text = fSubTotalSalesFretTariff.ToString("F");
                e.Row.Cells[16].Text = fSubTotalProposedSalesTariff.ToString("F");
                e.Row.Cells[17].Text = fSubTotalSalesTurnOver.ToString("F");
                e.Row.Cells[18].Text = fSubTotalSalesGrossMargin.ToString("F");
                e.Row.Cells[19].Text = fSubtotalCompare.ToString("F");
                e.Row.Cells[20].Text = fSubTotalCompareMargin.ToString("F");
            }

        }

        protected void gvOuterList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string strSimulationID = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gv = (GridView)e.Row.FindControl("gvCalculationList");
                Label LabelValue = (Label)e.Row.FindControl("lblID");
                strSimulationID = LabelValue.Text;

                if (!(strSimulationID.Equals(ddlSimulationID.SelectedValue.Trim())))
                {
                    irow = 0;
                    List<GridDetail> gridDetail = new List<GridDetail>();
                    gridDetail = LoadSimulationGridList(strSimulationID);

                    LabelValue = (Label)e.Row.FindControl("lblGHShipcountry");
                    LabelValue.Text = " : " + gridDetail[0].gvOriginCountry;

                    LabelValue = (Label)e.Row.FindControl("lblGHDeliverycountry");
                    LabelValue.Text = " : " + gridDetail[0].gvDestinationCountry;

                    LabelValue = (Label)e.Row.FindControl("lblGHMasterService");
                    LabelValue.Text = " : " + gridDetail[0].gvMasterServiceName;

                    gv.DataSource = gridDetail;
                    gv.DataBind();
                }
                else
                {
                    e.Row.Visible = false;
                }

                #region Commented

                //#region 4. Calculation Grid

                //List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
                //sSimulationTariff = proxy.GetSimulationTariff(strSimulationID);


                //foreach (var s in sSimulationTariff)
                //{
                //    GridDetail g = new GridDetail();

                //    //g.gvActualPublicTariff
                //    g.gvADV = s.ADV.ToString();
                //    g.gvAverageWeight = s.AverageWeight.ToString();
                //    g.gvCarrier = s.PurchaseCarrier;
                //    g.gvComparisonMargin = (float)s.ComparisonMargin;
                //    g.gvComparisonSales = (float)s.ComparisonSaleTariff;
                //    g.gvCurrentSaleTariff = (float)s.PurchaseTariff;
                //    g.gvCurrentTurnOver = (float)s.PublicTurnOver;
                //    g.gvDiscount = (float)s.PublicDiscount;
                //    g.gvFreightPurchase = (float)s.PurchaseFreight;
                //    g.gvGrossMargin = (float)s.SaleGrossMargin;
                //    g.gvMasterServiceName = s.MasterServiceName;
                //    g.gvOriginCountry = s.ShipCountry;
                //    g.gvProposedSalesTariff = s.SaleTariff;
                //    g.gvPublicCarrier = s.PublicCarrier;
                //    //g.gvIsPublicTariffUpdated
                //    //g.gvPublicSurcharge
                //    //g.gvOldPublicTariff
                //    //g.gvPublicSurcharge
                //    g.gvPublicTariff = s.PublicTariff;
                //    g.gvPurchaseTariff = s.PurchaseTariff;
                //    g.gvSalesFretTariff = s.SaleFreight;
                //    g.gvSalesGrossMargin = s.SaleGrossMargin;
                //    g.gvSalesTurnOver = s.SaleTurnOver;
                //    g.gvShippingCountry = s.ShipCountry;
                //    g.gvWeight = s.WeightRange;
                //    strSelectedOrigin = s.ShipCountry.Trim();
                //    strSelectedDestination = s.DeliveryCountry.Trim();

                //    gridDetail.Add(g);
                //}

                //#endregion

                //#region 5. Grid Sub Total

                //SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();
                //sSimulationSubTotal = proxy.GetSimulationSubTotal(strSimulationID);
                //fSubTotalADV = sSimulationSubTotal.SubTotalADV;
                //fSubtotalCompare = sSimulationSubTotal.SubtotalCompare;
                //fSubTotalCompareMargin = sSimulationSubTotal.SubTotalCompareMargin;
                //fSubTotalCurrentDiscount = sSimulationSubTotal.SubTotalCurrentDiscount;
                //fSubTotalCurrentSale = sSimulationSubTotal.SubTotalCurrentSale;
                //fSubTotalFreight = sSimulationSubTotal.SubTotalFreight;
                //fSubTotalGrossMargin = sSimulationSubTotal.SubTotalGrossMargin;
                //fSubTotalProposedSalesTariff = sSimulationSubTotal.SubTotalProposedSalesTariff;
                //fSubTotalPublic = sSimulationSubTotal.SubTotalPublic;
                //fSubTotalPurchase = sSimulationSubTotal.SubTotalPurchase;
                //fSubTotalSalesFretTariff = sSimulationSubTotal.SubTotalSalesFretTariff;
                //fSubTotalSalesGrossMargin = sSimulationSubTotal.SubTotalSalesGrossMargin;
                //fSubTotalSalesTurnOver = sSimulationSubTotal.SubTotalSalesTurnOver;
                //fSubTotalTurnOver = sSimulationSubTotal.SubTotalTurnOver;
                //fSubTotalWeight = sSimulationSubTotal.SubTotalWeight;

                //#endregion
                #endregion
            }
        }

        protected List<GridDetail> LoadSimulationGridList(string strSimulationID)
        {
            //    List<SSimulationList> sSimulationList = new List<SSimulationList>();
            //    sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
            //    strSimulationID = sSimulationList[0].DataField;

            #region 4. Calculation Grid

            List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
            sSimulationTariff = proxy.GetSimulationTariff(strSimulationID);
            List<GridDetail> gridDetail = new List<GridDetail>();

            foreach (var s in sSimulationTariff)
            {
                GridDetail g = new GridDetail();

                //g.gvActualPublicTariff
                g.gvADV = s.ADV.ToString();
                g.gvAverageWeight = s.AverageWeight.ToString();
                g.gvCarrier = s.PurchaseCarrier;
                g.gvComparisonMargin = (float)s.ComparisonMargin;
                g.gvComparisonSales = (float)s.ComparisonSaleTariff;
                //g.gvCurrentSaleTariff = (float)s.PurchaseTariff;
                g.gvCurrentSaleTariff = (float)s.PublicTariff;

                g.gvCurrentTurnOver = (float)s.PublicTurnOver;
                g.gvDiscount = (float)s.PublicDiscount;
                g.gvFreightPurchase = (float)s.PurchaseFreight;
                g.gvGrossMargin = (float)s.SaleMargin;
                g.gvMasterServiceName = s.MasterServiceName;
                g.gvOriginCountry = s.ShipCountry;
                g.gvProposedSalesTariff = s.SaleTariff;
                g.gvPublicCarrier = s.PublicCarrier;
                //g.gvIsPublicTariffUpdated
                g.gvPublicSurcharge = s.PublicSurcharge;
                //g.gvOldPublicTariff
                //g.gvPublicSurcharge
                g.gvPublicTariff = s.PublicFreight;
                g.gvPurchaseTariff = s.PurchaseTariff;
                g.gvSalesFretTariff = s.SaleFreight;
                g.gvSalesGrossMargin = s.SaleGrossMargin;
                g.gvSalesTurnOver = s.SaleTurnOver;
                g.gvDestinationCountry = s.DeliveryCountry;
                g.gvWeight = s.WeightRange;

                strSelectedOrigin = s.ShipCountry.Trim();
                strSelectedDestination = s.DeliveryCountry.Trim();
                strSelectedMasterType = s.MasterServiceName.Trim();

                gridDetail.Add(g);
            }

            #endregion

            #region 5. Grid Sub Total

            SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();
            sSimulationSubTotal = proxy.GetSimulationSubTotal(strSimulationID);
            fSubTotalADV = sSimulationSubTotal.SubTotalADV;
            fSubtotalCompare = sSimulationSubTotal.SubtotalCompare;
            fSubTotalCompareMargin = sSimulationSubTotal.SubTotalCompareMargin;
            fSubTotalCurrentDiscount = sSimulationSubTotal.SubTotalCurrentDiscount;
            fSubTotalCurrentSale = sSimulationSubTotal.SubTotalCurrentSale;
            fSubTotalFreight = sSimulationSubTotal.SubTotalFreight;
            fSubTotalGrossMargin = sSimulationSubTotal.SubTotalGrossMargin;
            fSubTotalProposedSalesTariff = sSimulationSubTotal.SubTotalProposedSalesTariff;
            fSubTotalPublic = sSimulationSubTotal.SubTotalPublic;
            fSubTotalPurchase = sSimulationSubTotal.SubTotalPurchase;
            fSubTotalSalesFretTariff = sSimulationSubTotal.SubTotalSalesFretTariff;
            fSubTotalSalesGrossMargin = sSimulationSubTotal.SubTotalSalesGrossMargin;
            fSubTotalSalesTurnOver = sSimulationSubTotal.SubTotalSalesTurnOver;
            fSubTotalTurnOver = sSimulationSubTotal.SubTotalTurnOver;
            fSubTotalWeight = sSimulationSubTotal.SubTotalWeight;

            #endregion

            return gridDetail;
        }

        protected void gvCalcualtion_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow =
                new GridViewRow(0, 0, DataControlRowType.Header,
                DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                //HeaderCell.Text = "Shipping Profile";
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "SMShipProfile").ToString(); //"Shipping Profile";
                HeaderCell.ColumnSpan = 6;
                HeaderCell.CssClass = "gridHeaderCell1";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "SMPurchaseHeader").ToString(); //"Kaizos Purchase tariffs";
                HeaderCell.ColumnSpan = 3;
                HeaderCell.CssClass = "gridHeaderCell2";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "SMCurrentHeader").ToString(); //"Current Budget";
                HeaderCell.ColumnSpan = 5;
                HeaderCell.CssClass = "gridHeaderCell3";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "SMSalesHeader").ToString(); //"Sales tariffs";
                HeaderCell.ColumnSpan = 5;
                HeaderCell.CssClass = "gridHeaderCell4";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "SMComparisonHeader").ToString(); //"Comparison";
                HeaderCell.CssClass = "gridHeaderCell5";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                gvCalcualtion.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }

        protected void val_Header_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            decimal d;
            DateTime value;
            string DateFormat = "dd/MM/yyyy";

            if (ddlCustomerID.SelectedValue.Trim().Equals(""))
            {
                strError = strError + "*" + lblCustomerID.Text.Trim() + " " + valSelection.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (txtValidDate.Text.Equals(""))
            {
                strError = strError + "*" + lblValidUntil.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (!DateTime.TryParseExact(txtValidDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                strError = strError + "*" + lblValidUntil.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (DateTime.ParseExact(txtValidDate.Text, DateFormat, CultureInfo.InvariantCulture) < DateTime.Now)
            {
                strError = strError + "*" + lblValidUntil.Text.Trim() + " " + valGreater.Text.Trim() + " " + DateTime.Now.ToShortDateString() + "<br>";
                args.IsValid = false;
            }

            if (txtWeighLimit.Text.Equals(""))
            {
                strError = strError + "*" + lblWeightLimit.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtWeighLimit.Equals(""))) && (!Decimal.TryParse(txtWeighLimit.Text, out d)))
            {
                strError = strError + "*" + lblWeightLimit.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (!(isValidTwoDecimalPercentage(txtWeighLimit.Text)))
            {
                strError = strError + "*" + lblWeightLimit.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (txtWeightIncrement.Text.Equals(""))
            {
                strError = strError + "*" + lblWeightIncrement.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtWeightIncrement.Equals(""))) && (!Decimal.TryParse(txtWeightIncrement.Text, out d)))
            {
                strError = strError + "*" + lblWeightIncrement.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDecimal(txtWeightIncrement.Text) <= 0)
            {
                strError = strError + "*" + lblWeightIncrement.Text.Trim() + " " + ValPositive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (!(isValidTwoDecimalPercentage(txtWeightIncrement.Text)))
            {
                strError = strError + "*" + lblWeightIncrement.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (args.IsValid)
            {
                decimal WeightIncrement = Convert.ToDecimal(txtWeightIncrement.Text.Trim());
                decimal MaxWeight = Convert.ToDecimal(txtWeighLimit.Text.Trim());
                int rows = Convert.ToInt32(MaxWeight / WeightIncrement);
                int MaxAllowed = Convert.ToInt32(ViewState["MaxRow"]);
                if (rows > MaxAllowed)
                {
                    strError = strError + string.Format(GetGlobalResourceObject("LocalString", "SMMaxAllowedRows").ToString(), MaxAllowed.ToString().Trim()) + "<br>";
                    args.IsValid = false;
                }
            }

            if (!(args.IsValid))
            {
                val_Header.ErrorMessage = strError;
				errorMsg1.Attributes["style"] = "display: block;";
            }

        }

        protected void val_Calculation_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            decimal d;

            bool isPublicSelected = true;   // false; Test purpsose turned true;
            bool isPurchaseSelected = false;

            string strNumericFuelCarrier = "";
            string strEmptyFuelCarrier = "";
            string strLessFuelCarrier = "";
            string strInvalidFuelCarrier = "";

            string strNumericSafetyCarrier = "";
            string strEmptySafetyCarrier = "";
            string strLessSafetyCarrier = "";
            string strInvalidSafetyCarrier = "";

            string strPublicCarrierList = "";

            bool isEmptyAvgWeight = false;
            bool isEmptyADV = false;
            bool isEmptyDiscount = false;
            bool isEmptyPublicTariff = false;
            bool isEmptyMargin = false;

            bool isValidAvgWeight = true;
            bool isValidADV = true;
            bool isValidDiscount = true;
            bool isValidPublicTariff = true;
            bool isValidMargin = true;
            bool isValidFormatMargin = true;
            bool isHigherDiscount = false;
            bool isHigherMargin = false;

            #region Tariff Based selection

            for (int i = 0; i < gvCarrierTariff.Rows.Count; i++)
            {
                Label lblSName = (Label)gvCarrierTariff.Rows[i].FindControl("lblServiceName");
                DropDownList ddlPTariff = (DropDownList)gvCarrierTariff.Rows[i].FindControl("ddlPurchaseTariffList");
                DropDownList ddlPubTariff = (DropDownList)gvCarrierTariff.Rows[i].FindControl("ddlPublicTariffList");
                if (ddlPTariff.SelectedValue != "N/A")
                {
                    isPurchaseSelected = true;
                }

                if ((ddlPTariff.SelectedValue != "N/A") && (ddlPubTariff.SelectedValue == "N/A"))
                {
                    strPublicCarrierList = strPublicCarrierList + lblSName.Text + ", ";
                    isPublicSelected = false;
                }
            }

            if (!isPurchaseSelected)
            {
                strError = strError + "*" + GetGlobalResourceObject("LocalString", "AtleastOne").ToString() + " " +
                              GetGlobalResourceObject("LocalString", "Purchase").ToString() + lblCarrier.Text + " " + valSelection.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!isPublicSelected)
            {
                strError = strError + "*" + strPublicCarrierList.Substring(0, strPublicCarrierList.Length - 2) +
                    GetGlobalResourceObject("LocalString", "Public").ToString() + lblCarrier.Text + " " + valSelection.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            #endregion

            #region Surcharge Discount

            for (int i = 0; i < gvCustomerDiscount.Rows.Count; i++)
            {
                Label lblLabel = (Label)gvCustomerDiscount.Rows[i].FindControl("lblCustomerServiceName");
                TextBox txtText = (TextBox)gvCustomerDiscount.Rows[i].FindControl("txtFuelDiscount");

                if (txtText.Text.Equals(""))
                {
                    strEmptyFuelCarrier = strEmptyFuelCarrier + lblLabel.Text.Trim() + ",";
                    //strError = strError + "* '" + lblLabel.Text.Trim() + "' " + gvCustomerDiscount.HeaderRow.Cells[1].Text  + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    strNumericFuelCarrier = strNumericFuelCarrier + lblLabel.Text.Trim() + ",";
                    // strError = strError + "* '" + lblLabel.Text.Trim() + "' " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valNumber.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!(isValidTwoDecimalPercentage(txtText.Text)))
                {
                    strInvalidFuelCarrier = strInvalidFuelCarrier + lblLabel.Text.Trim() + ",";

                    args.IsValid = false;
                }
                else if (Convert.ToDecimal(txtText.Text) > 100)
                {
                    strLessFuelCarrier = strLessFuelCarrier + lblLabel.Text.Trim() + ",";
                    //strError = strError + "* '" + lblLabel.Text.Trim() + "' " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valCalLess.Text.Trim() + " 100 <br>";
                    args.IsValid = false;
                }

                txtText = (TextBox)gvCustomerDiscount.Rows[i].FindControl("txtSafetyDiscount");

                if (txtText.Text.Equals(""))
                {
                    strEmptySafetyCarrier = strEmptySafetyCarrier + lblLabel.Text.Trim() + ",";
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    strNumericSafetyCarrier = strNumericSafetyCarrier + lblLabel.Text.Trim() + ",";
                    args.IsValid = false;
                }
                else if (!(isValidTwoDecimalPercentage(txtText.Text)))
                {
                    strInvalidSafetyCarrier = strInvalidSafetyCarrier + lblLabel.Text.Trim() + ",";

                    args.IsValid = false;
                }
                else if (Convert.ToDecimal(txtText.Text) > 100)
                {
                    strLessSafetyCarrier = strLessSafetyCarrier + lblLabel.Text.Trim() + ",";
                    args.IsValid = false;
                }

            }

            // Fuel
            if (strNumericFuelCarrier.Length > 2)
            {
                strNumericFuelCarrier = strNumericFuelCarrier.Substring(0, strNumericFuelCarrier.Length - 1);
                strError = strError + "* " + strNumericFuelCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valNumber.Text.Trim() + "<br>";
            }

            if (strEmptyFuelCarrier.Length > 2)
            {
                strEmptyFuelCarrier = strEmptyFuelCarrier.Substring(0, strEmptyFuelCarrier.Length - 1);
                strError = strError + "* " + strEmptyFuelCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valEmpty.Text.Trim() + "<br>";
            }

            if (strLessFuelCarrier.Length > 2)
            {
                strLessFuelCarrier = strLessFuelCarrier.Substring(0, strLessFuelCarrier.Length - 1);
                strError = strError + "* " + strLessFuelCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valCalLess.Text.Trim() + "100<br>";
            }

            if (strInvalidFuelCarrier.Length > 2)
            {
                strInvalidFuelCarrier = strInvalidFuelCarrier.Substring(0, strInvalidFuelCarrier.Length - 1);
                strError = strError + "* " + strInvalidFuelCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[1].Text + " " + valInvalid.Text.Trim() + "<br>";
            }

            // Safety
            if (strNumericSafetyCarrier.Length > 2)
            {
                strNumericSafetyCarrier = strNumericSafetyCarrier.Substring(0, strNumericSafetyCarrier.Length - 1);
                strError = strError + "* " + strNumericSafetyCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[2].Text + " " + valNumber.Text.Trim() + "<br>";
            }

            if (strEmptySafetyCarrier.Length > 2)
            {
                strEmptySafetyCarrier = strEmptySafetyCarrier.Substring(0, strEmptySafetyCarrier.Length - 1);
                strError = strError + "* " + strEmptySafetyCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[2].Text + " " + valEmpty.Text.Trim() + "<br>";
            }

            if (strLessSafetyCarrier.Length > 2)
            {
                strLessSafetyCarrier = strLessSafetyCarrier.Substring(0, strLessSafetyCarrier.Length - 1);
                strError = strError + "* " + strLessSafetyCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[2].Text + " " + valCalLess.Text.Trim() + "100<br>";
            }

            if (strInvalidSafetyCarrier.Length > 2)
            {
                strInvalidSafetyCarrier = strInvalidSafetyCarrier.Substring(0, strInvalidSafetyCarrier.Length - 1);
                strError = strError + "* " + strInvalidSafetyCarrier + " " + gvCustomerDiscount.HeaderRow.Cells[2].Text + " " + valInvalid.Text.Trim() + "<br>";
            }

            #endregion

            #region Calculation Grid

            for (int i = 0; i < gvCalcualtion.Rows.Count; i++)
            {
                TextBox txtText = (TextBox)gvCalcualtion.Rows[i].FindControl("txtAverageWeight");

                if (txtText.Text.Equals(""))
                {
                    isEmptyAvgWeight = true;
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    isValidAvgWeight = false;
                    args.IsValid = false;
                }

                txtText = (TextBox)gvCalcualtion.Rows[i].FindControl("txtAdv");
                if (txtText.Text.Equals(""))
                {
                    isEmptyADV = true;
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    isValidADV = false;
                    args.IsValid = false;
                }

                txtText = (TextBox)gvCalcualtion.Rows[i].FindControl("txtDiscount");
                if (txtText.Text.Equals(""))
                {
                    isEmptyDiscount = true;
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    isValidDiscount = false;
                    args.IsValid = false;
                }
                else if (Convert.ToDecimal(txtText.Text) > 100)
                {
                    isHigherDiscount = true;
                    args.IsValid = false;
                }


                txtText = (TextBox)gvCalcualtion.Rows[i].FindControl("txtPublicFreight");
                if (txtText.Text.Equals(""))
                {
                    isEmptyPublicTariff = true;
                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    isValidPublicTariff = false;
                    args.IsValid = false;
                }

                txtText = (TextBox)gvCalcualtion.Rows[i].FindControl("txtGrossMarign");
                if (txtText.Text.Equals(""))
                {
                    isEmptyMargin = true;

                    args.IsValid = false;
                }
                else if ((!(txtText.Equals(""))) && (!Decimal.TryParse(txtText.Text, out d)))
                {
                    isValidMargin = false;

                    args.IsValid = false;
                }
                else if (!(isValidPercentage(txtText.Text)))
                {
                    isValidFormatMargin = false;

                    args.IsValid = false;
                }
                else if (Convert.ToDecimal(txtText.Text) > 100)
                {
                    isHigherMargin = true;

                    args.IsValid = false;
                }

            }


            if (isEmptyAvgWeight)
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMAverageWeight").ToString() + " " + valEmpty.Text.Trim() + "<br>";

            if (!(isValidAvgWeight))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMAverageWeight").ToString() + " " + valNumber.Text.Trim() + "<br>";


            if (isEmptyADV)
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMADV").ToString() + " " + valEmpty.Text.Trim() + "<br>";

            if (!(isValidADV))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMADV").ToString() + " " + valNumber.Text.Trim() + "<br>";


            if (isEmptyDiscount)
                strError = strError + "* '" + GetGlobalResourceObject("LocalString", "SMDiscount").ToString() + " " + valEmpty.Text.Trim() + "<br>";

            if (!(isValidDiscount))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMDiscount").ToString() + " " + valNumber.Text.Trim() + "<br>";

            if (isHigherDiscount)
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMDiscount").ToString() + " " + valCalLess.Text.Trim() + " 100<br>";


            if (isEmptyPublicTariff)
                strError = strError + "* '" + GetGlobalResourceObject("LocalString", "SMPublicTariff").ToString() + " " + valEmpty.Text.Trim() + "<br>";

            if (!(isValidPublicTariff))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMPublicTariff").ToString() + " " + valNumber.Text.Trim() + "<br>";


            if (isEmptyMargin)
                strError = strError + "* '" + GetGlobalResourceObject("LocalString", "SMMargin").ToString() + " " + valEmpty.Text.Trim() + "<br>";

            if (!(isValidMargin))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMMargin").ToString() + " " + valNumber.Text.Trim() + "<br>";

            if (!(isValidFormatMargin))
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMMargin").ToString() + " " + valInvalid.Text.Trim() + "<br>";

            if (isHigherMargin)
                strError = strError + "* " + GetGlobalResourceObject("LocalString", "SMMargin").ToString() + " " + valCalLess.Text.Trim() + " 100<br>";

            #endregion

            if (!(args.IsValid))
            {
                val_Calculation.ErrorMessage = strError;
                errorMsg2.Attributes["style"] = "display: block;";
            }
        }

        protected bool isValidDate(string Date)
        {
            bool result = true;
            DateTime d;
            DateTime startDate;
            string DateFormat = "dd/MM/yyyy";
            // Date = Convert.ToDateTime(Date).ToString("MM/dd/yyyy"); 

            if (!(DateTime.TryParse(Date, out d)))
            {
                result = false;
            }
            return result;
        }

        protected bool isValidPercentage(string Value)
        {
            string DecimalPattern = @"^\d{0,3}(\.\d{0,3})?$";

            Regex strPassword = new Regex(DecimalPattern);

            return (strPassword.IsMatch(Value.Trim()));

        }

        protected bool isValidTwoDecimalPercentage(string Value)
        {
            string DecimalPattern = @"^\d{0,3}(\.\d{0,2})?$";

            Regex strPassword = new Regex(DecimalPattern);

            return (strPassword.IsMatch(Value.Trim()));

        }

        protected void LoadFullList()
        {
            List<SSimulationList> sSimulationList = new List<SSimulationList>();

            try
            {
                sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
            }

             /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "ddlCustomerID_SelectedIndexChanged", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }

            gvOuterList.DataSource = sSimulationList;
            gvOuterList.DataBind();

        }

        protected void ClearAll()
        {
            txtValidDate.Text = "";
            txtWeighLimit.Text = "";
            txtWeightIncrement.Text = "";

            //Carrier list must be taken from someother SP later.
            List<STariffMaster> sTariffMaster = new List<STariffMaster>();
            sTariffMaster = proxy.GetAllCarrierNames();

            //Just to fill default
            foreach (var s in sTariffMaster)
            {
                PublicDiscount p = new PublicDiscount();
                p.CarrierName = s.CarrierName;
                p.FuelDiscount = "0";
                p.SafetyDiscount = "0";
                publicDiscountDB.Add(p);
            }

            gvCustomerDiscount.DataSource = sTariffMaster;
            gvCustomerDiscount.DataBind();
            gvCalcualtion.DataSource = null;
            gvCalcualtion.DataBind();

            gvOuterList.DataSource = null;
            gvOuterList.DataBind();

            gvGrossTotal.DataSource = null;
            gvGrossTotal.DataBind();

            //ddlSimulationID.SelectedValue = " ";
            ddlSimulationID.Items.Clear();

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            String strData = "";
            string limiter = ";";
            StringWriter stringWriter = new StringWriter();
            string FileName = ddlCustomerID.SelectedItem.Text.Trim() + "_Simulation.csv";

            #region Get List for the customer

            List<SSimulationList> sSimulationList = new List<SSimulationList>();

            try
            {
                sSimulationList = proxy.GetSimulationID(ddlCustomerID.SelectedValue);
            }

             /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnExport_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmGrossMarginCalculation.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "SMToolCalculationRefresh").ToString();
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "SMToolSimulationID").ToString(), ddlCustomerID.SelectedValue.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }

            #endregion

            foreach (SSimulationList i in sSimulationList)
            {
                //gridDetail = LoadSimulationGridList(s.DataField);

                #region Grid Details

                List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
                sSimulationTariff = proxy.GetSimulationTariff(i.DataField);
                List<GridDetail> gridDetail = new List<GridDetail>();

                foreach (var s in sSimulationTariff)
                {
                    GridDetail g = new GridDetail();


                    g.gvADV = s.ADV.ToString();
                    g.gvAverageWeight = s.AverageWeight.ToString();
                    g.gvCarrier = s.PurchaseCarrier;
                    g.gvComparisonMargin = (float)s.ComparisonMargin;
                    g.gvComparisonSales = (float)s.ComparisonSaleTariff;
                    g.gvCurrentSaleTariff = (float)s.PurchaseTariff;
                    g.gvCurrentTurnOver = (float)s.PublicTurnOver;
                    g.gvDiscount = (float)s.PublicDiscount;
                    g.gvFreightPurchase = (float)s.PurchaseFreight;
                    g.gvGrossMargin = (float)s.SaleMargin;
                    g.gvMasterServiceName = s.MasterServiceName;
                    g.gvOriginCountry = s.ShipCountry;
                    g.gvProposedSalesTariff = s.SaleTariff;
                    g.gvPublicCarrier = s.PublicCarrier;
                    g.gvPublicSurcharge = s.PublicSurcharge;
                    g.gvPublicTariff = s.PublicTariff;
                    g.gvPurchaseTariff = s.PurchaseTariff;
                    g.gvSalesFretTariff = s.SaleFreight;
                    g.gvSalesGrossMargin = s.SaleGrossMargin;
                    g.gvSalesTurnOver = s.SaleTurnOver;
                    g.gvDestinationCountry = s.DeliveryCountry;
                    g.gvWeight = s.WeightRange;
                    gridDetail.Add(g);
                }

                #endregion

                foreach (GridDetail g in gridDetail)
                {
                    List<string> LDetails = new List<string>();
                    LDetails.Add(" " + g.gvWeight.Replace(".", ",")); // [02APRKM] [BugID:1329]
                    LDetails.Add(g.gvOriginCountry);
                    LDetails.Add(g.gvDestinationCountry);
                    LDetails.Add(g.gvMasterServiceName);
                    LDetails.Add(g.gvCarrier);
                    LDetails.Add(g.gvSalesFretTariff.ToString().Replace(".", ",")); // [02APRKM] [BugID:1329]
                    strData = string.Join(limiter, LDetails.ToArray());
                    stringWriter.WriteLine(strData);
                }
            }
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentType = "text/csv";
            Response.Write(stringWriter.ToString());
            Response.Flush();
            Response.Close();


        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                List<SSimulationTariff> sSimulationTariff = new List<SSimulationTariff>();
                SSimulationSubTotal sSimulationSubTotal = new SSimulationSubTotal();
                string ShipCountry = "";
                string DeliveryCountry = "";
                string MasterService = "";
                string strSimulationID = ddlSimulationID.SelectedValue.Trim();
                string strTariffAssigned = "";

                strTariffAssigned = ddlDefaultTariff.SelectedValue.Trim();
                int i = 0;
                int iResult = 0;

                foreach (GridViewRow gr in gvCalcualtion.Rows)
                {
                    SSimulationTariff s = new SSimulationTariff();
                    if (i == 0)
                    {
                        ShipCountry = ((DropDownList)gr.Cells[1].FindControl("ddlShippingCountry")).SelectedValue;
                        DeliveryCountry = ((DropDownList)gr.Cells[2].FindControl("ddlDeliveryCountry")).SelectedValue;
                        MasterService = ((DropDownList)gr.Cells[3].FindControl("ddlMasterServiceName")).SelectedValue;
                        i++;
                    }
                    s.AccountNo = ddlCustomerID.SelectedValue;
                    s.SimulationID = strSimulationID;
                    s.WeightRange = ((Label)gr.Cells[0].FindControl("lblWeightRangeCol1")).Text;
                    s.ShipCountry = ShipCountry;
                    s.DeliveryCountry = DeliveryCountry;
                    s.MasterServiceName = MasterService;
                    s.AverageWeight = (float)Convert.ToDouble(((TextBox)gr.Cells[4].FindControl("txtAverageWeight")).Text);
                    s.ADV = 0;
                    s.PurchaseFreight = 0;
                    s.PurchaseTariff = 0;
                    s.PurchaseCarrier = ((Label)gr.Cells[8].FindControl("lblCarriers")).Text;
                    s.PublicDiscount = 0;
                    //s.PublicCarrier = "";
                    s.PublicCarrier = strTariffAssigned;        //Used to take assigned tariff Public1/Public2
                    s.PublicFreight = 0;
                    s.PublicTariff = 0;
                    s.PublicTurnOver = 0;
                    s.SaleMargin = (float)Convert.ToDouble(((TextBox)gr.Cells[14].FindControl("txtGrossMarign")).Text);
                    s.SaleFreight = 0;
                    s.SaleTariff = 0;
                    s.SaleTurnOver = 0;
                    s.SaleGrossMargin = 0;
                    s.ComparisonSaleTariff = 0;
                    s.ComparisonMargin = 0;
                    s.PublicSurcharge = "";
                    s.ContainerType = "Parcel";

                    sSimulationTariff.Add(s);
                }

                if (i == 0)
                {
                    SSimulationTariff s = new SSimulationTariff();

                    s.AccountNo = ddlCustomerID.SelectedValue;
                    s.SimulationID = strSimulationID;
                    s.WeightRange = "0-0";
                    s.ShipCountry = ShipCountry;
                    s.DeliveryCountry = DeliveryCountry;
                    s.MasterServiceName = MasterService;
                    s.AverageWeight = 0;
                    s.ADV = 0;
                    s.PurchaseFreight = 0;
                    s.PurchaseTariff = 0;
                    s.PurchaseCarrier = "";
                    s.PublicDiscount = 0;
                    //s.PublicCarrier = "";
                    s.PublicCarrier = strTariffAssigned;        //Used to take assigned tariff Public1/Public2
                    s.PublicFreight = 0;
                    s.PublicTariff = 0;
                    s.PublicTurnOver = 0;
                    s.SaleMargin = 0;
                    s.SaleFreight = 0;
                    s.SaleTariff = 0;
                    s.SaleTurnOver = 0;
                    s.SaleGrossMargin = 0;
                    s.ComparisonSaleTariff = 0;
                    s.ComparisonMargin = 0;
                    s.PublicSurcharge = "";
                    s.ContainerType = "";

                    sSimulationTariff.Add(s);
                }

                iResult = proxy.SimulationAssign(sSimulationTariff);

                string strBody = "";
                string strSubject = "";
                string strUrl = "";

                SUser UserDetails = new SUser();
                UserDetails = proxy.GetUserDetailForAssignTariff(ddlCustomerID.SelectedValue);

                // Self created users
                if (iResult == 1)
                {
                    strSubject = GetGlobalResourceObject("LocalString", "AssignTariffMailSubject_Self").ToString();
                    strBody = string.Format(GetGlobalResourceObject("LocalString", "AssignTariffMailBody_Self").ToString(), UserDetails.Name);

                    SKeyValue sKeyvalue = new SKeyValue();
                    sKeyvalue = proxy.GetValueFromParameter("ACTIVATE_URL");
                    strUrl = sKeyvalue.Value.Trim() + "?UserID=" + proxy.EncryptString(ddlCustomerID.SelectedItem.Text, "Password");
                    strBody += "<BR><BR>" + strUrl;

                    proxy.sendMail(ddlCustomerID.SelectedItem.Text, KaizosSession.Current.UserId.Trim(), strBody, strSubject);
                }

                // Admin created Users
                if (iResult == 2)
                {
                    strSubject = GetGlobalResourceObject("LocalString", "AssignTariffMailSubject").ToString();
                    strBody = string.Format(GetGlobalResourceObject("LocalString", "AssignTariffMailBody").ToString(), UserDetails.Name, UserDetails.TelephoneNo.Trim(), UserDetails.CreatedBy.Trim());

                    SKeyValue sKeyvalue = new SKeyValue();
                    sKeyvalue = proxy.GetValueFromParameter("ACTIVATE_URL");
                    strUrl = sKeyvalue.Value.Trim() + "?UserID=" + proxy.EncryptString(ddlCustomerID.SelectedItem.Text, "Password");
                    strBody += "<BR><BR>" + strUrl;

                    proxy.sendMail(ddlCustomerID.SelectedItem.Text, KaizosSession.Current.UserId.Trim(), strBody, strSubject);
                }

            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    //logger.Debug(errorMessage);

                    //KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShippingError").ToString();
                    //Server.Transfer("frmResult.aspx", false);
                }
            }

        }

    }
}