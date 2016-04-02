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
    public partial class frmTos : BasePage
    {
        public string PreviousTermsofService;

        ILog logger = log4net.LogManager.GetLogger(typeof(frmTos));
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    SToS stos = new SToS();
                    stos.ID = 0;
                    stos.TermsOfService = txtTos.Text.Trim();
                    stos.LastUpdate = DateTime.Now;
                    stos.Active = SEnumFlag.Yes;
                    stos.AccountNo = KaizosSession.Current.AccountNo;
                    
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    
                    if (proxy.SpInsertTos(stos) == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TosInsertSuccess").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TosInsertFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [24JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TOSFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [24JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Button1_Click", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TOSFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmTOS").ToString();
                    SToS stos = new SToS();
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    stos = proxy.GetActiveToS();
                    txtTos.Text = stos.TermsOfService;
                    PreviousTermsofService = stos.TermsOfService;
                }
                else
                {
                    SToS stos = new SToS();
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    stos = proxy.GetActiveToS();
                    PreviousTermsofService = stos.TermsOfService;
                }
                if (KaizosSession.Current.UserType == "AD")
                    btnUpdate.Enabled = true;
                else
                    btnUpdate.Enabled = false;
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

        protected void val_Login_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = true;
                string strError = "";

                // To validate if it is empty
                if (txtTos.Text.Length == 0)
                {
                    strError = strError + "*" + lblTos.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                // To validate if it is exceeds 100000 characters
                if (txtTos.Text.Length == 100001)
                {
                    strError = strError + "*" + lblTos.Text.Trim() + " " + valMaxAllowed100000.Text.Trim() + "<br>";
                    args.IsValid = false;

                }
                // To validate if any changes made or not
                int result = string.Compare(txtTos.Text, PreviousTermsofService);
                if (result == 0)
                {
                    strError = strError + "*" + lblTos.Text.Trim() + " " + valTosSame.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (!(args.IsValid))
                {
                    val_Tos.ErrorMessage = strError.Trim();
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [24JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TOSValFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [24JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_Login_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TOSValFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }
    }   
}