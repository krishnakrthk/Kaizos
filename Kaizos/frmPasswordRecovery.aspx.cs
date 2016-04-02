using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using KaizosServiceInvokers.KaizosServiceReference;
using System.Security.Cryptography;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Channels;
using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmPasswordRecovery : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmPasswordRecovery));

        protected void btnPasswordRecovery_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                 KaizosServiceContractClient context = new KaizosServiceContractClient();
                 KaizosSession.Current.PWDEmail = txtEmail.Text.Trim();
                 SKeyValue sKeyValue = context.GetValueFromParameter("CUSTOMER_SUPPORT_MAIL_ID");

                 if (!context.sendConfirmPassord(txtEmail.Text.Trim(), sKeyValue.Value.Trim()))
                 {
                     KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                     KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                     Server.Transfer("frmResult.aspx", false);
                 }
                 else
                 { 
                     KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                     KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MailSendSuccess").ToString();
                     Server.Transfer("frmResult.aspx", false);
                 }
            }
        }

        protected void valPWRecovery_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                if (txtEmail.Text.Trim().Length == 0)
                {
                    //valPWRecovery.ErrorMessage = "User ID must be filled";
                    valPWRecovery.ErrorMessage = lblEmail.Text.Trim() + " * " + valEmpty.Text.Trim();
                    args.IsValid = false;
                    return;
                }
                else if (proxy.ValidateEmail(txtEmail.Text.Trim()) != 0)
                {
                    //valPWRecovery.ErrorMessage = "Please enter valid EMail address";
                    valPWRecovery.ErrorMessage = lblEmail.Text.Trim() + " * " + valInvalid.Text.Trim();
                    args.IsValid = false;
                    return;
                
                }
                else
                {
                    SUser sUser = new SUser();
                    sUser = proxy.GetLogin(txtEmail.Text.Trim());
                    if (sUser.Email.Trim() == "NO USER")
                    {
                        valPWRecovery.ErrorMessage = "User ID is not exists in registered list";
                        args.IsValid = false;
                        return;
                    }
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file*/
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Client, "valPWRecovery_ServerValidate1()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmPasswordRecovery.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ConfirmPWDFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Page.Title = GetGlobalResourceObject("LocalString", "frmPasswordRecovery").ToString();
        }
    }

}