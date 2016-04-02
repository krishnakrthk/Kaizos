using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;


namespace Kaizos
{
    public partial class frmCustomerServiceList : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCustomerServiceList));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCustomerServiceList").ToString();
                try
                {
                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        List<SFranchiseContact> sFranchise;
                        sFranchise = proxy.GetCustomerServiceList().ToList();
                        gvCSList.DataSource = sFranchise.ToArray();
                        gvCSList.DataBind();
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
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
        }

        protected void gvCSList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "GoEdit")
                {
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    String CompanyName = row.Cells[1].Text;
                    String Email = row.Cells[2].Text;
                    String ContactName = row.Cells[3].Text;
                    String Language = row.Cells[4].Text;

                    KaizosSession.Current.CSCompanyName = CompanyName;
                    KaizosSession.Current.CSEmail = Email;
                    KaizosSession.Current.CSLanguage = Language;
                    KaizosSession.Current.CSContactName = ContactName;

                    Response.Redirect("frmCustomerServiceUpdateAdmin.aspx");//?&x=" + Email);
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

        protected void gvCSList_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void gvCSList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
    }
}