using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization; //07FEB12RM
using System.Text.RegularExpressions; //08FEB12RM

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmTariffCreation : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmTariffCreation));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmTariffCreation").ToString();
                /* Load carrier name in the drop down list for the first time */
                FillCombo();

                /* Disable error message by default */
                lblContainerTypeErrMsg.Visible  = false;
                lblKeyUserErrMsg.Visible        = false;
                lblStartDateErrMsg.Visible      = false;
                lblDateValidation.Visible       = false;

                trKeyUserRef1.Visible= false;

				errorMsg.Attributes["style"] = "display: none;";
            }
        }

        protected STariffMaster GetBusinessEntity()
        {
            STariffMaster sTariffMaster = new STariffMaster();
         
            //1.Carrier Name
            sTariffMaster.CarrierName       = ddlCarrier.SelectedValue;

            //2.Tariff Reference
            sTariffMaster.TariffReference = txtTariffReference.Text.Trim(); 

            //3.Tariff Type
            if (rblTariffType.SelectedValue.Trim() == "Purchase")
                sTariffMaster.TariffType = SEnumTariffType.Purchase;
            else if (rblTariffType.SelectedValue.Trim() == "CarrierPublic")
                sTariffMaster.TariffType = SEnumTariffType.CarrierPublic;
            else if (rblTariffType.SelectedValue.Trim() == "KeyCustomer")
                sTariffMaster.TariffType = SEnumTariffType.Negotiated;

            //4.ContainerType
            string strContainerType="";

            for (int i = 0; i < cblContainerType.Items.Count; i++)
            {
                ListItem li = cblContainerType.Items[i];
                if (li.Selected) strContainerType = strContainerType + li.Text + ",";
            }
            sTariffMaster.ContainerType = strContainerType.Substring(0, (strContainerType.Length - 1));

            //5.KeyUserReference
            sTariffMaster.KeyUserReference = "";
            if (rblTariffType.SelectedValue=="KeyCustomer") 
                sTariffMaster.KeyUserReference = txtKeyUserReference.Text.Trim();

            //6.Start Date
            string format = "dd/MM/yyyy";   // to fix bug 1169 [07FEB12RM]
            //sTariffMaster.StartDate = Convert.ToDateTime(txtStartDate.Text); 
            sTariffMaster.StartDate = DateTime.ParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture);

            //7.End Date
            //sTariffMaster.EndDate= Convert.ToDateTime(txtEndDate.Text);  // to fix bug 1169 [07FEB12RM]
            sTariffMaster.EndDate = DateTime.ParseExact(txtEndDate.Text, format, CultureInfo.InvariantCulture);

            return sTariffMaster;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (ValidateScreenControls())
            {
                try
                {
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    STariffMaster sTariffMaster=GetBusinessEntity();

                    STariffCreationAcknowledgement sTariffCreationAck = proxy.CreateMasterTariff(sTariffMaster);

                    switch (sTariffCreationAck.CreationStatus)
	                {
		                case 0:
                                KaizosSession.Current.ReturnURL     = "frmZoneCreationUpdate.aspx";
                                KaizosSession.Current.ErrorMessage  = string.Format(GetGlobalResourceObject("LocalString", "TariffCreationSuccess").ToString(), txtTariffReference.Text);
                                Server.Transfer("frmResult.aspx", false);
                                break;
                        case 1:

                                KaizosSession.Current.ReturnURL     = "frmTariffCreation.aspx";
                                KaizosSession.Current.ErrorMessage  = string.Format(GetGlobalResourceObject("LocalString", "TariffCreationSuccess").ToString(), txtTariffReference.Text);
                                Server.Transfer("frmResult.aspx", false);
                                break;
                        case 2:
                                KaizosSession.Current.ReturnURL     = "frmTariffImportation.aspx";
                                KaizosSession.Current.ErrorMessage  = string.Format(GetGlobalResourceObject("LocalString", "TariffCreationOverlap").ToString(), txtTariffReference.Text, sTariffCreationAck.TariffReference,txtStartDate.Text, txtEndDate.Text);
                                Server.Transfer("frmResult.aspx", false);
                                break;

                        case 3:
                                KaizosSession.Current.ReturnURL     = "frmTariffCreation.aspx";
                                KaizosSession.Current.ErrorMessage  = string.Format(GetGlobalResourceObject("LocalString", "TariffCreationExistsAlready").ToString(), txtTariffReference.Text);
                                Server.Transfer("frmResult.aspx", false);
                                break;
                    }

                }
                catch (Exception error)
                {
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    { 
                        string userName = User.Identity.Name;
                        string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                        logger.Debug(errorMessage);

                        KaizosSession.Current.ReturnURL = "frmTariffCreation.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffCreationFailure").ToString() + txtTariffReference.Text.Trim();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
        }

        //To fix bug 1073[08FEB12RM]
        protected bool IsValidTariffRef(string strTariff)
        {

            string PasswordPattern = @"^[a-zA-Z0-9_]+$";  //08FEB12RM added _ 

            Regex strPassword = new Regex(PasswordPattern);

            return (strPassword.IsMatch(strTariff.Trim()));
        }


        /// <summary>
        /// To validate the screen mandatory fields
        /// </summary>
        /// <returns></returns>
        protected bool ValidateScreenControls()
        {

            bool result=true;

            bool IsContainerFailured = false;
            bool IsTariffFailured    = false;
            bool IsDateFailured      = false;
            bool IsTariffRefFailured = false;

            lblContainerTypeErrMsg.Visible  = false;
            lblKeyUserErrMsg.Visible        = false;
            lblStartDateErrMsg.Visible      = false;
            lblDateValidation.Visible       = false;
            lblTariffRefErrMsg.Visible = false; //08FEB12RM

            
            //08FEB12RM added tariff reference validation
            if (txtTariffReference.Text.Trim().Length == 0 || !IsValidTariffRef(txtTariffReference.Text.Trim()))
            {
                lblTariffRefErrMsg.Visible = true;
                lblTariffRefErrMsg.Text = "";
                lblTariffRefErrMsg.Text = lblTariffReference.Text + " " + GetGlobalResourceObject("LocalString", "ValidationInvalid").ToString();
                IsTariffRefFailured = true;
            }

            if (cblContainerType.SelectedIndex == -1)
            {
                lblContainerTypeErrMsg.Visible  = true;
                lblContainerTypeErrMsg.Text     = GetGlobalResourceObject("LocalString", "TariffContainerErr").ToString();  //"Atleast one container must be selected !";
                IsContainerFailured             = true;
            }

            if (rblTariffType.SelectedIndex==-1)
            {
                lblKeyUserErrMsg.Visible = true;
                lblKeyUserErrMsg.Text = lblTariffType.Text + " " + GetGlobalResourceObject("LocalString", "ValidationAccept").ToString();  //"Must for Negotiated tariff type";
                IsTariffFailured      = true;
            }

            if (!IsTariffFailured)
            {
            if (((rblTariffType.SelectedValue.Trim() == "KeyCustomer")) && (txtKeyUserReference.Text.Trim() == ""))
            {
                lblKeyUserErrMsg.Visible = true;
                lblKeyUserErrMsg.Text = lblKeyUserReference.Text + " " +GetGlobalResourceObject("LocalString", "ValidationEmpty").ToString();  //"Must for Negotiated tariff type";
                IsTariffFailured      = true;
            }
            }

            if (txtStartDate.Text.Trim() == "" || txtEndDate.Text.Trim() == "")
            {
                lblStartDateErrMsg.Visible = true;
                lblStartDateErrMsg.Text = lblStartDate.Text + " and " + lblEndDate.Text + " " + GetGlobalResourceObject("LocalString", "ValidationEmpty").ToString();  //"Date must be filled ";
                IsDateFailured = true;
            }


            if (!IsDateFailured)  //31JAN12RM  to fix bug 1120  
            {
                DateTime value;
                string format = "dd/MM/yyyy";

                // to fix bug 1169 [07FEB12RM]
                //if ((!DateTime.TryParse(txtStartDate.Text, out value)) || (!DateTime.TryParse(txtEndDate.Text, out value) ))
                //DateTime.TryParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture,DateTimeStyles.None, out value)

                if (
                        (!DateTime.TryParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture,DateTimeStyles.None, out value)) 
                            ||
                        (!DateTime.TryParseExact(txtEndDate.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)) 
                   )
                {
                    lblDateValidation.Visible = true;
                    lblDateValidation.Text =  lblStartDate.Text  + "/" + lblEndDate.Text + " " + GetGlobalResourceObject("LocalString", "ValidationInvalid").ToString();
                    IsDateFailured = true;
                }
            }


            if (!IsDateFailured)
            {
                DateTime startDate;
                DateTime endDate;
                
                string format = "dd/MM/yyyy";

                startDate   = DateTime.ParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture);
                endDate     = DateTime.ParseExact(txtEndDate.Text, format, CultureInfo.InvariantCulture);

                //if (Convert.ToDateTime(txtStartDate.Text) >= Convert.ToDateTime(txtEndDate.Text)) // to fix bug 1169 [07FEB12RM]
                
                if (startDate >= endDate)
                {
                    lblStartDateErrMsg.Visible = true;
                    lblStartDateErrMsg.Text = lblStartDate.Text + " " + GetGlobalResourceObject("LocalString", "ValLessThan").ToString() + lblEndDate.Text;  //"Start date must be less then end date";
                    IsDateFailured = true;
                }
            }


			if (IsContainerFailured || IsTariffFailured || IsDateFailured || IsTariffRefFailured)
			{
				result = false;
				errorMsg.Attributes["style"] = "display: block;";
			}
			
            return result;

        }

        protected void FillCombo()
        {
            /* 1.create proxy */
            KaizosServiceAgent proxy = new KaizosServiceAgent();

            List<SComboText> sComboText = new List<SComboText>();

            SComboTableField sComboTableField = new SComboTableField();

            //To fill Origin drop down list
            sComboTableField.FieldName = "CARRIER_NAME";
            sComboTableField.TableName = "CARRIER_MAST";
            sComboText = proxy.FillCombo(sComboTableField).ToList();

            ddlCarrier.DataSource = sComboText;
            ddlCarrier.DataTextField = "ComboText";
            ddlCarrier.DataBind();

        }

        protected void rblTariffType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //31JAN12RM
            trKeyUserRef1.Visible = false;

            if (rblTariffType.SelectedIndex == 2)
            {
                trKeyUserRef1.Visible = true;
            }
        }
    }
}