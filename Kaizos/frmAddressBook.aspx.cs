using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;



namespace Kaizos
{
    public partial class frmAddressBook : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAddressBook));

        protected void Page_Load(object sender, EventArgs e)
        {
			if(!(IsPostBack))
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAddressBook").ToString();
                try
                {
                    if (KaizosSession.Current.UserType.Trim() != "CS")
                    {
                        StatusChange(false);
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        //To fill Dimension Units drop down list
                        List<SCountryTable> sCountryTable = new List<SCountryTable>();
                        sCountryTable = proxy.FillCountryCombo().ToList();
                        ddlCountry.DataSource = sCountryTable;
                        ddlCountry.DataTextField = "CodeName";
                        ddlCountry.DataValueField = "CountryCode";
                        ddlCountry.DataBind();
                        optResidential.Checked = true;
                        optDeliveryAddress.Checked = true;
                        lblCompanyName.Visible = false;
                        txtCompanyName.Visible = false;

                        // Load the carrier account reference popup details
                        List<string> strCarrierList = null;

                        strCarrierList = proxy.GetAllRefCarrierList().ToList();
                        ddlNamedCarrier.DataSource = strCarrierList.ToArray();
                        ddlNamedCarrier.DataBind();

						errMsg.Attributes["style"] = "display: none;";

                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }

                /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
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
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            int InsertStatus = 1;

            if (IsValid)
            {
                try
                {
                    SAddressBook sAddressBook = new SAddressBook();
                    KaizosServiceContractClient context = new KaizosServiceContractClient();

                    sAddressBook.AccountNo = KaizosSession.Current.AccountNo;
                    sAddressBook.Address1 = txtAddress1.Text.Trim();
                    sAddressBook.Address2 = txtAddress2.Text.Trim();
                    sAddressBook.Address3 = txtAddress3.Text.Trim();

                    if (optCommercial.Checked)
                        sAddressBook.AddressType = SEnumAddressType.Company;
                    else
                        sAddressBook.AddressType = SEnumAddressType.Residential;

                    if (optShippingAddress.Checked)
                        sAddressBook.AddressUsedFor = SEnumDeliveryType.ShippingAddress;
                    else if (optDeliveryAddress.Checked)
                        sAddressBook.AddressUsedFor = SEnumDeliveryType.DeliveryAddress;
                    else
                        sAddressBook.AddressUsedFor = SEnumDeliveryType.Both;

                    sAddressBook.City = txtCity.Text.Trim();
                    sAddressBook.Comments = mtxtComments.Text.Trim();
                    sAddressBook.CompanyName = txtCompanyName.Text.Trim();
                    sAddressBook.Country = ddlCountry.SelectedValue.Trim();
                    sAddressBook.CreatedDate = DateTime.Now;
                    sAddressBook.Email = txtEmail.Text.Trim();
                    sAddressBook.LastPickupMondayToThursday = txtLastPickupMT.Text.Trim();
                    sAddressBook.LastPickupFriday = txtLastPickupF.Text.Trim();
                    sAddressBook.LastUpdated = DateTime.Now;
                    sAddressBook.Name = txtName.Text.Trim();

                    if (optShipPreference1.Checked)
                        sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
                    else if (optShipPreference2.Checked)
                        sAddressBook.ShipPreference = SEnumShipPreference.Fastest;
                    else
                        sAddressBook.ShipPreference = SEnumShipPreference.NamedCarrier;

                    sAddressBook.State = txtState.Text.Trim();
                    sAddressBook.TelephoneNo = txtPhoneNumber.Text.Trim();
                    sAddressBook.ZipCode = txtZipcode.Text.Trim();
                    sAddressBook.FaxNo = txtFaxNumber.Text.Trim();
                    sAddressBook.NamedCarrier = "";//added by HV 16APR2012
                    sAddressBook.ShipPreferenceOrder = "";//added by HV 16APR2012
                    InsertStatus = context.InsertAddressBook(sAddressBook);
                    if (InsertStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmAddressBooklist.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "AddressBookSuccess").ToString(), txtAddress1.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else if (InsertStatus == 2)
                    {
                        //KaizosSession.Current.ReturnURL = "frmAddressBook.aspx";
                        //KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "AddressBookAlreadyExists").ToString(), txtCompanyName.Text.Trim());
                        //Server.Transfer("frmResult.aspx", false);
                        lblMessage.Text = String.Format(GetGlobalResourceObject("LocalString", "AddressBookAlreadyExists").ToString(), txtAddress1.Text.Trim());
                        MdlAddressExist.Show();
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmAddressBook.aspx";
						KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "AddressBookFailure").ToString(), txtAddress1.Text.Trim());
						Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "AddressBookFailure").ToString(), txtAddress1.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [23JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Button1_Click", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "AddressBookFailure").ToString(), txtAddress1.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
        }

        protected void val_AddressBook_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";

            KaizosServiceContractClient context = new KaizosServiceContractClient();
            try
            {
                if (!optResidential.Checked)
                {
                    if (txtCompanyName.Text.Equals(""))
                    {
                        strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else
                    {
                        //if (!(context.isAlphaNumericValidation(txtCompanyName.Text.Trim())))
                        //{
                        //    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        //    args.IsValid = false;
                        //}
                    }
                }

                if (txtName.Text.Equals(""))
                {
                    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                else
                {
                    //if (!(context.isAlphaNumericValidation(txtName.Text.Trim())))
                    //{
                    //    strError = strError + "*" + lblName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    //    args.IsValid = false;
                    //}
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
                else
                { 
                    string strActualFormat = string.Empty;
                    strActualFormat = context.GetPostalCode(ddlCountry.SelectedValue);
                    if (!context.ValidatePostalcode(strActualFormat,txtZipcode.Text.Trim()))
                    {
                        strError = strError + "*" + lblZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtCity.Text.Equals(""))
                {
                    strError = strError + "*" + lblCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    //if (!(context.isAlphaNumericValidation(txtCity.Text.Trim())))
                    //{
                    //    strError = strError + "*" + lblCity.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    //    args.IsValid = false;
                    //}
                }

                if (txtState.Text.Equals(""))
                {
                    //if (!(context.isAlphaNumericValidation(txtState.Text.Trim())))
                    //{
                    strError = strError + "*" + lblState.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                    //}
                }

                if (ddlCountry.Text.Equals(""))
                {
                    strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
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

                if (!txtFaxNumber.Text.Equals(""))
                {
                    if (!(context.isNumericValidation(txtFaxNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblFaxNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtPhoneNumber.Text.Equals(""))
                {
                    strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(context.isNumericValidation(txtPhoneNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    { 
                        strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (txtLastPickupMT.Text.Equals(""))
                {
                    strError = strError + "*" + lblLastPickupMT.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (context.ValidateTime(txtLastPickupMT.Text.Trim())!=0)
                {
                    strError = strError + "*" + lblLastPickupMT.Text.Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtLastPickupF.Text.Equals(""))
                {
                    strError = strError + "*" + lblLastPickupF.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if (context.ValidateTime(txtLastPickupF.Text.Trim()) != 0)
                {
                    strError = strError + "*" + lblLastPickupF.Text.Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                //if (chkEnableShippingPreference.Checked)
                //{
                //    if (optShipPreference3.Checked)
                //    {
                //        if (txtNamedCarrier.Text.Equals(""))
                //        {
                //            strError = strError + "*" + lblNamedCarrier.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                //            args.IsValid = false;
                //        }
                //    }
                //}
				
				if (!(args.IsValid))
				{
					val_AddressBook.ErrorMessage = strError;
					errMsg.Attributes["style"] = "display: block;";
					//errMsg.Visible = true;
				}
				else {
					//errMsg.Visible = false;
				}
            }
                /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookValidationFailure").ToString();
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

        protected void StatusChange(bool bStatus)
        {
            lblShipPreference.Visible = bStatus;
            optShipPreference1.Visible = bStatus;
            optShipPreference2.Visible = bStatus;
            optShipPreference3.Visible = bStatus;
            lblAddressUserFor.Visible = bStatus;
            optShippingAddress.Visible = bStatus;
            optDeliveryAddress.Visible = bStatus;
            optBoth.Visible = bStatus;

            optShipPreference3.Checked = bStatus;
            //txtNamedCarrier.Text = "";
            lblNamedCarrier.Visible = bStatus;
            //txtNamedCarrier.Visible = bStatus;
            ddlNamedCarrier.Visible = bStatus;
            optDeliveryAddress.Checked = bStatus;

            if (bStatus == false)
            {
                optShipPreference1.Checked = bStatus;
                optShipPreference2.Checked = bStatus;
                optShippingAddress.Checked = bStatus;
                optBoth.Checked = bStatus;
            }

        }

        protected void chkEnableShippingPreference_CheckedChanged(object sender, EventArgs e)
        {
            StatusChange(chkEnableShippingPreference.Checked);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            StatusChange(false);
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtCompanyName.Text = "";
            txtEmail.Text = "";
            txtFaxNumber.Text = "";
            txtLastPickupF.Text = "";
            txtLastPickupMT.Text = "";
            txtName.Text = "";
            //txtNamedCarrier.Text = "";
            txtPhoneNumber.Text = "";
            txtState.Text = "";
            txtZipcode.Text = "";
            mtxtComments.Text = "";
            chkEnableShippingPreference.Checked = false;
            lblNamedCarrier.Visible = false;
            //txtNamedCarrier.Visible = false;
            //txtNamedCarrier.Text = "";
            ddlNamedCarrier.Visible = false;
        }

        protected void optShipPreference3_CheckedChanged(object sender, EventArgs e)
        {
            //txtNamedCarrier.Text = "";
            lblNamedCarrier.Visible = true;
            //txtNamedCarrier.Visible = true;
            ddlNamedCarrier.Visible = true;
        }

        protected void optShipPreference1_CheckedChanged(object sender, EventArgs e)
        {
            //txtNamedCarrier.Text = "";
            lblNamedCarrier.Visible = false;
            //txtNamedCarrier.Visible = false;
            ddlNamedCarrier.Visible = false;
        }

        protected void optShipPreference2_CheckedChanged(object sender, EventArgs e)
        {
            //txtNamedCarrier.Text = "";
            lblNamedCarrier.Visible = false;
            //txtNamedCarrier.Visible = false;
            ddlNamedCarrier.Visible = false;
        }

        protected void optResidential_CheckedChanged(object sender, EventArgs e)
        {
            txtCompanyName.Visible = false;
            lblCompanyName.Visible = false;
            txtCompanyName.Text = "";

        }

        protected void optCommercial_CheckedChanged(object sender, EventArgs e)
        {
            txtCompanyName.Visible = true;
            lblCompanyName.Visible = true;
            txtCompanyName.Text = "";
        }
    }
}