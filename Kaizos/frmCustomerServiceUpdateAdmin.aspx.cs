using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using KaizosServiceInvokers.KaizosServiceReference;
using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;


namespace Kaizos
{
    public partial class frmCustomerServiceUpdateAdmin : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmCustomerServiceUpdateAdmin));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmCustomerServiceUpdateAdmin").ToString();

                    if (KaizosSession.Current.UserType.Trim() == "AD")
                    {

                        txtEmail.Text = KaizosSession.Current.CSEmail; 

                        StatusLoad();
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    txtEmail.Enabled = false;
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void val_CustomerServiceUpdateByAdmin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";

            if (txtEmail.Text.Equals(""))
            {
                strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_CustomerServiceUpdateByAdmin.ErrorMessage = strError;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string status = "";
            if (IsValid)
            {
                try
                {
                    if (IsValid)
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        if (optBeingCreated.Checked)
                            status = "B";
                        else if (optDisabled.Checked)
                            status = "D";
                        else if (optEnabled.Checked)
                            status = "E";
                        else if (optArchi.Checked)
                            status = "A";

                        if (proxy.CustomerServiceUpdateAdmin(txtEmail.Text.Trim(), status.Trim()) == 0)
                        {
                            KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                            KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateSuccess").ToString(), txtEmail.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                        else
                        {
                            KaizosSession.Current.ReturnURL = "frmFranchiseList.aspx";
                            KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                            Server.Transfer("frmResult.aspx", false);
                        }
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSubmit_Click", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //StatusLoad();
            Server.Transfer("frmCustomerServiceList.aspx");
        }

        protected void StatusLoad()
        {
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            SFranchise sFranchise = new SFranchise();
            sFranchise = proxy.GetFranchise(txtEmail.Text.Trim());

            if (sFranchise.Status == SEnumUserStatus.BeingCreated)
                optBeingCreated.Checked = true;
            else if (sFranchise.Status == SEnumUserStatus.Disabled)
                optDisabled.Checked = true;
            else if (sFranchise.Status == SEnumUserStatus.Enabled)
                optEnabled.Checked = true;
            else
                optArchi.Checked = true;
        }
    }
}