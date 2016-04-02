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
    public partial class frmTOSShow : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTos));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmTOS").ToString();
                    SToS sTos = new SToS();
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    sTos = proxy.GetActiveToS();
                    lblTos.Text = sTos.TermsOfService;
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
        }
    }
}