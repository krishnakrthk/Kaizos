using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;
namespace Kaizos
{
    public partial class frmCarrier : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCarrier));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (KaizosSession.Current.UserType == "AD")
                {
                    lblCarrierName.Text = lblCarrierName.Text + "*";
                    lblClaimResoulutionDelay.Text = lblClaimResoulutionDelay.Text + "*";

                    if (!Page.IsPostBack)
                    {
                        Page.Title = GetGlobalResourceObject("LocalString", "frmCarrier").ToString();
                        //string strCarrier = Request.QueryString["x"];
                        //string referencedCarrier = Request.QueryString["y"];
                        //string claimDelay = Request.QueryString["z"];
                        //string active = Request.QueryString["w"];
                        //string strCarrierID = Request.QueryString["u"];
                         string active = KaizosSession.Current.CMActive;
                         string strCarrierID= KaizosSession.Current.CMCarrieID;
                         string strCarrier = KaizosSession.Current.CMCarrierName;
                         string claimDelay= KaizosSession.Current.CMClaimDelay ;
                         string referencedCarrier = KaizosSession.Current.CMReferencedCarrier;



                        if (!(strCarrierID == null) && (!strCarrierID.Equals(string.Empty)))
                        {
                            hdnMode.Value = strCarrierID;
                            trActive.Visible = true;
                            chkActive.Visible = true;
                            if (active == "True")
                            {
                                chkActive.Checked = true;
                            }
                            KaizosSession.Current.CMActive = string.Empty;
                            KaizosSession.Current.CMCarrieID = string.Empty;
                            KaizosSession.Current.CMCarrierName = string.Empty;
                            KaizosSession.Current.CMClaimDelay = string.Empty;
                            KaizosSession.Current.CMReferencedCarrier = string.Empty;
                        }
                        else
                        {
                            hdnMode.Value = "0";
                            trActive.Visible = false;
                            chkActive.Visible = false;
                        }
                        if (!hdnMode.Value.Equals("0"))
                        {
                            hdnMode.Value = strCarrierID;
                            btnCreate.Text = GetGlobalResourceObject("LocalString", "Update").ToString();
                            lblModuleHeading.Text = GetGlobalResourceObject("LocalString", "CarrierUpdation").ToString();
                            txtCarrierName.Text = strCarrier;
                            //txtCarrierName.ReadOnly = true; 30MAR12SM bug id : 1239
                            txtClaimResolutionDelays.Text = claimDelay;
                            if (referencedCarrier == "True")
                            {
                                chkReferencedCarrier.Checked = true;
                            }

                        }



                    }
                }
            }
             catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    if (hdnMode.Value.Equals("0"))
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierCreateFailure").ToString(), txtCarrierName.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierUpdateFailure").ToString(), txtCarrierName.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }

                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if(IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    SCarrier sCarrier = new SCarrier();
                    sCarrier.CarrierName = txtCarrierName.Text; //30MAR12SM bug id: 1239
                    sCarrier.ClaimDelay = int.Parse(txtClaimResolutionDelays.Text);
                    //chkActive.checked
                    sCarrier.ReferencedCarrier = chkReferencedCarrier.Checked;

                    if (hdnMode.Value.Equals("0"))
                    {
                        sCarrier.Active = false;
                        sCarrier.CarrierName = txtCarrierName.Text;
                   
                        if (proxy.CreateCarrier(sCarrier) == 0)
                        {
                            KaizosSession.Current.ReturnURL = "frmCarrierList.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "CarrierCreationSuccess").ToString(), txtCarrierName.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }
                    else
                    {
                        sCarrier.CarrierID = int.Parse(hdnMode.Value);
                        sCarrier.Active = chkActive.Checked;
                        
                        if (proxy.UpdateCarrier(sCarrier) == 0)
                        {
                            KaizosSession.Current.ReturnURL = "frmCarrierList.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "CarrierUpdateSuccess").ToString(), txtCarrierName.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }
                }

            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);
                if (hdnMode.Value.Equals("0"))
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierCreateFailure").ToString(), txtCarrierName.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierUpdateFailure").ToString(), txtCarrierName.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    if (hdnMode.Value.Equals("0"))
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierCreateFailure").ToString(), txtCarrierName.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "CarrierUpdateFailure").ToString(), txtCarrierName.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }

                }
            }
        }

        protected void val_Carrier_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
            string strError = string.Empty;
            string strEmpty = string.Empty;
            KaizosServiceContractClient context = new KaizosServiceContractClient();
            string strCarrierName = txtCarrierName.Text.Trim();
            string sClaimResolutionDelay = txtClaimResolutionDelays.Text;
            try
            {
                if (strCarrierName.Equals(strEmpty))
                {
                    strError = strError + "*" + lblCarrierName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;

                }
                else if (!Regex.IsMatch(strCarrierName, @"^[a-zA-Z][a-zA-Z ]+$"))
                {
                    strError = strError + "*" + lblCarrierName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!(int.Parse(hdnMode.Value) >0))
                {
                    if (context.CheckForDuplicateCarrierName(strCarrierName))
                    {
                        strError = strError + "*" + lblCarrierName.Text.Trim() + " " + valAlready.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (!context.isNumericValidation(sClaimResolutionDelay, System.Globalization.NumberStyles.Integer))
                {
                    strError = strError + "*" + lblClaimResoulutionDelay.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!(int.Parse(sClaimResolutionDelay) > 0))
                {
                    strError = strError + "*" + lblClaimResoulutionDelay.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (!(args.IsValid))
                {
                    val_Carrier.ErrorMessage = strError;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CarrierValidationFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_Carrier_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CarrierValidationFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }
    }
}