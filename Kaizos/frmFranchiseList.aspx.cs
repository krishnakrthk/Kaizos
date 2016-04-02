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
    public partial class frmFranchiseList : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmFranchiseList));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmFranchiseList").ToString();
                try
                {
                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                        List<SFranchiseContact> sFranchise;
                        sFranchise = proxy.GetFranchiseList().ToList();
                        gvFranchiseList.DataSource = sFranchise.ToArray();
                        gvFranchiseList.DataBind();
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageFranchiseListFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageFranchiseListFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void gvFranchiseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "GoEdit")
                {
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    KaizosSession.Current.CompanyName = row.Cells[1].Text;
                    KaizosSession.Current.Email = row.Cells[2].Text;
                    KaizosSession.Current.ContactName = row.Cells[3].Text;
                    KaizosSession.Current.Language = row.Cells[4].Text;
                    Server.Transfer("frmFranchiseUpdateByAdmin.aspx");
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageFranchiseListFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "gvFranchiseList_RowCommand", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageFranchiseListFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void gvFranchiseList_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void gvFranchiseList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
    }
}