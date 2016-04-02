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
    public partial class frmEndCustomerUpdate : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmEndCustomerUpdate));

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes.Add("Value", txtPassword.Text);
            txtConfirmPassword.Attributes.Add("Value", txtConfirmPassword.Text);
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmEndCustomerUpdate").ToString();

                #region FillGeneralCombo
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                List<SComboText> sComboText = new List<SComboText>();
                SComboTableField sComboTableField = new SComboTableField();
                sComboTableField.FieldName = "COMPANY_NAME";
                sComboTableField.TableName = "FRANCHISE";
                sComboText = proxy.FillCombo(sComboTableField).ToList();
                //how to associate a commercial attache
                ddlCommercialAttach.DataSource = sComboText;
                ddlCommercialAttach.DataTextField = "ComboText";
                ddlCommercialAttach.DataBind();

                sComboTableField.FieldName = "DEFERED_PAYMENT_TYPE";
                sComboTableField.TableName = "DEFERRED_PAYMENT_TYPE";
                sComboText = proxy.FillCombo(sComboTableField).ToList();
                ddlDPType.DataSource = sComboText;
                ddlDPType.DataTextField = "ComboText";
                ddlDPType.DataBind();

                sComboTableField.FieldName = "INS_METHOD";
                sComboTableField.TableName = "INSURANCE_METHOD";
                sComboText = proxy.FillCombo(sComboTableField).ToList();
                ddlShipInsuredMethod.DataSource = sComboText;
                ddlShipInsuredMethod.DataTextField = "ComboText";
                ddlShipInsuredMethod.DataBind();

                sComboTableField.FieldName = "CARRIER_NAME";
                sComboTableField.TableName = "CUST_CARRIER_MAST";
                sComboText = proxy.FillCombo(sComboTableField).ToList();
                //ddlForCarrier.DataSource = sComboText;
                //ddlForCarrier.DataTextField = "ComboText";
                //ddlForCarrier.DataBind();

                //added by HV for bug 1108
                List<SCarrier> lstCarrier = proxy.GetCarriers().ToList();
                lbCarrier.DataSource = lstCarrier;
                lbCarrier.DataTextField = "CarrierName";
                // ddKeyCarrier.DataValueField = "CarrierName";
                lbCarrier.DataBind();
                lbCarrier.Visible = false;
                txtSubcription.Visible = false;
                ddKeyCarrier.Visible = false;
                //List<SCarrier> lstCarrier = proxy.GetCarriers().ToList();
                ddlShipNamedCarrier.DataSource = lstCarrier;
                ddlShipNamedCarrier.DataTextField = "CarrierName";
                //ddlShipNamedCarrier.DataValueField = "CarrierName";
                ddlShipNamedCarrier.DataBind();

                List<SCountryTable> sCountryTable = new List<SCountryTable>();
                sCountryTable = proxy.FillCountryCombo().ToList();

                ddlCountry.DataSource = sCountryTable;
                ddlCountry.DataTextField = "CodeName";
                ddlCountry.DataValueField = "CountryCode";
                ddlCountry.DataBind();

                List<SIndustry> sIndustry = new List<SIndustry>();
                sIndustry = proxy.GetIndustry().ToList();

                ddlIndustry.DataGroupField = "Department";
                ddlIndustry.DataTextField = "Activity";
                ddlIndustry.DataValueField = "Activity";
                ddlIndustry.DataSource = sIndustry;
                ddlIndustry.DataBind();
                //ddlShipNamedCarrier.DataSource = sComboText;
                //ddlShipNamedCarrier.DataTextField = "ComboText";
                //ddlShipNamedCarrier.DataBind();
                #endregion
                txtEmail.Text = KaizosSession.Current.Email.Trim();
                txtEmail.Enabled = false;
                btnGet.Visible = false;

                Display();
                chkKeyAccount.Checked = false;
                lblSubscription.Visible = false;
                if (txtEmail.Text.Trim().Length != 0)
                    btnGet_Click(sender, e);


             

            }
        }

        private void Display()
        {
            #region  Dispaly details
            try
            {
               
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                List<SComboText> sComboText = new List<SComboText>();
                SComboTableField sComboTableField = new SComboTableField();

                SToS sTos = new SToS();
                sTos = proxy.GetActiveToS();
                lblMessage.Text = sTos.TermsOfService.Trim();

                if (KaizosSession.Current.UserType != "CS")
                {
                    #region Fill combos
                    //To fill Dimension Units drop down list
                    //List<SCountryTable> sCountryTable = new List<SCountryTable>();
                    //sCountryTable = proxy.FillCountryCombo().ToList();

                    //ddlCountry.DataSource = sCountryTable;
                    //ddlCountry.DataTextField = "CodeName";
                    //ddlCountry.DataValueField = "CountryCode";
                    //ddlCountry.DataBind();
                    trSelectShippingReference.Visible = false;
                    //ddlShipPreference.Items.Add("Fastest");
                    //ddlShipPreference.Items.Add("MostCompetitive");
                    //ddlShipPreference.Items.Add("NamedCarrier");

                    //List<string> FunctionList = proxy.GetFunction().ToList();

                 
                    //ddlIndustry.DataSource = FunctionList;
                    //ddlIndustry.DataBind();
                    rlShippingPreference.EnableViewState = true;
                    ddlCustomerCategory.Items.Add("");
                    ddlCustomerCategory.Items.Add("A");
                    ddlCustomerCategory.Items.Add("B");
                    ddlCustomerCategory.Items.Add("C");

                    if (KaizosSession.Current.UserType == "AD" || KaizosSession.Current.UserType == "FR")
                    {
                        sComboTableField.FieldName = "CARRIER_NAME";
                        sComboTableField.TableName = "CUST_CARRIER_MAST";
                        sComboText = proxy.FillCombo(sComboTableField).ToList();

                        #region Pallet Economy Carrier Combos
                        ddlPalletEconomyCarrierEup.DataSource = sComboText;
                        ddlPalletEconomyCarrierEup.DataTextField = "ComboText";
                        ddlPalletEconomyCarrierEup.DataBind();

                        ddlPalletEconomyCarrierFr.DataSource = sComboText;
                        ddlPalletEconomyCarrierFr.DataTextField = "ComboText";
                        ddlPalletEconomyCarrierFr.DataBind();

                        ddlPalletEconomyCarrierInt.DataSource = sComboText;
                        ddlPalletEconomyCarrierInt.DataTextField = "ComboText";
                        ddlPalletEconomyCarrierInt.DataBind();
                        #endregion

                        #region Pallet Express Carrier Combos
                        ddlPalletExpressCarrierFr.DataSource = sComboText;
                        ddlPalletExpressCarrierFr.DataTextField = "ComboText";
                        ddlPalletExpressCarrierFr.DataBind();

                        ddlPalletExpressCarrierEup.DataSource = sComboText;
                        ddlPalletExpressCarrierEup.DataTextField = "ComboText";
                        ddlPalletExpressCarrierEup.DataBind();

                        ddlPalletExpressCarrierInt.DataSource = sComboText;
                        ddlPalletExpressCarrierInt.DataTextField = "ComboText";
                        ddlPalletExpressCarrierInt.DataBind();
                        #endregion

                        #region Parcel Express Carrier Combos
                        ddlParcelCarrierFr.DataSource = sComboText;
                        ddlParcelCarrierFr.DataTextField = "ComboText";
                        ddlParcelCarrierFr.DataBind();

                        ddlParcelCarrierEup.DataSource = sComboText;
                        ddlParcelCarrierEup.DataTextField = "ComboText";
                        ddlParcelCarrierEup.DataBind();

                        ddlParcelCarrierInt.DataSource = sComboText;
                        ddlParcelCarrierInt.DataTextField = "ComboText";
                        ddlParcelCarrierInt.DataBind();
                        #endregion

                        #region Parcel Economy Carrier Combos
                        ddlParcelEconomyCarrierFr.DataSource = sComboText;
                        ddlParcelEconomyCarrierFr.DataTextField = "ComboText";
                        ddlParcelEconomyCarrierFr.DataBind();

                        ddlParcelEconomyCarrierEup.DataSource = sComboText;
                        ddlParcelEconomyCarrierEup.DataTextField = "ComboText";
                        ddlParcelEconomyCarrierEup.DataBind();

                        ddlParcelEconomyCarrierInt.DataSource = sComboText;
                        ddlParcelEconomyCarrierInt.DataTextField = "ComboText";
                        ddlParcelEconomyCarrierInt.DataBind();
                        #endregion

                    }

                    #endregion

                    #region Authorize settings
                    if (KaizosSession.Current.UserType.Trim() == "AZ")
                    {

                        #region AZ user Disable settings

                        optStatusEnable.Enabled = false; //az,rf
                        optStatusDisable.Enabled = false;//az,rf
                        optStatusArchieve.Enabled = false;//az,rf
                        optCreditCard.Enabled = false;
                        optDeferdPayment.Enabled = false;
                        txtCommercialAttach.Enabled = false; //az,N2,RF,
                        txtHQZipcode.Enabled = false; //az,N2

                        txtCompanyName.Enabled = false;//az                           
                        //txtContact.Enabled = false; //az 
                        txtPhoneNumber.Enabled = false;//az
                        txtFaxNo.Enabled = false;
                        txtAddress1.Enabled = false; //az
                        txtAddress2.Enabled = false;//az
                        txtAddress3.Enabled = false; //az
                        txtCity.Enabled = false; //az
                        txtZipcode.Enabled = false;//az,rf
                        ddlCountry.Enabled = false;//az

                        lblDPType.Enabled = false;
                        ddlDPType.Enabled = false;
                        txtDepositAmount.Enabled = false;
                        lblDepositAmount.Enabled = false;
                        txtTranportBudget.Enabled = false;
                        lblTransportBudget.Enabled = false;

                        lblSubscription.Enabled = false;
                        txtSubcription.Enabled = false;

                        #endregion

                        #region AZ User Visible Settings

                        lblName.Enabled = false;
                        lblName.Visible = false;
                        txtName.Visible = false;
                        txtName.Enabled = false;
                        txtContact.Visible = false; //az 
                        lblContact.Visible = false;
                        lblCustomerCategory.Enabled = false;
                        lblCustomerCategory.Visible = false;
                        ddlCustomerCategory.Visible = false;
                        ddlCustomerCategory.Enabled = false;
                        ddlIndustry.Visible = false;
                        lblIndustry.Visible = false;
                        ddlIndustry.Enabled = false;
                        lblIndustry.Enabled = false;
                        lblLegalForm.Visible = false;
                        txtLegalForm.Visible = false;
                        lblLegalForm.Enabled = false;
                        txtLegalForm.Enabled = false;
                        lblManPower.Visible = false;
                        txtManPower.Visible = false;
                        lblManPower.Enabled = false;
                        txtManPower.Enabled = false;
                        //Fieldset6.Visible = false;
                        //Fieldset7.Visible = false;
                        lblTurnover.Visible = false;
                        txtTurnover.Visible = false;
                        lblGrossMargin.Visible = false;
                        txtGrossMargin.Visible = false;
                        lblADV.Visible = false;
                        txtADV.Visible = false;
                        lblCurrentPaymentMethod.Visible = false;
                        optCreditCard.Visible = false;
                        optDeferdPayment.Visible = false;
                        txtPaymentDelay.Visible = false;
                        lblPaymentDelay.Visible = false;
                        lblKeyAccount.Visible = false;
                        chkKeyAccount.Visible = false;
                        lblForCarrier.Visible = false;
                        lbCarrier.Visible = false;
                        chkKeyAccount.Visible = false;
                        lblSubscription.Visible = false;
                        txtSubcription.Visible = false;
                        Fieldset5.Visible = false;
                        //Fieldset6.Visible = false;
                        //Fieldset7.Visible = false;
                        lblDPType.Visible = false;
                        ddlDPType.Visible = false;
                        #endregion

                        //Insurance method is If the option is currently in "Systematic", not enable for :  Authorized user  Referent user
                        chkReqestDP.Enabled = false;
                       

                    }
                    #endregion

                    #region Referent settings
                    if (KaizosSession.Current.UserType.Trim() == "RF")
                    {

                        #region RF user Field Disable settings
                        optStatusEnable.Enabled = false;
                        optStatusDisable.Enabled = false;
                        optStatusArchieve.Enabled = false;

                        txtCommercialAttach.Enabled = false;
                        txtHQZipcode.Enabled = false;
                        // txtZipcode.Enabled = false;
                        lblDPType.Enabled = false;
                        ddlDPType.Enabled = false;
                        txtPaymentDelay.Enabled = false;
                        lblPaymentDelay.Enabled = false;
                        lblKeyAccount.Enabled = false;
                        chkKeyAccount.Enabled = false;
                        lblForCarrier.Enabled = false;
                        chkKeyAccount.Enabled = false;
                        lblSubscription.Enabled = false;
                        txtSubcription.Enabled = false;

                        lblTurnover.Visible = false;
                        txtTurnover.Visible = false;
                        lblGrossMargin.Visible = false;
                        txtGrossMargin.Visible = false;
                        lblADV.Visible = false;
                        txtADV.Visible = false;
                        #endregion

                        #region RF User Visible Settings
                        optCreditCard.Visible = false;
                        optDeferdPayment.Visible = false;
                        lblName.Enabled = false;
                        lblName.Visible = false;
                        txtName.Visible = false;
                        txtName.Enabled = false;
                        lblCustomerCategory.Enabled = false;
                        lblCustomerCategory.Visible = false;
                        ddlCustomerCategory.Visible = false;
                        ddlCustomerCategory.Enabled = false;
                        ddlIndustry.Visible = false;
                        lblIndustry.Visible = false;
                        ddlIndustry.Enabled = false;
                        lblIndustry.Enabled = false;
                        lblLegalForm.Visible = false;
                        txtLegalForm.Visible = false;
                        lblLegalForm.Enabled = false;
                        txtLegalForm.Enabled = false;
                        lblManPower.Visible = false;
                        txtManPower.Visible = false;
                        lblManPower.Enabled = false;
                        txtManPower.Enabled = false;
                        lblCurrentPaymentMethod.Visible = false;
                        optCreditCard.Visible = false;
                        optDeferdPayment.Visible = false;
                        txtPaymentDelay.Visible = false;
                        lblPaymentDelay.Visible = false;
                        lblKeyAccount.Visible = false;
                        chkKeyAccount.Visible = false;
                        lblForCarrier.Visible = false;
                        lbCarrier.Visible = false;
                        chkKeyAccount.Visible = false;
                        lblSubscription.Visible = false;
                        txtSubcription.Visible = false;
                        Fieldset5.Visible = false;
                        //Fieldset6.Visible = false;
                        //Fieldset7.Visible = false;
                        lblDPType.Visible = false;
                        ddlDPType.Visible = false;
                        #endregion

                       
                        ////Insurance method is If the option is currently in "Systematic", not enable for :  Authorized user  Referent user
                        //// This should be done at btnget_click method 
                    }
                    #endregion

                    #region Franchise settings
                    if (KaizosSession.Current.UserType.Trim() == "FR")
                    {
                        txtCommercialAttach.Enabled = false;
                        txtHQZipcode.Enabled = false;
                        chkTOS.Enabled = false;
                    }
                    #endregion

                    #region Admin Settings
                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {
                        chkTOS.Enabled = false;
                    }
                    #endregion

                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageUserFetchFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file*/
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnGet_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageUserFetchFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
            #endregion
        }

        protected void val_EndCustomerUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            args.IsValid = true;
            string strError = "";
            decimal d = 0;
            try
            {
                #region Parcel & Pallet Validation
                    // To validate if it is empty

                    #region User
                    if (optStatusArchieve.Visible && optStatusDisable.Visible && optStatusEnable.Visible)
                    {
                        if ((!optStatusArchieve.Checked) && (!optStatusDisable.Checked) && (!optStatusEnable.Checked))
                        {
                            strError = strError + "*" + lblStatus.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }

                    if ((txtPassword.Text.Trim().Length != 0) && (txtConfirmPassword.Text.Trim().Length != 0))
                    {
                        if (txtConfirmPassword.Text.Trim().Trim() != txtPassword.Text.Trim().Trim())
                        {
                            strError = strError + "*" + lblPasswordConfirmation.Text.Trim() + "/" + lblPassword.Text.Trim() + " " + valShouldSame.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        else if (proxy.ValidatePassword(txtConfirmPassword.Text.Trim()) !=0)
                        {
                            strError = strError + "*" + lblPassword.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }

                    }

                    #endregion

                    #region Invoice
                    if (txtHQZipcode.Text.Length == 0 && txtHQZipcode.Visible)
                    {
                        strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtADV.Text.Length == 0 && txtADV.Visible)
                    {
                        strError = strError + "*" + lblADV.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtContact.Text.Length == 0 && txtContact.Visible)
                    {
                        strError = strError + "*" + lblContact.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtPhoneNumber.Text.Length == 0 && txtPhoneNumber.Visible)
                    {
                        strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtSiretNo.Text.Length == 0 && txtSiretNo.Visible)
                    {
                        strError = strError + "*" + lblSiretNo.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtAddress1.Text.Length == 0 && txtAddress1.Visible)
                    {
                        strError = strError + "*" + lblAddress1.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    //if (txtAddress2.Text.Length == 0 && txtAddress2.Visible)
                    //{
                    //    strError = strError + "*" + lblAddress2.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    //    args.IsValid = false;
                    //}

                    //if (txtAddress3.Text.Length == 0 && txtAddress3.Visible)
                    //{
                    //    strError = strError + "*" + lblAddress3.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    //    args.IsValid = false;
                    //}

                    if (txtZipcode.Text.Length == 0 && txtZipcode.Visible)
                    {
                        strError = strError + "*" + lblZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (txtCity.Text.Length == 0 && txtCity.Visible)
                    {
                        strError = strError + "*" + lblCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                    if (ddlCountry.Text.Length == 0 && ddlCountry.Visible)
                    {
                        strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                        args.IsValid = false;
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
                    }
                    else if (proxy.ValidateHQZipcode(ddlCountry.SelectedValue.ToString()+txtHQZipcode.Text.Trim()) == "2")
                    {
                        strError = strError + "*" + lblHQZipcode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    #endregion
                   
                    #region Payment Method

                        if ((!optCreditCard.Checked) && (!optDeferdPayment.Checked))
                        {
                            strError = strError + "*" + lblPaymentMethod.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if(chkReqestDP.Checked)
                        {
                                if (txtDepositAmount.Text.Trim().Length != 0)
                                {
                                    if (!proxy.isNumericValidation(txtDepositAmount.Text.Trim(), System.Globalization.NumberStyles.Float))
                                    {
                                        strError = strError + "*" + lblDepositAmount.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                        args.IsValid = false;
                                    }
                                    if (args.IsValid)
                                    {
                                        if (!(decimal.Parse(txtDepositAmount.Text.Trim()) >= 0))
                                        {
                                            strError = strError + "*" + lblDepositAmount.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                            args.IsValid = false;
                                        }
                                    }
                                }
                                else
                                {
                                     strError = strError + "*" + lblDepositAmount.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                                     args.IsValid = false;

                                }

                      
                                if (txtTranportBudget.Text.Trim().Length != 0)
                                {
                                    if (!proxy.isNumericValidation(txtTranportBudget.Text.Trim(), System.Globalization.NumberStyles.Float))
                                    {
                                        strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                        args.IsValid = false;
                                    }
                                    if (args.IsValid)
                                    {
                                        if (!(decimal.Parse(txtTranportBudget.Text.Trim()) >= 0))
                                        {
                                            strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                            args.IsValid = false;
                                        }
                                    }
                                }
                               else
                                {
                                     strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                                     args.IsValid = false;

                                }

                        }

                        if (txtPaymentDelay.Text.Trim().Length != 0)
                        {
                            if (!proxy.isNumericValidation(txtPaymentDelay.Text.Trim(), System.Globalization.NumberStyles.Float))
                            {
                                strError = strError + "*" + lblPaymentDelay.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                        }
                       else
                       {
                            strError = strError + "*" + lblTransportBudget.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                       }
                    
              #endregion

                    #region Key Account

                    if (chkKeyAccount.Visible)
                    {
                        if ((chkKeyAccount.Checked) && (txtSubcription.Text.Trim().Length == 0))
                        {
                            strError = strError + "*" + lblSubscription.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }

                    if (txtSubcription.Text.Trim().Length != 0)
                    {
                        if (!proxy.isNumericValidation(txtSubcription.Text.Trim(), System.Globalization.NumberStyles.Float))
                        {
                            strError = strError + "*" + lblSubscription.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (args.IsValid)
                        {
                            if (!(decimal.Parse(txtSubcription.Text.Trim()) >= 0))
                            {
                                strError = strError + "*" + lblSubscription.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                        }
                    }
                    if (ddKeyCarrier.SelectedValue == null)
                    {
                        strError = strError + "*" + lblForCarrier.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;

                    }
                    #endregion

                    #region TOS
                    if (chkTOS.Enabled)
                    {
                        if (!chkTOS.Checked)
                        {
                            strError = strError + "*" + chkTOS.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }


                    #endregion

                    if (KaizosSession.Current.UserType == "AD" || KaizosSession.Current.UserType == "FR")
                    {
                        #region Parcel Express
                        //France
                        //if (Isfill(txtParcelADVFr.Text.Trim(), txtParcelWPPFr.Text.Trim(), txtParcelCarrierFr.Text.Trim(), txtParcelDiscountFr.Text.Trim()))
                        if (Isfill(txtParcelADVFr.Text.Trim(), txtParcelWPPFr.Text.Trim(), txtParcelDiscountFr.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelExpress.Text.Trim() + "-" + lblParcelFr.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelDiscountFr.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelDiscountFr.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelFr.Text.Trim() + "-" + lblParcelDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelDiscountFr.Text) > 100 && (!txtParcelDiscountFr.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelFr.Text.Trim() + "-" + lblParcelDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //Europe
                        //if (Isfill(txtParcelADVEup.Text.Trim(), txtParcelWPPEup.Text.Trim(), txtParcelCarrierEup.Text.Trim(), txtParcelDiscountEup.Text.Trim()))
                        if (Isfill(txtParcelADVEup.Text.Trim(), txtParcelWPPEup.Text.Trim(), txtParcelDiscountEup.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelExpress.Text.Trim() + "-" + lblParcelEup.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelDiscountEup.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelDiscountEup.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelEup.Text.Trim() + "-" + lblParcelDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelDiscountEup.Text) > 100 && (!txtParcelDiscountEup.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelEup.Text.Trim() + "-" + lblParcelDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //International
                        //if (Isfill(txtParcelADVInt.Text.Trim(), txtParcelWPPInt.Text.Trim(), txtParcelCarrierInt.Text.Trim(), txtParcelDiscountInt.Text.Trim()))
                        if (Isfill(txtParcelADVInt.Text.Trim(), txtParcelWPPInt.Text.Trim(), txtParcelDiscountInt.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelExpress.Text.Trim() + "-" + lblParcelInt.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelDiscountInt.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelDiscountInt.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcel.Text.Trim() + "-" + lblParcelInt + "-" + lblParcelDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelDiscountInt.Text) > 100 && (!txtParcelDiscountInt.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelInt + "-" + lblParcelDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        #endregion

                        #region Parcel Economy
                        //France
                        //if (Isfill(txtParcelEconomyAdvFr.Text.Trim(), txtParcelEconomyWppFr.Text.Trim(), txtParcelEconomyCarrierFr.Text.Trim(), txtParcelEconomyDiscountFr.Text.Trim()))
                        if (Isfill(txtParcelEconomyAdvFr.Text.Trim(), txtParcelEconomyWppFr.Text.Trim(), txtParcelEconomyDiscountFr.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelEconomy.Text.Trim() + "-" + lblPalletFr.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelEconomyDiscountFr.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelEconomyDiscountFr.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelEconomyDiscountFr.Text) > 100 && (!txtParcelEconomyDiscountFr.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //Europe
                        //if (Isfill(txtParcelEconomyAdvFr.Text.Trim(), txtParcelEconomyWppFr.Text.Trim(), txtParcelEconomyCarrierFr.Text.Trim(), txtParcelEconomyDiscountFr.Text.Trim()))
                        if (Isfill(txtParcelEconomyAdvFr.Text.Trim(), txtParcelEconomyWppFr.Text.Trim(), txtParcelEconomyDiscountFr.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelEconomy.Text.Trim() + "-" + lblPalletEup.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelEconomyDiscountEup.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelEconomyDiscountEup.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelEconomyDiscountEup.Text) > 100 && (!txtParcelEconomyDiscountEup.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //International
                        //if (Isfill(txtParcelEconomyAdvInt.Text.Trim(), txtParcelEconomyWppInt.Text.Trim(), txtParcelCarrierInt.Text.Trim(), txtParcelEconomyDiscountInt.Text.Trim()))
                        if (Isfill(txtParcelEconomyAdvInt.Text.Trim(), txtParcelEconomyWppInt.Text.Trim(), txtParcelEconomyDiscountInt.Text.Trim()))
                        {
                            strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelEconomy.Text.Trim() + lblParcelInt.Text.Trim() + "-" + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtParcelEconomyDiscountInt.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtParcelEconomyDiscountInt.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelInt.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtParcelEconomyDiscountInt.Text) > 100 && (!txtParcelEconomyDiscountInt.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblParcel.Text.Trim() + "-" + lblParcelInt.Text.Trim() + "-" + lblParcelEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }
                        #endregion

                        #region Pallet Express
                        //France
                        //if (Isfill(txtPalletExpressADVFr.Text.Trim(), txtPalletExpressWppFr.Text.Trim(), txtPalletExpressCarrierFr.Text.Trim(), txtPalletExpressDiscountFr.Text.Trim()))
                        if (Isfill(txtPalletExpressADVFr.Text.Trim(), txtPalletExpressWppFr.Text.Trim(), txtPalletExpressDiscountFr.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletExpress.Text.Trim() + "-" + lblPalletFr.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletExpressDiscountFr.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletExpressDiscountFr.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + lblPalletExpressDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletExpressDiscountFr.Text) > 100 && (!txtPalletExpressDiscountFr.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + txtPalletExpressDiscountFr.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //Europe
                        //if (Isfill(txtPalletExpressADVEup.Text.Trim(), txtPalletExpressWppEup.Text.Trim(), txtPalletExpressCarrierEup.Text.Trim(), txtPalletExpressDiscountEup.Text.Trim()))
                        if (Isfill(txtPalletExpressADVEup.Text.Trim(), txtPalletExpressWppEup.Text.Trim(), txtPalletExpressDiscountEup.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletExpress.Text.Trim() + "-" + lblPalletEup.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletExpressDiscountEup.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletExpressDiscountEup.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblPalletExpressDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletExpressDiscountEup.Text) > 100 && (!txtPalletExpressDiscountEup.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblPalletExpressDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //International
                        //if (Isfill(txtPalletExpressADVInt.Text.Trim(), txtPalletExpressWppInt.Text.Trim(), txtPalletExpressCarrierInt.Text.Trim(), txtPalletExpressDiscountInt.Text.Trim()))
                        if (Isfill(txtPalletExpressADVInt.Text.Trim(), txtPalletExpressWppInt.Text.Trim(), txtPalletExpressDiscountInt.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletExpress.Text.Trim() + "-" + lblPalletIn.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletExpressDiscountInt.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletExpressDiscountInt.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletIn.Text.Trim() + "-" + lblPalletExpressDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletExpressDiscountInt.Text) > 100 && (!txtPalletExpressDiscountInt.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletIn.Text.Trim() + "-" + lblPalletExpressDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        #endregion

                        #region Pallet Economy
                        //France
                        //if (Isfill(txtPalletEconomyAdvFr.Text.Trim(), txtPalletEconomyWppFr.Text.Trim(), txtPalletEconomyCarrierFr.Text.Trim(), txtPalletEconomyDiscountFr.Text.Trim()))
                        if (Isfill(txtPalletEconomyAdvFr.Text.Trim(), txtPalletEconomyWppFr.Text.Trim(), txtPalletEconomyDiscountFr.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEconomy.Text.Trim() + "-" + lblPalletFr.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletEconomyDiscountFr.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletEconomyDiscountFr.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletEconomyDiscountFr.Text) > 100 && (!txtPalletEconomyDiscountFr.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletFr.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //Europe
                        //if (Isfill(txtPalletEconomyAdvEup.Text.Trim(), txtPalletEconomyWppEup.Text.Trim(), txtPalletEconomyCarrierEup.Text.Trim(), txtPalletEconomyDiscountEup.Text.Trim()))
                        if (Isfill(txtPalletEconomyAdvEup.Text.Trim(), txtPalletEconomyWppEup.Text.Trim(), txtPalletEconomyDiscountEup.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEconomy.Text.Trim() + "-" + lblPalletEup.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletEconomyDiscountEup.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletEconomyDiscountEup.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletEconomyDiscountEup.Text) > 100 && (!txtPalletEconomyDiscountEup.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEup.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }

                        //International
                        //if (Isfill(txtPalletEconomyAdvInt.Text.Trim(), txtPalletEconomyWppInt.Text.Trim(), txtPalletEconomyCarrierInt.Text.Trim(), txtPalletEconomyDiscountInt.Text.Trim()))
                        if (Isfill(txtPalletEconomyAdvInt.Text.Trim(), txtPalletEconomyWppInt.Text.Trim(), txtPalletEconomyDiscountInt.Text.Trim()))
                        {
                            strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletEconomy.Text.Trim() + "-" + lblPalletIn.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                        if (txtPalletEconomyDiscountInt.Text.Trim().Length != 0)
                        {
                            if ((!Decimal.TryParse(txtPalletEconomyDiscountInt.Text.Trim(), out d)))
                            {
                                strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletIn.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                                args.IsValid = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(txtPalletEconomyDiscountInt.Text) > 100 && (!txtPalletEconomyDiscountInt.Text.Equals("")))
                                {
                                    strError = strError + "*" + lblPallet.Text.Trim() + "-" + lblPalletIn.Text.Trim() + "-" + lblPalletEconomyDiscount.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                                    args.IsValid = false;
                                }
                            }
                        }
                        #endregion
                    }
                #endregion

                if (!(args.IsValid))
                {
                    val_EndCustomerUpdate.ErrorMessage = strError.Trim();
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
/*
        protected bool Isfill(string ADV, string WPP, string Carrier, string Discount)
        {
            if (ADV.Trim().Length == 0 && WPP.Trim().Length == 0 && Carrier.Trim().Length == 0 && Discount.Trim().Length == 0)
                return false;
            else if (ADV.Trim().Length != 0 && WPP.Trim().Length != 0 && Carrier.Trim().Length != 0 && Discount.Trim().Length != 0)
                return false;
            else
                return true;
        }
        */
        protected bool Isfill(string ADV, string WPP, string Discount)
        {
            if (ADV.Trim().Length == 0 && WPP.Trim().Length == 0 && Discount.Trim().Length == 0)
                return false;
            else if (ADV.Trim().Length != 0 && WPP.Trim().Length != 0 && Discount.Trim().Length != 0)
                return false;
            else
                return true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    string Sessoin_UserID = KaizosSession.Current.UserId;
                    string Sessoin_UserType = KaizosSession.Current.UserType;
                    string strCreatePassword = string.Empty;
                    string strPassword = string.Empty;

                    #region UpdateCustomer

                    SCustomer sCustomer = new SCustomer();
                    sCustomer = proxy.GetCustomer(KaizosSession.Current.Email.Trim());

                    #region Admin settings
                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {
                        #region SCustomer Fill Area
                        if (!txtPassword.Text.Trim().Equals(string.Empty))
                        {
                           strCreatePassword = txtConfirmPassword.Text.Trim();
                           strPassword = proxy.EncryptString(strCreatePassword, "Password");
                           if (txtConfirmPassword.Text.Trim().Length != 0) //09APR12SM - Bug id : 1343 
                               sCustomer.Password = strPassword.Trim();

                        }
                        // sCustomer.AccountNo = "";
                        sCustomer.Email = txtEmail.Text.Trim();
                       
                        sCustomer.UserType = sCustomer.UserType;
                        sCustomer.Status = sCustomer.Status;
                        sCustomer.IsSalesTarrifAssigned = sCustomer.IsSalesTarrifAssigned;
                        sCustomer.IsToSAccepted = sCustomer.IsToSAccepted;
                        sCustomer.Language = KaizosSession.Current.UserLanguage.Trim();
                        sCustomer.IsChangePasswordRequired = sCustomer.IsChangePasswordRequired;
                        sCustomer.CreatedBy = Sessoin_UserID.Trim();

                        sCustomer.HqZipcode = ddlCountry.SelectedValue + txtHQZipcode.Text.Trim().Substring(2, txtHQZipcode.Text.Trim().Length - 2);
                        sCustomer.CompanyName = txtCompanyName.Text.Trim();
                        sCustomer.TurnOver = Convert.ToDecimal(txtTurnover.Text.Trim());
                        sCustomer.GrossMargin = Convert.ToDecimal(txtGrossMargin.Text.Trim());
                        sCustomer.ADV = Convert.ToInt32(txtADV.Text.Trim());

                        if (ddlCustomerCategory.SelectedValue == "A")
                            sCustomer.CustomerCategory = SEnumCustCategory.A;
                        else if (ddlCustomerCategory.SelectedValue == "B")
                            sCustomer.CustomerCategory = SEnumCustCategory.B;
                        else if (ddlCustomerCategory.SelectedValue == "C")
                            sCustomer.CustomerCategory = SEnumCustCategory.C;

                        sCustomer.Name = txtName.Text.Trim();
                        sCustomer.IndustryType = ddlIndustry.SelectedValue.Trim();
                        sCustomer.LegalForm = txtLegalForm.Text.Trim();
                        sCustomer.ManPower = Convert.ToInt32(txtManPower.Text.Trim());
                        sCustomer.ChiefContact = txtContact.Text.Trim();
                        sCustomer.InvoicePhoneNumber = txtPhoneNumber.Text.Trim();
                        sCustomer.InvoiceFaxNo = txtFaxNo.Text.Trim();
                        sCustomer.VatNo = txtVatNo.Text.Trim();
                        sCustomer.SiretNo = txtSiretNo.Text.Trim();
                        sCustomer.InvoiceAddress1 = txtAddress1.Text.Trim();
                        sCustomer.InvoiceAddress2 = txtAddress2.Text.Trim();
                        sCustomer.InvoiceAddress3 = txtAddress3.Text.Trim();
                        sCustomer.InvoiceZipcode = txtZipcode.Text.Trim();
                        sCustomer.InvoiceCity = txtCity.Text.Trim();
                        sCustomer.InvoiceCountry = ddlCountry.SelectedValue;


                        sCustomer.TelephoneNo = txtPhoneNumber.Text.Trim();
                        sCustomer.Country = ddlCountry.SelectedValue.Trim();

                        if (chkInvoiceAddress.Checked)
                            sCustomer.UsedForShipping = SEnumFlag.Yes;
                        else
                            sCustomer.UsedForShipping = SEnumFlag.No;
                        sCustomer.KEY_CARRIER = string.Empty;
                        if (chkKeyAccount.Checked)
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.Yes;
                            sCustomer.CustomerType = SEnumCustomerType.KeyCustomer;
                            ////added by HV for bug 1108
                            sCustomer.KEY_CARRIER = ddKeyCarrier.SelectedValue;
                            //foreach (ListItem li in lbCarrier.Items)
                            //{
                            //    if (li.Selected)
                            //    {
                            //        sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER + li.Value + "|";
                            //    }
                            //}
                            //sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER.Substring(0, sCustomer.KEY_CARRIER.Length);
                        }
                        else
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.No;
                            sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                            sCustomer.KEY_CARRIER = string.Empty;

                        }
                        //if (ddlShipPreference.SelectedValue == "Fastest")
                        //    sCustomer.ShipmentPreference = SEnumShipPreference.Fastest;
                        //else if (ddlShipPreference.SelectedValue == "MostCompetitive")
                        //    sCustomer.ShipmentPreference = SEnumShipPreference.MostCompetitive;
                        //else
                        //    sCustomer.ShipmentPreference = SEnumShipPreference.NamedCarrier;
                        string strShippingReference = string.Empty;
                        sCustomer.CarrierName = string.Empty;
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
                        sCustomer.CarrierName = ddlShipNamedCarrier.SelectedValue.Trim();

                        sCustomer.InsuredMethod = ddlShipInsuredMethod.SelectedValue.Trim();

                        if (optCreditCard.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;
                        else if (optDeferdPayment.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;

                        if (chkReqestDP.Checked)
                            sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                        else
                            sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                        sCustomer.DEFERED_PAYMENT_TYPE = ddlDPType.SelectedValue.Trim();
                        if (!txtTranportBudget.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.BudgetAmount = Convert.ToDecimal(txtTranportBudget.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtDepositAmount.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.DepositAmount = Convert.ToDecimal(txtDepositAmount.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtPaymentDelay.Text.Trim().Equals(string.Empty))
                        {
                             sCustomer.PaymentDelayDays = Convert.ToInt32(txtPaymentDelay.Text.Trim());
                        }
                        else
                        {
                            sCustomer.PaymentDelayDays = 0;
                        }

                        if (!txtSubcription.Text.Trim().Equals(string.Empty))
                        {
                           sCustomer.SUBSCRIPTION_AMOUNT  = Convert.ToInt32(txtSubcription.Text.Trim());
                        }
                        else
                        {
                           sCustomer.SUBSCRIPTION_AMOUNT  = 0.0m;
                        }



                       
                        sCustomer.EXTRA_INFO = txtExtraInfo.Text.Trim();
                        if (chkActiveAccount.Checked)
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        else
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        sCustomer.LastUpdate = DateTime.Now;

                        sCustomer.ToSAcceptedDate = sCustomer.ToSAcceptedDate;
                        sCustomer.LastLogin = DateTime.Now;
                        #endregion
                    }
                    #endregion

                    #region N2 settings
                    if (KaizosSession.Current.UserType.Trim() == "FR")
                    {
                        #region SCustomer Fill Area
                        if (!txtPassword.Text.Trim().Equals(string.Empty))
                        {
                            strCreatePassword = txtConfirmPassword.Text.Trim();
                            strPassword = proxy.EncryptString(strCreatePassword, "Password");
                            if (txtConfirmPassword.Text.Trim().Length != 0) //09APR12SM - Bug id : 1343 
                                sCustomer.Password = strPassword.Trim();

                        }
                        // sCustomer.AccountNo = "";
                        #region UserActivityData
                        sCustomer.TurnOver = Convert.ToDecimal(txtTurnover.Text.Trim());
                        sCustomer.GrossMargin = Convert.ToDecimal(txtGrossMargin.Text.Trim());
                        sCustomer.ADV = Convert.ToInt32(txtADV.Text.Trim());
                        #endregion
                        
                        #region Personal Inforamtion
                        sCustomer.Email = txtEmail.Text.Trim(); 
                        if(optStatusEnable.Checked)
                        {
                            sCustomer.Status = SEnumUserStatus.Enabled; 
                        }
                        else if (optStatusDisable.Checked)
                        {
                            sCustomer.Status = SEnumUserStatus.Disabled;
                        }
                        else
                        {
                            sCustomer.Status = SEnumUserStatus.Archived; 
                        }
                        sCustomer.Language = KaizosSession.Current.UserLanguage.Trim();


                        sCustomer.HqZipcode = ddlCountry.SelectedValue + txtHQZipcode.Text.Trim().Substring(2, txtHQZipcode.Text.Trim().Length - 2);
                        sCustomer.CompanyName = txtCompanyName.Text.Trim();
                       
                        if (ddlCustomerCategory.SelectedValue == "A")
                            sCustomer.CustomerCategory = SEnumCustCategory.A;
                        else if (ddlCustomerCategory.SelectedValue == "B")
                            sCustomer.CustomerCategory = SEnumCustCategory.B;
                        else if (ddlCustomerCategory.SelectedValue == "C")
                            sCustomer.CustomerCategory = SEnumCustCategory.C;

                        sCustomer.Name = txtName.Text.Trim();
                        sCustomer.IndustryType = ddlIndustry.SelectedValue.Trim();
                        sCustomer.LegalForm = txtLegalForm.Text.Trim();
                        sCustomer.ManPower = Convert.ToInt32(txtManPower.Text.Trim());
                        sCustomer.ChiefContact = txtContact.Text.Trim();
                        sCustomer.InvoicePhoneNumber = txtPhoneNumber.Text.Trim();
                        sCustomer.InvoiceFaxNo = txtFaxNo.Text.Trim();
                        sCustomer.VatNo = txtVatNo.Text.Trim();
                        sCustomer.SiretNo = txtSiretNo.Text.Trim();
                        sCustomer.InvoiceAddress1 = txtAddress1.Text.Trim();
                        sCustomer.InvoiceAddress2 = txtAddress2.Text.Trim();
                        sCustomer.InvoiceAddress3 = txtAddress3.Text.Trim();
                        sCustomer.InvoiceZipcode = txtZipcode.Text.Trim();
                        sCustomer.InvoiceCity = txtCity.Text.Trim();
                        sCustomer.InvoiceCountry = ddlCountry.SelectedValue;


                        sCustomer.TelephoneNo = txtPhoneNumber.Text.Trim();
                        sCustomer.Country = ddlCountry.SelectedValue.Trim();

                        if (chkInvoiceAddress.Checked)
                            sCustomer.UsedForShipping = SEnumFlag.Yes;
                        else
                            sCustomer.UsedForShipping = SEnumFlag.No;

                        #endregion

                        #region Key Account
                        sCustomer.KEY_CARRIER = string.Empty;
                        if (chkKeyAccount.Checked)
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.Yes;
                            sCustomer.CustomerType = SEnumCustomerType.KeyCustomer;
                            ////added by HV for bug 1108
                            sCustomer.KEY_CARRIER = ddKeyCarrier.SelectedValue;
                            sCustomer.SUBSCRIPTION_AMOUNT = Convert.ToDecimal(txtSubcription.Text.Trim());
                            //foreach (ListItem li in lbCarrier.Items)
                            //{
                            //    if (li.Selected)
                            //    {
                            //        sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER + li.Value + "|";
                            //    }
                            //}
                            //sCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER.Substring(0, sCustomer.KEY_CARRIER.Length);
                        }
                        else
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.No;
                            sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                            sCustomer.KEY_CARRIER = string.Empty;

                        }
                        #endregion

                        #region Shipping Preferances
                        string strShippingReference = string.Empty;
                        sCustomer.CarrierName = string.Empty;
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

                        sCustomer.ShipmentPreference = string.Empty;
                        sCustomer.CarrierName = ddlShipNamedCarrier.SelectedValue.Trim();
                        sCustomer.EXTRA_INFO = txtExtraInfo.Text.Trim();
                        #endregion

                        #region Payment and Insurance Details
                        sCustomer.InsuredMethod = ddlShipInsuredMethod.SelectedValue.Trim();

                        if (optCreditCard.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;
                        else if (optDeferdPayment.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;

                        if (chkReqestDP.Checked)
                            sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                        else
                            sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                        sCustomer.DEFERED_PAYMENT_TYPE = ddlDPType.SelectedValue.Trim();
                        if (!txtTranportBudget.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.BudgetAmount = Convert.ToDecimal(txtTranportBudget.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtDepositAmount.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.DepositAmount = Convert.ToDecimal(txtDepositAmount.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtPaymentDelay.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.PaymentDelayDays = Convert.ToInt32(txtPaymentDelay.Text.Trim());
                        }
                        else
                        {
                            sCustomer.PaymentDelayDays = 0;
                        }

                        if (!txtSubcription.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.SUBSCRIPTION_AMOUNT = Convert.ToInt32(txtSubcription.Text.Trim());
                        }
                        else
                        {
                            sCustomer.SUBSCRIPTION_AMOUNT = 0.0m;
                        }
                        #endregion


                        //sCustomer.KEY_CARRIER = ddlForCarrier.SelectedValue.Trim();

                        #region Account and History information
                        if (chkActiveAccount.Checked)
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        else
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        sCustomer.LastUpdate = DateTime.Now;

                        sCustomer.ToSAcceptedDate = sCustomer.ToSAcceptedDate;
                        sCustomer.LastLogin = DateTime.Now;
                        #endregion


                        #endregion


                    }
                    #endregion

                    #region Referent settings
                    if (KaizosSession.Current.UserType.Trim() == "RF")
                    {
                        #region SCustomer Fill Area

                        #region Password Information
                        if (!txtPassword.Text.Trim().Equals(string.Empty))
                        {
                            strCreatePassword = txtConfirmPassword.Text.Trim();
                            strPassword = proxy.EncryptString(strCreatePassword, "Password");
                            if (txtConfirmPassword.Text.Trim().Length != 0) //09APR12SM - Bug id : 1343 
                                sCustomer.Password = strPassword.Trim();

                        }
                        #endregion

                        #region UserActivityData
                        sCustomer.TurnOver = Convert.ToDecimal(txtTurnover.Text.Trim());
                        sCustomer.GrossMargin = Convert.ToDecimal(txtGrossMargin.Text.Trim());
                        sCustomer.ADV = Convert.ToInt32(txtADV.Text.Trim());
                        #endregion

                        #region Personal Inforamtion

                        sCustomer.Email = txtEmail.Text.Trim();
                        sCustomer.CompanyName = txtCompanyName.Text.Trim();
                        sCustomer.Name = txtName.Text.Trim();
                      
                        sCustomer.InvoicePhoneNumber = txtPhoneNumber.Text.Trim();
                        sCustomer.InvoiceFaxNo = txtFaxNo.Text.Trim();
                        sCustomer.VatNo = txtVatNo.Text.Trim();
                        sCustomer.SiretNo = txtSiretNo.Text.Trim();
                        sCustomer.InvoiceAddress1 = txtAddress1.Text.Trim();
                        sCustomer.InvoiceAddress2 = txtAddress2.Text.Trim();
                        sCustomer.InvoiceAddress3 = txtAddress3.Text.Trim();
                        sCustomer.InvoiceZipcode = txtZipcode.Text.Trim();
                        sCustomer.InvoiceCity = txtCity.Text.Trim();
                        sCustomer.InvoiceCountry = ddlCountry.SelectedValue;
                        sCustomer.TelephoneNo = txtPhoneNumber.Text.Trim();
                        sCustomer.Country = ddlCountry.SelectedValue.Trim();
                        sCustomer.ChiefContact = txtContact.Text.Trim();

                        if (chkInvoiceAddress.Checked)
                            sCustomer.UsedForShipping = SEnumFlag.Yes;
                        else
                            sCustomer.UsedForShipping = SEnumFlag.No;
                        #endregion

                        #region Key Account Inforamtion
                        sCustomer.KEY_CARRIER = string.Empty;
                        if (chkKeyAccount.Checked)
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.Yes;
                            sCustomer.CustomerType = SEnumCustomerType.KeyCustomer;
                            ////added by HV for bug 1108
                            sCustomer.KEY_CARRIER = ddKeyCarrier.SelectedValue;
                            sCustomer.SUBSCRIPTION_AMOUNT = Convert.ToDecimal(txtSubcription.Text.Trim());
                        }
                        else
                        {
                            sCustomer.IsKeyAccount = SEnumFlag.No;
                            sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                            sCustomer.KEY_CARRIER = string.Empty;

                        }
                        #endregion

                        #region Shipping Preference
                        string strShippingReference = string.Empty;
                        sCustomer.CarrierName = string.Empty;
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
                        sCustomer.CarrierName = ddlShipNamedCarrier.SelectedValue.Trim();
                        sCustomer.EXTRA_INFO = txtExtraInfo.Text.Trim();   
                        #endregion

                        #region Payment and Insuracne Details
                        sCustomer.InsuredMethod = ddlShipInsuredMethod.SelectedValue.Trim();

                        if (optCreditCard.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;
                        else if (optDeferdPayment.Checked)
                            sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;

                        if (chkReqestDP.Checked)
                            sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                        else
                            sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                        sCustomer.DEFERED_PAYMENT_TYPE = ddlDPType.SelectedValue.Trim();
                        if (!txtTranportBudget.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.BudgetAmount = Convert.ToDecimal(txtTranportBudget.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtDepositAmount.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.DepositAmount = Convert.ToDecimal(txtDepositAmount.Text.Trim());
                        }
                        else
                        {
                            sCustomer.BudgetAmount = 0.0m;
                        }
                        if (!txtPaymentDelay.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.PaymentDelayDays = Convert.ToInt32(txtPaymentDelay.Text.Trim());
                        }
                        else
                        {
                            sCustomer.PaymentDelayDays = 0;
                        }

                        if (!txtSubcription.Text.Trim().Equals(string.Empty))
                        {
                            sCustomer.SUBSCRIPTION_AMOUNT = Convert.ToInt32(txtSubcription.Text.Trim());
                        }
                        else
                        {
                            sCustomer.SUBSCRIPTION_AMOUNT = 0.0m;
                        }
                        #endregion

                        #region Account and History details
                        if (chkActiveAccount.Checked)
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        else
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                                          
                        sCustomer.LastUpdate = DateTime.Now;
                        sCustomer.ToSAcceptedDate = sCustomer.ToSAcceptedDate;
                        sCustomer.LastLogin = DateTime.Now;
                        #endregion
                        #endregion

                    }
                        #endregion

                    #region Authorized settings
                    if (KaizosSession.Current.UserType.Trim() == "AZ")
                    {        
                        #region SCustomer Fill Area
                       
                        if (!txtPassword.Text.Trim().Equals(string.Empty))
                        {
                            strCreatePassword = txtConfirmPassword.Text.Trim();
                            strPassword = proxy.EncryptString(strCreatePassword, "Password");
                            if (txtConfirmPassword.Text.Trim().Length != 0) //09APR12SM - Bug id : 1343 
                                sCustomer.Password = strPassword.Trim();

                        }
                        // sCustomer.AccountNo = "";
                        sCustomer.Email = txtEmail.Text.Trim();
                        sCustomer.Language = KaizosSession.Current.UserLanguage.Trim();
                        //sCustomer.IsChangePasswordRequired = sCustomer.IsChangePasswordRequired;
                        //sCustomer.CreatedBy = Sessoin_UserID.Trim();

                        #region UserActivityData
                        sCustomer.TurnOver = Convert.ToDecimal(txtTurnover.Text.Trim());
                        sCustomer.GrossMargin = Convert.ToDecimal(txtGrossMargin.Text.Trim());
                        sCustomer.ADV = Convert.ToInt32(txtADV.Text.Trim());
                        #endregion

                        sCustomer.VatNo = txtVatNo.Text.Trim();
                        sCustomer.SiretNo = txtSiretNo.Text.Trim();
                        if (chkInvoiceAddress.Checked)
                            sCustomer.UsedForShipping = SEnumFlag.Yes;
                        else
                            sCustomer.UsedForShipping = SEnumFlag.No;
                        string strShippingReference = string.Empty;
                        sCustomer.CarrierName = string.Empty;
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
                        sCustomer.InsuredMethod = ddlShipInsuredMethod.SelectedValue.Trim();
                        sCustomer.EXTRA_INFO = txtExtraInfo.Text.Trim();
                        if (chkActiveAccount.Checked)
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        else
                            sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                        sCustomer.LastUpdate = DateTime.Now;
                        sCustomer.ToSAcceptedDate = DateTime.Now;
                        sCustomer.LastLogin = DateTime.Now;
                        #endregion
                    }
                    #endregion

                    #region Updation Routine
                    int InsertStatus = proxy.UpdateEndCustomer(sCustomer);
                    if (InsertStatus == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmail.Text.Trim());
                    }
                    else if (InsertStatus == 2)
                    {
                        KaizosSession.Current.ReturnURL = "frmEndCustomerUpdate.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserAlreadyExist").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmEndCustomerUpdate.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    #endregion

                    #endregion

                    #region InsertADV
                    if ((KaizosSession.Current.UserType.Trim() == "AD" )||   (KaizosSession.Current.UserType.Trim() == "FR"))
                    {
                           
                            List<SAdv> sAdv = new List<SAdv>();
                            SUser sUser = new SUser();
                            sUser = proxy.GetLogin(txtEmail.Text.Trim());

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                            #region FillADV Array
                            // Parcel - Express - France
                            SAdv sAdv1 = new SAdv();
                            sAdv1.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv1.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelADVFr.Text.Trim()));
                            sAdv1.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelWPPFr.Text.Trim()));
                            if (txtParcelCarrierFr.Text.Trim().Length == 0)
                                sAdv1.CARRIER_NAME = ddlParcelCarrierFr.SelectedValue.Trim();
                            else
                                sAdv1.CARRIER_NAME = txtParcelCarrierFr.Text.Trim();
                            sAdv1.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelDiscountFr.Text.Trim()));
                            sAdv1.PACKAGE_TYPE = "Parcels";
                            sAdv1.PRIORITY = "Express";
                            sAdv1.SHIPMENT_TYPE = "France";
                            sAdv.Add(sAdv1);

                            // Parcel - Express - Europe
                            SAdv sAdv2 = new SAdv();
                            sAdv2.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv2.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelADVEup.Text.Trim()));
                            sAdv2.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelWPPEup.Text.Trim()));
                            if (txtParcelCarrierEup.Text.Trim().Length == 0)
                                sAdv2.CARRIER_NAME = ddlParcelCarrierEup.SelectedValue.Trim();
                            else
                                sAdv2.CARRIER_NAME = txtParcelCarrierEup.Text.Trim();
                            sAdv2.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelDiscountEup.Text.Trim()));
                            sAdv2.PACKAGE_TYPE = "Parcels";
                            sAdv2.PRIORITY = "Express";
                            sAdv2.SHIPMENT_TYPE = "Europe";
                            sAdv.Add(sAdv2);

                            // Parcel - Express - InterNational
                            SAdv sAdv3 = new SAdv();
                            sAdv3.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv3.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelADVInt.Text.Trim()));
                            sAdv3.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelWPPInt.Text.Trim()));
                            if (txtParcelCarrierInt.Text.Trim().Length == 0)
                                sAdv3.CARRIER_NAME = ddlParcelCarrierInt.SelectedValue.Trim();
                            else
                                sAdv3.CARRIER_NAME = txtParcelCarrierInt.Text.Trim();
                            sAdv3.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelDiscountInt.Text.Trim()));
                            sAdv3.PACKAGE_TYPE = "Parcels";
                            sAdv3.PRIORITY = "Express";
                            sAdv3.SHIPMENT_TYPE = "International";
                            sAdv.Add(sAdv3);

                            // Parcel - Economy - France
                            SAdv sAdv4 = new SAdv();
                            sAdv4.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv4.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyAdvFr.Text.Trim()));
                            sAdv4.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyWppFr.Text.Trim()));
                            if (txtParcelEconomyCarrierFr.Text.Trim().Length == 0)
                                sAdv4.CARRIER_NAME = ddlParcelEconomyCarrierFr.SelectedValue.Trim();
                            else
                                sAdv4.CARRIER_NAME = txtParcelEconomyCarrierFr.Text.Trim();
                            sAdv4.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyDiscountFr.Text.Trim()));
                            sAdv4.PACKAGE_TYPE = "Parcels";
                            sAdv4.PRIORITY = "Economy";
                            sAdv4.SHIPMENT_TYPE = "France";
                            sAdv.Add(sAdv4);

                            // Parcel - Economy - Europe
                            SAdv sAdv5 = new SAdv();
                            sAdv5.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv5.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyAdvEup.Text.Trim()));
                            sAdv5.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyWppEup.Text.Trim()));
                            if (txtParcelEconomyCarrierEup.Text.Trim().Length == 0)
                                sAdv5.CARRIER_NAME = ddlParcelEconomyCarrierEup.SelectedValue.Trim();
                            else
                                sAdv5.CARRIER_NAME = txtParcelEconomyCarrierEup.Text.Trim();
                            sAdv5.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyDiscountEup.Text.Trim()));
                            sAdv5.PACKAGE_TYPE = "Parcels";
                            sAdv5.PRIORITY = "Economy";
                            sAdv5.SHIPMENT_TYPE = "Europe";
                            sAdv.Add(sAdv5);

                            // Parcel - Economy - InterNational
                            SAdv sAdv6 = new SAdv();
                            sAdv6.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv6.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyAdvInt.Text.Trim()));
                            sAdv6.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyWppInt.Text.Trim()));
                            if (txtParcelEconomyCarrierInt.Text.Trim().Length == 0)
                                sAdv6.CARRIER_NAME = ddlParcelEconomyCarrierInt.SelectedValue.Trim();
                            else
                                sAdv6.CARRIER_NAME = txtParcelEconomyCarrierInt.Text.Trim();
                            sAdv6.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtParcelEconomyDiscountInt.Text.Trim()));
                            sAdv6.PACKAGE_TYPE = "Parcels";
                            sAdv6.PRIORITY = "Economy";
                            sAdv6.SHIPMENT_TYPE = "International";
                            sAdv.Add(sAdv6);

                            // Pallet - Express - France
                            SAdv sAdv7 = new SAdv();
                            sAdv7.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv7.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletExpressADVFr.Text.Trim()));
                            sAdv7.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressWppFr.Text.Trim()));
                            if (txtPalletExpressCarrierFr.Text.Trim().Length == 0)
                                sAdv7.CARRIER_NAME = ddlPalletExpressCarrierFr.SelectedValue.Trim();
                            else
                                sAdv7.CARRIER_NAME = txtPalletExpressCarrierFr.Text.Trim();
                            sAdv7.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressDiscountFr.Text.Trim()));
                            sAdv7.PACKAGE_TYPE = "Pallets";
                            sAdv7.PRIORITY = "Express";
                            sAdv7.SHIPMENT_TYPE = "France";
                            sAdv.Add(sAdv7);

                            // Pallet - Express - Europe
                            SAdv sAdv8 = new SAdv();
                            sAdv8.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv8.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletExpressADVEup.Text.Trim()));
                            sAdv8.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressWppEup.Text.Trim()));
                            if (txtPalletExpressCarrierEup.Text.Trim().Length == 0)
                                sAdv8.CARRIER_NAME = ddlPalletExpressCarrierEup.SelectedValue.Trim();
                            else
                                sAdv8.CARRIER_NAME = txtPalletExpressCarrierEup.Text.Trim();
                            sAdv8.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressDiscountEup.Text.Trim()));
                            sAdv8.PACKAGE_TYPE = "Pallets";
                            sAdv8.PRIORITY = "Express";
                            sAdv8.SHIPMENT_TYPE = "Europe";
                            sAdv.Add(sAdv8);

                            // Pallet - Express - International
                            SAdv sAdv9 = new SAdv();
                            sAdv9.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv9.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletExpressADVInt.Text.Trim()));
                            sAdv9.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressWppInt.Text.Trim()));
                            if (txtPalletExpressCarrierInt.Text.Trim().Length == 0)
                                sAdv9.CARRIER_NAME = ddlPalletExpressCarrierInt.SelectedValue.Trim();
                            else
                                sAdv9.CARRIER_NAME = txtPalletExpressCarrierInt.Text.Trim();
                            sAdv9.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletExpressDiscountInt.Text.Trim()));
                            sAdv9.PACKAGE_TYPE = "Pallets";
                            sAdv9.PRIORITY = "Express";
                            sAdv9.SHIPMENT_TYPE = "International";
                            sAdv.Add(sAdv9);

                            // Pallet - Economy - France
                            SAdv sAdv10 = new SAdv();
                            sAdv10.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv10.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyAdvFr.Text.Trim()));
                            sAdv10.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyWppFr.Text.Trim()));
                            if (txtPalletEconomyCarrierFr.Text.Trim().Length == 0)
                                sAdv10.CARRIER_NAME = ddlPalletEconomyCarrierFr.SelectedValue.Trim();
                            else
                                sAdv10.CARRIER_NAME = txtPalletEconomyCarrierFr.Text.Trim();
                            sAdv10.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyDiscountFr.Text.Trim()));
                            sAdv10.PACKAGE_TYPE = "Pallets";
                            sAdv10.PRIORITY = "Economy";
                            sAdv10.SHIPMENT_TYPE = "France";
                            sAdv.Add(sAdv10);

                            // Pallet - Economy - Europe
                            SAdv sAdv11 = new SAdv();
                            sAdv11.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv11.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyAdvEup.Text.Trim()));
                            sAdv11.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyWppEup.Text.Trim()));
                            if (txtPalletEconomyCarrierEup.Text.Trim().Length == 0)
                                sAdv11.CARRIER_NAME = ddlPalletExpressCarrierEup.SelectedValue.Trim();
                            else
                                sAdv11.CARRIER_NAME = txtPalletEconomyCarrierEup.Text.Trim();
                            sAdv11.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyDiscountEup.Text.Trim()));
                            sAdv11.PACKAGE_TYPE = "Pallets";
                            sAdv11.PRIORITY = "Economy";
                            sAdv11.SHIPMENT_TYPE = "Europe";
                            sAdv.Add(sAdv11);

                            // Pallet - Economy - International
                            SAdv sAdv12 = new SAdv();
                            sAdv12.ACCOUNT_NO = sUser.AccountNo.Trim();
                            sAdv12.AVERAGE_VALUE = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyDiscountInt.Text.Trim()));
                            sAdv12.AVERAGE_WEIGHT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyWppInt.Text.Trim()));
                            if (txtPalletEconomyCarrierInt.Text.Trim().Length == 0)
                                sAdv12.CARRIER_NAME = ddlPalletEconomyCarrierInt.SelectedValue.Trim();
                            else
                                sAdv12.CARRIER_NAME = txtPalletEconomyCarrierInt.Text.Trim();
                            sAdv12.DISCOUNT = Convert.ToDecimal(IsValidFormatString(txtPalletEconomyDiscountInt.Text.Trim()));
                            sAdv12.PACKAGE_TYPE = "Pallets";
                            sAdv12.PRIORITY = "Economy";
                            sAdv12.SHIPMENT_TYPE = "International";
                            sAdv.Add(sAdv12);
                            #endregion

                            InsertStatus = proxy.InsertAdv(sAdv.ToArray());
                            if (InsertStatus == 0)
                            {
                                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                                KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + string.Format(GetGlobalResourceObject("LocalString", "MessageUserADVSuccess").ToString(), txtEmail.Text.Trim());
                            }
                            else
                            {
                                KaizosSession.Current.ReturnURL = "frmEndCustomerUpdate.aspx";
                                KaizosSession.Current.ErrorMessage = KaizosSession.Current.ErrorMessage + string.Format(GetGlobalResourceObject("LocalString", "MessageUserADVFailure").ToString(), txtEmail.Text.Trim());
                            }
                            Server.Transfer("frmResult.aspx", false);



                    }
                    #endregion
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
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
                /* Generalized exception handling and logging detailed exception into log4net file*/
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

        protected void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                SCustomer sCustomer = new SCustomer();
                SUser sUser = new SUser();

                if (txtEmail.Text.Trim().Length != 0)
                {
                    sCustomer = proxy.GetEndCustomer(txtEmail.Text.Trim());
                    sUser = proxy.GetLogin(txtEmail.Text.Trim());
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmEndCustomerUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageUserIDEmpty").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }

                if (sCustomer.Status == SEnumUserStatus.Archived)
                    optStatusArchieve.Checked = true;
                else if (sCustomer.Status == SEnumUserStatus.Enabled)
                    optStatusEnable.Checked = true;
                else if (sCustomer.Status == SEnumUserStatus.Disabled)
                    optStatusDisable.Checked = true;

                //txtHQZipcode.Text = sCustomer.HqZipcode.Trim().Substring(2, sCustomer.HqZipcode.Trim().Length-2);
                txtHQZipcode.Text = sCustomer.HqZipcode.Trim();
                txtCompanyName.Text = sCustomer.CompanyName;
                txtTurnover.Text = Convert.ToString(sCustomer.TurnOver);
                txtGrossMargin.Text = Convert.ToString(sCustomer.GrossMargin);
                txtADV.Text = Convert.ToString(sCustomer.ADV);
                
                if(sCustomer.CustomerCategory == SEnumCustCategory.A)
                    ddlCustomerCategory.SelectedValue = "A";
                else if(sCustomer.CustomerCategory == SEnumCustCategory.B)
                    ddlCustomerCategory.SelectedValue = "B";
                else if (sCustomer.CustomerCategory == SEnumCustCategory.C)
                    ddlCustomerCategory.SelectedValue = "C";

                txtName.Text = sCustomer.Name;
                if (!sCustomer.IndustryType.Equals(string.Empty))
                {
                    ddlIndustry.SelectedValue = sCustomer.IndustryType;
                }
                txtLegalForm.Text = sCustomer.LegalForm;
                txtManPower.Text = Convert.ToString(sCustomer.ManPower);
                txtContact.Text = sCustomer.ChiefContact;
                txtPhoneNumber.Text = sCustomer.InvoicePhoneNumber;
                txtFaxNo.Text = sCustomer.InvoiceFaxNo;
                txtVatNo.Text = sCustomer.VatNo;
                txtSiretNo.Text = sCustomer.SiretNo;
                txtAddress1.Text = sCustomer.InvoiceAddress1;
                txtAddress2.Text = sCustomer.InvoiceAddress2;
                txtAddress3.Text = sCustomer.InvoiceAddress3;
                txtZipcode.Text = sCustomer.InvoiceZipcode;
                txtCity.Text = sCustomer.InvoiceCity;
                ddlCountry.SelectedValue = sCustomer.HqZipcode.Trim().Substring(0,2);

                if (sCustomer.UsedForShipping == SEnumFlag.Yes)
                    chkInvoiceAddress.Checked = true;
                else
                    chkInvoiceAddress.Checked = false;

                //if (sCustomer.ShipmentPreference == SEnumShipPreference.Fastest)
                //    ddlShipPreference.SelectedValue = "Fastest";
                //else if (sCustomer.ShipmentPreference == SEnumShipPreference.MostCompetitive)
                //    ddlShipPreference.SelectedValue = "MostCompetitive";
                //else if (sCustomer.ShipmentPreference == SEnumShipPreference.NamedCarrier)
                //    ddlShipPreference.SelectedValue = "NamedCarrier";

                if (!(sCustomer.ShipmentPreference == null) && !sCustomer.ShipmentPreference.Equals(string.Empty))
                {
                    List<ShippingPreference> lstShippingPreferance = new List<ShippingPreference>();

                    string[] strPreferance = sCustomer.ShipmentPreference.Split('|');
                    for (int i = 0; i < 3; i++)
                    {
                        ShippingPreference sp = new ShippingPreference();// This class is present in the frmendcustomer.aspx.
                        if (strPreferance[i].Equals("MC"))
                        {
                            sp.Id = 1;
                            sp.ShippingPreferenceType = "the most competitive";
                            sp.priority = i;
                            lstShippingPreferance.Add(sp);
                        }
                        if (strPreferance[i].Equals("NC"))
                        {
                            sp.Id = 3;
                            sp.ShippingPreferenceType = "named carrier";
                            sp.priority = i;
                            lstShippingPreferance.Add(sp);
                        }
                        if (strPreferance[i].Equals("F"))
                        {
                            sp.Id = 2;
                            sp.ShippingPreferenceType = "the fastest";
                            sp.priority = i;
                            lstShippingPreferance.Add(sp);
                        }
                    }
                    rlShippingPreference.DataSource = lstShippingPreferance;
                    rlShippingPreference.DataBind();
                    chkEnableShippingPreferance.Checked  = true;
                    trSelectShippingReference.Visible =true;
                    lblShipNamedCarrier.Visible = true;
                    ddlShipNamedCarrier.Visible = true;
                    ddlShipNamedCarrier.SelectedValue = sCustomer.CarrierName;
                }
                else
                {

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
                   // chkEnableShippingPreferance.Enabled = false; ;
                    trSelectShippingReference.Visible = false;
                    chkEnableShippingPreferance.Checked = false;
                    lblShipNamedCarrier.Visible = false;
                    ddlShipNamedCarrier.Visible = false;
                }

                if (sCustomer.IsKeyAccount == SEnumFlag.Yes)
                {
                    //trCarrier.Visible = true;
                    //lbCarrier.Visible = true;
                    chkKeyAccount.Enabled = true;
                    chkKeyAccount.Checked = true;
                    chkKeyAccount_CheckedChanged(this, EventArgs.Empty);
                    
                    if (!(sCustomer.KEY_CARRIER == null) && !sCustomer.KEY_CARRIER.Equals(string.Empty))
                    {
                       // ddKeyCarrier.SelectedValue = sCustomer.KEY_CARRIER;
                        lbCarrier.Visible = true;
                        #region Not using multiselect
                        if (sCustomer.KEY_CARRIER.Contains("|"))
                        {
                            string[] strCarrier = sCustomer.KEY_CARRIER.Split('|');

                            foreach (string s in strCarrier)
                            {
                                foreach (ListItem li in lbCarrier.Items)
                                {

                                    if (li.Value == s)
                                    {
                                        li.Selected = true;
                                        break;
                                    }
                                }

                            }

                        }
                        else
                        {
                            foreach (ListItem li in lbCarrier.Items)
                            {

                                if (li.Value == sCustomer.KEY_CARRIER)
                                {
                                    li.Selected = true;
                                    break;
                                }
                            }

                        }
                        #endregion
                    }

                }
                
                if (sCustomer.CarrierName.Trim() != "")
                    ddlShipNamedCarrier.SelectedValue = sCustomer.CarrierName;
                
                if (sCustomer.InsuredMethod.Trim() != "")
                    ddlShipInsuredMethod.SelectedValue = sCustomer.InsuredMethod;

                if (sCustomer.PaymentMethod == SEnumPaymentType.CreditCard)
                    optCreditCard.Checked = true;
                else
                    optDeferdPayment.Checked = true;

                if (sCustomer.DeferedPaymentRequired == SEnumFlag.Yes)
                {
                    chkReqestDP.Checked = true;
                    txtTranportBudget.Text = Convert.ToString(sCustomer.BudgetAmount);
                    txtDepositAmount.Text = Convert.ToString(sCustomer.DepositAmount);
                }
                else
                    chkReqestDP.Checked = false;

                if (sCustomer.DEFERED_PAYMENT_TYPE.Trim() != "")
                    ddlDPType.SelectedValue = sCustomer.DEFERED_PAYMENT_TYPE;

                
                txtPaymentDelay.Text = Convert.ToString(sCustomer.PaymentDelayDays);

                if (sCustomer.IsKeyAccount == SEnumFlag.Yes)
                {
                    chkKeyAccount.Checked = true;
                    //if (sCustomer.KEY_CARRIER.Trim() != "")
                    //    ddlForCarrier.SelectedValue = sCustomer.KEY_CARRIER;
                    txtSubcription.Text = Convert.ToString(sCustomer.SUBSCRIPTION_AMOUNT);
                }
                else
                    chkKeyAccount.Checked = false;

                
                txtExtraInfo.Text = Convert.ToString(sCustomer.EXTRA_INFO);
                
                if (sCustomer.FICTIVE_ACCOUNT == SEnumFlag.Yes)
                    chkActiveAccount.Checked = true;
                else
                    chkActiveAccount.Checked = false;

                if (sUser.IsToSAccepted == SEnumFlag.Yes)
                    chkTOS.Checked = true;
                else
                    chkTOS.Checked = false;

            }
            /* Introduced faultexception handling and logging detailed exception into log4net file*/
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageUserFetchFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file*/
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnGet_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageUserFetchFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnCance_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim().Length != 0)
                btnGet_Click(sender, e);
            Display();
        }

        protected string IsValidFormatString(string strValue)
        {
            string result = strValue.Trim();

            if (strValue.Trim().Length == 0)
                result = "0";
            return result;
        }

        protected void optCreditCard_CheckedChanged(object sender, EventArgs e)
        {
            if (optCreditCard.Checked)
            {
                lblDPType.Visible = false;
                ddlDPType.Visible = false;
            }
            else if (optCreditCard.Checked)
            {
                lblDPType.Visible = true;
                ddlDPType.Visible = true;
            }

        }

        protected void chkReqestDP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReqestDP.Checked)
            {
                lblTransportBudget.Visible = true;
                txtTranportBudget.Visible = true;
                lblDepositAmount.Visible = true;
                txtDepositAmount.Visible = true;
                txtTranportBudget.Text = "";
                txtDepositAmount.Text = "";
            }
            else
            {
                lblTransportBudget.Visible = false;
                txtTranportBudget.Visible = false;
                lblDepositAmount.Visible = false;
                txtDepositAmount.Visible = false;
                txtTranportBudget.Text = "";
                txtDepositAmount.Text = "";
            }
        }

        protected void optDeferdPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (optDeferdPayment.Checked)
            {
                lblDPType.Visible = true;
                ddlDPType.Visible = true;
            }
            else if (optCreditCard.Checked)
            {
                lblDPType.Visible = false;
                ddlDPType.Visible = false;
            }

        }

        protected void chkKeyAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKeyAccount.Checked)
            {
                lblForCarrier.Visible = true;
                trCarrier.Visible = true;
                lbCarrier.Visible = true;
              //  ddlForCarrier.Visible = true;
                lblSubscription.Visible = true;
                txtSubcription.Visible = true;
                txtSubcription.Text = "";
            }
            else
            {
                lblForCarrier.Visible = false;
                trCarrier.Visible = false;
                lbCarrier.Visible = false;
                //ddlForCarrier.Visible = false;
                lblSubscription.Visible = false;
                txtSubcription.Visible = false;
                txtSubcription.Text = "";
            }
        }

        protected void chkEnableShippingPreferance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableShippingPreferance.Checked)
            {
                trSelectShippingReference.Visible = true;
                lblShipNamedCarrier.Visible = true;
                ddlShipNamedCarrier.Visible = true;
            }
            else
            {
                trSelectShippingReference.Visible = false;
                lblShipNamedCarrier.Visible = false;
                ddlShipNamedCarrier.Visible = false;
            }
        }
    }
}