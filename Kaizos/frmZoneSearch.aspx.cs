using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;



namespace Kaizos
{
    public partial class frmZoneSearch : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmZoneSearch));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Page.Title = GetGlobalResourceObject("LocalString", "frmZoneSearch").ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //to fix bug 1069 [03FEB12RM]
                lblSearchResult.Visible = false;

                gvSearch.DataSource = null;
                gvSearch.DataBind();

                if (txtTariffReference.Text.Trim().Length == 0) 
                {
                    lblSearchResult.Visible = true;
                    lblSearchResult.Text = string.Format(GetGlobalResourceObject("LocalString", "SearchWithNoData").ToString(), lblTariffReferencE.Text);
                    return;
                }
                //
                
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                List<SZoneSearchDetails> sZoneSearchDetails = proxy.GetZoneDetails(txtTariffReference.Text.Trim());
                gvSearch.DataSource = sZoneSearchDetails;
                gvSearch.DataBind();

                if (sZoneSearchDetails.Count == 0) //to fix bug 1069 [03FEB12RM]
                {
                    lblSearchResult.Visible = true;
                    lblSearchResult.Text = string.Format(GetGlobalResourceObject("LocalString", "SearchResult").ToString(), lblTariffReferencE.Text);
                }

            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    /* Detail exception into log file */
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);

                    /* User friendly message */
                    KaizosSession.Current.ReturnURL = "frmZoneSearch.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ZoneSearchFailure").ToString(), txtTariffReference.Text);
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void gvSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                string ZoneID = e.CommandArgument.ToString();
                KaizosSession.Current.ModeFlag = "Update";
                KaizosSession.Current.ZoneID = ZoneID;
                Server.Transfer("frmZoneCreationUpdate.aspx");
                //Server.Transfer("frmZoneCreationUpdate.aspx?ModeFlag=Update&ZoneID=" + ZoneID);
            }
        }

        //05MAR12RM [1255]
        protected string FormatDateString(string date)
        {
            //string format = "dd/MM/yyyy";
            //return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture).ToString();
            return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
        }
    }
}