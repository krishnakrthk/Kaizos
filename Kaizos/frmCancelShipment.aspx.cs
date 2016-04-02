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
    public partial class frmCancelShipmentaspx : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));
        KaizosServiceAgent proxy = new KaizosServiceAgent();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCancelShipment").ToString();
                errorMsg.Attributes["style"] = "display: none;";
            }

        }

        protected void val_Shipment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";

            if (txtShipmentReference.Text.Equals(""))
            {
                strError = strError + "*" + lblShipRef.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else
            {
                SShipmentOrder sShipmentOrder = new SShipmentOrder();
                sShipmentOrder = proxy.GetOrderInformation(txtShipmentReference.Text.Trim());
                if (sShipmentOrder.ShipReference.Equals(""))
                {
                    strError = strError + "*" + string.Format(GetGlobalResourceObject("LocalString", "ReferenceNotAvailable").ToString(), txtShipmentReference.Text.Trim()) + "<br>";
                    args.IsValid = false;
                }
            }
            if (!(args.IsValid))
            {
                val_Shipment.ErrorMessage = strError;
                errorMsg.Attributes["style"] = "display: block;";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                int iResult = 1;

                try
                {
                    iResult = proxy.CancelShipment(txtShipmentReference.Text.Trim());
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmCancelShipmentaspx.aspx";
                    //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CancelFailure").ToString();
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CancelShipment").ToString(), txtShipmentReference.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnCancel_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmCancelShipmentaspx.aspx";
                        //KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "LoginFailure").ToString();
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CancelShipment").ToString(), txtShipmentReference.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                if (iResult == 0)
                {


                }
            }
            //Response.Write("Yes Pressed");
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {

        }
    }
}