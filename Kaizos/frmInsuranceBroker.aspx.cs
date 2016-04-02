using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmInsuranceBroker : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmLogin));
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmInsuranceBroker").ToString();
                    KaizosServiceAgent proxy = new KaizosServiceAgent();
                    txtEmail.Text = proxy.getBrokerEmailId();
                }
            }
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

               // KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "BrokerEmailFailed").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                  //  KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "BrokerEmailFailed").ToString());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    int result = 0;
                    KaizosServiceAgent proxy = new KaizosServiceAgent();
                    result = proxy.UpdateBrokerEmailId(txtEmail.Text);
                   
                    if (result == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "UpdationEmailSucess").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "UpdationEmailFailed").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }

                }
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "UpdationEmailFailed").ToString());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSubmit_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "UpdationEmailFailed").ToString());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void val_Broker_EmailID(object source, ServerValidateEventArgs args)
        {
             args.IsValid = true;
            string strError = "";
            KaizosServiceContractClient context = new KaizosServiceContractClient();
            try
            {
                if (txtEmail.Text.Trim().Equals(""))
                {
                    strError = strError + "*" + lblInsurancebrokeremailaddress.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (txtEmail.Text.Trim().Length != 0)
                {
                    if (context.ValidateEmail(txtEmail.Text.Trim()) != 0)
                    {
                        strError = strError + "*" + lblInsurancebrokeremailaddress.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }
                if (!(args.IsValid))
                {
                    val_BrokerEmailId.ErrorMessage = strError;
                }
            }
             catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "BrokerEmailValidaterFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_AddressBook_ServerValidate", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookValidationFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
        }
    }
}