using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmAddressBookList : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAddressBookList));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAddressBookList").ToString();
            }
            try
            {
                if (KaizosSession.Current.UserType.Trim() != "CS")
                {
                    if (!IsPostBack)
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        List<SAddressBook> sAddressList;
                        sAddressList = proxy.GetAddress().ToList();

                        gvAddressBookList.DataSource = sAddressList.ToArray();
                        gvAddressBookList.DataBind();
                    }
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [27JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookLoadFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }
    }
}