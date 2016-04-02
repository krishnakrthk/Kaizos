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
    public partial class frmTosAccept : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTos));

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                SToS stos = new SToS();
                stos.TermsOfService = "";
                stos.LastUpdate = DateTime.Now;
                stos.Active = SEnumFlag.Yes;
                stos.AccountNo = KaizosSession.Current.AccountNo;

                KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                if (proxy.SpInsertTos(stos) == 0)
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TosApprovedSuccess").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmTosAccept.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TosApprovedFailure").ToString();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Page.Title = GetGlobalResourceObject("LocalString", "frmTOSAccept").ToString();
        }
    }
}