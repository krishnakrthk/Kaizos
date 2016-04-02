using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.IO;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmTariffImportation : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffCreation));

        DataTable dtCalculationGrid;

        protected void GenerateCalculationTable()
        {
            /*
            List<STariffCalculationRule> sTariffCalculationRule = new List<STariffCalculationRule>();
            STariffCalculationRule single = new STariffCalculationRule();
            single.ZoneList             = "ZoneList";
            single.GrossMargin          = "Gross Margin";
            single.ServiceTypeCode      = "Service Type Code";
            sTariffCalculationRule.Add(single);
            gvCalculationRule.DataSource = sTariffCalculationRule;
            gvCalculationRule.DataBind();
            ViewState["CalculationRule"] = sTariffCalculationRule;  //store it for first time
             * */
        }

        protected void FillCalculationTable()
        {
            /*
            if (ViewState["CalculationRule"] != null)
            {
                gvCalculationRule.DataSource = (List<STariffCalculationRule>)Session["CalculationRule"];
                gvCalculationRule.DataBind();
            }
            else
            {
                GenerateCalculationTable(); //for first time
            }
             * */
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmTariffImporation").ToString();
                /* Load carrier name in the drop down list for the first time */
                FillCarrierCombo();
                dtCalculationGrid = new DataTable();
                MakeDataTable();

                /* Defaulted Tariff drop down list [20JAN12RM] */
                ddlCarrier_TextChanged(this, EventArgs.Empty);

            }
            else
            {
                dtCalculationGrid = (DataTable)ViewState["dtCalculationGrid"];
            }

            ViewState["dtCalculationGrid"] = dtCalculationGrid;
			errorMsg1.Attributes["style"] = "display: none;";
			errorMsg2.Attributes["style"] = "display: none;";

        }

        protected void FillCarrierCombo()
        {
            /* [20JAN12RM] Introduced try catch */
            try 
            {
                /* 1.create proxy */
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<SComboText> sComboText = new List<SComboText>();

                SComboTableField sComboTableField = new SComboTableField();

                //To fill Origin drop down list
                sComboTableField.FieldName = "CARRIER_NAME";
                sComboTableField.TableName = "CARRIER_MAST";
                sComboText = proxy.FillCombo(sComboTableField).ToList();

                ddlCarrier.DataSource = sComboText;
                ddlCarrier.DataTextField = "ComboText";
                ddlCarrier.DataBind();
            }
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmTariffImportation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "FillCarrierCombo()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmTariffImportation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
             }



        }

        protected void FillTariffReferenceCombo()
        {
            /* [20JAN12RM] handled try/catch exceptions */
            try
            {
                /* 1.create proxy */
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                List<String> lsTariffRef = proxy.GetOpenImportTariff(ddlCarrier.SelectedValue);
                ddlTariffReference.DataSource = lsTariffRef;
                ddlTariffReference.DataBind();
                ddlTariffReference.SelectedIndex = -1;

            }
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmTariffImportation.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "FillTariffReferenceCombo()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmTariffImportation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
             }

        }

        protected void ddlCarrier_TextChanged(object sender, EventArgs e)
        {
            FillTariffReferenceCombo();
        }

        protected void btnAddLine_Click(object sender, EventArgs e)
        {
			if (val_Rule.IsValid) /* Introduced validation [20JAN12RM] */
			{
				AddToDataTable();
				BindGrid();
				txtServiceType.Text = "";
				txtZoneList.Text = "";
				txtGrossMargin.Text = "";
			}
			else {
				errorMsg2.Attributes["style"] = "display: block;";
			}
        }

        private void MakeDataTable()
        {
            dtCalculationGrid.Columns.Add("ServiceTypeCode");
            dtCalculationGrid.Columns.Add("ZoneList");
            dtCalculationGrid.Columns.Add("GrossMargin");
        }

        private void AddToDataTable()
        {
            DataRow dtRow = dtCalculationGrid.NewRow();
            dtRow["ServiceTypeCode"]    = txtServiceType.Text.Trim();
            dtRow["ZoneList"]           = txtZoneList.Text.Trim();
            dtRow["GrossMargin"]        = txtGrossMargin.Text.Trim();
            dtCalculationGrid.Rows.Add(dtRow);
            //ViewState["dtShipGrid"] = dtCalculationGrid;
        }

        private void BindGrid()
        {
            gv_Calculation.DataSource = dtCalculationGrid;
            gv_Calculation.DataBind();
        }

        protected void gv_Calculation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gv_Calculation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                gv_Calculation.DeleteRow(row.RowIndex);
                dtCalculationGrid.Rows.RemoveAt(row.RowIndex);
                BindGrid();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            
            try
            {
				if (val_Shipment.IsValid)
				{
					/* 1.Collect Calculation Rule */
					List<STariffCalculationRule> sTariffCalculationRule = FormCaculationRuleFromTable();

					/* 2.create proxy */
					KaizosServiceAgent proxy = new KaizosServiceAgent();

					//string strImageName = "sample";
					if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.FileName != "")
					{

						byte[] tariffFile = new byte[FileUpload1.PostedFile.ContentLength];

						HttpPostedFile uploadedImage = FileUpload1.PostedFile;

						uploadedImage.InputStream.Read(tariffFile, 0, (int)FileUpload1.PostedFile.ContentLength);

						string strDisplayMsg = "";

						string CarrierName = ddlCarrier.SelectedValue.Trim();
						string TariffRef = ddlTariffReference.SelectedValue.Trim();


						List<SFileImportStatus> sFileImportStatus = proxy.ImportTariff(CarrierName, TariffRef, sTariffCalculationRule, tariffFile);


						string ImportResult = "";

						if (sFileImportStatus.Count == 1)
						{
							ImportResult = sFileImportStatus[0].ErrorDescription;
						}
						else
						{

							//Imported successfully except following data
							string Temp = "<h1>" + GetGlobalResourceObject("LocalString", "FileImportStatus").ToString() + "</h1> <br>";

							Temp = Temp + "<table border='1'>" +
											"<tr>" +
												"<td>" +
												   "Line #" +
												"</td>" +
												"<td>" +
												   "Field" +
												"</td>" +
												"<td>" +
												   "Error Description" +
												"</td>" +
											"</tr>";


							for (int i = 1; i < sFileImportStatus.Count; i++)
							{
								Temp = Temp +

												"<tr>" +
												"<td>" +
												   sFileImportStatus[i].RowNumber.ToString() +
												"</td>" +
												"<td>" +
												   sFileImportStatus[i].FieldName +
												"</td>" +
												"<td>" +
												   sFileImportStatus[i].ErrorDescription +
												"</td>" +
											"</tr>";

							}

							Temp = Temp + "</table>";

							ImportResult = Temp;
						}

						string strReturnUrl = "frmMasterServiceTypeUpdate.aspx";
						KaizosSession.Current.ErrorMessage = ImportResult;
						KaizosSession.Current.ReturnURL = strReturnUrl;
						Server.Transfer("frmResult.aspx", false);

					}
				}
				else {
					errorMsg1.Attributes["style"] = "display: block;";
				}

            }
                /* Introduced faultexception handling and logging detailed exception into log4net file [18JAN12RM] */
                catch (FaultException<SGeneralFault> ex)
                {

                    string ErrorDetails = ex.Detail.Details;
                    string MethodName   = ex.Detail.Issue;

                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmTariffImportation.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [18JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnCreate_Click()", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL     = "frmTariffImportation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
        }

        protected List<STariffCalculationRule> FormCaculationRuleFromTable()
        {
            List<STariffCalculationRule> sTariffCalculationRule = new List<STariffCalculationRule>();

            for (int i = 0; i < dtCalculationGrid.Rows.Count; i++)
            {
                STariffCalculationRule rule = new STariffCalculationRule();
                rule.ServiceTypeCode    = dtCalculationGrid.Rows[i]["ServiceTypeCode"].ToString().Trim();
                rule.ZoneList           = dtCalculationGrid.Rows[i]["ZoneList"].ToString().Trim();
                rule.GrossMargin        = dtCalculationGrid.Rows[i]["GrossMargin"].ToString().Trim();
                sTariffCalculationRule.Add(rule);
            }

            return sTariffCalculationRule;
        }

        protected void val_Shipment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string strError = "";
            
            /* 1. To check the file extension */
            string strFileExt = Path.GetExtension(FileUpload1.FileName).ToUpper();

            /* 2. Carrier must be selected [20JAN12RM] */
            if (ddlCarrier.SelectedIndex == -1)
            {
                strError = strError + "*" + lblCarrier.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            /* 3. Tariff must be selected [20JAN12RM]*/
            if (ddlTariffReference.SelectedIndex == -1)
            {
                strError = strError + "*" + lblTariffRef.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            /* 4. File must be chosen [20JAN12RM]*/
            if (! FileUpload1.HasFile)
            {
                strError = strError + "*" + Label1.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            else if (!strFileExt.Equals(".CSV"))
            {
                strError = strError + "*" + Label1.Text.Trim() + " " + valInvalidExtension.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            /* 5. To allow max of 10MB file size */
            int FileSizeinBytes = FileUpload1.PostedFile.ContentLength;
            
            float FileSizeinMB = ((FileSizeinBytes / 1024) / 1024);

            if (FileSizeinMB > 10)
            {
                strError = strError + "*" + valFileSize.Text.Trim() + " " + lblMax.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_Shipment.ErrorMessage = strError;
            }
        }

        protected void val_Rule_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string strError = "";

            /* Make sure the given margin is valid numeric value [20JAN12RM] */ 
            if (!isNumericValidation(txtGrossMargin.Text,System.Globalization.NumberStyles.Float))
            {
                strError = strError + "*" + Label4.Text.Trim() + " " + val_Numeric.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            /* Gross Margin should be less then or equal to 100 [20JAN12RM] */
            if (args.IsValid)
            {

                if (float.Parse(txtGrossMargin.Text) > 100)
                {
                    strError = strError + "*" + Label4.Text.Trim() + " " + val_lessthanN.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }

            /* Service type should be entered [20JAN12RM] */
            if (txtServiceType.Text.Length==0)
            {
                    strError = strError + "*" + lbl1.Text.Trim() + " " + val_Empty.Text.Trim() + "<br>";
                    args.IsValid = false;
            }

            /* zone should be entered [20JAN12RM] */
            if (txtZoneList.Text.Length==0)
            {
                    strError = strError + "*" + Label3.Text.Trim() + " " + val_Empty.Text.Trim() + "<br>";
                    args.IsValid = false;
            }
            
            if (!(args.IsValid))
            {
                val_Rule.ErrorMessage = strError;
            }
        }

        protected bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,System.Globalization.CultureInfo.CurrentCulture, out result);
        }
    }

    

}