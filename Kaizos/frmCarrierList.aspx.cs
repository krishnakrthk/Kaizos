using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;

namespace Kaizos
{
    public partial class frmCarrierList : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCarrierList));

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCarrierList").ToString();
                try
                {
                    if (KaizosSession.Current.UserType == "AD")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        List<SAuthorized> sAuthorized = new List<SAuthorized>();
                        SCustomer sCustomer = new SCustomer();
                        List<SCarrier> lstCarrier = new List<SCarrier>();
                        
                        SCarrier sCarrier = new SCarrier();
                        lstCarrier = proxy.GetCarriers().ToList();

                        

                        gvCarrier.DataSource = lstCarrier;
                        gvCarrier.DataBind();
                        
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
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AZUserListLoadFailure").ToString();
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
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AZUserListLoadFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }
        
        #endregion

        protected void gvCarriers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "GoEdit")
                {
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    string CarrierName = row.Cells[1].Text;
                    CheckBox chkRef =row.FindControl("chkReferencedCarrier") as CheckBox;
                    string strCarrierID =   gvCarrier.DataKeys[row.RowIndex].Value.ToString();
                    String ReferenceCarrier = chkRef.Checked.ToString();
                    CheckBox chkAct = row.FindControl("chkActive") as CheckBox;
                    String Active = chkAct.Checked.ToString();
                    String ClaimDelay = row.Cells[3].Text;

                    KaizosSession.Current.CMActive = Active;
                    KaizosSession.Current.CMCarrieID = strCarrierID;
                    KaizosSession.Current.CMCarrierName = CarrierName;
                    KaizosSession.Current.CMClaimDelay = ClaimDelay;
                    KaizosSession.Current.CMReferencedCarrier = ReferenceCarrier;

                    Server.Transfer("frmCarrier.aspx");//?x=" + HttpUtility.UrlEncode(CarrierName) + "&y=" + ReferenceCarrier + "&z=" + ClaimDelay + "&w=" + Active + "&u=" + strCarrierID);
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
    }
}