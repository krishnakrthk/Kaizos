using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;



namespace Kaizos
{
    public partial class frmTariffOptionsAndSurcharge : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffOptionsAndSurcharge));

        int SearchRecordCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
			errorMsg_OptionParam.Attributes["style"] = "display: none;";
			errorMsg_surcharge.Attributes["style"] = "display: none;";

            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmTariffOptionsAndSurcharge").ToString();
                ViewState["MasterServiceList"] = GetMasterServiceNameList();
            }
        }

        protected List<SComboText> GetMasterServiceNameList()
        {
            List<SComboText> MasterServiceList=null;

            try
            {
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                SComboTableField table = new SComboTableField();
                table.TableName = "MASTER_SERVICE";
                table.FieldName = "MASTER_SERVICE_NAME";
                MasterServiceList = proxy.FillCombo(table);
            }
            catch (FaultException<SGeneralFault> sGeneralFault)
            {
                SGeneralFault fault = sGeneralFault.Detail;
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + fault.Issue + fault.Details;
                Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                logger.Debug(errorMessage);
            }
            catch (Exception error)
            {
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                //Response.Write(errorMessage);
                Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                logger.Debug(errorMessage);
            }


            return MasterServiceList;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
			if (val_surcharge.IsValid)
			{

			}
			else { errorMsg_surcharge.Attributes["style"] = "display: block;"; }
        }

        protected bool FillSurchargeMaster(string TariffReference)
        {
            bool result = false;

            try
            {

                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<SSurchargeMaster> sSurchargeMaster = proxy.GetSurchargeMaster(txtTariffRef.Text.Trim());

                if (sSurchargeMaster.Count != 0)
                {
                    gvSurchargeMaster.DataSource = sSurchargeMaster;
                    gvSurchargeMaster.DataBind();
                    result = true;
                }

            }
            catch (FaultException<SGeneralFault> sGeneralFault)
            {
                SGeneralFault fault = sGeneralFault.Detail;
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + fault.Issue + fault.Details;
                Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                logger.Debug(errorMessage);
            }
            catch (Exception error)
            {
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                //Response.Write(errorMessage);
                Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                logger.Debug(errorMessage);
            }


            return result;
        }

        protected bool IsActive(string flag)
        {
            return flag.ToUpper().Equals("Y") ? true : false;
        }

        protected void val_Shipment_ServerValidate(object source, ServerValidateEventArgs args)
        {

            //to fix similar scenari in option and surcharge 1191 [21FEB12RM]
            gvSurchargeDetail.DataSource = null;
            gvSurchargeDetail.DataBind();
            gvSurchargeDetail.Visible = false;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;

            args.IsValid = true;

            string strError = "";

            #region 1. Mandatory field check
            if (txtTariffRef.Text.Trim().Equals(""))
            {
                strError = strError + "*" + lblTariffRef.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            #endregion

            #region Fill Surcharge Master
            
            gvSurchargeMaster.DataSource = null;
            gvSurchargeMaster.DataBind();
            
            if (args.IsValid)
            {
                if (!FillSurchargeMaster(txtTariffRef.Text.Trim()))
                {
                    strError = strError + "*" + lblTariffRef.Text.Trim() + " [" + txtTariffRef.Text.Trim() + "] " + valNotFound.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

            }
            #endregion

            if (!(args.IsValid))
            {
                val_surcharge.ErrorMessage= strError;
            }
        
        }

        protected string SetSurchargeType(string SurChargeType)
        {
            string result ="";

            if (SurChargeType.ToUpper().Equals("O"))
                result = "Option";
            else
                result = "Surcharge";

            return result;
        }

        protected List<String> GetServiceNameDataMember(String DelimitedServiceName)
        {
            List<String> ServiceNameList=null;
            
            string[] ServiceNameArray = DelimitedServiceName.Split(',');

            ServiceNameList = ServiceNameArray.ToList();

            return ServiceNameList;
        }

        protected void gvSurchargeMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {

            /*  to enhance 1179 & 1131 [17FEB12RM] */
            gvSurchargeDetail.Visible = false;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;


            gvSurchargeMaster.EditIndex = e.NewEditIndex;

            FillSurchargeMaster(txtTariffRef.Text.Trim());
        }

        protected void gvSurchargeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "parameter") // if "paramenter" link is clicked
            {
                gvSurchargeDetail.Visible   = true;
                btnUpdate.Visible           = true;
                btnCancel.Visible           = true; //to fix 1179 & 1131 [17FEB12RM]

                string SurchargeCode        = Convert.ToString(e.CommandArgument);
                KaizosServiceAgent proxy    = new KaizosServiceAgent();
                List<SSurchargeDetails> sSurchargeDetails = proxy.GetSurchargeDetails(txtTariffRef.Text.Trim(), SurchargeCode).ToList();
                gvSurchargeDetail.DataSource = sSurchargeDetails;
                gvSurchargeDetail.DataBind();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (val_OptionParam.IsValid)
            {
                List<SSurchargeDetails> parameters = new List<SSurchargeDetails>();

                int ServiceID = 0;
                string TariffReference = "";
                string SurchargeCode = "";
                string ServiceName = "";
                string ParamID = "";
                decimal ParamValue = 0;

                foreach (GridViewRow gr in gvSurchargeDetail.Rows)
                {
                    string temp = gr.Cells[0].Text;
                    string temp2 = gr.Cells[2].Text;

                    ServiceID       = 0; // Convert.ToInt32(gr.Cells[0].Text);  //17FEB12RM 1179
                    TariffReference = gr.Cells[0].Text.Trim();
                    SurchargeCode   = gr.Cells[1].Text.Trim();
                    ServiceName     = gr.Cells[2].Text.Trim();
                    ParamID         = gr.Cells[3].Text.Trim();
                    ParamValue      = decimal.Parse((((TextBox)gr.Cells[1].FindControl("txtParamValue")).Text));

                    SSurchargeDetails parameter = new SSurchargeDetails();
                    parameter.ServiceID         = ServiceID;
                    parameter.TariffReference   = TariffReference;
                    parameter.SurchageCode      = SurchargeCode;
                    parameter.ParamID           = ParamID;
                    parameter.ParamValue        = ParamValue;
                    parameters.Add(parameter);
                }

                KaizosServiceAgent proxy = new KaizosServiceAgent();
                proxy.UpdateSurchargeDetails(parameters);
                parameters = proxy.GetSurchargeDetails(txtTariffRef.Text.Trim(), SurchargeCode).ToList();
                gvSurchargeDetail.DataSource = parameters;
                gvSurchargeDetail.DataBind();
            
                gvSurchargeDetail.Visible = false;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;  //to enhance 1179 & 1131 [17feb12RM]
			}
			else
			{
				errorMsg_OptionParam.Attributes["style"] = "display: block;";
			}
        }

        protected void gvSurchargeMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSurchargeMaster.EditIndex = -1;
            FillSurchargeMaster(txtTariffRef.Text.Trim());
        }

        protected void gvSurchargeMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            /*1.Get the primary key for the edited row */
            string SurchargeCode = gvSurchargeMaster.DataKeys[e.RowIndex].Values[0].ToString().Trim();

            
            /*2.Retrieve active */
            CheckBox cbNewActive = (CheckBox)gvSurchargeMaster.Rows[e.RowIndex].FindControl("cbActive");
            SEnumFlag sActive = cbNewActive.Checked ? SEnumFlag.Yes : SEnumFlag.No;


            /* 3.Retrieve Master Service Name from Check Box List */
            CheckBoxList cblNewMasterServiceList = (CheckBoxList)gvSurchargeMaster.Rows[e.RowIndex].FindControl("cblMasterServieName");
            string MasterServiceName = "";
            for (int i = 0; i < cblNewMasterServiceList.Items.Count; i++)
            {
                if (cblNewMasterServiceList.Items[i].Selected) MasterServiceName = MasterServiceName + cblNewMasterServiceList.Items[i].Value + ",";
            }
            if (MasterServiceName.Length != 0) MasterServiceName = MasterServiceName.Substring(0, (MasterServiceName.Length - 1));

            /*4.Create business object*/
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            /*5.Update record in database*/
            proxy.UpdateSurchargeMater(SurchargeCode, MasterServiceName, sActive);

            /*6.cancel edit mode */
            gvSurchargeMaster.EditIndex = -1;

            /*7.Load again updated DataBind */
            FillSurchargeMaster(txtTariffRef.Text.Trim());
        }

        protected void gvSurchargeMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<SComboText> MasterServiceList = null;

                if (ViewState["MasterServiceList"] != null)
                {
                    MasterServiceList = (List<SComboText>)ViewState["MasterServiceList"];
                }

                //if direct text then asp.net inserts databoundliteralcontrol
                //string percentage = ((DataBoundLiteralControl)e.Row.Cells[7].Controls[0]).Text;  
                
                Label lblServiceList = (Label)e.Row.FindControl("lblMasterServiceList");
                string[] MasterService = lblServiceList.Text.Split(',');
                
                lblServiceList.Text = "";
                
                CheckBoxList chkList = e.Row.FindControl("cblMasterServieName") as CheckBoxList;
                chkList.DataSource = MasterServiceList;
                chkList.DataBind();

                for (int i = 0; i < chkList.Items.Count; i++)
                {
                    for (int j = 0; j < MasterService.Length; j++)
                        if (MasterService[j] == chkList.Items[i].Value) chkList.Items[i].Selected = true;
                }

            }
        }

  
        //08FEB12HN To fix Bug ID: 1170
        protected bool isValidTwoDecimal(string Value)
        {
            string DecimalPattern = @"^\d{0,6}(\.\d{0,2})?$";

            Regex strPassword = new Regex(DecimalPattern);

            return (strPassword.IsMatch(Value.Trim()));

        }

        //17FEBRM to enhance 1179 & 1131
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            gvSurchargeDetail.Visible   = false;
            btnUpdate.Visible           = false;
            btnCancel.Visible           = false;
        }

        //event was not called previously[20FEB12RM]
        protected void val_OptionParam_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
            bool isEmpty = false;
            bool isInvalid = false;
            string strError = "";

            for (int i = 0; i < gvSurchargeDetail.Rows.Count; i++)
            {
                TextBox txtText = (TextBox)gvSurchargeDetail.Rows[i].FindControl("txtParamValue");
                if (txtText.Text.Trim().Equals(""))
                {
                    isEmpty = true;
                    args.IsValid = false;
                }
                else if (!(isValidTwoDecimal(txtText.Text)))
                {
                    isInvalid = true;
                    args.IsValid = false;
                }
            }

            if (isEmpty)
            {
                strError = GetGlobalResourceObject("LocalString", "ValParameterEmpty").ToString();
                val_OptionParam.ErrorMessage = strError;
            }

            if (isInvalid)
            {
                strError = GetGlobalResourceObject("LocalString", "ValParameterInvalid").ToString();
                val_OptionParam.ErrorMessage = strError;
            }
        }

        protected void gvSurchargeMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)  //27feb12RM
            {
                e.Row.Cells[0].CssClass = "hiddencol";
            }
        }



    }
   
}