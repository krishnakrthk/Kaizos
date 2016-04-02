using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmManualShipping : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));
        KaizosServiceAgent proxy = new KaizosServiceAgent();
        List<SCountryTable> sCountryTable = new List<SCountryTable>();
        string strError = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<SComboText> sComboText = new List<SComboText>();
                SComboTableField sComboTableField = new SComboTableField();

                string strFlag = "";
                if (Request.QueryString["Flag"] != null)
                    strFlag = Request.QueryString["Flag"];

                if (!Page.IsPostBack)
                {
                    
                    Page.Title = GetGlobalResourceObject("LocalString", "frmManualShipping").ToString();

                    errorMsg1.Attributes["style"] = "display: none;";

                    #region Fill combos
                    
                    //To fill Country drop down list.
                    sCountryTable = proxy.FillCountryCombo().ToList();

                    ddlSenderCountry.DataSource = sCountryTable;
                    ddlSenderCountry.DataTextField = "CodeName";
                    ddlSenderCountry.DataValueField = "CountryCode";
                    ddlSenderCountry.DataBind();

                    ddlReturnCountry.DataSource = sCountryTable;
                    ddlReturnCountry.DataTextField = "CodeName";
                    ddlReturnCountry.DataValueField = "CountryCode";
                    ddlReturnCountry.DataBind();

                    ddlRecipientCountry.DataSource = sCountryTable;
                    ddlRecipientCountry.DataTextField = "CodeName";
                    ddlRecipientCountry.DataValueField = "CountryCode";
                    ddlRecipientCountry.DataBind();


                    //To fill Currency code drop down list
                    sComboTableField.FieldName = "CURRENCY_TYPE";
                    sComboTableField.TableName = "CURRENCY";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlCurrency.DataSource = sComboText;
                    ddlCurrency.DataTextField = "ComboText";
                    ddlCurrency.DataBind();

                    //To fill Package Type drop down list
                    sComboTableField.FieldName = "PACKAGE_TYPE";
                    sComboTableField.TableName = "PACK_TYPE";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlPackagingType.DataSource = sComboText;
                    ddlPackagingType.DataTextField = "ComboText";
                    ddlPackagingType.DataBind();

                    //To fill Package Type drop down list
                    sComboTableField.FieldName = "CLOSING_MATERIAL";
                    sComboTableField.TableName = "CLOSE_MATERIAL";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlClosedUsed.DataSource = sComboText;
                    ddlClosedUsed.DataTextField = "ComboText";
                    ddlClosedUsed.DataBind();

                    //To fill Package Type drop down list
                    sComboTableField.FieldName = "PACKING_MATERIAL";
                    sComboTableField.TableName = "PACKING_MATERIAL";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlPackageMaterial.DataSource = sComboText;
                    ddlPackageMaterial.DataTextField = "ComboText";
                    ddlPackageMaterial.DataBind();


                    //To fill Civility drop down list
                    sComboTableField.FieldName = "CIVILITY";
                    sComboTableField.TableName = "CIVILITY";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlSenderCivility.DataSource = sComboText;
                    ddlSenderCivility.DataTextField = "ComboText";
                    ddlSenderCivility.DataBind();

                    ddlRecipientCivility.DataSource = sComboText;
                    ddlRecipientCivility.DataTextField = "ComboText";
                    ddlRecipientCivility.DataBind();

                    ddlReturnCivility.DataSource = sComboText;
                    ddlReturnCivility.DataTextField = "ComboText";
                    ddlReturnCivility.DataBind();

                    #endregion Fill combos

                    ddlSenderCountry.SelectedValue = KaizosSession.Current.SenderCountry.Substring(0, 2);
                    ddlRecipientCountry.SelectedValue = KaizosSession.Current.RecipientCountry.Substring(0, 2);

                    #region Fill fields for edit
                    if (strFlag.Equals("Edit"))
                    {
                        
                       //Sender Info
                        SShipmentOrder OrderInfo = new SShipmentOrder();
                        OrderInfo = KaizosSession.Current.ShipmentOrder;
                        KaizosSession.Current.ShipReference = OrderInfo.ShipReference;
                        string[] SplittedArray;

                        txtSenderCompany.Text = OrderInfo.SenderCompany;

                        SplittedArray = OrderInfo.SenderName.ToString().Split('-');
                        ddlSenderCivility.SelectedValue = SplittedArray[0];
                        txtSenderFirstName.Text = SplittedArray[1];
                        txtSenderLastName.Text = SplittedArray[2];

                        txtSenderPhone.Text = OrderInfo.SenderPhone;
                        txtSenderEmail.Text = OrderInfo.SenderEmail;
                        txtSenderAddress1.Text = OrderInfo.SenderAddress1;
                        txtSenderAddress2.Text = OrderInfo.SenderAddress2;
                        txtSenderAddress3.Text = OrderInfo.SenderAddress3;
                        txtSenderCity.Text = OrderInfo.SenderCity;
                        KaizosSession.Current.SenderCity = OrderInfo.SenderCity; 
                        txtSenderZipCode.Text = OrderInfo.SenderZipCode;
                        KaizosSession.Current.SenderZip = OrderInfo.SenderZipCode;
                        txtSenderState.Text = OrderInfo.SenderState;
                        txtWishedShipDate.Text = OrderInfo.WishedShipDate.ToString("dd/MM/yyyy");
                        txtCollectionDeadLine.Text = OrderInfo.SenderCollectDeadLine;
                        txtSenderCondition.Text = OrderInfo.SenderComments;
                        ddlSenderCountry.SelectedValue = OrderInfo.SenderCountry;
                        
                        // Return Info
                        if (OrderInfo.SameReturnAddress.Equals(SEnumFlag.Yes))
                        {
                            ChangeReturnAddressStatus(false);
                            chkUseSender.Checked = true;
                        }
                        else
                        {
                            txtReturnCompany.Text = OrderInfo.ReturnCompany;
                            SplittedArray = OrderInfo.ReturnName.ToString().Split('-');
                            ddlReturnCivility.SelectedValue = SplittedArray[0];
                            txtReturnFirstName.Text = SplittedArray[1];
                            txtReturnLastName.Text = SplittedArray[2];

                            txtReturnPhone.Text = OrderInfo.ReturnPhone;
                            txtReturnEmail.Text = OrderInfo.ReturnEmail;
                            txtReturnAddress1.Text = OrderInfo.ReturnAddress1;
                            txtReturnAddress2.Text = OrderInfo.ReturnAddress2;
                            txtReturnAddress3.Text = OrderInfo.ReturnAddress3;
                            txtReturnCity.Text = OrderInfo.ReturnCity;
                            txtReturnZipCode.Text = OrderInfo.ReturnZipCode;
                            txtReturnState.Text = OrderInfo.ReturnState;

                            ddlReturnCountry.SelectedValue = OrderInfo.ReturnCountry;
                        }

                        //ddlReturnCountry.SelectedValue = "FR - FRANCE"; //getCountry(OrderInfo.ReturnCountry);
                        //txtReturnCondition.Text = OrderInfo.ret

                        //Recipient info

                        txtRecipientCompanyName.Text = OrderInfo.RecipientCompany;
                        SplittedArray = OrderInfo.RecipientName.ToString().Split('-');
                        ddlRecipientCivility.SelectedValue = SplittedArray[0];
                        txtRecipientFirstName.Text = SplittedArray[1];
                        txtRecipientLastName.Text = SplittedArray[2];

                        txtRecipientPhone.Text = OrderInfo.RecipientPhone;
                        txtRecipientEmail.Text = OrderInfo.RecipientEmail;
                        txtRecipientAddress1.Text = OrderInfo.RecipientAddress1;
                        txtRecipientAddress2.Text = OrderInfo.RecipientAddress2;
                        txtRecipientAddress3.Text = OrderInfo.RecipientAddress3;

                        txtRecipientCity.Text = OrderInfo.RecipientCity;
                        KaizosSession.Current.RecipientCity = OrderInfo.RecipientCity;
                        txtRecipientZipCode.Text = OrderInfo.RecipientZipCode;
                        KaizosSession.Current.RecipientZip = OrderInfo.RecipientZipCode;
                        txtRecipientState.Text = OrderInfo.RecipientState;
                        ddlRecipientCountry.SelectedValue = OrderInfo.RecipientCountry;
                        txtRecipientDeliveryDeadLine.Text = OrderInfo.RecipientDeliveryDeadLine;
                        txtRecipientConditions.Text = OrderInfo.RecipientComments;

                        txtCustomerInternalRef.Text = OrderInfo.CustomerInternalReference;

                        txtCustomsValue.Text = OrderInfo.CustomsValue.ToString();

                        if (OrderInfo.SenderNotification.Equals(SEnumFlag.Yes))
                        {
                            chkSenderNotification.Checked = true;
                        }
                        else
                        {
                            chkSenderNotification.Checked = false;
                        }

                        if (OrderInfo.RecipientNotification.Equals(SEnumFlag.Yes))
                        {
                            chkRecipientNotification.Checked = true;
                        }
                        else
                        {
                            chkRecipientNotification.Checked = false;
                        }
                        chkAcceptShipingCodition.Checked = true;                                                

                        if (OrderInfo.Insured == SEnumFlag.Yes)
                        {
                            chkInsurance.Checked = true;
                            txtDeclaredValue.Text = OrderInfo.DeclaredValue.ToString();
                            ddlCurrency.SelectedValue = OrderInfo.CurrencyType;

                            if (!(ddlPackagingType.Items.Contains(new ListItem(OrderInfo.PackageType))))
                            {
                                ddlPackagingType.SelectedValue = "Other";
                                txtPackagingType.Text = OrderInfo.PackageType;
                                txtPackagingType.Visible = true;
                            }
                            else
                            {
                                ddlPackagingType.SelectedValue = OrderInfo.PackageType;
                            }

                            ddlClosedUsed.SelectedValue = OrderInfo.ClosingMateiral;
                            ddlPackageMaterial.SelectedValue = OrderInfo.PackageMaterial;
                            chkInsuranceTermsAccept.Checked = true;
                            ChangeInsuranceStatus(true);
                            
                        }
                                            
                    }
                    #endregion Fill fields for edit

                    #region Enable/Disable

                    txtWishedShipDate.Text = KaizosSession.Current.ShippingDate.ToString("dd/MM/yyyy");
                    txtWishedShipDate.Enabled = false;

                    if (KaizosSession.Current.SenderCity.Equals("")) 
                    {
                        txtSenderCity.Enabled = true;  
                    }
                    else
                    {
                        txtSenderCity.Text = KaizosSession.Current.SenderCity;
                    }

                    if (KaizosSession.Current.SenderZip.Equals(""))
                    {
                        txtSenderZipCode.Enabled = true;
                    }
                    else
                    {
                        txtSenderZipCode.Text = KaizosSession.Current.SenderZip;
                    }

                    if (KaizosSession.Current.RecipientCity.Equals(""))
                    {
                        txtRecipientCity.Enabled = true;
                    }
                    else
                    {
                        txtRecipientCity.Text = KaizosSession.Current.RecipientCity;
                    }
                    if (KaizosSession.Current.RecipientZip.Equals(""))
                    {
                        txtRecipientZipCode.Enabled = true;
                    }
                    else
                    {
                        txtRecipientZipCode.Text = KaizosSession.Current.RecipientZip;
                    }

                    if ((ddlSenderCountry.SelectedValue.Equals("UE")) && (!(ddlRecipientCountry.SelectedValue.Equals("UE"))))
                    {
                        txtCustomsValue.Visible = true;
                        lblCustomsValue.Visible = true;
                    }

                    #endregion
                    chkUseSender.Checked = true;
                    chkUseSender_CheckedChanged(this, EventArgs.Empty);

                    if ((KaizosSession.Current.checking == 1) || (KaizosSession.Current.checking == 10))
                    {
                        filladdress();

                    }


                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipPageLoad").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "PageLoad", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipPageLoad").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }

        protected void ddlPackagingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if  ((ddlPackagingType.SelectedValue.ToUpper() == "OTHERS") ||
                    (ddlPackagingType.SelectedValue.ToUpper() == "OTHER"))
            {
                txtPackagingType.Visible = true;
            }
            else
            {
                txtPackagingType.Visible = false;
            }
        }

        protected void chkUseSender_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseSender.Checked)
            {
                ChangeReturnAddressStatus(false);
                fillreturn();
            }
            else
            {
                ChangeReturnAddressStatus(true);
            }

        }

        protected void fillreturn()
        {
            txtReturnAddress1.Text = txtSenderAddress1.Text;
            txtReturnAddress2.Text = txtSenderAddress2.Text;
            txtReturnAddress3.Text = txtSenderAddress3.Text;
            txtReturnCity.Text = txtSenderCity.Text;
            txtReturnCompany.Text = txtSenderCompany.Text;
            txtReturnDeliveryDeadline.Text = txtCollectionDeadLine.Text;
            txtReturnEmail.Text = txtSenderEmail.Text;
            txtReturnFirstName.Text = txtSenderFirstName.Text;
            txtReturnLastName.Text = txtSenderLastName.Text;
            txtReturnPhone.Text = txtSenderPhone.Text;
            txtReturnState.Text = txtSenderState.Text;
            txtReturnZipCode.Text = txtSenderZipCode.Text;
            
            txtReturnCondition.Text = txtSenderCondition.Text;
            ddlReturnCivility.SelectedValue = ddlSenderCivility.SelectedValue;
            ddlReturnCountry.SelectedValue = ddlSenderCountry.SelectedValue;
            
        }

        //protected void chkEnableShipPreference_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkEnableShipPreference.Checked)
        //    {
        //        rdShipPreference1.Visible = true;
        //        rdShipPreference2.Visible = true;
        //        rdShipPreference3.Visible = true;
        //        if (rdShipPreference3.Checked)
        //        {
        //            ddlCarrier.Visible = true;
        //            lblCarrier.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        rdShipPreference1.Visible = false;
        //        rdShipPreference2.Visible = false;
        //        rdShipPreference3.Visible = false;
        //        ddlCarrier.Visible = false;
        //        lblCarrier.Visible = false;
        //    }
        //}

        //protected void rdShipPreference3_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdShipPreference3.Checked)
        //    {
        //        ddlCarrier.Visible = true;
        //        lblCarrier.Visible = true;
        //    }
        //    else
        //    {
        //        ddlCarrier.Visible = false;
        //        lblCarrier.Visible = false;
        //    }
        //}

        protected void chkInsurance_CheckedChanged(object sender, EventArgs e)
        {
            ChangeInsuranceStatus(chkInsurance.Checked);
        }

        protected void ChangeReturnAddressStatus(bool bStatus)
        {
            txtReturnAddress1.Enabled = bStatus;
            txtReturnAddress2.Enabled = bStatus;
            txtReturnAddress3.Enabled = bStatus;
            txtReturnCity.Enabled = bStatus;
            txtReturnCompany.Enabled = bStatus;
            txtReturnDeliveryDeadline.Enabled = bStatus;
            txtReturnEmail.Enabled = bStatus;
            txtReturnFirstName.Enabled = bStatus;
            txtReturnLastName.Enabled = bStatus;
            txtReturnPhone.Enabled = bStatus;
            txtReturnState.Enabled = bStatus;
            txtReturnZipCode.Enabled = bStatus;
            txtReturnZipCode.Enabled = bStatus;
            txtReturnCondition.Enabled = bStatus;
            ddlReturnCivility.Enabled = bStatus;
            ddlReturnCountry.Enabled = bStatus;
            btnRtAddAddress.Enabled = bStatus;
            btnRtPicKAnAddress.Enabled = bStatus;

        }

        protected void ChangeInsuranceStatus(bool bStatus)
        {
            trInsurance1.Visible = bStatus;
            trInsurance2.Visible = bStatus;
            trInsurance3.Visible = bStatus;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                
                int iResult = 1;
                SShipmentOrder sShipmentOrder = new SShipmentOrder();
                sShipmentOrder = fillShipOrder();
                try
                {
                    iResult = proxy.ConfirmShipment(sShipmentOrder);

                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [20JAN12HN] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipConfirmation").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [20JAN12HN] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmManualShipping.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ManualShipConfirmation").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                if (iResult == 0)
                {
                    KaizosSession.Current.shipflag = 0;
                    KaizosSession.Current.checking = 0;
                    KaizosSession.Current.sshipmentquotation = null;
                    Response.Redirect("frmShipmentConfirmation.aspx");
                }
            }
        }

        private SShipmentOrder fillShipOrder()
        {
            string format = "dd/MM/yyyy";
            SShipmentOrder sShipmentOrder = new SShipmentOrder();

            /*  Insurance */
            if (chkInsurance.Checked)
            {
                sShipmentOrder.Insured = (chkInsurance.Checked) ? SEnumFlag.Yes : SEnumFlag.No;
                sShipmentOrder.DeclaredValue = (float)Convert.ToDouble(txtDeclaredValue.Text);
                sShipmentOrder.ClosingMateiral = ddlClosedUsed.SelectedValue;
                
                sShipmentOrder.PackageMaterial = txtPackagingType.Text.Trim();
                sShipmentOrder.CurrencyType = ddlCurrency.SelectedValue;
                if (ddlPackagingType.SelectedValue == "Other")
                {
                    sShipmentOrder.PackageType= txtPackagingType.Text.Trim();
                }
                else
                {
                    sShipmentOrder.PackageType = ddlPackagingType.SelectedValue;
                }
            }
            else
            { 
                sShipmentOrder.Insured = (chkInsurance.Checked) ? SEnumFlag.Yes : SEnumFlag.No;
                sShipmentOrder.DeclaredValue = 0;
                sShipmentOrder.ClosingMateiral = "";
                sShipmentOrder.PackageType = "";
                sShipmentOrder.PackageMaterial = "";
                sShipmentOrder.CurrencyType = "";
            }

            /*References */
            
            sShipmentOrder.AccountNo =  KaizosSession.Current.AccountNo;
            sShipmentOrder.ShipReference = KaizosSession.Current.ShipReference; //"1021";
            sShipmentOrder.Status = "DRAFT";
            sShipmentOrder.ChosenPreference = SEnumShipPreference.Fastest;
            
            /*Sender*/
            sShipmentOrder.SenderCompany = txtSenderCompany.Text;
            sShipmentOrder.SenderName = ddlSenderCivility.SelectedValue.Trim() + "-" + txtSenderFirstName.Text.Trim() + "-" + txtSenderLastName.Text.Trim();
            sShipmentOrder.SenderPhone = txtSenderPhone.Text;
            sShipmentOrder.SenderEmail = txtSenderEmail.Text;
            sShipmentOrder.SenderAddress1 = txtSenderAddress1.Text;
            sShipmentOrder.SenderAddress2 = txtSenderAddress2.Text;
            sShipmentOrder.SenderAddress3 = txtSenderAddress3.Text;
            sShipmentOrder.SenderCity = txtSenderCity.Text;
            sShipmentOrder.SenderCollectDeadLine = txtCollectionDeadLine.Text;
            sShipmentOrder.SenderCountry = ddlSenderCountry.SelectedValue;
            sShipmentOrder.SenderState = txtSenderState.Text;
            sShipmentOrder.SenderZipCode = txtSenderZipCode.Text;
            sShipmentOrder.SenderComments = txtSenderCondition.Text;
            sShipmentOrder.ShipDateTime = DateTime.ParseExact(txtWishedShipDate.Text, format, CultureInfo.InvariantCulture);//Convert.ToDateTime(txtWishedShipDate.Text);
            sShipmentOrder.SenderNotification = (chkSenderNotification.Checked) ? SEnumFlag.Yes : SEnumFlag.No;

            /*Return */
            sShipmentOrder.ReturnAddress1 = txtReturnAddress1.Text;
            sShipmentOrder.ReturnAddress2 = txtReturnAddress2.Text;
            sShipmentOrder.ReturnAddress3 = txtReturnAddress3.Text;
            sShipmentOrder.ReturnCity = txtReturnCity.Text;
            sShipmentOrder.ReturnCompany = txtReturnCompany.Text;
            sShipmentOrder.ReturnCountry = ddlReturnCountry.Text;
            sShipmentOrder.ReturnName = ddlSenderCivility.SelectedValue.Trim() + "-" + txtReturnFirstName.Text.Trim() + "-" + txtReturnLastName.Text.Trim();
            sShipmentOrder.ReturnPhone = txtReturnPhone.Text;
            sShipmentOrder.ReturnState = txtReturnState.Text;
            sShipmentOrder.ReturnZipCode = txtReturnZipCode.Text;
            sShipmentOrder.SameReturnAddress = (chkUseSender.Checked) ? SEnumFlag.Yes : SEnumFlag.No;
            
            /*Recipient*/
            sShipmentOrder.RecipientCompany = txtRecipientCompanyName.Text;
            sShipmentOrder.RecipientName = ddlRecipientCivility.SelectedValue.Trim() + "-" + txtRecipientFirstName.Text.Trim() + "-" + txtRecipientLastName.Text.Trim();
            sShipmentOrder.PaymentType = SEnumPaymentType.DeferredPayment;
            sShipmentOrder.RecipientAddress1 = txtRecipientAddress1.Text;
            sShipmentOrder.RecipientAddress2 = txtRecipientAddress2.Text;
            sShipmentOrder.RecipientAddress3 = txtRecipientAddress3.Text;
            sShipmentOrder.RecipientCity = txtRecipientCity.Text;
            sShipmentOrder.RecipientComments = txtRecipientConditions.Text;
            sShipmentOrder.RecipientCountry = ddlRecipientCountry.SelectedValue;
            sShipmentOrder.RecipientDeliveryDeadLine = txtRecipientDeliveryDeadLine.Text;
            sShipmentOrder.RecipientEmail = txtRecipientEmail.Text;
            sShipmentOrder.RecipientPhone = txtRecipientPhone.Text.Trim();
            sShipmentOrder.RecipientState = txtRecipientState.Text.Trim();
            sShipmentOrder.RecipientType = SEnumAddressType.Company;
            sShipmentOrder.RecipientZipCode = txtRecipientZipCode.Text;
            sShipmentOrder.RecipientNotification = (chkRecipientNotification.Checked) ? SEnumFlag.Yes : SEnumFlag.No;

            /*Incoterm*/

            /*Additional Details*/
            sShipmentOrder.CustomerInternalReference = txtCustomerInternalRef.Text;
            if (txtCustomsValue.Text.Equals(""))
            {
                sShipmentOrder.CustomsValue = 0;
            }
            else
            {
                sShipmentOrder.CustomsValue = (float)Convert.ToDouble(txtCustomsValue.Text);
            }

            /*Defaul values passed to Order object*/
            List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
            SShipmentResult sShipmentResult = new SShipmentResult();

            sShipmentOrder.CalculatedDeliveryTime = "";
            sShipmentOrder.CalculatedShipDate = DateTime.Now;
            sShipmentOrder.CancelResponsible = "";
            sShipmentOrder.Carrier = KaizosSession.Current.CarrierName;
            sShipmentOrder.CarrierService = KaizosSession.Current.ServiceName;
            sShipmentOrder.ContainerType = "";
            sShipmentOrder.FreightAmount = 0;
            sShipmentOrder.FuelCharge = 0;
            sShipmentOrder.Options = "";
            sShipmentOrder.OptionsCharges = "";
            sShipmentOrder.OrderCreationTime = DateTime.Now;
            sShipmentOrder.OrderType = SEnumOrderType.Manual;
            sShipmentOrder.ShipDetail = sShipmentDetails.ToArray(); //KaizosSession.Current.ShipmentDetail.ToArray();
            sShipmentOrder.ShipGroupID = 0;
            sShipmentOrder.TaxableWeight = 0;
            sShipmentOrder.TotalAmount =0;
            sShipmentOrder.TotalWeight = 0;
            sShipmentOrder.UODCount = 0;
            sShipmentOrder.WishedShipDate = DateTime.Now;

            sShipmentResult.LabelError = "";
            sShipmentResult.isLabelGenerated = SEnumFlag.No;
            sShipmentResult.ManifestError = "";
            sShipmentResult.isManifestGenerated = SEnumFlag.No;
            sShipmentResult.FeasibilityError = "";
            sShipmentResult.isFeasibility = SEnumFlag.No;
            sShipmentResult.OtherError = "";
            sShipmentResult.isOther = SEnumFlag.No;

            sShipmentOrder.ShipmentResult = sShipmentResult;

        
            return sShipmentOrder;
        
        }

        protected void val_Insurance_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            strError = "";
            decimal d;

            #region Insurance validation

            if (chkInsurance.Checked)
            {
                if (txtDeclaredValue.Text.Equals(""))
                {
                    strError = strError + "*" + lblDeclaredValue.Text.Trim() +" " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if ((!Decimal.TryParse(txtDeclaredValue.Text, out d)) && (!txtDeclaredValue.Text.Equals("")))
                {
                    strError = strError + "*" + lblDeclaredValue.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                else if ((KaizosSession.Current.ParcelCount == 1) && (Convert.ToDecimal(txtDeclaredValue.Text) > 25000))
                {
                    strError = strError + "*" + lblDeclaredValue.Text.Trim() + " " + valSingle.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                else if ((KaizosSession.Current.ParcelCount > 1) && (Convert.ToDecimal(txtDeclaredValue.Text) > 50000))
                {
                    strError = strError + "*" + lblDeclaredValue.Text.Trim() + " " + valMulti.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (ddlPackagingType.SelectedValue.Equals("Other") && txtPackagingType.Text.Equals(""))
                {
                    strError = strError + "*" + lblPackagingType.Text.Trim() + " " + valEmpty.Text.Trim() + " <br>";
                    args.IsValid = false;
                }

                if (!(chkInsuranceTermsAccept.Checked))
                {
                    strError = strError + "*" + valAccept.Text.Trim() + " - '" + chkInsuranceTermsAccept.Text.Trim() + "'<br>";
                    args.IsValid = false;
                }
            }

            #endregion

            bool SenderResult = ValidateSenderDetails();
            if (!(SenderResult))
                args.IsValid = SenderResult;

            bool ReturnResult =ValidateReturnDetails();
            if (!(ReturnResult))
                args.IsValid = ReturnResult;

            bool RecipientResult =ValidateRecipientDetails();
            if (!(RecipientResult))
                args.IsValid = RecipientResult;

            if ((ddlSenderCountry.SelectedValue.Equals("UE")) && (!(ddlRecipientCountry.SelectedValue.Equals("UE"))) && (txtCustomsValue.Text.Equals("")))
            {
                strError = strError + "*" + lblCustomsValue.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!Decimal.TryParse(txtCustomsValue.Text, out d)) && (!txtCustomsValue.Text.Equals("")))
            {
                strError = strError + "*" + lblCustomsValue.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(chkAcceptShipingCodition.Checked))
            {
                strError = strError + "*" + valAccept.Text.Trim() + " - '" + chkAcceptShipingCodition.Text.Trim() + "'<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_Insurance.ErrorMessage = strError;
                errorMsg1.Attributes["style"] = "display: block;";
            }

        }

        protected bool isValidDate(string Date)
        {
            bool result = true;
            DateTime value;
            string DateFormat = "dd/MM/yyyy";
            if (!(DateTime.TryParseExact(Date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)))
            {
                result = false;
            }

            return result;
        }

        protected bool isValidAddress(string Address1, string Address2, string Address3)
        {
            bool result = true;

            if (!(proxy.isAlphaNumericValidation(Address1)))
            {
                result = false;
            }

            if (!(Address2.Equals("")))
            {
                if (!(proxy.isAlphaNumericValidation(Address2)))
                {
                    result = false;
                }
            }

            if (!(Address3.Equals("")))
            {
                if (!(proxy.isAlphaNumericValidation(Address3)))
                {
                    result = false;
                }
            }
            return result;
        }

        protected bool ValidateSenderDetails()
        {
            #region Sender Validation

            string DateFormat = "dd/MM/yyyy";
            bool isValid = true;
            int i = 0;

            if (txtSenderCompany.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(proxy.isAlphaNumericValidation(txtSenderCompany.Text.Trim())))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSCompanyName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtSenderFirstName.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(proxy.isAlphaNumericValidation(txtSenderFirstName.Text.Trim())))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtSenderPhone.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(Int32.TryParse(txtSenderPhone.Text, out i)))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSPhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (!(txtSenderEmail.Text.Equals("")) && (proxy.ValidateEmail(txtSenderEmail.Text) > 0))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtSenderAddress1.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSAddress.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(isValidAddress(txtSenderAddress1.Text.Trim(), txtSenderAddress2.Text.Trim(), txtSenderAddress3.Text.Trim())))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSAddress.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtSenderCity.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            //else if (!(proxy.isAlphaNumericValidation(txtSenderCity.Text.Trim())))
            //{
            //    strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSCity.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
            //    isValid = false;
            //}

            if (txtSenderZipCode.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSZipCode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            //else if (!(proxy.isAlphaNumericValidation(txtSenderZipCode.Text.Trim())))
            //{
            //    strError = strError + "*" + lblLegendSender.Text.Trim() + " " + lblSZipCode.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
            //    isValid = false;
            //}

            if (txtWishedShipDate.Text.Equals(""))
            {
                strError = strError + "*" + lblWishedShipDate.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!isValidDate(txtWishedShipDate.Text))
            {
                strError = strError + "*" + lblWishedShipDate.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (DateTime.ParseExact(txtWishedShipDate.Text, DateFormat, CultureInfo.InvariantCulture) < DateTime.Now)
            {
                strError = strError + "*" + lblWishedShipDate.Text.Trim() + " " + valGreater.Text.Trim() + " " + DateTime.Now.ToString("dd/MM/yyyy") + "<br>";
                isValid = false;
            }

            if (!(txtCollectionDeadLine.Text.Equals("")))
            {
                if (proxy.ValidateTime(txtCollectionDeadLine.Text.Trim()) == 1)
                {
                    strError = strError + "*" + lblCollectionDeadline.Text.Trim() + " " + valInvalid.Text.Trim() + ". "
                            + GetGlobalResourceObject("LocalString", "ExpectedTimeFormat").ToString() + "<br>";
                    isValid = false;
                }
            }

            return isValid;

            #endregion
        }

        protected void btnSenderAddress_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                SAddressBook sAddressBook = new SAddressBook();
                KaizosServiceContractClient context = new KaizosServiceContractClient();

                sAddressBook.AccountNo = KaizosSession.Current.AccountNo;
                sAddressBook.Address1 = txtSenderAddress1.Text.Trim();
                sAddressBook.Address2 = txtSenderAddress2.Text.Trim();
                sAddressBook.Address3 = txtSenderAddress3.Text.Trim();
                sAddressBook.AddressType = KaizosSession.Current.RecipientAddressType;
                sAddressBook.AddressUsedFor = SEnumDeliveryType.ShippingAddress;
                sAddressBook.City = txtSenderCity.Text.Trim();
                sAddressBook.Comments = txtSenderCondition.Text.Trim();
                sAddressBook.CompanyName = txtSenderCompany.Text.Trim();
                sAddressBook.Country = ddlSenderCountry.SelectedValue.Trim();
                sAddressBook.CreatedDate = DateTime.Now;
                sAddressBook.Email = txtSenderEmail.Text.Trim();
                sAddressBook.LastPickupMondayToThursday = txtCollectionDeadLine.Text.Trim();
                sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
                sAddressBook.LastPickupFriday = "";
                sAddressBook.LastUpdated = DateTime.Now;
                sAddressBook.Name = ddlSenderCivility.SelectedItem.Text.Trim() + "-" + txtSenderFirstName.Text.Trim() + "-" + txtSenderLastName.Text.Trim();

                sAddressBook.State = txtSenderState.Text.Trim();
                sAddressBook.TelephoneNo = txtSenderPhone.Text.Trim();
                sAddressBook.ZipCode = txtSenderZipCode.Text.Trim();
                sAddressBook.FaxNo = "";
                sAddressBook.NamedCarrier = "";//added by HV 16APR2012
                sAddressBook.ShipPreferenceOrder = "";//added by HV 16APR2012

                int InsertStatus = context.InsertAddressBook(sAddressBook);
                if (InsertStatus == 0)
                {
                    lblSenderMessage.Text = String.Format(GetGlobalResourceObject("LocalString", "AddressBookSuccess").ToString(), txtSenderAddress1.Text.Trim());
                    ModalPopupSender.Show();
                }
                else if (InsertStatus == 2)
                {
                    lblSenderMessage.Text = GetGlobalResourceObject("LocalString", "AddressBookAlreadyExists").ToString();
                    ModalPopupSender.Show();
                }
            }
            else
            {
                lblSenderMessage.Text = strError;
                ModalPopupSender.Show();
            
            }
        }

        protected void val_SenderAddress_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            strError = "";
            args.IsValid = ValidateSenderDetails();
            if (!(args.IsValid))
            {
                val_SenderAddress.ErrorMessage = strError;
            }
        }

        protected bool ValidateReturnDetails()
        {
            bool isValid = true;
            int i = 0;

            #region Return Address

            if (!(chkUseSender.Checked))
            {
                if (txtReturnCompany.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtCompany.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (txtReturnFirstName.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (txtReturnPhone.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtPhone.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }
                else if (!(Int32.TryParse(txtReturnPhone.Text, out i)))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtPhone.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (txtReturnAddress1.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtAddress.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }
                else if (!(isValidAddress(txtReturnAddress1.Text.Trim(), txtReturnAddress2.Text.Trim(), txtReturnAddress3.Text.Trim())))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtAddress.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (txtReturnCity.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (txtReturnZipCode.Text.Equals(""))
                {
                    strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblRtZipCode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    isValid = false;
                }

                if (!(txtReturnDeliveryDeadline.Text.Equals("")))
                {
                    if (proxy.ValidateTime(txtReturnDeliveryDeadline.Text.Trim()) == 1)
                    {
                        strError = strError + "*" + lblLegendReturn.Text.Trim() + " " + lblDeliveryDeadLine.Text.Trim() + " " + valInvalid.Text.Trim() + ". "
                                + GetGlobalResourceObject("LocalString", "ExpectedTimeFormat").ToString() + "<br>";
                        isValid = false;
                    }
                }

            }
            #endregion

            return isValid;
        }

        protected void val_ReturnAddress_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            strError = "";
            args.IsValid = ValidateReturnDetails();
            //args.IsValid = ValidateRecipientDetails();
            if (!(args.IsValid))
            {
                val_ReturnAddress.ErrorMessage = strError;
            }
        }

        protected void btnRtAddAddress_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                SAddressBook sAddressBook = new SAddressBook();
                KaizosServiceContractClient context = new KaizosServiceContractClient();

                sAddressBook.AccountNo = KaizosSession.Current.AccountNo;
                sAddressBook.Address1 = txtReturnAddress1.Text.Trim();
                sAddressBook.Address2 = txtReturnAddress2.Text.Trim();
                sAddressBook.Address3 = txtReturnAddress3.Text.Trim();
                sAddressBook.AddressType = KaizosSession.Current.RecipientAddressType;
                sAddressBook.AddressUsedFor = SEnumDeliveryType.ShippingAddress;
                sAddressBook.City = txtReturnCity.Text.Trim();
                sAddressBook.Comments = txtReturnCondition.Text.Trim();
                sAddressBook.CompanyName = txtReturnCompany.Text.Trim();
                sAddressBook.Country = ddlReturnCountry.SelectedValue.Trim();
                sAddressBook.CreatedDate = DateTime.Now;
                sAddressBook.Email = txtReturnEmail.Text.Trim();
                sAddressBook.LastPickupMondayToThursday = txtReturnDeliveryDeadline.Text.Trim();
                sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
                sAddressBook.LastPickupFriday = "";
                sAddressBook.LastUpdated = DateTime.Now;
                sAddressBook.Name = ddlReturnCivility.SelectedItem.Text.Trim() + "-" + txtReturnFirstName.Text.Trim() + "-" + txtReturnLastName.Text.Trim();

                sAddressBook.State = txtReturnState.Text.Trim();
                sAddressBook.TelephoneNo = txtReturnPhone.Text.Trim();
                sAddressBook.ZipCode = txtReturnZipCode.Text.Trim();
                sAddressBook.FaxNo = "";
                sAddressBook.NamedCarrier = "";//added by HV 16APR2012
                sAddressBook.ShipPreferenceOrder = "";//added by HV 16APR2012
                int InsertStatus = context.InsertAddressBook(sAddressBook);
                if (InsertStatus == 0)
                {
                    lblMessage.Text = String.Format(GetGlobalResourceObject("LocalString", "AddressBookSuccess").ToString(), txtReturnAddress1.Text.Trim());
                    ModalPopupExtender1.Show();
                }
                else if (InsertStatus == 2)
                {
                    lblMessage.Text = GetGlobalResourceObject("LocalString", "AddressBookAlreadyExists").ToString();
                    ModalPopupExtender1.Show();
                }
            }
            else
            {
                lblRetrunMessage.Text = strError;
                ModalPopupReturn.Show();
            }
            
        }

        protected bool ValidateRecipientDetails()
        {

            #region Recipient Address

            bool isValid = true;
            int i = 0;

            if (txtRecipientCompanyName.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReCompany.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientFirstName.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientPhone.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblRePhone.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(Int32.TryParse(txtRecipientPhone.Text, out i)))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblRePhone.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (!(txtRecipientEmail.Text.Equals("")) && (proxy.ValidateEmail(txtRecipientEmail.Text) > 0))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientAddress1.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReAddress.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(isValidAddress(txtRecipientAddress1.Text.Trim(), txtRecipientAddress2.Text.Trim(), txtRecipientAddress3.Text.Trim())))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReAddress.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientCity.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientZipCode.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReZip.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }

            if (txtRecipientDeliveryDeadLine.Text.Equals(""))
            {
                strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReDelivery.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                isValid = false;
            }
            else if (!(txtRecipientDeliveryDeadLine.Text.Equals("")))
            {
                if (proxy.ValidateTime(txtRecipientDeliveryDeadLine.Text.Trim()) == 1)
                {
                    strError = strError + "*" + lblLegendRecipient.Text.Trim() + " " + lblReDelivery.Text.Trim() + " " + valInvalid.Text.Trim() + ". "
                            + GetGlobalResourceObject("LocalString", "ExpectedTimeFormat").ToString() + "<br>";
                    isValid = false;
                }
            }

            return isValid;

            #endregion
        }

        protected void btnRecipientAddToAddress_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                SAddressBook sAddressBook = new SAddressBook();
                KaizosServiceContractClient context = new KaizosServiceContractClient();

                sAddressBook.AccountNo = KaizosSession.Current.AccountNo;
                sAddressBook.Address1 = txtRecipientAddress1.Text.Trim();
                sAddressBook.Address2 = txtRecipientAddress2.Text.Trim();
                sAddressBook.Address3 = txtRecipientAddress3.Text.Trim();
                sAddressBook.AddressType = KaizosSession.Current.RecipientAddressType;
                sAddressBook.AddressUsedFor = SEnumDeliveryType.DeliveryAddress;
                sAddressBook.City = txtRecipientCity.Text.Trim();
                sAddressBook.Comments = txtRecipientConditions.Text.Trim();
                sAddressBook.CompanyName = txtRecipientCompanyName.Text.Trim();
                sAddressBook.Country = ddlRecipientCountry.SelectedValue.Trim();
                sAddressBook.CreatedDate = DateTime.Now;
                sAddressBook.Email = txtRecipientEmail.Text.Trim();
                sAddressBook.LastPickupMondayToThursday = txtRecipientDeliveryDeadLine.Text.Trim();
                sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
                sAddressBook.LastPickupFriday = "";
                sAddressBook.LastUpdated = DateTime.Now;
                sAddressBook.Name = ddlRecipientCivility.SelectedItem.Text.Trim() + "-" + txtRecipientFirstName.Text.Trim() + "-" + txtRecipientLastName.Text.Trim();

                sAddressBook.State = txtRecipientState.Text.Trim();
                sAddressBook.TelephoneNo = txtRecipientPhone.Text.Trim();
                sAddressBook.ZipCode = txtRecipientZipCode.Text.Trim();
                sAddressBook.FaxNo = "";
                sAddressBook.NamedCarrier = "";//added by HV 16APR2012
                sAddressBook.ShipPreferenceOrder = "";//added by HV 16APR2012
                int InsertStatus = context.InsertAddressBook(sAddressBook);
                if (InsertStatus == 0)
                {
                    lblReceipentMessage.Text = String.Format(GetGlobalResourceObject("LocalString", "AddressBookSuccess").ToString(), txtReturnAddress1.Text.Trim());
                    ModalPopupReceipent.Show();
                }
                else if (InsertStatus == 2)
                {
                    lblReceipentMessage.Text = GetGlobalResourceObject("LocalString", "AddressBookAlreadyExists").ToString();
                    ModalPopupReceipent.Show();
                }
            }
            else
            {
                lblReceipentMessage.Text = strError;
                ModalPopupReceipent.Show();
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            strError = "";
            //args.IsValid = ValidateReturnDetails();
            args.IsValid = ValidateRecipientDetails();
            if (!(args.IsValid))
            {
                CustomValidator1.ErrorMessage = strError;
            }
        }

        protected void btnChooseProduct_Click(object sender, EventArgs e)
        {
            lblSError.Visible = false;
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            List<SAddressBook> sAddressList;
            string zcode = txtSenderZipCode.Text.ToString();
            string sub = ddlSenderCountry.SelectedItem.ToString();
            string scountry = sub.Substring(0, 2);
            if (zcode == "" || scountry == "")
            {
                sAddressList = proxy.GetAddressBookSearch(txt1.Text.Trim(), "S").ToList();
            }
            else
            {
                sAddressList = proxy.GetAddressnew(txt1.Text.Trim(), "S", zcode, scountry).ToList();
            }
            if (sAddressList.Count > 0)
            {
                gvAddressBookList.DataSource = sAddressList.ToArray();
            }
            else
            {
                lblSError.Visible = true;
                lblSError.Text = GetGlobalResourceObject("LocalString", "NoRecordsFound").ToString();
                gvAddressBookList.DataSource = null;
            }
            gvAddressBookList.DataBind();
            mpeThePopup.Show();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            mpeThePopup.Hide();
        }

        protected void gvAddressBookList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("MyEdit"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label comments = (Label)row.FindControl("Label1");
                
                txtSenderCompany.Text = Server.HtmlDecode(row.Cells[2].Text.Trim());
                string[] Name;
                string sName = Server.HtmlDecode(row.Cells[3].Text.Trim());
                Name = sName.ToString().Split('-');
                if (Name.Count() >= 2)
                {
                    ddlSenderCivility.SelectedValue = Name[0];
                    txtSenderFirstName.Text = Name[1];
                    txtSenderLastName.Text = Name[2];
                }
                else
                {
                    txtSenderFirstName.Text = sName;                
                }

                txtSenderPhone.Text = Server.HtmlDecode(row.Cells[4].Text.Trim()); 
                txtSenderAddress1.Text = Server.HtmlDecode(row.Cells[6].Text.Trim()); 
                txtSenderAddress2.Text = Server.HtmlDecode(row.Cells[7].Text.Trim());
                txtSenderAddress3.Text = Server.HtmlDecode(row.Cells[8].Text.Trim());
                txtSenderZipCode.Text = Server.HtmlDecode(row.Cells[9].Text.Trim());
                txtSenderCity.Text = Server.HtmlDecode(row.Cells[10].Text.Trim());
                txtSenderState.Text = Server.HtmlDecode(row.Cells[11].Text.Trim());
                txtSenderEmail.Text = Server.HtmlDecode(row.Cells[13].Text.Trim());
                txtCollectionDeadLine.Text = Server.HtmlDecode(row.Cells[14].Text.Trim());

                txtSenderCondition.Text = comments.Text;
                
                mpeThePopup.Hide();
            }
        }

        protected void gvAddressBookList_RT_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("MyEdit"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                txtReturnCompany.Text = Server.HtmlDecode(row.Cells[2].Text.Trim());
                Label comments = (Label)row.FindControl("Label1");

                string[] Name;
                string sName = Server.HtmlDecode(row.Cells[3].Text.Trim());
                Name = sName.ToString().Split('-');
                if (Name.Count() >= 2)
                {
                    ddlReturnCivility.SelectedValue = Name[0];
                    txtReturnFirstName.Text = Name[1];
                    txtReturnLastName.Text = Name[2];
                }
                else
                {
                    txtReturnFirstName.Text = sName;
                }

                txtReturnPhone.Text = Server.HtmlDecode(row.Cells[4].Text.Trim());
                txtReturnAddress1.Text = Server.HtmlDecode(row.Cells[6].Text.Trim());
                txtReturnAddress2.Text = Server.HtmlDecode(row.Cells[7].Text.Trim());
                txtReturnAddress3.Text = Server.HtmlDecode(row.Cells[8].Text.Trim());
                txtReturnZipCode.Text = Server.HtmlDecode(row.Cells[9].Text.Trim());
                txtReturnCity.Text = Server.HtmlDecode(row.Cells[10].Text.Trim());
                txtReturnState.Text = Server.HtmlDecode(row.Cells[11].Text.Trim());
                txtReturnEmail.Text = Server.HtmlDecode(row.Cells[13].Text.Trim());
                txtReturnDeliveryDeadline.Text = Server.HtmlDecode(row.Cells[14].Text.Trim());
               
                txtReturnCondition.Text = comments.Text;

                ddlReturnCountry.SelectedValue = Server.HtmlDecode(row.Cells[12].Text.Trim());

                ModalPopupExtender3.Hide();
            }
        }

        protected void btnRtAddress_Click(object sender, EventArgs e)
        {
            lblRtError.Visible = false;
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            List<SAddressBook> sAddressList;
            string zcode = txtReturnZipCode.Text.ToString();
            string sub = ddlReturnCountry.SelectedItem.ToString();
            string scountry = sub.Substring(0, 2);
            if (zcode == "" || scountry == "")
            {
                sAddressList = proxy.GetAddressBookSearch(TextBox1.Text.Trim(), "S").ToList();
            }
            else
            {
                sAddressList = proxy.GetAddressnew(TextBox1.Text.Trim(), "S", zcode, scountry).ToList();
            }
            if (sAddressList.Count > 0)
            {
                gvAddressBookList_RT.DataSource = sAddressList.ToArray();
            }
            else
            {
                lblRtError.Visible = true;
                lblRtError.Text = GetGlobalResourceObject("LocalString", "NoRecordsFound").ToString();
                gvAddressBookList_RT.DataSource = null;          
            }

            gvAddressBookList_RT.DataBind();
            ModalPopupExtender3.Show();
        }

        protected void btnReAddress_Click(object sender, EventArgs e)
        {
            lblReError.Visible = false;
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            List<SAddressBook> sAddressList;
            string zcode = txtRecipientZipCode.Text.ToString();
            string sub = ddlRecipientCountry.SelectedItem.ToString();
            string scountry = sub.Substring(0, 2);
            if (zcode == "" || scountry == "")
            {
                sAddressList = proxy.GetAddressBookSearch(TextBox2.Text.Trim(), "D").ToList();
            }
            else
            {
                sAddressList = proxy.GetAddressnew(TextBox2.Text.Trim(), "D", zcode, scountry).ToList();
            }
            if (sAddressList.Count > 0)
            {
                gvAddressBookList_RE.DataSource = sAddressList.ToArray();
            }
            else
            {
                lblReError.Visible = true;
                lblReError.Text = GetGlobalResourceObject("LocalString", "NoRecordsFound").ToString();

                gvAddressBookList_RE.DataSource = null;
            }
            gvAddressBookList_RE.DataBind();
            ModalPopupExtender2.Show();
        }

        protected void gvAddressBookList_RE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("MyEdit"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                txtRecipientCompanyName.Text = Server.HtmlDecode(row.Cells[2].Text.Trim());
                Label comments = (Label)row.FindControl("Label1");

                string[] Name;
                string sName = Server.HtmlDecode(row.Cells[3].Text.Trim());
                Name = sName.ToString().Split('-');
                if (Name.Count() >= 2)
                {
                    ddlRecipientCivility.SelectedValue = Name[0];
                    txtRecipientFirstName.Text = Name[1];
                    txtRecipientLastName.Text = Name[2];
                }
                else
                {
                    txtRecipientFirstName.Text = sName;
                }

                txtRecipientPhone.Text = Server.HtmlDecode(row.Cells[4].Text.Trim());
                txtRecipientAddress1.Text = Server.HtmlDecode(row.Cells[6].Text.Trim());
                txtRecipientAddress2.Text = Server.HtmlDecode(row.Cells[7].Text.Trim());
                txtRecipientAddress3.Text = Server.HtmlDecode(row.Cells[8].Text.Trim());
                txtRecipientZipCode.Text = Server.HtmlDecode(row.Cells[9].Text.Trim());
                txtRecipientCity.Text = Server.HtmlDecode(row.Cells[10].Text.Trim());
                txtRecipientState.Text = Server.HtmlDecode(row.Cells[11].Text.Trim());
                txtRecipientEmail.Text = Server.HtmlDecode(row.Cells[13].Text.Trim());
                txtRecipientDeliveryDeadLine.Text = Server.HtmlDecode(row.Cells[14].Text.Trim());
                
                //ddlReturnCountry.SelectedValue = row.Cells[12].Text.Trim();
                txtRecipientConditions.Text = comments.Text;

                ModalPopupExtender2.Hide();
            }
        }

        /// <summary>
        /// introducing back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            #region back
            List<SAddressBook> ss = new List<SAddressBook>();
            SAddressBook saddress = new SAddressBook();
            saddress.CompanyName = txtSenderCompany.Text;
            saddress.City = txtSenderCity.Text;
            saddress.Name = txtSenderFirstName.Text;
            saddress.TelephoneNo = txtSenderPhone.Text;
            saddress.Email = txtSenderEmail.Text;
            saddress.Address1 = txtSenderAddress1.Text;
            saddress.Address2 = txtSenderAddress2.Text;
            saddress.Address3 = txtSenderAddress3.Text;
            saddress.ZipCode = txtSenderZipCode.Text;
            saddress.State = txtSenderState.Text;
            saddress.Comments = txtSenderCondition.Text;
            saddress.LastPickupMondayToThursday = txtCollectionDeadLine.Text;
            saddress.AccountNo = txtSenderLastName.Text;
            saddress.AddressID = txtCustomerInternalRef.Text;

            SAddressBook raddress = new SAddressBook();
            raddress.CompanyName = txtRecipientCompanyName.Text;
            raddress.City = txtRecipientCity.Text;
            raddress.Name = txtRecipientFirstName.Text;
            raddress.TelephoneNo = txtRecipientPhone.Text;
            raddress.Email = txtRecipientEmail.Text;
            raddress.Address1 = txtRecipientAddress1.Text;
            raddress.Address2 = txtRecipientAddress2.Text;
            raddress.Address3 = txtRecipientAddress3.Text;
            raddress.ZipCode = txtRecipientZipCode.Text;
            raddress.State = txtRecipientState.Text;
            raddress.Comments = txtRecipientConditions.Text;
            raddress.LastPickupMondayToThursday = txtRecipientDeliveryDeadLine.Text;
            raddress.AccountNo = txtRecipientLastName.Text;


            SAddressBook rraddress = new SAddressBook();
            rraddress.CompanyName = txtReturnCompany.Text;
            rraddress.City = txtReturnCity.Text;
            rraddress.Name = txtReturnFirstName.Text;
            rraddress.TelephoneNo = txtReturnPhone.Text;
            rraddress.Email = txtReturnEmail.Text;
            rraddress.Address1 = txtReturnAddress1.Text;
            rraddress.Address2 = txtReturnAddress2.Text;
            rraddress.Address3 = txtReturnAddress3.Text;
            rraddress.ZipCode = txtReturnZipCode.Text;
            rraddress.State = txtReturnState.Text;
            rraddress.Comments = txtReturnCondition.Text;
            rraddress.LastPickupMondayToThursday = txtReturnDeliveryDeadline.Text;
            rraddress.Country = ddlReturnCountry.SelectedIndex.ToString();
            rraddress.AccountNo = txtReturnLastName.Text;


            ss.Add(saddress);
            ss.Add(raddress);
            ss.Add(rraddress);

            KaizosSession.Current.saddressbook = ss;
            KaizosSession.Current.declared_value = txtDeclaredValue.Text;
            KaizosSession.Current.Declared_type = ddlCurrency.SelectedIndex;
            KaizosSession.Current.pacakagetype = ddlPackagingType.SelectedIndex;
            KaizosSession.Current.closingunits = ddlClosedUsed.SelectedIndex;
            KaizosSession.Current.Material_unit = ddlPackageMaterial.SelectedIndex;
            KaizosSession.Current.sendermail = chkSenderNotification.Checked;
            KaizosSession.Current.rcivemail = chkRecipientNotification.Checked;
            KaizosSession.Current.accept_term = chkAcceptShipingCodition.Checked;
            if (chkInsuranceTermsAccept.Checked == true)
            {
                KaizosSession.Current.insure_term_accept = 1;
            }
            else
                KaizosSession.Current.insure_term_accept = 0;
            if (chkInsurance.Checked == true)
            {
                KaizosSession.Current.insure_shipping = 1;
            }
            else
                KaizosSession.Current.insure_shipping = 0;

            KaizosSession.Current.checkreturn = chkUseSender.Checked;
            KaizosSession.Current.checking = 1;
            KaizosSession.Current.gender_type = ddlSenderCivility.SelectedIndex.ToString() + "!" + ddlRecipientCivility.SelectedIndex.ToString() + "!" + ddlReturnCivility.SelectedIndex.ToString();
            Response.Redirect("rptTariffDelayInterrogation.aspx");

            #endregion
        }


        private void filladdress()
        {
            chkSenderNotification.Checked = KaizosSession.Current.sendermail;
            chkRecipientNotification.Checked = KaizosSession.Current.rcivemail;
            chkAcceptShipingCodition.Checked = KaizosSession.Current.accept_term;

            string[] Sp = KaizosSession.Current.gender_type.Split(new Char[] { '!' });

            ddlSenderCivility.SelectedIndex = Convert.ToInt32(Sp[0]);
            ddlRecipientCivility.SelectedIndex = Convert.ToInt32(Sp[1]);
            ddlReturnCivility.SelectedIndex = Convert.ToInt32(Sp[2]);

            if (KaizosSession.Current.insure_shipping == 1)
            {
                chkInsurance.Checked = true;
                chkInsurance_CheckedChanged(this, EventArgs.Empty);
                txtDeclaredValue.Text = KaizosSession.Current.declared_value;
                ddlCurrency.SelectedIndex = KaizosSession.Current.Declared_type;
                ddlPackagingType.SelectedIndex = KaizosSession.Current.pacakagetype;
                ddlClosedUsed.SelectedIndex = KaizosSession.Current.closingunits;
                ddlPackageMaterial.SelectedIndex = KaizosSession.Current.Material_unit;
                if (KaizosSession.Current.insure_term_accept == 1)
                {
                    chkInsuranceTermsAccept.Checked = true;
                }
            }
            chkUseSender.Checked = KaizosSession.Current.checkreturn;
            if (KaizosSession.Current.checkreturn == true)
                chkUseSender_CheckedChanged(this, EventArgs.Empty);
            List<SAddressBook> ss = new List<SAddressBook>();
            ss = KaizosSession.Current.saddressbook;

            SAddressBook saddress = new SAddressBook();
            SAddressBook raddress = new SAddressBook();
            SAddressBook rraddress = new SAddressBook();
            saddress = ss[0];
            raddress = ss[1];
            rraddress = ss[2];
            /////////////////////////////////////////////////////////////////////

            txtSenderCompany.Text = saddress.CompanyName;
            txtSenderCity.Text = saddress.City;
            txtSenderFirstName.Text = saddress.Name;
            txtSenderPhone.Text = saddress.TelephoneNo;
            txtSenderEmail.Text = saddress.Email;
            txtSenderAddress1.Text = saddress.Address1;
            txtSenderAddress2.Text = saddress.Address2;
            txtSenderAddress3.Text = saddress.Address3;
            txtSenderZipCode.Text = saddress.ZipCode;
            txtSenderState.Text = saddress.State;
            txtSenderCondition.Text = saddress.Comments;
            txtCollectionDeadLine.Text = saddress.LastPickupMondayToThursday;
            txtSenderLastName.Text = saddress.AccountNo;
            txtCustomerInternalRef.Text = saddress.AddressID;


            txtRecipientCompanyName.Text = raddress.CompanyName;
            txtRecipientCity.Text = raddress.City;
            txtRecipientFirstName.Text = raddress.Name;
            txtRecipientPhone.Text = raddress.TelephoneNo;
            txtRecipientEmail.Text = raddress.Email;
            txtRecipientAddress1.Text = raddress.Address1;
            txtRecipientAddress2.Text = raddress.Address2;
            txtRecipientAddress3.Text = raddress.Address3;
            txtRecipientZipCode.Text = raddress.ZipCode;
            txtRecipientState.Text = raddress.State;
            txtRecipientConditions.Text = raddress.Comments;
            txtRecipientDeliveryDeadLine.Text = raddress.LastPickupMondayToThursday;
            txtRecipientLastName.Text = raddress.AccountNo;


            txtReturnCompany.Text = rraddress.CompanyName;
            txtReturnCity.Text = rraddress.City;
            txtReturnFirstName.Text = rraddress.Name;
            txtReturnPhone.Text = rraddress.TelephoneNo;
            txtReturnEmail.Text = rraddress.Email;
            txtReturnAddress1.Text = rraddress.Address1;
            txtReturnAddress2.Text = rraddress.Address2;
            txtReturnAddress3.Text = rraddress.Address3;
            txtReturnZipCode.Text = rraddress.ZipCode;
            txtReturnState.Text = rraddress.State;
            txtReturnCondition.Text = rraddress.Comments;
            txtReturnDeliveryDeadline.Text = rraddress.LastPickupMondayToThursday;
            ddlReturnCountry.SelectedIndex = Convert.ToInt32(rraddress.Country);
            txtReturnLastName.Text = rraddress.AccountNo;


        }


    }
  
}