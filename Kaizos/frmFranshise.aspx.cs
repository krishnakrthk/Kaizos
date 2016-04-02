using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

using KaizosServiceInvokers.KaizosServiceReference;
using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

namespace Kaizos
{


    public partial class frmFranshise : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmLogin));

        public class CarrierList
        {
            public string CarrierName{get; set;}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmFranchise").ToString();
                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        //To fill Dimension Units drop down list
                        List<SCountryTable> sCountryTable = new List<SCountryTable>();
                        sCountryTable = proxy.FillCountryCombo().ToList();
                        ddlCountry.DataSource = sCountryTable;
                        ddlCountry.DataTextField = "CodeName";
                        ddlCountry.DataValueField = "CountryCode";
                        ddlCountry.DataBind();

                        ddlLanguage.DataSource = proxy.GetLanguage(ddlCountry.SelectedValue.Trim());
                        ddlLanguage.DataBind();
                        ddlCountry.SelectedValue = "FR";

                        // Load the carrier account reference popup details
                        List<string> strCarrierList = null;
                        List<CarrierList> strCarrierList2 = new List<CarrierList>();

                        strCarrierList = proxy.GetAllRefCarrierList().ToList();

                        for(int i = 0; i<strCarrierList.Count();i++)
                        {
                            CarrierList strCarrierList1 = new CarrierList();
                            strCarrierList1.CarrierName = strCarrierList[i].ToString();
                            strCarrierList2.Add(strCarrierList1);
                        }

                        gvCarrierAccountReference.DataSource = strCarrierList2.ToArray();
                        gvCarrierAccountReference.DataBind();
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

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            ddlLanguage.DataSource = proxy.GetLanguage(ddlCountry.SelectedValue.Trim());
            ddlLanguage.DataBind();
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string DateFormat = "dd/MM/yyyy";
            try
            {
                int InsertStatus = 1;
                string[] CarrierName = (string[])ViewState["CarrierName"];
                string[] AccountRef = (string[])ViewState["AccountRef"];
                
                // Added country code as perfix with given each Chalandise zone
                //List<string[]> arrays = new List<string[]>();
                //var primeArray = rtxtChalandiseZone.Text.Split(',');
                //for (int i = 0; i < primeArray.Length; i ++)
                //{
                //    var first = primeArray[i].Trim();
                //    primeArray[i] = first;
                //}

                //rtxtChalandiseZone.Text = String.Join(",", primeArray.ToArray());
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                string Sessoin_UserID = KaizosSession.Current.UserId;
                string Sessoin_UserType = KaizosSession.Current.UserType;
                // Need to call password generation method
                string strCreatePassword = proxy.CreatePassword(8);
                string strPassword = proxy.EncryptString(strCreatePassword, "Password");

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
                    //sFranchise.RegistrationNo = txtProfessionalAccount.Text.Trim();
                    sFranchise.SiretNo = txtSiretNo.Text.Trim();
                    sFranchise.VatNo = txtSiretNo.Text.Trim();
                    sFranchise.ScannedDoc = "";
                    sFranchise.Status = SEnumUserStatus.BeingCreated;
                    sFranchise.TelephoneNo = txtPhoneNumber.Text.Trim();
                    sFranchise.ToSAcceptedDate = DateTime.Now;
                    sFranchise.UserType = SEnumUserType.Franchise;
                    sFranchise.ZipCode = txtZipCode.Text.Trim();
                    sFranchise.FirmCreationDate = DateTime.ParseExact(txtValidDate.Text, DateFormat, CultureInfo.InvariantCulture);
                    sFranchise.CarrierAccountRef = "";
                    sFranchise.PaymentDelay = Convert.ToInt32(txtPaymentDelay.Text.Trim());

                    InsertStatus = proxy.InsertFranchise(sFranchise);
                    InsertStatus = proxy.InsertCarrierAccountRef(CarrierName, AccountRef, txtEmail.Text.Trim());

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
                    else if (InsertStatus == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmFranshise.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(),txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmFranshise.aspx";
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

        protected void val_Franchise_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                args.IsValid = true;
                string strError = "";
                string DateFormat = "dd/MM/yyyy";
                DateTime value;

                if (txtCompanyName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblcompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtCommercialName.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblCommercialName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtManPower.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblManPower.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (!(proxy.isNumericValidation(txtManPower.Text.Trim(), System.Globalization.NumberStyles.Integer))) 
                {
                    strError = strError + "*" + lblManPower.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
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

                if (txtPhoneNumber.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
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

                if (txtZipCode.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblZipCode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
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

                if (ddlLanguage.SelectedValue.Trim().Length == 0)
                {
                    strError = strError + "*" + lblLanguage.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtLegalForm.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblLegalForm.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (rtxtChalandiseZone.Text.Trim().Length == 0)
                {
                    strError = strError + "*" + lblChalandiseZone.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (rtxtChalandiseZone.Text.Trim().Length > 5000)
                {
                    strError = strError + "*" + lblChalandiseZone.Text.Trim() + " " + valMaxAllowed.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    var primeArray = rtxtChalandiseZone.Text.Split(',');
                    for (int i = 0; i < primeArray.Length; i++)
                    {
                        var first = primeArray[i].Trim();
                        if (proxy.ValidateHQZipcode(first.Trim()) == "2")
                        {
                            strError = strError + "*" + lblChalandiseZone.Text.Trim()+ "-" + first.Trim() + " " + valAlready.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }

                }
                if (txtValidDate.Text.Trim().Length != 0)
                {
                    if (!DateTime.TryParseExact(txtValidDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                    {
                        strError = strError + "*" + lblFirmCreateDate.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
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

                if (!(args.IsValid))
                {
                    val_Franchise.ErrorMessage = strError;
                }
                //Need to validate Telephone number, zipcode format
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
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_Franchise_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserCreationFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtCommercialName.Text = "";
            txtCompanyName.Text = "Kaizos";
            txtEmail.Text = "";
            txtFaxNumber.Text = "";
            txtLegalForm.Text = "";
            txtManPower.Text = "";
            txtName.Text = "";
            txtPhoneNumber.Text = "";
            txtVatNo.Text = "";
            txtSiretNo.Text = "";
            txtZipCode.Text = "";
            ddlCountry.SelectedValue = "FR";
        }

        protected void btnCarrierAccount_Click(object sender, EventArgs e)
        {
        }

        protected void gvCarrierAccountReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnPPSubmit_Click(object sender, EventArgs e)
        {
            string[] CarrierName = new string[gvCarrierAccountReference.Rows.Count];
            string[] AccountRef = new string[gvCarrierAccountReference.Rows.Count];

              if (gvCarrierAccountReference.Rows.Count > 0)
              {
                  for (int i = 0; i < gvCarrierAccountReference.Rows.Count; i++)
                  {
                      CarrierName[i] = ((Label)gvCarrierAccountReference.Rows[i].FindControl("lblCarrierName")).Text;
                      AccountRef[i] = ((TextBox)gvCarrierAccountReference.Rows[i].FindControl("txtAccountReference")).Text;
                  }
                  ViewState["CarrierName"] = CarrierName;
                  ViewState["AccountRef"] = AccountRef;
              }
        }
    }
}