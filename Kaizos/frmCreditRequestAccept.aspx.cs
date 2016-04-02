using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmCreditRequestAccept : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCreditRequestAccept));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmCreditRequestAccept").ToString();
                FillGridView();
            }
        }

        protected void grdAuthorizedList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
       }

        protected void grdAuthorizedList_RowEditing(object sender,  GridViewEditEventArgs e)
        {
            grdAuthorizedList.EditIndex = e.NewEditIndex;
            FillGridView();

            // To enable the particular Check box
            ((CheckBox)grdAuthorizedList.Rows[e.NewEditIndex].Cells[12].FindControl("chkCS")).Enabled = true;
        }

        protected void grdAuthorizedList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int result = 1;

                KaizosServiceAgent proxy = new KaizosServiceAgent();

                if (((CheckBox)grdAuthorizedList.Rows[e.RowIndex].Cells[12].FindControl("chkCS")).Checked)
                {
                    result = proxy.UpdateCustomerCredit(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[0].FindControl("txtAccountNo")).Text.Trim(),
                                Convert.ToDecimal(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[2].FindControl("txtWishedBudgetAmount")).Text.Trim()),
                                Convert.ToDecimal(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[4].FindControl("txtInsuredCreditAmount")).Text.Trim()),
                                Convert.ToInt32(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[5].FindControl("txtPaymentDelayDays")).Text.Trim()),
                                Convert.ToDecimal(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[6].FindControl("txtGrossMargin")).Text.Trim()),
                                Convert.ToInt32(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[7].FindControl("txtPaymentDelayMonth")).Text.Trim()),
                                Convert.ToDecimal(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[8].FindControl("txtCompensationRate")).Text.Trim()),
                                Convert.ToDecimal(((TextBox)grdAuthorizedList.Rows[e.RowIndex].Cells[11].FindControl("txtAuthorizedCreditAmount")).Text.Trim()),
                                "Y");
                }
                /*cancel edit mode */
                grdAuthorizedList.EditIndex = -1;
                FillGridView();
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [27JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CreditRequestUpdateFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "grdAuthorizedList_RowUpdating", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CreditRequestUpdateFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void FillGridView()
        {
            try
            {
                if (KaizosSession.Current.UserType.Trim() == "AD" || KaizosSession.Current.UserType.Trim() == "FR")
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    List<SCustomer> sCustomer = new List<SCustomer>();
                    sCustomer = proxy.GetCustomerCredit(KaizosSession.Current.AccountNo, KaizosSession.Current.UserType).ToList();
                    grdAuthorizedList.DataSource = sCustomer.ToArray();
                    grdAuthorizedList.DataBind();
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
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CreditRequestLoadFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "FillGridView", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "CreditRequestLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void grdAuthorizedList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdAuthorizedList.EditIndex = -1;
            FillGridView();
        }
    }
}