using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;

using log4net;
using log4net.Config;
using System.Web.Security;

namespace Kaizos
{
    public partial class frmConfirmPassword : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmConfirmPassword));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmConfirmPassword").ToString();            
            }
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                if (!IsPostBack)
                    ViewState["UserId"] = proxy.DecryptString(Request.QueryString["UserID"].Trim(), "Password"); ;
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

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), ViewState["UserId"].ToString().Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int ConfirmStatus = 1;
            string strUserId = ViewState["UserId"].ToString().Trim();
            if (IsValid)
            {
                try
                {
                    KaizosServiceContractClient context = new KaizosServiceContractClient();
                    SUser sUser = context.GetLogin(strUserId.Trim());
                    SKeyValue sKeyValue = context.GetValueFromParameter("CUSTOMER_SUPPORT_MAIL_ID");
                    
                    ConfirmStatus = context.ConfirmPassword(sUser.AccountNo.Trim(), context.EncryptString(txtConfirmPassword.Text.Trim(), "Password"));

                    if (ConfirmStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ConfirmPWDSuccess").ToString();
                        //if (!context.sendMessage(strUserId.Trim(), txtConfirmPassword.Text.Trim().Trim(), sKeyValue.Value.Trim()))
                        //{
                        //    KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                        //    KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + "-" + GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                        //}
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (ConfirmStatus == 1)
                    {
                        KaizosSession.Current.ReturnURL = "frmConfirmPassword.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (ConfirmStatus == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDNotExists").ToString(), sUser.AccountNo.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [05JAN12RM] */
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
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(strUserId.Trim(), ErrorSource.Client, "Button1_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), strUserId.Trim());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void val_ConfirmPage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";

            KaizosServiceContractClient context = new KaizosServiceContractClient();
            if (txtPassword.Text.Trim().Length == 0)
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (txtPassword.Text.Trim().Length < 8 || txtPassword.Text.Trim().Length > 15)
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valLength.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            if (txtConfirmPassword.Text.Trim().Length == 0)
            {
                strError = strError + "*" + lblConfirmPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valShouldSame.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (context.ValidatePassword(txtConfirmPassword.Text.Trim()) != 0)
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_ConfirmPasword.ErrorMessage = strError;
            }

        }
    }
}