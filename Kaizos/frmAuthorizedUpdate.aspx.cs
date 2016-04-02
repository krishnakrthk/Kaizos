using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using KaizosServiceInvokers.KaizosServiceReference;
using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmAuthorizedUpdate : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAuthorizedUpdate));
        public string struserId = "", strAccount_no = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAuthorizedUpdate").ToString();

                if (KaizosSession.Current.UserType == "RF")
                {
                    txtCompanyName.Text = KaizosSession.Current.AZName;
                    txtEmail.Text = KaizosSession.Current.AZEmail;
                    txtPhoneNo.Text = KaizosSession.Current.AZPhoneNo;
                    strAccount_no = KaizosSession.Current.AZAccountNo;

                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void val_AZU_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            try
            {
                KaizosServiceContractClient context = new KaizosServiceContractClient();
                if (txtCompanyName.Text.Equals(""))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!context.isAlphaNumericValidation(txtCompanyName.Text.Trim()))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;                
                }

                if (txtEmail.Text.Trim().Length != 0)
                {
                    if (context.ValidateEmail(txtEmail.Text.Trim()) != 0)
                    {
                        strError = strError + "*" + lblEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtPhoneNo.Text.Equals(""))
                {
                    strError = strError + "*" + lblPhoneNo.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(context.isNumericValidation(txtPhoneNo.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblPhoneNo.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (!(args.IsValid))
                {
                    val_AZU.ErrorMessage = strError;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_AZU_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int Status = 1;
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient context = new KaizosServiceContractClient();
                    if (!chkDelete.Checked)
                    {
                        Status = context.UpdateAuthorize(KaizosSession.Current.AZAccountNo, txtCompanyName.Text, txtEmail.Text, txtPhoneNo.Text, KaizosSession.Current.AccountNo);
                        if (Status == 0)
                        {
                            KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmail.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                        else
                        {
                            KaizosSession.Current.ReturnURL = "frmAuthorizedUpdate.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }
                    else
                    {
                        Status = context.DeleteAuthorized(KaizosSession.Current.AZEmail.Trim());
                            
                        if (Status == 0)
                        {
                            KaizosSession.Current.ReturnURL = "frmAuthorizedUserList.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserDeleteSuccess").ToString(), txtEmail.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                        else
                        {
                            KaizosSession.Current.ReturnURL = "frmAuthorizedUpdate.aspx";
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserDeleteFailure").ToString(), txtEmail.Text.Trim());
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

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnUpdate_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }
   }
}