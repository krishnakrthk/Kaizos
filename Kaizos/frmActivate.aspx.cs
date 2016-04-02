using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmActivate : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmActivate));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmActivate").ToString();
            }
            try
            {
                string UserId = "";
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                SUser sUser = new SUser();
                UserId = proxy.DecryptString(Request.QueryString["UserID"].Trim(), "Password");
                ViewState["UserId"] = UserId.Trim();
                sUser = proxy.GetLogin(UserId.Trim());

                if (sUser == null)
                {
                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserNotExist").ToString(), UserId.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(ViewState["UserId"].ToString().Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), ViewState["UserId"].ToString().Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file*/
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(ViewState["UserId"].ToString().Trim(), ErrorSource.Client, "Page_Load()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), ViewState["UserId"].ToString().Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }
        
        protected void val_ActivatePasword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = true;
                string strError = "";
                KaizosServiceContractClient context = new KaizosServiceContractClient();
                if (txtNewPassword.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblNewPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (txtNewPassword.Text.Trim().Length < 8 || txtNewPassword.Text.Trim().Length > 15)
                {
                    strError = strError + "*" + lblNewPassword.Text.Trim() + " " + valLength.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (context.ValidatePassword(txtNewPassword.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblNewPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (!(args.IsValid))
                {
                    val_ActivatePasword.ErrorMessage = strError;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(ViewState["UserId"].ToString().Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), ViewState["UserId"].ToString().Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file*/
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(ViewState["UserId"].ToString().Trim(), ErrorSource.Client, "val_ActivatePasword_ServerValidate()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), ViewState["UserId"].ToString().Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnActivatePassword_Click(object sender, EventArgs e)
        {
            string strUserId = ViewState["UserId"].ToString().Trim();
            int ConfirmStatus = 1;
            if (IsValid)
            {
                try
                {
                    KaizosServiceContractClient context = new KaizosServiceContractClient();
                    SUser sUser = context.GetLogin(strUserId.Trim());
                    SKeyValue sKeyValue = context.GetValueFromParameter("CUSTOMER_SUPPORT_MAIL_ID");

                    ConfirmStatus = context.ConfirmPassword(sUser.AccountNo.Trim(), context.EncryptString(txtNewPassword.Text.Trim(), "Password"));

                    if (ConfirmStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ConfirmPWDSuccess").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (ConfirmStatus == 1)
                    {
                        KaizosSession.Current.ReturnURL = "frmActivate.aspx?UserID=" + context.EncryptString(strUserId.Trim(), "Password");
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (ConfirmStatus == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserNotExist").ToString(), strUserId.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file*/
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(strUserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), strUserId.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file*/
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(strUserId.Trim(), ErrorSource.Client, "btnActivatePassword_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), strUserId.Trim());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }

            }
        }
    }
}