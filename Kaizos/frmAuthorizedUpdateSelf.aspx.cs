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
    public partial class frmAuthorizedUpdateSelf : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAuthorizedUpdateSelf));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmAuthorizedUpdateSelf").ToString();
                    errorMsg.Attributes["style"] = "display: none;";
                    if (KaizosSession.Current.UserType != "AZ")
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        
                        SAuthorized sAuthorized  = new SAuthorized();
                        sAuthorized = proxy.GetAuthorized(KaizosSession.Current.AccountNo);
                        txtName.Text = sAuthorized.ContactName;
                        txtEmail.Text = KaizosSession.Current.UserId.Trim();
                        txtPhoneNo.Text = sAuthorized.TelephoneNo.Trim();
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
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void val_ConfirmPage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            try
            {
                KaizosServiceContractClient context = new KaizosServiceContractClient();
                if (txtName.Text.Equals(""))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtEmail.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                //if (txtPassword.Text.Equals(""))
                //{
                //    strError = strError + "*" + lblPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                //if (txtConfirmPassword.Text.Equals(""))
                //{
                //    strError = strError + "*" + lblConfirmPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                if ((txtPassword.Text.Trim().Length != 0) && (txtConfirmPassword.Text.Trim().Length != 0))
                {
                    if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                    {
                        strError = strError + "*" + lblPassword.Text.Trim() + "/" + lblConfirmPassword.Text + " " + valShouldSame.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else if (txtPassword.Text.Trim().Length != 0)
                    {
                        if (context.ValidatePassword(txtPassword.Text.Trim()) != 0)
                        {
                            strError = strError + "*" + lblPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
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
                    val_Authorized.ErrorMessage = strError;
                    errorMsg.Attributes["style"] = "display: block;";
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
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_ConfirmPage_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                    if (KaizosSession.Current.UserType != "AZ")
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        SAuthorized sAuthorized = new SAuthorized();
                        sAuthorized = proxy.GetAuthorized(KaizosSession.Current.AccountNo);
                        txtName.Text = sAuthorized.ContactName;
                        txtEmail.Text = KaizosSession.Current.UserId.Trim();
                        txtPhoneNo.Text = sAuthorized.TelephoneNo.Trim();
                    }
            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);
                }
            }

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

                    UpdateStatus = proxy.UpdateAuthorizedSelf(KaizosSession.Current.AccountNo.Trim(), strPassword.Trim(), txtPhoneNo.Text.Trim());

                    if (UpdateStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmAuthorizedUpdateSelf.aspx";
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

    }
}