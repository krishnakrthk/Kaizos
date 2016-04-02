using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmMasterServiceTypeCreation : BasePage
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(frmMasterServiceTypeCreation));

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				Page.Title = GetGlobalResourceObject("LocalString", "frmMasterServiceTypeCreation").ToString();
				errorMsg.Attributes["style"] = "display: none;";
			}

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {

                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    SMasterServiceType sMasterServiceType = new SMasterServiceType();

                    sMasterServiceType.ServiceTypeName = txtServiceTypeName.Text.Trim();
                    sMasterServiceType.Priority = rblPriority.SelectedValue.Trim();
                    sMasterServiceType.Type = rblType.SelectedValue.Trim();
                    sMasterServiceType.isBulkShippingAvailable = rblBulkShipping.SelectedValue == "Y" ? SEnumFlag.Yes : SEnumFlag.No;

                    int result = proxy.CreateMasterService(sMasterServiceType);

                    if (result == 0)
                    {
                            KaizosSession.Current.ErrorMessage = String.Format(GetGlobalResourceObject("LocalString", "MasterTypeCreationSucess").ToString(), txtServiceTypeName.Text);
                    }
                    else if (result == 1)
                    {
						KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterTypeFailure").ToString();
                    }
                    else if (result ==2)
                    {
                        KaizosSession.Current.ErrorMessage = sMasterServiceType.ServiceTypeName.Trim() + " " + GetGlobalResourceObject("LocalString", "MasterTypeAlreadyExists").ToString();
                    }

                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeCreation.aspx";
                    Server.Transfer("frmResult.aspx", false);
                    

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
        }

        protected void val_MasterServiceYype_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            string strError = "";

            
            if (txtServiceTypeName.Text.Trim().Equals(""))
            {
                strError = strError + "*" + lblServiceTypeName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }
            
            if (rblPriority.SelectedIndex == -1)
            {
                strError = strError + "*" + lblPriority.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (rblType.SelectedIndex == -1)
            {
                strError = strError + "*" + lblType.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }


            if (rblBulkShipping.SelectedIndex == -1)
            {
                strError = strError + "*" + lblBulkShipping.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_MasterServiceYype.ErrorMessage = strError;
            }
			errorMsg.Attributes["style"] = "display: block;";
        }

        
    }
}