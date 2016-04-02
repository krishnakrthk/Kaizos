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
    public partial class frmCustomerService : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCustomerService));

        public string struserId = "", strAccount_no = "" , strUserType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmCustomerService").ToString();
                    if (KaizosSession.Current.UserType == "AD")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        struserId = KaizosSession.Current.UserId;
                        strAccount_no = KaizosSession.Current.AccountNo;
                        strUserType = KaizosSession.Current.UserType;
                        List<string> ComboText = proxy.GetLanguageList().ToList();
                        ddlLanguage.DataSource = ComboText;
                        ddlLanguage.DataBind();
                        ddlLanguage.SelectedValue = "FR";
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
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
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
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
            txtCustomerName.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int InsertStatus = 1;
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    SFranchise sFranchise = new SFranchise();
                    sFranchise.Name = txtCustomerName.Text.Trim();
                    sFranchise.Email = txtEmail.Text.Trim();
                    string strCreatePassword = proxy.CreatePassword(8);
                    sFranchise.Password = proxy.EncryptString(strCreatePassword.Trim(), "Password");

                    sFranchise.UserType = SEnumUserType.CustomerService;

                    sFranchise.AccountNo = "";
                    sFranchise.CreatedDate = DateTime.Now;
                    sFranchise.CreatedBy = struserId.Trim();
                    if (strUserType.Trim() == "AD")
                        sFranchise.CreatedUserType = SEnumUserType.Administrator;
                    else if (strUserType.Trim() == "FR")
                        sFranchise.CreatedUserType = SEnumUserType.Franchise;
                    else if (strUserType.Trim() == "CS")
                        sFranchise.CreatedUserType = SEnumUserType.CustomerService;
                    else if (strUserType.Trim() == "AZ")
                        sFranchise.CreatedUserType = SEnumUserType.Authorized;
                    else if (strUserType.Trim() == "RF")
                        sFranchise.CreatedUserType = SEnumUserType.Referent;
                    else
                        sFranchise.CreatedUserType = SEnumUserType.Franchise;

                    //Need to assign this through one textbox if require
                    sFranchise.CustomerType = SEnumCustomerType.RegularCustomer;
                    sFranchise.CustomerTypeChanged = SEnumFlag.No;
                    sFranchise.IsChangePasswordRequired = SEnumFlag.Yes;
                    sFranchise.IsSalesTarrifAssigned = SEnumFlag.No;
                    sFranchise.IsToSAccepted = SEnumFlag.No;
                    sFranchise.Language = ddlLanguage.SelectedValue.Trim();
                    sFranchise.LastLogin = DateTime.Now;
                    sFranchise.LastUpdate = DateTime.Now;
                    sFranchise.Status = SEnumUserStatus.Enabled;
                    sFranchise.ToSAcceptedDate = DateTime.Now;
                    sFranchise.CarrierAccountRef = string.Empty;
                    sFranchise.FirmCreationDate = DateTime.Today;
                    InsertStatus = proxy.InsertFranchise(sFranchise);
                    if (InsertStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationSuccess").ToString(), txtEmail.Text.Trim());

                        if (!proxy.sendMessage(txtEmail.Text.Trim(), strCreatePassword.Trim(), KaizosSession.Current.UserId.Trim()))
                        {
                            KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                            KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + "-" + GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                        }

                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if ((InsertStatus == 2))
                    {
                        KaizosSession.Current.ReturnURL = "frmCustomerService.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmCustomerService.aspx";
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

        protected void val_CustomerService_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                args.IsValid = true;
                string strError = "";

                if (txtCustomerName.Text.Equals(""))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
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

                if (!(args.IsValid))
                {
                    val_CustomerService.ErrorMessage = strError;
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
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_CustomerService_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }
    }
}