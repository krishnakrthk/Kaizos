using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmTariffValidity : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffValidity));

        protected void FillCarrierCombo()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            lblDateValidation.Text = ""; //31JAN12RM bug ID 1130
			errorMsg1.Attributes["style"] = "display: none;";
			errorMsg2.Attributes["style"] = "display: none;";

            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmTariffValidity").ToString();
                FillCarrierCombo();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                gvTariffRef.EditIndex = -1; // to fix 1254 [06MAR12RM]
                FillTariffReferenceGrid(ddlCarrier.SelectedValue, rblTariffType.SelectedValue);
            }
            catch (Exception error)
            {
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                Response.Write(errorMessage);
                logger.Debug(errorMessage);
            }
        }

        protected void FillTariffReferenceGrid(string Carrier, string TariffType)
        {
            try
            {
                lblSearchError.Text     = "";
                gvTariffRef.DataSource  = null;
                gvTariffRef.DataBind();
                
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<STariffReferenceList> sTariffReferenceList  = proxy.GetTariffReference(Carrier, TariffType).ToList();
                
              
                if (sTariffReferenceList.Count != 0)
                {
                    gvTariffRef.DataSource = sTariffReferenceList.ToArray();
                    gvTariffRef.DataBind();
                }
                else
                {
                    lblSearchError.Text = "No records found for the given criteria !";
					errorMsg1.Attributes["style"] = "display: block;";
                }

            }
            catch (FaultException<SGeneralFault> sGeneralFault)
            {
                SGeneralFault fault = sGeneralFault.Detail;
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + fault.Issue + fault.Details;
                Response.Write(errorMessage);
                logger.Debug(errorMessage);
            }
            catch (Exception error)
            {
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                Response.Write(errorMessage);
                logger.Debug(errorMessage);
            }
        }

        protected void gvTariffRef_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblDateValidation.Text = ""; //31JAN12RM bug ID 1130
            gvTariffRef.EditIndex  = -1;
            FillTariffReferenceGrid(ddlCarrier.SelectedValue, rblTariffType.SelectedValue);
        }

        protected void gvTariffRef_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTariffRef.EditIndex = e.NewEditIndex;
            FillTariffReferenceGrid(ddlCarrier.SelectedValue, rblTariffType.SelectedValue);
        }

        //27FEB12RM [1236]
        protected string FormatDateString(string date)
        {
            //string format = "dd/MM/yyyy";
            //return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture).ToString();
            return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
        }
        /*06APR12KM*/
        /* To check and remove the tariff itself while archive it and before that put it in HISTORY DATABASE*/
        protected void gvTariffRef_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            /*1.Retrieve Start Date, End Date and Archived in <EditTemplate> */
            TextBox txtNewStartDate = (TextBox)gvTariffRef.Rows[e.RowIndex].FindControl("txtStartDate");
            TextBox txtNewEndDate = (TextBox)gvTariffRef.Rows[e.RowIndex].FindControl("txtEndDate");

            bool bArchived = ((CheckBox)gvTariffRef.Rows[e.RowIndex].FindControl("cbArchived")).Checked ? true : false;

            string formats = "dd/MM/yyyy";
            string com = ddlCarrier.SelectedItem.ToString();
            string opt = rblTariffType.SelectedItem.ToString();
            if (opt.Contains(" "))
            {
                string op = opt.Replace(" ", "");
                opt = op;
            }

            string TariffReference = gvTariffRef.DataKeys[e.RowIndex].Values[0].ToString();
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            STariffMaster sTariffMaster = new STariffMaster(); ;// = GetBusinessEntity();
            sTariffMaster.CarrierName = ddlCarrier.SelectedItem.ToString();
            if (rblTariffType.SelectedValue.Trim() == "Purchase")
                sTariffMaster.TariffType = SEnumTariffType.Purchase;
            else if (rblTariffType.SelectedValue.Trim() == "CarrierPublic")
                sTariffMaster.TariffType = SEnumTariffType.CarrierPublic;
            else if (rblTariffType.SelectedValue.Trim() == "KeyCustomer")
                sTariffMaster.TariffType = SEnumTariffType.Negotiated;
            sTariffMaster.StartDate = DateTime.ParseExact(txtNewStartDate.Text, formats, CultureInfo.InvariantCulture);
            sTariffMaster.EndDate = DateTime.ParseExact(txtNewEndDate.Text, formats, CultureInfo.InvariantCulture);
            sTariffMaster.TariffReference = gvTariffRef.DataKeys[e.RowIndex].Values[0].ToString();


            string t = proxy.Updatetarrifmaster(sTariffMaster);

            //31JAN12RM bug ID 1130
            DateTime value;
            //if ((!DateTime.TryParse(txtNewStartDate.Text, out value)) || (!DateTime.TryParse(txtNewEndDate.Text, out value)))
            string DateFormat = "dd/MM/yyyy";  //27FEB12RM  Bug Id 1236

            if ((!DateTime.TryParseExact(txtNewStartDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                 ||
             (!DateTime.TryParseExact(txtNewEndDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)))
            {
                lblDateValidation.Visible = true;
                string startDateCaption = gvTariffRef.HeaderRow.Cells[1].Text;
                string endDatecaption = gvTariffRef.HeaderRow.Cells[2].Text;
                lblDateValidation.Text = startDateCaption + "/" + endDatecaption + " " + GetGlobalResourceObject("LocalString", "ValidationInvalid").ToString();
                return;
            }



            //int res = (int)context.uSP_TARIFF_VAL(com, opt, txtNewStartDate, txtNewStartDate);
            //int res = 0;
            if (t != "0")
            {
                if (t == null)
                {
                    t = "";
                }
                lblDateValidation.Text = GetGlobalResourceObject("LocalString", "OverLap").ToString() +" " + "[" + t.ToString()+ "]";
                return;
            }

            else
            {
                /*2.Get the primary key for the edited row */
                TariffReference = gvTariffRef.DataKeys[e.RowIndex].Values[0].ToString();


                /*3.Create business object*/
                //KaizosServiceAgent proxy = new KaizosServiceAgent();

                /*4.Update record in database*/
                //DateTime.ParseExact(txtShipingDate.Text, format, CultureInfo.InvariantCulture);
                //proxy.UpdateTariffReference(TariffReference,Convert.ToDateTime(txtNewStartDate.Text),Convert.ToDateTime(txtNewEndDate.Text),bArchived);

                string format = "dd/MM/yyyy";  //27FEB12RM [1236]
                proxy.UpdateTariffReference(TariffReference, DateTime.ParseExact(txtNewStartDate.Text, format, CultureInfo.InvariantCulture), DateTime.ParseExact(txtNewEndDate.Text, format, CultureInfo.InvariantCulture), bArchived);


                /*5.cancel edit mode */
                gvTariffRef.EditIndex = -1;

                /*6.Load again updated DataBind */
                FillTariffReferenceGrid(ddlCarrier.SelectedValue, rblTariffType.SelectedValue);
            }
        }

        protected void rblTariffType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvTariffRef.DataSource = null;
            gvTariffRef.DataBind(); //1254 [24FEB12RM]
        }

    }
}