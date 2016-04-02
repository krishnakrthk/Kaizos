using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

using System.Web.Configuration;
using System.Web.Security;

namespace Kaizos
{
    public partial class frmLogin : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmLogin));

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    SUser sUser = proxy.GetLogin(txtEmail.Text.Trim());
                    KaizosSession.Current.UserResourceFile = proxy.GetLanguageResource(sUser.Country, sUser.Language);
                    SToS sTos = new SToS();

                    sTos = proxy.GetActiveTos();

                    #region Session Variable
                    KaizosSession.Current.AccountNo = sUser.AccountNo.Trim();
                    KaizosSession.Current.UserId = sUser.Email.Trim();
                    
                    System.Web.SessionState.HttpSessionState ss = HttpContext.Current.Session;
                    KaizosSession.Current.SessionID = ss.SessionID;

                    if (sUser.UserType == SEnumUserType.Administrator)
                        KaizosSession.Current.UserType = "AD";
                    else if (sUser.UserType == SEnumUserType.Authorized)
                        KaizosSession.Current.UserType = "AZ";
                    else if (sUser.UserType == SEnumUserType.CustomerService)
                        KaizosSession.Current.UserType = "CS";
                    else if (sUser.UserType == SEnumUserType.Franchise)
                        KaizosSession.Current.UserType = "FR";
                    else
                        KaizosSession.Current.UserType = "RF";

                    if (sUser.Status == SEnumUserStatus.Archived)
                        KaizosSession.Current.UserStatus = "A";
                    else if (sUser.Status == SEnumUserStatus.BeingCreated)
                        KaizosSession.Current.UserStatus = "B";
                    else if (sUser.Status == SEnumUserStatus.Enabled)
                        KaizosSession.Current.UserStatus = "E";
                    else
                        KaizosSession.Current.UserStatus = "D";

                    KaizosSession.Current.UserLanguage = sUser.Language.Trim();


                    if (sUser.IsSalesTarrifAssigned == SEnumFlag.Yes)
                        KaizosSession.Current.IsSalesTarifAssigned = "Y";
                    else
                        KaizosSession.Current.IsSalesTarifAssigned = "N";

                    if (sUser.IsToSAccepted == SEnumFlag.Yes)
                        KaizosSession.Current.IsTosAccepted = "Y";
                    else
                        KaizosSession.Current.IsTosAccepted = "N";
                    #endregion

                    SFranchise sFranchise = proxy.GetFranchise(sUser.Email.Trim());

                    if (sUser.IsChangePasswordRequired == SEnumFlag.Yes)
                        Response.Redirect("frmConfirmPassword.aspx");

                    else if (sUser.ToSAcceptedDate == null)
                    {
                        //Response.Redirect("frmTosAccept.aspx");
                        lblMessage.Text = sTos.TermsOfService;
                        TOSAcceptModalPopupExtender.Show();
                    }

                    else if (sUser.ToSAcceptedDate < sTos.LastUpdate)
                    {
                        //Response.Redirect("frmTosAccept.aspx");
                        lblMessage.Text = sTos.TermsOfService;
                        TOSAcceptModalPopupExtender.Show();
                    }
                    else if (sFranchise.CustomerTypeChanged == SEnumFlag.Yes && sFranchise.CustomerType == SEnumCustomerType.RegularCustomer)
                    {
                        //Response.Redirect("frmTosAccept.aspx");
                        lblMessage.Text = sTos.TermsOfService;
                        TOSAcceptModalPopupExtender.Show();
                    }
                    else
                    {
                        Response.Redirect("frmTariffDelayInterrogation.aspx");
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [05JAN12RM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Client, "Button1_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }
      
        protected void val_Login_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";

            // To validate if it is empty
            if (txtEmail.Text.Length == 0)
            {
                strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            if (txtPassword.Text.Length == 0)
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;

            }
            
            //To validate Password Length
            if (txtPassword.Text.Trim().Length < 8 || txtPassword.Text.Trim().Length > 15)
            {
                strError = strError + "*" + lblPassword.Text.Trim() + " " + valLength.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            try
            {
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                string strEncryptedPassword = proxy.EncryptString(txtPassword.Text.Trim(), "Password");

                SUser sUser = new SUser();
                SCustomer sCustomer = new SCustomer();
                SFranchise fFranchise = new SFranchise();
                SAuthorized sAuthorized = new SAuthorized(); //[15FEB12SM]

                if (txtEmail.Text.Trim().Length != 0)
                {
                    sUser = proxy.GetLogin(txtEmail.Text.Trim());
                    if (proxy.ValidateUser(txtEmail.Text.Trim(), strEncryptedPassword.Trim()) != 0)
                    {
                        strError = strError + "*" + lblEmail.Text.Trim() + "/" + lblPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    //[15FEB12SM]
                    //else if (sUser.UserType == SEnumUserType.Authorized || sUser.UserType == SEnumUserType.Referent)
                    else if (sUser.UserType == SEnumUserType.Referent)
                    {
                        sCustomer = proxy.GetCustomer(txtEmail.Text.Trim());
                        fFranchise = proxy.GetFranchise(sUser.CreatedBy.Trim());
                        if ((sUser.LastLogin == null) && (sUser.CreatedUserType == SEnumUserType.Referent))
                        {
                            // Need to add the mapping details in this message
                            strError = strError + string.Format(GetGlobalResourceObject("LocalString", "MessageLoginEndCustomer").ToString(), sCustomer.Name.Trim(), sCustomer.TelephoneNo.Trim(), fFranchise.TelephoneNo, fFranchise.Email) + "<br>";
                            args.IsValid = false;
                        }
                        if (sUser.IsSalesTarrifAssigned == SEnumFlag.No)
                        {
                            // Need to add the mapping details in this message
                            strError = strError + string.Format(GetGlobalResourceObject("LocalString", "MessageLoginEndCustomerIsTariffAssign").ToString(), sCustomer.Name.Trim(), sCustomer.TelephoneNo.Trim(), fFranchise.TelephoneNo, fFranchise.Email) + "<br>";
                            args.IsValid = false;
                        }
                    }
                    // Introduced for Authorized user [15FEB12SM]
                    else if (sUser.UserType == SEnumUserType.Authorized)
                    {
                        string strReferentUserID = "";
                        sAuthorized = proxy.GetAuthorized(sUser.AccountNo.Trim());
                        strReferentUserID = proxy.GetUser(sAuthorized.ReferentUserId.Trim());
                        sCustomer = proxy.GetCustomer(strReferentUserID.Trim());
                        fFranchise = proxy.GetFranchise(sUser.CreatedBy.Trim());
                        if ((sUser.LastLogin == null) && (sUser.CreatedUserType == SEnumUserType.Referent))
                        {
                            // Need to add the mapping details in this message
                            strError = strError + string.Format(GetGlobalResourceObject("LocalString", "MessageLoginEndCustomer").ToString(), sAuthorized.ContactName.Trim(), sAuthorized.TelephoneNo.Trim(), fFranchise.TelephoneNo, fFranchise.Email) + "<br>";
                            args.IsValid = false;
                        }
                        if (sUser.IsSalesTarrifAssigned == SEnumFlag.No)
                        {
                            // Need to add the mapping details in this message
                            strError = strError + string.Format(GetGlobalResourceObject("LocalString", "MessageLoginEndCustomerIsTariffAssign").ToString(), sAuthorized.ContactName.Trim(), sAuthorized.TelephoneNo.Trim(), fFranchise.TelephoneNo, fFranchise.Email) + "<br>";
                            args.IsValid = false;
                        }
                    }

                    else
                    {
                        if (proxy.UpdateLastLogin(txtEmail.Text.Trim(), DateTime.Now) == 0)
                        {
                            args.IsValid = true;
                            return;
                        }
                        else
                        {
                            strError = strError + "*" + lblEmail.Text.Trim() + " " + valNotAvailable.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }
                }
                if (!(args.IsValid))
                {
                    val_Login.ErrorMessage = strError.Trim();
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [24JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginValFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [24JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Client, "val_Login_ServerValidate()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginValFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void BtnOK_Click(object sender, EventArgs e)
        {
            int result = 1;

            try
            {
                SToS stos = new SToS();
                stos.TermsOfService = "";
                stos.LastUpdate = DateTime.Now;
                stos.Active = SEnumFlag.Yes;
                stos.AccountNo = KaizosSession.Current.AccountNo;

                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                result = proxy.SpInsertTos(stos);
                if (result == 0)
                    Response.Redirect("frmTariffDelayInterrogation.aspx");
                else
                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "TOSFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [24JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [24JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(txtEmail.Text.Trim(), ErrorSource.Client, "BtnOK_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "LoginFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Page.Title = GetGlobalResourceObject("LocalString", "frmLogin").ToString();

            if (WebConfigurationManager.AppSettings["Emergency"].ToString().Equals("Off"))
            {
                Server.Transfer("frmResultLogin.aspx", false);
            }
        }
    }
}
