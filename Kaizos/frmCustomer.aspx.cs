using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using KaizosServiceInvokers.KaizosServiceReference;
using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmCustomer : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCustomer));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCusotmer").ToString();
                try
                {
                    if (KaizosSession.Current.UserType.Trim() == "AD" || KaizosSession.Current.UserType.Trim() == "FR")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        List<SCountryTable> sCountryTable = new List<SCountryTable>();

                        sCountryTable = proxy.FillCountryCombo().ToList();

                        //To fill Dimension Units drop down list
                        ddlCountry.DataSource = sCountryTable;
                        ddlCountry.DataTextField = "CodeName";
                        ddlCountry.DataValueField = "CountryCode";
                        ddlCountry.DataBind();
                        ddlCountry.SelectedValue = "FR"; // bug id: 1273 by 16MAR12SM

                        //added by HV for bug 1108
                        List<SCarrier> lstCarrier = proxy.GetCarriers().ToList();
                        lbCarrier.DataSource = lstCarrier;
                        lbCarrier.DataTextField = "CarrierName";
                        lbCarrier.DataValueField = "CarrierName";
                        lbCarrier.SelectionMode = ListSelectionMode.Multiple;
                        lbCarrier.DataBind();
                        lbCarrier.Visible = true;

                        ddInsuranceMethod.Items.Add("Adhoc");
                        ddInsuranceMethod.Items.Add("Systematic");
                       // ModalPopupExtender1.Hide(); 
                        ddlCustomerCategory.Items.Add("");
                        ddlCustomerCategory.Items.Add("A");
                        ddlCustomerCategory.Items.Add("B");
                        ddlCustomerCategory.Items.Add("C");

                        List<SIndustry> sIndustry = new List<SIndustry>();
                        sIndustry = proxy.GetIndustry().ToList();

                        OptionDropDownList1.DataGroupField = "Department";
                        OptionDropDownList1.DataTextField = "Activity";
                        OptionDropDownList1.DataValueField = "Activity";
                        OptionDropDownList1.DataSource = sIndustry;
                        OptionDropDownList1.DataBind();
                        OptionDropDownList1.SelectedValue = "Not specified";
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
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            txtEmail.Text = "";
            txtHQZipcode.Text = "";
            txtInvoiceName.Text = "";
            txtLegalForm.Text = "";
            txtManPower.Text = "";
            txtName.Text = "";
            txtPhoneNumber.Text = "";
            chkKeyAccount.Checked = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                string Sessoin_UserID = KaizosSession.Current.UserId;
                string Sessoin_UserType = KaizosSession.Current.UserType;
                string strCreatePassword = proxy.CreatePassword(8);
                string strPassword = proxy.EncryptString(strCreatePassword, "Password");

                if (IsValid)
                {
                    #region BCustomer Fill Area
                    SCustomer sCustomer = new SCustomer();
                    sCustomer.AccountNo = "";
                    sCustomer.Email = txtEmail.Text.Trim();
                    sCustomer.Password = strPassword.Trim();
                    sCustomer.UserType = SEnumUserType.Referent;
                    sCustomer.Status = SEnumUserStatus.BeingCreated;
                    sCustomer.IsSalesTarrifAssigned = SEnumFlag.No;
                    sCustomer.IsToSAccepted = SEnumFlag.No;
                    sCustomer.Language = KaizosSession.Current.UserLanguage.Trim();
                    sCustomer.IsChangePasswordRequired = SEnumFlag.Yes;
                    sCustomer.CreatedBy = Sessoin_UserID.Trim();

                    if (Sessoin_UserType.Trim() == "AD")
                        sCustomer.CreatedUserType = SEnumUserType.Administrator;
                    else if (Sessoin_UserType.Trim() == "FR")
                        sCustomer.CreatedUserType = SEnumUserType.Franchise;
                    else if (Sessoin_UserType.Trim() == "AZ")
                        sCustomer.CreatedUserType = SEnumUserType.Authorized;
                    else if (Sessoin_UserType.Trim() == "RF")
                        sCustomer.CreatedUserType = SEnumUserType.Referent;
                    else if (Sessoin_UserType.Trim() == "CS")
                        sCustomer.CreatedUserType = SEnumUserType.CustomerService;
                    else
                        sCustomer.CreatedUserType = SEnumUserType.Administrator;

                    sCustomer.CompanyName = txtCompanyName.Text.Trim();
                    sCustomer.Name = txtName.Text.Trim();
                    sCustomer.TelephoneNo = txtPhoneNumber.Text.Trim();
                    sCustomer.HqZipcode = txtHQZipcode.Text.Trim();
                    sCustomer.Country = ddlCountry.SelectedValue.Trim();

                    if (chkKeyAccount.Checked)
                    {
                        sCustomer.IsKeyAccount = SEnumFlag.Yes;
                        sCustomer.CustomerType = SEnumCustomerType.KeyCustomer;
                        //added by HV for bug 1108
                        foreach (ListItem li in lbCarrier.Items)
                        {
                            if (li.Selected)
                            {
                                sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER + li.Value + "|";
                            }
                        }
                        sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER.Substring(0, sCustomer.KEY_CARRIER.Length);
                    }
                    else
                    {
                        sCustomer.IsKeyAccount = SEnumFlag.No;
                        sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                        sCustomer.KEY_CARRIER = string.Empty;

                    }

                    if (ddlCustomerCategory.SelectedValue == "A")
                        sCustomer.CustomerCategory = SEnumCustCategory.A;
                    else if (ddlCustomerCategory.SelectedValue == "B")
                        sCustomer.CustomerCategory = SEnumCustCategory.B;
                    else if(ddlCustomerCategory.SelectedValue == "C")
                        sCustomer.CustomerCategory = SEnumCustCategory.C;
                    else
                        sCustomer.CustomerCategory = SEnumCustCategory.A;

                    sCustomer.ChiefContact = txtInvoiceName.Text.Trim();
                    sCustomer.IndustryType = OptionDropDownList1.SelectedValue.Trim();
                    sCustomer.LegalForm = txtLegalForm.Text.Trim();
                    sCustomer.AdministratorUserId = KaizosSession.Current.AccountNo;

                    sCustomer.CreatedDate = DateTime.Now;
                    sCustomer.LastUpdate = DateTime.Now;
                    sCustomer.ToSAcceptedDate = DateTime.Now;
                    sCustomer.LastLogin = DateTime.Now;

                    if (txtManPower.Text.Trim().Length != 0)
                        sCustomer.ManPower = Convert.ToInt32(txtManPower.Text.Trim());
                    else
                        sCustomer.ManPower = 0;

                    sCustomer.UsedForShipping = SEnumFlag.No;
                    sCustomer.UsedForReturn = SEnumFlag.No;
                    sCustomer.ShipmentPreference = "";// SEnumShipPreference.MostCompetitive;
                    sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;
                    sCustomer.DeferedPaymentRequired = SEnumFlag.No;
                    sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;
                    sCustomer.CustomerTypeChanged = SEnumFlag.No;
                    if (chkFictiveAcoount.Checked)
                    {
                        sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                    }
                    else
                    {
                        sCustomer.FICTIVE_ACCOUNT = SEnumFlag.No;
                    }
                    //21-FEB-2012 HV
                    sCustomer.InsuredMethod = ddInsuranceMethod.SelectedValue;
                    if(txtPaymentDelay.Text.Trim().Length != 0)
                        sCustomer.PaymentDelayDays = int.Parse(txtPaymentDelay.Text.Trim());
                    else
                        sCustomer.PaymentDelayDays = 0;
                    DateTime firmDate;

                    string format = "dd/MM/yyyy";

                    if (txtFirmDate.Text.Trim().Length != 0)
                        firmDate = DateTime.ParseExact(txtFirmDate.Text, format, CultureInfo.InvariantCulture);
                    else
                        firmDate = DateTime.Now;

                    sCustomer.FirmDate = firmDate;
                    #endregion

                    string CompanyName = txtCompanyName.Text.Trim();
                    string Email = txtEmail.Text.Trim(); 
                    int InsertStatus = proxy.InsertCustomer(sCustomer);
                    if (InsertStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmEndCustomerList.aspx?x=" + CompanyName + "&y=" + Email;
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationSuccess").ToString(), txtEmail.Text.Trim());
                        //commended by 19MAR12SM
                        //if (!proxy.sendMessage(txtEmail.Text.Trim(), strCreatePassword.Trim(), KaizosSession.Current.UserId.Trim()))
                        //{
                        //    KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + "-" + GetGlobalResourceObject("LocalString", "MailSendFailure").ToString();
                        //}
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (InsertStatus == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmCustomer.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                        //lblDBError.Text = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        //ModalPopupExtender1.Show();
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmCustomer.aspx";
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

        private bool IsAlphaNum(string strText)
        {
            if (string.IsNullOrEmpty(strText))
                return false;
            return (strText.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c) || Char.IsPunctuation('.')));
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

        protected void val_Franchise_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool IsDateFailured = true;
            try
            {
               args.IsValid = true;
                string strError = "";
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();

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

                if (txtName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

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
                else if (!txtEmail.Text.Trim().Equals(txtCustomerConfirmationEmail.Text.Trim()))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + ", " + lblCustomerConfirmationEmail.Text.Trim() + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (ddlCountry.SelectedValue.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (txtPhoneNumber.Text.Equals(""))
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
               

                if (txtHQZipcode.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!(proxy.isAlphaNumericValidation(txtHQZipcode.Text.Trim())))
                {
                    strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                } //Validating Zip Code -added by HV [MAR142012]
                else
                {
                    string strActualFormat = string.Empty;
                    strActualFormat = proxy.GetPostalCode(ddlCountry.SelectedValue);
                    if (!ValidateZipCode(strActualFormat))
                    {
                        strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                }
                //Commented By HV [MAR142012]
                //else if (proxy.ValidateHQZipcode(ddlCountry.SelectedValue.Trim()+txtHQZipcode.Text.Trim()) == "2")
                //{
                //    strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}        

                //if (ddlCustomerCategory.SelectedValue.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblCustomerCategory.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                //if (OptionDropDownList1.SelectedValue.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblIndustry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}
                //if (txtInvoiceName.Text.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblInvoiceName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                //if (txtLegalForm.Text.Trim().Length == 0)
                //{
                //    strError = strError + "*" + lblLegalForm.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}

                if (txtManPower.Text.Trim().Length != 0)
                {
                //    strError = strError + "*" + lblManPower.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //    args.IsValid = false;
                //}
                //else
                //{
                    if (!(proxy.isNumericValidation(txtManPower.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblManPower.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }
                if (txtPaymentDelay.Text.Trim().Length != 0)
                {
                    if (!(proxy.isNumericValidation(txtPaymentDelay.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblPaymentDelay.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }
                if (chkKeyAccount.Checked)
                {
                    if (lbCarrier.GetSelectedIndices().Count() == 0)
                    {
                        strError = strError + "*" + lblCarrier.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }
                //if (txtFirmDate.Text.Trim() == "")
                //{

                //    strError = strError + "*" + lblFirmDate.Text + " " + GetGlobalResourceObject("LocalString", "ValidationEmpty").ToString();  //"Date must be filled ";
                //    IsDateFailured = true;
                //    args.IsValid = false;
                //}


                if (!IsDateFailured)  //21feb2012 HV  to fix bug 1201 
                {
                    DateTime value;
                    string format = "dd/MM/yyyy";

                    // to fix bug 1169 [07FEB12RM]
                    //if ((!DateTime.TryParse(txtStartDate.Text, out value)) || (!DateTime.TryParse(txtEndDate.Text, out value) ))
                    //DateTime.TryParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture,DateTimeStyles.None, out value)

                    if (
                            (!DateTime.TryParseExact(txtFirmDate.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                                ||
                            (!DateTime.TryParseExact(txtFirmDate.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                       )
                    {

                        strError = strError + "*" + lblFirmDate.Text + " " + GetGlobalResourceObject("LocalString", "ValidationInvalid").ToString();
                        IsDateFailured = true;
                        args.IsValid = false;
                    }
                }


                if (!IsDateFailured)
                {
                    DateTime startDate;
                    DateTime endDate;

                    string format = "dd/MM/yyyy";

                    startDate = DateTime.ParseExact(txtFirmDate.Text, format, CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(DateTime.Today.ToString(), format, CultureInfo.InvariantCulture);

                    //if (Convert.ToDateTime(txtStartDate.Text) >= Convert.ToDateTime(txtEndDate.Text)) // to fix bug 1169 [07FEB12RM]

                    if (startDate >= endDate)
                    {
                        strError = strError + "*" + lblFirmDate.Text + " " + GetGlobalResourceObject("LocalString", "ValLessThan").ToString() + endDate.ToString();  //"Start date must be less then end date";
                        IsDateFailured = true;
                        args.IsValid = false;
                    }
                }
                if (!(args.IsValid))
                {
                    val_Customer.ErrorMessage = strError;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                args.IsValid = false;
                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);
               

            }
            catch (Exception error)
            {

                args.IsValid = false;
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_Franchise_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void chkKeyAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKeyAccount.Checked)
            {
                lbCarrier.Visible = true;
            }
            else
            {
                lbCarrier.Visible = false;
                foreach (ListItem li in lbCarrier.Items)
                {
                    li.Selected = false;
                }
            }
        }

        protected void btnCloseError_Click(object sender, EventArgs e)
        {
            //ModalPopupExtender1.Hide();
        }
    }
}