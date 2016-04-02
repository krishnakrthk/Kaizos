using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmShipmentConfirmation : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));
        KaizosServiceAgent proxy = new KaizosServiceAgent();
        List<string> strClosedReference = new List<string>();
        List<SShipmentOrder> sShipmentOrder = new List<SShipmentOrder>();
        float fTotalAmount = 0;
        int intGRID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
           if (!IsPostBack)
               Page.Title = GetGlobalResourceObject("LocalString", "frmShipmentConfirmation").ToString();
            try
            {
                List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
                sShipmentOrder = proxy.GetShipmentDetails(KaizosSession.Current.GroupID, KaizosSession.Current.SessionID, "DRAFT", KaizosSession.Current.AccountNo);
                //sShipmentOrder = proxy.GetShipmentDetails(14, "zt3c5ddg0hoo3o0m5xyypbv3", "DRAFT", "123");
                
                for (int i = 0; i < sShipmentOrder.Count; i++)
                {
                    strClosedReference.Add(sShipmentOrder[i].ShipReference);
                    fTotalAmount = fTotalAmount + sShipmentOrder[i].TotalAmount;
                }
              
                gvShipments.DataSource = sShipmentOrder;
                gvShipments.DataBind();


            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);

                    KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ShipmentConfirmationError").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (strClosedReference.Count > 0)
            {
                try
                {
                    SPaymentInfo sPaymentInfo = new SPaymentInfo();
                    bool bIsPaymentReady = false;
                    bool bIsShipmentCompleted = true;
                    string ShipmentError = "";
                    string ShipmentSuccess = "";
                    //sPaymentInfo = proxy.GetPaymentMethodAndInfo("123", "");
                    sPaymentInfo = proxy.GetPaymentMethodAndInfo(KaizosSession.Current.AccountNo, KaizosSession.Current.UserType);

                    if ((sPaymentInfo.PaymentMethod.Equals("DP")) && (sPaymentInfo.AvailableCreditLimit > fTotalAmount))
                    {
                        //int result = proxy.DeferredPayment(strClosedReference,"123", fTotalAmount);
                        bIsPaymentReady = true;
                    }
                    else if ((sPaymentInfo.PaymentMethod.Equals("DP")) && (sPaymentInfo.AvailableCreditLimit < fTotalAmount))
                    {
                        KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ShippingInsufficientFund").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (sPaymentInfo.PaymentMethod.Equals("CC"))
                    {
                        KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ShippingCreditCardPayment").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (sPaymentInfo.PaymentMethod.Equals(""))
                    {
                        KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ShippingInvalidUser").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }

                    List<SShipmentOrder> lShipmentOrder = new List<SShipmentOrder>();
                    List<SShipmentResult> lShipmenResult = new List<SShipmentResult>();

                    if (bIsPaymentReady)
                    {
                        for (int i = 0; i < sShipmentOrder.Count; i++)
                        {
                            SShipmentOrder Order = new SShipmentOrder();
                            SShipmentResult ErrorResult = new SShipmentResult();
                            ErrorResult.isLabelGenerated = SEnumFlag.Yes;
                            ErrorResult.LabelError = "";
                            Order = proxy.CarrierProcessing(sShipmentOrder[i], SEnumCarrierProcess.Label);
                            if (Order.ShipmentResult.isLabelGenerated == SEnumFlag.No)
                            {
                                //bIsShipmentCompleted = false;
                                ErrorResult.isLabelGenerated = SEnumFlag.No;
                                ErrorResult.LabelError = Order.ShipReference + " : " + Order.ShipmentResult.LabelError;
                                //ShipmentError = ShipmentError + Order.ShipReference + " : " + Order.ShipmentResult.LabelError + "<BR>";
                            }
                            lShipmenResult.Add(ErrorResult);
                            lShipmentOrder.Add(Order);
                        }
                    }


                    //1. Update Shipment Details
                    for (int i = 0; i < lShipmentOrder.Count; i++)
                    {
                        if (lShipmenResult[i].isLabelGenerated == SEnumFlag.No)
                        {
                            ShipmentError = ShipmentError + lShipmenResult[i].LabelError + "<br>";
                            bIsShipmentCompleted = false;
                        }
                        else
                        {
                            List<SShipmentDetails> shipDetail = new List<SShipmentDetails>();
                            shipDetail = lShipmentOrder[i].ShipDetail.ToList();
                            for (int j = 0; j < shipDetail.Count; j++)
                            {
                                proxy.ShipmentDetailUpdate(lShipmentOrder[i].ShipReference, shipDetail[j].TrackingNo, j.ToString());
                            }
                            ShipmentSuccess = ShipmentSuccess + lShipmentOrder[i].ShipReference + ", ";
                            SendMail();
                        }

                    }

                    //2. Payment update
                    int result = proxy.DeferredPayment(strClosedReference, KaizosSession.Current.AccountNo, fTotalAmount);
                    if (!(bIsShipmentCompleted))
                    {
                        ShipmentError = GetGlobalResourceObject("LocalString", "ShipmentFailure").ToString() + "<br>" + ShipmentError;
                    }

                    if (ShipmentSuccess.Trim().Length > 2)
                    {
                        ShipmentSuccess = GetGlobalResourceObject("LocalString", "ShipmentSucess").ToString() + ShipmentSuccess.Trim().Substring(0, ShipmentSuccess.Trim().Length - 1);
                    }

                    KaizosSession.Current.ErrorMessage = ShipmentError + "<br>" + ShipmentSuccess;
                    KaizosSession.Current.ReturnURL = "frmcarrierLabelDisplay.aspx";

                    Server.Transfer("frmResult.aspx", false);

                }
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ShipmentFailure").ToString(), KaizosSession.Current.UserId);
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnSend_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmShipmentConfirmation.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ShipmentFailure").ToString(), KaizosSession.Current.UserId);
                        Server.Transfer("frmResult.aspx", false);

                    }
                }


            }
        }
        

        protected void gvShipments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            e.Row.ID = "sampleGridRow_" + intGRID.ToString();
            intGRID += 1 ;
            string strWeightUnit = ""; // 10MAY12KM
            string strCurrencyType = ""; // 28MAY12KM

            string strShipRference  = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
                strShipRference = gvShipments.DataKeys[e.Row.RowIndex].Value.ToString();
            
            GridView grd2 = new GridView();

            grd2 = (GridView)e.Row.FindControl("gvShipmentDetail");
            List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();

            for (int i = 0; i < sShipmentOrder.Count; i++)
            {
                if (strShipRference.Equals(sShipmentOrder[i].ShipReference))
                {
                    strWeightUnit = sShipmentOrder[i].ShipDetail[0].WeightUnit; // 10MAY12KM
                    strCurrencyType = sShipmentOrder[i].CurrencyType; // 28MAY12KM
                    grd2.DataSource = sShipmentOrder[i].ShipDetail;
                    grd2.DataBind();
                }
            }
            /*** [10MAY12KM] ***/
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblText = e.Row.FindControl("lblWeightUnit1") as Label;
                lblText.Text = strWeightUnit;
                Label lblText1 = e.Row.FindControl("lblWeightUnit2") as Label;
                lblText1.Text = strWeightUnit;
                Label lblcur = e.Row.FindControl("lblCurrency") as Label;
                lblcur.Text = strCurrencyType;
            }
            
        }

        protected void gvShipments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strShipReference = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "Delete1") // if "paramenter" link is clicked
            {
                int result = proxy.DeleteShipment(strShipReference);

                //sShipmentOrder = proxy.GetShipmentDetails(14, "zt3c5ddg0hoo3o0m5xyypbv3", "DRAFT", "123");
                sShipmentOrder = proxy.GetShipmentDetails(KaizosSession.Current.GroupID,KaizosSession.Current.SessionID,"DRAFT",KaizosSession.Current.AccountNo);

                for (int i = 0; i < sShipmentOrder.Count; i++)
                {
                    strClosedReference.Add(sShipmentOrder[i].ShipReference);
                }

                gvShipments.DataSource = sShipmentOrder;
                gvShipments.DataBind();
            }

            if (e.CommandName == "Edit1") // if "paramenter" link is clicked
            {
                KaizosSession.Current.ShipmentOrder = proxy.GetOrderInformation(strShipReference);
                Server.Transfer("frmManualShipping.aspx?Flag=Edit");
            }

        }

        protected void btnAddShipment_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTariffDelayInterrogation.aspx");
        }

        protected string MyFormat(string value)
        {
            //string  d = 
            return Convert.ToDateTime(value).ToString("dd/MM/yyyy");
        }

        protected string MyFormatDecimal(string input)
        {
            return Convert.ToDouble(input).ToString("#0.00");
        }

        protected string MyName(string value)
        {
            return value.Replace("-", " ");
        }

        protected void SendMail()
        {
            for (int i = 0; i < sShipmentOrder.Count; i++)
            {
                string strSenderText = "";
                string strReceiverText = "";

                string strBodyText = "";
                string strSenderSubject = "";
                string strReceiverSubject = "";
                string strBody = "";

                string colWidthRight = "style=\"width:400x; font-family: Arial, Helvetica, sans-serif; font-size: 10px;\" align=\"right\" valign=\"top\"> ";
                string colWidth = "style=\"width:400px; font-family: Arial, Helvetica, sans-serif; font-size: 10px;\" > <b>";

                string DetColHeader = "style=\"width:160px; background-color: #C0C0C0;font-family: Arial, Helvetica, sans-serif; font-size: 10px;\" align=\"right\" valign=\"top\"> ";
                string DetColWidth = "style=\"width:160px;font-family: Arial, Helvetica, sans-serif; font-size: 10px;\" > <b>";

                strSenderText += GetGlobalResourceObject("LocalString", "ShippingSenderMailLine1").ToString() + "<BR><BR>";
                strSenderText += GetGlobalResourceObject("LocalString", "ShippingSenderMailLine2").ToString() + "<BR><br>";
                strSenderSubject = GetGlobalResourceObject("LocalString", "ShippingReceiverMailSubject").ToString();
                
                strReceiverText += GetGlobalResourceObject("LocalString", "ShippingSenderMailLine1").ToString() + "<BR><BR>";

                strReceiverText += string.Format(GetGlobalResourceObject("LocalString", "ShippingReceiverMailLine2").ToString(), MyName(sShipmentOrder[i].SenderName))
                    + "<BR><br>";
                strReceiverSubject = GetGlobalResourceObject("LocalString", "ShippingSenderMailSubject").ToString();
                
                if ((sShipmentOrder[i].SenderNotification.Equals(SEnumFlag.Yes)) ||
                        (sShipmentOrder[i].RecipientNotification.Equals(SEnumFlag.Yes)))
                {

                    strBodyText += "<table> ";
                    strBodyText += "<tr><td>" + GetGlobalResourceObject("LocalString", "Service").ToString() + "</td>";
                    strBodyText += "<td>" + sShipmentOrder[i].Carrier + "</td> </tr> ";
                    strBodyText += "<tr><td>" + GetGlobalResourceObject("LocalString", "WishedShippingDate").ToString() + "</td>";
                    strBodyText += "<td>" + MyFormat(sShipmentOrder[i].WishedShipDate.ToString()) + "</td> </tr> </table>";
                    strBodyText += "<BR><br>";
                    strBodyText += "<table border=1 style=\"border: .5px inset #000000; border-spacing: 0px;\">";
                    strBodyText += "<tr><td" + colWidthRight + GetGlobalResourceObject("LocalString", "ShippingAddress").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + sShipmentOrder[i].SenderCompany;
                    strBodyText += "<BR>" + MyName(sShipmentOrder[i].SenderName);
                    strBodyText += "<BR>" + sShipmentOrder[i].SenderAddress1;
                    strBodyText += "<BR>" + sShipmentOrder[i].SenderAddress2;
                    strBodyText += "<BR>" + sShipmentOrder[i].SenderAddress3;
                    strBodyText += "<BR>" + sShipmentOrder[i].SenderZipCode + "   " + sShipmentOrder[i].SenderCity;
                    strBodyText += "</td>";
                    strBodyText += "<td" + colWidthRight + GetGlobalResourceObject("LocalString", "DeliveryAddress").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + sShipmentOrder[i].RecipientCompany;
                    strBodyText += "<BR>" + MyName(sShipmentOrder[i].RecipientName);
                    strBodyText += "<BR>" + sShipmentOrder[i].RecipientAddress1;
                    strBodyText += "<BR>" + sShipmentOrder[i].RecipientAddress2;
                    strBodyText += "<BR>" + sShipmentOrder[i].RecipientAddress3;
                    strBodyText += "<BR>" + sShipmentOrder[i].RecipientZipCode + "   " + sShipmentOrder[i].RecipientCity;
                    strBodyText += "</td> </tr>";

                    strBodyText += "<tr><td" + colWidthRight + GetGlobalResourceObject("LocalString", "IndicativeDeliveryDate").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + MyFormat(sShipmentOrder[i].CalculatedShipDate.ToString()) + "  " + sShipmentOrder[i].CalculatedDeliveryTime + "</td>";
                    strBodyText += "<td" + colWidthRight + GetGlobalResourceObject("LocalString", "TotalWeight").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + sShipmentOrder[i].TotalWeight.ToString();
                    strBodyText += "</td> </tr>";

                    strBodyText += "<tr><td" + colWidthRight + GetGlobalResourceObject("LocalString", "TotalParcel").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + sShipmentOrder[i].UODCount.ToString() + "</td>";
                    strBodyText += "<td" + colWidthRight + GetGlobalResourceObject("LocalString", "TotalTax").ToString() + "</td>";
                    strBodyText += "<td" + colWidth + MyFormatDecimal(sShipmentOrder[i].TaxableWeight.ToString());
                    strBodyText += "</td> </tr>";
                    strBodyText += "</table>";
                    strBodyText += "<BR><br>";

                    List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
                    sShipmentDetails = sShipmentOrder[i].ShipDetail.ToList();
                    strBodyText += "<table border=1 style=\"border: .5px inset #000000; border-spacing: 0px;\">";
                    strBodyText += "<tr><td" + DetColHeader + GetGlobalResourceObject("LocalString", "ParcelNumber").ToString() + "</td>";
                    strBodyText += "<td" + DetColHeader + GetGlobalResourceObject("LocalString", "ContentType").ToString() + "</td>";
                    strBodyText += "<td" + DetColHeader + GetGlobalResourceObject("LocalString", "Container").ToString() + "</td>";
                    strBodyText += "<td" + DetColHeader + GetGlobalResourceObject("LocalString", "Dimension").ToString() + "</td>";
                    strBodyText += "<td" + DetColHeader + GetGlobalResourceObject("LocalString", "ContainerGrossWeight").ToString() + "</td>";
                    strBodyText += "<td" + DetColHeader + GetGlobalResourceObject("LocalString", "TaxableWeight").ToString() + "</td></tr>";

                    for (int j = 0; j < sShipmentDetails.Count; j++)
                    {


                        strBodyText += "<tr><td" + DetColWidth + sShipmentDetails[j].ParcelNo + "</td>";
                        strBodyText += "<td" + DetColWidth + sShipmentDetails[j].ContentType + "</td>";
                        strBodyText += "<td" + DetColWidth + sShipmentDetails[j].Container + "</td>";
                        strBodyText += "<td" + DetColWidth + sShipmentDetails[j].Length.ToString() + " * "
                            + sShipmentDetails[j].Width.ToString()
                            + " * " + sShipmentDetails[j].Height.ToString()
                            + " * " + sShipmentDetails[j].DimensionUnit.ToString() + "</td>";
                        strBodyText += "<td" + DetColWidth + sShipmentDetails[j].Weight.ToString() + "  "
                            + sShipmentDetails[j].WeightUnit + "</td>";
                        strBodyText += "<td" + DetColWidth + MyFormatDecimal(sShipmentDetails[j].TaxableWeight.ToString()) + "</td></tr>";
                    }
                    strBodyText += "</table>";


                }

                if (sShipmentOrder[i].SenderNotification.Equals(SEnumFlag.Yes))
                {
                    strBody = strSenderText + strBodyText;
                    proxy.sendMail(sShipmentOrder[i].SenderEmail, KaizosSession.Current.UserId.Trim(), strBody, strSenderSubject);
                }



                if (sShipmentOrder[i].RecipientNotification.Equals(SEnumFlag.Yes))
                {
                    strBody = strReceiverText + strBodyText;
                    proxy.sendMail(sShipmentOrder[i].RecipientEmail, KaizosSession.Current.UserId.Trim(), strBody, strReceiverSubject);
                }
            }

        }

    }
}