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
    public partial class frmUserUpdateByAdmin : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmLogin));
        public string struserId = "", strAccount_no = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmFranchiseUpdate").ToString();
                if (KaizosSession.Current.UserType.Trim() == "AD")
                {
                    txtEmail.Text = KaizosSession.Current.Email.Trim();
                    txtCompanyName.Text = KaizosSession.Current.CompanyName.Trim();
                    txtLanguage.Text = KaizosSession.Current.Language.Trim();

                    struserId = KaizosSession.Current.UserId;
                    strAccount_no = KaizosSession.Current.AccountNo;

                    txtEurope.Enabled = false;
                    txtFrance.Enabled = false;
                    txtInternational.Enabled = false;
                    fileUpload.Enabled = false;
                    optArcheive.Enabled = false;
                    optDisable.Enabled = false;
                    optEnable.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSubmit.Enabled = false;
                    btnGet_Click(sender, e);
                }
                else
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                SFranchise sFranchise = new SFranchise();
                List<SMonthlyFee> sMonthlyFee = new List<SMonthlyFee>();

                txtEurope.Enabled = true;
                txtFrance.Enabled = true;
                txtInternational.Enabled = true;
                fileUpload.Enabled = true;
                optArcheive.Enabled = true;
                optDisable.Enabled = true;
                optEnable.Enabled = true;
                btnCancel.Enabled = true;
                btnSubmit.Enabled = true;


                sFranchise = proxy.GetFranchise(txtEmail.Text.Trim());
                sMonthlyFee = proxy.GetMonthlyFees(txtEmail.Text.Trim()).ToList();
                txtCompanyName.Text = sFranchise.CompanyName;

                rtxtChalandiseZone.Text = sFranchise.AssignedZone;
               lblCreditRequest1.Text = sFranchise.ScannedDoc;

                if (sFranchise.Status == SEnumUserStatus.Enabled)
                   optEnable.Checked = true;
               if (sFranchise.Status == SEnumUserStatus.Disabled)
                   optDisable.Checked = true;
               if (sFranchise.Status == SEnumUserStatus.Archived)
                   optArcheive.Checked = true;
                
                foreach (var sMonthlyFeeList in sMonthlyFee)
                {
                    if (sMonthlyFeeList.ShipmentType == "France")
                        txtFrance.Text = sMonthlyFeeList.FeeRate.ToString();
                    if (sMonthlyFeeList.ShipmentType == "International")
                        txtInternational.Text = sMonthlyFeeList.FeeRate.ToString();
                    if (sMonthlyFeeList.ShipmentType == "Europe")
                        txtEurope.Text = sMonthlyFeeList.FeeRate.ToString();
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
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "FranchiseLoadFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnGet_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "FranchiseLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnGet_Click(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int InsertStatus = 1;
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    SFranchise sFranchise = new SFranchise();
                    SMonthlyFee sMonthlyFee = new SMonthlyFee();

                    sFranchise = proxy.GetFranchise(txtEmail.Text.Trim());

                    sMonthlyFee.AccountNo = sFranchise.AccountNo;
                    sMonthlyFee.AdminAccountNo = strAccount_no.Trim();
                    sMonthlyFee.LastUpdate = DateTime.Now;

                    if (txtFrance.Text.Trim() != "")
                    {
                        sMonthlyFee.FeeRate = decimal.Parse(txtFrance.Text);
                        sMonthlyFee.ShipmentType = "France";
                    }
                    else
                    {
                        sMonthlyFee.FeeRate = 0;
                        sMonthlyFee.ShipmentType = "France";
                    }

                    InsertStatus = proxy.InsertMonthlyFee(sMonthlyFee);
                    if (txtInternational.Text.Trim() != "")
                    {
                        sMonthlyFee.FeeRate = decimal.Parse(txtInternational.Text.Trim());
                        sMonthlyFee.ShipmentType = "International";
                    }
                    else
                    {
                        sMonthlyFee.FeeRate = 0;
                        sMonthlyFee.ShipmentType = "International";
                    }
                    InsertStatus = proxy.InsertMonthlyFee(sMonthlyFee);

                    if (txtEurope.Text.Trim() != "")
                    {
                        sMonthlyFee.FeeRate = decimal.Parse(txtEurope.Text.Trim()); ;
                        sMonthlyFee.ShipmentType = "Europe";
                    }
                    else
                    {
                        sMonthlyFee.FeeRate = 0;
                        sMonthlyFee.ShipmentType = "Europe";
                    }
                    InsertStatus = proxy.InsertMonthlyFee(sMonthlyFee);

                    if (InsertStatus == 0)
                    {

                        #region Franchise update Part
                        sFranchise.AccountNo = strAccount_no;
                        sFranchise.AssignedZone = rtxtChalandiseZone.Text.Trim();
                        sFranchise.CompanyName = txtCompanyName.Text.Trim();
                        if (fileUpload.PostedFile != null && fileUpload.PostedFile.FileName != "")
                        {
                            sFranchise.ScannedDoc = Server.MapPath(fileUpload.PostedFile.FileName); ;
                            lblCreditRequest1.Text = Server.MapPath(fileUpload.PostedFile.FileName); ;
                        }

                        if (optArcheive.Checked)
                            sFranchise.Status = SEnumUserStatus.Archived;
                        if (optDisable.Checked)
                            sFranchise.Status = SEnumUserStatus.Disabled;
                        if (optEnable.Checked)
                            sFranchise.Status = SEnumUserStatus.Enabled;

                        if (proxy.UpdateFranchiseAdmin(sFranchise) == 0)
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
                        #endregion
                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmFranchiseList.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageMonthlyFeeFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
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

        protected void val_FranchiseUpdateByAdmin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            SUser sUSER = new SUser();
            sUSER = proxy.GetLogin(KaizosSession.Current.Email.Trim());
            try
            {
                args.IsValid = true;
                string strError = "";
                decimal d = 0;

                if (txtCompanyName.Text.Equals(""))
                {
                    strError = strError + "*" + lblCompanyName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtEmail.Text.Equals(""))
                {
                    strError = strError + "*" + lblEmail.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
                if (txtLanguage.Text.Equals(""))
                {
                    strError = strError + "*" + lblLanguage.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (txtFrance.Text.Equals("") && txtEurope.Text.Equals("") && txtInternational.Text.Equals(""))
                {
                    strError = strError + "*" + lblMonthlyFees.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (!(txtFrance.Text.Equals("")))
                {
                    if ((!Decimal.TryParse(txtFrance.Text, out d)))
                    {
                        strError = strError + "*" + lblFrance.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else
                    {
                        if (Convert.ToDouble(txtFrance.Text) > 100 && (!txtFrance.Text.Equals("")))
                        {
                            strError = strError + "*" + lblFrance.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }
                }

                if (!(txtEurope.Text.Equals("")))
                {
                    if ((!Decimal.TryParse(txtEurope.Text, out d)))
                    {
                        strError = strError + "*" + lblEurope.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else if (Convert.ToDouble(txtEurope.Text) > 100)
                    {
                        strError = strError + "*" + lblEurope.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                }
                if ((!txtInternational.Text.Equals("")))
                {
                    if ((!Decimal.TryParse(txtInternational.Text, out d)))
                    {
                        strError = strError + "*" + lblInternational.Text.Trim() + " " + valNumber.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                    else if (Convert.ToDouble(txtInternational.Text) > 100 && (!txtInternational.Text.Equals("")))
                    {
                        strError = strError + "*" + lblInternational.Text.Trim() + " " + valLess100.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                //if (lblCreditRequest1.Text.Trim().Length != 0) //27FEB12SM 
                if (fileUpload.FileName.Trim().Length != 0)
                {
                    if (System.IO.Path.GetExtension(fileUpload.FileName) != ".pdf")
                    {
                        strError = strError + "*" + lblCreditRequest.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }
                }

                if (rtxtChalandiseZone.Text.Trim().Length != 0)
                {
                    var primeArray = rtxtChalandiseZone.Text.Split(',');
                    for (int i = 0; i < primeArray.Length; i++)
                    {
                        var first = primeArray[i].Trim();
                        if (proxy.ValidateHQZipcode(first.Trim() + ',' + sUSER.AccountNo.Trim()) != "2")
                        {
                            strError = strError + "*" + lblChalandiseZone.Text.Trim() + "-" + first.Trim() + " " + valAlready.Text.Trim() + "<br>";
                            args.IsValid = false;
                        }
                    }
                }

                if (!(args.IsValid))
                {
                    val_FranchiseUpdateByAdmin.ErrorMessage = strError;
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
                KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_FranchiseUpdateByAdmin_ServerValidate", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "MessageUserUpdateFailure").ToString(), txtEmail.Text.Trim());
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }
    }
}