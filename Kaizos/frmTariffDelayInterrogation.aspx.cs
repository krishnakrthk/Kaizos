using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmTariffDelayInterrogation : BasePage
    {
       
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffDelayInterrogation));

        public DateTime Dday;
        public int weekday;
        DataTable dtShipmentGrid;
        static string AutoSenderCountry = "";
        static string AutoReceiverCountry = "";

        KaizosServiceAgent proxy = new KaizosServiceAgent();

        protected void Page_Load(object sender, EventArgs e)
        {
			errorMsg1.Attributes["style"] = "display: none;";
            errorMsg2.Attributes["style"] = "display: none;";
			try
            {
                List<SCountryTable> sCountryTable = new List<SCountryTable>();

                #region Post Back
                if (!Page.IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmTariffDelayInterrogation").ToString();
                    //KaizosSession.Current.AccountNo = "123";

                    List<SComboText> sComboText = new List<SComboText>();
                    SComboTableField sComboTableField = new SComboTableField();

                    //To fill Dimension Units drop down list
                    sComboTableField.FieldName = "DIMENSION_TYPE";
                    sComboTableField.TableName = "DIMENSION_UNIT_TYPE";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlDimensionUnit.DataSource = sComboText;
                    ddlDimensionUnit.DataTextField = "ComboText";
                    ddlDimensionUnit.DataBind();

                    //To fill Country drop down list.
                    sCountryTable = proxy.FillCountryCombo().ToList();
                    ddlCountry.DataSource = sCountryTable;
                    ddlCountry.DataTextField = "CodeName";
                    ddlCountry.DataBind();
                    ddlRCountry.DataSource = sCountryTable;
                    ddlRCountry.DataTextField = "CodeName";
                    ddlRCountry.DataBind();

                    ddlCountry.SelectedValue = "FR - FRANCE";
                    ddlRCountry.SelectedValue = "FR - FRANCE";

                    //To fill Container drop down list
                    sComboTableField.FieldName = "CONTAINER_TYPE";
                    sComboTableField.TableName = "CONTAINER";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlContainer.DataSource = sComboText;
                    ddlContainer.DataTextField = "ComboText";
                    ddlContainer.DataBind();

                    //To fill Weight Units drop down list
                    sComboTableField.FieldName = "WEIGHT_TYPE";
                    sComboTableField.TableName = "WEIGHT_UNIT_TYPE";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();

                    ddlWeightUnit.DataSource = sComboText;
                    ddlWeightUnit.DataTextField = "ComboText";
                    ddlWeightUnit.DataBind();

                    txtParcelNo.Text = "1";

                    dtShipmentGrid = new DataTable();
                    MakeDataTable();

                    //To load Prohibited List details
                    sComboTableField.FieldName = "DESCRIPTION";
                    sComboTableField.TableName = "PROHIBITED_LIST";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();
                    gvProhibitedList.DataSource = sComboText;
                    gvProhibitedList.DataBind();

                    //To fill Options
                    sComboTableField.FieldName = "DESCRIPTION";
                    sComboTableField.TableName = "SURCHARGE_MAST";
                    sComboText = proxy.FillCombo(sComboTableField).ToList();
                    if (KaizosSession.Current.shipflag == 0)
                    {
                        for (int i = 0; i < sComboText.Count; i++)
                        {
                            cblOptions.Items.Add(new ListItem(sComboText[i].ComboText, sComboText[i].ComboText));
                        }
                    }
                    if (KaizosSession.Current.shipflag == 1)
                    {

                        dtShipmentGrid = copytotable();
                        ViewState["dtShipGrid"] = dtShipmentGrid;

                        BindGrid();
                        txtCity.Text  = KaizosSession.Current.SenderCity;
                        txtRCity.Text  = KaizosSession.Current.RecipientCity;
                        ddlCountry.SelectedValue  = KaizosSession.Current.SenderCountryCode;
                        ddlRCountry.SelectedValue = KaizosSession.Current.RecipientCountryCode;
                        txtTotalGrossWeight.Text  =Convert.ToString(KaizosSession.Current.GrossWeight);
                        DateTime   g =KaizosSession.Current.ShippingDate;
                        //var parsedDateTime = DateTime.ParseExact(g, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string parsedDateTime = g.ToString("dd/MM/yyyy");
                        txtShipingDate.Text = parsedDateTime.ToString(); 
                        txtTotalParcelNo.Text = Convert.ToString(KaizosSession.Current.ParcelCount);
                        txtParcelNo.Text = Convert.ToString(KaizosSession.Current.ParcelCount+1);
                        txtZip.Text = KaizosSession.Current.SenderZip;
                        txtRZipCode.Text = KaizosSession.Current.RecipientZip;
                        string option = KaizosSession.Current.Options.Replace("'","");
                        string[] Split = option.Split(new Char[] { ',' });
                        for (int i = 0; i < sComboText.Count; i++)
                        {
                            cblOptions.Items.Add(new ListItem(sComboText[i].ComboText, sComboText[i].ComboText, true ));
                            for (int j = 0; j < Split.Length; j++)
                            {
                                if (sComboText[i].ComboText == Split[j])
                                {
                                    cblOptions.Items[i].Selected = true;

                                }
                                
                            }
                        }
                        if (KaizosSession.Current.RecipientAddressType == SEnumAddressType.Company)
                        {
                            rdAddressType1.Checked = true;
                        }
                        else
                        {
                            rdAddressType2.Checked = true;
                        }

                    }
                }
                else
                {
                    dtShipmentGrid = (DataTable)ViewState["dtShipGrid"];
                }
                #endregion

                ViewState["dtShipGrid"] = dtShipmentGrid;

                AutoSenderCountry = ddlCountry.SelectedValue;
                AutoSenderCountry = AutoSenderCountry.Substring(0, AutoSenderCountry.IndexOf("-")).Trim();

                AutoReceiverCountry = ddlRCountry.SelectedValue;
                AutoReceiverCountry = AutoReceiverCountry.Substring(0, AutoReceiverCountry.IndexOf("-")).Trim();
            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);

                    KaizosSession.Current.ReturnURL = "frmTariffDelayInterrogation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffDelayInterrogationError").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnGetQuote_Click(object sender, EventArgs e)
        {
            if (KaizosSession.Current.checking == 1)
                KaizosSession.Current.checking = 10;

            lblGridValudation.Visible = false;
            if (dtShipmentGrid.Rows.Count <= 0)
            {
                lblGridValudation.Visible = true;
                lblGridValudation.Text = "Add shipments for getting Quote";
                errorMsg2.Attributes["style"] = "display: block;";
            }
            else
            {

                SShipmentOrder sShipmentOrder = new SShipmentOrder();
                SShipmentQuotation sShipmentQuotation = new SShipmentQuotation();

                #region shiporder

                //sShipmentOrder.AccountNo = "Account";
                //sShipmentOrder.CalculatedDeliveryTime = "DL:TM";
                //sShipmentOrder.CalculatedShipDate = DateTime.Now;
                //sShipmentOrder.CancelResponsible = "CancelRes";
                //sShipmentOrder.Carrier = "Carrier";
                //sShipmentOrder.ChosenPreference = SEnumShipPreference.Fastest;
                //sShipmentOrder.ClosingMateiral = "Closing Material";
                //sShipmentOrder.CurrencyType = "CUR";
                //sShipmentOrder.CustomerInternalReference = "Internal";
                //sShipmentOrder.CustomsValue = 0;
                //sShipmentOrder.DeclaredValue = 1;
                //sShipmentOrder.FreightAmount = 2;
                //sShipmentOrder.FuelCharge = 3;
                //sShipmentOrder.Insured = SEnumFlag.Yes;
                //sShipmentOrder.Options = "Options";
                //sShipmentOrder.OptionsCharges = "Opt Charges";
                //sShipmentOrder.OrderCreationTime = DateTime.Now;
                //sShipmentOrder.OrderType = SEnumOrderType.Import;
                //sShipmentOrder.PackageMaterial = "Package";
                //sShipmentOrder.PackageType = "Pack Type";
                //sShipmentOrder.PaymentType = SEnumPaymentType.CreditCard;
                //sShipmentOrder.RecipientAddress1 = "R A1";
                //sShipmentOrder.RecipientAddress2 = "R A2";
                //sShipmentOrder.RecipientAddress3 = "R A3";
                //sShipmentOrder.RecipientCity = "R City";
                //sShipmentOrder.RecipientComments = " R Comment";
                //sShipmentOrder.RecipientCompany = "R Comp";
                //sShipmentOrder.RecipientCountry = "R Country";
                //sShipmentOrder.RecipientDeliveryDeadLine = "DL:DL";
                //sShipmentOrder.RecipientEmail = "R Email";
                //sShipmentOrder.RecipientName = "R Name";
                //sShipmentOrder.RecipientNotification = SEnumFlag.No;
                //sShipmentOrder.RecipientPhone = "R Phone";
                //sShipmentOrder.RecipientState = "R State";
                //sShipmentOrder.RecipientType = SEnumAddressType.Company;
                //sShipmentOrder.RecipientZipCode = "R Zip";
                //sShipmentOrder.ReturnAddress1 = "RT A1";
                //sShipmentOrder.ReturnAddress2 = "RT A2";
                //sShipmentOrder.ReturnAddress3 = "RT A3";
                //sShipmentOrder.ReturnCity = " RT City";
                //sShipmentOrder.ReturnCompany = "TR Company";
                //sShipmentOrder.ReturnCountry = "RT Country";
                //sShipmentOrder.ReturnName = "RT Name";
                //sShipmentOrder.ReturnPhone = "RT Phone";
                //sShipmentOrder.ReturnState = "RT State";
                //sShipmentOrder.ReturnZipCode = "RT Zip";
                //sShipmentOrder.SameReturnAddress = SEnumFlag.Yes;
                //sShipmentOrder.SenderAddress1 = "S A1";
                //sShipmentOrder.SenderAddress2 = "S A2";
                //sShipmentOrder.SenderAddress3 = "S A3";
                //sShipmentOrder.SenderCity = "S City";
                //sShipmentOrder.SenderCollectDeadLine = "CL:DL";
                //sShipmentOrder.SenderComments = "S Comment";
                //sShipmentOrder.SenderCompany = "S Company";
                //sShipmentOrder.SenderCountry = "S Country";
                //sShipmentOrder.SenderEmail = "S Email";
                //sShipmentOrder.SenderName = "S Name";
                //sShipmentOrder.SenderNotification = SEnumFlag.Yes;
                //sShipmentOrder.SenderPhone = "S Phone";
                //sShipmentOrder.SenderState = "S State";
                //sShipmentOrder.SenderZipCode = "S Zip";
                ////sShipmentOrder.ShipDetail = "S Detail";
                //sShipmentOrder.ShipGroupID = 1111;
                //sShipmentOrder.ShipReference = "ShipRef";
                //sShipmentOrder.ShipDateTime = DateTime.Now;
                //sShipmentOrder.Status = "Status";
                //sShipmentOrder.TaxableWeight = 4;
                //sShipmentOrder.TotalAmount = 5;
                //sShipmentOrder.TotalWeight = 6;
                //sShipmentOrder.UODCount = 7;
                //sShipmentOrder.WishedShipDate = DateTime.Now;
                //sShipmentQuotation.CarrierName = "Carrier";
                //sShipmentQuotation.CarrierPriority = "EXP";
                //sShipmentQuotation.CarrierType = "CType";
                //sShipmentQuotation.DeliveryDate = DateTime.Now;
                //sShipmentQuotation.FuelSurcharge = 8;

                #endregion

                PassFormValue();
                KaizosSession.Current.shipflag = 1;
                
                Response.Redirect("rptTariffDelayInterrogation.aspx");
            }
        }

        private void MakeDataTable()
        {
            dtShipmentGrid.Columns.Add("gvParcel");
            dtShipmentGrid.Columns.Add("gvContent");
            dtShipmentGrid.Columns.Add("gvContainer");
            dtShipmentGrid.Columns.Add("gvWeight");
            dtShipmentGrid.Columns.Add("gvLength");
            dtShipmentGrid.Columns.Add("gvWidth");
            dtShipmentGrid.Columns.Add("gvHeight");
            dtShipmentGrid.Columns.Add("gvWeightUnit");
            dtShipmentGrid.Columns.Add("gvDimensionUnit");
        }

        private void AddToDataTable()
        {
            int records = 0;
            records = Convert.ToInt32(txtCount.Text);
            for (int i = 0; i < records; i++)
            {
                DataRow dtRow = dtShipmentGrid.NewRow();
                dtRow["gvParcel"] = txtParcelNo.Text;
                dtRow["gvContent"] = txtContentType.Text;
                dtRow["gvContainer"] = ddlContainer.SelectedValue;
                dtRow["gvWeight"] = txtContainerGrossWeight.Text;
                dtRow["gvLength"] = txtLength.Text;
                dtRow["gvWidth"] = txtWidth.Text;
                dtRow["gvHeight"] = txtHeight.Text;
                dtRow["gvWeightUnit"] = ddlWeightUnit.SelectedValue;
                dtRow["gvDimensionUnit"] = ddlDimensionUnit.SelectedValue;
                dtShipmentGrid.Rows.Add(dtRow);

                int ParcelCount = Convert.ToInt32(txtParcelNo.Text);
                float TotalWeight = (float)(Convert.ToDouble(txtTotalGrossWeight.Text) + Convert.ToDouble(txtContainerGrossWeight.Text));

                txtParcelNo.Text = (ParcelCount + 1).ToString();
                txtTotalParcelNo.Text = ParcelCount.ToString();
                txtTotalGrossWeight.Text = TotalWeight.ToString();

            }
            

            ViewState["dtShipGrid"] = dtShipmentGrid;
        }

       

        private void BindGrid()
        {
            gv_Shipment.DataSource = dtShipmentGrid;
            gv_Shipment.DataBind();
        }

        private DataTable copytotable()
        {
            DataTable dt = new DataTable();
            List<SShipmentDetails> ss = new List<SShipmentDetails>();
            ss = KaizosSession.Current.ShipmentDetail;

            dt.Columns.Add("gvParcel");
            dt.Columns.Add("gvContent");
            dt.Columns.Add("gvContainer");
            dt.Columns.Add("gvWeight");
            dt.Columns.Add("gvLength");
            dt.Columns.Add("gvWidth");
            dt.Columns.Add("gvHeight");
            dt.Columns.Add("gvWeightUnit");
            dt.Columns.Add("gvDimensionUnit");

            foreach (SShipmentDetails aa in ss)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["gvParcel"] = aa.ParcelNo;
                dtRow["gvContent"] = aa.ContentType;
                dtRow["gvContainer"] = aa.Container;
                dtRow["gvWeight"] = aa.Weight;
                dtRow["gvLength"] = aa.Length;
                dtRow["gvWidth"] = aa.Width;
                dtRow["gvHeight"] = aa.Height;
                dtRow["gvWeightUnit"] = aa.WeightUnit;
                dtRow["gvDimensionUnit"] = aa.DimensionUnit;
                dt.Rows.Add(dtRow);
            }

            return dt;
        }

        private List<SShipmentDetails> CopyDataTableToServiceObject()
        {
            //Taking UOD details
            List<SShipmentDetails> sShipmentDetails = new List<SShipmentDetails>();
            int iRowNumber = 1;
            foreach (DataRow drow in dtShipmentGrid.Rows)
            {
                SShipmentDetails s = new SShipmentDetails();
                s.ParcelNo = iRowNumber++;
                //s.ParcelNo = Convert.ToInt32(drow["gvParcel"]);
                s.Container = drow["gvContainer"].ToString();
                s.ContentType = drow["gvContent"].ToString();
                s.DimensionUnit = drow["gvDimensionUnit"].ToString();
                s.Height = (float)Convert.ToDouble(drow["gvHeight"]);
                s.Length = (float)Convert.ToDouble(drow["gvLength"]);
                s.Weight = (float)Convert.ToDouble(drow["gvWeight"]);
                s.WeightUnit = drow["gvWeightUnit"].ToString();
                s.Width = (float)Convert.ToDouble(drow["gvWidth"]);
                sShipmentDetails.Add(s);
            }
            return sShipmentDetails;
        }

        private void ResetShipmentFields()
        {
            // Calculations
            //int ParcelCount = Convert.ToInt32(txtParcelNo.Text);
            //float TotalWeight = (float)(Convert.ToDouble(txtTotalGrossWeight.Text) + Convert.ToDouble(txtContainerGrossWeight.Text));

            //txtParcelNo.Text = (ParcelCount + 1).ToString();
            //txtTotalParcelNo.Text = ParcelCount.ToString();
            //txtTotalGrossWeight.Text = TotalWeight.ToString();
            txtContentType.Text = "";
            txtContainerGrossWeight.Text = "";
            txtLength.Text = "";
            txtWidth.Text = "";
            txtHeight.Text = "";
        }

        private void PassFormValue()
        {
            //string strSenderZip = txtZipCodeCity.Text;
            //string strRecipentZip = txtRZipCodeCity.Text;
            string option = "";
            string strSenderZip = "";   // txtZipCodeCity.Text;
            string strRecipentZip = ""; // txtRZipCodeCity.Text;

            string strSenderCountryCode = ddlCountry.SelectedValue;
            string strRecipientCountryCode = ddlRCountry.SelectedValue;
            string strSenderCity = "";
            string strRecipientCity = "";

            option = CheckboxListSelections(cblOptions);
            if (option == "")
            {
            }
            else
            option = option.Substring(0, option.Length - 1);


            strSenderCountryCode = strSenderCountryCode.Substring(0, strSenderCountryCode.IndexOf("-")).Trim();
            strRecipientCountryCode = strRecipientCountryCode.Substring(0, strRecipientCountryCode.IndexOf("-")).Trim();

            strSenderCity = txtCity.Text.Trim();
            strSenderZip = txtZip.Text.Trim();

            strRecipientCity = txtRCity.Text.Trim();
            strRecipentZip = txtRZipCode.Text.Trim();

            //if (strSenderZip.Contains("-"))
            //{
            //    strSenderCity = strSenderZip.Substring(strSenderZip.IndexOf("-") + 1).Trim();
            //    strSenderZip = strSenderZip.Substring(0, strSenderZip.IndexOf("-")).Trim();

            //}
            //if (strRecipentZip.Contains("-"))
            //{
            //    strRecipientCity = strRecipentZip.Substring(strRecipentZip.IndexOf("-") + 1).Trim();
            //    strRecipentZip = strRecipentZip.Substring(0, strRecipentZip.IndexOf("-")).Trim();
            //}
            string format = "dd/MM/yyyy";

            //Copies Gridview values to Service shipment object
            KaizosSession.Current.ShipmentDetail = CopyDataTableToServiceObject();

            // Form data 
            //KaizosSession.Current.AccountNo = "123";
            KaizosSession.Current.GrossWeight = (float)Convert.ToDouble(txtTotalGrossWeight.Text);
            KaizosSession.Current.ParcelCount = Convert.ToInt32(txtTotalParcelNo.Text);
            KaizosSession.Current.RecipientAddressType = (rdAddressType1.Checked) ? SEnumAddressType.Company : SEnumAddressType.Residential;
            KaizosSession.Current.RecipientCountryCode = strRecipientCountryCode;
            KaizosSession.Current.RecipientCountry = ddlRCountry.SelectedValue;
            KaizosSession.Current.RecipientZip = strRecipentZip;
            KaizosSession.Current.SenderCountryCode = strSenderCountryCode;
            KaizosSession.Current.SenderCountry = ddlCountry.SelectedValue;
            KaizosSession.Current.SenderZip = strSenderZip;
            KaizosSession.Current.SenderCity = strSenderCity;
            KaizosSession.Current.RecipientCity = strRecipientCity;
            KaizosSession.Current.Options = option;


            if (txtShipingDate.Text.Equals(""))
                KaizosSession.Current.ShippingDate = DateTime.Now;
            else
                KaizosSession.Current.ShippingDate = DateTime.ParseExact(txtShipingDate.Text, format, CultureInfo.InvariantCulture);
            //KaizosSession.Current.ShippingDate = Convert.ToDateTime(txtShipingDate.Text);

        }

        protected void val_Shipment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            decimal d;
            int i;
            DateTime value;
            string DateFormat = "dd/MM/yyyy";
            DateTime MaxDate = AddDays(DateTime.Now, 14);

            /***1301 08MAY12 KM ***/
            if (txtShipingDate.Text.Equals(""))
            {
                Dday = DateTime.Now;
                weekday = (int)Dday.DayOfWeek;
            }
            else
            {
                Dday = DateTime.ParseExact(txtShipingDate.Text, DateFormat, CultureInfo.InvariantCulture);
                weekday = (int)Dday.DayOfWeek;
            }

            //if ((txtCity.Text.Trim().Length > 0) || (txtZip.Text.Trim().Length > 0))
            //{
            //    if (!(proxy.isAlphaNumericValidation(txtCity.Text)) || (!(proxy.isAlphaNumericValidation(txtZip.Text))))
            //    {
            //        strError = strError + "*" + lblLegendSender.Text + " " + lblZipCodeCity.Text.Trim() + " " + valInvalid.Text.Trim() + ". <br>";
            //        args.IsValid = false;
            //    }
            //}

            //if ((txtRCity.Text.Trim().Length > 0) || (txtRZipCode.Text.Trim().Length > 0))
            //{
            //    if (!(proxy.isAlphaNumericValidation(txtRCity.Text)) || (!(proxy.isAlphaNumericValidation(txtRZipCode.Text))))
            //    {
            //        strError = strError + "*" + lgRecipient.Text + " " + lblRZipCode.Text.Trim() + " " + valInvalid.Text.Trim() + ". <br>";
            //        args.IsValid = false;
            //    }
            //}

            if ((txtZip.Text.Trim().Length > 0))
            {
                if (!(proxy.isAlphaNumericValidation(txtZip.Text)))
                {
                    strError = strError + "*" + lblLegendSender.Text + " " + lblZipCodeCity.Text.Trim() + " " + valInvalid.Text.Trim() + ". <br>";
                    args.IsValid = false;
                }
            }


            if ((txtRZipCode.Text.Trim().Length > 0))
            {
                if (!(proxy.isAlphaNumericValidation(txtRZipCode.Text)))
                {
                    strError = strError + "*" + lgRecipient.Text + " " + lblRZipCode.Text.Trim() + " " + valInvalid.Text.Trim() + ". <br>";
                    args.IsValid = false;
                }
            }

            if (txtShipingDate.Text.Equals(""))
            {
                strError = strError + "*" + lblShipDate.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (!DateTime.TryParseExact(txtShipingDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                strError = strError + "*" + lblShipDate.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ( (DateTime.ParseExact(txtShipingDate.Text, DateFormat, CultureInfo.InvariantCulture) < Convert.ToDateTime(DateTime.Now.ToShortDateString())) ||
                (DateTime.ParseExact(txtShipingDate.Text, DateFormat, CultureInfo.InvariantCulture) > Convert.ToDateTime(MaxDate.ToShortDateString())) )
                           //(DateTime.ParseExact(txtShipingDate.Text, DateFormat, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now.AddDays(14).ToShortDateString())) )
                
            {
                strError = strError + "*" + lblShipDate.Text.Trim() + " " + valBetween.Text.Trim() + " " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " & " + MaxDate.ToString("dd/MM/yyyy") + "<br>";
                args.IsValid = false;
            }

            /***1301 08MAY12 KM ***/
            else if (weekday == 0 || weekday == 6)
            {
                strError = strError + "*" + lblShipDate.Text.Trim() + " " + valWeekend.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (txtContentType.Text.Equals(""))
            {
                strError = strError + "*" + lblContentType.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            //else if(!(proxy.isAlphaNumericValidation(txtContentType.Text.Trim())))
            //{
            //    strError = strError + "*" + lblContentType.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
            //    args.IsValid = false;
            //}

            if (txtCount.Text.Equals(""))
            {
                strError = strError + "*" + lblParcelNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtCount.Equals(""))) && (!Int32.TryParse(txtCount.Text, out i)))
            {
                strError = strError + "*" + lblParcelNumber.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDouble(txtCount.Text) <= 0)
            {
                strError = strError + "*" + lblParcelNumber.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!isVaidCount(txtCount.Text)))
            {
                //strError = strError + "*" + lblParcelNumber.Text.Trim() + " " + valLess.Text.Trim() + " 100 <br>";
                strError = strError + valMaxAllowed.Text.Trim();
                args.IsValid = false;
            }


            if (txtContainerGrossWeight.Text.Equals(""))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtContainerGrossWeight.Equals(""))) && (!Decimal.TryParse(txtContainerGrossWeight.Text, out d)))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDouble(txtContainerGrossWeight.Text) <= 0)
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!isValidWeight(txtContainerGrossWeight.Text)))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valLess.Text.Trim() + " 1000 Kg<br>";
                args.IsValid = false;
            }

            if (txtLength.Text.Equals(""))
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtLength.Equals(""))) && (!Decimal.TryParse(txtLength.Text, out d)))
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDouble(txtLength.Text) <= 0)
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!isValidSize(txtLength.Text))) //(Convert.ToDouble(txtLength.Text) > 999.99)
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                args.IsValid = false;
            }

            if (txtWidth.Text.Equals(""))
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtWidth.Equals(""))) && (!Decimal.TryParse(txtWidth.Text, out d)))
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDouble(txtWidth.Text) <= 0)
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!isValidSize(txtWidth.Text))) //(Convert.ToDouble(txtWidth.Text) > 999.99)
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                args.IsValid = false;
            }


            if (txtHeight.Text.Equals(""))
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!(txtHeight.Equals(""))) && (!Decimal.TryParse(txtHeight.Text, out d)))
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (Convert.ToDouble(txtHeight.Text) <= 0)
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if ((!isValidSize(txtHeight.Text))) //(Convert.ToDouble(txtHeight.Text) > 999.99)
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                args.IsValid = false;
            }

            if ((Convert.ToInt32(txtParcelNo.Text.Trim()) > 100) && (!(txtParcelNo.Text.Trim().Equals(""))))
            {
                strError = strError + valMaxAllowed.Text.Trim();
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
				errorMsg1.Attributes["style"] = "display: block;";
				val_Shipment.ErrorMessage = strError;
            }
        }

        protected void btnAddShipment_Click(object sender, EventArgs e)
        {
            lblGridValudation.Visible = false;
            if (IsValid)
            {
                AddToDataTable();
                BindGrid();
                ResetShipmentFields();
            }
        }


        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            string strSenderCountryCode = ddlCountry.SelectedValue;
            string strRecipientCountryCode = ddlRCountry.SelectedValue;
            strSenderCountryCode = strSenderCountryCode.Substring(0, strSenderCountryCode.IndexOf("-")).Trim();
            strRecipientCountryCode = strRecipientCountryCode.Substring(0, strRecipientCountryCode.IndexOf("-")).Trim();

            City = proxy.CityZipcodeAutoFill(prefixText, strSenderCountryCode, count);
            string[] arrCity = City.ToArray();
            return arrCity;

        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListCity(string prefixText, int count, string contextKey)
        {
            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            City = proxy.CityZipcodeAutoFill(prefixText, AutoSenderCountry, count);
            string[] arrCity = City.ToArray();
            //string[] Final = new string[arrCity.Count()];
            //string[] Temp = new string[2];
            //string strCity = "";
            //for (int i = 0; i < arrCity.Count(); i++)g
            //{
            //    Temp = arrCity[i].Split('+');
            //    strCity = Temp[1];
            //    Final[i] = strCity;
            //}
            //List<string> lArray = new List<string>();

            //for (int i = 0; i < Final.Count(); i++)
            //{
            //    if (!(lArray.Contains(Final[i])))
            //        lArray.Add(Final[i]);
            //}
            //Final = lArray.ToArray();

            //return Final;
            return arrCity;
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListZip(string prefixText, int count, string contextKey)
        {
            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            City = proxy.CityZipcodeAutoFill(prefixText, AutoSenderCountry, count);
            string[] arrCity = City.ToArray();
            //string[] Final = new string[arrCity.Count()];
            //string[] Temp = new string[2];
            //string strCity = "";
            //for (int i = 0; i < arrCity.Count(); i++)
            //{
            //    Temp = arrCity[i].Split('+');
            //    strCity = Temp[0];
            //    Final[i] = strCity;
            //}
            //List<string> lArray = new List<string>();

            //for (int i = 0; i < Final.Count(); i++)
            //{
            //    if (!(lArray.Contains(Final[i])))
            //        lArray.Add(Final[i]);
            //}
            //Final = lArray.ToArray();

            //return Final;

            return arrCity;
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListRCity(string prefixText, int count, string contextKey)
        {
            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            City = proxy.CityZipcodeAutoFill(prefixText, AutoReceiverCountry, count);
            string[] arrCity = City.ToArray();

            return arrCity;
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListRZip(string prefixText, int count, string contextKey)
        {
            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            City = proxy.CityZipcodeAutoFill(prefixText, AutoReceiverCountry, count);
            string[] arrCity = City.ToArray();
            return arrCity;
        }

        protected bool isValidZipcode(string ZipCode)
        {
            bool result = true;

            if (!(ZipCode.Equals("")) &&
                   (!(proxy.isAlphaNumericValidation(ZipCode)) || (ZipCode.Split('-').Length != 2)))
            {
                result = false;
            }
            else if (!(ZipCode.Equals("")))
            {
                string[] List = ZipCode.Split('-');
                if ((List[0].Trim().Equals("")) || (List[1].Trim().Equals("")))
                {
                    result = false;
                }
            }

            return result;
        }

        protected bool isValidDate(string Date)
        {
            bool result = true;
            DateTime d;

            if (!(DateTime.TryParse(Date, out d)))
            {
                result = false;
            }

            return result;
        }

        protected bool isValidWeight(string Weight)
        {
            bool result = true;
            double ConvertedWeight = ConvertToKg(Weight, ddlWeightUnit.SelectedValue.Trim());

            if (ConvertedWeight > 1000)
            {
                result = false;
            }
            return result;
        }

        protected bool isValidSize(string Size)
        {
            bool result = true;
            double ConvertedWeight = ConvertToCM(Size, ddlDimensionUnit.SelectedValue.Trim());

            if (ConvertedWeight > 300)
            {
                result = false;
            }
            return result;

        }

        protected bool isVaidCount(string Value)
        {
            bool result = true;
            int Count = Convert.ToInt32(Value) + Convert.ToInt32(txtParcelNo.Text) - 1;

            if (Count > 100)
            {
                result = false;
            }
            return result;

        }


        protected double ConvertToCM(string Dimension, string DimensionUnit)
        {
            double result = 0;
            result = Convert.ToDouble(Dimension);
            if (DimensionUnit.Equals("in"))
            {
                result = result * 2.54;
            }
            return result;
        }

        protected double ConvertToKg(string Weight, string WeightUnit)
        {
            double result = 0;
            result = Convert.ToDouble(Weight);
            if (WeightUnit.Equals("lbs"))
            {
                result = result * 0.45359237;
            }
            return result;
        }

        protected void gv_Shipment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (gv_Shipment.Rows.Count > 1)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //string ContainerWeight = gv_Shipment.Rows[row.RowIndex].Cells[3].Text;
                    //DropDownList ddlCarrList = e.Row.FindControl("dllPublicCarrier") as DropDownList;
                    Label lblText = gv_Shipment.Rows[row.RowIndex].FindControl("lblWeight") as Label;
                    string ContainerWeight = lblText.Text.Trim();
                    gv_Shipment.DeleteRow(row.RowIndex);
                    dtShipmentGrid.Rows.RemoveAt(row.RowIndex);
                    BindGrid();

                    int ParcelCount = Convert.ToInt32(txtParcelNo.Text) - 1;
                    int TotalParcelCount = Convert.ToInt32(txtTotalParcelNo.Text) - 1;
                    //int TotalWeight = Convert.ToInt32(txtTotalGrossWeight.Text) - Convert.ToInt32(ContainerWeight);  18JAN12HN
                    float TotalWeight = (float)(Convert.ToDouble(txtTotalGrossWeight.Text) - Convert.ToDouble(ContainerWeight));

                    txtParcelNo.Text = ParcelCount.ToString();
                    txtTotalParcelNo.Text = TotalParcelCount.ToString();
                    txtTotalGrossWeight.Text = TotalWeight.ToString();
                }
            }

        }

        protected void gv_Shipment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gv_Shipment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_Shipment.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gv_Shipment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblGridValudation.Visible = false;
            gv_Shipment.EditIndex = -1;
            BindGrid();
        }

        protected void gv_Shipment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string CurrentWeight = "";
            string NewWeight = "";
            string strContent = "";
            string strContainer = "";
            string strWeight = "";
            string strLength = "";
            string strWidth = "";
            string strHeight = "";


            TextBox TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtContentType");
            strContent = TextBox.Text;

            //TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtContainer");
            //strContainer = TextBox.Text;

            DropDownList ddlList = (DropDownList)gv_Shipment.Rows[e.RowIndex].FindControl("ddlgvContainer");
            strContainer = ddlList.SelectedValue.Trim();

            TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtWeight");
            strWeight = TextBox.Text;

            CurrentWeight = dtShipmentGrid.Rows[e.RowIndex]["gvWeight"].ToString();
            NewWeight = TextBox.Text;

            TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtLength");
            strLength = TextBox.Text;

            TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtWidth");
            strWidth = TextBox.Text;

            TextBox = (TextBox)gv_Shipment.Rows[e.RowIndex].FindControl("txtHeight");
            strHeight = TextBox.Text;

            if (IsValidData(strContent, strWeight, strLength, strWidth, strHeight))
            {
                lblGridValudation.Visible = false;

                float TotalWeight = (float)(Convert.ToDouble(txtTotalGrossWeight.Text) - Convert.ToDouble(CurrentWeight));
                TotalWeight = (float)(TotalWeight + Convert.ToDouble(NewWeight));
                txtTotalGrossWeight.Text = TotalWeight.ToString("######0.00");

                dtShipmentGrid.Rows[e.RowIndex]["gvContent"] = strContent.Trim();
                dtShipmentGrid.Rows[e.RowIndex]["gvContainer"] = strContainer.Trim();
                dtShipmentGrid.Rows[e.RowIndex]["gvLength"] = strLength.Trim();
                dtShipmentGrid.Rows[e.RowIndex]["gvWidth"] = strWidth.Trim();
                dtShipmentGrid.Rows[e.RowIndex]["gvHeight"] = strHeight.Trim();
                dtShipmentGrid.Rows[e.RowIndex]["gvWeight"] = strWeight.Trim();

                gv_Shipment.EditIndex = -1;
                BindGrid();
            }

        }

        protected void gv_Shipment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gv_Shipment.EditIndex == e.Row.RowIndex && e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlList = (DropDownList)e.Row.Cells[2].FindControl("ddlgvContainer");

                List<SComboText> sComboText = new List<SComboText>();
                SComboTableField sComboTableField = new SComboTableField();

                //To fill Container drop down list
                sComboTableField.FieldName = "CONTAINER_TYPE";
                sComboTableField.TableName = "CONTAINER";
                sComboText = proxy.FillCombo(sComboTableField).ToList();
                ddlList.DataSource = sComboText;
                ddlList.DataTextField = "ComboText";
                ddlList.DataBind();
                ddlList.SelectedValue = ((Label)e.Row.Cells[2].FindControl("lblContainer1")).Text.Trim();
            }

        }

        protected bool IsValidData(string strContent, string strWeight, string strLength, string strWidth, string strHeight)
        {
            bool result = true;
            string strError = "";
            decimal d;
            lblGridValudation.Visible = false;

            if (strContent.Equals(""))
            {
                strError = strError + "*" + lblContentType.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                result = false;
            }
            //else if (!(proxy.isAlphaNumericValidation(strContent.Trim())))
            //{
            //    strError = strError + "*" + lblContentType.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
            //    result = false;
            //}

            if (strWeight.Equals(""))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!(strWeight.Equals(""))) && (!Decimal.TryParse(strWeight, out d)))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                result = false;
            }
            else if (Convert.ToDouble(strWeight) <= 0)
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!isValidWeight(strWeight)))
            {
                strError = strError + "*" + lblContainerGrossWeight.Text.Trim() + " " + valLess.Text.Trim() + " 1000 Kg<br>";
                result = false;
            }

            if (strLength.Equals(""))
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!(strLength.Equals(""))) && (!Decimal.TryParse(strLength, out d)))
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                result = false;
            }
            else if (Convert.ToDouble(strLength) <= 0)
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!isValidSize(strLength))) //(Convert.ToDouble(strLength) > 999.99)
            {
                strError = strError + "*" + lblLength.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                result = false;
            }

            if (strWidth.Equals(""))
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!(strWidth.Equals(""))) && (!Decimal.TryParse(strWidth, out d)))
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                result = false;
            }
            else if (Convert.ToDouble(strWidth) <= 0)
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!isValidSize(strWidth))) //(Convert.ToDouble(strWidth) > 999.99)
            {
                strError = strError + "*" + lblWidht.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                result = false;
            }


            if (strHeight.Equals(""))
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!(strHeight.Equals(""))) && (!Decimal.TryParse(strHeight, out d)))
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                result = false;
            }
            else if (Convert.ToDouble(strHeight) <= 0)
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valPossitive.Text.Trim() + "<br>";
                result = false;
            }
            else if ((!isValidSize(strHeight))) //(Convert.ToDouble(strHeight) > 999.99)
            {
                strError = strError + "*" + lblHeight.Text.Trim() + " " + valLess.Text.Trim() + " 300 cm<br>";
                result = false;
            }

            if (!(result))
            {
                lblGridValudation.Visible = true;
                lblGridValudation.Text = strError.Trim();
                errorMsg2.Attributes["style"] = "display: block;";
            }

            return result;

        }

        protected void txtCity_TextChanged(object sender, EventArgs e)
        {
            //txtZip.Text = GetZip(txtCity.Text, "S");
            GetCity(txtCity.Text, "S");
        }

        protected void txtZip_TextChanged(object sender, EventArgs e)
        {
            //txtCity.Text = GetCity(txtZip.Text,"S");
            GetCity(txtZip.Text, "S");
        }

        protected void GetCity(string ZipCode, string Type)
        {
            string result = "";

            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();
            string[] Temp = new string[2];
            Temp = ZipCode.Split('-');
            //City = proxy.CityZipcodeAutoFill(Temp[1], 10);
            string sencity = "";
            string zip = "";
            if (Temp.Length == 2)
            {
                if (Type.Equals("S"))
                {
                    txtZip.Text = Temp[1];
                    txtCity.Text = Temp[0];
                }
                else
                {
                    txtRZipCode.Text = Temp[1];
                    txtRCity.Text = Temp[0];
                }
            }
            else if (Temp.Length == 1)
            {

            }
            else
            {
                for (int i = 0; i < Temp.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        sencity = Temp[i];
                    }
                    else
                        sencity = sencity + "-" + Temp[i];
                }


                int k = Temp.Length - 1;
                zip = Temp[k];
                if (Type.Equals("S"))
                {
                    txtZip.Text = zip;
                    txtCity.Text = sencity;
                }
                else
                {
                    txtRZipCode.Text = zip;
                    txtRCity.Text = sencity;
                }

            }
            //string[] arrCity = City.ToArray();
            //string[] Final = new string[arrCity.Count()];
            ////string[] Temp = new string[2];
            //string strCity = "";
            //for (int i = 0; i < arrCity.Count(); i++)
            //{
            //    Temp = arrCity[i].Split('+');
            //    strCity = Temp[1];
            //    Final[i] = strCity;
            //}
            //result = Final[0];
            //return result;
        }

        protected string GetZip(string strCity, string Type)
        {
            string result = "";

            List<string> City = new List<string>();
            KaizosServiceAgent proxy = new KaizosServiceAgent();
            string[] Temp = new string[2];
            Temp = strCity.Split('-');
            

            if (Type.Equals("S"))
            {
                City = proxy.CityZipcodeAutoFill(Temp[1], AutoSenderCountry,  10);
                txtCity.Text = Temp[1];
            }
            else
            {
                City = proxy.CityZipcodeAutoFill(Temp[1], AutoReceiverCountry, 10);
                txtRCity.Text = Temp[1];
            }
            //City = proxy.CityZipcodeAutoFill(txtCity.Text.Trim(), 10);
            string[] arrCity = City.ToArray();
            string[] Final = new string[arrCity.Count()];
            //string[] Temp = new string[2];
            string strZip = "";
            for (int i = 0; i < arrCity.Count(); i++)
            {
                Temp = arrCity[i].Split('+');
                strZip = Temp[0];
                Final[i] = strZip;
            }
            result = Final[0];


            return result;
        }

        protected void txtRCity_TextChanged(object sender, EventArgs e)
        {
            //txtRZipCode.Text = GetZip(txtRCity.Text, "R"); ;
            GetCity(txtRCity.Text, "R");
        }

        protected void txtRZipCode_TextChanged(object sender, EventArgs e)
        {
            //txtRCity.Text = GetCity(txtRZipCode.Text, "R");
            GetCity(txtRZipCode.Text, "R");
        }

        //protected DateTime AddBusinessDays1(DateTime current, int days)
        //{
        //    DateTime result;
        //    var sign = Math.Sign(days);
        //    var unsignedDays = Math.Abs(days);
        //    for (var i = 0; i < unsignedDays; i++)
        //    {
        //        do
        //        {
        //            current = current.AddDays(sign);
        //        }
        //        while (current.DayOfWeek == DayOfWeek.Saturday ||
        //            current.DayOfWeek == DayOfWeek.Sunday);
        //    }
        //    result = current;
        //    return result;
        //}

        protected DateTime AddDays(DateTime current, int days)
        {

            DateTime result = DateTime.Now;
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                    current.DayOfWeek == DayOfWeek.Sunday);
            }
            result = current;

            return result;

        
        }

        /********************[19MAR12KS]*******************/
        public string CheckboxListSelections(System.Web.UI.WebControls.CheckBoxList list)
        {
            string values = "";
            for (int counter = 0; counter < list.Items.Count; counter++)
            {
                if (list.Items[counter].Selected)
                {
                    values = values + "'" + list.Items[counter].Value + "'" + ",";
                }
            }
            return values;
        }



     

    }
}