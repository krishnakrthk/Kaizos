using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmEndCustomer : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmEndCustomer));
        public  string UserID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCusotmer").ToString();
                try
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                    //To fill Dimension Units drop down list
                    List<SCountryTable> sCountryTable = new List<SCountryTable>();
                    sCountryTable = proxy.FillCountryCombo().ToList();

                    ddlCountry.DataSource = sCountryTable;
                    ddlCountry.DataTextField = "CodeName";
                    ddlCountry.DataValueField = "CountryCode";
                    ddlCountry.DataBind();

                    ddlInvoiceCountry.DataSource = sCountryTable;
                    ddlInvoiceCountry.DataTextField = "CodeName";
                    ddlInvoiceCountry.DataValueField = "CountryCode";
                    ddlInvoiceCountry.DataBind();

                    //ddlShippingPreference.Items.Add("Fastest");
                    //ddlShippingPreference.Items.Add("Most Competitive");
                    //ddlShippingPreference.Items.Add("Named Carrier");
                   // trSelectShippingReference.Visible = false;
                    List<SCarrier> lstCarrier = proxy.GetCarriers().ToList();
                    ddlShipNamedCarrier.DataSource = lstCarrier;
                    ddlShipNamedCarrier.DataTextField = "CarrierName";
                    ddlShipNamedCarrier.DataValueField = "CarrierName";
                    ddlShipNamedCarrier.DataBind();


                    List<ShippingPreference> lstShippingPreferance = new List<ShippingPreference>();
                    ShippingPreference sp = new ShippingPreference();
                    sp.Id = 1;
                    sp.ShippingPreferenceType = "the most competitive";
                    sp.priority = 1;
                    lstShippingPreferance.Add(sp);
                    sp = new ShippingPreference();
                    sp.Id = 2;
                    sp.priority = 2;
                    sp.ShippingPreferenceType = "the fastest";
                    lstShippingPreferance.Add(sp);
                    sp = new ShippingPreference();
                    sp.Id = 3;
                    sp.ShippingPreferenceType = "named carrier";
                    sp.priority = 3;
                    lstShippingPreferance.Add(sp);
                    rlShippingPreference.DataSource = lstShippingPreferance;
                    rlShippingPreference.DataBind();
                    trSelectShippingReference.Visible = false;
                    lblShipNamedCarrier.Visible = false;
                    ddlShipNamedCarrier.Visible = false; 
                   // trSelectShippingNamedCarrier.Visible = false ;
                    List<string> FunctionList = proxy.GetFunction().ToList();

                    ddlFunction.DataSource = FunctionList;
                    ddlFunction.DataBind();

                    txtTransportBudget.Visible = false;
                    lblTransportBudget.Visible = false;
                    txtTransportBudget.Text = "";

                    SToS sTos = new SToS();
                    sTos = proxy.GetActiveToS();
                    lblMessage.Text = sTos.TermsOfService.Trim();
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtCompanyName.Text = "";
            txtConfirmPassword.Text = "";
            txtContactName.Text = "";
            txtEmail.Text = "";
            txtEmailConfirmation.Text = "";
            txtHQZipcode.Text = "";
            txtInvoicePhoneNumber.Text = "";
            txtName.Text = "";
            txtPassword.Text = "";
            txtPhoneNumber.Text = "";
            txtVatNo.Text = "";
            txtSiretNo.Text = "";
            txtTransportBudget.Text = "";
            txtZipcode.Text = "";
            chkDeferredPayment.Checked = false;
            chkTOS.Checked = false;
            chkUseInvoiceAddress.Checked = false;
            chkUserReturnAddress.Checked = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int InsertResult = 1;
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    SCustomer sCustomer = new SCustomer();
                    SFranchise sFranchise = new SFranchise();
                    if (UserID != null)
                    {
                        sFranchise = proxy.GetFranchise(UserID.Trim());
                    }

                    //Login
                    sCustomer.AccountNo = "";
                    sCustomer.Email = txtEmail.Text.Trim();
                    sCustomer.Password = proxy.EncryptString(txtPassword.Text.Trim(), "Password");
                    sCustomer.UserType = SEnumUserType.Referent;
                    sCustomer.Status = SEnumUserStatus.BeingCreated;
                    sCustomer.IsSalesTarrifAssigned = SEnumFlag.No;
                    if (chkTOS.Checked)
                    {
                        sCustomer.IsToSAccepted = SEnumFlag.Yes;
                        sCustomer.ToSAcceptedDate = DateTime.Now;
                    }
                    else
                        sCustomer.IsToSAccepted = SEnumFlag.No;

                    sCustomer.Language = sFranchise.Language.Trim();
                    sCustomer.IsChangePasswordRequired = SEnumFlag.Yes;
                    sCustomer.CreatedBy = UserID.Trim();
                    sCustomer.AdministratorUserId = sFranchise.AccountNo.Trim();
                    sCustomer.CreatedUserType = SEnumUserType.Referent;
                    sCustomer.IsKeyAccount = SEnumFlag.No;
                    sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                    sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;
                    sCustomer.CustomerCategory = SEnumCustCategory.A;

                    sCustomer.CreatedDate = DateTime.Now;
                    sCustomer.LastUpdate = DateTime.Now;
                    sCustomer.LastLogin = DateTime.Now;

                    //Company
                    sCustomer.CompanyName = txtCompanyName.Text.Trim();
                    sCustomer.Name = txtName.Text.Trim();
                    sCustomer.Designation = ddlFunction.SelectedValue.Trim();
                    sCustomer.Email = txtEmail.Text.Trim();
                    sCustomer.Password = proxy.EncryptString(txtPassword.Text.Trim(), "Password");
                    sCustomer.TelephoneNo = txtPhoneNumber.Text.Trim();
                    sCustomer.HqZipcode = ddlCountry.SelectedValue.Trim() + txtHQZipcode.Text.Trim();
                    sCustomer.Country = ddlCountry.SelectedValue.Trim();

                    //Invoice Address
                    sCustomer.ContactName = txtContactName.Text.Trim(); ;
                    sCustomer.InvoicePhoneNumber = txtInvoicePhoneNumber.Text.Trim();
                    sCustomer.InvoiceFaxNo = txtInvoiceFaxNo.Text.Trim();
                    sCustomer.VatNo = txtVatNo.Text.Trim();
                    sCustomer.SiretNo = txtSiretNo.Text.Trim();
                    sCustomer.InvoiceAddress1 = txtAddress1.Text.Trim();
                    sCustomer.InvoiceAddress2 = txtAddress2.Text.Trim();
                    sCustomer.InvoiceAddress3 = txtAddress3.Text.Trim();
                    sCustomer.InvoiceZipcode = txtZipcode.Text.Trim();
                    sCustomer.InvoiceCity = txtCity.Text.Trim();
                    sCustomer.InvoiceCountry = ddlInvoiceCountry.SelectedValue.Trim();

                    //Shipping
                    if (chkUseInvoiceAddress.Checked)
                        sCustomer.UsedForShipping = SEnumFlag.Yes;
                    else
                        sCustomer.UsedForShipping = SEnumFlag.No;

                    if (chkUserReturnAddress.Checked)
                        sCustomer.UsedForReturn = SEnumFlag.Yes;
                    else
                        sCustomer.UsedForReturn = SEnumFlag.No;
                    string strShippingReference = string.Empty;
                    sCustomer.CarrierName = string.Empty;

                    //enable shipping preferance and name carrier should be stored
                    if (chkEnableShippingPreferance.Checked)
                    {

                        for (int i = 0; i < rlShippingPreference.Items.Count; i++)
                        {
                            if (rlShippingPreference.DataKeys[i].ToString().Equals("1"))
                            {
                                strShippingReference = strShippingReference + "MC|";

                            }
                            else if (rlShippingPreference.DataKeys[i].ToString().Equals("2"))
                            {
                                strShippingReference = strShippingReference + "F|";
                            }
                            else
                            {
                                strShippingReference = strShippingReference + "NC|";
                            }
                        }
                        if (ddlShipNamedCarrier.SelectedValue == null)
                        {
                            sCustomer.CarrierName = string.Empty;
                        }
                        else
                        {
                            sCustomer.CarrierName = ddlShipNamedCarrier.SelectedValue;
                        }
                    }
                   

                    sCustomer.ShipmentPreference = strShippingReference;
                    sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;
                    sCustomer.CustomerTypeChanged = SEnumFlag.No;
                    sCustomer.DeferedPaymentRequired = SEnumFlag.No;
                    sCustomer.FICTIVE_ACCOUNT = SEnumFlag.No;  
                    //Payment Method
                        sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;

                        if (chkDeferredPayment.Checked)
                        {
                            sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                            sCustomer.BudgetAmount = decimal.Parse(txtTransportBudget.Text.Trim());
                        }
                        else
                        {
                            sCustomer.DeferedPaymentRequired = SEnumFlag.No;
                            sCustomer.BudgetAmount = 0;
                        }
                        if (txtDepositAmount.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.DepositAmount = 0;
                        }
                        else
                        {
                            sCustomer.DepositAmount = decimal.Parse(txtDepositAmount.Text.Trim());
                        }
                    InsertResult = proxy.InsertEndCustomer(sCustomer);

                    if (InsertResult == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationSuccess").ToString(), txtEmail.Text.Trim());

                       // commended by 19MAR12SM
                        if (!proxy.sendMessage(txtEmail.Text.Trim(), txtPassword.Text.Trim(), UserID.Trim()))
                        {
                            KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                            KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + "-" + GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                        }
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (InsertResult == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        private bool IsAlphaNum(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return (str.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c) || Char.IsPunctuation('.')));
        }

        private bool ValidateZipCode(string strActualFormat)
        {
            string strHQZipCode = txtHQZipcode.Text.Trim();
            char[] chrHQzipcode = strHQZipCode.ToCharArray();
            string strformat = string.Empty;
            bool result = false;
            foreach (char c in chrHQzipcode)
            {
                if (char.IsDigit(c))
                {
                    strformat = strformat + "N";


                }
                else if (char.IsLetter(c))
                {
                    strformat = strformat + "A";
                }
            }
            if (strActualFormat.IndexOf(',') > 0)
            {
                string[] sFormat = strActualFormat.Split(',');
                foreach (string s in sFormat)
                {
                    if (s.Length == strformat.Length)
                    {
                        result = s.Contains(strformat);
                        if (result)
                        {
                            break;
                        }
                    }
                }

            }
            else
            {
                if (strActualFormat.Length == strformat.Length)
                {
                    result = strActualFormat.Contains(strformat);
                }
            }
            return result;
        }

        protected void val_EndCustomer_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            KaizosServiceContractClient Context = new KaizosServiceContractClient();
            try
            {
                if (txtCompanyName.Text.Equals(""))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(IsAlphaNum(txtCompanyName.Text.Trim())))
                    {
                        strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtName.Text.Equals(""))
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(Context.isAlphaNumericValidation(txtName.Text.Trim())))
                    {
                        strError = strError + "*" + lblName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtEmail.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtPassword.Text.Equals(""))
                {
                    strError = strError + "*" + lblPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtPhoneNumber.Text.Equals(""))
                {
                    strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(Context.isNumericValidation(txtPhoneNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtInvoiceFaxNo.Text.Trim().Length != 0)
                {
                    if (!(Context.isNumericValidation(txtInvoiceFaxNo.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblFaxNo.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (ddlCountry.SelectedValue.Equals(""))
                {
                    strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                UserID = Context.ValidateHQZipcode(ddlCountry.SelectedValue.Trim() + txtHQZipcode.Text.Trim());

                if (txtHQZipcode.Text.Equals(""))
                {
                    strError = strError + "*" + lbHQZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!Context.isAlphaNumericValidation(txtHQZipcode.Text.Trim()))
                {
                    strError = strError + "*" + lbHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                //else if (UserID == "2")
                //{
                //    strError = strError + "*" + lbHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}
                else
                {
                    string strActualFormat = string.Empty;
                    strActualFormat = Context.GetPostalCode(ddlCountry.SelectedValue);
                    if (!ValidateZipCode(strActualFormat))
                    {
                        strError = strError + "*" + lbHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                }

                if (txtEmailConfirmation.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmailConfirmation.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtConfirmPassword.Text.Equals(""))
                {
                    strError = strError + "*" + lblConfirmPassword.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

               

                if (txtContactName.Text.Equals(""))
                {
                    strError = strError + "*" + lblContactName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtInvoicePhoneNumber.Text.Equals(""))
                {
                    strError = strError + "*" + lblInvoicePhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(Context.isNumericValidation(txtInvoicePhoneNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblInvoicePhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }


                if (txtSiretNo.Text.Equals(""))
                {
                    strError = strError + "*" + lblSiretNo.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtAddress1.Text.Equals(""))
                {
                    strError = strError + "*" + lblAddress1.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtZipcode.Text.Equals(""))
                {
                    strError = strError + "*" + lblZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtCity.Text.Equals(""))
                {
                    strError = strError + "*" + lblCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (ddlInvoiceCountry.SelectedValue.Equals(""))
                {
                    strError = strError + "*" + lblInvoiceCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (chkDeferredPayment.Checked)
                {
                    if (txtTransportBudget.Text.Trim().Equals(""))
                    {
                        strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else
                    {
                        if (!(Context.isNumericValidation(txtTransportBudget.Text.Trim(), System.Globalization.NumberStyles.Float)))
                        {
                            strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }
                }
                if (!txtDepositAmount.Text.Trim().Equals(""))
                {
                    if (!(Context.isNumericValidation(txtDepositAmount.Text.Trim(), System.Globalization.NumberStyles.Float)))
                    {
                        strError = strError + "*" + lblDepositAmount.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }
                
                if (txtEmail.Text.Trim() != txtEmailConfirmation.Text.Trim())
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + "/" + lblEmailConfirmation.Text.Trim()+ " " + valShouldSame.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (Context.ValidateEmail(txtEmail.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }


                if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                {
                    strError = strError + "*" + lblPassword.Text.Trim() + "/" + lblConfirmPassword.Text.Trim() + " " + valShouldSame.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (Context.ValidatePassword(txtPassword.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (!chkTOS.Checked)
                {
                    strError = strError + "*" + lblTos.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (!(args.IsValid))
                {
                    val_EndCustomer.ErrorMessage = strError;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(UserID.Trim(), ErrorSource.Client, "val_EndCustomer_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmLogin.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void chkDeferredPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeferredPayment.Checked)
            {
                txtTransportBudget.Visible = true;
                lblTransportBudget.Visible = true;
                txtTransportBudget.Text = "";
            }
            else
            {
                txtTransportBudget.Visible = false;
                lblTransportBudget.Visible = false;
                txtTransportBudget.Text = "";
            }
        }

        protected void chkEnableShippingPreferance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableShippingPreferance.Checked)
            {
                trSelectShippingReference.Visible = true;
                //trSelectShippingNamedCarrier.Visible = true;
                lblShipNamedCarrier.Visible = true;
                ddlShipNamedCarrier.Visible = true; 
            }
            else
            {
                trSelectShippingReference.Visible = false;
               // trSelectShippingNamedCarrier.Visible = false;
                lblShipNamedCarrier.Visible = false;
                ddlShipNamedCarrier.Visible = false; 
            }
        }
    
   
    }
    public class ShippingPreference
    {

        public int Id { get; set; }
        public string ShippingPreferenceType { get; set; }
        public int priority { get; set; }
    }
}