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

    public partial class frmDeliveryDelayUpdate : BasePage
    {
        int ServiceID;

        ILog logger = log4net.LogManager.GetLogger(typeof(frmDeliveryDelayUpdate));


        protected void Page_Load(object sender, EventArgs e)
        {

            //ServiceID = Convert.ToInt32(Request.QueryString["ServiceID"]); //To fix 1258 [06MAR12RM]
            ServiceID = Convert.ToInt32(KaizosSession.Current.ServiceID);

            
            Page.Title = Page.Title + " [" + KaizosSession.Current.CarrierName + "-" + KaizosSession.Current.ServiceName + "]";

            if (!IsPostBack)
            {
                //To fix 1258 [06MAR12RM]
                lblCaption.Text = lblCaption.Text + " [" + KaizosSession.Current.CarrierName + "-" + KaizosSession.Current.ServiceName + "]";

                LoadDeliveryDelayGrid(ServiceID);
                FillCombo();
            }
        }

        protected void FillCombo()
        {
            /* 1.create proxy */
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            List<SComboText> sComboText = new List<SComboText>();

            SComboTableField sComboTableField = new SComboTableField();

            //To fill Origin drop down list
            sComboTableField.FieldName = "COUNTRY_CODE";
            sComboTableField.TableName = "COUNTRY";
            sComboText = proxy.FillCombo(sComboTableField).ToList();

            ddlOrigin.DataSource = sComboText;
            ddlOrigin.DataTextField = "ComboText";
            ddlOrigin.DataBind();


            //To fill Destination drop down list
            sComboTableField.FieldName = "COUNTRY_CODE";
            sComboTableField.TableName = "COUNTRY";
            sComboText = proxy.FillCombo(sComboTableField).ToList();

            ddlDestination.DataSource = sComboText;
            ddlDestination.DataTextField = "ComboText";
            ddlDestination.DataBind();


        }

        protected void LoadDeliveryDelayGrid(int serviceID)
        {
            /* 1.create proxy */
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            /* 2.call business method using proxy and update the record */
            List<SDeliveryDelay> sDeliveryDelay = proxy.GetCarrierServiceDeliveryDelay(serviceID);

            /*. Load the grid */
            gvDeliveryDelay.DataSource = sDeliveryDelay;
            gvDeliveryDelay.DataBind();
        }

        protected void gvDeliveryDelay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("MyEdit"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string origin = row.Cells[1].Text.Trim(); //Origin
                int index = ddlOrigin.Items.IndexOf(ddlOrigin.Items.FindByValue(origin));
                ddlOrigin.SelectedIndex = index;

                string destination = row.Cells[2].Text.Trim(); //Destination
                index = ddlOrigin.Items.IndexOf(ddlOrigin.Items.FindByValue(destination));
                ddlDestination.SelectedIndex = index;

                txtDelay.Text = row.Cells[3].Text.Trim(); //Destination

                btnUpdate.Focus();
            }
        }

        protected void gvDeliveryDelay_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDeliveryDelay.EditIndex = -1;
            gvDeliveryDelay.DataBind();
        }

        protected int GetDeliveryDelay(string Origin, string Destination)
        {
            int result = -1;

            for (int i = 0; i < gvDeliveryDelay.Rows.Count; i++)
            {

                string org = gvDeliveryDelay.Rows[i].Cells[1].Text.Trim();
                string dest = gvDeliveryDelay.Rows[i].Cells[2].Text.Trim();

                if (Origin.Trim().Equals(org) && Destination.Trim().Equals(dest))
                {
                    result = Convert.ToInt32(gvDeliveryDelay.Rows[i].Cells[3].Text);
                    break;
                }
            }
            return result;
        }

        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDelay.Text = "";
            //ddlOrigin.SelectedIndex = -1;
            int index = GetDeliveryDelay(ddlOrigin.SelectedValue.Trim(), ddlDestination.SelectedValue.Trim());
            if (index != -1) txtDelay.Text = index.ToString();
        }

        protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDelay.Text = "";
            //ddlDestination.SelectedIndex = -1;
            int index = GetDeliveryDelay(ddlOrigin.SelectedValue, ddlDestination.SelectedValue);

            if (index != -1) txtDelay.Text = index.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                /* 1.create proxy */
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                /* 2.call business method using proxy and update the record */
                proxy.UpdateCarrierServiceDelay(ServiceID, ddlOrigin.SelectedValue, ddlDestination.SelectedValue, Convert.ToInt32(txtDelay.Text));

                /* 3. Refresh the grid with new value */
                LoadDeliveryDelayGrid(ServiceID);
            }

        }

        protected bool IsNumber(string number)
        {
            bool result = false;
            string pattern = @"^\d{1,3}$";
            Regex ex = new Regex(pattern);
            if (ex.IsMatch(number)) result = true;
            return result;
        }

        protected void val_DeliveryDelayUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            string strError = "";

            if (!IsNumber(txtDelay.Text.Trim()))
            {
                strError = strError + "*" + lblDeliveryDelay.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_DeliveryDelayUpdate.ErrorMessage = strError;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("frmMasterServiceTypeUpdate.aspx", false);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnBack_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "DeliveryDelayBack").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }
    }
}