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


public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string UserType;
    protected void Page_Load(object sender, EventArgs e)
    {
            //Fill Session variable
            UserType = KaizosSession.Current.UserType.Trim();

            string uMenuItem = "";
            KaizosServiceAgent proxy = new KaizosServiceAgent();
            List<SFunctionality> sFunctionality = new List<SFunctionality>();
            List<SFunctionalProfile> sFunctionalityProfile = new List<SFunctionalProfile>();
            Menu uMENU = (Menu)FindControl("MainMenu");

            sFunctionality = proxy.GetFunctionality().ToList();
            sFunctionalityProfile = proxy.GetFunctionalProfile().ToList();
            for (int i = 0; i < sFunctionalityProfile.Count; i++)
            {
                if (UserType.Trim() == sFunctionalityProfile[i].ProfileCode.Trim())
                {
                    for (int j = 0; j < sFunctionality.Count; j++)
                    {
                        if (sFunctionalityProfile[i].FunctionalCode == sFunctionality[j].FunctionalCode)
                        {
                            uMenuItem = HttpUtility.HtmlEncode(sFunctionality[j].FunctionalName.Trim());
                            if (uMENU.FindItem(uMenuItem).NavigateUrl != "")
                                uMENU.FindItem(uMenuItem).Enabled = true;
                        }
                    }
                }
            }
            if (KaizosSession.Current.UserType == "AD")
                uMENU.FindItem("My Account/Personnal information").Enabled = false;
            else if (KaizosSession.Current.UserType == "FR")
                uMENU.FindItem("My Account/Personnal information").NavigateUrl = "frmFranchiseUpdate.aspx";
            else if (KaizosSession.Current.UserType == "CS")
                uMENU.FindItem("My Account/Personnal information").NavigateUrl = "frmCustomerServiceUpdate.aspx";
            else if (KaizosSession.Current.UserType == "RF")
            {
                KaizosSession.Current.Email = KaizosSession.Current.UserId.Trim();
                uMENU.FindItem("My Account/Personnal information").NavigateUrl = "frmEndCustomerUpdate.aspx";
            }
            else if (KaizosSession.Current.UserType == "AZ")
                uMENU.FindItem("My Account/Personnal information").NavigateUrl = "frmAuthorizedUpdateSelf.aspx";
    }

}
