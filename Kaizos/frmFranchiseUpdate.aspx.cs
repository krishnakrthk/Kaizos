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
    public partial class frmFranchiseUpdate : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmFranchiseUpdate));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmFranchiseUpdate").ToString();
                try
                {
                    if (KaizosSession.Current.UserType.Trim() == "FR")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        //To fill Dimension Units drop down list
                        List<SCountryTable> sCountryTable = new List<SCountryTable>();
                        sCountryTable = proxy.FillCountryCombo().ToList();
                        ddlCountry.DataSource = sCountryTable;
                        ddlCountry.DataTextField = "CodeName";
                        ddlCountry.DataValueField = "CountryCode";
                        ddlCountry.DataBind();

                        ddlLanguage.DataSource = proxy.GetLanguageList();
                        ddlLanguage.DataBind();

                        btnLoad.Visible = false;
                        txtEmail.Text = KaizosSession.Current.UserId.Trim();
                        btnLoad_Click(sender, e);
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
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CountryCodeLoadFailure").ToString();
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
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CountryCodeLoadFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                SFranchise sFranchise = new SFranchise();

                sFranchise = proxy.GetFranchise(txtEmail.Text.Trim());

                ddlLanguage.Text = sFranchise.Language;
                txtCompanyName.Text = sFranchise.CompanyName;
                txtName.Text = sFranchise.Name;
                txtLegalForm.Text = sFranchise.LegalForm;
                txtCommercialName.Text = sFranchise.CommercialName;
                txtManPower.Text = Convert.ToString(sFranchise.ManPower);
                txtcEmail.Text = sFranchise.Email;
                txtPhoneNumber.Text = sFranchise.TelephoneNo;
                txtFaxNumber.Text = sFranchise.FaxNo;
                txtSiretNo.Text = sFranchise.SiretNo;
                txtVatNo.Text = sFranchise.VatNo;
                txtAddress1.Text = sFranchise.Address1;
                txtAddress2.Text = sFranchise.Address2;
                txtAddress3.Text = sFranchise.Address3;
                txtZipcode.Text = sFranchise.ZipCode;
                txtCity.Text = sFranchise.City;
                if (sFranchise.Country.Trim().Length != 0)
                  ddlCountry.SelectedValue = sFranchise.Country.Trim();

                ViewState["ScanDoc"] = sFranchise.ScannedDoc.Trim();
                rtxtChalandiseZone.Text = sFranchise.AssignedZone;
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "FranchiseLoadFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnLoad_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "FranchiseLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void bbtSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                string Sessoin_UserID = KaizosSession.Current.UserId;
                string Sessoin_UserType = KaizosSession.Current.UserType;
                string strPassword = "";
                // Need to call password generation method
                if (txtConfirmPassword.Text.Trim().Length != 0)
                    strPassword = proxy.EncryptString(txtConfirmPassword.Text.Trim(), "Password");

                if (IsValid)
                {
                    SFranchise sFranchise = new SFranchise();

                    sFranchise.AccountNo = "";
                    sFranchise.Address1 = txtAddress1.Text.Trim();
                    sFranchise.Address2 = txtAddress2.Text.Trim();
                    sFranchise.Address3 = txtAddress3.Text.Trim();
                    sFranchise.AssignedZone = rtxtChalandiseZone.Text.Trim();
                    sFranchise.City = txtCity.Text.Trim();
                    sFranchise.CommercialName = txtCommercialName.Text.Trim();
                    sFranchise.CompanyName = txtCompanyName.Text.Trim();
                    sFranchise.Country = ddlCountry.SelectedValue.Trim();
                    sFranchise.CreatedDate = DateTime.Now;
                    sFranchise.CreatedBy = Sessoin_UserID.Trim();

                    if (Sessoin_UserType.Trim() == "AD")
                        sFranchise.CreatedUserType = SEnumUserType.Administrator;
                    else if (Sessoin_UserType.Trim() == "FR")
                        sFranchise.CreatedUserType = SEnumUserType.Franchise;
                    else if (Sessoin_UserType.Trim() == "AZ")
                        sFranchise.CreatedUserType = SEnumUserType.Authorized;
                    else if (Sessoin_UserType.Trim() == "RF")
                        sFranchise.CreatedUserType = SEnumUserType.Referent;
                    else if (Sessoin_UserType.Trim() == "CS")
                        sFranchise.CreatedUserType = SEnumUserType.CustomerService;
                    else
                        sFranchise.CreatedUserType = SEnumUserType.Administrator;

                    //Need to assign this through one textbox if require
                    sFranchise.CustomerType = SEnumCustomerType.RegularCustomer;
                    sFranchise.CustomerTypeChanged = SEnumFlag.No;
                    sFranchise.Email = txtEmail.Text.Trim();
                    sFranchise.FaxNo = txtFaxNumber.Text.Trim();
                    sFranchise.IsChangePasswordRequired = SEnumFlag.Yes;
                    sFranchise.IsSalesTarrifAssigned = SEnumFlag.No;
                    sFranchise.IsToSAccepted = SEnumFlag.No;
                    sFranchise.Language = ddlLanguage.SelectedValue.Trim();
                    sFranchise.LastLogin = DateTime.Now;
                    sFranchise.LastUpdate = DateTime.Now;
                    sFranchise.LegalForm = txtLegalForm.Text.Trim();
                    sFranchise.ManPower = Convert.ToInt32(txtManPower.Text.Trim());
                    sFranchise.Name = txtName.Text.Trim();
                    sFranchise.Password = strPassword.Trim();
                    sFranchise.RegistrationNo = "";
                    sFranchise.ScannedDoc = ViewState["ScanDoc"].ToString();
                    sFranchise.Status = SEnumUserStatus.BeingCreated;
                    sFranchise.TelephoneNo = txtPhoneNumber.Text.Trim();
                    sFranchise.ToSAcceptedDate = DateTime.Now;
                    sFranchise.UserType = SEnumUserType.Franchise;
                    sFranchise.ZipCode = txtZipcode.Text.Trim();
                    sFranchise.SiretNo = txtSiretNo.Text.Trim();
                    sFranchise.VatNo = txtVatNo.Text.Trim();

                    if (proxy.UpdateFranchise(sFranchise) == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(),txtcEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmFranchiseUpdate.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(),txtcEmail.Text.Trim());
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtcEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "bbtSubmit_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtcEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void bbtCancel_Click(object sender, EventArgs e)
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtcEmail.Text = "";
            txtCity.Text = "";
            txtCommercialName.Text = "";
            txtCompanyName.Text = "";
            txtConfirmPassword.Text = "";
            txtEmail.Text = "";
            txtFaxNumber.Text = "";
            txtLegalForm.Text = "";
            txtManPower.Text = "";
            txtName.Text = "";
            txtNewPassword.Text = "";
            txtPhoneNumber.Text = "";
            txtSiretNo.Text = "";
            txtVatNo.Text = "";
            txtZipcode.Text = "";
            rtxtChalandiseZone.Text = "";
        }

        protected void val_FranchiseUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = true;
                string strError = "";
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                if (txtEmail.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (proxy.ValidateEmail(txtEmail.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                //if (txtNewPassword.Text.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblNewPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                //if (txtConfirmPassword.Text.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblConfirmPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                if (txtNewPassword.Text.Trim().Length != 0 && txtConfirmPassword.Text.Trim().Length != 0)
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


                if (ddlLanguage.SelectedValue.Trim().Length == 0)
                {
                    strError = strError + "*" + lblLanguage.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtCompanyName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(proxy.isAlphaNumericValidation(txtCompanyName.Text.Trim())))
                    {
                        strError = strError + "*" + txtCompanyName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(proxy.isAlphaNumericValidation(txtName.Text.Trim())))
                    {
                        strError = strError + "*" + txtName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtLegalForm.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblLegalForm.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtCommercialName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCommercialName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(proxy.isAlphaNumericValidation(txtCommercialName.Text.Trim())))
                    {
                        strError = strError + "*" + lblCommercialName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtManPower.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblManPower.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(proxy.isNumericValidation(txtManPower.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblManPower.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtPhoneNumber.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(proxy.isNumericValidation(txtPhoneNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtSiretNo.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblSiretNo.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtAddress1.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblAddress1.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtZipcode.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtCity.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (ddlCountry.SelectedValue.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (rtxtChalandiseZone.Text.Trim().Length > 5000)
                {
                    strError = strError + "*" + lblChalandiseZone.Text.Trim() + " " + valMaxAllowed.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (!(args.IsValid))
                {
                    val_FranchiseUpdate.ErrorMessage = strError;
                }
                //Need to validate zipcode format
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtcEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_FranchiseUpdate_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtcEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }
    }
}
