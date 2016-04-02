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
    public partial class frmCustomerServiceUpdate : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCustomerServiceUpdate));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (KaizosSession.Current.UserType == "CS")
                {
                    if (!IsPostBack)
                    {
                        Page.Title = GetGlobalResourceObject("LocalString", "frmCustomerServiceUpdate").ToString();

                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        txtEmail.Text = KaizosSession.Current.UserId.Trim();

                        List<string> ComboText = proxy.GetLanguageList().ToList();
                        ddlLanguage.DataSource = ComboText;
                        ddlLanguage.DataBind();

                        ddlLanguage.SelectedValue = KaizosSession.Current.UserLanguage.Trim();
                        txtEmail.Enabled = false;
                    }
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int UpdateStatus = 1;
            string strPassword = "";
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    if (txtConfirmPassword.Text.Trim().Length != 0)
                        strPassword = proxy.EncryptString(txtConfirmPassword.Text.Trim(), "Password");

                    UpdateStatus = proxy.CustomerServiceUpdate(KaizosSession.Current.AccountNo.Trim(), strPassword.Trim(), ddlLanguage.SelectedValue.Trim());

                    if (UpdateStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmCustomerServiceUpdate.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void val_CustomerServiceUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                args.IsValid = true;
                string strError = "";

                if (txtEmail.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (proxy.ValidateEmail(txtEmail.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (ddlLanguage.Text.Equals(""))
                {
                    strError = strError + "*" + lblLanguage.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if ((txtNewPassword.Text.Trim().Length != 0) && (txtConfirmPassword.Text.Trim().Length != 0))
                {
                    if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                    {
                        strError = strError + "*" + lblConfirmPassword.Text.Trim() + "/" + lblNewPassword.Text.Trim() + " " + valShouldSame.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    else if (proxy.ValidatePassword(txtNewPassword.Text.Trim()) != 0)
                    {
                        strError = strError + "*" + lblNewPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;

                    }
                }
                
                if (!(args.IsValid))
                {
                    val_CustomerServiceUpdate.ErrorMessage = strError;
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_CustomerServiceUpdate_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }
    }
}