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
    public partial class frmFuelSurchargeManagement : BasePage
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(frmFuelSurchargeManagement));

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (val_FuelSurcharge.IsValid)
            {
                try
                {
                    
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    SMasterServiceType sMasterServiceType = new SMasterServiceType();

                    string tariffType = GetSelectedTariffType();

                    string keyAccountRef="";

                    if (tariffType.Equals("KeyCustomer"))
                        keyAccountRef = txtKeyActRef.Text.Trim();

                    gvFuel.EditIndex = -1; // 29feb12rm 1082

                    FillFuelGridView(tariffType, keyAccountRef);

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
        }

        protected void FillParameterGrid(int ServiceID)
        {
            try
            {
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                List<SFuelSurchargeParameter> sFuelSurchargeParameter = proxy.GetFuelChargeParameter(ServiceID).ToList();
               // gvParam.DataSource = sFuelSurchargeParameter;
               // gvParam.DataBind();

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

        protected void FillFuelGridView(string tariffType,string keyAccountRef)
        {
            try
            {
                gvFuel.DataSource = null;
                gvFuel.DataBind();

                //to fix 1191 [21FEB12RM]
                gvParameter.DataSource  = null;
                gvParameter.DataBind();
                gvParameter.Visible     = false;
                btnUpdate.Visible       = false;
                btnCancel.Visible       = false; 


                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<SFuelSurcharge> sFuelSurcharge = proxy.GetFuelCharge(tariffType, keyAccountRef).ToList();
                
                lblSearchError.Text = ""; //31JAN12RM

                if (sFuelSurcharge.Count != 0)
                {
                    lblSearchError.Text = "";
                    gvFuel.DataSource = sFuelSurcharge.ToArray();
                    gvFuel.DataBind();
                }
                else
                {
					errorMsg2.Attributes["style"] = "display: block;";
                    lblSearchError.Text = "No records found for the given criteria !";
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

        protected string GetSelectedTariffType()
        {
            string tariffType = "";

            if (rblTariffType.SelectedValue == "CarrierPublic")
                tariffType = "CarrierPublic";
            if (rblTariffType.SelectedValue == "KaizosPurchase")
                tariffType = "KaizosPurchase";
            else if (rblTariffType.SelectedValue == "KeyCustomer")
                tariffType = "KeyCustomer";

            return tariffType;
        }

        protected void gvFuel_RowEditing(object sender, GridViewEditEventArgs e)
        {

            /*  to fix 1090 [03FEB12RM] */
            gvParameter.Visible = false;
            btnUpdate.Visible   = false;
            btnCancel.Visible   = false;

            gvFuel.EditIndex = e.NewEditIndex;

            string tariffType = GetSelectedTariffType();

            string keyAccountRef = "";

            if (tariffType.Equals("KeyCustomer"))
                keyAccountRef = txtKeyActRef.Text.Trim();

            FillFuelGridView(tariffType, keyAccountRef);

        }

        protected void gvFuel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvFuel.EditIndex = -1;
            
            lblError.Visible = false;  //08FEB11RM

            string tariffType = GetSelectedTariffType();
            string keyAccountRef = "";

            if (tariffType.Equals("KeyCustomer"))
                keyAccountRef = txtKeyActRef.Text.Trim();

            FillFuelGridView(tariffType, keyAccountRef);
        }

        protected void gvFuel_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "parameter") // if "paramenter" link is clicked
            {
                gvParameter.Visible     = true;
                val_FuelParam.Visible   = true;
                btnUpdate.Visible       = true;
                btnCancel.Visible       = true;  //to fix 1090 [03FEB12RM]

                string serviceID = Convert.ToString(e.CommandArgument);
                
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<SFuelSurchargeParameter> sFuelSurchargeParameter = proxy.GetFuelChargeParameter(Convert.ToInt32(serviceID)).ToList();

                gvParameter.DataSource = sFuelSurchargeParameter;

                gvParameter.DataBind();

            }


        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

			if (val_FuelParam.IsValid) //03FEB12RM to fix bug 1084
			{
				List<SFuelSurchargeParameter> parameters = new List<SFuelSurchargeParameter>();

				string serviceID = "";
				string paramDesc = "";
				string paramValue = "";

				foreach (GridViewRow gr in gvParameter.Rows)
				{

					serviceID = gr.Cells[0].Text;
					paramDesc = gr.Cells[1].Text;
					paramValue = ((TextBox)gr.Cells[2].FindControl("txtParamValue")).Text;

					SFuelSurchargeParameter parameter = new SFuelSurchargeParameter();
					parameter.ServiceID = Convert.ToInt32(serviceID);
					parameter.ParameterDescription = paramDesc;
					parameter.ParameterValue = paramValue;
					parameters.Add(parameter);
				}

				KaizosServiceAgent proxy = new KaizosServiceAgent();
				proxy.UpdateFuelChargeParameter(parameters);
				gvParameter.DataSource = proxy.GetFuelChargeParameter(Convert.ToInt32(serviceID)).ToList();
				gvParameter.DataBind();

				gvParameter.Visible = false;
				btnUpdate.Visible = false;
				btnCancel.Visible = false; //to fix 1090 [03FEB12RM]
			}
			else { errorMsg3.Attributes["style"] = "display: block;"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
			errorMsg1.Attributes["style"] = "display: none;";
			errorMsg2.Attributes["style"] = "display: none;";
			errorMsg3.Attributes["style"] = "display: none;";
			errorMsg4.Attributes["style"] = "display: none;";

            if (!Page.IsPostBack) //31JAN12RM bugid : 1118
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmFuelSurchargeManagement").ToString();
                txtKeyActRef.Visible = false; //default its false and make it enable only if tariff type is key customer
                lblKeyActRef.Visible = false;
            }
            else
            {
                if (rblTariffType.SelectedIndex != 2)
                {
                    txtKeyActRef.Visible = false;
                    lblKeyActRef.Visible = false;
                }
            }
        }

        //08FEBRM Bug ID 1119
        protected bool isValidDate(string txtDate)
        {
            bool result = true;

            DateTime value;
            string DateFormat = "dd/MM/yyyy";

            if (!DateTime.TryParseExact(txtDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                result = false;
            }

            return result;
        }

        protected void gvFuel_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            /*1.Retrieve new date from Textbox in <EditTemplate> */
            TextBox txtNewStartDate = (TextBox)gvFuel.Rows[e.RowIndex].FindControl("txtStartDate");

            if (isValidDate(txtNewStartDate.Text.Trim())) //08FEBRM Bug ID 1119
            {
                lblError.Visible = false;


                /*2.Get the primary key for the edited row */
                int ServiceID = Convert.ToInt32(gvFuel.DataKeys[e.RowIndex].Values[0]);


                /*3.Create business object*/
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                /*4.Update record in database*/
                //proxy.UpdateFuelChargeStartDate(ServiceID,Convert.ToDateTime(txtNewStartDate.Text));  
                proxy.UpdateFuelChargeStartDate(ServiceID, DateTime.ParseExact(txtNewStartDate.Text, "dd/MM/yyyy", null)); //31JAN12RM bug id 1119

                /*5.cancel edit mode */
                gvFuel.EditIndex = -1;

                /*6.Load again updated DataBind */

                string tariffType = GetSelectedTariffType();
                string keyAccountRef = "";

                if (tariffType.Equals("KeyCustomer"))
                    keyAccountRef = txtKeyActRef.Text.Trim();

                FillFuelGridView(tariffType, keyAccountRef);

            }
            else//08FEBRM Bug ID 1119
            {
                lblError.Visible = true;
                lblError.Text = GetGlobalResourceObject("LocalString", "DateFormat").ToString() + " " +
                    GetGlobalResourceObject("LocalString", "ValidationInvalid").ToString();
				errorMsg4.Attributes["style"] = "display: block;";
            }

             
        }

        protected void rblTariffType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSearchError.Text = ""; //31JAN12RM

            gvFuel.DataSource = null; //1082 [24FEB12RM]
            gvFuel.DataBind(); //1082 [24FEB12RM]

            gvParameter.DataSource = null;  //1082 [29FEB12RM]
            gvParameter.DataBind();

            btnCancel.Visible = false; //05mar12 [1082]
            btnUpdate.Visible = false; //05mar12 [1082]

            if (rblTariffType.SelectedIndex == 2) //31JAN12RM bugid : 1118
            {
                txtKeyActRef.Visible = true;
                lblKeyActRef.Visible = true;
                txtKeyActRef.Text    = "";
                
                btnCancel.Visible = false; //29feb12 [1082]
                btnUpdate.Visible = false; //29feb12 [1082]
            }
        }

        protected void val_FuelSurcharge_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;  //31JAN12RM bug id 1118

            string strError = "";
            
            if (rblTariffType.SelectedIndex == -1)
            {
                strError = strError + "*" + lblTariffType.Text.Trim() + " " + valAccept.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            if (rblTariffType.SelectedIndex == 2)
            {
                if (txtKeyActRef.Visible == true && txtKeyActRef.Text.Length == 0)
                {
                    strError = strError + "*" + lblKeyActRef.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }

            if (!(args.IsValid))
            {
				errorMsg1.Attributes["style"] = "display: block;";
                val_FuelSurcharge.ErrorMessage = strError;
            }
        }

        protected string DateFormat(string date)
        {
            //return Convert.ToDateTime(date).ToShortDateString().ToString();
            return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
        }

        //To fix bug 03FEB12RM
        protected void val_FuelParam_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string strError = "";

            /* 1. Param must be entered and it should not emtpy */
            if (IsParamValueTextEmpty())
            {
                strError = strError + "*" + valParamEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            /* 2. Param must be Greater than 0 and Less than 100  19APRKM*/
            if (IsParamValueVal())
            {
                strError = strError + "*" + "INVALID INPUT" + "<br>";
                args.IsValid = false;

            }
            /* 3. Param must be entered and it should not emtpy */
            if (args.IsValid)
            {
                if (!IsNumericParam())
                {
                    strError = strError + "*" + valParamNumber.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

            }

            if (!(args.IsValid))
            {
				errorMsg3.Attributes["style"] = "display: block;";
				val_FuelParam.ErrorMessage = strError;
            }

        }

        /* To check the given value is numeric or not 03FEB12RM */
        protected bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        

        /* To check any empty param entered 03FEB12RM */
        protected bool IsParamValueTextEmpty()
        {
            bool result = false;

            for (int i = 0; i < gvParameter.Rows.Count; i++)
            {
                TextBox txtParam = (TextBox) gvParameter.Rows[i].FindControl("txtParamValue");
                if (txtParam.Text.Trim().Length == 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        /* To check Range of param entered 19APR12KM */
        protected bool IsParamValueVal()
        {
            bool result = false;
            for (int i = 0; i < gvParameter.Rows.Count; i++)
            {
                TextBox txtParam = (TextBox)gvParameter.Rows[i].FindControl("txtParamValue");
                float range = Convert.ToSingle(txtParam.Text);
                if (range <= 0 || range >= 100)
                {
                    result = true;
                    break;

                }
            }

            return result;

        }


        /* To check any empty param entered 03FEB12RM */
        protected bool IsNumericParam()
        {
            bool result = true;

            for (int i = 0; i < gvParameter.Rows.Count; i++)
            {
                TextBox txtParam = (TextBox)gvParameter.Rows[i].FindControl("txtParamValue");

                if (!isNumericValidation(txtParam.Text, System.Globalization.NumberStyles.Float))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            gvParameter.Visible = false;
            btnUpdate.Visible   = false;
            btnCancel.Visible   = false;
        }

        protected void gvParameter_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType != DataControlRowType.Pager) //08feb12rm
            {
                e.Row.Cells[0].CssClass = "Hidden";
            }
        }
    }
}