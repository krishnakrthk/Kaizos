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
    public partial class frmEndCustomerLevel1 : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmEndCustomerLevel1));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCusotmer").ToString();
                try
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    txtEmailConfirmation.Text = Request.QueryString["y"];
                    txtCompanyName.Text = Request.QueryString["x"];
                    //txtLanguage.Text = Request.QueryString["w"];

                    //To fill Dimension Units drop down list
                    List<SCountryTable> sCountryTable = new List<SCountryTable>();
                    sCountryTable = proxy.FillCountryCombo().ToList();

                    ddlInvoiceCountry.DataSource = sCountryTable;
                    ddlInvoiceCountry.DataTextField = "CodeName";
                    ddlInvoiceCountry.DataValueField = "CountryCode";
                    ddlInvoiceCountry.DataBind();

                    ddlShippingPreference.Items.Add("Fastest");
                    ddlShippingPreference.Items.Add("MostCompetitive");
                    ddlShippingPreference.Items.Add("NamedCarrier");

                    DeferredPaymentEnable(false);
                    SToS sTos = new SToS();
                    sTos = proxy.GetActiveToS();
                    lblMessage.Text = sTos.TermsOfService.Trim();
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
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
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void DeferredPaymentEnable(bool flag)
        {
            txtTransportBudget.Visible = flag;
            lblTransportBudget.Visible = flag;
            txtTransportBudget.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtContactName.Text = "";
            txtInvoicePhoneNumber.Text = "";
            txtVatNo.Text = "";
            txtSiretNo.Text = "";
            txtTransportBudget.Text = "";
            txtZipcode.Text = "";
            chkDeferredPayment.Checked = false;
            chkTOS.Checked = false;
            chkUseInvoiceAddress.Checked = false;
            chkUserReturnAddress.Checked = false;
        }

        protected void val_EndCustomer_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            KaizosServiceContractClient Context = new KaizosServiceContractClient();
            try
            {
                if (txtContactName.Text.Equals(""))
                {
                    strError = strError + "*" + lblContactName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else
                {
                    if (!(Context.isAlphaNumericValidation(txtContactName.Text.Trim())))
                    {
                        strError = strError + "*" + lblContactName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
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

                if (!txtInvoiceFaxNo.Text.Equals(""))
                {
                    if (!(Context.isNumericValidation(txtInvoiceFaxNo.Text.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + lblFaxNo.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
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
                    if (txtTransportBudget.Text.Equals(""))
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


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_EndCustomer_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
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
                    SCustomer sCustomerGet = new SCustomer();
                    sCustomerGet = proxy.GetCustomer(txtEmailConfirmation.Text.Trim());

                    #region Entity fill
                    //Login
                    sCustomer.AccountNo = "";
                    sCustomer.Email = txtEmailConfirmation.Text.Trim();
                    sCustomer.UserType = sCustomerGet.UserType;
                    sCustomer.Status = SEnumUserStatus.Enabled;
                    sCustomer.CreatedUserType = sCustomerGet.CreatedUserType;

                    sCustomer.IsSalesTarrifAssigned = sCustomerGet.IsSalesTarrifAssigned;
                    if (chkTOS.Checked)
                    {
                        sCustomer.IsToSAccepted = SEnumFlag.Yes;
                        sCustomer.ToSAcceptedDate = DateTime.Now;
                    }
                    else
                        sCustomer.IsToSAccepted = SEnumFlag.No;

                    sCustomer.IsChangePasswordRequired = sCustomerGet.IsChangePasswordRequired;
                    sCustomer.AdministratorUserId = KaizosSession.Current.AccountNo.Trim();
                    sCustomer.IsKeyAccount = sCustomerGet.IsKeyAccount;
                    sCustomer.CustomerType = sCustomerGet.CustomerType;
                    sCustomer.IsDeferredPaymentAgreed = sCustomerGet.IsDeferredPaymentAgreed;
                    sCustomer.CustomerCategory = sCustomerGet.CustomerCategory;
                    sCustomer.LastUpdate = DateTime.Now;

                    //Company
                    sCustomer.CompanyName = txtCompanyName.Text.Trim();

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

                    //if (ddlShippingPreference.SelectedValue.Trim() == "Fastest")
                    //    sCustomer.ShipmentPreference = SEnumShipPreference.Fastest;
                    //else if (ddlShippingPreference.SelectedValue.Trim() == "MostCompetitive")
                    //    sCustomer.ShipmentPreference = SEnumShipPreference.MostCompetitive;
                    //else
                    //    sCustomer.ShipmentPreference = SEnumShipPreference.NamedCarrier;

                    sCustomer.ShipmentPreference = string.Empty;
                    sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;
                    sCustomer.CustomerTypeChanged = SEnumFlag.No;
                    sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                    //Payment Method
                    sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;
                    if (chkDeferredPayment.Checked)
                    {
                        sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                        sCustomer.BudgetAmount = decimal.Parse(txtTransportBudget.Text.Trim());
                    }
                    else
                        sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                    #endregion

                    InsertResult = proxy.UpdateEndCustomerByAdmin(sCustomer);

                    if (InsertResult == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmailConfirmation.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmEndCustomerLevel1.aspx";
                        KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
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
                KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
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
                    KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmailConfirmation.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void chkDeferredPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeferredPayment.Checked)
                DeferredPaymentEnable(true);
            else
                DeferredPaymentEnable(false);
        }

        }
    }
