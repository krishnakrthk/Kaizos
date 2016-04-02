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

public class KaizosMenuList
{
    public string MenuName;
    public string MenuValue; 

}
public partial class NewSiteMaster : System.Web.UI.MasterPage
{
    public List<KaizosMenuList> LoadMenuList()
    {
        List<KaizosMenuList> mnu = new List<KaizosMenuList>();

        KaizosMenuList mnuList;
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuTariff";
        mnuList.MenuValue = "Tariff";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuZoning";
        mnuList.MenuValue = "Zoning";
        mnu.Add(mnuList);
		mnuList = new KaizosMenuList();
		mnuList.MenuName = "MenuZoningCreation";
		mnuList.MenuValue = "Administration/Tariff/Zoning/Creation";
		mnu.Add(mnuList);
		mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuZoningSearch";
		mnuList.MenuValue = "Administration/Tariff/Zoning/Search";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuImportation";
		mnuList.MenuValue = "Administration/Tariff/Importation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuServiceTypes";
        mnuList.MenuValue = "Service Types";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuServiceTypesCreation";
		mnuList.MenuValue = "Administration/Tariff/Service Types/Creation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuServiceTypesUpdate";
		mnuList.MenuValue = "Administration/Tariff/Service Types/Update";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuServiceTypesValidity";
		mnuList.MenuValue = "Administration/Tariff/Validity Update-Disable";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFuelSurcharge";
		mnuList.MenuValue = "Administration/Tariff/Fuel Surcharge";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuOptionSurcharges";
		mnuList.MenuValue = "Administration/Tariff/Options Surcharges";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuPublicLevels";
		mnuList.MenuValue = "Administration/Tariff/Public levels";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuPublicLevelsPublicTariffManagement";
        mnuList.MenuValue = "Administration/Tariff/Public levels/Public Tariff Management";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuPublicLevelsGrossMarginSimulation";
		mnuList.MenuValue = "Administration/Tariff/Public levels/Gross Margin Simulation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccounts";
        mnuList.MenuValue = "Accounts";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccountsCustomers";
        mnuList.MenuValue = "Customers";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersListC";
        mnuList.MenuValue = "Administration/Accounts/Customers/List";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccountsCustomersUpdate";
        mnuList.MenuValue = "Update";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccountsCustomersGeneral";
        mnuList.MenuValue = "General";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccountsCustomersSalesGrossMarginTable";
		mnuList.MenuValue = "Administration/Accounts/Customers/Update/Sales gross margin table";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAccountsCustomersCreation";
		mnuList.MenuValue = "Administration/Accounts/Customers/Creation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuProfile";
		mnuList.MenuValue = "Administration/Accounts/Profile";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();     

        mnuList.MenuName = "MenuFranchisePartners";
		mnuList.MenuValue = "Administration/Franchise partners";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersList";
        mnuList.MenuValue = "Administration/Accounts/Franchise partners/List";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersUpdate";
        mnuList.MenuValue = "Administration/Accounts/Franchise partners/Update";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersCreation";
        mnuList.MenuValue = "Administration/Accounts/Franchise partners/Creation";
        mnu.Add(mnuList);

		mnuList = new KaizosMenuList();
		mnuList.MenuName = "MenuCustomerService";
		mnuList.MenuValue = "Administration/Accounts/Customer Service";
		mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCustomerServiceUpdate";
        mnuList.MenuValue = "Administration/Accounts/Customer Service/Update";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCustomerServiceCreation";
		mnuList.MenuValue = "Administration/Accounts/Customer Service/Creation";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersListCS";
        mnuList.MenuValue = "Administration/Accounts/Franchise partners/List";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAlert";
        mnuList.MenuValue = "Alerts";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAlertCustomerCreditRequests";
		mnuList.MenuValue = "Administration/Alerts/Customer Credit Requests";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuAlertDisplayAlerts";
		mnuList.MenuValue = "Administration/Alerts/Display alerts";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuTos";
		mnuList.MenuValue = "Administration/Terms of Service";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuShipping";
        mnuList.MenuValue = "Shipping";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMainShippingCancel";
		mnuList.MenuValue = "Administration/Shipping/Cancellation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuInvoices";
        mnuList.MenuValue = "Invoices";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCarrierInvoices";
		mnuList.MenuValue = "Administration/Invoices/Carrier invoices";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCarrierInvoicesUnknown";
		mnuList.MenuValue = "Administration/Invoices/Carrier invoices/Unknown";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCarrierInvoicesUnmatchedShipping";
		mnuList.MenuValue = "Administration/Invoices/Carrier invoices/Unmatched Shipping";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuInvoicesCustomerinvoices";
        mnuList.MenuValue = "Customer invoices";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuInvoicesCustomerinvoicesManualCreation";
		mnuList.MenuValue = "Administration/Invoices/Customer invoices/Manual Creation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCarriers";
        mnuList.MenuValue = "Carriers";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuCarriersCreation";
		mnuList.MenuValue = "Administration/Carriers/Creation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuFranchisePartnersListCarrier";
		mnuList.MenuValue = "Administration/Carriers/List";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuClaim";
        mnuList.MenuValue = "Claim";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuSetup";
        mnuList.MenuValue = "Setup";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuSetupInsuranceBroker";
		mnuList.MenuValue = "Administration/Setup/Insurance broker";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuSetupAlertsSettings";
		mnuList.MenuValue = "Administration/Setup/Alerts settings";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuSetupAlertsInvoicing";
		mnuList.MenuValue = "Administration/Setup/Invoicing";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuSetupAlertsFAQ";
		mnuList.MenuValue = "Administration/Setup/FAQ";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMainShipping";
        mnuList.MenuValue = "Shipping";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMainShippingGetQuote";
        mnuList.MenuValue = "Shipping/Get a Quote and ship";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMainShippingBulkShipping";
        mnuList.MenuValue = "Shipping/Bulk Shipping";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMainShippingPendingShipping";
        mnuList.MenuValue = "Shipping/Pending shipments";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuTracking";
        mnuList.MenuValue = "Tracking";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccount";
        mnuList.MenuValue = "My Account";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuPersionalInformation";
		mnuList.MenuValue = "My Account/Personnal information";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAuthorizedUser";
        mnuList.MenuValue = "Authorized User";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAuthorizedUserCreation";
        mnuList.MenuValue = "My Account/Authorized User/Creation";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAuthorizedUserList";
        mnuList.MenuValue = "My Account/Authorized User/List";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAuthorizedUserUpdate";
        mnuList.MenuValue = "My Account/Authorized User/Update";
        mnu.Add(mnuList);

        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountSendAClaim";
		mnuList.MenuValue = "My Account/Send a Claim";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountInvoices";
		mnuList.MenuValue = "My Account/Invoices";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAddressBook";
        mnuList.MenuValue = "Address Book";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAddressBookAddAnAddress";
        mnuList.MenuValue = "My Account/Address Book/Add an address";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAddressBookImportAddress";
        mnuList.MenuValue = "My Account/Address Book/Import addresses";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountAddressBookUpdate";
        mnuList.MenuValue = "My Account/Address Book/Update";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuMyAccountShipmentResult";
        mnuList.MenuValue = "My Account/Shipping Documents";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuDashboard";
        mnuList.MenuValue = "Dashboard";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuDashboardInvoicingVisalization";
		mnuList.MenuValue = "Dashboard/Invoicing Visalization";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuDashboardActivityProfitability";
		mnuList.MenuValue = "Dashboard/Activity Profitability";
        mnu.Add(mnuList);
        mnuList = new KaizosMenuList();
        mnuList.MenuName = "MenuDashboardPerformance";
		mnuList.MenuValue = "Dashboard/Performance";
        mnu.Add(mnuList);
      
        return mnu;

    }
    
