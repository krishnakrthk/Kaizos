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
    public partial class frmAuthorized : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAuthorized));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAuthorized").ToString();

                if (KaizosSession.Current.UserType != "RF")
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
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
                if (txtEmail.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
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

                if (txtName.Text.Trim().Equals(""))
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtPhoneNo.Text.Trim().Equals(""))
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
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
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtName.Text = "";
            txtPhoneNo.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int InsertStatus = 1;
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                    SAuthorized sAuthorize = new SAuthorized();

                    sAuthorize.Name = txtName.Text.Trim();
                    sAuthorize.Email = txtEmail.Text.Trim();

                    string strCreatePassword = proxy.CreatePassword(8);
                    sAuthorize.Password = proxy.EncryptString(strCreatePassword.Trim(), "Password");

                    sAuthorize.UserType = SEnumUserType.Authorized;

                    sAuthorize.AccountNo = "";
                    sAuthorize.CreatedDate = DateTime.Now;
                    sAuthorize.LastUpdate = DateTime.Now;
                    sAuthorize.LastLogin = DateTime.Now;

                    sAuthorize.CreatedBy = KaizosSession.Current.UserId.Trim();

                    if (KaizosSession.Current.UserType.Trim() == "AD")
                        sAuthorize.CreatedUserType = SEnumUserType.Administrator;
                    else if (KaizosSession.Current.UserType.Trim() == "FR")
                        sAuthorize.CreatedUserType = SEnumUserType.Franchise;
                    else if (KaizosSession.Current.UserType.Trim() == "CS")
                        sAuthorize.CreatedUserType = SEnumUserType.CustomerService;
                    else if (KaizosSession.Current.UserType.Trim() == "AZ")
                        sAuthorize.CreatedUserType = SEnumUserType.Authorized;
                    else if (KaizosSession.Current.UserType.Trim() == "RF")
                        sAuthorize.CreatedUserType = SEnumUserType.Referent;
                    else
                        sAuthorize.CreatedUserType = SEnumUserType.Referent;

                    //Need to assign this through one textbox if require
                    sAuthorize.CustomerType = SEnumCustomerType.RegularCustomer;
                    sAuthorize.CustomerTypeChanged = SEnumFlag.No;
                    sAuthorize.IsChangePasswordRequired = SEnumFlag.Yes;
                    sAuthorize.IsSalesTarrifAssigned = SEnumFlag.No;
                    sAuthorize.IsToSAccepted = SEnumFlag.No;
                    sAuthorize.Language = KaizosSession.Current.UserLanguage.Trim();
                    sAuthorize.LastUpdate = DateTime.Now;
                    sAuthorize.Status = SEnumUserStatus.BeingCreated;
                    sAuthorize.ToSAcceptedDate = DateTime.Now;

                    sAuthorize.ContactName = txtName.Text.Trim();
                    sAuthorize.TelephoneNo = txtPhoneNo.Text.Trim();
                    sAuthorize.ReferentUserId = KaizosSession.Current.AccountNo.Trim();

                    InsertStatus = proxy.InsertAuthorized(sAuthorize);
                    if (InsertStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmAuthorizedUserList.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationSuccess").ToString(), txtEmail.Text.Trim());

                        if (!proxy.sendMessage(txtEmail.Text.Trim(), strCreatePassword.Trim(), KaizosSession.Current.UserId.Trim()))
                        {
                            KaizosSession.Current.ReturnURL = "frmAuthorizedUserList.aspx";
                            KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + "-" + GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                        }

                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if ((InsertStatus == 2))
                    {
                        KaizosSession.Current.ReturnURL = "frmAuthorized.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmAuthorized.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
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
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }

        }
    }
}