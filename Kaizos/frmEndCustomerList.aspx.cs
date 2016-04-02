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
    public partial class frmEndCustomerList : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmEndCustomerList));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmEndCustomerList").ToString();
                try
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    List<SFranchiseContact> sFranchise;
                    sFranchise = proxy.GetEndCustomerList().ToList();
                    gvFranchiseList.DataSource = sFranchise.ToArray();
                    gvFranchiseList.DataBind();
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file*/
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
                    /* Generalized exception handling and logging detailed exception into log4net file*/
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

                    KaizosSession.Current.CompanyName = row.Cells[2].Text;
                    KaizosSession.Current.Email = row.Cells[3].Text;
                    KaizosSession.Current.ContactName = row.Cells[4].Text;
                    KaizosSession.Current.Language = row.Cells[5].Text;
                    Server.Transfer("frmEndCustomerUpdate.aspx");
                }
                if (e.CommandName == "GoSimulate")
                {
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    KaizosSession.Current.Email = row.Cells[3].Text;
                    Server.Transfer("frmGrossMarginCalculation.aspx");
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