    public string UserType;
    protected void Page_Load(object sender, EventArgs e)
    {

        System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
        //Fill Session variable
        if (KaizosSession.Current.UserType != null)
        {
            UserType = KaizosSession.Current.UserType.Trim();
        }
        else
        {
            for (int pc = 0; pc < this.Controls.Count; pc++)
            {
                
                if (Controls[pc].GetType().ToString().Contains("HtmlForm"))
                {

                    form = (System.Web.UI.HtmlControls.HtmlForm)Controls[pc];

                        for (int pc1 = 0; pc1 < form.Controls.Count; pc1++)
                        {
                            if (form.Controls[pc1].GetType().ToString().Contains("HyperLink"))
                            {
                                HyperLink hlnk = (HyperLink)form.Controls[pc1];
                                hlnk.Enabled = false;
                                hlnk.Visible = false;
                            }
                            
                        }
              }
              //break;    
            }
            return;
        }
       
        //GetGlobalResourceObject("LocalString", "frmTariffValidity").ToString();
        string uMenuItem = "";

        
        KaizosServiceAgent proxy = new KaizosServiceAgent();
        List<SFunctionality> sFunctionality = new List<SFunctionality>();
        List<SFunctionalProfile> sFunctionalityProfile = new List<SFunctionalProfile>();
        Menu uMENU = (Menu)FindControl("MainMenu");

        for (int pc = 0; pc < this.Controls.Count; pc++)
        {

            if (Controls[pc].GetType().ToString().Contains("HtmlForm"))
            {

                form = (System.Web.UI.HtmlControls.HtmlForm)Controls[pc];

                if (KaizosSession.Current.UserType != "AD")
                {
                    for (int pc1 = 0; pc1 < form.Controls.Count; pc1++)
                    {
                        if (form.Controls[pc1].GetType().ToString().Contains("HyperLink"))
                        {
                            HyperLink hlnk = (HyperLink)form.Controls[pc1];
                            hlnk.Enabled = false;
                            hlnk.Visible = false;
                        }
                    }
                }
                break;
            }
        }

		// Get the menu entries
		List<KaizosMenuList> lstMnu = LoadMenuList();
        sFunctionality = proxy.GetFunctionality().ToList();
        sFunctionalityProfile = proxy.GetFunctionalProfile().ToList();

		// Get the main menu
        HyperLink hyper = (HyperLink)form.FindControl("MenuPersionalInformation");
        HyperLink admin = (HyperLink)form.FindControl("admin");
        HyperLink shipping = (HyperLink)form.FindControl("MenuMainShipping");
        HyperLink tracking = (HyperLink)form.FindControl("MenuTracking");
        HyperLink addressbook = (HyperLink)form.FindControl("MenuMyAccountAddressBook");
        HyperLink myaccount = (HyperLink)form.FindControl("MenuMyAccount");
        HyperLink accounts = (HyperLink)form.FindControl("MenuAccounts");
        HyperLink authorized = (HyperLink)form.FindControl("MenuMyAccountAuthorizedUser");
        HyperLink customer = (HyperLink)form.FindControl("MenuAccountsCustomers");
        HyperLink MenuZoning = (HyperLink)form.FindControl("MenuZoning");
        HyperLink MenuServiceTypes = (HyperLink)form.FindControl("MenuServiceTypes");
        HyperLink MenuPublicLevels = (HyperLink)form.FindControl("MenuPublicLevels");
        HyperLink MenuAccounts = (HyperLink)form.FindControl("MenuAccounts");
        HyperLink MenuFranchisePartners = (HyperLink)form.FindControl("MenuFranchisePartners");
        HyperLink MenuCustomerService = (HyperLink)form.FindControl("MenuCustomerService");
        HyperLink MenuAlert = (HyperLink)form.FindControl("MenuAlert");
        HyperLink MenuInvoices = (HyperLink)form.FindControl("MenuInvoices");
        HyperLink MenuCarrierInvoices = (HyperLink)form.FindControl("MenuCarrierInvoices");
        HyperLink MenuInvoicesCustomerinvoices = (HyperLink)form.FindControl("MenuInvoicesCustomerinvoices");
        HyperLink MenuCarriers = (HyperLink)form.FindControl("MenuCarriers");
        HyperLink MenuClaim = (HyperLink)form.FindControl("MenuClaim");
        HyperLink MenuSetup = (HyperLink)form.FindControl("MenuSetup");
        HyperLink MenuMyAccountAuthorizedUser = (HyperLink)form.FindControl("MenuMyAccountAuthorizedUser");
        HyperLink MenuMainShipping = (HyperLink)form.FindControl("MenuMainShipping");
        HyperLink MenuMyAccount = (HyperLink)form.FindControl("MenuMyAccount");
        HyperLink MenuPersionalInformation = (HyperLink)form.FindControl("MenuPersionalInformation");       
        HyperLink MenuDashboard = (HyperLink)form.FindControl("MenuDashboard");

		// Display or hide the main menus according to the profiles
        hyper.Enabled = true;
        admin.Visible = true;
        admin.Enabled = true;
        shipping.Visible = true;
        shipping.Enabled = true;
        tracking.Visible = true;
        tracking.Enabled = true;
        addressbook.Visible = true;
        addressbook.Enabled = true;
        myaccount.Visible = true;
        myaccount.Enabled = true;
           HyperLink dashboard = (HyperLink)form.FindControl("tracking");
		
		if (KaizosSession.Current.UserType == "AD")
        {
            hyper.Enabled = false;
            hyper.Visible = false;
            authorized.Enabled = false;
            authorized.Visible = false;
        }
        else if (KaizosSession.Current.UserType == "FR")
        {
			MenuCustomerService.Enabled = false;
        	MenuCustomerService.Visible = false;

			MenuFranchisePartners.Enabled = false;
			MenuFranchisePartners.Visible = false;

			MenuAlert.Enabled = true;
			MenuAlert.Visible = true;

            accounts.Enabled = true;
            accounts.Visible = true;

            customer.Enabled = true;
            customer.Visible = true;

			MenuMainShipping.Enabled = true;
			MenuMainShipping.Visible = true;

			MenuInvoices.Enabled = true;
			MenuInvoices.Visible = true;

			MenuClaim.Enabled = true;
			MenuClaim.Visible = true;

			MenuInvoicesCustomerinvoices.Enabled = true;
			MenuInvoicesCustomerinvoices.Visible = true;
			
			MenuDashboard.Enabled = true;
			MenuDashboard.Visible = true;
            
			hyper.Visible = true;
            myaccount.Enabled = true;
            hyper.NavigateUrl = "frmFranchiseUpdate.aspx";
        }
        else if (KaizosSession.Current.UserType == "CS")
        {
            MenuMyAccount.Visible = true;
            addressbook.Enabled = false;
            addressbook.Visible  = false;
            admin.Visible = false;
            admin.Enabled = false;
            shipping.Visible = false;
            shipping.Enabled = false;
            hyper.NavigateUrl = "frmCustomerServiceUpdate.aspx";
        }
        else if (KaizosSession.Current.UserType == "RF")
        {
            hyper.Visible = true;
            myaccount.Enabled = true;
            authorized.Enabled  = true;
            authorized.Visible = true;
            admin.Visible = false;
            admin.Enabled = false;
            KaizosSession.Current.Email = KaizosSession.Current.UserId.Trim();
            hyper.NavigateUrl = "frmEndCustomerUpdate.aspx";
        }
        else if (KaizosSession.Current.UserType == "AZ")
        {
            admin.Visible = false;
            admin.Enabled = false;
            authorized.Visible = false;
            authorized.Enabled =  false;
            hyper.NavigateUrl = "frmAuthorizedUpdateSelf.aspx";
        }

        // Hide the end menus according to the profiles setup in DB
		for (int i = 0; i < sFunctionalityProfile.Count; i++)
        {
            if (UserType.Trim() == sFunctionalityProfile[i].ProfileCode.Trim())
            {
                for (int j = 0; j < sFunctionality.Count; j++)
                {
                    if (sFunctionalityProfile[i].FunctionalCode == sFunctionality[j].FunctionalCode)
                    {

                        KaizosMenuList result = lstMnu.Find(delegate(KaizosMenuList sk)
                                {
                                    return sk.MenuValue == sFunctionality[j].FunctionalName;
                                });

                                if (result != null)
                                {
                                    HyperLink hlnk = (HyperLink)form.FindControl(result.MenuName);
                                    hlnk.Enabled = true;
                                    hlnk.Visible = true;
                                    break;
                               }
                            }
                        }
                    }
                }
            }
    }
                
        
   

