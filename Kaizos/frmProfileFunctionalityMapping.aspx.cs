using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;


using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{

    public class ProfileTemptable
    {
        public string FunctionalName { get; set; }
        public Boolean CheckN1 { get; set; }
        public Boolean CheckN2 { get; set; }
        public Boolean CheckCustomerService { get; set; }
        public Boolean CheckReferent { get; set; }
        public Boolean CheckAuthorize { get; set; }
    }

    public partial class frmProfileFunctionalityMapping : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmProfileFunctionalityMapping));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(IsPostBack))
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmProfile").ToString();
                    KaizosServiceAgent proxy = new KaizosServiceAgent();
                    List<SFunctionality> sFunctionality = new List<SFunctionality>();
                    List<ProfileTemptable> lProfileTemptables = new List<ProfileTemptable>();

                    sFunctionality = proxy.GetFunctionality().ToList();
                    for (int i = 0; i < sFunctionality.Count; i++)
                    {
                        ProfileTemptable lProfileTemptable = new ProfileTemptable();

                        lProfileTemptable.FunctionalName = sFunctionality[i].FunctionalName;
                        lProfileTemptable.CheckN1 = ProfileFunctionalCheck("AD", sFunctionality[i].FunctionalCode);
                        lProfileTemptable.CheckN2 = ProfileFunctionalCheck("FR", sFunctionality[i].FunctionalCode);
                        lProfileTemptable.CheckCustomerService = ProfileFunctionalCheck("CS", sFunctionality[i].FunctionalCode);
                        lProfileTemptable.CheckReferent = ProfileFunctionalCheck("RF", sFunctionality[i].FunctionalCode);
                        lProfileTemptable.CheckAuthorize = ProfileFunctionalCheck("AZ", sFunctionality[i].FunctionalCode);
                        lProfileTemptables.Add(lProfileTemptable);
                    }

                    if (lProfileTemptables != null)
                    {
                        gv_ProfileMapping.DataSource = lProfileTemptables.ToList();
                        gv_ProfileMapping.DataBind();
                    }
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
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ProfileLoadFailure").ToString();
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
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ProfileLoadFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
        }

        protected bool ProfileFunctionalCheck(string ProfileCode, int FunctionalCode)
        {
            bool result = false;
            try
            {
                KaizosServiceAgent proxy1 = new KaizosServiceAgent();
                List<SFunctionalProfile> sFunctionalityProfile = new List<SFunctionalProfile>();
                sFunctionalityProfile = proxy1.GetFunctionalProfile().ToList();

                for (int j = 0; j < sFunctionalityProfile.Count; j++)
                {
                    if (sFunctionalityProfile[j].ProfileCode == ProfileCode.Trim() && sFunctionalityProfile[j].FunctionalCode == FunctionalCode)
                    {
                        result = true;
                        return result;
                    }
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
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ProfileLoadFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "ProfileFunctionalCheck", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "ProfileLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
            return result;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Need to update the altered details
                List<ProfileTemptable> lProfileTemptables = new List<ProfileTemptable>();

                foreach (GridViewRow gr in gv_ProfileMapping.Rows)
                {
                    ProfileTemptable lProfileTemptable = new ProfileTemptable();
                    lProfileTemptable.FunctionalName = HttpUtility.HtmlDecode(gr.Cells[0].Text);

                    lProfileTemptable.CheckN1 = ((CheckBox)gr.Cells[1].FindControl("chkN1")).Checked ? true : false;
                    lProfileTemptable.CheckN2 = ((CheckBox)gr.Cells[2].FindControl("chkN2")).Checked ? true : false;
                    lProfileTemptable.CheckCustomerService = ((CheckBox)gr.Cells[3].FindControl("chkCS")).Checked ? true : false;
                    lProfileTemptable.CheckReferent = ((CheckBox)gr.Cells[4].FindControl("chkRF")).Checked ? true : false;
                    lProfileTemptable.CheckAuthorize = ((CheckBox)gr.Cells[5].FindControl("chkAZ")).Checked ? true : false;
                    lProfileTemptables.Add(lProfileTemptable);
                }

                KaizosServiceAgent proxy = new KaizosServiceAgent();
                List<SFunctionality> sFunctionalities = new List<SFunctionality>();
                List<SFunctionalProfile> lsFunctionalProfile = new List<SFunctionalProfile>();

                sFunctionalities = proxy.GetFunctionality().ToList();

                for (int i = 0; i < lProfileTemptables.Count; i++)
                {
                    for (int j = 0; j < sFunctionalities.Count; j++)
                    {
                        if (lProfileTemptables[i].FunctionalName.Trim() == sFunctionalities[j].FunctionalName.Trim())
                        {
                            if (lProfileTemptables[i].CheckN1)
                            {
                                SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                                sFunctionalProfile.FunctionalCode = sFunctionalities[j].FunctionalCode;
                                sFunctionalProfile.ProfileCode = "AD";
                                lsFunctionalProfile.Add(sFunctionalProfile);
                            }
                            if (lProfileTemptables[i].CheckN2)
                            {
                                SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                                sFunctionalProfile.FunctionalCode = sFunctionalities[j].FunctionalCode;
                                sFunctionalProfile.ProfileCode = "FR";
                                lsFunctionalProfile.Add(sFunctionalProfile);
                            }
                            if (lProfileTemptables[i].CheckCustomerService)
                            {
                                SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                                sFunctionalProfile.FunctionalCode = sFunctionalities[j].FunctionalCode;
                                sFunctionalProfile.ProfileCode = "CS";
                                lsFunctionalProfile.Add(sFunctionalProfile);
                            }
                            if (lProfileTemptables[i].CheckReferent)
                            {
                                SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                                sFunctionalProfile.FunctionalCode = sFunctionalities[j].FunctionalCode;
                                sFunctionalProfile.ProfileCode = "RF";
                                lsFunctionalProfile.Add(sFunctionalProfile);
                            }
                            if (lProfileTemptables[i].CheckAuthorize)
                            {
                                SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                                sFunctionalProfile.FunctionalCode = sFunctionalities[j].FunctionalCode;
                                sFunctionalProfile.ProfileCode = "AZ";
                                lsFunctionalProfile.Add(sFunctionalProfile);
                            }
                        }
                    }
                }

                if (proxy.DeleteFunctionalProfile() == 0)                {

                    if (proxy.InsertFunctionalProfile(lsFunctionalProfile) == 0)
                    {
                        KaizosSession.Current.ReturnURL = "frmProfileFunctionalityMapping.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageProfileSuccess").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmProfileFunctionalityMapping.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageProfileFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmProfileFunctionalityMapping.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageProfileDeleteFailure").ToString();
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
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageProfileFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnUpdate_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageProfileFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }
    }
}